using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NPCDataManager : MonoBehaviour
{
    // NPC singleton
    public static NPCDataManager instance;
    
    // 구글 스프레드시트 정보
    private static string npcSheetAddress =
        "https://docs.google.com/spreadsheets/d/1kvUgw2itgct0aEOObbZ3S3NXUhytNOiK33ZcowjBE-I";

    private static string npcSheetDataRange = "A2:B";
    private static string npcDataSheetId = "0";

    private static string npcDatas;
    // Npc 데이터 리스트
    public List<NPCData> npcDataList;
    
    // 싱글톤을 이용하여 구현함
    public NPCDataManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(NPCDataManager)) as NPCDataManager;

                if (instance == null)
                {
                    Debug.Log("no NpcDataManager Singleton");
                }
            }

            return instance;
        }
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    
        StartCoroutine(ReadNpcSheetData());
        
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator ReadNpcSheetData()
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
        string address = SheetDataManager.SheetAddressToString(npcSheetAddress, npcSheetDataRange, npcDataSheetId);
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
        npcDatas = SheetDataManager.datas;

        // 할당된 데이터를 사용하여 NPC 데이터 리스트 생성
        npcDataList = GetNPCDatas(npcDatas);
        
        // 테스트 코드 입력
        foreach (NPCData element in npcDataList)
        {
            Debug.Log("id = " + element.NpcId + "name = " + element.NpcName);
        }
    }
    
    // 이후 코드는 리팩토링 필요한 코드입니다
    // DataManager를 상속하는걸 목표로 리팩토링 할 예정입니다
    public List<NPCData> GetNPCDatas(string data)
    {
        if (npcDataList == null)
        {
            npcDataList = new List<NPCData>();
        }
        string[] splitedData = data.Split("\n");

        foreach (string element in splitedData)
        {
            string[] datas = element.Split("\t");
            npcDataList.Add(GetNPCData(datas));
        }

        return npcDataList;
    }
    
    
    public List<NPCData> GetNPCList()
    {
        return npcDataList;
    }

    public NPCData GetNPCData(string[] datas)
    {
        object data = Activator.CreateInstance(typeof(NPCData));
        
        FieldInfo[] fields = typeof(NPCData)
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < datas.Length; i++)
        {
            try
            {
                // string > parse
                Type type = fields[i].FieldType;

                if (string.IsNullOrEmpty(datas[i])) continue;

                if (type == typeof(int))
                    fields[i].SetValue(data, int.Parse(datas[i]));

                else if (type == typeof(float))
                    fields[i].SetValue(data, float.Parse(datas[i]));

                else if (type == typeof(bool))
                    fields[i].SetValue(data, bool.Parse(datas[i]));

                else if (type == typeof(string))
                    fields[i].SetValue(data, datas[i]);

                // enum
                else
                    fields[i].SetValue(data, Enum.Parse(type, datas[i]));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        
        
        return (NPCData)data;
    }
}
