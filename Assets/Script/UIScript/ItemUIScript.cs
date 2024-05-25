using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class ItemUIScript : MonoBehaviour
{
    public GameObject itemPrefab;
    public GameObject itemDetailPanelPrefab;
    public DataManager dataManager;

    private GameObject player;
    private Vector3 playerPosition;
    private GameObject activePanel;
    
    
    private List<ItemData> itemDataList;
    private List<ItemData> playerItemDataList;

    private bool isHide;
    private string imageUrl;

    void ItemUIInit()
    {
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
        itemDataList = dataManager.itemDataManager.GetList;
        playerItemDataList = dataManager.GetComponent<PlayerData>().PlayerItemList;
        playerPosition = dataManager.GetComponent<PlayerData>().playerPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        ItemUIInit();
        // 테스트용 코드입니다
        // playerItemDataList.Add(itemDataList[0]);
        // playerItemDataList.Add(itemDataList[2]);
        // playerItemDataList.Add(itemDataList[4]);
        InputPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Street");
        }
    }

    void InputPrefab()
    {
        GameObject go = GameObject.Find("ItemList").transform.gameObject;

        Transform t1 = go.transform.Find("ItemListTitle").transform;
        
        float firstDirX = t1.position.x;
        float firstDirY = t1.position.y - 50;

        for (int i = 0; i < itemDataList.Count; i++)
        {
            // 각 아이템의 위치 계산
            float dirX = firstDirX + (i % 4) * 220; // 120은 아이템 간 간격
            float dirY = firstDirY - (i / 4) * 220;
            Vector3 itemDir = new Vector3(dirX, dirY, 0);

            // 프리팹 인스턴스화 및 위치 지정
            GameObject newItem = Instantiate(itemPrefab, itemDir, Quaternion.identity,
                GameObject.Find("ItemList").transform);
            Button button = newItem.GetComponent<Button>();
            TextMeshProUGUI itemText = newItem.GetComponentInChildren<TextMeshProUGUI>();
            if (CheckPossesionOfItem(itemDataList[i]))
            {
                // 스프라이트 이미지 경로 확인
                Sprite newButtonSprite = Resources.Load<Sprite>(GetSpriteImagePath(i));
                if (newButtonSprite != null)
                {
                    button.image.sprite = newButtonSprite;
                    itemText.text = "";
                    newItem.gameObject.name = "Button_ID " + i;

                    // 버튼 이벤트 리스너 추가
                    button.onClick.AddListener(() => ItemButtonClick(newItem));
                }
                else
                {
                    Debug.LogError("Sprite Image is not Found");
                }
            }
            else
            {
                // 획득하지 못한 아이템은 정보 확인 불가능
                button.interactable = false;
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
        SceneManager.LoadScene("Street");
    }

    private string GetSpriteImagePath(int id)
    {
        return "ItemImage/" + (id + 1);
    }

    // 버튼을 클릭시 아이템 세부 정보 나타난다
    public void ItemButtonClick(GameObject clickedButton)
    {
        // 클릭된 버튼의 이름에서 아이템 ID 파싱
        string clickedButtonName = clickedButton.name;
        string[] splittedName = clickedButtonName.Split(" ");
        int clickedItemId = Int32.Parse(splittedName[1]);
        
        // 같은 아이템을 클릭했을 때 패널을 숨김
        if (activePanel != null && activePanel.name == "ItemDetailPanel #" + clickedItemId)
        {
            Destroy(activePanel);
            activePanel = null;
            return;
        }
        
        if (activePanel != null)
        {
            Destroy(activePanel);
        }
        // 아이템 디테일 패널 인스턴스 생성
        GameObject itemDetailPanel = Instantiate(itemDetailPanelPrefab, GameObject.Find("ItemList").transform);
        itemDetailPanel.gameObject.name = "ItemDetailPanel #" + clickedItemId;

        // 아이템 디테일 패널에 정보 설정
        SetPanel(itemDetailPanel, clickedItemId);

        activePanel = itemDetailPanel;
    }

    private void SetPanel(GameObject panel, int id)
    {
        // 이미지 넣기
        Sprite panelSprite = Resources.Load<Sprite>(GetSpriteImagePath(id));

        // 이미지 사진이 수정 안되서 일단 버튼으로 만듭니다
        Button image = panel.transform.Find("ItemDetailImage").gameObject.GetComponent<Button>();
        image.image.sprite = panelSprite;
        
        // 텍스트 업데이트
        TextMeshProUGUI itemDetailName = panel.transform.Find("ItemDetailName").
            gameObject.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemDetailText = panel.transform.Find("ItemDetailText").
            gameObject.GetComponent<TextMeshProUGUI>();

        itemDetailName.text = itemDataList[id].GetItemName;
        itemDetailText.text = itemDataList[id].GetItemInfo;
    }
}
