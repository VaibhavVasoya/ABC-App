using System;
using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPopDownloadSize : UIScreenView
{
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtMsg;

    Action callBack = null;

    public override void OnScreenHideAnimationCompleted()
    {
        base.OnScreenHideAnimationCompleted();
        txtMsg.text = txtTitle.text = "";
    }
    public override void OnBack()
    {
        base.OnBack();
        ClosePopup();
    }

    public void SetMsg(string title, string msg, Action callback = null)
    {
        txtTitle.text = title;
        txtMsg.text = msg;
        callBack = callback;
    }

    public void ClosePopup()
    {
        callBack?.Invoke();
        UIController.instance.HideScreen(ScreenType.PopUpDownloadSize);
    }

    public void OnClickOk()
    {
        Services.LoadBundlesParellel(ApiHandler.instance.DownloadFiles);
        ClosePopup();
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }
}

