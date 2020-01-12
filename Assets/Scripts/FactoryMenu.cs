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

    private void LateUpdate()
    {
        
        if (GameManager.gameState == GameManager.state.NAVIGATING_FACTORY_MENU)
        {
            SelectMenuOption();
        }
    }

    void Update()
    {
        //if (GameManager.gameState == GameManager.state.NAVIGATING_FACTORY_MENU)
        //{
        //    SelectMenuOption();
        //}
    }

    public void ActivateFactoryMenu(ClickableTile.factoryType factoryType) {
        currentOption = 0;
        activeButtons.Clear();
        foreach(GameObject button in allButtons)
        {
            button.SetActive(false);
        }
        GameManager.instance.activePlayer.FillFactory(factoryType);
        menu.SetActive(true);
        GameManager.gameState = GameManager.state.NAVIGATING_FACTORY_MENU;
    }

    void DeactivateFactoryMenu()
    {
        //currentOption = 0;
        menu.SetActive(false);
        GameManager.gameState = GameManager.state.MOVING_CURSOR;
    }

    public void FillOption(int optionNumber, Unit unit)
    {
        buttonTexts[optionNumber].text = unit.name;
        allButtons[optionNumber].SetActive(true);
        activeButtons.Add(allButtons[optionNumber]);
    }

    public void SelectCurrentMenuOption()
    {
        DeactivateFactoryMenu();
        //TODO: Create unit in factory
    }

    void SelectMenuOption()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            activeButtons[currentOption].GetComponent<Image>().color = Color.white;
            if (currentOption != 0)
            {
                currentOption--;
            }
            else
            {
                currentOption = activeButtons.Count - 1;
            }
            activeButtons[currentOption].GetComponent<Image>().color = Color.yellow;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            activeButtons[currentOption].GetComponent<Image>().color = Color.white;
            if (currentOption != activeButtons.Count - 1)
            {
                currentOption++;
            }
            else
            {
                currentOption = 0;
            }
            activeButtons[currentOption].GetComponent<Image>().color = Color.yellow;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactivateFactoryMenu();
        }
    }
}
