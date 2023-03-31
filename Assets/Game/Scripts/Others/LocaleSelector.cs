using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Localization.Settings;
using Master;
public class LocaleSelector : Singleton<LocaleSelector>
{
    public override void OnAwake()
    {
        base.OnAwake();
        DontDestroyOnLoad(this);
    }
    public void ChangeLonguage()
    {
        //StartCoroutine(SetLocale(PlayerPrefs.GetInt("PortaDownLanguage", 0)));
    }

    //IEnumerator SetLocale(int _localID)
    //{
    //    ApiHandler.instance.currentLanguage = ApiHandler.instance.data.multiLanguages.Find(x => x.name.ToLower() == ((LANGUAGES)_localID).ToString().ToLower());
    //    //yield return LocalizationSettings.InitializationOperation;
    //    //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localID];
        
    //}

}
