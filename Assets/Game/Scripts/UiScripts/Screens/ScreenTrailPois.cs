using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenTrailPois : UIScreenView
{
    [SerializeField] Text txtpoiTitle;
    [SerializeField] GameObject trail_poi_Prefab;
    [SerializeField] Transform contentParent;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] Toggle poiListToggle;
    [SerializeField] Toggle poiMapToggle;

    [SerializeField] GameObject poiListContentObj;
    [SerializeField] GameObject poiMapContentObj;

    ContentSizeFitter[] contentSizeFitters;

    [SerializeField] OnlineMapsRawImageTouchForwarder onlineMapsRawImageTouchForwarder;
    [SerializeField] float Zoom_offset = -1;

    [SerializeField] ImageLoader poiImg;
    [SerializeField] Text txtpoiName;
    [SerializeField] TextMeshProUGUI txtpoiSubDesc;
    [SerializeField] MovePanelAnimate poiPanelAnimate;

    int currentSelectedPoiPin = 0;
    [SerializeField] List<Poi> pois;



    private void OnEnable()
    {
        Events.WebRequestCompleted += SculptureTrailPoisCallBack;
        //isCheckFeedBack = true;
    }
    private void OnDisable()
    {
        Events.WebRequestCompleted -= SculptureTrailPoisCallBack;
        //isCheckFeedBack = false;
    }
    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        //Invoke("CheckSculpNearestMe",1);
        //TrailsHandler.instance.CheckSculpNearestMe();
        //check location permissiton
        //MapController.instance.CkeckLocationPermission();
        //TrailsHandler.instance.MethodInvoke();
        TrailsHandler.instance.CurrentTrailPoi = null;
        //TrailsHandler.instance.isInvokeNearestSculp = true;
        poiListToggle.isOn = true;
        OpenTab(poiMapToggle.isOn);
        if (UIController.instance.previousScreen == ScreenType.TrailList)
        {
            Debug.Log("instiate");
            SculptureTrailPoisCallBack(API_TYPE.API_TRAIL_POIS, "");
        }
    }

    public override void OnScreenShowAnimationCompleted()
    {
        base.OnScreenShowAnimationCompleted();
        MapController.instance.CkeckLocationPermission();
        TrailsHandler.instance.MethodInvoke();
        TrailsHandler.instance.isInvokeNearestSculp = true;

    }

    public override void OnScreenHideCalled()
    {
        base.OnScreenHideCalled();
        MapController.instance.StopPermissionCheck();
        //if(UIController.instance.previousScreen == ScreenType.Poi)
        //{
        //    TrailsHandler.instance.isInvokeNearestSculp = false;
        //}
        poiPanelAnimate.HideAnimation();
        RemoveMapScean();
    }
    public override void OnBack()
    {
        base.OnBack();
        BackToTrailList();
    }
    public void BackToTrailList()
    {
        UIController.instance.ShowNextScreen(ScreenType.TrailList);
        TrailsHandler.instance.MethodCancleInvoke();
        //CancelInvoke("CheckSculpNearestMe");
        TrailsHandler.instance.isInvokeNearestSculp = false;
        Helper.Execute(this, () => OpenTab(false), 0.4f);
        currentSelectedPoiPin = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Zoom_offset = UIController.AspectRatio * -1.8f;
        OpenTab(false);
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }
    }

    async void SculptureTrailPoisCallBack(API_TYPE aPI_TYPE, string obj)
    {
        if (aPI_TYPE != API_TYPE.API_TRAIL_POIS) return;
        if (TrailsHandler.instance.CurrentTrail == null) return;
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(contentParent.GetChild(i).gameObject);
        }

        await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        foreach (var poi in ApiHandler.instance.data.trailPois)
        {
            if (poi.intro_id != TrailsHandler.instance.CurrentTrail.num) continue;
            GameObject go = Instantiate(trail_poi_Prefab, contentParent);
            PoiCeil Poi = go.GetComponent<PoiCeil>();
            Poi.SetTrail(poi);
        }
        await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        Refresh();
    }
    public void OnClickMap()
    {
        //Debug.Log("mapopen");
        if (!Services.CheckInternetConnection())
        {
            OpenTab(false);
            return;
        }
        Debug.Log("open map");
        OpenTab(true);
    }
    public void OpenTab(bool IsMap)
    {
        if (IsMap && !Services.CheckInternetConnection()) return;
        poiListContentObj.SetActive(!IsMap);
        poiMapContentObj.SetActive(IsMap);
        poiListToggle.isOn = !IsMap;
        poiMapToggle.isOn = IsMap;
        MapController.instance.MapInteractable(IsMap);
        if (IsMap) LoadMapScean();
        if (IsMap)
        {
            txtpoiTitle.text = "Trails";
        }
        else
        {
            txtpoiTitle.text = "Point of Interest";

        }
    }

    async void Refresh()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        Array.Reverse(contentSizeFitters);
        UIController.instance.RefreshContent(contentSizeFitters);
    }

    async void LoadMapScean()
    {
        //UIController.instance.getScreen(UIController.instance.getCurrentScreen()).isBackWorking = false;
        ToggleInteraction(false);
        if (TrailsHandler.instance.CurrentTrail == null)// (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.num))
        {
            Debug.Log("123456789 trail screen popup");
            UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.", "Ok");
            return;
        }
        List<Vector2> markers = new List<Vector2>();
        pois = ApiHandler.instance.data.trailPois.FindAll(x => (x.intro_id == TrailsHandler.instance.CurrentTrail.num));
        foreach (var poi in pois)
        {
            Debug.LogError("Add Marker : " + poi.Name);
            if (string.IsNullOrEmpty(poi.longitude) || string.IsNullOrEmpty(poi.latitude)) continue;
            markers.Add(new Vector2(float.Parse(poi.longitude), float.Parse(poi.latitude)));
        }
        await MapController.instance.LoadMapScean(onlineMapsRawImageTouchForwarder, markers.ToArray(), Zoom_offset, currentSelectedPoiPin);
        OnClickMarkerSetPoiDetails(currentSelectedPoiPin, true);
        poiPanelAnimate.ShowAnimation();
        //UIController.instance.getScreen(UIController.instance.getCurrentScreen()).isBackWorking = true;
        ToggleInteraction(true);
    }
    void RemoveMapScean()
    {
        MapController.instance.RemoveMapScean();
    }
    public void ResetZoom()
    {
        MapController.instance.ResetZoom(Zoom_offset);
    }
    public void OnClickMarkerSetPoiDetails(int index, bool forceUpdateChange)
    {
        Debug.Log("marker click");
        if (!forceUpdateChange)
        {
            if (currentSelectedPoiPin == index) return;
        }
        currentSelectedPoiPin = index;
        poiImg.Downloading(pois[index].num, pois[index].thumbnail);
        txtpoiName.text = pois[index].Name;
        txtpoiSubDesc.text = pois[index].short_desc;
    }

    public void OnClickShowPoiDetailsScreen()
    {
        TrailsHandler.instance.CurrentTrailPoi = pois[currentSelectedPoiPin];
        UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
    }
}
