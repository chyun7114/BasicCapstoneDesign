using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float yAxis;
    public float xAxis;
    
    public Transform target;
    
    private float rotSensitive = 3.0f;
    private float dis = 7.5f;
    private float rotationMin = -13.5f;
    private float rotationMax = 80f;
    private float smoothSpeed = 0.125f;
    
    private Vector3 targetRotation;
    private Vector3 currentVel;
    private bool whileChatting = false;
    private bool isDragging = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // 초기화
        targetRotation = new Vector3(0, 180, 0); // 약간 아래를 바라보도록 설정 (원하는 각도로 조정 가능)
        transform.eulerAngles = targetRotation;

        // 타겟의 뒤쪽으로 카메라 위치 설정
        Vector3 setTargetPosition = target.position + new Vector3(0, 2, 0);
        transform.position = setTargetPosition - transform.forward * dis;
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

        // 채팅 중이 아니면 카메라를 이동시킴
        if (!whileChatting)
        {
            // 마우스 클릭 여부 확인
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                // 마우스 좌우 움직임 이용
                yAxis += Input.GetAxis("Mouse X") * rotSensitive;
                // 마우스 상하 움직임 이용
                xAxis -= Input.GetAxis("Mouse Y") * rotSensitive;

                // x축 무한 회전 방지
                xAxis = Mathf.Clamp(xAxis, rotationMin, rotationMax);

                // 부드러운 카메라 회전
                targetRotation = Vector3.SmoothDamp(targetRotation, new Vector3(xAxis, yAxis), ref currentVel, smoothSpeed);

                transform.eulerAngles = targetRotation;
            }

            // 카메라는 항상 일정 거리 유지
            Vector3 setTargetPosition = target.position + new Vector3(0, 2, 0);
            transform.position = setTargetPosition - transform.forward * dis;
        }
    }
}
