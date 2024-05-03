using Script.DataScript.Data.Interface;
using UnityEngine;

public class NPCData : IData
{
    [SerializeField] 
    private int npcId;
    private string npcName;

    public void print()
    {
        Debug.Log($"npcId : {npcId} npcName : {npcName}");
    }

    public bool matches(string kwd)
    {
        if (npcName.Equals(kwd))
            return true;
        return false;
    }
    
    /*
     * C#의 getter, setter는
     * getId, setId 따로 두는게 아니라 한꺼번에 둘 수 있어서
     * getId = player.Id
     * player.Id = 20
     * 이런 방식으로 사용하셔야 됩니다
     */
    public int NpcId
    {
        get => npcId;
        set => npcId = value;
    }

    public string NpcName
    {
        get => npcName;
        set => npcName = value;
    }
}
