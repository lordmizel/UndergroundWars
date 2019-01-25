using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
	public static InGameMenu inGameMenu;
	[SerializeField]
	GameObject menuPanel;

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
	void Update () 
	{
		if (GameManager.gameState == GameManager.state.MOVING_CURSOR) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				ActivateMenuOption (MenuOption.menuOptions.END_TURN);
				ActivateMenu ();
				GameManager.gameState = GameManager.state.NAVIGATING_MENU;
			}
		}
		else if (GameManager.gameState == GameManager.state.NAVIGATING_MENU) 
		{
			SelectMenuOption ();
		}

	}

	void SelectMenuOption()
	{
		if (Input.GetKeyDown (KeyCode.W)) 
		{
			optionsShowing [optionIndex].GetComponent<Image> ().color = Color.white;
			if (optionIndex != 0) 
			{
				optionIndex--;
			} 
			else 
			{
				optionIndex = optionsShowing.Count - 1;
			}
			optionsShowing [optionIndex].GetComponent<Image> ().color = Color.yellow;
		}
		if (Input.GetKeyDown (KeyCode.S)) 
		{
			optionsShowing [optionIndex].GetComponent<Image> ().color = Color.white;
			if (optionIndex != optionsShowing.Count - 1) 
			{
				optionIndex++;
			} 
			else 
			{
				optionIndex = 0;
			}
			optionsShowing [optionIndex].GetComponent<Image> ().color = Color.yellow;
		}
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			SelectOption (optionIndex);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			if (GameManager.instance.activePlayer.unitSelected != null) 
			{
				GameManager.instance.activePlayer.unitSelected.ReturnBackToOrigin ();
			}
			HideMenu ();
			GameManager.gameState = GameManager.state.MOVING_CURSOR;
		}

	}

	void SelectOption(int optionSelected)
	{
		switch (optionsShowing [optionSelected].myOption) {
		case MenuOption.menuOptions.ATTACK:
			GameManager.instance.activePlayer.unitSelected.PrepareToAttack();
			GameManager.gameState = GameManager.state.AFTER_MENU_ATTACK_BUFFER;
			Debug.Log ("Attack selected");
			break;
        case MenuOption.menuOptions.CAPTURE:
            GameManager.instance.activePlayer.unitSelected.CaptureTile();
            GameManager.instance.activePlayer.unitSelected.EstablishNewTile();
            GameManager.gameState = GameManager.state.AFTER_MENU_BUFFER;
            Debug.Log("Capture Selected");
            break;
		case MenuOption.menuOptions.WAIT:
			GameManager.instance.activePlayer.unitSelected.EstablishNewTile ();
			GameManager.gameState = GameManager.state.AFTER_MENU_BUFFER;
			Debug.Log ("Wait selected");
			break;
		case MenuOption.menuOptions.END_TURN:
			Debug.Log ("End turn selected");
			GameManager.instance.PassTurn ();
			GameManager.gameState = GameManager.state.AFTER_MENU_BUFFER;
			break;
		case MenuOption.menuOptions.TEST_OPTION:
			Debug.Log ("Test option selected");
			break;
		default:
			Debug.Log ("Something else selected");
			break;
		}
		HideMenu ();
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
				optionsShowing.Add (optionInMenu);
			}
		}
		optionIndex = 0;
		optionsShowing [0].GetComponent<Image> ().color = Color.yellow;
		menuPanel.SetActive (true);
	}

	void HideMenu()
	{
		foreach (MenuOption optionInMenu in optionsShowing) 
		{
			optionInMenu.GetComponent<Image> ().color = Color.white;
			optionInMenu.gameObject.SetActive (false);
		}
		optionIndex = 0;
		optionsShowing.Clear ();
		menuPanel.SetActive (false);
	}
}
