using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour {

	int currentPositionX;
	int currentPositionY;

	// Use this for initialization
	void Start () {

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
			MoveCursor ();
		}
	}

	public void TeleportCursorToTile(int x, int y)
	{
		currentPositionX = x;
		currentPositionY = y;
		SetCursorPosition ();
	}

	void SetCursorPosition()
	{
		gameObject.transform.position = new Vector3 (currentPositionX, currentPositionY, gameObject.transform.position.z);
        CameraController.instance.MoveCameraToPoint(currentPositionX, currentPositionY);
	}

	void MoveCursor()
	{
		if (Input.GetKeyDown (KeyCode.W) && currentPositionY < Map.instance.mapHeight - 1) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR || 
				GameManager.gameState == GameManager.state.MOVING_UNIT && Map.instance.GetTile(currentPositionX, currentPositionY + 1).IsReachableByMovement()) 
			{
				currentPositionY++;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.S) && currentPositionY > 0) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR ||
			    GameManager.gameState == GameManager.state.MOVING_UNIT && Map.instance.GetTile (currentPositionX, currentPositionY - 1).IsReachableByMovement ()) 
			{
				currentPositionY--;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.D) && currentPositionX < Map.instance.mapWidth - 1) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR ||
			    GameManager.gameState == GameManager.state.MOVING_UNIT && Map.instance.GetTile (currentPositionX + 1, currentPositionY).IsReachableByMovement ()) 
			{
				currentPositionX++;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.A) && currentPositionX > 0) 
		{
			if (GameManager.gameState == GameManager.state.MOVING_CURSOR ||
			    GameManager.gameState == GameManager.state.MOVING_UNIT && Map.instance.GetTile (currentPositionX - 1, currentPositionY).IsReachableByMovement ()) 
			{
				currentPositionX--;
				SetCursorPosition ();
			}
		}
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			Map.instance.GetTile (currentPositionX, currentPositionY).TileSelected ();
		}
	}


}
