using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoMenu : MonoBehaviour
{
    public static CargoMenu instance;

    [SerializeField]
    GameObject menuContainer;
    [SerializeField]
    List<GameObject> unitButtons;
    [SerializeField]
    List<Text> buttonTexts;

    List<GameObject> activeButtons;
    public int buttonIndex = 0;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == GameManager.state.NAVIGATING_CARGO_MENU)
        {
            SelectMenuOption();
        }
    }

    public void ActivateUnitCargoMenu(Unit[] unitsToShow)
    {
        activeButtons = new List<GameObject>();
        buttonIndex = 0;
        for (int x = 0; x < unitsToShow.Length; x++)
        {
            Debug.Log(unitsToShow[x]);
            if (unitsToShow[x] != null)
            {
                buttonTexts[x].text = unitsToShow[x].name;
                unitButtons[x].SetActive(true);
                unitButtons[x].GetComponent<Image>().color = Color.white;
                activeButtons.Add(unitButtons[x]);
                
            }
        }
        menuContainer.SetActive(true);
        activeButtons[0].GetComponent<Image>().color = Color.yellow;
    }

    public void DeactivateUnitCargoMenu()
    {
        for (int x = 0; x < unitButtons.Count; x++)
        {
            buttonTexts[x].text = "";
            unitButtons[x].SetActive(false);
        }
        menuContainer.SetActive(false);
    }

    void SelectMenuOption()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            activeButtons[buttonIndex].GetComponent<Image>().color = Color.white;
            if (buttonIndex != 0)
            {
                buttonIndex--;
            }
            else
            {
                buttonIndex =  activeButtons.Count - 1;
            }
            activeButtons[buttonIndex].GetComponent<Image>().color = Color.yellow;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            activeButtons[buttonIndex].GetComponent<Image>().color = Color.white;
            if (buttonIndex != activeButtons.Count - 1)
            {
                buttonIndex++;
            }
            else
            {
                buttonIndex = 0;
            }
            activeButtons[buttonIndex].GetComponent<Image>().color = Color.yellow;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.unitSelected.gameObject.GetComponent<Cargo>().alreadyUnloadedAnUnit)
            {
                DeactivateUnitCargoMenu();
                GameManager.instance.unitSelected.EstablishNewTile();
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
            }
            else
            {
                DeactivateUnitCargoMenu();
                GameManager.instance.unitSelected.ArrivedAtDestination(GameManager.instance.unitSelected.possibleDestination.GetTileCoordX(), GameManager.instance.unitSelected.possibleDestination.GetTileCoordY());
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            DeactivateUnitCargoMenu();
            Unit activeUnit = GameManager.instance.unitSelected;
            activeUnit.LayOutInteractableTiles(activeUnit.GetComponent<Cargo>().GetUnloadArea(buttonIndex));
            GameManager.gameState = GameManager.state.SELECTING_OBJECTIVE;
        }
    }
}
