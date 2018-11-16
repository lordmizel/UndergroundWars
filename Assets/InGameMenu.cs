using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour {

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
		Debug.Log (menuPanel.transform.childCount);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.O)) 
		{
			ActivateMenuOption (MenuOption.menuOptions.WAIT);
		} 
		if (GameManager.gameState == GameManager.state.NAVIGATING_MENU) {
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
		}
	}

	void SelectOption(int optionSelected)
	{
		switch (optionsShowing [optionSelected].myOption) {
		case MenuOption.menuOptions.ATTACK:
			Debug.Log ("Attack selected");
			break;
		case MenuOption.menuOptions.WAIT:
			Debug.Log ("Wait selected");
			break;
		case MenuOption.menuOptions.END_TURN:
			Debug.Log ("End turn selected");
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
		optionsShowing.Clear ();
		menuPanel.SetActive (false);
		GameManager.gameState = GameManager.state.MOVING_CURSOR;
	}
}
