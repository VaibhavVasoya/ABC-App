using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetMarkerIndex : MonoBehaviour
{
    [SerializeField] TextMeshPro markerIndexTxt;

    public void SetIndex(int index)
    {
        Debug.Log("123prefavdhsvsdj " + index);
        markerIndexTxt.text = index.ToString();
    }
}
