using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTrailsCat : UIScreenView
{
    [SerializeField] GameObject trail_Prefab;
    [SerializeField] Transform contentParent;
    [SerializeField] ScrollRect scrollRect;

    ContentSizeFitter[] contentSizeFitters;
    private void OnEnable()
    {
        Events.WebRequestCompleted += TrailCatsCallBack;
    }
    private void OnDisable()
    {
        Events.WebRequestCompleted -= TrailCatsCallBack;
    }
    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        TrailsHandler.instance.currentTrailCat = null;
        Refresh();
    }
   
    // Start is called before the first frame update
    void Start()
    {
        ApiHandler.instance.GetTrailCat(false);
    }

    void TrailCatsCallBack(API_TYPE aPI_TYPE, string obj)
    {
        if (aPI_TYPE != API_TYPE.API_TRAIL_CAT) return;
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(contentParent.GetChild(i).gameObject);
        }

        foreach (var trailCat in ApiHandler.instance.data.trailCats)
        {
            GameObject go = Instantiate(trail_Prefab, contentParent);
            TrailCatCeil trailCatCeil = go.GetComponent<TrailCatCeil>();
            trailCatCeil.SetTrailCat(trailCat);
        }
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        Array.Reverse(contentSizeFitters);
        Refresh();
    }

    async void Refresh()
    {
        await UIController.instance.RefreshContent(contentSizeFitters);
        scrollRect.verticalNormalizedPosition = 1f;
    }
}

