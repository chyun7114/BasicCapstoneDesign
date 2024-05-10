using System;
using Script.DataScript.Data.Interface;
using UnityEngine;


public class ItemData : IData
{
    [SerializeField]
    private int itemId;
    private string itemName;
    private string itemInfo;

    public void print()
    {
        Debug.Log($"itemId : {itemId} itemName : {itemName} itemInfo : {itemInfo}");
    }

    public bool matches(string kwd)
    {
        if (Int32.Parse(kwd) == itemId)
            return true;
        return false;
    }

    public int GetItemId
    {
        get => itemId;
    }

    public string GetItemName
    {
        get => itemName;
    }

    public string GetItemInfo
    {
        get => itemInfo;
    }
}
