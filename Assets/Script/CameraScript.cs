using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float yAxis;
    public float xAxis;
    
    public Transform target;
    
    private float rotSensitive = 3.0f;
    private float dis = 5f;
    private float rotationMin = 0f;
    private float rotationMax = 80f;
    private float smoothSpeed = 0.125f;
    
    private Vector3 targetRotation;
    private Vector3 currentVel;
    private bool whileChatting = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void StartChat()
    {
        whileChatting = true;
    }
    public void EndChat()
    {
        whileChatting = false;
    }
    
    // Update is called once per frame
    void Update()
    {

        yAxis = yAxis - Input.GetAxis("Mouse X") * -rotSensitive; // 마우스 좌우 움직임 이용
        xAxis = xAxis - Input.GetAxis("Mouse Y") * rotSensitive; // 마우스 상하 움직임 이용

        // x축 무한 회전 방지
        xAxis = Mathf.Clamp(xAxis, rotationMin, rotationMax);

        // 부드러운 카메라 회전
        targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(xAxis, yAxis),
            ref currentVel, smoothSpeed);

        this.transform.eulerAngles = targetRotation;

        // 카메라는 항상 일정 거리 유지
        transform.position = target.position - transform.forward * dis;
    }
}
