using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortraitSelector : MonoBehaviour
{
    [SerializeField]
    Image highlightBorder;
    
    public void HighlightMe()
    {
        var tempcolor = highlightBorder.color;
        tempcolor.a = 1f;
        highlightBorder.color = tempcolor;
    }

    public void UnHighlightMe()
    {

    }
}
