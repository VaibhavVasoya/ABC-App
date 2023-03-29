using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static event Action<API_TYPE, string> WebRequestCompleted = delegate { };

    public static event Action<float> OnDownlodProgress;
    public static event Action OnDownlodFailed;

    public static event Action<string,bool> OnNearestSculpture;

    public static event Action OnLanguageChanged;

    public static void OnWebRequestComplete(API_TYPE aPI_TYPE,string obj)
    {
        if (WebRequestCompleted != null)
            WebRequestCompleted(aPI_TYPE,obj);
    }

    public static void DownloadProgress(float progress)
    {
        if (OnDownlodProgress != null)
            OnDownlodProgress(progress);
    }

    public static void DownloadFailed()
    {
        if (OnDownlodFailed != null)
            OnDownlodFailed();
    }

    public static void NearestSculpture(string sculpId,bool isNearest)
    {
        if (OnNearestSculpture != null)
            OnNearestSculpture(sculpId,isNearest);
    }

    public static void LanguageChanged()
    {
        if (OnLanguageChanged != null)
            OnLanguageChanged();
    }
}
