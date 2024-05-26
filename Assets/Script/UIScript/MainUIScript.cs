using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    private GameObject menuPanel;
    private GameObject saveGamePanel;
    private GameObject charInfoPanel;
    public static bool isMenuPanelOpen = false;

    private PlayerScript playerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        menuPanel = GameObject.Find("Canvas").transform.Find("MenuPanel").gameObject;
        saveGamePanel = GameObject.Find("Canvas").transform.Find("SaveGamePanel").gameObject;
        charInfoPanel = GameObject.Find("MainScreen").transform.Find("CharInfo").gameObject;
        playerScript = GameObject.Find("rose").GetComponent<PlayerScript>();

        charInfoPanel.SetActive(false);
        menuPanel.SetActive(false);
        saveGamePanel.SetActive(false);
    }

    // Update is called once per frame
    public void OnClickSaveAndExitButton()
    {
        menuPanel.SetActive(false);
        saveGamePanel.SetActive(true);
    }
    
    public void OnClickXButtonInMenuPanel(GameObject anyPanel)
    {
        anyPanel.SetActive(false);
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
