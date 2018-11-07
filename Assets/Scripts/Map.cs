using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Map : MonoBehaviour {

	GameManager gameManager;
	[HideInInspector]
	public TerrainMovement unitMovementManager;

	[SerializeField]
	TileType[] tileTypes;

	int[,] tilesSeed;
	ClickableTile[,] tiles;


	public int mapWidth = 10;
	public int mapHeight = 10;

	bool movementMode = false; 

	// Use this for initialization
	void Awake () 
	{
		gameManager = FindObjectOfType<GameManager> ();
		unitMovementManager = gameObject.GetComponent<TerrainMovement> ();
		EstablishMapData ();
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

		for (int x = 0; x < mapWidth; x++) 
		{
			for (int y = 0; y < mapHeight; y++) 
			{
				TileType typeOfTile = tileTypes[tilesSeed [x, y]];
				GameObject tile = (GameObject)Instantiate (typeOfTile.tileVisualPrefab, new Vector3 (x, y, 0), Quaternion.identity, gameObject.transform);
				ClickableTile tileData = tile.GetComponent<ClickableTile> ();
				tileData.SetTileCoordinates (x, y);
				tileData.typeOfTerrain = typeOfTile;
				tiles [x, y] = tileData;
			}
		}
	}

	void EstablishMapData()
	{
		tilesSeed = new int[mapWidth, mapHeight];

		for (int x = 0; x < mapWidth; x++) 
		{
			for (int y = 0; y < mapHeight; y++) 
			{
				tilesSeed [x, y] = (int)TileType.typeOfTerrain.PLAINS;
			}
		}

		tilesSeed [5, 5] = (int)TileType.typeOfTerrain.MOUNTAIN;
		tilesSeed [5, 6] = (int)TileType.typeOfTerrain.MOUNTAIN;
		tilesSeed [5, 7] = (int)TileType.typeOfTerrain.MOUNTAIN;
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
		
}
