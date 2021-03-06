﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    public static InGameMenu inGameMenu;

    [SerializeField]
    GameObject menuPanel;
    [SerializeField]
    CargoMenu cargoMenu;
    [SerializeField]
    FactoryMenu factoryMenu;

    [SerializeField]
    List<MenuOption> allOptions;

    List<MenuOption> optionsShowing = new List<MenuOption>();

    

    int optionIndex = 0;

    void Awake()
    {
        inGameMenu = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.gameState == GameManager.state.MOVING_CURSOR)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ShowNeutralMenu();
            }
        }
        else if (GameManager.gameState == GameManager.state.NAVIGATING_MENU)
        {
            SelectMenuOption();
        }
    }

    void SelectMenuOption()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            optionsShowing[optionIndex].GetComponent<Image>().color = Color.white;
            if (optionIndex != 0)
            {
                optionIndex--;
            }
            else
            {
                optionIndex = optionsShowing.Count - 1;
            }
            optionsShowing[optionIndex].GetComponent<Image>().color = Color.yellow;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            optionsShowing[optionIndex].GetComponent<Image>().color = Color.white;
            if (optionIndex != optionsShowing.Count - 1)
            {
                optionIndex++;
            }
            else
            {
                optionIndex = 0;
            }
            optionsShowing[optionIndex].GetComponent<Image>().color = Color.yellow;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.unitSelected != null)
            {
                GameManager.instance.unitSelected.ReturnBackToOrigin();
            }
            HideMenu();
            GameManager.gameState = GameManager.state.MOVING_CURSOR;
        }
    }

    public void SelectCurrentMenuOption()
    {
        StartCoroutine(SelectOption(optionIndex));
    }

    IEnumerator SelectOption(int optionSelected)
    {
        yield return new WaitForEndOfFrame();

        switch (optionsShowing[optionSelected].myOption)
        {
            case MenuOption.menuOptions.ATTACK:
                GameManager.instance.attackingWasSelected = true;
                GameManager.instance.unitSelected.PrepareToAttack();
                GameManager.gameState = GameManager.state.SELECTING_OBJECTIVE;
                Debug.Log("Attack selected");
                break;
            case MenuOption.menuOptions.LOAD:
                //TODO: Program loading option
                Debug.Log("Load selected");
                GameManager.instance.unitSelected.PrepareToLoad();
                GameManager.gameState = GameManager.state.SELECTING_OBJECTIVE;
                break;
            case MenuOption.menuOptions.UNLOAD:
                Debug.Log("Unload selected");
                //GameManager.instance.unitSelected.EstablishNewTile();
                GameManager.instance.unitSelected.GetComponent<Cargo>().UnloadSelected();
                GameManager.gameState = GameManager.state.NAVIGATING_CARGO_MENU;
                break;
            case MenuOption.menuOptions.CAPTURE:
                GameManager.instance.unitSelected.CaptureTile();
                GameManager.instance.unitSelected.EstablishNewTile();
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
                Debug.Log("Capture Selected");
                break;
            case MenuOption.menuOptions.SUPPLY:
                GameManager.instance.unitSelected.ResupplyUnits();
                GameManager.instance.unitSelected.EstablishNewTile();
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
                Debug.Log("Supply Selected");
                break;
            case MenuOption.menuOptions.WAIT:
                GameManager.instance.unitSelected.EstablishNewTile();
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
                Debug.Log("Wait selected");
                break;
            case MenuOption.menuOptions.POWER:
                GameManager.instance.activePlayer.ActivatePower(1);
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
                Debug.Log("Power selected");
                break;
            case MenuOption.menuOptions.SUPER_POWER:
                GameManager.instance.activePlayer.ActivatePower(2);
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
                Debug.Log("Super power selected");
                break;
            case MenuOption.menuOptions.END_TURN:
                Debug.Log("End turn selected");
                GameManager.instance.PassTurn();
                GameManager.gameState = GameManager.state.MOVING_CURSOR;
                break;
            case MenuOption.menuOptions.TEST_OPTION:
                Debug.Log("Test option selected");
                break;
            default:
                Debug.Log("Something else selected");
                break;
        }
        HideMenu();
    }

    public void ActivateMenuOption(MenuOption.menuOptions option)
    {
        foreach (MenuOption optionInMenu in allOptions)
        {
            if (optionInMenu.myOption == option)
            {
                optionInMenu.gameObject.SetActive(true);
            }
        }
    }

    public void ActivateMenu()
    {
        foreach (MenuOption optionInMenu in allOptions)
        {
            if (optionInMenu.gameObject.activeSelf == true)
            {
                optionsShowing.Add(optionInMenu);
            }
        }
        optionIndex = 0;
        optionsShowing[0].GetComponent<Image>().color = Color.yellow;
        menuPanel.SetActive(true);
    }

    void HideMenu()
    {
        foreach (MenuOption optionInMenu in optionsShowing)
        {
            optionInMenu.GetComponent<Image>().color = Color.white;
            optionInMenu.gameObject.SetActive(false);
        }
        optionIndex = 0;
        optionsShowing.Clear();
        menuPanel.SetActive(false);
    }

    public void ShowNeutralMenu()
    {
        if (GameManager.instance.activePlayer.currentSpecialPower >= GameManager.instance.activePlayer.mediumPowerThreshold)
        {
            ActivateMenuOption(MenuOption.menuOptions.POWER);
        }
        if (GameManager.instance.activePlayer.currentSpecialPower >= GameManager.instance.activePlayer.GetMaxPower())
        {
            ActivateMenuOption(MenuOption.menuOptions.SUPER_POWER);
        }
        ActivateMenuOption(MenuOption.menuOptions.END_TURN);
        ActivateMenu();
        GameManager.gameState = GameManager.state.NAVIGATING_MENU;
    }
}
