using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Master;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using Master.UIKit;
public class KVPList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
{
    public void Add(TKey Key, TValue Value)
    {
        base.Add(new KeyValuePair<TKey, TValue>(Key, Value));
    }
}

public class APIRequest
{
    public METHOD RequestMethod;
    public string URL;
    public KVPList<string, string> data;
    public KVPList<string, byte[]> rawdata;
    public Action<string> OnServiceCallBack;
    public bool withTimeOut = false;

    public APIRequest(METHOD requestMethod, string requestURL, Action<string> requestCallBack, KVPList<string, string> requestData = null, KVPList<string, byte[]> requestRawdata = null, bool requestwithTimeOut = false)
    {
        RequestMethod = requestMethod;
        URL = requestURL;
        OnServiceCallBack = requestCallBack;
        data = requestData;
        rawdata = requestRawdata;
        withTimeOut = requestwithTimeOut;
    }
}

public class RequestScheduler
{
    List<APIRequest> RequestQueue;
    public bool isRequestActive;

    bool ActiveScheduler;

    public RequestScheduler()
    {
        RequestQueue = new List<APIRequest>();

        ActiveScheduler = true;
        RequestsHandler();
    }

    public void KillRequestSchedular()
    {
        ActiveScheduler = false;
    }

    public bool IsRequeastPresentInQueue(string URL)
    {
        foreach (var req in RequestQueue)
        {
            if (req.URL.Contains(URL))
            {
                return true;
            }
        }
        return false;
    }

    public async void RequestsHandler()
    {
        do
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (RequestQueue.Count > 0 && !isRequestActive)
                {
                    SendRequest(RequestQueue[0]);
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(2));
        } while (ActiveScheduler);
    }

    public void SendRequest(APIRequest request)
    {
        Debug.Log("<color='magenta'>Sending Request " + request.URL + "</color>");

        isRequestActive = true;

        switch (request.RequestMethod)
        {
            case METHOD.GET:

                Services.Get(request.URL, (response) =>
                {
                    RequestQueue.RemoveAt(0);
                    isRequestActive = false;
                    Debug.Log("<color='magenta'>Completed Request : " + request.URL + "</color>");
                    if (Services.IsValidJson(response))
                    {
                        request.OnServiceCallBack(response);
                    }
                    else
                    {
                        AddRequest(request);
                    }
                }, request.withTimeOut, false, true);

                break;
            case METHOD.POST:
                {
                    Services.Post(request.URL, request.data, (response) =>
                    {
                        RequestQueue.RemoveAt(0);
                        isRequestActive = false;
                        Debug.Log("<color='magenta'>Completed Request : " + request.URL + "</color>");
                        if (Services.IsValidJson(response))
                        {
                            request.OnServiceCallBack(response);
                        }
                        else
                        {
                            AddRequest(request);
                        }
                    }, false, true);
                    break;
                }
        }
    }

    public void AddRequest(APIRequest request)
    {
        APIRequest lookreq = RequestQueue.Find(x => x.URL.Equals(request.URL));

        if (lookreq != null)
        {
            RequestQueue.Remove(lookreq);
            Debug.Log("<color='magenta'>Removed Request : " + request.URL + "</color>");
        }
        RequestQueue.Add(request);
        Debug.Log("<color='magenta'>Added Request : " + request.URL + "</color>");
        Debug.Log("<color='green'>Queue count : " + RequestQueue.Count + "</color>");
    }
}

public enum METHOD
{
    GET,
    POST,
    POST_RAW,
    POST_MIX,
}

public class Services : Singleton<Services>
{
    public static RequestScheduler APIRequestScheduler;
    static bool isSchedulerImplemented = true;
    static string pathAV;
    private void Awake()//Start()
    {
        pathAV = Application.persistentDataPath + "/AudioVideos/";
        APIRequestScheduler = new RequestScheduler();
        CreateFolder(GameData.VIDEO_DOWNLOAD_FOLDER);
        CreateFolder(GameData.VIDEO_FOLDER);
        Debug.Log("<color='cyan'>" + NewAppDir() + "</color>");
    }

    public string GetFileFrom(string folderName, string fileName)
    {
        return Path.Combine(NewGetDir(folderName), fileName);
    }

    public void CreateFolder(string name)
    {
        if (!Directory.Exists(NewGetDir(name)))
        {
            Directory.CreateDirectory(NewGetDir(name));
        }
    }

    public string NewGetDir(string name)
    {
        return Path.Combine(NewAppDir(), name);
    }

    public string NewAppDir()
    {
        return Path.Combine(GetPersistentDir(), "AppData");
    }

    public string GetPersistentDir()
    {
        return Application.isEditor & Application.platform == RuntimePlatform.IPhonePlayer ? "file://" + Application.persistentDataPath + "/" : Application.persistentDataPath;
    }
    private void OnDestroy()
    {
        APIRequestScheduler.KillRequestSchedular();
    }

    #region POST METHOD

    public static async void Post(string postURL, KVPList<string, string> data, Action<string> OnServiceCallBack,
         bool isToBeAddedToScheduler = true, bool isFromScheduler = false)
    {
        if (isToBeAddedToScheduler && isSchedulerImplemented)
        {
            APIRequestScheduler.AddRequest(new APIRequest(METHOD.POST, postURL, OnServiceCallBack, data, null));
            return;
        }
        Debug.Log("<color='blue'> URL :: " + postURL + "</color>");
        WWWForm formData = new WWWForm();
        string m_body = "";
        for (int count = 0; count < data.Count; count++)
        {
            m_body = m_body + data[count].Key + ":" + data[count].Value + "\n";
            formData.AddField(data[count].Key, data[count].Value.ToString());
        }
        Debug.Log("Body " + "\n" + m_body);
        UnityWebRequest request = UnityWebRequest.Post(postURL, formData);
        request.timeout = 10;
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success && IsValidJson(request.downloadHandler.text))
        {
            Debug.Log("<color=green>" + request.downloadHandler.text + "</color>");
            OnServiceCallBack(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("API ERROR : " + request.error);

            if (isFromScheduler)
            {
                OnServiceCallBack(request.downloadHandler.text);
            }
        }
    }
    #endregion

    #region GET METHOD

    public static async void Get(string getURL, Action<string> OnServiceCallBack,
       bool withTimeOut = false, bool isToBeAddedToScheduler = true, bool isFromScheduler = false, bool isOnlyEnglish = false)
    {
        getURL = GameData.GetUrl(getURL);
        //Debug.Log("getUrl 1 : "+getURL);
        if (!getURL.Contains("lang_id") && !isOnlyEnglish)
        {
            //Debug.Log("getUrl 2 : " + getURL);
            getURL += (getURL.Contains("&")) ? "lang_id=" : "?lang_id=" + ApiHandler.instance.currentLanguage.num;
            //Debug.Log("getUrl 3 : " + getURL);
        }
        if (isToBeAddedToScheduler && isSchedulerImplemented)
        {
            APIRequestScheduler.AddRequest(new APIRequest(METHOD.GET, getURL, OnServiceCallBack, null, null, withTimeOut));
            return;
        }
        Debug.Log("123url = " + getURL);
        UnityWebRequest request = UnityWebRequest.Get(getURL);

        if (withTimeOut)
        {
            request.timeout = 5;
        }
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success && IsValidJson(request.downloadHandler.text))
        {
            Debug.Log("<color=green>" + request.downloadHandler.text + "</color>");
            OnServiceCallBack(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(getURL + " API ERROR : " + request.downloadHandler.text);

            if (isFromScheduler)
            {
                OnServiceCallBack(request.downloadHandler.text);
            }
        }
    }
    #endregion

    #region IMAGE_DOWNLOAD
    public static async Task Download(string prefix, string url, Action<Texture2D> downloadedTexture = null, Action<float> progress = null)
    {
        if (string.IsNullOrEmpty(url)) return;
        string imgFileName = prefix + "_" + Path.GetFileNameWithoutExtension(url);
        string imgFolder = Application.persistentDataPath + "/" + GameData.ImagesDirectoryPath + "/";
        if (!Directory.Exists(imgFolder)) Directory.CreateDirectory(imgFolder);

        bool imgExists = false;

        if (File.Exists(imgFolder + imgFileName))
        {
            imgExists = true;
            url = "file://" + imgFolder + imgFileName;
        }

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        await www.SendWebRequest();
        progress?.Invoke(1);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.url + " - " + www.error);
        }
        else
        {
            downloadedTexture?.Invoke(((DownloadHandlerTexture)www.downloadHandler).texture);
            if (!imgExists && www.downloadHandler.data != null)
            {
                File.WriteAllBytes(imgFolder + imgFileName, ((DownloadHandlerTexture)www.downloadHandler).texture.EncodeToPNG());// www.downloadHandler.data);
            }
        }
    }
    #endregion

    #region MixedData

    public static async void PostMixedData(string PostURL, KVPList<string, string> data, KVPList<string, byte[]> rawdata,
        Action<string> OnServiceCallBack,
        bool shouldAuthorize = true)
    {

        Debug.Log("<color='blue'> URL :: " + PostURL + "</color>");


        WWWForm formData = new WWWForm();

        string m_body = "";

        for (int count = 0; count < data.Count; count++)
        {
            m_body = m_body + data[count].Key + ":" + data[count].Value + "\n";
            formData.AddField(data[count].Key, data[count].Value.ToString());
        }

        for (int count = 0; count < rawdata.Count; count++)
        {
            m_body = m_body + rawdata[count].Key + ":" + rawdata[count].Value + "\n";
            formData.AddBinaryData(rawdata[count].Key, rawdata[count].Value);
        }

        Debug.Log("Body " + "\n" + m_body);

        UnityWebRequest request = UnityWebRequest.Post(PostURL, formData);

        request.timeout = 10;

        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success && IsValidJson(request.downloadHandler.text))
        {
            Debug.Log("<color=green>" + request.downloadHandler.text + "</color>");
            OnServiceCallBack(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("API ERROR : " + request.downloadHandler.text);

        }
    }

    #endregion


    #region Utilities

    public static bool IsValidJson(string strInput)
    {
        strInput = strInput.Trim();
        if (!string.IsNullOrEmpty(strInput))
        {
            try
            {
                var obj = JSON.Parse(strInput);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Video_Audio_Download
    public static bool isStop;
    public static bool isDownloding;
    public static async Task DownloadVideoAndAudio(string prefix, string videoURL, string audioURL, bool isShowLoading, Action<string, AudioClip> callback)
    {
        isStop = false;
        string localURL = null;
        AudioClip clip = null;
        if (isShowLoading) LoadingUI.instance.OnScreenShow();
        await DownloadVideo(prefix, videoURL, (_localUrl) => { localURL = _localUrl; });
        await DownloadAudio(prefix, audioURL, (_clip) => { clip = _clip; });
        if (isShowLoading) LoadingUI.instance.OnScreenHide();
        callback(localURL, clip);
    }
    public static async Task DownloadVideo(string prefix, string url, Action<string> localUrl, Action<float> progress = null)
    {
        if (string.IsNullOrEmpty(url)) return;
        if (isStop) return;
        if (string.IsNullOrEmpty(Path.GetExtension(url))) return;
        string VideoFileName = prefix + "_" + Path.GetFileName(url);
        if (!Directory.Exists(pathAV))
            Directory.CreateDirectory(pathAV);

        bool videoExists = false;
        //"file://" +
        await CheckForUpdateTheVideo(url, pathAV + VideoFileName);

        if (File.Exists(pathAV + VideoFileName))
        {
            //Debug.Log("File Found");
            videoExists = true;

            url = pathAV + VideoFileName;

            localUrl(url);
            return;
        }
        else
        {
            //Debug.Log("File NOT Found");
        }
        isDownloding = true;
        UnityWebRequest www = UnityWebRequest.Get(url);

        www.SendWebRequest();

        while (!www.isDone && www.error == null && !isStop)
        {
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
        if (isStop) www.Abort();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Events.DownloadFailed();
            Debug.LogError(www.url + " - " + www.error);
            localUrl(url);
        }

        if (!videoExists && www.downloadHandler.data != null && !isStop)
        {
            File.WriteAllBytes(pathAV + VideoFileName, www.downloadHandler.data);
            await Task.Delay(TimeSpan.FromSeconds(0.4f));
            url = pathAV + VideoFileName;
            localUrl(url);// (videoFolder + VideoFileName);
        }
        www.Dispose();
        progress(1);
        isDownloding = false;
    }
    public static async Task DownloadAudio(string prefix, string url, Action<AudioClip> audioClip, Action<float> progress = null)
    {
        if (isStop) return;
        if (string.IsNullOrEmpty(url)) return;
        if (string.IsNullOrEmpty(Path.GetExtension(url))) return;
        isDownloding = true;
        string filename = prefix + "_" + Path.GetFileNameWithoutExtension(url);
        if (!Directory.Exists(pathAV))
            Directory.CreateDirectory(pathAV);

        await CheckForUpdateTheVideo(url, "file://" + pathAV + filename);

        bool audioExists = false;

        if (File.Exists(pathAV + filename))
        {
            Debug.Log("Audio File Found.");
            audioExists = true;
            url = "file://" + pathAV + filename;
        }
        else
        {
            Debug.Log("Audio File NOT Found.");
        }
        //if(!audioExists) UIController.instance.ShowDownlodingPopup("360 Audio");
        UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);

        ((DownloadHandlerAudioClip)webRequest.downloadHandler).streamAudio = false;

        webRequest.SendWebRequest();
        while (!webRequest.isDone && webRequest.error == null && !isStop)
        {
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
        progress?.Invoke(1);
        //progress(1);
        if (!audioExists) await Task.Delay(TimeSpan.FromSeconds(0.8f));
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Events.DownloadFailed();
            Debug.LogError(webRequest.error);
            isDownloding = false;
            return;
        }
        else
        {
            var clip = ((DownloadHandlerAudioClip)webRequest.downloadHandler).audioClip;
            audioClip(clip);
        }

        if (!audioExists && webRequest.downloadHandler.data != null && !isStop)
        {
            File.WriteAllBytes(pathAV + filename, webRequest.downloadHandler.data);
        }
        webRequest.Dispose();
        isDownloding = false;
    }
    static async Task CheckForUpdateTheVideo(string url, string localUrl)
    {
        if (!File.Exists(localUrl)) return;

        var headRequest = UnityWebRequest.Head(url);

        await headRequest.SendWebRequest();

        var totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));

        var stream = new FileStream(localUrl, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        var fileLength = stream.Length;

        stream.Close();
        stream.Dispose();

        if (fileLength != totalLength)
        {
            //Debug.Log("Updated Video File Found.");
            if (File.Exists(localUrl))
            {
                Debug.Log("Delete File From : " + localUrl);
                File.Delete(localUrl);
            }
        }
    }
    public static bool FileExist(string prefix, string url, bool isVideoFile)
    {
        if (url.Length < 5) return true;
        return (File.Exists(pathAV + prefix + "_" + ((isVideoFile) ? Path.GetFileName(url) : Path.GetFileNameWithoutExtension(url))));
    }
    public static bool ImageFileExist(string prefix, string url)
    {
        if (url.Length < 5) return true;
        return (File.Exists(Application.persistentDataPath + "/" + GameData.ImagesDirectoryPath + "/" + prefix + "_" + Path.GetFileNameWithoutExtension(url)));
    }
    #endregion

    #region multi downloading
    static int numberOfBundle;
    public static async void LoadBundlesParellel(List<ItemType> downloadFiles)
    {
        if (downloadFiles.Count == 0)
        {
            numberOfBundle = 0;
        }
        else if (downloadFiles.Count < totalBundle)
        {
            numberOfBundle = 1;
        }
        else
        {
            numberOfBundle = downloadFiles.Count / totalBundle;
        }
        //Debug.Log("count fhusj;hfjahsfj " + downloadFiles.Count);
        var totalBlocks = downloadFiles.Count / numberOfBundle;
        int bundlesInLastBlock = downloadFiles.Count % numberOfBundle;
        if (bundlesInLastBlock != 0)
            totalBlocks++;
        else
            bundlesInLastBlock = numberOfBundle;
        List<Task> allBlockTasks = new List<Task>();
        int totalLoadedBundles = 0;
        for (int index = 0; index < totalBlocks; index++)
        {
            int bundlesInThisBlock = numberOfBundle;
            if (index == totalBlocks - 1)
            {
                bundlesInThisBlock = bundlesInLastBlock;
            }
            allBlockTasks.Add(LoadBundlesBlock(downloadFiles.GetRange(index * numberOfBundle, bundlesInThisBlock), (bundleLoadCount) =>
            {
                totalLoadedBundles++;
                Events.DownloadProgress(totalLoadedBundles / (float)downloadFiles.Count);
            }));
        }
        allBlockTasks.FindAll(x => x.IsCompleted);

        await Task.WhenAll(allBlockTasks);
        Debug.Log("All Bundles Loaded");
    }
    static async Task LoadBundlesBlock(List<ItemType> bundlesList, Action<float> onEachBundleLoaded)
    {
        Debug.Log("count " + bundlesList.Count);

        for (int i = 0; i < bundlesList.Count; i++)
        {
            //Debug.Log("log" + i);
            if (bundlesList[i].fileType == FileType.IMAGE)
            {
                await Download(bundlesList[i].prefix, bundlesList[i].url, null, (val) => onEachBundleLoaded(val));
            }
            else if (bundlesList[i].fileType == FileType.AUDIO)
            {
                await DownloadAudio(bundlesList[i].prefix, bundlesList[i].url, (clip) =>
                {
                    //Debug.Log("<color=green>" + bundlesList[i].prefix + " : audio Download completed.</color>");
                }, (val) => onEachBundleLoaded(val));
            }
            else if (bundlesList[i].fileType == FileType.VIDEO || bundlesList[i].fileType == FileType.VIDEO360)
            {
                await DownloadVideo(bundlesList[i].prefix, bundlesList[i].url, (local) =>
                {
                    //Debug.Log("<color=green>" + bundlesList[i].prefix + " : Transparent video Download completed.</color>");
                }, (val) => onEachBundleLoaded(val));
            }
        }
    }



    #region Data Download Size

    public static int totalDataSize;
    static int numberOfBundles;
    public static long totalSizeInByte = 0;
    //static AndroidJavaClass runtimeClass = new AndroidJavaClass("java.lang.Runtime");
    //static AndroidJavaObject runtime = runtimeClass.CallStatic<AndroidJavaObject>("getRuntime");
    public static int totalBundle = 3;

    public static async Task GetDataSize(List<ItemType> downloadFiles)
    {
        Debug.Log("123 available thread = "+totalBundle);
        if (downloadFiles.Count == 0)
        {
            numberOfBundles = 0;
        }
        else if (downloadFiles.Count < totalBundle)
        {
            numberOfBundles = 1;
        }
        else
        {
            numberOfBundles = downloadFiles.Count / totalBundle;
        }
        //Debug.Log("count fhusj;hfjahsfj " + downloadFiles.Count);
        var totalBlocks = downloadFiles.Count / numberOfBundles;
        int bundlesInLastBlock = downloadFiles.Count % numberOfBundles;
        if (bundlesInLastBlock != 0)
            totalBlocks++;
        else
            bundlesInLastBlock = numberOfBundles;
        List<Task> allBlockTasks = new List<Task>();
        int totalLoadedDataSize = 0;
        for (int index = 0; index < totalBlocks; index++)
        {
            int bundlesInThisBlock = numberOfBundles;
            if (index == totalBlocks - 1)
            {
                bundlesInThisBlock = bundlesInLastBlock;
            }
            allBlockTasks.Add(GetSize(downloadFiles.GetRange(index * numberOfBundle, bundlesInThisBlock), (value) =>
            {
                totalSizeInByte += value;
            }));
        }
        allBlockTasks.FindAll(x => x.IsCompleted);
        await Task.WhenAll(allBlockTasks);
        totalDataSize = (int)totalSizeInByte / 1000000;
        Debug.LogError("Total Size in Byte: " + (totalSizeInByte));
        Debug.LogError("Total Size in MB: " + (totalSizeInByte) / 1000000);
    }

    public static async Task GetSize(List<ItemType> bundlesList, Action<long> filesize)
    {
        for (int i = 0; i < bundlesList.Count; i++)
        {
            filesize(await GetFileSize(bundlesList[i].url));

        }
    }

    public static async Task<long> GetFileSize(string url)
    {
        // if (!File.Exists(url)) return 0;
        var headRequest = UnityWebRequest.Head(url);

        await headRequest.SendWebRequest();
        if (headRequest.result != UnityWebRequest.Result.Success) return 0;
        string length = headRequest.GetResponseHeader("Content-Length");
        Debug.Log(Path.GetFileNameWithoutExtension(url) + "  File Legth : " + length);
        if (string.IsNullOrEmpty(length)) return 0;
        return long.Parse(length);
    }

    static double ConvertBytesToMegabytes(long bytes)
    {
        return bytes / Math.Pow(1024, 2);
    }

    #endregion

    public static bool CheckInternetConnection()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable) return true;
        UIController.instance.ShowPopupMsg("Internet Connection", "Please check your connection and try again!!", "Ok");
        return false;
    }
}

#endregion

