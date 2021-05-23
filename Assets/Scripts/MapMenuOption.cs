using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenuOption : MonoBehaviour
{
    public Text buttonText;

    public void ChangeName(string newName)
    {
        buttonText.text = newName;
    }
}
