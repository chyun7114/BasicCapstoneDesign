using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public DialogueDataManager dialogueDataManager;
    public ItemDataManager itemDataManager;
    public NPCDataManager npcDataManager;
    public QuestDataManager questDataManager;

    public GameObject loadingPanel;
    
    // 나중에 다른 메소드로 바꿔서
    // 게임 시작버튼을 누르면 데이터를 로딩하던가 하는식으로
    // 메소드를 변경할 예정입니다.
    void Start()
    {
        // 데이터 매니저 불러오는 부분 한번에 합쳤습니다
        // 추후 로딩씬 구현때 사용할 예정
        dialogueDataManager = DialogueDataManager.Instance;
        itemDataManager = ItemDataManager.Instance;
        npcDataManager = NPCDataManager.Instance;
        questDataManager = QuestDataManager.Instance;
        
        // 로딩 패널 활성화
        ShowLoadingPanel();
        
        // 데이터 불러옴
        StartCoroutine(LoadAllData());
        
        DontDestroyOnLoad(gameObject);
    }
    
    
    // 데이터를 모두 불러온다
    // 이거 다 실행되기 전까지 로딩화면 띄울거임
    private IEnumerator LoadAllData()
    {
        yield return StartCoroutine(dialogueDataManager.ReadDialogueSheetData());
        yield return StartCoroutine(itemDataManager.ReadItemSheetData());
        yield return StartCoroutine(npcDataManager.ReadNpcSheetData());
        yield return StartCoroutine(questDataManager.ReadQuestSheetData());

        HideLoadingPanel();
    }
    
    void ShowLoadingPanel()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
    }

    void HideLoadingPanel()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
