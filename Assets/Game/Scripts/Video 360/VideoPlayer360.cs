using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Master.UIKit;

public class VideoPlayer360 : MonoBehaviour
{
    string videoServerUrl = null;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;

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
    public async void DownloadAndLoadVideo(string prefix,string _url, string _audioUrl)
    {
        videoServerUrl = _url;
        await Services.DownloadVideoAndAudio(prefix,_url, _audioUrl, true, (localUrl, clip) =>
        {
            LoadLiveVideo(localUrl);
            audioSource.clip = clip;
        });
    }
    public void LoadLiveVideo(string _url)
    {
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
        print("Videoplayer Error->" + msg);
        LoadingUI.instance.OnScreenHide();
        UIController.instance.ShowPopupMsg("Videoplayer Error","Something went wrong.",()=> { if (!videoPlayer.isPrepared) LoadingUI.instance.OnScreenShow(); });
        videoPlayer.url = videoServerUrl.Replace("http:","https:");
        videoPlayer.Prepare();
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
        LoadingUI.instance.OnScreenHide();
        videoPlayer.Play();
    }

    void Started(VideoPlayer v)
    {
        print("Videoplayer Started");
        audioSource.Play();
    }

    public void Stop360Video()
    {
        videoPlayer.Stop();
        audioSource.Stop();
        videoPlayer.clip = null;
        audioSource.clip = null;
        //videoPlayer.url = string.Empty;
        //VideoContnetObj.SetActive(false);
        
    }

   
}
