using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Master.UIKit;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;
using UnityEngine.Networking;
using System;

public class MyImagePlayer : MonoBehaviour
{    
    public string imageFileUrl;

    public Material matriel360ForImage;

    public async void ApplyImageFileToSkybox()
    {
        await NewDownload(imageFileUrl, (a) => matriel360ForImage.mainTexture = a);       
    }

    //public static async Task Download(string url, Action<Texture2D> downloadedTexture = null)
    //{
    //    if (string.IsNullOrEmpty(url)) return;
        
    //    UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

    //    await www.SendWebRequest();
       
    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.LogError(www.url + " - " + www.error);
    //    }
    //    else
    //    {
    //        Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
    //        downloadedTexture?.Invoke(myTexture as Texture2D);           
    //    }
    //}
    
    private void Start()
    {      
        ApplyImageFileToSkybox(); 
    }

    public static async Task NewDownload(string url, Action<Texture2D> downloadedTexture = null)
    {
        if (string.IsNullOrEmpty(url)) return;
        string path = Application.persistentDataPath + "/";

        string imgFileName = Path.GetFileNameWithoutExtension(url);
        //Debug.Log("File Name : " + imgFileName);
        string imgFolder = path + GameData.ImagesDirectoryPath + "/";
        if (!Directory.Exists(imgFolder)) Directory.CreateDirectory(imgFolder);

        bool imgExists = false;

        if (File.Exists(imgFolder + imgFileName))
        {
            imgExists = true;
            url = "file://" + imgFolder + imgFileName;
        }
        else
        {
            Debug.Log("File NOT Found");
        }
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        await www.SendWebRequest();
        while (!www.isDone && www.error == null)
        {
            Events.DownloadProgress(www.downloadProgress);
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }

        Events.DownloadProgress(1);
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.url + " - " + www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            downloadedTexture?.Invoke(myTexture as Texture2D);

            if (!imgExists && www.downloadHandler.data != null)
            {
                File.WriteAllBytes(imgFolder + imgFileName, ((DownloadHandlerTexture)www.downloadHandler).texture.EncodeToPNG());// www.downloadHandler.data);
                //File.WriteAllBytes(Path.Combine(Application.persistentDataPath, imgFolder + imgFileName), www.downloadHandler.data);
                Debug.Log("Image saved");
            }
        }
    }
}
