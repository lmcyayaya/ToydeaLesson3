using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 pivot;
    public float zoomSpeed;
    Camera mainCamera;
    Vector3 mouseStartPos;
    bool getStartPos;
    // bool IsZoom = false;
    // float DoubleTouchCurrDis;
    // float DoubleTouchLastDis;
    float targetSize;
    void Start()
    {
        mainCamera = Camera.main;
        targetSize = mainCamera.orthographicSize;
    }

    void FixedUpdate()
    {
        Zoom();
        CameraFollow();
    }
    void CameraFollow()
    {
        Vector3 targetPos;
        if(Input.GetMouseButton(2))
        {
            if(!getStartPos)
            {
                getStartPos = true;
                mouseStartPos = Input.mousePosition;
            }
            targetPos = player.transform.position+(pivot * mainCamera.orthographicSize / 2 ) - (Input.mousePosition - mouseStartPos) * 0.001f * mainCamera.orthographicSize;
            targetPos = new Vector3(Mathf.Clamp(targetPos.x,-4.48f,4.48f),Mathf.Clamp(targetPos.y,-6.64f,3.6f),targetPos.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,targetPos,0.7f);
        }
        else
        {
            getStartPos = false;
            targetPos = player.transform.position+(pivot * mainCamera.orthographicSize / 2 );
            targetPos = new Vector3(Mathf.Clamp(targetPos.x,-4.48f,4.48f),Mathf.Clamp(targetPos.y,-6.64f,3.6f),targetPos.z);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,targetPos,0.7f);
        }
    }
    void Zoom()
    {
        
        if( Input.GetAxis("Mouse ScrollWheel") < 0 )
        {
            targetSize += zoomSpeed * Time.deltaTime;
        }
        else if( Input.GetAxis("Mouse ScrollWheel") > 0 )
        {
            targetSize -= zoomSpeed * Time.deltaTime;
        }
        targetSize = Mathf.Clamp(targetSize,2,5);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize,targetSize,0.2f);
    }
}
