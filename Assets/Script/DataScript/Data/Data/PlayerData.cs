using System;
using System.Collections.Generic;
using System.Xml;
using Script.DataScript.Data.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData : MonoBehaviour, IData
{
    [SerializeField] 
    private string playerName;

    private List<QuestData> playerQuestList;
    private List<ItemData> playerItemList;
    
    public Vector3 playerPositionInStreet;
    public Vector3 playerPositionInHospital;

    public int talkCount = 0;
    public int lastTalkId = -1;
    public int currentTalkId = 0;

    public string nowPlace = null;
    
    private GameObject player;
    
    private void Start()
    {
        playerItemList = new List<ItemData>();
        playerQuestList = new List<QuestData>();
        // 나중에 게임 시작시 위치 설정
        playerPositionInStreet = new Vector3(0, 0, 0);
        // 병원신 입장 위치 설정
        playerPositionInHospital = new Vector3(2.57f,0,-3.03f);
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Street")
        {
            player = GameObject.Find("rose");
            playerPositionInStreet = player.transform.position;
        }
        else if (SceneManager.GetActiveScene().name == "Hospital")
        {
            player = GameObject.Find("rose");
            playerPositionInHospital = player.transform.position;
        }
    }

    public void print()
    {
        Debug.Log($"player name : {playerName}");
    }

    public bool matches(string kwd)
    {
        return false;
    }
    
    public string PlayerName 
    { 
        get => playerName;
        set => playerName = value;
    }

    public List<QuestData> PlayerQuestList
    {
        get => playerQuestList;
        set => playerQuestList = value;
    }

    public List<ItemData> PlayerItemList
    {
        get => playerItemList; 
        set => playerItemList = value;
    }
}