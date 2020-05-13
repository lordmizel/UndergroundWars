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
    List<FactoryMenuOption> allButtons;

    List<Image> buttonImages = new List<Image>();
    List<Text> buttonTexts = new List<Text>();
    List<Text> buttonPrices = new List<Text>();

    List<GameObject> activeButtons = new List<GameObject>();
    List<Unit> currentUnitsShown = new List<Unit>();

    int currentOption = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach(FactoryMenuOption o in allButtons)
        {
            buttonImages.Add(o.unitSprite);
            buttonTexts.Add(o.unitName);
            buttonPrices.Add(o.unitPrice);
        }
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
        foreach(FactoryMenuOption button in allButtons)
        {
            button.gameObject.SetActive(false);
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
        buttonImages[optionNumber].sprite = unit.menuSprite;
        buttonTexts[optionNumber].text = unit.name;
        buttonPrices[optionNumber].text = unit.moneyValue.ToString() + "G";
        if(unit.moneyValue > GameManager.instance.activePlayer.GetFunds())
        {
            buttonPrices[optionNumber].color = Color.red;
        }
        else
        {
            buttonPrices[optionNumber].color = Color.black;
        }
        allButtons[optionNumber].gameObject.SetActive(true);
        activeButtons.Add(allButtons[optionNumber].gameObject);
        currentUnitsShown.Add(unit);
    }

    public void SelectCurrentMenuOption()
    {
        if (currentUnitsShown[currentOption].moneyValue > GameManager.instance.activePlayer.GetFunds())
        {
            //Unit is too expensive for the player.
            //TODO: Play some sound here.
        }
        else
        {
            //Create unit in factory tile
            Unit newUnit = Instantiate(currentUnitsShown[currentOption], PlayerCursor.instance.transform.position, Quaternion.identity);
            GameManager.instance.activePlayer.AddUnitToArmy(newUnit);
            newUnit.TireUnit();

            GameManager.instance.activePlayer.ChangeFunds(-currentUnitsShown[currentOption].moneyValue);

            ///////////////////////
            //TODO: this should eventually be deleted as the initialX/Y variables are just for debug
            newUnit.initialX = (int)PlayerCursor.instance.transform.position.x;
            newUnit.initialY = (int)PlayerCursor.instance.transform.position.y;
            ///////////////////////

            //newUnit.EstablishNewTile(Map.instance.GetTile((int)PlayerCursor.instance.transform.position.x, (int)PlayerCursor.instance.transform.position.y));
            DeactivateFactoryMenu();
        }
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
