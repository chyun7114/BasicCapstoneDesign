using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemUIScript : MonoBehaviour
{
    public GameObject prefab;
    public GameObject dataManager;
        
    private List<ItemData> itemDataList;
    private List<ItemData> playerItemDataList;

    
    void ItemUIInit()
    {
        dataManager = GameObject.Find("DataManager");
        itemDataList = dataManager.GetComponent<ItemDataManager>().GetList;
        playerItemDataList = dataManager.GetComponent<PlayerData>().PlayerItemList;
    }
    // Start is called before the first frame update
    void Start()
    {
        ItemUIInit();
        InputPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("TestScene");
        }
    }

    void InputPrefab()
    {
        Transform t1 = GameObject.Find("ItemList").transform;
        
        float firstDirX = -550 + t1.position.x;
        float firstDirY = 250 + t1.position.y - 50;
        
        for (int i = 0; i < itemDataList.Count; i++)
        {
            float dirX = firstDirX + (i % 4) * 20+ 100 * (i % 4);
            float dirY = firstDirY - (i / 4) * 100 - (i / 4) * 20;
            Vector3 itemDir = new Vector3(dirX, dirY, 0);
            
            Instantiate(prefab, itemDir, Quaternion.identity, GameObject.Find("ItemList").transform);

            GameObject itemText = GameObject.Find("ItemText");
            
            if (CheckPossesionOfItem(itemDataList[i]))
            {
                itemText.GetComponent<TextMeshProUGUI>().text = itemDataList[i].GetItemName;
            }
        }
    }

    bool CheckPossesionOfItem(ItemData item)
    {
        foreach (var element in playerItemDataList)
        {
            if (element.matches(item.GetItemId.ToString()))
            {
                return true;
            }
        }
        return false;
    }

    public void ItemUIExit()
    {
        SceneManager.LoadScene("TestScene");
    }
}
