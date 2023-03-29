using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenInformationalTrailsUI : UIScreenView
    {
    //    [SerializeField] GameObject trail_poi_Prefab;
    //    [SerializeField] Transform contentParent;
    //    [SerializeField] ScrollRect scrollRect;

    //    [SerializeField]ContentSizeFitter[] contentSizeFitters;

    //    private void OnEnable()
    //    {
    //        Events.WebRequestCompleted += SculptureTrailPoisCallBack;
    //    }
    //    private void OnDisable()
    //    {
    //        Events.WebRequestCompleted -= SculptureTrailPoisCallBack;
    //    }
    //    public override void OnScreenShowAnimationCompleted()
    //    {
    //        base.OnScreenShowAnimationCompleted();
    //        Refresh();
    //    }
    //    public override void OnBack()
    //    {
    //        base.OnBack();
    //        BackToTrailList();
    //    }
    //    public void BackToTrailList()
    //    {
    //        UIController.instance.ShowNextScreen(ScreenType.TrailList);
    //        TrailsHandler.instance.CurrentTrail = null;
    //    }
    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        for (int i = contentParent.childCount - 1; i >= 0; i--)
    //        {
    //            DestroyImmediate(contentParent.GetChild(i).gameObject);
    //        }

    //    }

    //    async void SculptureTrailPoisCallBack(API_TYPE aPI_TYPE, string obj)
    //    {
    //        if (aPI_TYPE != API_TYPE.API_TRAIL_POIS) return;
    //        if (TrailsHandler.instance.CurrentTrail == null || TrailsHandler.instance.CurrentTrail.type != TRAIL_TYPE.INFORMATIONAL) return;
    //        for (int i = contentParent.childCount - 1; i >= 0; i--)
    //        {
    //            DestroyImmediate(contentParent.GetChild(i).gameObject);
    //        }
    //        //await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
    //        foreach (var poi in ApiHandler.instance.data.trailPois)
    //        {
    //            if (poi.intro_id != TrailsHandler.instance.CurrentTrail.num) continue;
    //            //Debug.Log($"POI : {poi.intro_id} || Current Trail : {TrailsHandler.instance.CurrentTrail.num}");
    //            GameObject go = Instantiate(trail_poi_Prefab, contentParent);
    //            SculpTrailPoi sculpPoi = go.GetComponent<SculpTrailPoi>();
    //            sculpPoi.SetTrail(poi);
    //        }
    //        await Task.Delay(TimeSpan.FromSeconds(0.4f));
    //        Refresh();
    //    }

    //    async void Refresh()
    //    {
    //        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
    //        Array.Reverse(contentSizeFitters);
    //        await UIController.instance.RefreshContent(contentSizeFitters);
    //        scrollRect.verticalNormalizedPosition = 1f;
    //    }
    }
}