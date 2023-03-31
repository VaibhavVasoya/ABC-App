using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
//using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenLanguageUI : UIScreenView
    {
    //    [SerializeField] Dropdown languageDropdown;

    //    [SerializeField] Slider downloadProgress;
    //    [SerializeField] Text txtProgress;
    //    [SerializeField] MovePanelAnimate[] loadingPanelAnimate;
    //    [SerializeField] GameObject loading;
    //    private void Start()
    //    {
    //        Debug.LogError("init mul lang");
    //        languageDropdown.ClearOptions();
    //        foreach (var mulLang in ApiHandler.instance.data.multiLanguages)
    //        {
    //            Dropdown.OptionData od = new Dropdown.OptionData();
    //            od.text = mulLang.name;
    //            od.image = Asset.GetSprite(mulLang.texture2D);
    //            languageDropdown.options.Add(od);
    //        }
    //        languageDropdown.RefreshShownValue();
    //    }

    //    public override void OnScreenShowCalled()
    //    {
    //        TextUpdate();
    //        base.OnScreenShowCalled();
    //    }
    //    public override void OnBack()
    //    {
    //        base.OnBack();
    //        BackToTrails();
    //    }
    //    public void BackToTrails()
    //    {
    //        UIController.instance.ShowNextScreen(ScreenType.TrailList);
    //    }

    //    public async void SubmitAndSave()
    //    {
    //        if (Application.internetReachability == NetworkReachability.NotReachable)
    //        {
    //            ApiHandler.instance.ShowInternetMessage();
    //            return;
    //        }
    //        int lastLangDropValue = PlayerPrefs.GetInt("PortaDownLanguage", 0);
    //        lastProgress = -1;
    //        downloadProgress.value = 0;
    //        PlayerPrefs.SetInt("PortaDownLanguage", languageDropdown.value);
    //        int currentLangDropDownValue = PlayerPrefs.GetInt("PortaDownLanguage", 0);

    //        if (lastLangDropValue == currentLangDropDownValue) return;

    //        Debug.Log("Selectd Language : " + (LANGUAGES)languageDropdown.value);
    //        ApiHandler.instance.currentLanguage = ApiHandler.instance.data.multiLanguages.Find(x => x.name.ToLower() == ((LANGUAGES)languageDropdown.value).ToString().ToLower());
    //        LocaleSelector.instance.ChangeLonguage();
    //        if (ApiHandler.instance.currentLanguage != null)
    //        {
    //            //Events.WebRequestCompleted += InitDataFetched;
    //            loading.SetActive(true);
    //            ApiHandler.instance.LoadInitData();
    //            await Task.Delay(TimeSpan.FromSeconds(2));
    //            while (lastProgress < totalNumOfItems && !ApiHandler.instance.AllDataFetched)
    //            {
    //                await Task.Delay(TimeSpan.FromSeconds(1));
    //            }
    //            downloadProgress.value = 1;
    //            txtProgress.text = completeStr + " 100%";
    //            Events.OnDownlodProgress -= DownloadProgressUpdate;
    //            foreach (var item in loadingPanelAnimate)
    //            {
    //                item.HideAnimation();
    //            }
    //            loading.SetActive(false);
    //            //ApiHandler.instance.GetMenuList();
    //        }
    //    }



    //    void TextUpdate()
    //    {
    //        languageDropdown.value = PlayerPrefs.GetInt("PortaDownLanguage", (int)LANGUAGES.English);
    //    }

    //    void InitDataFetched(API_TYPE type, string data)
    //    {
    //        switch (type)
    //        {
    //            case API_TYPE.API_MENU:
    //                ApiHandler.instance.GetSculpturesTrails();
    //                break;
    //            case API_TYPE.API_TRAILS:
    //                ApiHandler.instance.GetTrailPois();
    //                break;
    //            case API_TYPE.API_TRAIL_POIS:
    //                //ApiHandler.instance.GetSculptureEvetns();
    //                Events.WebRequestCompleted -= InitDataFetched;
    //                LoadingUI.instance.OnScreenHide();
    //                break;
    //            //case API_TYPE.API_EVENTS:
    //            //    ApiHandler.instance.GetVillageDiscount();
    //            //    break;
    //            //case API_TYPE.API_DISCOUNTS:
    //            //    Events.WebRequestCompleted -= InitDataFetched;
    //            //    LoadingUI.instance.OnScreenHide();
    //            //    break;
    //            default:
    //                break;
    //        }
    //    }

    //    int totalNumOfItems;
    //    int itemFetched = 0;
    //    string currentFile = "";
    //    float lastProgress = -1;
    //    string completeStr;
    //    public void StartDownloading(int totalItems)
    //    {
    //        if (totalItems == 0) return;
    //        totalNumOfItems = totalItems;
    //        completeStr = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Completed");
    //        Events.OnDownlodProgress += DownloadProgressUpdate;
    //        foreach (var item in loadingPanelAnimate)
    //        {
    //            item.ShowAnimation();
    //        }
    //    }
    //    void DownloadProgressUpdate(float val)
    //    {
    //        if (currentFile != name)
    //        {
    //            currentFile = name;
    //            itemFetched++;
    //            Debug.LogError("Item : " + itemFetched);
    //        }
    //        lastProgress = (val + itemFetched);
    //        Debug.Log("Item:"+name+"total Item:" + totalNumOfItems + " || val:" + val + " || lastprogress:" + lastProgress);
    //        downloadProgress.value = Mathf.Clamp01((Helper.Map(0, totalNumOfItems, 0, 1, lastProgress)));
    //        txtProgress.text = completeStr + " " + (int)(downloadProgress.value * 100) + "%";
    //    }
    }
}