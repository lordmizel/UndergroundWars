using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour {

	int currentPositionX;
	int currentPositionY;

	Map map;

	// Use this for initialization
	void Start () {
		map = FindObjectOfType<Map> ();

		//TODO: Initialize to a real valid position, this is just for debug
		currentPositionX = 0;
		currentPositionY = 0;
		/////////

		SetCursorPosition ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gameState == GameManager.state.MOVING_CURSOR || GameManager.gameState == GameManager.state.MOVING_UNIT) 
		{
			CheckForInput ();
		}
	}

	void SetCursorPosition()
	{
		gameObject.transform.position = new Vector3 (currentPositionX, currentPositionY, gameObject.transform.position.z);
	}

	void CheckForInput()
	{
		if (Input.GetKeyDown (KeyCode.W) && currentPositionY < map.mapHeight - 1) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR || 
				GameManager.gameState == GameManager.state.MOVING_UNIT && map.GetTile(currentPositionX, currentPositionY + 1).IsReachableByMovement()) 
			{
				currentPositionY++;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.S) && currentPositionY > 0) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR ||
			    GameManager.gameState == GameManager.state.MOVING_UNIT && map.GetTile (currentPositionX, currentPositionY - 1).IsReachableByMovement ()) 
			{
				currentPositionY--;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.D) && currentPositionX < map.mapWidth - 1) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR ||
			    GameManager.gameState == GameManager.state.MOVING_UNIT && map.GetTile (currentPositionX + 1, currentPositionY).IsReachableByMovement ()) 
			{
				currentPositionX++;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.A) && currentPositionX > 0) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR ||
			    GameManager.gameState == GameManager.state.MOVING_UNIT && map.GetTile (currentPositionX - 1, currentPositionY).IsReachableByMovement ()) 
			{
				currentPositionX--;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			map.GetTile (currentPositionX, currentPositionY).TileSelected ();
		}
	}
}
