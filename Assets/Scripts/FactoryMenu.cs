using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryMenu : MonoBehaviour
{
    public static FactoryMenu instance;

    [SerializeField]
    GameObject menu;

    [SerializeField]
    List<GameObject> allButtons;

    List<GameObject> activeButtons = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void ActivateFactoryMenu() {
        menu.SetActive(true);
    }
}
