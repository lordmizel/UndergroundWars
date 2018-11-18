using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum state
	{
		MOVING_CURSOR,
		IDLE,
		MOVING_UNIT,
		NAVIGATING_MENU,
		AFTER_MENU_BUFFER
	}

	public static state gameState;

	Army[] players;
	public Army activePlayer;
	int playerTurnIndex = 0;

	// Use this for initialization
	void Start () {
		gameState = state.MOVING_CURSOR;
		players = FindObjectsOfType<Army> ();
		activePlayer = players [0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		//This is a patch for avoiding selecting a tile right as you select an option from the menu
		if (gameState == state.AFTER_MENU_BUFFER) 
		{
			if (Input.GetKeyUp (KeyCode.Return)) 
			{
				gameState = state.MOVING_CURSOR;
			}
		}
	}

	public void PassTurn()
	{
		if (playerTurnIndex == players.Length - 1) 
		{
			playerTurnIndex = 0;
		} 
		else 
		{
			playerTurnIndex++;
		}
		activePlayer = players [playerTurnIndex];
		activePlayer.RefreshAllUnits ();
	}
}
