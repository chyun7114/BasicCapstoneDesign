using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatPrint : MonoBehaviour
{
    public  GameObject dataManager;
    
    public List<DialogueData> dialogueList;
    
    public TMP_Text chatText;
    public TMP_Text speakerName;
    
    public int chatSpeed;
    public float Speed_0=0.1f;
    public float Speed_1=0.05f;
    public float Speed_2=0.01f;
    public float Speed_3=0f;
    
    private float delay=0.1f;
    
    private bool isTyping = false;
    private bool isChatting = false;
    
    private string strtoType;
    private string chattingNPC;
    
    IEnumerator typeCoroutine;
    
    private GameObject chatpanel;
    private GameObject player;
    
    private int[] currentchat = new int[50];
    //임시 변수, 현재 대화진도 저장 후 불러오거나 퀘스트 진도에 따라 대화가 달라지게 구현 후 지울 것


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        dataManager = GameObject.Find("DataManager");
        changeSpeed();
        chatpanel = GameObject.Find("ChatCanvas");
        chatpanel.SetActive(false);
        dialogueList = dataManager.GetComponent<DialogueDataManager>().GetList;
    }
    void changeSpeed()
    {
        switch(chatSpeed)
        {
            case 0:
                delay = Speed_0;
                break;
            case 1:
                delay = Speed_1;
                break;
            case 2:
                delay = Speed_2;
                break;
            case 3:
                delay = Speed_3;
                break;
            default:
                delay = Speed_0;
                break;
        }
    }
    public void ChatPrinting(string str)
    {
        strtoType=str;
        if(delay!=0f)
        {
            chatText.text="";
            typeCoroutine=typing();
            StartCoroutine(typeCoroutine);
            isTyping=true;
        }
        else chatText.text=str;
    }
    IEnumerator typing()
    {
        int count = 0;
        while(count != strtoType.Length)
        {
            if(count<strtoType.Length)
            {
                chatText.text += strtoType[count];
                count++;
            }
            if(count==strtoType.Length)isTyping=false;
            yield return new WaitForSeconds(delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(isTyping)
            {
                StopCoroutine(typeCoroutine);
                chatText.text=strtoType;
                isTyping=false;
                typeCoroutine=null;
            }
            else if(isChatting)
            {
                strtoType = nextChat();
                if(strtoType!=null)
                {
                    ChatPrinting(strtoType);
                }
                else
                {
                    ChatClose();
                }
            }
        }
    }
    public void ChatOpen(string npcName)
    {
        speakerName.text=npcName;
        chatpanel.SetActive(true);
        isChatting=true;
        chattingNPC=npcName;
        strtoType = nextChat();
        if(strtoType!=null)
        {
            ChatPrinting(strtoType);
        }
        else
        {
            ChatClose();
        }
    }
    string nextChat()
    {
        foreach(var element in dialogueList)
        {
            // Debug.Log(element.NpcName);
            if(chattingNPC.Equals(element.NpcName))
            {
                // Debug.Log("Found");
                if(currentchat[element.NPCId]==9999)
                {
                    // 밑에 코드 없으면 한 npc에게 한번만 대화 할 수 있어서 수정함
                    currentchat[element.NPCId] = 0;
                    return null;
                }
                else if(currentchat[element.NPCId]==0 ||currentchat[element.NPCId]==element.DialogueId)
                {
                    currentchat[element.NPCId]=element.NextDialogueId;
                    return element.DialogueString;
                }
            }
        }
        return null;
    }
    void ChatClose()
    {
        Camera.main.GetComponent<CameraScript>().EndChat();
        player.GetComponent<NewBehaviourScript>().EndChat();
        isChatting = false;
        chatpanel.SetActive(false);
    }
}
