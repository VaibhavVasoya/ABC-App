using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master.UI;
using Master.UIKit;

public class ImagePage : MonoBehaviour
{
    [SerializeField] ImageLoader img;

    public void LoadImage(PoiImage poiImg)
    {
        img.Downloading(poiImg.num, poiImg.images);
    }
}
