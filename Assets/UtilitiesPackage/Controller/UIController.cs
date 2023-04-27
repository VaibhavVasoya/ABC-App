using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Master.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Master.UIKit
{
    [Serializable]
    public class UIScreen
    {
        public ScreenType screenType;
        public UIScreenView screenView;
    }

    public enum ScreenType
    {
        None = 1,
        Splash = 2,
        PopupMSG = 3,
        PopupDownloding = 4,
        Menu = 5,
        Walkthrough = 6,
        TrailCat = 7,
        TrailList = 8,
        Poi = 9,
        PoiDetails = 10,
        UpcomingEvents = 11,
        EventsDetails = 12,
        VillageDiscount = 13,
        VillageDiscDetails = 14,
        Quiz = 15,
        AboutUs = 16,
        AboutThisApp = 17,
        PrepareForLaunch = 18,
        Preferences = 19,
        EventPoiMap = 20,
        DiscountPoiMap = 21,
        PoiMap = 22,
        Video360 = 23,
        Image360 = 24,
        Feedback = 25,
        PopUpDownloadSize = 26,
        notificationPopUp = 27
    }



    public class UIController : Singleton<UIController>
    {
        public GenralText GenralText;
        public ScreenType StartScreen;
        public List<UIScreen> Screens;
        [SerializeField] Image bottomImage;

        [SerializeField] ScreenType currentScreens;
        [SerializeField] public List<UIScreen> currentPopup;
        //[HideInInspector]
        public ScreenType previousScreen;
        //public static float AspectRatio;

        //public override void OnAwake()
        //{
        //    base.OnAwake();
        //    AspectRatio = Screen.width / (Screen.height * 1f);
        //}

        public void IsBottomImageEnable(bool isOn)
        {
            bottomImage.gameObject.SetActive(isOn);
        }

        private IEnumerator Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            currentPopup = new List<UIScreen>();

            yield return null;
            ShowScreen(StartScreen);

            yield return new WaitForSeconds(1f);

            SavedDataHandler.instance.SetFirstLaunch();
        }

        public void ShowNextScreen(ScreenType screenType, float Delay = 0f)
        {
            Events.ScreenChange(false);
            if (currentScreens != ScreenType.None)
            {
                previousScreen = currentScreens;//.Last();
                HideScreen(previousScreen);
            }
            
            StartCoroutine(ExecuteAfterDelay(Delay, () =>
            {
                ShowScreen(screenType);
            }));
        }

        void ShowScreen(ScreenType screenType)
        {
           currentScreens = (screenType);
           getScreen(screenType).Show();
        }

        void HideScreen(ScreenType screenType)
        {
            getScreen(screenType).Hide();
            previousScreen = screenType;
        }

        public UIScreenView getScreen(ScreenType screenType)
        {
            return Screens.Find(screen => screen.screenType == screenType).screenView;
        }

        public ScreenType getCurrentScreen()
        {
            //return currentScreens.Last();
            return currentScreens;
        }
        UIScreen _popup = null;
        public void ShowPopup(ScreenType popup)
        {
            Debug.Log("Pop up show : "+popup.ToString());
            _popup = Screens.Find(x => x.screenType == popup);
            if (_popup == null) return;
            Events.ScreenChange(false);
            _popup.screenView.previousScreen = (currentPopup.Count == 0)? getScreen(getCurrentScreen()) : currentPopup.Last().screenView;
            currentPopup.Add(_popup);
            _popup.screenView.Show();
        }
        public void HidePopup(ScreenType popupType)
        {
            Events.ScreenChange(false);
            GetPopup(popupType).Hide();
            currentPopup.Remove(currentPopup.Find(pop => pop.screenType == popupType));
            //if (currentPopup.Count == 0)
            //    Helper.Execute(this, () => getScreen(getCurrentScreen()).ToggleRaycaster(true), 0.8f);
        }
        public UIScreenView GetPopup(ScreenType popupType)
        {
            return Screens.Find(pop => pop.screenType == popupType).screenView;
        }
        public bool IsPopupEnable(ScreenType popupType)
        {
            return currentPopup.Exists(pop => pop.screenType == popupType);
        }
        IEnumerator ExecuteAfterDelay(float Delay, Action CallbackAction)
        {
            yield return new WaitForSeconds(Delay);

            CallbackAction();
        }

        //public void ShowPopupMsg(string title, string msg, string btnName)
        //{
        //    //Debug.LogError("=======>>>>> PopupEnable");
        //    getScreen(ScreenType.PopupMSG).GetComponent<PopupMsgUI>().SetMsg(title, msg, btnName);
        //    ShowPopup(ScreenType.PopupMSG);
        //    //Debug.LogError("=======>>>>> PopupEnable completed");
        //}
        public void ShowPopupMsg(string title, string msg, string btnName, Action callback = null)
        {
            getScreen(ScreenType.PopupMSG).GetComponent<PopupMsgUI>().SetMsg(title, msg, btnName, callback);
            ShowPopup(ScreenType.PopupMSG);
        }
        public void OpenMenuScreen()
        {
            ShowPopup(ScreenType.Menu);
        }

        public void ShowDownlodingPopup(string downloadAssetName)
        {
            getScreen(ScreenType.PopupDownloding).GetComponent<PopupDownlodingUI>().DownlodAssetName = downloadAssetName;
            ShowPopup(ScreenType.PopupDownloding);
        }
        
        public void ChangeOrientation(ScreenOrientation screenOrientation)
        {
            if (ScreenOrientation.Portrait == screenOrientation)
            {
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = false;
                Screen.autorotateToLandscapeRight = false;
                Screen.orientation = ScreenOrientation.Portrait;
            }
            else
            {
                Screen.autorotateToPortrait = false;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            }
            Helper.Execute(this, () => Screen.orientation = ScreenOrientation.AutoRotation, 2f);
        }

        public async Task RefreshContent(ContentSizeFitter[] csf)
        {
            if (csf == null || csf.Length == 0) return;
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
            foreach (var item in csf)
            {
                if (item == null) continue;
                item.enabled = false;
                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
                item.enabled = true;
            }
        }

        public string HtmlToStringParse(string mainStr)
        {
            mainStr = mainStr.Replace("<p>", "");
            mainStr = mainStr.Replace("</p>", "");
            mainStr = mainStr.Replace("<strong>", "<b>");
            mainStr = mainStr.Replace("</strong>", "</b>");
            mainStr = mainStr.Replace("&nbsp;", " ");
            return mainStr;
        }

        //public void ShowPopUpDownloadSize(string title, string msg)
        //{
        //    getScreen(ScreenType.PopUpDownloadSize).GetComponent<ScreenPopDownloadSize>().SetMsg(title, msg);
        //    ShowPopup(ScreenType.PopUpDownloadSize);
        //}
        public void ShowPopUpDownloadSize(string title, string msg, Action callback = null)
        {
            getScreen(ScreenType.PopUpDownloadSize).GetComponent<ScreenPopDownloadSize>().SetMsg(title, msg, callback);
            ShowPopup(ScreenType.PopUpDownloadSize);
        }

        public void ShowNotificationPopUp(string title)
        {
            getScreen(ScreenType.notificationPopUp).GetComponent<NotificationPopup>().SetMsg(title);
            ShowPopup(ScreenType.notificationPopUp);
        }
    }

}