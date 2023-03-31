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
        //[SerializeField] ContentSizeFitter contentSizeFitter;
        [SerializeField] ScrollRect scrollRect;

        public ContentSizeFitter[] contentSizeFitters;

        private void Start()
        {
            Debug.Log("About Us: pre ");
            Debug.LogError("About iS enable ===>> " + (scrollRect.transform.GetChild(0).GetChild(0).gameObject.activeInHierarchy));
            contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
            Array.Reverse(contentSizeFitters);
            Debug.Log("length of CSF About US: "+contentSizeFitters.Length);
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
                UIController.instance.ShowPopupMsg("", "Data not found.");
                return;
            }
            img.Downloading("1",ApiHandler.instance.data.aboutUs.about_image);
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