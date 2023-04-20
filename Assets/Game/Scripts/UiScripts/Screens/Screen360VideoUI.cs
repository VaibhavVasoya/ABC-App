using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master.UIKit;
using System.Threading.Tasks;
using System;

public class Screen360VideoUI : UIScreenView
{
    ScreenType lastScreen;

    [SerializeField] GameObject VideoContnetObj;
    [SerializeField] Transform videoContentParent;
    MyVideoPlayer _myVideoPlayer;

    public override void OnBack()
    {
        base.OnBack();
        BackToLastScreen();
    }
    public async void BackToLastScreen()
    {
        StopDownloading();
        await Task.Delay(TimeSpan.FromSeconds(0.6f));
        UIController.instance.ShowNextScreen(UIController.instance.previousScreen);
        Stop360Video();
    }

    void StopDownloading()
    {
        Services.isStop = true;
        if (UIController.instance.IsPopupEnable(ScreenType.PopupDownloding))
            UIController.instance.HidePopup(ScreenType.PopupDownloding);
    }

    public void DownloadAndLoadVideo(string prefix, string _url, string _audioUrl)
    {
        if (string.IsNullOrEmpty(_url) || _url.ToLower() == "na") return;
        _myVideoPlayer = Instantiate(VideoContnetObj, videoContentParent).GetComponent<MyVideoPlayer>();
        _myVideoPlayer.DownloadVideoAndAudio(prefix, _url, _audioUrl);
    }

    public void Stop360Video()
    {
        _myVideoPlayer.StopVideo();
        Destroy(_myVideoPlayer.gameObject);
        _myVideoPlayer = null;
    }
}
