using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;

public class ScreenFeedbackUI : UIScreenView
{

    [SerializeField] MovePanelAnimate feedbackPanel;

    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        ShowFeedbackPanel();
    }

    async void ShowFeedbackPanel()
    {
        await Task.Delay(TimeSpan.FromSeconds(0.8f));
        feedbackPanel.ShowAnimation();
    }

    public void OnClickFeedBack0()
    {
        ApiHandler.instance.PostFeedback("1",TrailsHandler.instance.CurrentTrail.num,ApiHandler.instance.data.feedBackOptions[0].num);
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
