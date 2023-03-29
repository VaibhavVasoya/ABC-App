using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCamera : MonoBehaviour
{
    public WebCamTexture backCamera = null;
    public WebCamTexture frontCamera = null;
    public WebCamTexture activeCameraTexture = null;
    [SerializeField] RawImage img;
    WebCamDevice[] devices;

    [SerializeField] Material material;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Script has been started");


        devices = WebCamTexture.devices;
        Debug.LogError("Camera Found : " + devices.Length);
        if (devices.Length == 0)
            Debug.LogError("Camera Not Found.");

        foreach (var device in devices)
        {
            if (device.isFrontFacing)
                frontCamera = new WebCamTexture(device.name, Screen.width, Screen.height);
            else
                backCamera = new WebCamTexture(device.name, Screen.width, Screen.height);
        }
        PlayCamera();
    }

    public AspectRatioFitter fit;
    private void Update()
    {
        float ratio = (float)activeCameraTexture.width / (float)activeCameraTexture.height;
        fit.aspectRatio = ratio;


        //float ScaleY = activeCameraTexture.videoVerticallyMirrored ? -1f : 1f;
        //img.rectTransform.localScale = new Vector3(1f, ScaleY, 1f);

        int orient = -activeCameraTexture.videoRotationAngle;
        img.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
    bool isOnFrontCamera = false;
    public void ChangeCamera()
    {
        isOnFrontCamera = !isOnFrontCamera;
        PlayCamera();
    }
    void PlayCamera()
    {
        CameraSetup(backCamera, !isOnFrontCamera);
        CameraSetup(frontCamera, isOnFrontCamera);
    }

    void CameraSetup(WebCamTexture mCamera, bool isPlay)
    {
        if (mCamera != null)
        {
            if (isPlay)
                mCamera.Play();
            else
                mCamera.Stop();
            //material.mainTexture = mCamera;
            img.texture = mCamera;
            activeCameraTexture = mCamera;
            //img.rectTransform.localEulerAngles = new Vector3(0, 0, (activeCameraTexture.videoRotationAngle + 90) % 360);

            //material.mainTexture = mCamera;
        }
    }

    
}
