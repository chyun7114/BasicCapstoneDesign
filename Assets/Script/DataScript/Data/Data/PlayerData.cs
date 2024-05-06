using System;
using System.Collections.Generic;
using System.Xml;
using Script.DataScript.Data.Interface;
using UnityEngine;

public class PlayerData : MonoBehaviour , IData
{
    [SerializeField] 
    private string playerName;
    private List<QuestData> playerQuestList;
    private List<ItemData> playerItemList;
    
    public void print()
    {
        Debug.Log($"player name : {playerName}");
    }

    public bool matches(string kwd)
    {
        return false;
    }
    
    public string PlayerName { get; set; }
    public List<QuestData> PlayerQuestList { get; set; }
    public List<ItemData> PlayerItemList { get; set; }
}