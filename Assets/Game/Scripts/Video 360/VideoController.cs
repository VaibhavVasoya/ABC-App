using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public static VideoController inst;

    [SerializeField] GameObject VideoContnetObj;
    [SerializeField] VideoPlayer360 _myVideoPlayer;
    private void Awake()
    {
        inst = this;
    }
    
    void SpawnVideoPlayer()
    {
        _myVideoPlayer = Instantiate(VideoContnetObj,transform).GetComponent<VideoPlayer360>();
        
    }
    void RemoveVideoPlayer()
    {
        Destroy(_myVideoPlayer.gameObject);
        _myVideoPlayer = null;
    }
    public void DownloadAndLoadVideo(string prefix,string _url,string _audioUrl)
    {
        SpawnVideoPlayer();
        _myVideoPlayer.DownloadAndLoadVideo(prefix,_url, _audioUrl);
    }
   
    public void Stop360Video()
    {
        _myVideoPlayer.Stop360Video();
        RemoveVideoPlayer();
    }


}
