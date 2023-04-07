using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Master.UIKit;
using System.Threading.Tasks;
using System;

namespace Master.UI
{
    public class ScreenMenuUI : UIScreenView
    {
        [SerializeField] List<Text> MenuTexts;
        [SerializeField] GameObject menuCeilPrefab;
        [SerializeField] Transform menuListParent;
        [SerializeField] ToggleGroup toggleGroup;

        [SerializeField] Toggle digitalTrail;
        [SerializeField] Toggle sculptureToggle;
        [SerializeField] Toggle upcomingEvetns;
        [SerializeField] Toggle villageDiscount;
        [SerializeField] Toggle aboutUs;
        [SerializeField] Toggle aboutThisApp;
        [SerializeField] Toggle shareWithOthers;
        [SerializeField] List<Toggle> trailsToggles;


        private void OnEnable()
        {
            Events.WebRequestCompleted += MenuListCallBack;
        }
        private void OnDisable()
        {
            Events.WebRequestCompleted -= MenuListCallBack;
        }

        private void Start()
        {
            InitializeMenuList();
        }
        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            SetSelectedScreen();
            upcomingEvetns.gameObject.SetActive(ApiHandler.instance.data.sculptureEvents.Count != 0);
            UIController.instance.IsBottomImageEnable(false);
        }
        public override void OnScreenShowAnimationCompleted()
        {
            base.OnScreenShowAnimationCompleted();
            UIController.instance.IsBottomImageEnable(true);
        }
        public override void OnBack()
        {
            base.OnBack();
            CloseMenu();
        }
        void InitializeMenuList()
        {
            Debug.LogError("Insitialize main menu....");            

            if (ApiHandler.instance.data.menuList.Count == 0)
            {
                ApiHandler.instance.GetMenuList();
                return;
            }

            MenuTexts[0].text = ApiHandler.instance.data.menuList[0].title;
            MenuTexts[1].text = ApiHandler.instance.data.menuList[1].title;
            MenuTexts[2].text = ApiHandler.instance.data.menuList[2].title;
            MenuTexts[3].text = ApiHandler.instance.data.menuList[3].title;

        }
        [SerializeField] float delay = 0.2f;
        public void CloseMenu()
        {
            UIController.instance.HideScreen(ScreenType.Menu);
        }


        public async void OpenTrailList()
        {
            //CancelInvoke("CheckSculpNearestMe");
            TrailsHandler.instance.MethodCancleInvoke();
            await Task.Delay(TimeSpan.FromSeconds(delay));
            CloseMenu();
            if (UIController.instance.getCurrentScreen() == ScreenType.TrailCat) return;
            UIController.instance.ShowNextScreen(ScreenType.TrailCat);
            ApiHandler.instance.GetTrailCat();
            TrailsHandler.instance.isInvokeNearestSculp = false;
        }

        public async void OpenUpcomingEvents()
        {
            //CancelInvoke("CheckSculpNearestMe");
            TrailsHandler.instance.MethodCancleInvoke();
            await Task.Delay(TimeSpan.FromSeconds(delay));
            CloseMenu();
            if (UIController.instance.getCurrentScreen() == ScreenType.UpcomingEvents) return;
            UIController.instance.ShowNextScreen(ScreenType.UpcomingEvents);
            ApiHandler.instance.GetSculptureEvetns();
            TrailsHandler.instance.isInvokeNearestSculp = false;
        }
        public async void OpenVillageDiscount()
        {
            //CancelInvoke("CheckSculpNearestMe");
            TrailsHandler.instance.MethodCancleInvoke();
            await Task.Delay(TimeSpan.FromSeconds(delay));
            CloseMenu();
            if (UIController.instance.getCurrentScreen() == ScreenType.VillageDiscount) return;
            UIController.instance.ShowNextScreen(ScreenType.VillageDiscount);
            ApiHandler.instance.GetVillageDiscount();
            TrailsHandler.instance.isInvokeNearestSculp = false;
        }
        public async void OpenAboutUsScreen()
        {
            //CancelInvoke("CheckSculpNearestMe");
            TrailsHandler.instance.MethodCancleInvoke();
            await Task.Delay(TimeSpan.FromSeconds(delay));
            CloseMenu();
            if (UIController.instance.getCurrentScreen() == ScreenType.AboutUs) return;
            //call api when u need.
            if (ApiHandler.instance.data.aboutUs == null)
            {
                ApiHandler.instance.GetAboutUsDetails();
            }
            UIController.instance.ShowNextScreen(ScreenType.AboutUs);
            TrailsHandler.instance.isInvokeNearestSculp = false;
        }
        public async void OpenAboutThisAppScreen()
        {
            //CancelInvoke("CheckSculpNearestMe");
            TrailsHandler.instance.MethodCancleInvoke();
            await Task.Delay(TimeSpan.FromSeconds(delay));
            CloseMenu();
            if (UIController.instance.getCurrentScreen() == ScreenType.AboutThisApp) return;
            //call api when u need.
            if (ApiHandler.instance.data.aboutTheApp == null)
            {
                ApiHandler.instance.GetAboutThisApp();
            }
            UIController.instance.ShowNextScreen(ScreenType.AboutThisApp);
            TrailsHandler.instance.isInvokeNearestSculp = false;
        }
        //public async void OpenMultiLanguageScreen()
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(delay));
        //    CloseMenu();
        //    if (UIController.instance.getCurrentScreen() == ScreenType.MultiLanguage) return;
        //    UIController.instance.ShowNextScreen(ScreenType.MultiLanguage);
        //}
        void SetSelectedScreen()
        {
            toggleGroup.SetAllTogglesOff();

            //digitalTrail.gameObject.SetActive(TrailsHandler.instance.currentTrailCat != null);

            switch (UIController.instance.getCurrentScreen())
            {
                case ScreenType.TrailCat:
                    digitalTrail.isOn = true;
                    break;
                case ScreenType.UpcomingEvents:
                    upcomingEvetns.isOn = true;
                    break;
                //case ScreenType.SculpPoi:
                //case ScreenType.SculpPoiDetails:
                //    digitalTrail.isOn = true;
                //    break;
                //case ScreenType.TrailPois:
                //case ScreenType.TrailPoiDetails:
                //    trailsToggles[0].isOn = true;
                //    break;
                //case ScreenType.VillageDiscount:
                //case ScreenType.VillageDiscountDetails:
                //case ScreenType.VillageDiscountMap:
                //    villageDiscount.isOn = true;
                //    break;
                //case ScreenType.SculptureEvents:
                //case ScreenType.SculptureEventsDetails:
                //    upcomingEvetns.isOn = true;
                //    break;
                //case ScreenType.AboutUs:
                //    aboutUs.isOn = true;
                //    break;
                //case ScreenType.AboutThisApp:
                //    aboutThisApp.isOn = true;
                //    break;

                default:
                    break;
            }
        }
        void MenuListCallBack(API_TYPE aPI_TYPE, string obj)
        {
            if (aPI_TYPE != API_TYPE.API_MENU) return;
            InitializeMenuList();
        }

    }
}