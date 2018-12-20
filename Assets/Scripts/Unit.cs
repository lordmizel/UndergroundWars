using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	SpriteRenderer mySprite;
	Animator myAnimator;
	GameManager gameManager;
	PlayerCursor cursor;
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
	List<ClickableTile> attackSpots;
	int attackIndex;
	[HideInInspector]
	public bool readyToAttack = false;

	[Header("Movement stuff")]
	public ClickableTile originTile;
	public ClickableTile possibleDestination;
	public int movementPoints = 5;
	bool unitMoving = false;
	List<ClickableTile> path;
	float moveSpeed = 5f;
	public enum typeOfMovement
	{
		FOOT,
		VEHICLE,
		FLYING,
		WATER
	}
	public typeOfMovement movementType = typeOfMovement.FOOT;

	[Header("Unit stats")]
	int hp = 10;
	int maxHP = 10;
	//TODO: Attack should be based on a table. The attack is given face-value here for debugging purposes.
	[SerializeField]
	int attack = 60;
	[SerializeField]
	int attackMultiplier = 100;
	[SerializeField]
	int defenseMultiplier = 100;


	// Use this for initialization
	void Start () {
		gameManager = FindObjectOfType<GameManager> ();
		map = FindObjectOfType<Map> ();
		mySprite = gameObject.GetComponent<SpriteRenderer> ();
		cursor = FindObjectOfType<PlayerCursor> ();
		myAnimator = gameObject.GetComponent<Animator> ();

		attackSpots = new List<ClickableTile> ();

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
		if (GameManager.gameState == GameManager.state.SELECTING_ATTACK && readyToAttack == true && gameManager.activePlayer.unitSelected == this) 
		{
			SelectEnemyToAttack ();
		}
	}

	//Unit has been selected by the cursor
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
			//TODO: What happens when you select a unit you cannot move (other player's unit or already used unit)
			gameManager.activePlayer.unitSelected = this;
			map.ActivateAttackArea(originTile.GetTileCoordX (), originTile.GetTileCoordY (), movementPoints, ranged, minAttackRange, maxAttackRange);
			GameManager.gameState = GameManager.state.CHECKING_ENEMY_UNIT;
		}
	}

	//Unit has received orders to move
	public void StartMoving(List<ClickableTile> newPath)
	{
		if (newPath != null) 
		{
			map.ReturnTilesToNormal ();
			path = newPath;
			ChangeRunningAnimation (path [path.Count - 1].GetTileCoordX (), path [path.Count - 1].GetTileCoordY ());
			unitMoving = true;
			unitHasMoved = true;
		}
	}

	//Unit is moving
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
			else 
			{
				ChangeRunningAnimation (path [path.Count - 1].GetTileCoordX (), path [path.Count - 1].GetTileCoordY ());
			}
		}
	}

	//Unit has finished moving
	public void ArrivedAtDestination(int x, int y)
	{
		if (myAnimator != null) 
		{
			transform.localScale = new Vector3 (1, 1, 1);
			myAnimator.SetTrigger ("idleState");
		}
		possibleDestination = map.GetTile (x, y);
		cursor.TeleportCursorToTile (x, y);
		if (ranged == false || unitHasMoved == false) 
		{
			List<ClickableTile> tilesInAttackRange = map.unitMovementManager.CalculateRangeMatrix (x, y, maxAttackRange, minAttackRange);
			foreach (ClickableTile tile in tilesInAttackRange) 
			{
				tile.ActivateAttackOverlay ();
				if (tile.GetUnitAssigned () != null && tile.GetUnitAssigned ().propietary != propietary) 
				{
					attackSpots.Add (tile);
					InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.ATTACK);
				}
			}
		}
		InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.WAIT);
		InGameMenu.inGameMenu.ActivateMenu ();
		GameManager.gameState = GameManager.state.NAVIGATING_MENU;
	}

	void ChangeRunningAnimation(float nextX, float nextY)
	{
		if (myAnimator != null) 
		{
			if (nextY > transform.position.y) 
			{
				myAnimator.SetTrigger ("runningUp");
			} 
			else if (nextY < transform.position.y) 
			{
				myAnimator.SetTrigger ("runningDown");
			} 
			else 
			{
				myAnimator.SetTrigger ("runningSide");
				if (nextX < transform.position.x) 
				{
					transform.localScale = new Vector3 (-1, 1, 1);
				}
				else 
				{
					transform.localScale = new Vector3 (1, 1, 1);
				}
			}
		}
	}

	//Player confirms the movement after the unit has moved and/or attacked
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

	//Player cancels the movement after the unit has moved
	public void ReturnBackToOrigin()
	{
		gameObject.transform.position = new Vector3(originTile.GetTileCoordX(), originTile.GetTileCoordY(), gameObject.transform.position.z);
		propietary.unitSelected = null;
		attackSpots.Clear ();
		unitHasMoved = false;
		map.ReturnTilesToNormal ();
		cursor.TeleportCursorToTile (originTile.GetTileCoordX (), originTile.GetTileCoordY ());
	}

	//Unit has already moved and won't be used again this turn
	public void TireUnit()
	{
		GrayUnGray (true);
		unitUsed = true;
	}

	//New turn starts, unit is refreshed
	public void RefreshUnit()
	{
		attackSpots.Clear ();
		GrayUnGray (false);
		unitHasMoved = false;
		unitUsed = false;
	}

	//Change the visual aspect of the unit
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

	public void PrepareToAttack()
	{
		attackIndex = 0;
		readyToAttack = true;
		PinpointEnemy (attackSpots [attackIndex]);
	}

	void SelectEnemyToAttack()
	{
		if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.D)) 
		{
			if (attackIndex >= attackSpots.Count - 1) 
			{
				attackIndex = 0;
			} 
			else 
			{
				attackIndex++;
			}
			PinpointEnemy (attackSpots [attackIndex]);
		} 
		else if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S)) 
		{
			if (attackIndex <= 0) 
			{
				attackIndex = attackSpots.Count - 1;
			} 
			else 
			{
				attackIndex--;
			}
			PinpointEnemy (attackSpots [attackIndex]);
		} 
		else if (Input.GetKeyDown (KeyCode.Return)) 
		{
			AttackUnit (attackSpots [attackIndex].GetUnitAssigned ());
			EstablishNewTile ();

			//TODO: Maybe should go to another state while the attack transpires before going back to moving the cursor
			GameManager.gameState = GameManager.state.AFTER_MENU_BUFFER;
		} 
		else if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			ArrivedAtDestination (possibleDestination.GetTileCoordX (), possibleDestination.GetTileCoordY ());
		}
	}

	void PinpointEnemy(ClickableTile objective)
	{
		cursor.TeleportCursorToTile (objective.GetTileCoordX (), objective.GetTileCoordY ());
	}

	void AttackUnit(Unit enemy)
	{
		readyToAttack = false;
		float rawDamage = (((attack * attackMultiplier) / 100f) + Random.Range (0, 9)) * (hp / 10f) * ((200f - (enemy.defenseMultiplier + enemy.originTile.typeOfTerrain.defensiveStat * enemy.hp)) / 100f);
		int actualDamage = (int)rawDamage / 10;
		enemy.GetDamaged (actualDamage);
		//Counter
		if (ranged == false && enemy != null && enemy.ranged == false) 
		{
			rawDamage = (((enemy.attack * enemy.attackMultiplier) / 100f) + Random.Range (0, 9)) * (enemy.hp / 10f) * ((200f - (defenseMultiplier + originTile.typeOfTerrain.defensiveStat * hp)) / 100f);
			actualDamage = (int)rawDamage / 10;
			GetDamaged (actualDamage);
		}
	}

	public void GetDamaged(int damage)
	{
		hp = hp - damage;
		Debug.Log ("Unit " + name + " is at " + hp + " hp");
		if (hp <= 0) 
		{
			hp = 0;
			//TODO: Destroy effect.
			Destroy(gameObject);
		}
	}
}
