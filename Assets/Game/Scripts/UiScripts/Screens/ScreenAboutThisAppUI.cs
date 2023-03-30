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
        [SerializeField] Text txtTitle;
        [SerializeField] TextMeshProUGUI txtDetails;
        //[SerializeField] ContentSizeFitter contentSizeFitter;
        [SerializeField] ScrollRect scrollRect;

        public ContentSizeFitter[] contentSizeFitters;

        private void OnEnable()
        {
            Events.WebRequestCompleted += AboutUsCallBack;
        }
        private void OnDisable()
        {
            Events.WebRequestCompleted -= AboutUsCallBack;
        }

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
            //txtTitle.text = ApiHandler.instance.data.menuList[3].title;
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

        void AboutUsCallBack(API_TYPE aPI_TYPE, string obj)
        {
            if (aPI_TYPE != API_TYPE.API_ABOUT_THIS_APP) return;

            TextUpdate();
        }

        async void TextUpdate()
        {
            if (ApiHandler.instance.data.aboutTheApp == null)
            {
                UIController.instance.ShowPopupMsg("", "Data not found.");
                return;
            }
            //txtTitle.text = ApiHandler.instance.data.AboutUs;
            //contentSizeFitter.enabled = false;
            txtDetails.text = UIController.instance.HtmlToStringParse(ApiHandler.instance.data.aboutTheApp.about_app_text);
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
//}