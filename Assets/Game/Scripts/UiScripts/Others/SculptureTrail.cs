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

    [SerializeField] Image imgStopsPointIcon;

    [SerializeField] Sprite spStops;
    [SerializeField] Sprite spPoints;

    int poiCount = 0;
    public void SetTrail(Trail _trail)
    {
        trail = _trail;
        txtname.text = _trail.title;
        txtShortDesc.text = _trail.sub_title;
        poiCount = ApiHandler.instance.data.trailPois.FindAll(x => x.intro_id == trail.num).Count;
        txtPoiCount.text = poiCount.ToString();// + " " + ((_trail.type == TRAIL_TYPE.PHYSICAL) ? LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Stops") : LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Points"));
        if (string.IsNullOrEmpty(txtPoiCount.text) || txtPoiCount.text == "0") txtPoiCount.gameObject.SetActive(false);
        txtPoiTime.gameObject.SetActive(trail.type == TRAIL_TYPE.PHYSICAL);
        if (string.IsNullOrEmpty(_trail.estimated_duration)) txtPoiTime.gameObject.SetActive(false);
        txtPoiTime.text = _trail.estimated_duration;// + " " + LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Hours");
        bg.Downloading(_trail.num, _trail.cover_image);
        imgStopsPointIcon.sprite = (trail.type == TRAIL_TYPE.PHYSICAL) ? spStops : spPoints;
    }

    public void OnClickTrail()
    {
        TrailsHandler.instance.CurrentTrail = trail;
        UIController.instance.ShowNextScreen(ScreenType.TrailList);
    }
}
