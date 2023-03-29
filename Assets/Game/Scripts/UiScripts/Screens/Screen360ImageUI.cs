using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master.UIKit;
using System.Threading.Tasks;
using System;
using UnityEngine.Networking;
using System.IO;

public class Screen360ImageUI : UIScreenView
{
    //public Material matriel360ForImage;
    //public GameObject image360PlayerPrefab;
    //GameObject currentImage;

    //[SerializeField] Transform image360Parent;

    //Material panaMat;

    //public override void OnScreenShowCalled()
    //{
    //    base.OnScreenShowCalled();
    //    ApplyImageFileToSkybox();
    //}
    //public override void OnScreenHideCalled()
    //{
    //    base.OnScreenHideCalled();
    //    if (currentImage != null)
    //    {
    //        Destroy(currentImage);
    //    }

    //    if (matriel360ForImage.mainTexture != null)
    //    {
    //        DestroyImmediate(matriel360ForImage.mainTexture, true);
    //    }
    //}
    //public override void OnBack()
    //{
    //    base.OnBack();
    //    BackToLastScreen();
    //}
    //public async void BackToLastScreen()
    //{
    //    await Task.Delay(TimeSpan.FromSeconds(0.6f));
    //    UIController.instance.ShowNextScreen(UIController.instance.previousScreen);
    //}

    ////[SerializeField] Texture2D tex;
    //public async void ApplyImageFileToSkybox()
    //{
    //    Debug.Log("apply image file to skybox called");
    //    LoadingUI.instance.OnScreenShow();
    //    //matriel360ForImage.SetTexture("_MainTex", tex);
    //    //await Services.Download(TrailsHandler.instance.CurrentTrailPoi.image_360_file, (tex) => matriel360ForImage.SetTexture("_MainTex", tex));
    //    await Services.Download(TrailsHandler.instance.CurrentTrailPoi.num, TrailsHandler.instance.CurrentTrailPoi.image_360_file, (tex) => { if (matriel360ForImage.mainTexture != null) DestroyImmediate(matriel360ForImage.mainTexture, true); matriel360ForImage.SetTexture("_MainTex", tex); });



    //    //UnityWebRequest www = UnityWebRequestTexture.GetTexture(TrailsHandler.instance.CurrentTrailPoi.image_360_file);

    //    //await www.SendWebRequest();

    //    //Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //    //matriel360ForImage.SetTexture("_MainTex",((DownloadHandlerTexture)www.downloadHandler).texture);

    //    currentImage = Instantiate(image360PlayerPrefab, image360Parent);

    //    LoadingUI.instance.OnScreenHide();

    //}


}
