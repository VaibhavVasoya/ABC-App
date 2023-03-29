using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;
using Master.UI;

public class TrailCatCeil : MonoBehaviour
{
    public TrailCat trailCat;

    [SerializeField] ImageLoader bg;
    [SerializeField] Text txtname;
    [SerializeField] Text subTitle;


    public void SetTrailCat(TrailCat trail_Cat)
    {
        trailCat = trail_Cat;
        txtname.text = trail_Cat.title;
        subTitle.text = trail_Cat.sub_title;
        bg.Downloading(trail_Cat.num, trail_Cat.image);
    }

    public void OnClickTrail()
    {
        TrailsHandler.instance.currentTrailCat = trailCat;
        UIController.instance.ShowNextScreen(ScreenType.TrailList);
    }

        //void LoadPoi(ScreenType poiname)
        //{
        //    if (poiCount == 0)
        //    {
        //        UIController.instance.ShowPopupMsg("oops", "PoiNotFound");
        //        return;
        //    }
        //    TrailsHandler.instance.currentTrailCat = trailCat;
        //    ApiHandler.instance.GetTrailPois();
        //    //        Debug.Log("Current Trail : "+trail.category_id);
        //    UIController.instance.ShowNextScreen(poiname);
        //}
    }
