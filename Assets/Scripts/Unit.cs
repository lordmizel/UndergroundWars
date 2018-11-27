using System.Collections;
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
	bool unitHasMoved = false;
	bool unitSelected = false;

	[SerializeField]
	bool ranged = false;
	[SerializeField]
	int minAttackRange = 1;
	[SerializeField]
	int maxAttackRange = 1;

	[Header("Movement stuff")]
	public ClickableTile originTile;
	public ClickableTile possibleDestination;
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
				map.ActivateMovementArea (originTile.GetTileCoordX (), originTile.GetTileCoordY (), movementPoints);
				GameManager.gameState = GameManager.state.MOVING_UNIT;
			} 
			else 
			{
				ArrivedAtDestination (originTile.GetTileCoordX (), originTile.GetTileCoordY ());
			}
		} 
		else 
		{
			//TODO: What happens when you select a unit in a turn that is not it's?
			gameManager.activePlayer.unitSelected = this;
			map.ActivateAttackArea(originTile.GetTileCoordX (), originTile.GetTileCoordY (), movementPoints, ranged, minAttackRange, maxAttackRange);
			GameManager.gameState = GameManager.state.CHECKING_ENEMY_UNIT;
		}
	}

	public void StartMoving(List<ClickableTile> newPath)
	{
		if (newPath != null) 
		{
			map.ReturnTilesToNormal ();
			path = newPath;
			unitMoving = true;
			unitHasMoved = true;
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
				ArrivedAtDestination ((int)destination.x, (int)destination.y);
				unitMoving = false;
			}
		}
	}

	public void ArrivedAtDestination(int x, int y)
	{
		possibleDestination = map.GetTile (x, y);
		if (ranged == false || unitHasMoved == false) {
			List<ClickableTile> tilesInAttackRange = map.unitMovementManager.CalculateRangeMatrix (x, y, maxAttackRange, minAttackRange);
			foreach (ClickableTile tile in tilesInAttackRange) 
			{
				tile.ActivateAttackOverlay ();
				if (tile.GetUnitAssigned () != null && tile.GetUnitAssigned ().propietary != propietary) 
				{
					InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.ATTACK);
				}
			}
		}
		InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.WAIT);
		InGameMenu.inGameMenu.ActivateMenu ();
		GameManager.gameState = GameManager.state.NAVIGATING_MENU;
	}

	public void EstablishNewTile()
	{ 
		ClickableTile newTile = possibleDestination;

		gameObject.transform.position = new Vector3(newTile.GetTileCoordX(), newTile.GetTileCoordY(), gameObject.transform.position.z);
		originTile.UnassignUnit ();
		newTile.AssignUnit(this);
		originTile = newTile;
		propietary.unitSelected = null;
		TireUnit();
		map.ReturnTilesToNormal ();
	}

	public void ReturnBackToOrigin()
	{
		gameObject.transform.position = new Vector3(originTile.GetTileCoordX(), originTile.GetTileCoordY(), gameObject.transform.position.z);
		propietary.unitSelected = null;
		map.ReturnTilesToNormal ();
	}

	public void TireUnit()
	{
		GrayUnGray (true);
		unitUsed = true;
	}

	public void RefreshUnit()
	{
		GrayUnGray (false);
		unitHasMoved = false;
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
