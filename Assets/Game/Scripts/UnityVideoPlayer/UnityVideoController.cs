using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.Video;
//using LightShaft.Scripts;
using System;

public class UnityVideoController : MonoBehaviour
{
    //[SerializeField] GameObject youtubePlayerPrefab;
    //[SerializeField] GameObject unityPlayerPrefab;
    //[SerializeField] Transform playerParent;
    //[SerializeField] GameObject playerRoot;
    //GameObject playerObj;
    //public async void LoadPlayer(string prefix,bool isYoutubePlayer,string url)
    //{
    //    ToggleVideoParent(true);
    //    playerRoot.SetActive(true);
    //    playerObj = Instantiate((isYoutubePlayer)?youtubePlayerPrefab:unityPlayerPrefab , playerParent);
    //    playerObj.transform.localScale = Vector3.one;
    //    await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(0.5f));
    //    if (isYoutubePlayer)
    //        playerObj.transform.GetChild(0).GetChild(0).GetComponent<YoutubePlayer>().PreLoadVideo(url);
    //    else
    //        playerObj.transform.GetChild(0).GetComponent<Unity.VideoHelper.VideoController>().DownloadAndLoad(prefix,url);

    //}

    //public void ToggleVideoParent(bool val)
    //{
    //    playerRoot.gameObject.SetActive(val);
    //}

    //public void RemovePlayer()
    //{
    //    if (playerObj)
    //    {
    //        Destroy(playerObj);
    //        playerRoot.SetActive(false);
    //    }
    //}

}
