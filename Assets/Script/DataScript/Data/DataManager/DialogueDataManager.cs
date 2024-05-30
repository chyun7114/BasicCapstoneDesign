using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using UnityEngine;

public class DialogueDataManager : MonoBehaviour
{
    // singleton
    public static DialogueDataManager instance;
    
    // 구글 스프레드시트 정보
    private static string dialogueSheetAddress =
        "https://docs.google.com/spreadsheets/d/1bP_gWBIkRQGeJ9XJTMHWnvTFsLQ180DP7MQvgmfpfQk";

    private static string dialogueSheetDataRange = "A2:H";
    private static string dialogueDataSheetId = "0";

    public List<DialogueData> dialogueDataList;
    public Dictionary<int, List<DialogueData>> dialogueDictionary;
    
    private string dialogueDatas;
    
    public static DialogueDataManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.Find("DataManager").AddComponent<DialogueDataManager>();
            }

            return instance;
        }
        set => instance = value;
    }
    
    public IEnumerator ReadDialogueSheetData()
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
        string address = SheetDataManager.SheetAddressToString(dialogueSheetAddress, dialogueSheetDataRange, dialogueDataSheetId);
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
        dialogueDatas = SheetDataManager.datas;

        // 할당된 데이터를 사용하여 NPC 데이터 리스트 생성
        dialogueDataList = GetDatas(dialogueDatas);
        Debug.Log("DIALOGUE DATA LOAD COMPLETE");

        dialogueDictionary = new Dictionary<int, List<DialogueData>>();
        
        foreach (var dialogue in dialogueDataList)
        {
            if (!dialogueDictionary.ContainsKey(dialogue.DialogueGroupId))
            {
                dialogueDictionary[dialogue.DialogueGroupId] = new List<DialogueData>();
            }
            dialogueDictionary[dialogue.DialogueGroupId].Add(dialogue);
        }
    }
    
    
    public List<DialogueData> GetDatas(string data){
        if (dialogueDataList == null)
        {
            dialogueDataList = new List<DialogueData>();
        }
        string[] splitedData = data.Split("\n");

        foreach (string element in splitedData)
        {
            string[] datas = element.Split("\t");
            dialogueDataList.Add(GetData(datas));
        }

        return dialogueDataList;
    }
    public DialogueData GetData(string[] datas)
    {
        object data = Activator.CreateInstance(typeof(DialogueData));
        
        FieldInfo[] fields = typeof(DialogueData)
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

        return (DialogueData) data;
    }

    public List<DialogueData> GetList
    {
        get => dialogueDataList;
    }
}
