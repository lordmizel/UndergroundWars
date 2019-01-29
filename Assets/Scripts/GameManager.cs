using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

	public enum state
	{
		MOVING_CURSOR,
		IDLE,
		MOVING_UNIT,
		NAVIGATING_MENU,
		SELECTING_ATTACK,
		AFTER_MENU_BUFFER,
		AFTER_MENU_ATTACK_BUFFER,
		CHECKING_ENEMY_UNIT
	}

	public static state gameState;

	Army[] players;
	public Army activePlayer;
	int playerTurnIndex = 0;

    public Unit unitSelected;

    void Awake()
    {
        instance = this;
    }

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
		if (gameState == state.AFTER_MENU_BUFFER || gameState == state.AFTER_MENU_ATTACK_BUFFER || gameState == state.CHECKING_ENEMY_UNIT) 
		{
			if (Input.GetKeyUp (KeyCode.Return)) 
			{
				Map.instance.ReturnTilesToNormal ();
				if (gameState == state.AFTER_MENU_ATTACK_BUFFER) 
				{
					gameState = state.SELECTING_ATTACK;
				} else 
				{
					gameState = state.MOVING_CURSOR;
				}
			}
		} 
		else if (gameState == state.MOVING_UNIT) 
		{
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				Map.instance.ReturnTilesToNormal ();
				gameState = state.MOVING_CURSOR;
			}
		} 
		//else 
	}

	public void PassTurn()
	{
        activePlayer.RefreshAllUnits();
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
        Map.instance.RecountPlayerPropierties();
	}
}
