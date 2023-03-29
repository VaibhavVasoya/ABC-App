using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferencesSetting : MonoBehaviour
{
    //[SerializeField] private Slider withSubTitleSlider;
    //[SerializeField] private Slider withoutSubTitleSlider;
    [SerializeField] private Dropdown LanguageDropdown;

    //[SerializeField] private PlanetariumStaticData staticData;

    [SerializeField] AnimationCurve curver;
    private float timer;
    private float estimateTime = .1f;
    private float startvalue;
    private float endValue;

    [SerializeField] private bool isSubTitleOn = false;

    private void OnEnable()
    {
        //LanguageDropdown.value = SavedDataHandler.instance._saveData.currentLanguage;
    }
   
    public void DropDownValue()
    {
        //Debug.Log("fffChange Language : " + (LANGUAGES)LanguageDropdown.value);
        ApiHandler.instance.currentLanguage = LanguageParse(LanguageDropdown.value);
        SavedDataHandler.instance._saveData.currentLanguage = LanguageDropdown.value;
        SavedDataHandler.instance._saveData.currentLanguageId = ApiHandler.instance.currentLanguage.num;
        //staticData.SetDataFirstTime();
        // local language and call api
    }

    public void DropDown1()
    {
        ApiHandler.instance.currentLanguage = LanguageParse(SavedDataHandler.instance._saveData.currentLanguage);
        //staticData.SetDataFirstTime();
        //SavedDataHandler.instance._saveData.currentLanguage = LanguageDropdown.value;
    }

    private void Start()
    {
        LanguageDropdown.value = SavedDataHandler.instance._saveData.currentLanguage;
        //withSubTitleSlider.value = (SavedDataHandler.instance._saveData.videoWithSubTitle) ? 1 : 0;
        //withoutSubTitleSlider.value = (SavedDataHandler.instance._saveData.videoWithoutSubTitle) ? 1 : 0;
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
        //SavedDataHandler.instance._saveData.currentLanguage = LanguageDropdown.value;
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
        //ApiHandler.instance.currentLanguage = ApiHandler.instance.data.multiLanguages.Find(x => x.name.ToLower() == GetSelectedLanguage().ToLower());
        //ApiHandler.instance.currentLanguage = LanguageParse(LanguageDropdown.value);
    }


    public void SetLanguageNew(bool isFirstTime = false)
    {
        //Debug.Log("fffsetlanguage");
        Debug.Log("asd2");
        //DropDownValue();
        if(isFirstTime)
        {
            DropDown1();
        }
        else
        {
            DropDownValue();
        }
        //SavedDataHandler.instance._saveData.currentLanguage = LanguageDropdown.value;
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
        //ApiHandler.instance.currentLanguage = ApiHandler.instance.data.multiLanguages.Find(x => x.name.ToLower() == GetSelectedLanguage().ToLower());
        //ApiHandler.instance.currentLanguage = LanguageParse(LanguageDropdown.value);
    }


    public string GetSelectedLanguage()
    {
        return LanguageDropdown.options[LanguageDropdown.value].text;
    }
    /// <summary>
    /// =======>with SubTitle ON OFF<=======
    /// </summary>
    //public void WithSubTitleOnOff()
    //{
    //    StartCoroutine(WithSubTitleSliderOnOff());
    //    if (SavedDataHandler.instance._saveData.videoWithoutSubTitle)
    //        StartCoroutine(WithoutSubTitleSliderOnOff());
    //}
    //IEnumerator WithSubTitleSliderOnOff()
    //{
    //    timer = 0;
    //    startvalue = withSubTitleSlider.value;
    //    endValue = (withSubTitleSlider.value == 0) ? 1 : 0;


    //    while (timer < estimateTime)
    //    {
    //        timer += Time.deltaTime;
    //        withSubTitleSlider.value = Mathf.Lerp(startvalue, endValue, curver.Evaluate(timer / estimateTime));
    //        yield return null;
    //    }
    //    withSubTitleSlider.value = endValue;
       
    //    SavedDataHandler.instance._saveData.videoWithSubTitle = (withSubTitleSlider.value == 1);
    //}

    /// <summary>
    /// =======>with SubTitle ON OFF<=======
    /// </summary>
    //public void WithoutSubTitleOnOff()
    //{
    //    StartCoroutine(WithoutSubTitleSliderOnOff());
    //    if (SavedDataHandler.instance._saveData.videoWithSubTitle)
    //        StartCoroutine(WithSubTitleSliderOnOff());
    //}
    //IEnumerator WithoutSubTitleSliderOnOff()
    //{
    //    float timer = 0;
    //    float startvalue = withoutSubTitleSlider.value;
    //    float endValue = (withoutSubTitleSlider.value == 0) ? 1 : 0;

    //    while (timer < estimateTime)
    //    {
    //        timer += Time.deltaTime;
    //        withoutSubTitleSlider.value = Mathf.Lerp(startvalue, endValue, curver.Evaluate(timer / estimateTime));
    //        yield return null;
    //    }
    //    withoutSubTitleSlider.value = endValue;

    //    SavedDataHandler.instance._saveData.videoWithoutSubTitle = (withoutSubTitleSlider.value == 1);
    //}

    MultiLanguage LanguageParse(int languageIndex)
    {
        return ApiHandler.instance.data.multiLanguages.Find(x => x.name.ToLower() == ((LANGUAGES)languageIndex).ToString().ToLower());
    }
}
