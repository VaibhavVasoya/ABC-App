using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Transform spiner;

    private void OnDestroy()
    {
        SetNull();
    }

    private void Awake()
    {
        if (img == null)
            img = GetComponent<Image>();
        if (spiner == null)
            spiner = transform.GetChild(0);
    }

    public async void Downloading(string prefix,string url)
    {
        spiner.gameObject.SetActive(true);
        img.sprite = null;
        if (string.IsNullOrEmpty(url)) return;
        await Services.Download(prefix,url, (texture) =>
        {
            SetNull();
            img.sprite = Asset.GetSprite(texture);
        });
        spiner.gameObject.SetActive(false);
    }

    public void SetNull()
    {
        if (img.sprite != null) DestroyImmediate(img.sprite.texture);
    }
}
