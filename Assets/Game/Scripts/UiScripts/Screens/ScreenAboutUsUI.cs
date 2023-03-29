using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using TMPro;

namespace Master.UI
{
    public class ScreenAboutUsUI : UIScreenView
    {
        //[SerializeField] Text txtTitle;
        //[SerializeField] TextMeshProUGUI txtDetails;
        //[SerializeField] ContentSizeFitter contentSizeFitter;
        //[SerializeField] ScrollRect scrollRect;

        //private void OnEnable()
        //{
        //    Events.WebRequestCompleted += AboutUsCallBack;
        //}
        //private void OnDisable()
        //{
        //    Events.WebRequestCompleted -= AboutUsCallBack;
        //}

        //public override void OnScreenShowCalled()
        //{
        //    TextUpdate();
        //    txtTitle.text = ApiHandler.instance.data.menuList[3].title;
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

        //void AboutUsCallBack(API_TYPE aPI_TYPE, string obj)
        //{
        //    if (aPI_TYPE != API_TYPE.API_ABOUT_US) return;
            
        //    TextUpdate();
        //}

        //async void TextUpdate()
        //{
        //    if (string.IsNullOrEmpty(ApiHandler.instance.data.AboutUs)) {
        //        UIController.instance.ShowPopupMsg("","Data not found.");
        //        return; }
        //    contentSizeFitter.enabled = false;
        //    txtDetails.text = UIController.instance.HtmlToStringParse(ApiHandler.instance.data.AboutUs);
        //    await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        //    contentSizeFitter.enabled = true;
        //    await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        //    scrollRect.verticalNormalizedPosition = 1f;
        //}
    }
}