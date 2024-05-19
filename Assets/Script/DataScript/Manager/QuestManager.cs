using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // 퀘스트 관리를 위한 클래스
    
    // 데이터 매니저 객체
    private GameObject dataManager;
    // 게임 내 npc정보
    private List<NPCData> npcList;
    // 현재 게임 내 모든 퀘스트들의 정보
    private List<QuestData> questList;
    // 현재 플레이어 데이터 객체
    private List<QuestData> playerQuestList;
    // 플레이어가 지금까지 완료한 퀘스트 리스트
    private List<QuestData> successQuestList;
    // 현재 npc가 가지고 있는 퀘스트 정보
    private Dictionary<int, List<QuestData>> npcQuestDict;
    
    
    // 퀘스트 수행 가능 표시
    public GameObject newQuestPrefab;
    
    
    // 퀘스트 매니저 초기화 코드
    // 퀘스트 메니저는 퀘스트 데이터를 불러오자마자 바로 초기화된다
    public void Start()
    {
        dataManager = GameObject.Find("DataManager");
        npcList = dataManager.GetComponent<NPCDataManager>().NPCList;
        questList = dataManager.GetComponent<QuestDataManager>().GetList;
        playerQuestList = dataManager.GetComponent<PlayerData>().PlayerQuestList;
        npcQuestDict = new Dictionary<int, List<QuestData>>();
        
        SetQuestDict();
        // ShowQuestIcon();
        
        DontDestroyOnLoad(gameObject);
    }
    
    void SetQuestDict()
    {
        foreach (var element in questList)
        {
            if (!npcQuestDict.ContainsKey(element.NpcId))
            {
                npcQuestDict[element.NpcId] = new List<QuestData>();
            }
            npcQuestDict[element.NpcId].Add(element);
        }
        
        foreach (var kvp in npcQuestDict)
        {
            Debug.Log($"Quest ID: {kvp.Key}");

            foreach (var quest in kvp.Value)
            {
                Debug.Log($"Quest ID: {quest.QuestType}, Quest Name: {quest.QuestName}, Detail: {quest.QuestDetail}");
            }
        }
    }

    void ShowQuestIcon()
    {
        foreach (var element in npcList)
        {
            if (element.NpcQuestList.Count != 0)
            {
                GameObject go = GameObject.Find(element.NpcName);
                if (go != null)
                {
                    Vector3 newQuestDir = new Vector3(go.transform.position.x, go.transform.position.y + 2,
                        go.transform.position.z);

                    Instantiate(newQuestPrefab, newQuestDir, Quaternion.identity);
                }
            }
        }
    }
    
    // 먼저 퀘스트를 받기 위해 npc와의 상호작용이 끝나면 다음 코드 삽입
    public void InitQuest(int npcId)
    {
        
    }

    private NPCData GetNPCData(int npcId)
    {
        NPCData npcData = null;
        
        foreach (var element in npcList)
        {
            if (element.NpcId == npcId)
            {
                npcData = element;
                break;
            }
        }

        return npcData;
    }
    
    // 선행 퀘스트의 존재를 확인한다.
    // 만약 npc가 가지고 있는 퀘스트의 선행 퀘스트 존재 여부 확인시 사용
    private bool CheckPriorQuest(QuestData npcQuest)
    {
        if (npcQuest.PreviousQuestId == 9999)
            return true;
        return false;
    }
    // 플레이어의 선행 퀘스트 수행 여부 판단시 사용
    private bool CheckPriorQuest(QuestData npcQuest, QuestData playerQuest)
    {
        if (npcQuest.PreviousQuestId == 9999)
        {
            return true;
        }
        if (npcQuest.PreviousQuestId == playerQuest.QuestId)
        {
            if (playerQuest.IsProgress == true)
            {
                return true;
            }
        }
        return false;
    }
    
    // 퀘스트가 중복으로 받아지는지 확인시 사용
    private bool CheckDuplicateQuest(QuestData element, QuestData questData)
    {
        if (element.QuestId == questData.QuestId)
        {
            return true;
        }
        return false;
    }

    private List<QuestData> FindNPCQuest(int id)
    {
        List<QuestData> list = new List<QuestData>();

        foreach (var element in questList)
        {
            if (element.matches(id.ToString()))
            {
                list.Add(element);
            }
        }
        
        return list;
    }
}
