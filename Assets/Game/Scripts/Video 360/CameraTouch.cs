using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouch : MonoBehaviour
{
    [SerializeField] GameObject resetPosButton;

    private Vector3 firstpoint; //change type on Vector3
  private Vector3 secondpoint;
  private float xAngle = 0; //angle for axes x for rotation
  private float yAngle;
  private float xAngTemp; //temp variable for angle
  private float yAngTemp;

    void Start()
    {
        //Initialization our angles of camera
        xAngle = 0;
        yAngle = 0;
        this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);
        Application.targetFrameRate = 60;
        //resetPosButton.SetActive(!SystemInfo.supportsGyroscope);
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            if (this.transform.parent != null)
            {
                this.transform.parent.Rotate(new Vector3(90f, 0f, 0f));
            }
        }
    }
    private void OnDisable()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = false;
        }
    }
    void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            // Invert the z and w of the gyro attitude
            this.transform.localRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        }
        //Check count touches
        else if (Input.touchCount > 0)
        {
            //Touch began, save position
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                firstpoint = Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            //Move finger by screen
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                secondpoint = Input.GetTouch(0).position;
                //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180f / Screen.width;
                yAngle = yAngTemp + (secondpoint.y - firstpoint.y) * 90f / Screen.height;
                //Rotate camera
                this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);
            }
        }
#if UNITY_EDITOR
        Rotate();
        #endif
    }
   
    void Rotate()
    {
        if (Input.GetMouseButton(0))
        {
            //Touch began, save position
            if (Input.GetMouseButtonDown(0))
            {
                firstpoint = Input.mousePosition;// Input.GetTouch(0).position;
                xAngTemp = xAngle;
                yAngTemp = yAngle;
            }
            //Move finger by screen
            if (Input.GetMouseButton(0))
            {
                secondpoint = Input.mousePosition;
                //Mainly, about rotate camera. For example, for Screen.width rotate on 180 degree
                xAngle = xAngTemp - (secondpoint.x - firstpoint.x) * 180f / Screen.width;
                yAngle = yAngTemp + (secondpoint.y - firstpoint.y) * 90f / Screen.height;
                //Rotate camera
                this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);
            }
        }
    }

    public void ResetPosition()
    {
        if (SystemInfo.supportsGyroscope)
        {
            //transform.localRotation = Quaternion.identity;
        }
        else
        {
            xAngTemp = xAngle = 0;
            yAngTemp = yAngle = 0;
            transform.rotation = Quaternion.identity;
        }
    }
}
