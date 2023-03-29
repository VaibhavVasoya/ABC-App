using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TextSetup", menuName = "TextSetup")]
public class GenralText : ScriptableObject
{
    [SerializeField] public List<TextProperty> textProperties;
}

[System.Serializable]
public struct TextProperty
{
    public TextType type;
    public Font font;
    public FontStyle fontStyle;
    public int size;
    public float lineSpacing;
    public bool isBestFit;
    public int minSize;
    public int maxSize;
}


public enum TextType
{
    TITLE_1_60,
    TITLE_2_50,
    SUB_TITLE_60,
    SUB_TITLE_50,
    SHORT_DESCRIPTION,
    LONG_DESCRIPTION,
    BTN_TEXT 
}
