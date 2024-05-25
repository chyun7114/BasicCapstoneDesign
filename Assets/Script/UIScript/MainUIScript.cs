using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    private GameObject menuPanel;
    private GameObject saveGamePanel;

    public static bool isMenuPanelOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        menuPanel = GameObject.Find("Canvas").transform.Find("MenuPanel").gameObject;
        saveGamePanel = GameObject.Find("Canvas").transform.Find("SaveGamePanel").gameObject;
        menuPanel.SetActive(false);
        saveGamePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (menuPanel.activeSelf)
            isMenuPanelOpen = true;
        else
            isMenuPanelOpen = false;
    }

    public void OnClickSaveAndExitButton()
    {
        menuPanel.SetActive(false);
        saveGamePanel.SetActive(true);
    }

    public void OnClickXButtonInSaveAndExit()
    {
        menuPanel.SetActive(true);
        saveGamePanel.SetActive(false);
    }
    
    public void OnClickXButtonInMenuPanel()
    {
        menuPanel.SetActive(false);
    }

    public void OnClickSaveSlot(GameObject saveSlotNum)
    {
        SaveDataManager manager = GameObject.Find("DataManager").GetComponent<SaveDataManager>();
        manager.SaveDataInJson(saveSlotNum);
        TitleUIScript.LoadScene("GameStart");
    }

    public void onClickExitButton()
    {
        TitleUIScript.LoadScene("GameStart");
    }
}
