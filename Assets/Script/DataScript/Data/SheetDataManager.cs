using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SheetDataManager : MonoBehaviour
{
    public static string datas;
    // 시트에서 데이터를 읽어옵니다
    // 구글 드라이브에 저장된 구글 스프레드 시트에서 데이터를 읽어옵니다
    // 구글 스프레드시트 정보와 작성 방법은 추후 회의 때 말씀드리겠습니다.
    public static IEnumerator ReadSheetData(string sheetAddress)
    {   
        UnityWebRequest www = UnityWebRequest.Get(sheetAddress);
        yield return www.SendWebRequest();
        
        datas = www.downloadHandler.text;
        Debug.Log(datas);
    }
    
    // 스프레드시트의 tsv주소 형식을 반환합니다
    // 스프레드 시트를 불러올때 주소 방식은 다음과 같아야 합니다
    // address => 스프레드 시트의 주소 (/edit 전까지)
    // range => 불러올 데이터의 스프레드 시트 내 범위
    // sheetID => 스프레드 시트 주소 중 gid = (이 숫자) 단, string타입이어야합니다.
    public static string SheetAddressToString(string address, string range, string sheetId)
    {
        return $"{address}/export?format=tsv&range={range}&gid={sheetId}";
    }
}
