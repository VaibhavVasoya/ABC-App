using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master.UIKit;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;

namespace Master.UI
{
    public class ScreenPrepareForLaunch : UIScreenView
    {
        [SerializeField] Slider loadProgress;
        public List<string> downloadFiles;

        //Dictionary<string, float> downloadItems = new Dictionary<string, float>();

        //[SerializeField] float progress = 0, duration;

        //[SerializeField] float lastProgress = -1;
        //string filename = "";

        //[SerializeField] ParticleSystem boostEffect;
        [SerializeField] GameObject loader;

        [SerializeField] Text textForProgress;

        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            ApiHandler.instance.LoadInitData();
           
        }
        public int totalNumOfItems = 0;
        int itemFetched = 0;
        string currentFile = "";


        public void StartDownloading(int totalItems)
        {

            totalNumOfItems = totalItems;
            if (totalItems < 1)
            {
                ShowLoadingBarDummy();
            }
            else
            {
                Debug.Log("123 subscribe");
                Events.OnDownlodProgress += DownloadProgressUpdate;
            }

        }
        void DownloadProgressUpdate(float val)
        {
            loadProgress.value = val;
            textForProgress.text = "We are at  " + ((int)(val * 100)).ToString() + "%";
            //Debug.Log("download pregress update "+ loadProgress.value);
            if (loadProgress.value == 1)
            {
                Events.OnDownlodProgress -= DownloadProgressUpdate;
                //if (SceneManager.GetActiveScene().name == "Splash")
                {
                    LoadApp();
                }
                //else
                //{
                //    ShowTrailScreen();
                //}
            }
        }

        async void LoadApp()
        {
            //loader.SetActive(false);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("App 1");
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
            }
        }

        //async void ShowTrailScreen()
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(1));
        //    UIController.instance.ShowNextScreen(ScreenType.TrailList);
        //}

        public async void ShowLoadingBarDummy()
        {
            float t = 0, d = 3;
            while (t < d)// (ApiHandler.instance.downloadProgres < ApiHandler.instance.DownloadFiles.Count)
            {
                t += Time.deltaTime;
                loadProgress.value = t / d;
                textForProgress.text = "We are at "  + ((int)(loadProgress.value * 100)).ToString() + "%";
                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
            }
            //Debug.Log("<color=green>=========>>>>>> Donwloding done.<<<<<<==========</color>");
            loadProgress.value = 1;
            await Task.Delay(TimeSpan.FromSeconds(1f));
            //await Task.Delay(TimeSpan.FromSeconds(0.5f));
            //if (SceneManager.GetActiveScene().name == "Splash")
            {
                LoadApp();
            }
            //else
            //{
            //    ShowTrailScreen();
            //}
        }
        
    }

    
}