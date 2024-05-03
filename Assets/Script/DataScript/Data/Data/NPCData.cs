using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCData : MonoBehaviour
{
    [SerializeField] 
    private int npcId;
    private string npcName;

    public void print()
    {
        Debug.Log("id = " + this.npcId + "name = " + this.npcName);
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
