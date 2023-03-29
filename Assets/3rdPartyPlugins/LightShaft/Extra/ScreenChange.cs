using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScreenChange : MonoBehaviour
{
    public void ChangeOrientation()
    {
        Debug.LogError(Screen.orientation);
        
        Screen.autorotateToPortrait = !(ScreenOrientation.Portrait == Screen.orientation);
        Screen.autorotateToPortraitUpsideDown = !(ScreenOrientation.Portrait == Screen.orientation);
        Screen.autorotateToLandscapeLeft = (ScreenOrientation.Portrait == Screen.orientation);
        Screen.autorotateToLandscapeRight = (ScreenOrientation.Portrait == Screen.orientation);
        Screen.orientation = (ScreenOrientation.Portrait == Screen.orientation) ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}
