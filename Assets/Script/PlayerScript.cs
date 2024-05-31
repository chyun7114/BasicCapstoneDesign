using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviour
{
    public Animator animator;
    private Rigidbody rb;
    public GameObject player;
    private Transform mainCameraTransform;
    
    public float moveSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float forwardRotationSpeed = 10f;
    public float backwardRotationSpeed = 5f;
    
    public bool isRun = false;

    private GameObject runText;
    public  GameObject dataManager;
    public GameObject PressE;
    public GameObject chatting;
    public GameObject chatManager;
    public GameObject menuPanel;
    public GameObject charInfoPanel;
    
    public List<NPCData> npcList;
    
    private bool chatEnabled = false;
    private bool whileChatting = false;
    private bool isMenuPanelOpen = false;
    private string nearNPC;
    private string warpPointName = null;
    
    //캐릭터 정보창
    public bool ischarInfoOpen = false;
    public bool check = true;
    public bool menuPanelcheck = true;
    private bool isInWarpPoint = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCameraTransform = Camera.main.transform;
        
        dataManager = GameObject.Find("DataManager");
        PressE = GameObject.Find("PressE");
        npcList = dataManager.GetComponent<NPCDataManager>().NPCList;
        
        if(SceneManager.GetActiveScene().name == "Street")
            gameObject.transform.position = dataManager.GetComponent<PlayerData>().playerPositionInStreet;
        else if (SceneManager.GetActiveScene().name == "Hospital")
            gameObject.transform.position = dataManager.GetComponent<PlayerData>().playerPositionInHospital;
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // 카메라의 forward와 right 벡터를 이용하여 이동 방향을 결정합니다.
        Vector3 moveDirection = mainCameraTransform.forward * verticalInput;
        Vector3 sideDirection = mainCameraTransform.right * horizontalInput;

        // 캐릭터를 회전시킵니다.
        if ((verticalInput != 0 || horizontalInput != 0) && !whileChatting)
        {
            // 캐릭터의 이동 방향 벡터를 계산합니다.
            Vector3 rotateDirection = mainCameraTransform.forward * verticalInput +
                                      mainCameraTransform.right * horizontalInput;
            rotateDirection.y = 0f; // 캐릭터는 수직으로 회전하지 않도록 y값을 0으로 설정합니다.
            rotateDirection.Normalize(); // 벡터의 길이를 1로 만들어 정규화합니다.

            // 회전 속도를 설정합니다.
            float rotationSpeed = verticalInput > 0 ? forwardRotationSpeed : backwardRotationSpeed;

            // 캐릭터를 이동 방향으로 회전시킵니다.
            Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 입력이 있는 경우에만 캐릭터를 이동시킵니다.
        if ((verticalInput != 0 || horizontalInput != 0) && !whileChatting)
        {
            animator.SetBool("IsWalk", true); 
            Vector3 dir = moveDirection + sideDirection;
            if (isRun)
            {
                animator.SetBool("IsRun", true);
                rb.MovePosition(rb.position + dir * runSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("IsRun", false);
                rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("IsWalk", false);
            animator.SetBool("IsRun", false);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            isRun = isRun ? false : true;
        }
        
        // runText.GetComponent<TextMeshProUGUI>().text = (isRun == true ? "run" : "walk");
        
        // 대화 시작 키 => e키 누르면 대화 시작
        if(Input.GetKeyDown(KeyCode.E) && !whileChatting)
        {
            if(chatEnabled)
            {
                animator.SetBool("IsTalk" ,true);
                chatting.SetActive(true);
                whileChatting=true;
                Camera.main.GetComponent<CameraScript>().StartChat();
                chatting.GetComponent<ChatPrint>().ChatOpen(nearNPC);
            }
        }

        if (Input.GetKey(KeyCode.E) && isInWarpPoint)
        {
            if (warpPointName.Equals("HospitalWarppoint"))
            {
                TitleUIScript.LoadScene("Hospital");
            }
            else if (warpPointName.Equals("StreetWarppoint"))
            {
                TitleUIScript.LoadScene("Street");
            }
            
        }

        // 아이템 도감 진입 키 => I키 누르면 진입 및, esc키와 x버튼 클릭으로 종료
        if (Input.GetKey(KeyCode.I))
        {
            SceneManager.LoadScene("ItemListScene");
        }
        
        // 종료 메뉴 키 => esc버튼으로 동작
        OnclickESC();
        OnClickTab();
    }

    public void OnclickESC()
    {
        if (Input.GetKey(KeyCode.Escape) && menuPanelcheck)
        {
            check = false;

            if (isMenuPanelOpen == false)
            {
                isMenuPanelOpen = true;
                menuPanel.SetActive(isMenuPanelOpen);

            }
            else
            {
                isMenuPanelOpen = false;
                charInfoPanel.SetActive(isMenuPanelOpen);

            }
            StartCoroutine(WaitForIt());
        }
    }
    
    public void OnClickTab()
    {
        if (Input.GetKey(KeyCode.Tab) && check)
        {
            check = false;

            if (ischarInfoOpen == false)
            {
                ischarInfoOpen = true;
                charInfoPanel.SetActive(ischarInfoOpen);

            }
            else
            {
                ischarInfoOpen = false;
                charInfoPanel.SetActive(ischarInfoOpen);

            }
            StartCoroutine(WaitForIt());
        }

    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(0.3f);
        check = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnterTrigger")
        {
            SetEnterPlace(other.name);
        }
        
        if (other.CompareTag("Warppoint"))
        {
            Debug.Log("워프 가능");
            PressE.GetComponent<PressE>().Pop(other.gameObject.transform.position);
            isInWarpPoint = true;
            warpPointName = other.name;
        }

        if (other.CompareTag("NPC"))
        {
            Debug.Log("대화 가능");
            // 대화 진행 가능 여부 판단 후 대화 진행
            foreach (var element in npcList)
            {
                if (other.name.Equals(element.NpcName))
                {
                    chatEnabled = true;
                    nearNPC = other.name;
                    PressE.GetComponent<PressE>().Pop(other.gameObject.transform.position);
                    Debug.Log($"{element.NpcName}와 대화 진행 가능");
                }
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        // 대화가 끝나고 npc와 멀어지면 npc와의 상호작용 불가능
        foreach (var element in npcList)
        {
            if (other.name.Equals(element.NpcName))
            {
                chatEnabled=false;
                PressE.GetComponent<PressE>().Hide();
            }
        }
        
        if (other.CompareTag("Warppoint"))
        {
            PressE.GetComponent<PressE>().Hide();
            isInWarpPoint = false;
        }

        if (other.tag == "EnterTrigger")
        {
            dataManager.GetComponent<PlayerData>().nowPlace = null;
        }
    }
    public void EndChat()
    {
        whileChatting=false;
        animator.SetBool("IsTalk", false);
    }
    
    
    // 현재 플레이어가 어디 위치로 이동하였는지 알려줌
    public void SetEnterPlace(string placeName)
    {
        switch (placeName)
        {
            case "PlaygroundEnter":
                dataManager.GetComponent<PlayerData>().nowPlace = "playground";
                Debug.Log("놀이터 도착!");
                break;
            default:
                dataManager.GetComponent<PlayerData>().nowPlace = null;
                break;
        }
    }
}
