using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;
using Master.UI;

public class SculptureTrail : MonoBehaviour
{
    public Trail trail;

    [SerializeField] ImageLoader bg;
    [SerializeField] Text txtname;
    [SerializeField] Text txtShortDesc;
    [SerializeField] Text txtPoiCount;
    [SerializeField] Text txtPoiTime;

    //[SerializeField] Image imgStopsPointIcon;

    //[SerializeField] Sprite spStops;
    //[SerializeField] Sprite spPoints;

    int poiCount = 0;
    public void SetTrail(Trail _trail)
    {
        trail = _trail;
        if (!string.IsNullOrEmpty(_trail.title))
            txtname.text = _trail.title;
        else txtname.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(_trail.sub_title))
            txtShortDesc.text = _trail.sub_title;
        else txtShortDesc.gameObject.SetActive(false);
        poiCount = ApiHandler.instance.data.trailPois.FindAll(x => x.intro_id == trail.num).Count;

        txtPoiCount.text = poiCount.ToString() +" Stops";

        //if (string.IsNullOrEmpty(txtPoiCount.text) || txtPoiCount.text == "0") txtPoiCount.gameObject.SetActive(false);
        txtPoiTime.gameObject.SetActive(true);//if (string.IsNullOrEmpty(_trail.estimated_duration)) txtPoiTime.gameObject.SetActive(false);

        txtPoiTime.text = _trail.estimated_duration;// + " Hours"; // + " " + LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Hours");

        bg.Downloading(_trail.num, _trail.cover_image);
        //imgStopsPointIcon.sprite = (trail.type == TRAIL_TYPE.PHYSICAL) ? spStops : spPoints;
    }

    public void OnClickTrail()
    {
        TrailsHandler.instance.CurrentTrail = trail;
        UIController.instance.ShowNextScreen(ScreenType.Poi);
    }
}
