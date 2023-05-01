using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureScreenShot1 : MonoBehaviour
{


    int i = 0;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("CaptureSnap"))
        {
            i = PlayerPrefs.GetInt("CaptureSnap");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            i++;
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screens/1242_2208/Sceen_" + i + ".png");
        }
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("CaptureSnap",i);
        PlayerPrefs.Save();
    }
}
