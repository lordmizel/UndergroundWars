using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{
    public static PlayerCursor instance;

	int currentPositionX;
	int currentPositionY;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        TeleportCursorToLastTileOfCharacter();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.gameState == GameManager.state.MOVING_CURSOR || GameManager.gameState == GameManager.state.MOVING_UNIT) 
		{
			MoveCursor ();
		}
	}

    public void PinPointTile(ClickableTile tile)
    {
        //TeleportCursorToTile(tile.GetTileCoordX(), tile.GetTileCoordY());
        currentPositionX = tile.GetTileCoordX();
        currentPositionY = tile.GetTileCoordY();
        SetCursorPosition();
    }

	//public void TeleportCursorToTile(int x, int y)
	//{
	//	currentPositionX = x;
	//	currentPositionY = y;
	//	SetCursorPosition ();
	//}

	void SetCursorPosition()
	{
		gameObject.transform.position = new Vector3 (currentPositionX, currentPositionY, gameObject.transform.position.z);
        CameraController.instance.MoveCameraToPoint(currentPositionX, currentPositionY);
        UI.instance.UpdateTileInfo(Map.instance.GetTile(currentPositionX, currentPositionY));
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

        UIMovement.instance.MoveAside(transform.position);
	}

    public void GetCurrentTile()
    {
        Map.instance.GetTile(currentPositionX, currentPositionY).TileSelected();
    }

    public void TeleportCursorToLastTileOfCharacter()
    {
        Vector2 location = GameManager.instance.activePlayer.GetPlaceOfCursor();
        PinPointTile(Map.instance.GetTile((int)location.x, (int)location.y));
    }
}
