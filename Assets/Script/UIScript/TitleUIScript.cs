using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnclickStartGameButtonButton()
    {
        // newGame Button 누르면 게임 시작
        // 일단 SampleScene으로 넘어가게 해두었고
        // 추가적인 게임 시작시 필요한 메소드나 기능 필요시 그 기능 혹은 메소드로 넘어가도록 코드 변경
        LoadScene("SampleScene");
    }

    public void OnClickLoadGameButton()
    {
        // LoadGame Button을 누를 경우
        // 저장된 게임을 불러오는 기능 추가 예정
        // UI + 저장 데이터 불러오는 기능 필요
    }

    public void OnClickExitGameButton()
    {
        // 게임 종료 클릭할 경우
        // 게임이 종료됨
        // "게임을 종료하시겠습니까?"이런 멘트 들어간 팝업 띄우면 더 좋을듯
    }
}
