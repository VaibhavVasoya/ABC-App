using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Master.UI
{
    public class ScreenVidepPlayerUI : UIScreenView
    {
//        [SerializeField] FadePanelAnimate fadePanelAnimate;
//        [SerializeField] MyVideoPlayer myVideoPlayer;
//        public override void OnScreenShowCalled()
//        {
//            Services.isStop = false;
//            fadePanelAnimate.ShowAnimation();
//            LoadARScean();
//            myVideoPlayer.DownloadAndLoadVideo(TrailsHandler.instance.CurrentTrailPoi.num, TrailsHandler.instance.CurrentTrailPoi.transparent_video);
//            base.OnScreenShowCalled();
//        }
//        public override void OnScreenHideCalled()
//        {
//            base.OnScreenHideCalled();
//            //RemoveARScean();
//        }
//        public override void OnBack()
//        {
//            base.OnBack();
//            BackToTrailList();
//        }
//        public async void BackToTrailList()
//        {
//            StopDownloading();
//            await Task.Delay(TimeSpan.FromSeconds(0.6f));
//            StopVideo();
//            RemoveARScean();
//            fadePanelAnimate.ShowAnimation();
//            UIController.instance.ShowNextScreen(UIController.instance.previousScreen);
//        }

//        void StopDownloading()
//        {
//            Services.isStop = true;
//            if (UIController.instance.getCurrentScreen() == ScreenType.PopupDownloding)
//                UIController.instance.HideScreen(ScreenType.PopupDownloding);
//        }


//        public void StopVideo()
//        {
//            myVideoPlayer.StopVideo();
//        }

//        public async void LoadARScean()
//        {
//            LoadingUI.instance.OnScreenShow();
//            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("DeviceCamera", LoadSceneMode.Additive);
//            asyncOperation.allowSceneActivation = false;
//            while (!asyncOperation.isDone)
//            {
//                if (asyncOperation.progress >= 0.9f)
//                {
//                    await Task.Delay(TimeSpan.FromSeconds(1));
//                    asyncOperation.allowSceneActivation = true;
//                }
//                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
//            }
//            LoadingUI.instance.OnScreenHide();
//            await Task.Delay(TimeSpan.FromSeconds(0.4f));

//#if !UNITY_EDITOR

//            ARCameraManager aRCameraManager = FindObjectOfType<ARCameraManager>();
//            while (aRCameraManager == null)
//            {
//                await Task.Delay(TimeSpan.FromSeconds(0.1f));
//                aRCameraManager = FindObjectOfType<ARCameraManager>();
//            }
//            while (!aRCameraManager.permissionGranted)
//            {
//                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
//            }
//#endif
//            fadePanelAnimate.HideAnimation();
//                //FetchArData();
//            Debug.Log("Camera Load Successfuly.");
//        }

//        public async void RemoveARScean()
//        {
//            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync("DeviceCamera");
//            while (!asyncOperation.isDone)
//            {
//                if (asyncOperation.progress >= 0.9f)
//                {
//                    await Task.Delay(TimeSpan.FromSeconds(1));
//                }
//                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
//            }
            
//            Debug.Log("Camera Unload Successfuly.");
//        }

    }
}