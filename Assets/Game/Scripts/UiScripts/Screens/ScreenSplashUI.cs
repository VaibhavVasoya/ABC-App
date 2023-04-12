using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenSplashUI : UIScreenView
    {
        

        float splashTime = 4;
        async void Start()
        {
            while(!ApiHandler.instance.isReadyToStartSplash)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            Debug.Log("splashtime = " + splashTime);
            await Task.Delay(TimeSpan.FromSeconds(splashTime));
            splashTime = 0;
            //Debug.Log("moveright");
            if (splashTime == 0)
            {
                UIController.instance.ShowNextScreen(ScreenType.PrepareForLaunch);
            }
        }
        
        
    }
}