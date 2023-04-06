using UnityEngine;

public static class GameData
{
    public static string BaseURL = "https://abc.touristwise.co.uk/webservices/";
    public static string ImagesDirectoryPath = "Images";

    public const string API_MULTI_LANGUAGE = "language.php";
    public const string API_MENULIST = "menu.php";
    public const string API_DISCOUNTS = "village_discounts.php";
    public const string API_EVENTS = "events_list.php";
    public const string API_LOADINGSCREEN = "loadingscreen.php";
    public const string API_TRAIL_CAT = "trail_cat.php";
    public const string API_TRAILS = "trails.php";
    public const string API_POIS = "pois.php";
    public const string API_Feedback = "comments.php";
    public const string API_FeedbackPost = "trail_feedback.php";

    public const string API_ABOUT_US = "master.php?api=about_us&";
    public const string API_ABOUT_THIS_APP = "master.php?api=about_app&";
    public const string API_SHARE_WITH_OTHER = "master.php?api=share_text&";
    public const string API_APP_ICON = "app_icons.php";

    public const string VIDEO_FOLDER = "Video360";
    public const string VIDEO_DOWNLOAD_FOLDER = "TempVideo360";

    public const string MULTI_LANGUAGE = "PortaDownLanguage";

    public static string GetUrl(string endPoint)
    {
        if (endPoint.Contains(BaseURL))
            return endPoint;
        else
            return BaseURL + endPoint;
    }
}

public enum API_TYPE
{
    NONE,
    API_DISCOUNTS,
    API_EVENT_CAT,
    API_EVENTS,
    API_LOADINGSCREEN,
    API_TRAIL_POIS,
    API_TRAIL_CAT,
    API_TRAILS,
    API_ABOUT_US,
    API_ABOUT_THIS_APP,
    API_SHARE_WITH_OTHER,
    API_MENU,
    API_MULTI_LANGUAGE,
    API_Feedback,
    API_FEEDBACKPOST,
    API_APP_ICON
}

public enum TRAIL_TYPE
{
    NONE = 0,
    PHYSICAL = 1,
    INFORMATIONAL = 2
}

public enum LANGUAGES
{
    English,
    Spanish,
    German,
    Irish,
    Italian,
    Polish

}
public enum FileType
{
    NONE,
    IMAGE,
    AUDIO,
    VIDEO,
    VIDEO360
}
[System.Serializable]
public class ItemType
{
    public string prefix;
    public FileType fileType;
    public string url;

    public ItemType(FileType file, string _prefix, string _url)
    {
        prefix = _prefix;
        fileType = file;
        url = _url;
    }
}

[System.Serializable]
public class Asset
{
    public static Sprite GetSprite(Texture2D texture)
    {
        if (texture == null)
        {
            Debug.LogError("Texture null.");
            return null;
        }
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect);// .01f);
    }
}