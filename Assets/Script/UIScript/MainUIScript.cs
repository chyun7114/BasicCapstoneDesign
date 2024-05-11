using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickSaveButton()
    {
        SaveDataManager manager = GameObject.Find("DataManager").GetComponent<SaveDataManager>();
        manager.SaveDataInJson();
        SceneManager.LoadScene("GameStart");
    }
}
