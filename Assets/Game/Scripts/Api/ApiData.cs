using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "ApiData", menuName = "ApiData")]
public class ApiData : ScriptableObject
{
    public List<MultiLanguage> multiLanguages;
    //feedback:
    public List<WalkThrough> walkThroughs;
    public List<Menu> menuList;
    public List<TrailCat> trailCats;
    public List<Trail> trails;
    public List<Poi> trailPois;

    public List<SculptureEvent> sculptureEvents;
    public List<VillageDiscount> villageDiscounts;
    public List<Feedback> feedBackOptions;

    public ShareWithOther shareWithOther = null;

    //public string AboutUs = null;
    //public string AboutThisApp = null;

    public AboutUs aboutUs;
    public AboutTheApp aboutTheApp;

    [EasyButtons.Button]
    void ClearAllData()
    {
        walkThroughs.Clear();
        menuList.Clear();
        trails.Clear();
        trailPois.Clear();
        sculptureEvents.Clear();
        villageDiscounts.Clear();
        feedBackOptions.Clear();
    }

    [EasyButtons.Button]
    void DeletePlayerPrefbs()
    {
        PlayerPrefs.DeleteAll();
    }
}

[Serializable]
public class Menu
{
    public string num;
    public string title;
}

[Serializable]
public class WalkThrough
{
    public string num;
    public string title;
    public string description;
    public string image;
}

[Serializable]
public class TrailCat
{
    public string num;
    public string lang_id;
    public string user_id;
    public string title;
    public string sub_title;
    public string image;
    public string sort_order;
}

[Serializable]
public class Trail
{
    public string num;
    public string category_id;
    public string category_name;
    public string location_id;
    public string title;
    public string sub_title;
    public string summary;
    public string description;
    public string cover_image;
    public string estimated_duration;
    public string topic_id;
    public string user_id;

    public TRAIL_TYPE type;
   
    public void Parse()
    {
        if (category_id == "29")
            type = TRAIL_TYPE.PHYSICAL;
        else if (category_id == "30")
            type = TRAIL_TYPE.INFORMATIONAL;
        else
            type = TRAIL_TYPE.NONE;
        //(TRAIL_TYPE)int.Parse(cat_id);
    }
}

[Serializable]
public class Poi
{
    public string num;
    public string user_id;
    public string Name;
    public string sub_title;
    public string short_desc;
    public string description;
    public string thumbnail;
    public string place_address;
    public string audio_file;
    public string standard_video_file_url;
    public string standard_video_stream_url;
    public string image_360_url;
    public string image_360_file;
    public string video_360_url;
    public string video_360_file;
    public string transparent_video;
    public string latitude;
    public string longitude;
    public string booking_url;
    public string radius;
    public string n_message;
    public string tel;
    public string email;
    public string website;
    public string pin_id;
    public string intro_id;
    public List<PoiImage> poi_images;
    public List<Questions> questions;
}

[Serializable]
public class SculptureEvent
{
    public string num;
    public string name;
    public string category_id;
    public string short_desc;
    public string description;
    public string main_image;
    public string audio_url;
    public string vieo_url;
    public string video_360_url;
    public string video_360_image;
    public string start_date;
    public string end_date;
    public string address;
    public string latitude;
    public string logitude;
    public string tel;
    public string email;
    public string booking_url;
    public List<MultiImage> multiple_images;
}

[Serializable]
public class VillageDiscount
{
    public string num;
    public string business_name;
    public string short_description;
    public string full_description;
    public string image;
    public string address;
    public string latitude;
    public string longitude;
    public string tel;
    public string email;
    public string end_date;
}

#region Quiz
[Serializable]
public class Quiz
{
    public string num;
    public string name;
    public string sculp_id;
    public List<Questions> questions;
}
[Serializable]
public class Questions
{
    public string poi_id;
    public string question;
    public string type;
    public List<Answer> answers;
}
[Serializable]
public class Answer
{
    public string num;
    public string question_id;
    public string answer;
    public string is_correct;
}
[Serializable]
public class PoiImage
{
    public string num;
    public string poi_id;
    public string images;

}
[Serializable]
public class MultiImage
{
    public string num;
    public string lang_id;
    public string event_id;
    public string image;
}
#endregion

[Serializable]
public class ShareWithOther
{
    public string share_text;
    public string share_link;
}

[Serializable]
public class MultiLanguage
{
    public string num;
    public string name;
    public string isocode;
    public string icon;
    public string status;
    public Texture2D texture2D;

    public async Task DonwloadAssets()
    {
        await Services.Download(num,icon, (texture) => { texture2D = (texture); });
    }
}

//Feedback API integration
[Serializable]
public class Feedback
{
    public string num;
    public string comment;
}

//about us
[Serializable]
public class AboutUs
{
    public string about_text;
    public string about_image;
}

[Serializable]
public class AboutTheApp
{
    public string about_app_text;
    public string about_app_image;
}
