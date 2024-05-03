using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    // singleton
    public static ItemDataManager instance;
    
    // 구글 스프레드시트 정보
    private static string itemSheetAddress =
        "https://docs.google.com/spreadsheets/d/1ssSgrh5YppahWO402nMKX17EjPpaDy2rL8F-nBtMU10";

    private static string itemSheetDataRange = "A2:C";
    private static string itemDataSheetId = "0";

    public List<ItemData> itemDataList;
    
    private string itemDatas;
    
    public ItemDataManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(ItemDataManager)) as ItemDataManager;

                if (instance == null)
                {
                    Debug.Log("no ItemDataManager Singleton");
                }
            }

            return instance;
        }
    }
    
    // Start is called before the first frame update
    void Start()
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
        string address = SheetDataManager.SheetAddressToString(itemSheetAddress, itemSheetDataRange, itemDataSheetId);
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
        itemDatas = SheetDataManager.datas;

        // 할당된 데이터를 사용하여 NPC 데이터 리스트 생성
        itemDataList = GetDatas(itemDatas);
        Debug.Log("ITEM DATA LOAD COMPLETE");
        // 테스트 코드 입력
        // foreach (QuestData element in questDataList)
        // {
        //     Debug.Log(element.QuestId);
        // }
    }
    
    
    public List<ItemData> GetDatas(string data){
        if (itemDataList == null)
        {
            itemDataList = new List<ItemData>();
        }
        string[] splitedData = data.Split("\n");

        foreach (string element in splitedData)
        {
            string[] datas = element.Split("\t");
            itemDataList.Add(GetData(datas));
        }

        return itemDataList;
    }
    public ItemData GetData(string[] datas)
    {
        object data = Activator.CreateInstance(typeof(ItemData));
        
        FieldInfo[] fields = typeof(ItemData)
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

        return (ItemData) data;
    }

    public List<ItemData> GetList
    {
        get => itemDataList;
    }
}
