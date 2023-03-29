using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Master.UI
{
    public class ScreenAboutThisAppUI : UIScreenView
    {
        //[SerializeField] TextMeshProUGUI txtDetails;
        //[SerializeField] ContentSizeFitter contentSizeFitter;
        //[SerializeField] ScrollRect scrollRect;

        //private void OnEnable()
        //{
        //    Events.WebRequestCompleted += AboutThisAppCallBack;
        //}
        //private void OnDisable()
        //{
        //    Events.WebRequestCompleted -= AboutThisAppCallBack;
        //}

        //public override void OnScreenShowCalled()
        //{
        //    TextUpdate();
        //    base.OnScreenShowCalled();
        //}
        //public override void OnBack()
        //{
        //    base.OnBack();
        //    BackToTrails();
        //}
        //public void BackToTrails()
        //{
        //    UIController.instance.ShowNextScreen(ScreenType.TrailList);
        //}

        //void AboutThisAppCallBack(API_TYPE aPI_TYPE, string obj)
        //{
        //    if (aPI_TYPE != API_TYPE.API_ABOUT_THIS_APP) return;
        //    TextUpdate();
        //}

        //async void TextUpdate()
        //{
        //    contentSizeFitter.enabled = false;
        //    txtDetails.text = ApiHandler.instance.data.AboutThisApp;
        //    await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        //    contentSizeFitter.enabled = true;
        //    await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        //    scrollRect.verticalNormalizedPosition = 1f;
        //}
    }
}