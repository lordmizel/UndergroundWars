﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	SpriteRenderer mySprite;

	GameManager gameManager;
	Map map;
	//TODO: This is public for debug purposes
	public Army propietary;

	//Delete this
	public int initialX; 
	public int initialY;

	bool unitUsed = false;
	bool unitSelected = false;

	[Header("Movement stuff")]
	public ClickableTile originTile;
	public int movementPoints = 5;
	bool unitMoving = false;
	List<ClickableTile> path;
	float moveSpeed = 5f;
	public enum typeOfMovement{
		FOOT,
		VEHICLE,
		FLYING,
		WATER
	}
	public typeOfMovement movementType = typeOfMovement.FOOT;

	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();
		map = FindObjectOfType<Map> ();
		mySprite = gameObject.GetComponent<SpriteRenderer> ();

		//TODO: This is only for debug
		originTile = map.GetTile (initialX, initialY);
		originTile.AssignUnit (this);
		gameObject.transform.position = new Vector3(originTile.transform.position.x, originTile.transform.position.y, gameObject.transform.position.z);
		////////
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (unitMoving == true) 
		{
			OnMyMerryWay ();
		}
	}

	public void UnitSelected()
	{
		if (propietary == gameManager.activePlayer && unitUsed == false) 
		{
			if (GameManager.gameState != GameManager.state.MOVING_UNIT) 
			{
				unitSelected = true;
				propietary.unitSelected = this;
				map.unitMovementManager.CalculateMovementMatrix (originTile.GetTileCoordX (), originTile.GetTileCoordY (), movementPoints);
				GameManager.gameState = GameManager.state.MOVING_UNIT;
			} 
			else 
			{
				EstablishNewTile (originTile.GetTileCoordX (), originTile.GetTileCoordY ());
			}
		} 
		else 
		{
			//TODO: What happens when you select a unit in a turn that is not it's?
		}
	}

	public void StartMoving(List<ClickableTile> newPath)
	{
		if (newPath != null) 
		{
			map.unitMovementManager.ReturnTilesToNormal ();
			path = newPath;
			unitMoving = true;
		}
	}

	void OnMyMerryWay()
	{
		Vector3 destination = new Vector3 (path [path.Count - 1].GetTileCoordX (), path [path.Count - 1].GetTileCoordY (), transform.position.z);
		if (Vector3.Distance(transform.position, destination) > 0.01f) 
		{
			transform.position = Vector3.MoveTowards (transform.position, destination, moveSpeed * Time.deltaTime);
		} 
		else 
		{
			gameObject.transform.position = new Vector3(path [path.Count - 1].GetTileCoordX (), path [path.Count - 1].GetTileCoordY (), gameObject.transform.position.z);
			path.RemoveAt (path.Count - 1);
			if (path.Count == 0) 
			{
				EstablishNewTile ((int)destination.x, (int)destination.y);
				unitMoving = false;
			}
		}
	}

	public void EstablishNewTile(int x, int y)
	{
		gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
		ClickableTile newTile = map.GetTile (x, y);
		originTile.UnassignUnit ();
		newTile.AssignUnit(this);
		originTile = newTile;
		propietary.unitSelected = null;
		//TODO: Change these last lines to show the end of movement menu first
		TireUnit();
		GameManager.gameState = GameManager.state.MOVING_CURSOR;
	}

	public void TireUnit()
	{
		GrayUnGray (true);
		unitUsed = true;
	}

	public void RefreshUnit()
	{
		GrayUnGray (false);
		unitUsed = false;
	}

	void GrayUnGray(bool gray)
	{
		float h, s, v;
		Color.RGBToHSV(mySprite.color, out h, out s, out v);
		if (gray == true) 
		{
			v = 0.5f;
		} 
		else 
		{
			v = 1f;
		}
		mySprite.color = Color.HSVToRGB (h, s, v);
	}
}
