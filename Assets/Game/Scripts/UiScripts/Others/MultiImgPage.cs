using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiImgPage : MonoBehaviour
{
    [SerializeField] ImageLoader img;

    public void LoadImage(MultiImage mimg)
    {
        img.Downloading(mimg.num, mimg.image);
    }
}
