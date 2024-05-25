using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// json 직렬화를 통해 데이터를 저장합니다
[SerializeField]
public class SaveData
{
    // 나중에 플레이어의 마지막 저장 좌표 등의 데이터 추가 가능
    public string playerName;
    public string saveTime;

    public Vector3 playerPosition;
    
    public List<int> playerItemIdList;
    public List<int> playerQuestIdList;
}


public class SaveDataManager : MonoBehaviour
{
    public DataManager dataManager;
    public PlayerData playerData;
    
    // Start is called before the first frame update
    void Start()
    {
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        playerData = GetComponent<PlayerData>();
        
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveDataInJson(GameObject clickedButton)
    {
        string clickedButtonName = clickedButton.name;
        
        string[] split = clickedButtonName.Split("_");
        
        int id = Int32.Parse(split[1]);
        
        // 파일 이름은 SaveData #.json으로 통일
        SaveData saveData = new SaveData();
        
        // 플레이어 데이터에서 Json으로 데이터 바꾸기
        saveData.playerItemIdList = new List<int>();
        saveData.playerQuestIdList = new List<int>();

        foreach (var element in playerData.PlayerItemList)
        {
            saveData.playerItemIdList.Add(element.GetItemId);
        }

        foreach (var element in playerData.PlayerQuestList)
        {
            saveData.playerQuestIdList.Add(element.QuestId);
        }
        
        saveData.playerName = playerData.PlayerName;
        saveData.saveTime =  DateTime.Now.ToString(("yyyy-MM-dd HH:mm:ss"));
        saveData.playerPosition = playerData.playerPosition;
        
        string json = JsonUtility.ToJson(saveData, true);
        // 아이디는 나중에 저장 창 구현시 바꾸기
        File.WriteAllText(GetJsonPath(id), json);
    }

    public void LoadDataInJson(GameObject clickedButton)
    {
        // 버튼 이름 찾아서 파싱
        string clickedButtonName = clickedButton.name;
        
        string[] split = clickedButtonName.Split("_");
        
        int id = Int32.Parse(split[1]);
        
        // Json에서 플레이어 데이터로 바꾸기
        // saveData에서 loadData 만들고 playerData안에 집어넣기
        SaveData loadData = new SaveData();

        loadData.playerItemIdList = new List<int>();
        loadData.playerQuestIdList = new List<int>();
        
        string loadJson = File.ReadAllText(GetJsonPath(id));
        loadData = JsonUtility.FromJson<SaveData>(loadJson);

        if (loadData != null)
        {
            SetPlayerData(loadData);
        }
        
    }

    private void SetPlayerData(SaveData loadData)
    {
        List<ItemData> loadItemList = new List<ItemData>();
        List<QuestData> loadQuestList = new List<QuestData>();

        List<ItemData> itemDataList = dataManager.itemDataManager.GetList;
        List<QuestData> questDataList = dataManager.questDataManager.GetList;
        
        foreach (int id in loadData.playerItemIdList)
        {
            loadItemList.Add(itemDataList[id - 1]);
        }
        
        foreach (int id in loadData.playerQuestIdList)
        {
            loadQuestList.Add(questDataList[id - 1]); 
        }

        playerData.PlayerName = loadData.playerName;
        playerData.PlayerItemList = loadItemList;
        playerData.PlayerQuestList = loadQuestList;
    }
    
    private string GetJsonPath(int id)
    {
        return Path.Combine(Application.streamingAssetsPath + "/SaveData/", "SaveData " + id + ".json");
    }
}
