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
    public class ScreenAboutUsUI : UIScreenView
    {
        [SerializeField] Text txtTitle;
        [SerializeField] TextMeshProUGUI txtDetails;
        [SerializeField] ImageLoader img;
        [SerializeField] ImageLoader belowImg;
        //[SerializeField] ContentSizeFitter contentSizeFitter;
        [SerializeField] ScrollRect scrollRect;

        public ContentSizeFitter[] contentSizeFitters;

        private void Start()
        {
            contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
            Array.Reverse(contentSizeFitters);
        }

        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            TextUpdate();
            Refresh();
        }
        public override void OnBack()
        {
            base.OnBack();
            BackToTrails();
        }
        public void BackToTrails()
        {
            UIController.instance.ShowNextScreen(ScreenType.TrailCat);
        }

        //void AboutUsCallBack(API_TYPE aPI_TYPE, string obj)
        //{
        //    if (aPI_TYPE != API_TYPE.API_ABOUT_US) return;

        //    TextUpdate();
        //}

        async void TextUpdate()
        {
            if (ApiHandler.instance.data.aboutUs ==null)
            {
                UIController.instance.ShowPopupMsg("", "Data not found.", "Ok");
                return;
            }
            img.Downloading("1",ApiHandler.instance.data.aboutUs.about_image);
            if (!string.IsNullOrEmpty(ApiHandler.instance.data.aboutUs.below_content_image))
                belowImg.Downloading("11", ApiHandler.instance.data.aboutUs.below_content_image);
            else belowImg.transform.parent.gameObject.SetActive(false);
            txtTitle.text = ApiHandler.instance.data.aboutUs.about_title;
            txtDetails.text = UIController.instance.HtmlToStringParse(ApiHandler.instance.data.aboutUs.about_text);
            Refresh();
        }

        async void Refresh()
        {
            //contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
            //Array.Reverse(contentSizeFitters);
            await UIController.instance.RefreshContent(contentSizeFitters);
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}