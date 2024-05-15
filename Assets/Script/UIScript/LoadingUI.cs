
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public GameObject loadingPanel;
    
    public Slider loadingBar; // 프로그레스 바
    public TextMeshProUGUI progressText;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void ShowLoadingScreen()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
    }

    public void HideLoadingScreen()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }
    
    public void UpdateLoadingProgress(float loadedDataCount, float totalDataCount)
    {
        float progress = loadedDataCount / totalDataCount;
        if (loadingBar != null)
        {
            loadingBar.value = progress;
        }
        if (progressText != null)
        {
            progressText.text = (progress * 100).ToString("F0") + "%";
        }
    }
}