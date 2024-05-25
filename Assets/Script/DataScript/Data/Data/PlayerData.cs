﻿using System;
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
    public Vector3 playerPosition;

    private GameObject player;
    
    private void Start()
    {
        playerItemList = new List<ItemData>();
        playerQuestList = new List<QuestData>();
        playerPosition = new Vector3(0, 0, 0);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Street")
        {
            player = GameObject.Find("rose");
            playerPosition = player.transform.position;
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