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

    int currentOption = 0;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (GameManager.gameState == GameManager.state.NAVIGATING_FACTORY_MENU)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeactivateFactoryMenu();
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
            }
        }
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
        GameManager.gameState = GameManager.state.NAVIGATING_FACTORY_MENU;
    }

    void DeactivateFactoryMenu()
    {
        currentOption = 0;
        menu.SetActive(false);
    }

    public void FillOption(int optionNumber, Unit unit)
    {
        buttonTexts[optionNumber].text = unit.name;
        allButtons[optionNumber].SetActive(true);
    }
}
