using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryMenu : MonoBehaviour
{
    public static FactoryMenu instance;

    [SerializeField]
    GameObject menu;

    [SerializeField]
    List<GameObject> allButtons;
    [SerializeField]
    List<Text> buttonTexts;

    List<GameObject> activeButtons = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void ActivateFactoryMenu(ClickableTile.factoryType factoryType) {
        activeButtons.Clear();
        foreach(GameObject button in allButtons)
        {
            button.SetActive(false);
        }
        //TODO: FillFactory should take the type of factory as a parameter
        GameManager.instance.activePlayer.FillFactory(factoryType);
        menu.SetActive(true);
    }

    public void FillOption(int optionNumber, Unit unit)
    {
        buttonTexts[optionNumber].text = unit.name;
        allButtons[optionNumber].SetActive(true);
    }
}
