using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortraitSelector : MonoBehaviour
{
    [SerializeField]
    Image highlightBorder;
    public Image portrait;
    
    public void HighlightMe()
    {
        var tempcolor = highlightBorder.color;
        tempcolor.a = 1f;
        highlightBorder.color = tempcolor;
    }

    public void UnHighlightMe()
    {
        var tempcolor = highlightBorder.color;
        tempcolor.a = 0f;
        highlightBorder.color = tempcolor;
    }
}
