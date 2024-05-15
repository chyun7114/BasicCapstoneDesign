using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class QuestDataManager : MonoBehaviour
{
    // NPC singleton
    public static QuestDataManager instance;
    
    // 구글 스프레드시트 정보
    private static string questSheetAddress =
        "https://docs.google.com/spreadsheets/d/1ZeUuIqWMsNimTG9pKzrsbSvd5VKzeubUrnuxRmpg-wg";

    private static string questSheetDataRange = "A2:G";
    private static string questDataSheetId = "0";
    
    public List<QuestData> questDataList;
    private List<QuestData> npcQuestList;
    
    private string questDatas;
    
    // 퀘스트 순서 관리 객체
    public QuestManager questManager;
    
    public static QuestDataManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.Find("DataManager").AddComponent<QuestDataManager>();
            }

            return instance;
        }
    }
    
    
    public IEnumerator ReadQuestSheetData()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        
        // NPC 데이터 시트 주소 생성
        string address = SheetDataManager.SheetAddressToString(questSheetAddress, 
            questSheetDataRange, questDataSheetId);
        Debug.Log(address);

        // NPC 데이터 시트에서 데이터를 읽어오는 코루틴 시작
        yield return StartCoroutine(SheetDataManager.ReadSheetData(address));
        
        // 코루틴 실행 완료 후 데이터 저장
        StoreDataInList();
        
        DontDestroyOnLoad(gameObject);
    }
    
    // 코루틴 실행 후에 저장 및 테스트 가능합니다
    // 테스트 코드는 여기에 입력해 주세요
    private void StoreDataInList()
    {
        // 코루틴이 완료된 후에 npcDatas에 데이터 할당
        questDatas = SheetDataManager.datas;

        // 할당된 데이터를 사용하여 NPC 데이터 리스트 생성
        questDataList = GetDatas(questDatas);
        Debug.Log("QUEST DATA LOAD COMPLETE");
        // 테스트 코드 입력
        // foreach (QuestData element in questDataList)
        // {
        //     Debug.Log(element.QuestId);
        // }
    }
    
    
    public List<QuestData> GetDatas(string data){
        if (questDataList == null)
        {
            questDataList = new List<QuestData>();
        }
        string[] splitedData = data.Split("\n");

        foreach (string element in splitedData)
        {
            string[] datas = element.Split("\t");
            questDataList.Add(GetData(datas));
        }

        return questDataList;
    }
    public QuestData GetData(string[] datas)
    {
        object data = Activator.CreateInstance(typeof(QuestData));
        
        FieldInfo[] fields = typeof(QuestData)
            .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < datas.Length; i++)
        {
            string inputData = datas[i].Replace("\r", "");
            try
            {
                // string > parse
                Type type = fields[i].FieldType;

                if (string.IsNullOrEmpty(inputData)) continue;

                if (type == typeof(int))
                    fields[i].SetValue(data, int.Parse(inputData));

                else if (type == typeof(float))
                    fields[i].SetValue(data, float.Parse(inputData));

                else if (type == typeof(bool))
                    fields[i].SetValue(data, bool.Parse(inputData));

                else if (type == typeof(string))
                    fields[i].SetValue(data, inputData);

                // enum
                else
                    fields[i].SetValue(data, Enum.Parse(type, inputData));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        return (QuestData) data;
    }

    public List<QuestData> GetList
    {
        get => questDataList;
    }
}
