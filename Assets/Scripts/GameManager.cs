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

	// Use this for initialization
	void Start () {
		gameState = state.MOVING_CURSOR;
		players = FindObjectsOfType<Army> ();
		activePlayer = players [0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
