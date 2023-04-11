using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using System;
using Master.UIKit;
using Master.UI;
using Master;
using UnityEngine.SceneManagement;


public class MapController : Singleton<MapController>
{
    [SerializeField] AnimationCurve MovementAnimationCurve;
    [SerializeField] float lat;
    [SerializeField] float lang;
    [SerializeField] List<Vector2> markers;

    [SerializeField] GameObject LoadingObj;

    [SerializeField] Vector2 LatLong;
    /// <summary>
    /// Texture to be used if marker texture is not specified.
    /// </summary>
    public Texture2D currentLocationTexture;

    bool IsCheackLocation = true;

    public bool isAllowToMarkerClick = false;

    List<OnlineMapsMarker> mapsMarkers;
    List<OnlineMapsMarker> sculpMapsMarkers;
    OnlineMapsMarker my_marker;
    [SerializeField] float markerScale = 1.6f;
    // Start is called before the first frame update
    async void Start()
    {
        mapsMarkers = new List<OnlineMapsMarker>();
        sculpMapsMarkers = new List<OnlineMapsMarker>();

#if UNITY_EDITOR
        LatLong = new Vector2(-6.3160218f, 54.8566988f);
        return;
#endif
        //if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        //{
        //    Permission.RequestUserPermission(Permission.FineLocation);
        //    Permission.RequestUserPermission(Permission.CoarseLocation);
        //}
        //await Task.Delay(TimeSpan.FromSeconds(5f));
        
    }
    /// <summary>
    /// location permission
    /// </summary>
    public void OpenAppInfo()
    {
        try
        {
#if UNITY_ANDROID
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                string packageName = currentActivityObject.Call<string>("getPackageName");

                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
                {
                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
                    currentActivityObject.Call("startActivity", intentObject);
                }
            }
#endif
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    /// <summary>
    /// gps on 
    /// </summary>
    public void OpenLocationEnable()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaObject locationSettingsIntent = new AndroidJavaObject("android.content.Intent", "android.settings.LOCATION_SOURCE_SETTINGS");
        locationSettingsIntent.Call<AndroidJavaObject>("addFlags", 0x10000000);
        context.Call("startActivity", locationSettingsIntent);
#elif UNITY_IOS
        string url = iOSSettingsBinder.GetAppSettingsURL();
        Debug.Log("the settings url is:" + url);
        Application.OpenURL(url);
#endif
    }
    public void CkeckLocationPermission()
    {
        StartCoroutine("CheackLocationIsEnable");
    }

    private void OnDisable()
    {
        StopCoroutine("CheackLocationIsEnable");
    }
    IEnumerator CheackLocationIsEnable()
    {
        yield return new WaitForSeconds(1);
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Debug.Log("permissition ");
            //Input.location.Start();
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        yield return new WaitForSeconds(1);
        IsCheackLocation = true;
        //Application.OpenURL("intent:android.settings.APPLICATION_DETAILS_SETTINGS?package=com.tag.ABC#Intent;end;");
        while (true)
        {
            if (!Input.location.isEnabledByUser && IsCheackLocation)
            {
                IsCheackLocation = false;
                //UIController.instance.ShowPopupMsg("we can't find you...", "Your Location Services are turned off, Please turn on now.", () => { IsCheackLocation = true; });
                UIController.instance.ShowPopupMsg("Location", "By clicking on enable button , please turn on your location.","Enable",() => { IsCheackLocation = true;OpenAppInfo(); });
                //CallLocationService();
                
            }
            yield return new WaitForSeconds(5);
        }
    }

    bool IsMapActive()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Map")
                return true;
        }
        return false;
    }

    public async Task LoadMapScean(OnlineMapsRawImageTouchForwarder mapRawImg, Vector2 marker, float Zoom_offset = 0)
    {
        if (IsMapActive()) return;
        LoadingUI.instance.OnScreenShow();
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                asyncOperation.allowSceneActivation = true;
            }
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
        mapRawImg.Init();
        if (mapsMarkers != null)
            ClearMarker();
        //await Task.Delay(TimeSpan.FromSeconds(1));
        //await AddCurrentLocation();
        await Task.Delay(TimeSpan.FromSeconds(0.2f));
        AddMarker(marker.x, marker.y);
        await Task.Delay(TimeSpan.FromSeconds(1f));
        LoadingUI.instance.OnScreenHide();
        ResetZoom(Zoom_offset);
    }
    public async Task LoadMapScean(OnlineMapsRawImageTouchForwarder mapRawImg, Vector2[] _markers, float Zoom_offset = 0,int selectedPoi=0)
    {
        if (IsMapActive()) return;
        LoadingUI.instance.OnScreenShow();
        SelectedMarker = null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                asyncOperation.allowSceneActivation = true;
            }
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
        mapRawImg.Init();
        if (mapsMarkers != null)
            ClearMarker();
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        foreach (var marker in _markers)
        {
            AddMarkerWithClickEvent(marker.x, marker.y);
            await Task.Delay(TimeSpan.FromSeconds(0.5f));
        }
        if (mapsMarkers.Count > 0)
        {
            mapsMarkers[selectedPoi].scale = 1;
            SelectedMarker = mapsMarkers[selectedPoi];
        }
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        LoadingUI.instance.OnScreenHide();
        ResetZoom(Zoom_offset);
    }
    public async void RemoveMapScean()
    {
        if (!IsMapActive()) return;
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync("Map");
        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
        Debug.Log("Map Unload Successfuly.");
        Resources.UnloadUnusedAssets();       
        GC.Collect();
    }
    public void AddMarker(float _lang, float _lat)
    {
        if (OnlineMapsMarkerManager.instance == null) return;
        my_marker = OnlineMapsMarkerManager.instance.Create(_lang, _lat);
        //if (UIController.instance.getCurrentScreen() == ScreenType.PhysicalPoiMap)
            my_marker.scale = my_marker.scale / markerScale;
        markers.Add(new Vector2(_lang, _lat));
        ResetViewForMarkers(markers.ToArray());
    }
    public void AddMarkerWithClickEvent(float _lang, float _lat)
    {
        if (OnlineMapsMarkerManager.instance == null) return;
        my_marker = OnlineMapsMarkerManager.instance.Create(_lang, _lat);
        if (UIController.instance.getCurrentScreen() == ScreenType.Poi)
            my_marker.scale = my_marker.scale / markerScale;
        my_marker.OnClick += OnMarkerClick;
        mapsMarkers.Add(my_marker);
        markers.Add(new Vector2(_lang, _lat));
        ResetViewForMarkers(markers.ToArray());
    }
    OnlineMapsMarkerBase SelectedMarker;
    public void OnMarkerClick(OnlineMapsMarkerBase Marker)
    {
        if (SelectedMarker != null) SelectedMarker.scale = SelectedMarker.scale / markerScale;
        Debug.LogError("Trail pin Onclick Recived : " + Marker.position);
        if (!isAllowToMarkerClick)
        {
            Debug.LogError("Trail pin Onclick Recived, But this screen not allowed to marker click.");
            return;
        }
        
        for (int i = 0; i < mapsMarkers.Count; i++)
        {
            if (Marker == mapsMarkers[i])
            {
                Marker.scale = 1;
                SelectedMarker = Marker;
                UIController.instance.getScreen(ScreenType.Poi).GetComponent<ScreenTrailPois>().OnClickMarkerSetPoiDetails(i);
                break;
            }
        }
    }
    public async Task AddCurrentLocation()
    {
        //LoadingObj.SetActive(true);
#if !UNITY_EDITOR
        LatLong = await GetCurrentLocation();
#endif
        if (LatLong == Vector2.zero) return;
        //LoadingObj.SetActive(false);
        Debug.Log("Curent Location : " + Input.location.lastData.latitude + " : " + Input.location.lastData.longitude);

        if (OnlineMapsMarkerManager.instance == null) return;

        my_marker = OnlineMapsMarkerManager.instance.Create(LatLong.x, LatLong.y, currentLocationTexture);

        //if (UIController.instance.getCurrentScreen() == ScreenType.PhysicalPoiMap)
            my_marker.scale = my_marker.scale / markerScale;

        markers.Add(LatLong);
        ResetZoom();

    }
    public void ResetZoom()
    {
        if (OnlineMapsMarkerManager.instance == null) return;
        ResetViewForMarkers(markers.ToArray());
    }
    public void ResetZoom(float Zoom_offset)
    {
        if (OnlineMapsMarkerManager.instance == null) return;
        ResetViewForMarkers(markers.ToArray(), Zoom_offset, (Zoom_offset != 0));
    }
    public void ResetViewForMarkers(Vector2[] pointsToBeViewed, float Zoom_offset = 0, bool offset = false, float speed = 7f)
    {
        Vector2 Pos;
        int ZoomVal;

        OnlineMapsUtils.GetCenterPointAndZoom(pointsToBeViewed, out Pos, out ZoomVal);
        StopAllCoroutines();
        StartCoroutine(SetPositionAndZoom(Pos, ZoomVal + Zoom_offset, offset));
    }
    public void ClearMarker()
    {
        mapsMarkers.Clear();
        sculpMapsMarkers.Clear();
        markers.Clear();
    }
    IEnumerator SetPositionAndZoom(Vector2 Pos, float ZoomVal, bool offset)
    {
        double map_Lat, map_lng;
        OnlineMaps.instance.GetPosition(out map_Lat, out map_lng);

        //if (!offset)
        //{
        //    Pos.x -= Mathf.Pow(2, (OnlineMaps.MAXZOOM - ZoomVal)) * 0.00025f;
        //}

        float InitalZoom = OnlineMaps.instance.floatZoom;

        float timeToMove = 1.5f;
        float currenttime = 0;
        float Progressval;

        while (currenttime < timeToMove)
        {

            currenttime += Time.smoothDeltaTime;

            Progressval = MovementAnimationCurve.Evaluate(currenttime / timeToMove);


            map_Lat = Lerp(map_Lat, Pos.x, Progressval);
            map_lng = Lerp(map_lng, Pos.y, Progressval);
            OnlineMaps.instance.SetPosition(map_Lat, map_lng);

            OnlineMaps.instance.floatZoom = Mathf.Lerp(InitalZoom, ZoomVal
                , Progressval);

            yield return true;
        }
    }
    public static double Lerp(double from, double to, double t)
    {
        return from + (to - from) * Clamp01(t);
    }
    public static double Clamp01(double value)
    {
        if (value < 0.0)
            return 0.0d;
        if (value > 1.0)
            return 1d;
        else
            return value;
    }
    public async Task<Vector2> GetCurrentLocation()
    {
#if UNITY_EDITOR
        return LatLong;
#endif
        //changed
        //if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        //{
        //    Permission.RequestUserPermission(Permission.FineLocation);
        //    Permission.RequestUserPermission(Permission.CoarseLocation);
        //}
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            //UIController.instance.ShowPopupMsg("we can't find you...", "Your Location Services are turned off, Please turn on now.");
            while (!Input.location.isEnabledByUser)
                await Task.Delay(TimeSpan.FromSeconds(1f));
        }

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            UIController.instance.ShowPopupMsg("Oops!!", "Timed out", "Ok");
            print("Timed out");
            return Vector2.zero;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine device location", "Ok");
            print("Unable to determine device location");
            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();
            return Vector2.zero;
        }
        else
        {
            Vector2 latLong = new Vector2(Input.location.lastData.longitude, Input.location.lastData.latitude);
            // Access granted and location value could be retrieved
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();
            return latLong;
        }
    }

    public async void OpenMapViaLink(string destinationLong, string destinationLat)
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS IS DISABLED");
            UIController.instance.ShowPopupMsg("Location", "By clicking on enable button , please turn on your location.","Enable", () => { IsCheackLocation = true; OpenLocationEnable(); });
            return;
        }

            LoadingUI.instance.OnScreenShow();
        Vector2 currentLoca = await MapController.instance.GetCurrentLocation();
        string tempLatitude = currentLoca.y.ToString();
        string tempLongitude = currentLoca.x.ToString();

        var url = "https://www.google.com/maps/dir/?api=1";
        var origin = "&origin=" + tempLatitude + "," + tempLongitude;
        var destination = "&destination=" + destinationLat + "," + destinationLong;
        var newUrl = (url + origin + destination);
        Debug.LogError(newUrl);
        LoadingUI.instance.OnScreenHide();
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        Application.OpenURL(newUrl);
    }
    public void MapInteractable(bool isInteracte)
    {
        isAllowToMarkerClick = isInteracte;
        if (OnlineMapsTileSetControl.instance != null)
            OnlineMapsTileSetControl.instance.allowUserControl = isInteracte;
    }
}
