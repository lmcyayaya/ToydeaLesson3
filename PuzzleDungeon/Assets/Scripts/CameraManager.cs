using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 pivot;
    public float zoomSpeed;
    Camera mainCamera;
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
        if( Input.GetAxis("Mouse ScrollWheel") < 0 )
        {
            targetSize += zoomSpeed * Time.deltaTime;
            targetSize = Mathf.Clamp(targetSize,2,5);
        }
        else if( Input.GetAxis("Mouse ScrollWheel") > 0 )
        {
            targetSize -= zoomSpeed * Time.deltaTime;
        }
        targetSize = Mathf.Clamp(targetSize,2,5);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize,targetSize,0.2f);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,player.transform.position+(pivot * mainCamera.orthographicSize / 2 ),0.7f);
        // if ((Input.touchCount  ==2) && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
        // {
        //     Touch touch1 = Input.GetTouch(0);
        //     Touch touch2 = Input.GetTouch(1);

        //     DoubleTouchCurrDis = Vector2.Distance(touch1.position, touch2.position);

        //     if (!IsZoom )
        //     {
        //         DoubleTouchLastDis = DoubleTouchCurrDis;
        //         IsZoom = true;
        //     }

        //     float distance = DoubleTouchCurrDis - DoubleTouchLastDis;
        //     //mainCamera.updistance += (distance > 0 ? 1 : -1) * 1 ;

        //     DoubleTouchLastDis = DoubleTouchCurrDis;
        // }
    }
}
