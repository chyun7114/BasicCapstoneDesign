using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;

public class ChatPrint : MonoBehaviour
{
    public GameObject questManager;
    public List<DialogueData> dialogueList;
    public List<NPCData> npcList;

    public GameObject storyImage;

    public DataManager dataManager;
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
    
    private DialogueData strtoType;
    private string chattingNPC;
    private int chattingNPCId;
    
    IEnumerator typeCoroutine;
    
    private GameObject chatpanel;
    [SerializeField] public GameObject player;
    public PlayerData playerData;
    

    private Dictionary<int, List<DialogueData>> dialogueDictionary;
    int currentDialogeId = 0;
    private int currentDialogeGroupId = -1;
    int dialogueGroupId = -1;
    public bool isFreeChat = false;

    public StoryManager storyManager;

    public Dictionary<int, bool> isChatGroupEnd = new Dictionary<int, bool>();
    
    // Start is called before the first frame update
    void Start()
    {
        storyManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        questManager = GameObject.Find("QuestManager");
        changeSpeed();
        chatpanel = GameObject.Find("ChatCanvas");
        playerData = dataManager.GetComponent<PlayerData>();
        chatpanel.SetActive(false);
        dialogueList = dataManager.dialogueDataManager.GetList;
        npcList = dataManager.npcDataManager.NPCList;
        dialogueDictionary = dataManager.dialogueDataManager.dialogueDictionary;
        foreach (var element in dialogueDictionary)
        {
            isChatGroupEnd.Add(element.Key, false);
        }

        foreach (var element in isChatGroupEnd)
        {
            Debug.Log($"{element.Key} , {element.Value}");
        }
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
        strtoType.DialogueString = str;
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
        while(count != strtoType.DialogueString.Length)
        {
            if(count<strtoType.DialogueString.Length)
            {
                chatText.text += strtoType.DialogueString[count];
                count++;
            }
            if(count==strtoType.DialogueString.Length)isTyping=false;
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
                chatText.text=strtoType.DialogueString;
                isTyping=false;
                typeCoroutine=null;
            }
            else if(isChatting)
            {

                strtoType = nextChat();
                if(strtoType!=null)
                {
                    //storyImage.SetActive(true);
                    if (strtoType.NpcName == "나레이션")
                        speakerName.text = "";
                    else
                        speakerName.text = strtoType.NpcName;
                    
                    ChatPrinting(strtoType.DialogueString);
                }
                else
                {
                    //storyImage.SetActive(false);
                    ChatClose();
                }
            }
        }
    }
    public void ChatOpen(string npcName)
    {
        speakerName.text=npcName;
        chattingNPC=npcName;
        chatpanel.SetActive(true);
        strtoType = nextChat();
        isChatting=true;
        
        if(strtoType!=null)
        {
            ChatPrinting(strtoType.DialogueString);
        }
        else
        {
            ChatClose();
        }
    }
    
    DialogueData nextChat()
    {
        if (!isChatting)
        {
            foreach (var element in dialogueList)
            {
                // 대화 시작하는 npc를 찾습니다
                if (element.StartNPC == 9999)
                {
                    continue;
                }

                if (isFreeChat)
                {
                    dialogueGroupId = storyManager.dialogueGroupId;
                    currentDialogeGroupId = storyManager.dialogueGroupId;
                    isChatGroupEnd[storyManager.dialogueGroupId] = true;
                    Debug.Log($"현재 다이얼 로그 그룹 아이디 : {dialogueGroupId}");
                    break;
                }
                
                // 한번 한 대화는 다시 안나오게 => 순차적 퀘스트 진행을 위함
                if (chattingNPC.Equals(npcList[element.StartNPC].NpcName) && !isChatGroupEnd[element.DialogueGroupId])
                {
                    dialogueGroupId = element.DialogueGroupId;
                    currentDialogeGroupId = element.DialogueGroupId;
                    isChatGroupEnd[element.DialogueGroupId] = true;
                    Debug.Log($"현재 다이얼 로그 그룹 아이디 : {dialogueGroupId}");
                    break;
                }
                
                // 나중에 퀘스트랑 관련 없는 대화 셋 리턴하게 만들겁니다.
            }

        }

        List<DialogueData> currentDialogueList = SetNextDialogueSet(currentDialogeGroupId);

        if (currentDialogueList.Count <= currentDialogeId)
        {
            currentDialogeId = 0;
            return null;
        }
        
        return currentDialogueList[currentDialogeId++];
        
        // foreach(var element in dialogueList)
        // {
        //     // Debug.Log(element.NpcName);
        //     if(chattingNPC.Equals(element.NpcName))
        //     {
        //         // Debug.Log("Found");
        //         if(currentchat[element.NPCId]==9999)
        //         {
        //             // 밑에 코드 없으면 한 npc에게 한번만 대화 할 수 있어서 수정함
        //             currentchat[element.NPCId] = 0;
        //             return null;
        //         }
        //         else if(currentchat[element.NPCId]==0 ||currentchat[element.NPCId]==element.DialogueId)
        //         {
        //             currentchat[element.NPCId]=element.NextDialogueId;
        //             return element.DialogueString;
        //         }
        //     }
        // }
        
    }

    public List<DialogueData> SetNextDialogueSet(int groupId)
    {
        return dialogueDictionary[groupId];
    }
    
    void ChatClose()
    {
        playerData.lastChatNPC = chattingNPC;
        try
        {
            Camera.main.GetComponent<CameraScript>().EndChat();
        }
        catch(Exception e)
        {
            GameObject cameraObject = GameObject.Find("Main Camera");
            cameraObject.GetComponent<CameraScript>().EndChat();
        }

        player.GetComponent<PlayerScript>().EndChat();
        isChatting = false;
        chatpanel.SetActive(false);
        
        // 채팅 종료시 talkCount를 증가시켜 스토리 진행
        // 조건에 맞는지는 항상 확인!!
        
        // dialogueGroupId가 점차 올라가는 방식으로 진행하여
        // 일정 수준에 도달할 때까지 talkCount 올린다
        if (playerData.lastTalkId < currentDialogeGroupId)
        {
            playerData.lastTalkId = currentDialogeGroupId;
            playerData.talkCount++;
            Debug.Log(playerData.talkCount);
        }
        else
        {
            playerData.currentTalkId = currentDialogeGroupId;
        }

        isFreeChat = false;
        // QuestManager manager = questManager.GetComponent<QuestManager>();
        // foreach (var element in npcList)
        // {
        //     if (element.matches(chattingNPC))
        //     {
        //         chattingNPCId = element.NpcId;
        //         break;
        //     }
        // }
        // manager.InitQuest(chattingNPCId);
    }
}
