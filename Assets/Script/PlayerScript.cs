using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class NewBehaviourScript : MonoBehaviour
{
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

    public List<NPCData> npcList;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCameraTransform = Camera.main.transform;

        runText = GameObject.Find("IsRun");
        dataManager = GameObject.Find("DataManager");

        npcList = dataManager.GetComponent<NPCDataManager>().GetNPCList();
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // 카메라의 forward와 right 벡터를 이용하여 이동 방향을 결정합니다.
        Vector3 moveDirection = mainCameraTransform.forward * verticalInput;
        Vector3 sideDirection = mainCameraTransform.right * horizontalInput;

        // 캐릭터를 회전시킵니다.
        if (verticalInput != 0 || horizontalInput != 0)
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
        if (verticalInput != 0)
        {
            if (isRun)
            {
                // 캐릭터를 이동시킵니다.
                rb.MovePosition(rb.position + moveDirection * runSpeed * Time.deltaTime);
            }
            else
            {
                rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
            }
        }

        if (horizontalInput != 0)
        {
            if (isRun)
            {
                //캐릭터를 이동시킵니다
                rb.MovePosition(rb.position + sideDirection * runSpeed * Time.deltaTime);
            }
            else
            {
                rb.MovePosition(rb.position + sideDirection * moveSpeed * Time.deltaTime);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            isRun = isRun ? false : true;
        }
        
        runText.GetComponent<TextMeshProUGUI>().text = (isRun == true ? "run" : "walk");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 트리거 발동 대상이 npc이면
        // 대화 진행 가능하게 만들 것이다
        // npc에 적용되어 있는 충돌 관련 collider대신
        // 더 큰 반구로 적용되어있는 collider는 trigger발동용 콜라이더입니다.
        foreach (var element in npcList)
        {
            if (other.name.Equals(element.NpcName))
            {
                Debug.Log($"{element.NpcName}와 대화 진행 가능");
            }
        }
        
    }
}
