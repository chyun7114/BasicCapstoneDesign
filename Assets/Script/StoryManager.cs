using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    private static StoryManager instance; // instance 변수를 static으로 변경

    public PlayerData playerData;
    public DataManager dataManager;
    public ChatPrint chatPrint;
    public GameObject player;
    public GameObject littlePrince;
    public GameObject questInfo;
    public GameObject fox;
    
    private int mainQuestNum = 0;
    private int subQuestNum = 0;
    public int dialogueGroupId = 0;

    void Awake()
    {
        SetGameObject();
        
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 스토리 루틴 시작
        StartCoroutine(StartGame());
    }

    private void SetGameObject()
    {
        player = GameObject.Find("rose");
        littlePrince = GameObject.Find("어린 왕자");
        questInfo = GameObject.Find("MainScreen").transform.Find("QuestInfo").gameObject;
        playerData = GameObject.Find("DataManager").GetComponent<PlayerData>();
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        chatPrint = GameObject.Find("Chat").GetComponent<ChatPrint>();
    }
    
    private IEnumerator StartGame()
    {
        yield return StartCoroutine(StoryRoutine());
        yield return StartCoroutine(SubQuestRoutine());
    }
    
    private IEnumerator StoryRoutine()
    {
        // 게임 시작
        while (true)
        {
            // 초반 대화를 순서대로 끝마치는 경우 메인퀘스트 1번 얻음
            yield return StartCoroutine(CheckLastChatNPC("어린 왕자"));
            playerData.lastChatNPC = null;
            // 나레이션 나온다
            ChatSelfStart("나레이션", 2);
            
            // 다음 어린왕자와 대화함
            questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- 어린왕자와 대화하기";
            yield return StartCoroutine(CheckInitialConversations(3));
            // 나레이션 으로 서브퀘스트 받고
            ChatSelfStart("나레이션", 4);
            littlePrince.transform.position = new Vector3(999f, 0, -999f);
            
            subQuestNum++;
            questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- 놀이터로 이동하기";
            
            // 놀이터 가는 시간 기다리기
            // 어린왕자 놀이터로 이동시킴
            // 현재 위치가 놀이터인지 판단하기
            yield return StartCoroutine(CheckNowPlace("playground"));
            questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- 놀이터에서 어린왕자 찾기";
            ChatSelfStart("나레이션", 5);
            
            // 얘가 진짜 어디갔지?
            yield return StartCoroutine(CheckNowPlace("미끄럼틀"));
            ChatSelfStart("장미", 6);
            
            Debug.Log("프롤로그 종료");
            mainQuestNum++;
            GetMainQuest(mainQuestNum);
            Debug.Log("메인퀘스트 #1 획득 완료!!");
            
            // 모래성 차는 동작
            yield return StartCoroutine(CheckNowPlace("큰 놀이터"));
            ChatSelfStart("장미", 7);
            
            // 여우 나오고 도망치게 합니다
            yield return StartCoroutine(CheckNowPlace("모래놀이터"));
            ChatSelfStart("장미", 8);
            fox = GameObject.Find("Fox");
            fox.transform.position = GameObject.Find("FoxEnter").transform.position;
            // 일일 퀘스트 여우에게 밥을 주자 추가 예정

            
            // #2-6: Day+5, 집 앞
            // 집앞으로 이동하게 합니다
            // 집 완성시 캐릭터 좌표 이동하거나 신전환하여 집앞이나 집으로 이동시킵니다
            questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- 장미의 집앞으로 이동하기";
            yield return StartCoroutine(CheckNowPlace("room"));
            ChatSelfStart("장미", 9);
            
            // #2-7: Day+6, 놀이터
            questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- 다시 놀이터로 가보기";
            yield return StartCoroutine(CheckNowPlace("playground"));
            ChatSelfStart("장미",10);
            
            
            // #3-1: secret episode
            // 다음날 집으로 돌아간다 
            questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- 다시 집으로 돌아가자";
            yield return StartCoroutine(CheckNowScene("Room"));
            SetGameObject();
            ChatSelfStart("아빠", 11);

            yield return StartCoroutine(CheckNowScene("Street"));
            SetGameObject();
            littlePrince.transform.position = new Vector3(999f, 0, -999f);
            fox = GameObject.Find("Fox");
            fox.transform.position = GameObject.Find("FoxEnter").transform.position;
            questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- 병원으로 가보자";
            
            yield return StartCoroutine(CheckNowScene("Hospital"));
            SetGameObject();
            ChatSelfStart("나레이션", 12);
            
            
            // 여우가 뱀한테 물려갑니다
            yield return StartCoroutine(CheckNowScene("Street"));
            SetGameObject();
            littlePrince.transform.position = new Vector3(999f, 0, -999f);
            SetQuestInfo("골목에서 여우를 찾아보자");
            
            fox = GameObject.Find("Fox");
            fox.transform.position = GameObject.Find("FoxEnter").transform.position;
            
            yield return StartCoroutine(CheckNowPlace("fox"));
            fox.transform.position = GameObject.Find("FoxEnter").transform.position;
            ChatSelfStart("나레이션", 13);
            fox.transform.position = new Vector3(999f, 0, 999f);
            
            SetQuestInfo("여우를 찾아서 거리로 나가보자");
            yield return StartCoroutine(CheckNowPlace("#14"));
            ChatSelfStart("나레이션", 14);
            
            SetQuestInfo("거리의 이준호에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("이준호"));
            
            SetQuestInfo("거리의 김지연에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("김지연"));
            
            SetQuestInfo("거리의 임태우에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("임태우"));
            
            SetQuestInfo("거리의 박성우에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("박성우"));
            
            SetQuestInfo("거리의 황지영에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("황지영"));
            GameObject jiyeon = GameObject.Find("황지영");
            jiyeon.transform.position = new Vector3(900, 0, 900);
            ChatSelfStart("나레이션", 20);
            
            SetQuestInfo("슬프지만 아빠에게 되돌아간다");
            yield return StartCoroutine(CheckNowScene("Hospital"));
            SetGameObject();
            SetQuestInfo("슬프지만 아빠에게 되돌아간다");
            ChatSelfStart("나레이션", 21);
            
            // 아빠 옆에 가면 이야기 시작
            // 지금은 일단 시간 기다리기
            SetQuestInfo("아빠 옆으로 가기");
            yield return StartCoroutine(CheckNowPlace("아빠 옆"));
            ChatSelfStart("나레이션", 22);
            
            // 일단 밤낮 바궈야함
            yield return new WaitForSeconds(3f);
            ChatSelfStart("나레이션", 23);
            SetQuestInfo("슬프지만 거리로 나가보자");
            // 저녁임
            yield return StartCoroutine(CheckNowScene("Street"));
            // 거리로 돌아왔으니 info 정보 갱신
            SetGameObject();
            jiyeon = GameObject.Find("황지영");
            jiyeon.transform.position = new Vector3(900, 0, 900);
            fox = GameObject.Find("Fox");
            fox.transform.position = new Vector3(999f, 999f, 999f);
            littlePrince.transform.position = new Vector3(999f, 0, -999f);
            SetQuestInfo("슬프지만 거리로 나가보자");
            
            yield return StartCoroutine(CheckNowPlace("#24"));
            ChatSelfStart("나레이션", 24);
            
            SetQuestInfo("놀이터로 가보자");
            yield return StartCoroutine(CheckNowPlace("모래놀이터"));
            ChatSelfStart("나레이션", 25);
            
            // 벤치로 이동
            SetQuestInfo("슬픈 마음과 함께 벤치로 간다...");
            yield return StartCoroutine(CheckNowPlace("놀이터벤치"));
            ChatSelfStart("나레이션", 26);
            
            SetQuestInfo("???가 옆으로 왔다? e키눌러서 계속 진행");
            yield return StartCoroutine(WaitForKeyPress(KeyCode.E));
            ChatSelfStart("???", 27);
            
            SetQuestInfo("???와 대화하기");
            yield return StartCoroutine(WaitForKeyPress(KeyCode.E));
            ChatSelfStart("나레이션", 28);
            
            SetQuestInfo("???에게 비행기를 받자");
            yield return StartCoroutine(WaitForKeyPress(KeyCode.E));
            ChatSelfStart("나레이션", 29);
            
            // 아침이 밝아서 장미 손에 계속 비행기 들려있음
            yield return StartCoroutine(WaitForKeyPress(KeyCode.E));
            ChatSelfStart("나레이션", 30);
            
            SetQuestInfo("다시 아빠의 병실로 돌아가자");
            yield return StartCoroutine(CheckNowScene("Hospital"));
            SetGameObject();
            
            // 아빠 옆으로가면 대화 시작
            yield return StartCoroutine(CheckNowPlace("아빠 옆"));
            ChatSelfStart("나레이션", 31);
            
            // 어린왕자 사진이 보여지면서
            ChatSelfStart("장미", 32);

            
            // 다시 거리로
            SetQuestInfo("거리로 나가보자");
            yield return StartCoroutine(CheckNowScene("Street"));
            SetGameObject();
            jiyeon = GameObject.Find("황지영");
            jiyeon.transform.position = new Vector3(900, 0, 900);
            fox = GameObject.Find("Fox");
            fox.transform.position = new Vector3(999f, 999f, 999f);
            littlePrince.transform.position = new Vector3(999f, 0, -999f);
            
            SetQuestInfo("저기 사람들이 많다...?");
            yield return StartCoroutine(CheckNowPlace("#33"));
            ChatSelfStart("나레이션", 33);
            
            
            SetQuestInfo("거리의 최유진에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("최유진"));
            
            SetQuestInfo("거리의 강민호에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("강민호"));
            
            SetQuestInfo("거리의 송지우에게 여우를 물어보자");
            yield return StartCoroutine(CheckLastChatNPC("송지우"));

            // 어린왕자 찾았음
            // 어린왕자 놀이터로 보내기
            SetQuestInfo("어린왕자를 찾으러 놀이터로 가자!");
            littlePrince.transform.position = new Vector3(80, 0, -53);
            yield return StartCoroutine(CheckNowPlace("playground"));
            ChatSelfStart("나레이션", 37);
            
            SetQuestInfo("어린왕자와 계속 이야기 나누기");
            yield return StartCoroutine(CheckNowPlace("#38"));
            ChatSelfStart("나레이션", 38);
            yield return new WaitForSeconds(0.3f);
            ChatSelfStart("나레이션", 39);
            yield return new WaitForSeconds(0.3f);
            ChatSelfStart("나레이션", 40);
            
            SetQuestInfo("여우가 돌아온 것 같다!!");
            Vector3 foxPosition = GameObject.Find("Street#41Enter").transform.position;
            fox.transform.position = new Vector3(foxPosition.x, 0.18f, foxPosition.z);
            yield return StartCoroutine(CheckNowPlace("#41"));
            ChatSelfStart("여우", 41);
            
            SetQuestInfo("집에 돌아온 아빠?");
            yield return StartCoroutine(CheckNowScene("Room"));
            SetGameObject();
            ChatSelfStart("나레이션", 42);
            // 엔딩....
            yield return new WaitForSeconds(1f);
            ChatSelfStart("나레이션", 43);
            
            // 이후 로직 작성
            break;
        }
    }

    IEnumerator WaitForKeyPress(KeyCode key)
    {
        // 키가 눌릴 때까지 대기
        while (!Input.GetKeyDown(key))
        {
            yield return null; // 프레임을 대기
        }
    }

    private void SetQuestInfo(string questDetail)
    {
        questInfo.GetComponent<TextMeshProUGUI>().text = "퀘스트 정보\n- " + questDetail;
    }

    private IEnumerator SubQuestRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(CheckGetSubQuest(1));
        }
    }
    
    private void ChatSelfStart(string npcName, int id)
    {
        chatPrint.isFreeChat = true;
        dialogueGroupId = id;
        chatPrint.ChatOpen(npcName);

    }

    private IEnumerator CheckLastChatNPC(string npcName)
    {
        while (playerData.lastChatNPC != npcName)
        {
            yield return null;
        }
    }
    
    // 대화 횟수 체크해서 퀘스트 완료시킵니다
    private IEnumerator CheckInitialConversations(int i)
    {
        while (playerData.talkCount < i)
        {
            yield return null; // Wait until talkCount reaches i
        }
    }

    private IEnumerator CheckNowPlace(string placeName)
    {
        while (playerData.nowPlace != placeName)
        {
            yield return null;      // 지정 장소로 이동할 때까지 대기
        }
    }

    private IEnumerator CheckNowScene(string sceneName)
    {
        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;      // 지정 신으로 이동할 때까지 대기 
        }    
    }
    
    private void GetMainQuest(int questNum)
    {
        QuestData questData = dataManager.questDataManager.questDataList[questNum - 1];
        playerData.PlayerQuestList.Add(questData);
        Debug.Log("메인퀘스트 #1 리스트 저장 완료!!");
    }

    private IEnumerator CheckGetSubQuest(int questNum)
    {
        while (subQuestNum != questNum)
        {
            yield return null;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
