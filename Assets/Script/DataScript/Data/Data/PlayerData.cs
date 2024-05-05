
using System;
using System.Collections.Generic;
using Script.DataScript.Data.Interface;
using UnityEngine;

public class PlayerData : IData
{
    [SerializeField] 
    private string playerName;
    private List<QuestData> playerQuestList;
    private List<ItemData> playerItemList;
    
    public void print()
    {
        Debug.Log("Now plyaer name : " + playerName);
    }

    public bool matches(string kwd)
    {
        return false;
    }
    
    public string PlayerName { get; set; }
    public List<QuestData> PlayerQuestList { get; set; }
    public List<ItemData> PlyaerItemList { get; set; }
}
