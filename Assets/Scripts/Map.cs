using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Map : MonoBehaviour {

    public static Map instance;

	[HideInInspector]
	public TerrainMovement unitMovementManager;

	[SerializeField]
	TileType[] tileTypes;

	int[,] tilesSeed;
	ClickableTile[,] tiles;

	public int mapWidth = 10;
	public int mapHeight = 10;

	bool movementMode = false;

    [SerializeField]
    GameObject tileMap;

	//List<ClickableTile> tilesToWorkWith = new List<ClickableTile>();

	// Use this for initialization
	void Awake () 
	{
        instance = this;
		unitMovementManager = gameObject.GetComponent<TerrainMovement> ();
		GenerateMap ();
	}

	void Start()
	{
		AssignNeighbors ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void GenerateMap()
	{
		tiles = new ClickableTile[mapWidth, mapHeight];

        foreach (ClickableTile tile in tileMap.GetComponentsInChildren<ClickableTile>())
        {
            tile.SetTileCoordinates((int)tile.transform.position.x, (int)tile.transform.position.y);
            tiles[tile.GetTileCoordX(), tile.GetTileCoordY()] = tile;
        }
	}

	void AssignNeighbors()
	{
		for (int x = 0; x < mapWidth; x++) 
		{
			for (int y = 0; y < mapHeight; y++) 
			{
				if (x > 0) 
				{
					tiles[x,y].neighbors.Add(GetTile(x - 1, y));
				}
				if (x < mapWidth - 1) 
				{
					tiles[x,y].neighbors.Add(GetTile(x + 1, y));
				}
				if (y > 0) 
				{
					tiles[x,y].neighbors.Add(GetTile(x, y - 1));
				}
				if (y < mapHeight - 1) 
				{
					tiles[x,y].neighbors.Add(GetTile(x, y + 1));
				}
			}
		}
	}

	public ClickableTile GetTile(int x, int y)
	{
		return tiles [x, y];
	}

	public void ActivateMovementArea(int x, int y, int movementPoints)
	{
		List<ClickableTile> tilesToActivate = unitMovementManager.CalculateMovementMatrix (x, y, movementPoints);
		foreach (ClickableTile tile in tilesToActivate) 
		{
			tile.ActivateMoveOverlay ();
		}
	}

	public void ActivateAttackArea(int x, int y, int movementPoints, bool ranged, int minRange, int maxRange)
	{
		if (ranged == true) 
		{
			//TODO: Unit is ranged. Calculate attack matrix
			List<ClickableTile> tilesItCanShootTo = unitMovementManager.CalculateRangeMatrix (x, y, maxRange, minRange);
			foreach (ClickableTile tile in tilesItCanShootTo) 
			{
				tile.ActivateAttackOverlay ();
			}
		} 
		else 
		{
			List<ClickableTile> tilesItCanMoveTo = unitMovementManager.CalculateMovementMatrix (x, y, movementPoints);
			foreach (ClickableTile tile in tilesItCanMoveTo) 
			{
				foreach (ClickableTile neighbor in tile.neighbors) 
				{
					neighbor.ActivateAttackOverlay ();
				}
			}
		}
	}

	public void ReturnTilesToNormal()
	{
		for (int x = 0; x < mapWidth; x++) 
		{
			for (int y = 0; y < mapHeight; y++) 
			{
				tiles[x, y].DeactivateAllOverlays ();
			}
		}
	}

    public void RecountPlayerPropierties()
    {
        foreach(ClickableTile tile in tiles)
        {
            if(tile.propietary == GameManager.instance.activePlayer)
            {
                GameManager.instance.activePlayer.AddFunds();
            }
        }
    }
}
