using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Master.UI
{
    public class ScreenEventDetailsUI : UIScreenView
    {
        SculptureEvent _sculpEvent;
        [SerializeField] ImageLoader bg;
        [SerializeField] Text txtname;
        [SerializeField] Text txtStartDate;
        [SerializeField] TextMeshProUGUI txtDescription;
        [SerializeField] Text txtMobile;
        [SerializeField] Text txtEmail;
        [SerializeField] Text txtAddress;
        [SerializeField] Text txtBookingUrl;
        ContentSizeFitter[] contentSizeFitters;

        [SerializeField] ScrollRect scrollRect;

        //carousel images
        [SerializeField] GameObject ObjCarousel;
        [SerializeField] GameObject imgPrefab;
        [SerializeField] GameObject dotTrackPrefab;
        [SerializeField] GameObject carouselImgParent, dotParent;
        [SerializeField] ScrollControl scrollControl;
        [SerializeField] SwipeControl swipeControl;
        List<Transform> imgs;
        List<Toggle> dots;

        bool isShareInit = false;
        string shareStr;

        public override void OnAwake()
        {
            base.OnAwake();
            imgs = new List<Transform>();
            dots = new List<Toggle>();
            contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
            Array.Reverse(contentSizeFitters);
        }
        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            SetEventDetails();
            Events.WebRequestCompleted += ShareWithOtherCallBack;
            swipeControl.canSwipe = true;
        }
        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
            swipeControl.canSwipe = false;
            Events.WebRequestCompleted -= ShareWithOtherCallBack;

        }
        public override void OnScreenHideAnimationCompleted()
        {
            base.OnScreenHideAnimationCompleted();
            StopCoroutine("AutoScrollImages");
            Clear();
        }
        public override void OnBack()
        {
            base.OnBack();
            BackToEventsList();
        }
        public void BackToEventsList()
        {
            UIController.instance.ShowNextScreen(ScreenType.UpcomingEvents);
        }

        public void SetEventDetails()
        {
            _sculpEvent = TrailsHandler.instance.sculptureEvent;
            txtname.text = _sculpEvent.name;
            if (!string.IsNullOrEmpty(_sculpEvent.start_date) && _sculpEvent.start_date.Length > 4)
            {
                txtStartDate.transform.parent.gameObject.SetActive(true);
                txtStartDate.text = Helper.DateConvert(_sculpEvent.start_date).ToString("dd MMM");
            }
            else
                txtStartDate.transform.parent.gameObject.SetActive(false);
            txtDescription.text = UIController.instance.HtmlToStringParse(_sculpEvent.description);
            txtMobile.text = _sculpEvent.tel;
            txtEmail.text = _sculpEvent.email;
            txtAddress.text = _sculpEvent.address;
            txtBookingUrl.text = _sculpEvent.booking_url;

            if (_sculpEvent.multiple_images.Count > 1)
            {
                bg.gameObject.SetActive(false);
                ObjCarousel.SetActive(true);
                LoadImages();

            }
            else
            {
                bg.gameObject.SetActive(true);
                bg.Downloading(_sculpEvent.num, _sculpEvent.main_image);
                ObjCarousel.SetActive(false);
            }

            Refresh();
        }
        public void OnClickShare()
        {
            shareStr = "I thought you might be interested in this upcoming event:\n";
            shareStr += "Name : " + _sculpEvent.name + "\n";
            shareStr += "Start Date : " + _sculpEvent.start_date + "\n";
            shareStr += "Description : " + _sculpEvent.short_desc + "\n";
            shareStr += "Address : " + _sculpEvent.address + "\n";
            shareStr += "Telephone : " + _sculpEvent.tel + "\n";
            shareStr += "Email : " + _sculpEvent.email + "\n";
            shareStr += "Web : " + _sculpEvent.booking_url + "\n";
            shareStr += $"This event has been shared from the {Application.platform} mobile app, available for download from The ";

            if (ApiHandler.instance.data.shareWithOther == null)
            {
                ApiHandler.instance.GetShareWithOthers();
                isShareInit = true;
            }
            else
            {
                new NativeShare().SetSubject(Application.productName).SetTitle(_sculpEvent.name).SetText(shareStr).SetUrl(ApiHandler.instance.data.shareWithOther.share_link)
                .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
            }

        }
        public void OpenMobilenumber()
        {
            Application.OpenURL("tel:" + _sculpEvent.tel);

        }
        public void OpneMail()
        {
            Application.OpenURL("mailto:" + _sculpEvent.email);
        }
        public void OpneBookingUrl()
        {
            if (string.IsNullOrEmpty(_sculpEvent.booking_url) || _sculpEvent.booking_url.ToLower() == "na")
                UIController.instance.ShowPopupMsg("Oops!", "More info URL is Empty.", "Ok");
            else
                Application.OpenURL(_sculpEvent.booking_url);

        }
        public void Play360Video()
        {
            if (string.IsNullOrEmpty(_sculpEvent.video_360_url))
                UIController.instance.ShowPopupMsg("Oops!", "360 video not available.", "Ok");
            else
            {
                UIController.instance.ShowNextScreen(ScreenType.Video360);
                UIController.instance.getScreen(ScreenType.Video360).GetComponent<Screen360VideoUI>().DownloadAndLoadVideo(_sculpEvent.num, _sculpEvent.video_360_url, _sculpEvent.audio_url);
            }
        }



        async void Refresh()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            await UIController.instance.RefreshContent(contentSizeFitters);
            scrollRect.verticalNormalizedPosition = 1f;
        }

        //Map Content
        public void ShowGoogleMap()
        {
            if (!Services.CheckInternetConnection()) return;
            if (string.IsNullOrEmpty(_sculpEvent.logitude) || string.IsNullOrEmpty(_sculpEvent.latitude))
            {
                UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine event location.", "Ok");
                return;
            }
            UIController.instance.ShowNextScreen(ScreenType.EventPoiMap);
        }

        async void ShareWithOtherCallBack(API_TYPE aPI_TYPE, string obj)
        {
            if (aPI_TYPE != API_TYPE.API_SHARE_WITH_OTHER && !isShareInit) return;
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            isShareInit = false;
            new NativeShare().SetSubject(Application.productName).SetTitle(_sculpEvent.name).SetText(shareStr).SetUrl(ApiHandler.instance.data.shareWithOther.share_link)
                .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
        }

        public async void LoadImages()
        {
            StopCoroutine("AutoScrollImages");
            Clear();

            Toggle toggle = null;
            foreach (var item in _sculpEvent.multiple_images)
            {
                GameObject go = Instantiate(imgPrefab, carouselImgParent.transform);
                go.GetComponent<MultiImgPage>().LoadImage(item);

                GameObject dot = Instantiate(dotTrackPrefab, dotParent.transform);
                toggle = dot.GetComponent<Toggle>();
                toggle.group = dotParent.GetComponent<ToggleGroup>();

                EventTrigger trigger = dot.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerUp;
                entry.callback.AddListener((eventData) => { scrollControl.OnOpenScreen(trigger.transform.GetSiblingIndex()); });

                trigger.triggers.Add(entry);

                imgs.Add(go.transform);
                dots.Add(toggle);
            }
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            scrollControl.isNextWelcomeScreen = false;
            scrollControl.Init(imgs, dots);
            StartCoroutine("AutoScrollImages");
        }
        [SerializeField] float time;
        IEnumerator AutoScrollImages()
        {
            if (_sculpEvent.multiple_images.Count <= 1) yield break;
            while (true)
            {
                yield return new WaitForSeconds(time);
                scrollControl.IsImageTouchManual();
                scrollControl.BtnRight();
            }
        }
        public void StopAutoScrollImage()
        {
            StopCoroutine("AutoScrollImages");
        }

        void Clear()
        {
            if (imgs.Count > 0)
            {
                foreach (var img in imgs)
                {
                    DestroyImmediate(img.gameObject);
                }
            }
            if (dots.Count > 0)
            {
                foreach (var dot in dots)
                {
                    DestroyImmediate(dot.gameObject);
                }
            }
            imgs.Clear();
            dots.Clear();
        }
    }
}