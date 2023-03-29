using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class StaticVariable : MonoBehaviour
{
    [SerializeField] VariableName variableName;
    Text txt;

    //StaticLanguageData staticLangData;
    //StaticData staticData;

    //private void OnDestroy()
    //{
    //    Events.OnLanguageChange -= LanguageChange;
    //}

    //void Awake()
    //{
    //    txt = GetComponent<Text>();  
    //    Events.OnLanguageChange += LanguageChange;
    //}

    //public void LanguageChange(string lang_id)
    //{
    //    //txt.text = ApiHandler.instance.data.staticData.staticLangData.Find(x => x.lang_id == lang_id).data.Find(x => x.meta == variableName.ToString().ToLower()).value;
       
    //    staticLangData = GetLangData(lang_id);
    //    if (staticLangData != null)
    //    {
    //        staticData = GetStaticData();
    //        if (staticData != null)
    //            txt.text = staticData.value;
    //        else
    //            Debug.Log("<color=red> Static variable is not match. </color>");
    //    }
    //    else
    //        Debug.Log("<color=red> Static language data is empty. </color>");

    //}
    
    //StaticLanguageData GetLangData(string lang_id) => ApiHandler.instance.data.staticData.staticLangData.Find(x => x.lang_id == lang_id);
    
    //StaticData GetStaticData() => staticLangData.data.Find(x => x.meta == variableName.ToString().ToLower());

}
