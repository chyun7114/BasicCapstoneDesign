using System.Collections;
using System.Collections.Generic;
using Script.DataScript.Data.Interface;
using UnityEngine;


public class DialogueData : IData
{
    [SerializeField]
    private int dialogueId;
    private int nextDialogueId;
    private int npcId;
    private int questId;
    
    private string dialogueString;
    private string npcName;
    private int dialogueGroupId;

    // override method
    public void print()
    {
        Debug.Log($"{dialogueString}");
    }

    public bool matches(string kwd)
    {
        return false;
    }
    
    // getter, setter 설정 부분
    
    /*
     * C#의 getter, setter는 
     * getId, setId 따로 두는게 아니라 한꺼번에 둘 수 있어서
     * getId = player.Id
     * player.Id = 20
     * 이런 방식으로 사용하셔야 됩니다
     */
    public int DialogueId
    {
        get => dialogueId;
        set => dialogueId = value;
    }

    public int NextDialogueId
    {
        get => nextDialogueId;
        set => nextDialogueId = value;
    }
    
    public int NPCId
    {
        get => npcId;
        set => npcId = value;
    }

    public int QuestId
    {
        get => questId;
        set => questId = value;
    }
    
    public int DialogueGroupId
    {
        get => dialogueGroupId;
        set => dialogueGroupId = value;
    }

    public string DialogueString
    {
        get => dialogueString;
        set => dialogueString = value;
    }

    public string NpcName
    {
        get => npcName;
        set => npcName = value;
    }
}
