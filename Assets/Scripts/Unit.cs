using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	Map map;
	TerrainMovement movementManager;
	Army army;

	bool unitSelected = false;

	[Header("Movement stuff")]
	public ClickableTile originTile;
	[SerializeField]
	int movementPoints = 5;

	//Delete this
	public int initialX; 
	public int initialY;

	// Use this for initialization
	void Start () {
		map = FindObjectOfType<Map> ();
		army = FindObjectOfType<Army> ();

		//TODO: This is only for debug
		originTile = map.GetTile (initialX, initialY);
		originTile.AssignUnit (this);
		gameObject.transform.position = new Vector3(originTile.transform.position.x, originTile.transform.position.y, gameObject.transform.position.z);
		////////
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*void OnMouseDown()
	{
		if (GameManager.gameState != GameManager.state.MOVING_UNIT) {
			unitSelected = true;
			army.unitSelected = this;
			map.CalculateMovementMatrix (originTile.GetTileCoordX (), originTile.GetTileCoordY (), movementPoints);
			GameManager.gameState = GameManager.state.MOVING_UNIT;
		} 
		else 
		{
			MoveUnitTo (originTile.GetTileCoordX(), originTile.GetTileCoordY());
		}
	}*/

	public void UnitSelected()
	{
		if (GameManager.gameState != GameManager.state.MOVING_UNIT) 
		{
			unitSelected = true;
			army.unitSelected = this;
			map.CalculateMovementMatrix (originTile.GetTileCoordX (), originTile.GetTileCoordY (), movementPoints);
			GameManager.gameState = GameManager.state.MOVING_UNIT;
		} 
		else 
		{
			MoveUnitTo (originTile.GetTileCoordX(), originTile.GetTileCoordY());
		}
	}

	public void MoveUnitTo(int x, int y)
	{
		gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
		ClickableTile newTile = map.GetTile (x, y);
		originTile.UnassignUnit ();
		newTile.AssignUnit(this);
		originTile = newTile;
		army.unitSelected = null;
		map.ReturnTilesToNormal ();
		GameManager.gameState = GameManager.state.MOVING_CURSOR;
	}


}
