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
    List<Unit> currentUnitsShown = new List<Unit>();

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
        activeButtons[currentOption].GetComponent<Image>().color = Color.yellow;
        menu.SetActive(true);
        GameManager.gameState = GameManager.state.NAVIGATING_FACTORY_MENU;
    }

    void DeactivateFactoryMenu()
    {
        //currentOption = 0;
        foreach(GameObject button in activeButtons)
        {
            activeButtons[currentOption].GetComponent<Image>().color = Color.white;
        }
        menu.SetActive(false);
        currentUnitsShown.Clear();
        GameManager.gameState = GameManager.state.MOVING_CURSOR;
    }

    public void FillOption(int optionNumber, Unit unit)
    {
        buttonTexts[optionNumber].text = unit.name;
        allButtons[optionNumber].SetActive(true);
        activeButtons.Add(allButtons[optionNumber]);
        currentUnitsShown.Add(unit);
    }

    public void SelectCurrentMenuOption()
    {
        //Create unit in factory tile
        Unit newUnit = Instantiate(currentUnitsShown[currentOption], PlayerCursor.instance.transform.position, Quaternion.identity);

        ///////////////////////
        //TODO: this should eventually be deleted as the initialX/Y variables are just for debug
        newUnit.initialX = (int)PlayerCursor.instance.transform.position.x;
        newUnit.initialY = (int)PlayerCursor.instance.transform.position.y;
        ///////////////////////

        //newUnit.EstablishNewTile(Map.instance.GetTile((int)PlayerCursor.instance.transform.position.x, (int)PlayerCursor.instance.transform.position.y));
        DeactivateFactoryMenu();
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
