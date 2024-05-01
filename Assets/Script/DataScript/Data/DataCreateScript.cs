using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCreateScript : MonoBehaviour
{
    public NPCDataManager npcDataManager;
    // Start is called before the first frame update
    void Start()
    {
        npcDataManager = NPCDataManager.instance.Instance;
        StartCoroutine(npcDataManager.ReadNpcSheetData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
