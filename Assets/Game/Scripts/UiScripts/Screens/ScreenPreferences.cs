using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPreferences : UIScreenView
{
    //[SerializeField] PreferencesSetting preferencesSetting;

    private void OnEnable()
    {
        Events.WebRequestCompleted += OnWebRequestComplete;
    }

    private void OnDisable()
    {
        Events.WebRequestCompleted -= OnWebRequestComplete;
    }

    private void Start()
    {
        LanguageDropdown.value = SavedDataHandler.instance._saveData.currentLanguage;
    }

    public override void OnScreenHideAnimationCompleted()
    {
        base.OnScreenHideAnimationCompleted();
        if (ApiHandler.instance.data.multiLanguages != null && ApiHandler.instance.data.multiLanguages.Count == 0) //[ApiHandler.instance.data.multiLanguages.Count - 1].texture2D != null)
            LoadingUI.instance.OnScreenShow();
    }


    async void OnWebRequestComplete(API_TYPE tYPE, string s)
    {
        if (tYPE == API_TYPE.API_MULTI_LANGUAGE)
        {
            SetLanguage(true);
            LoadingUI.instance.OnScreenHide();
        }
    }

    [SerializeField] private Dropdown LanguageDropdown;

    //[SerializeField] private PlanetariumStaticData staticData;

    [SerializeField] AnimationCurve curver;

    [SerializeField] private bool isSubTitleOn = false;


    public void DropDownValue()
    {
        ApiHandler.instance.currentLanguage = LanguageParse(LanguageDropdown.value);
        SavedDataHandler.instance._saveData.currentLanguage = LanguageDropdown.value;
        SavedDataHandler.instance._saveData.currentLanguageId = ApiHandler.instance.currentLanguage.num;
    }

    public void DropDown1()
    {
        ApiHandler.instance.currentLanguage = LanguageParse(SavedDataHandler.instance._saveData.currentLanguage);
    }

    public void SetLanguage(bool isFirstTime = false)
    {
        if (isFirstTime)
        {
            DropDown1();
        }
        else
        {
            DropDownValue();
        }
        LanguageDropdown.ClearOptions();
        foreach (var mulLang in ApiHandler.instance.data.multiLanguages)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = mulLang.name;
            od.image = Asset.GetSprite(mulLang.texture2D);
            LanguageDropdown.options.Add(od);
        }
        LanguageDropdown.RefreshShownValue();
        LanguageDropdown.value = SavedDataHandler.instance._saveData.currentLanguage;
    }


    public void SetLanguageNew(bool isFirstTime = false)
    {
        if (isFirstTime)
        {
            DropDown1();
        }
        else
        {
            DropDownValue();
        }
        LanguageDropdown.ClearOptions();
        foreach (var mulLang in ApiHandler.instance.data.multiLanguages)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = mulLang.name;
            od.image = Asset.GetSprite(mulLang.texture2D);
            LanguageDropdown.options.Add(od);
        }
        LanguageDropdown.RefreshShownValue();
        LanguageDropdown.value = SavedDataHandler.instance._saveData.currentLanguage;
    }

    public string GetSelectedLanguage()
    {
        return LanguageDropdown.options[LanguageDropdown.value].text;
    }

    MultiLanguage LanguageParse(int languageIndex)
    {
        return ApiHandler.instance.data.multiLanguages.Find(x => x.name.ToLower() == ((LANGUAGES)languageIndex).ToString().ToLower());
    }

    public void OnClickContinue()
    {
        UIController.instance.ShowNextScreen(ScreenType.PrepareForLaunch);
        ApiHandler.instance.GetWalkthroughScreen();
    }
}
