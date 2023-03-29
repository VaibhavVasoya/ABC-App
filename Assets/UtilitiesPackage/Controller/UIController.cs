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
        Feedback = 25
    }
    public class UIController : Singleton<UIController>
    {
        public GenralText GenralText;
        public ScreenType StartScreen;
        public List<UIScreen> Screens;

        [SerializeField]
        List<ScreenType> currentScreens;
        //[HideInInspector]
        public ScreenType previousScreen;
        //public static float AspectRatio;

        //public override void OnAwake()
        //{
        //    base.OnAwake();
        //    AspectRatio = Screen.width / (Screen.height * 1f);
        //}

        private IEnumerator Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            currentScreens = new List<ScreenType>();

            yield return null;
            ShowScreen(StartScreen);

            yield return new WaitForSeconds(1f);

            SavedDataHandler.instance.SetFirstLaunch();
        }
        
        public void ShowNextScreen(ScreenType screenType, float Delay = 0.2f)
        {
            if (currentScreens.Count > 0)
            {
                if(screenType != ScreenType.PopupMSG)
                    previousScreen = currentScreens.Last();
                HideScreen(currentScreens.Last());
            }
            else
            {
                Delay = 0;
            }

            StartCoroutine(ExecuteAfterDelay(Delay, () =>
            {
                ShowScreen(screenType);
            }));
        }

        public bool isUpdatingUI;
        public void ShowScreen(ScreenType screenType)
        {
            currentScreens.Add(screenType);

            getScreen(screenType).Show();

            isUpdatingUI = false;
            /* if(currentScreens.Find(x => x == screenType) != screenType)
             {
             }*/

        }
       
        public void HideScreen(ScreenType screenType)
        {

            getScreen(screenType).Hide();
            currentScreens.Remove(screenType);
            previousScreen = screenType;

        }

        public UIScreenView getScreen(ScreenType screenType)
        {
            return Screens.Find(screen => screen.screenType == screenType).screenView;
        }

        public ScreenType getCurrentScreen()
        {
            //return currentScreens.Last();
            return currentScreens.First();
        }
       
        [EasyButtons.Button]
        void CurrentScreen()
        {
            Debug.LogError("Current Screen : "+ getCurrentScreen());
            Debug.LogError("Last Screen : " + GetLastOpenScreen());
        }
        public ScreenType GetLastOpenScreen()
        {
            return currentScreens[currentScreens.Count-1];
        }
        IEnumerator ExecuteAfterDelay(float Delay, Action CallbackAction)
        {
            yield return new WaitForSeconds(Delay);

            CallbackAction();
        }

        public void ShowPopupMsg(string title,string msg)
        {
            getScreen(ScreenType.PopupMSG).GetComponent<PopupMsgUI>().SetMsg(title,msg);
            ShowScreen(ScreenType.PopupMSG);
        }
        public void ShowPopupMsg(string title, string msg, Action callback)
        {
            getScreen(ScreenType.PopupMSG).GetComponent<PopupMsgUI>().SetMsg(title, msg, callback);
            ShowScreen(ScreenType.PopupMSG);
        }
        public void OpenMenuScreen()
        {
            ShowScreen(ScreenType.Menu);
        }

        public void ShowDownlodingPopup(string downloadAssetName)
        {
            getScreen(ScreenType.PopupDownloding).GetComponent<PopupDownlodingUI>().DownlodAssetName = downloadAssetName;
            ShowScreen(ScreenType.PopupDownloding);
        }
        public void OpenScreens()
        {
            string s = "";
            foreach (var item in currentScreens)
            {
                s += item.ToString() + " || ";
            }
            Debug.LogError("Opne screens : " + s);
        }
        public void ChangeOrientation(ScreenOrientation screenOrientation)
        {
            if(ScreenOrientation.Portrait == screenOrientation)
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
            Helper.Execute(this,()=> Screen.orientation = ScreenOrientation.AutoRotation, 2f);
        }

        public async Task RefreshContent(ContentSizeFitter[] csf)
        {
            if(csf == null || csf.Length == 0) return;
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
    }

}