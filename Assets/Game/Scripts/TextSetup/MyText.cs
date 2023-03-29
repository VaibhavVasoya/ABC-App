using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Master.UIKit;
public class MyText : MonoBehaviour
{
    [SerializeField] TextType type;
    TextProperty textProperty;
    Text txt;
    private void Awake()
    {
        txt = GetComponent<Text>();
        textProperty = UIController.instance.GenralText.textProperties.Find(x => x.type == type);
        txt.font = textProperty.font;
        txt.fontStyle = textProperty.fontStyle;
        txt.fontSize = textProperty.size;
        txt.lineSpacing = textProperty.lineSpacing;
        if (textProperty.isBestFit)
        {
            txt.resizeTextForBestFit = textProperty.isBestFit;
            txt.resizeTextMinSize = textProperty.minSize;
            txt.resizeTextMaxSize = textProperty.maxSize;
        }
    }


}
