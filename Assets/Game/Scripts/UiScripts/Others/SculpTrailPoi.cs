using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class SculpTrailPoi : MonoBehaviour
{
    public Poi poi;

    [SerializeField] ImageLoader bg;
    [SerializeField] Text txtname;
    [SerializeField] Text txtShortDesc;
    
    public void SetTrail(Poi _poi)
    {
        //Debug.Log(_poi.Name + " Init.");
        poi = _poi;
        if (string.IsNullOrEmpty(_poi.Name)) txtname.gameObject.SetActive(false);
        txtname.text = _poi.Name;
        if (string.IsNullOrEmpty(_poi.short_desc)) txtShortDesc.gameObject.SetActive(false);
        txtShortDesc.text = _poi.short_desc;
        
        bg.Downloading(_poi.num, _poi.thumbnail);
    }

}
