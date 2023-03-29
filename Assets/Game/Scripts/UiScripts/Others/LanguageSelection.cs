using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSelection : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;

    private void OnEnable()
    {
        Events.WebRequestCompleted += Init;
    }
    private void OnDisable()
    {
        Events.WebRequestCompleted -= Init;
    }

    public void Init(API_TYPE type, string data)
    {
        Events.WebRequestCompleted -= Init;
        if (type != API_TYPE.API_MULTI_LANGUAGE) return;
        dropdown.ClearOptions();
        foreach (var mulLang in ApiHandler.instance.data.multiLanguages)
        {
            Dropdown.OptionData od = new Dropdown.OptionData();
            od.text = mulLang.name;
            od.image = Asset.GetSprite(mulLang.texture2D);
            dropdown.options.Add(od);
        }
        dropdown.RefreshShownValue();
        
    }
   
}
