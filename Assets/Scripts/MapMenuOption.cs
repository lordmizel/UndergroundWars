using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenuOption : MonoBehaviour
{
    public Text buttonText;
    Image myImage;
    Color initialColor, litColor;

    private void Start()
    {
        myImage = this.GetComponent<Image>();
        initialColor = myImage.color;
        litColor = Color.yellow;
    }

    public void ChangeName(string newName)
    {
        buttonText.text = newName;
    }

    public void LightUp()
    {
        myImage.color = litColor;
    }

    public void UnLight()
    {
        myImage.color = initialColor;
    }
}
