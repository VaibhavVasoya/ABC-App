using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Master.UIKit;

public class MyVideoPlayer : MonoBehaviour
{
    string videoServerUrl = null;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    bool isGetVideoError = false; 
    public ulong Duration
    {
        get
        {
            return (ulong)(videoPlayer.frameCount / videoPlayer.frameRate);
        }
    }
    
    private void OnEnable()
    {
        videoPlayer.errorReceived += ErrorReceived;
        videoPlayer.frameReady += FrameReady;
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.prepareCompleted += PrepareCompleted;
        videoPlayer.started += Started;
    }
    private void OnDisable()
    {
        videoPlayer.errorReceived -= ErrorReceived;
        videoPlayer.frameReady -= FrameReady;
        videoPlayer.loopPointReached -= LoopPointReached;
        videoPlayer.prepareCompleted -= PrepareCompleted;
        videoPlayer.started -= Started;
    }
    private void Start()
    {
        videoPlayer.isLooping = true;
    }
    public async void DownloadVideoAndAudio(string prefix,string _url, string _audioUrl)
    {
        videoServerUrl = _url;
        await Services.DownloadVideoAndAudio(prefix,_url, _audioUrl, true, (localUrl, clip) =>
        {
            LoadLiveVideo(localUrl);
            if(audioSource) audioSource.clip = clip;
        });
    }
    public async void DownloadAndLoadVideo(string prefix, string _url)
    {
        Debug.Log("Load video : " + _url);
        videoServerUrl = _url;
        LoadingUI.instance.OnScreenShow();
        await Services.DownloadVideo(prefix,_url, (localUrl)=> LoadLiveVideo(localUrl));
        
    }
    public void LoadLiveVideo(string _url)
    {
        isGetVideoError = false;

        if (Services.isStop) return;

        if (videoPlayer.url != _url)
            videoPlayer.url = _url;
        
        if (!videoPlayer.isPrepared)
        {
            videoPlayer.Prepare();
            LoadingUI.instance.OnScreenShow();
        }
        else
            videoPlayer.Play();
    }

    void ErrorReceived(VideoPlayer video, string msg)
    {
        if (!isGetVideoError)
        {
            isGetVideoError = true;
            videoPlayer.url = videoServerUrl.Replace("http:", "https:");
            videoPlayer.Prepare();
        }
        else
        {
            LoadingUI.instance.OnScreenHide();
            //if (UIController.instance.getCurrentScreen() == ScreenType.PopupMSG) return;
            UIController.instance.ShowPopupMsg("Videoplayer Error", "Something went wrong.");
        }

        //LoadingUI.instance.OnScreenHide();
        //if (UIController.instance.getCurrentScreen() == ScreenType.PopupMSG) return;
        
        //UIController.instance.ShowPopupMsg("Videoplayer Error","Something went wrong.",()=>
        //{
        //    if (!videoPlayer.isPrepared && !isGetVideoError) LoadingUI.instance.OnScreenShow();
        //    if (!isGetVideoError)
        //    {
        //        isGetVideoError = true;
        //        videoPlayer.url = videoServerUrl.Replace("http:", "https:");
        //        videoPlayer.Prepare();
        //    }
        //});
    }

    void FrameReady(VideoPlayer v, long fram)
    {
    }

    void LoopPointReached(VideoPlayer v)
    {
        //print("Videoplayer Looppointreached");
    }
    void PrepareCompleted(VideoPlayer v)
    {
        print("Videoplayer prepare completed");
        videoPlayer.Play();
    }

    void Started(VideoPlayer v)
    {
        LoadingUI.instance.OnScreenHide();
        print("Videoplayer Play Started");
        if(audioSource) audioSource.Play();
    }

    public void StopVideo()
    {
        videoPlayer.Stop();
        videoPlayer.clip = null;
        //videoPlayer.url = null;
        if (audioSource)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

   
}
