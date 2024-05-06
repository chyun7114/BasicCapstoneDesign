using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressE : MonoBehaviour
{
    public Camera mainCam;
    bool chatEnabled;
    Vector3 centerP;
    float rightadj=1.4f;
    float frontadj=0.8f;
    float heightadj=0.5f;
    
    // Start is called before the first frame update
    void LoadScene()
    {
        chatEnabled=false;
    }

    void Update()
    {
        if(chatEnabled){
            float yrot =mainCam.transform.localEulerAngles.y*Mathf.PI/180;
            transform.position = new Vector3
            (
                centerP.x-frontadj*Mathf.Sin(yrot)+rightadj*Mathf.Cos(yrot),
                centerP.y-heightadj,
                centerP.z-frontadj*Mathf.Cos(yrot)-rightadj*Mathf.Sin(yrot)
            );
            transform.LookAt(mainCam.transform);
            transform.rotation = Quaternion.LookRotation(mainCam.transform.forward);
        }
    }
    public void Pop(Vector3 npcP)
    {
        chatEnabled=true;
        centerP=npcP;
    }
    
    public void Hide()
    {
        chatEnabled=false;
        transform.position = new Vector3(0,-100,0);
    }
}
