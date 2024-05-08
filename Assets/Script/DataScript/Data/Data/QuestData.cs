using Script.DataScript.Data.Interface;
using UnityEngine;

public class QuestData : IData
{
    private int questId;
    private int nextQuestId;
    private int previousQuestId;
    private int npcId;
    private string questName;
    private string questDetail;
    private bool isProgress;
    
    public void print()
    {
        Debug.Log("획득 퀘스트 : " + questName);
    }

    public bool matches(string kwd)
    {
        if (kwd.Equals(npcId.ToString()))
            return true;
        if (kwd.Equals(questId.ToString()))
            return true;
        return false;
    }
    
    public int QuestId
    {
        get => questId;
        set => questId = value;
    }

    public int NextQuestId
    {
        get => nextQuestId;
        set => nextQuestId = value;
    }

    public int PreviousQuestId
    {
        get => previousQuestId;
        set => previousQuestId = value;
    }

    public int NpcId
    {
        get => npcId;
        set => npcId = value;
    }

    public string QuestName
    {
        get => questName;
        set => questName = value;
    }

    public string QuestDetail
    {
        get => questDetail;
        set => questDetail = value;
    }

    public bool IsProgress
    {
        get => isProgress;
        set => isProgress = value;
    }
}
