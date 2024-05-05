using System;
using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // 퀘스트 관리를 위한 클래스
    
    // 데이터 매니저 객체
    private GameObject dataManager;
    // 현재 게임 내 모든 퀘스트들의 정보
    private List<QuestData> questList;
    // 현재 플레이어 데이터 객체
    private PlayerData playerData;
    private List<QuestData> playerQuestList;
    
    // 퀘스트 매니저 초기화 코드
    // 퀘스트 메니저는 퀘스트 데이터를 불러오자마자 바로 초기화된다
    public void SetQuestManager()
    {
        dataManager = GameObject.Find("DataManager");
        questList = dataManager.GetComponent<QuestDataManager>().GetList;
        playerData = dataManager.GetComponent<PlayerData>();
        questList = playerData.PlayerQuestList;
        
        DontDestroyOnLoad(gameObject);
    }

    // 먼저 퀘스트를 받기위해 npc와의 상호작용이 끝나면 다음 코드 삽입
    public void initQuest(int questId)
    {
        // initQuest가 수행시 먼저 questId로 퀘스트 정보 찾기
        QuestData newQuestData = FindQuest(questId);
        Debug.Log(questId);

        // 선행 퀘스트의 존재 여부 확인
        // 선행퀘스트가 존재하지 않는 퀘스트는 id가 9999
        // 만약 선행 퀘스트 수행 완료이거나 존재하지 않을 시 퀘스트 수행 가능
        
    }

    public QuestData FindQuest(int questId)
    {
        foreach (var element in questList)
        {
            if (element.matches(questId.ToString()))
            {
                return element;
            }
        }

        return null;
    }
}
