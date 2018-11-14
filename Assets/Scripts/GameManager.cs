using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public enum state
	{
		MOVING_CURSOR,
		IDLE,
		MOVING_UNIT
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
	void Update () {
		//TODO: This is not the way turns are going to work, just a placeholder for debug
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			PassTurn ();
		}
	}

	void PassTurn()
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
