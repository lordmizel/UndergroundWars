﻿using System.Collections;
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
        NAVIGATING_CARGO_MENU,
        NAVIGATING_FACTORY_MENU,
		SELECTING_OBJECTIVE,
		CHECKING_ENEMY_UNIT
	}

	public static state gameState;

    GameSpecifications gameSpecs;
	Army[] players;
	public Army activePlayer;
	int playerTurnIndex = 0;
    public int globalTurnIndex = 1;

    public Unit unitSelected;

    [SerializeField]
    List<Vector2> initialCursorPositions;

    [HideInInspector]
    public bool attackingWasSelected = false;

    [SerializeField]
    TurnLabel turnLabel;

    void Awake()
    {
        instance = this;
        gameSpecs = FindObjectOfType<GameSpecifications>();
        players = FindObjectsOfType<Army>();
        for (int i = 0; i < gameSpecs.playerNumber; i++)
        {
            players[i].COIdentity = gameSpecs.officers[i];
            players[i].SetLastPlaceOfCursor((int)initialCursorPositions[i].x, (int)initialCursorPositions[i].y);
        }
        activePlayer = players[0];
    }

    // Use this for initialization
    void Start ()
    {
		gameState = state.MOVING_CURSOR;
        turnLabel.gameObject.SetActive(true);
        Map.instance.RecountPlayerPropierties();
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(gameState == state.NAVIGATING_MENU)
            {
                InGameMenu.inGameMenu.SelectCurrentMenuOption();
            }
            else if (gameState == state.NAVIGATING_FACTORY_MENU)
            {
                FactoryMenu.instance.SelectCurrentMenuOption();
            }
            else if (gameState == state.MOVING_CURSOR || gameState == state.MOVING_UNIT)
            {
                PlayerCursor.instance.GetCurrentTile();
            }
            else if(gameState == state.SELECTING_OBJECTIVE)
            {
                if (unitSelected.readyToAttack)
                {
                    unitSelected.AttackNow();
                }
                else if (unitSelected.readyToLoad)
                {
                    unitSelected.LoadIntoCargo();
                }
                else
                {
                    Cargo unitCargo = unitSelected.GetComponent<Cargo>();
                    if(unitCargo != null && unitCargo.unloadingUnit)
                    {
                        StartCoroutine(unitCargo.UnloadUnitInTile());
                    }
                }
            }
        }

		//This is a patch for avoiding selecting a tile right as you select an option from the menu
		if (gameState == state.CHECKING_ENEMY_UNIT) 
		{
			if (Input.GetKeyUp (KeyCode.Return)) 
			{
				Map.instance.ReturnTilesToNormal ();
				//if (attackingWasSelected == true) 
				//{
				//	gameState = state.SELECTING_ATTACK;
				//} else 
				//{
					gameState = state.MOVING_CURSOR;
				//}
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

        /////////////////////////
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activePlayer.AddPower(2000);
        }
        /////////////////////////
    }

	public void PassTurn()
	{
        turnLabel.gameObject.SetActive(false);
        activePlayer.RefreshAllUnits();
        activePlayer.SetLastPlaceOfCursor((int)PlayerCursor.instance.transform.position.x, (int)PlayerCursor.instance.transform.position.y);
        if (playerTurnIndex == players.Length - 1) 
		{
			playerTurnIndex = 0;
            globalTurnIndex++;
		} 
		else 
		{
			playerTurnIndex++;
		}
        activePlayer = players [playerTurnIndex];
        activePlayer.StartNewTurn();
        PlayerCursor.instance.TeleportCursorToLastTileOfCharacter();
        Map.instance.RecountPlayerPropierties();
        UI.instance.UpdateFundsDisplay();
        UI.instance.UpdatePowerDisplay();
        MusicManager.instance.PlayMusic(activePlayer.COIdentity.armyTheme);
        turnLabel.gameObject.SetActive(true);
    }
}
