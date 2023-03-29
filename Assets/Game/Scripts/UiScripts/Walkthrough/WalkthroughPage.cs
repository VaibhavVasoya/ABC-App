using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalkthroughPage : MonoBehaviour
{
    [SerializeField] ImageLoader img;
    [SerializeField] Text title;
    [SerializeField] Text description;

    public void SetData(WalkThrough _walkThrough)
    {
        title.text = _walkThrough.title;
        if (description) description.text = _walkThrough.description;
        img.Downloading(_walkThrough.num, _walkThrough.image);
    }
}
