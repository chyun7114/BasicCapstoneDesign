using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGameUIScript : MonoBehaviour
{
    public DataManager dataManager;
    
    // Start is called before the first frame update
    void Start()
    {
        SetLoadGameUI();
        dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLoadGameUI()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject loadGamePanel = GameObject.Find("SaveData_" + (i + 1));
            string filename = GetJsonFileName(i + 1);
            
            // Debug.Log(filename);
            // Debug.Log(loadGamePanel);

            if (!File.Exists(filename))
            {
                // 세이브 파일이 존재하지 않으면 버튼 선택 불가능
                GameObject SaveDataName = loadGamePanel.transform.GetChild(0).gameObject;
                GameObject SaveDataTime = loadGamePanel.transform.GetChild(1).gameObject;
                
                SaveDataName.SetActive(false);
                SaveDataTime.SetActive(false);

                Button button = loadGamePanel.GetComponent<Button>();

                button.interactable = false;
            }
            else
            {
                // 세이브 파일 존재할 시 버튼 선택 가능하게 만듬
                GameObject NoData = loadGamePanel.transform.GetChild(2).gameObject;
                
                Button button = loadGamePanel.GetComponent<Button>();
                button.onClick.AddListener(() => OnClickPanel(loadGamePanel));
                NoData.SetActive(false);
            }
        }
    }

    private string GetJsonFileName(int id)
    {
        return Application.streamingAssetsPath + "/SaveData/saveData " + id + ".json";
    }
    
    public void OnClickExitButton()
    {
        SceneManager.LoadScene("GameStart");
    }

    public void OnClickPanel(GameObject clickedPanel)
    {
        SaveDataManager manager = GameObject.Find("DataManager").GetComponent<SaveDataManager>();
        manager.LoadDataInJson(clickedPanel);
        TitleUIScript.LoadScene("Street");
    }
}
