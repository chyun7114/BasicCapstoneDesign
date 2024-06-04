using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public DialogueDataManager dialogueDataManager;
    public ItemDataManager itemDataManager;
    public NPCDataManager npcDataManager;
    public QuestDataManager questDataManager;

    public GameObject mainPanel;
    public LoadingUI loadingUI;
    
    private float totalDataCount = 4f; // 로드해야 할 총 데이터 개수
    private float loadedDataCount = 0f; // 로드된 데이터 개수

    public bool isLoad = false;
    
    // 나중에 다른 메소드로 바꿔서
    // 게임 시작버튼을 누르면 데이터를 로딩하던가 하는식으로
    // 메소드를 변경할 예정입니다.
    public void LoadDataManager(string sceneName)
    {
        // 데이터 매니저 불러오는 부분 한번에 합쳤습니다
        // 추후 로딩씬 구현때 사용할 예정
        dialogueDataManager = DialogueDataManager.Instance;
        itemDataManager = ItemDataManager.Instance;
        npcDataManager = NPCDataManager.Instance;
        questDataManager = QuestDataManager.Instance;
        
        // 로딩 패널 활성화
        mainPanel = GameObject.Find("GameStartUI").transform.Find("MainPanel").gameObject;
        mainPanel.SetActive(false);
        loadingUI.ShowLoadingScreen();
        
        // 데이터 불러옴
        StartCoroutine(LoadAllData(sceneName));
        
        DontDestroyOnLoad(gameObject);
    }
    
    
    // 데이터를 모두 불러온다
    // 이거 다 실행되기 전까지 로딩화면 띄울거임
    IEnumerator LoadAllData(string sceneName)
    {
        // 개별 데이터 로딩 코루틴 실행
        if (!isLoad)
        {
            yield return StartCoroutine(LoadDataWithProgress(dialogueDataManager.ReadDialogueSheetData()));
            yield return StartCoroutine(LoadDataWithProgress(itemDataManager.ReadItemSheetData()));
            yield return StartCoroutine(LoadDataWithProgress(npcDataManager.ReadNpcSheetData()));
            yield return StartCoroutine(LoadDataWithProgress(questDataManager.ReadQuestSheetData()));
        }
        
        // 모든 데이터 로딩 완료 후 로딩 화면 비활성화
        loadingUI.HideLoadingScreen();
        isLoad = true;
        TitleUIScript.LoadScene(sceneName);
    }

    IEnumerator LoadDataWithProgress(IEnumerator dataLoadCoroutine)
    {
        yield return StartCoroutine(dataLoadCoroutine);
        loadedDataCount++;
        loadingUI.UpdateLoadingProgress(loadedDataCount,totalDataCount);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
