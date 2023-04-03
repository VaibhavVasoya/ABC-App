using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFeedbackUI : UIScreenView
{

    [SerializeField] MovePanelAnimate feedbackPanel;

    [SerializeField] Image img0;
    [SerializeField] Image img1;
    [SerializeField] Image img2;

    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        ShowFeedbackPanel();
    }

    public async void SetImage()
    {
        //img0.sprite = Services.Download(ApiHandler.instance.data.feedBackOptions[0].num, ApiHandler.instance.data.feedBackOptions[0].icons);
        await Services.Download(ApiHandler.instance.data.feedBackOptions[0].num, ApiHandler.instance.data.feedBackOptions[0].icons, (texture) =>
        {
            img0.sprite = Asset.GetSprite(texture);
        });
        await Services.Download(ApiHandler.instance.data.feedBackOptions[1].num, ApiHandler.instance.data.feedBackOptions[1].icons, (texture) =>
        {
            img1.sprite = Asset.GetSprite(texture);
        });
        await Services.Download(ApiHandler.instance.data.feedBackOptions[2].num, ApiHandler.instance.data.feedBackOptions[2].icons, (texture) =>
        {
            img2.sprite = Asset.GetSprite(texture);
        });
    }

    async void ShowFeedbackPanel()
    {
        await Task.Delay(TimeSpan.FromSeconds(0.8f));
        feedbackPanel.ShowAnimation();
    }

    public void OnClickFeedBack0()
    {
        ApiHandler.instance.PostFeedback("1", TrailsHandler.instance.CurrentTrail.num, ApiHandler.instance.data.feedBackOptions[0].num);
        UIController.instance.ShowNextScreen(ScreenType.TrailList);
    }

    public void OnClickFeedBack1()
    {
        ApiHandler.instance.PostFeedback("1", TrailsHandler.instance.CurrentTrail.num, ApiHandler.instance.data.feedBackOptions[1].num);
        UIController.instance.ShowNextScreen(ScreenType.TrailList);
    }

    public void OnClickFeedBack2()
    {
        ApiHandler.instance.PostFeedback("1", TrailsHandler.instance.CurrentTrail.num, ApiHandler.instance.data.feedBackOptions[2].num);
        UIController.instance.ShowNextScreen(ScreenType.TrailList);
    }
}
