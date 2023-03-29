using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselPage : MonoBehaviour
{
    [SerializeField] ImageLoader img;

    public void LoadImage(PoiImage poiImg)
    {
        img.Downloading(poiImg.num, poiImg.images);
    }
}
