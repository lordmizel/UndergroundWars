using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour {

	int coordX;
	int coordY;
	public TileType typeOfTerrain;

	Unit unitAssigned;

	[SerializeField]
	GameObject moveColorOverlay;
	[SerializeField]
	GameObject attackColorOverlay;

    public Army propietary = null;
    [SerializeField]
    SpriteRenderer propietaryOverlay = null;

	//Pathfinding stuff
	public int movementCost = 1;
	public List<ClickableTile> neighbors;
	public ClickableTile parent;
	[HideInInspector]
	public int gCost;
	public int hCost;
    

	public void SetTileCoordinates(int x, int y)
	{
		coordX = x;
		coordY = y;
	}

	public int GetTileCoordX()
	{
		return coordX;
	}

	public int GetTileCoordY()
	{
		return coordY;
	}

	public void AssignUnit(Unit unit)
	{
		unitAssigned = unit;
	}

	public void UnassignUnit()
	{
		unitAssigned = null;
	}

	public Unit GetUnitAssigned()
	{
		return unitAssigned;
	}

	public void TileSelected()
	{
		switch (GameManager.gameState) 
		{
		case GameManager.state.MOVING_CURSOR:
			if (unitAssigned != null) 
			{
				unitAssigned.UnitSelected ();
			} 
			else 
			{
				InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.END_TURN);
				InGameMenu.inGameMenu.ActivateMenu ();
				GameManager.gameState = GameManager.state.NAVIGATING_MENU;
			}
			break;

		case GameManager.state.MOVING_UNIT:
			if (GameManager.instance.activePlayer.unitSelected.originTile == this) 
			{
				Map.instance.ReturnTilesToNormal ();
				GameManager.instance.activePlayer.unitSelected.ArrivedAtDestination (coordX, coordY);
			} 
			else 
			{
				if (unitAssigned == null || unitAssigned == GameManager.instance.activePlayer.unitSelected) 
				{
					GameManager.instance.activePlayer.unitSelected.StartMoving (Map.instance.unitMovementManager.CalculateShortestPath (GameManager.instance.activePlayer.unitSelected.originTile, this));
				}
			}
			break;
		}
	}

	public void ActivateMoveOverlay()
	{
		moveColorOverlay.SetActive (true);
	}

	public void ActivateAttackOverlay()
	{
		attackColorOverlay.SetActive (true);
	}

	public void DeactivateAllOverlays()
	{
		moveColorOverlay.SetActive (false);
		attackColorOverlay.SetActive (false);
	}

	public bool IsReachableByMovement()
	{
		return moveColorOverlay.activeSelf;
	}

	public int GetFCost()
	{
		return gCost + hCost;
	}

    public void ChangePropietary(Army newPropietary)
    {
        propietary = newPropietary;
        propietaryOverlay.color = newPropietary.assignedColor;
    }
}
