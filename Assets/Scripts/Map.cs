using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Map : MonoBehaviour {

	GameManager gameManager;

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

	public void ActivateMovementOverlay()
	{
		gameManager.activePlayer.unitSelected.originTile.ActivateMoveOverlay ();
	}

	public void ReturnTilesToNormal()
	{
		for (int x = 0; x < mapWidth; x++) 
		{
			for (int y = 0; y < mapHeight; y++) 
			{
				tiles [x, y].DeactivateAllOverlays ();
			}
		}
	}

	public void CalculateMovementMatrix(int x, int y, int movementPoints)
	{
		int[,] costMatrix = new int[mapWidth, mapHeight];
		List<ClickableTile> pendingTiles = new List<ClickableTile>();
		List<ClickableTile> alreadyInspectedTiles = new List<ClickableTile>();
		costMatrix [x, y] = 0;
		pendingTiles.Add (tiles [x, y]);
		tiles [x, y].ActivateMoveOverlay ();
		Debug.Log (pendingTiles.Count);

		while (pendingTiles.Count > 0) 
		{
			foreach (ClickableTile tile in pendingTiles[0].neighbors) 
			{
				if (costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] == 0 || 
					costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] > costMatrix [pendingTiles[0].GetTileCoordX (), pendingTiles[0].GetTileCoordY ()] + tile.movementCost) 
				{
					costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] = costMatrix [pendingTiles [0].GetTileCoordX (), pendingTiles [0].GetTileCoordY ()] + tile.movementCost;
					if (costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] <= movementPoints && 
						alreadyInspectedTiles.Contains(tiles [tile.GetTileCoordX (), tile.GetTileCoordY ()]) == false
						&& tiles [tile.GetTileCoordX (), tile.GetTileCoordY ()].GetUnitAssigned() == null) 
					{
						pendingTiles.Add (tiles [tile.GetTileCoordX (), tile.GetTileCoordY ()]);
						tiles [tile.GetTileCoordX (), tile.GetTileCoordY ()].ActivateMoveOverlay ();
					}
				}
			}
			alreadyInspectedTiles.Add (pendingTiles [0]);
			pendingTiles.RemoveAt (0);
		}
	}

	public void /*ClickableTile[]*/ CalculateShortestPath(ClickableTile origin, ClickableTile destination)
	{
		ClickableTile[] path;
		int[,] heuristicMatrix = new int[mapWidth, mapHeight];
		int[,] costMatrix = new int[mapWidth, mapHeight];
		List<ClickableTile> pendingTiles = new List<ClickableTile> ();
		List<ClickableTile> alreadyInspectedTiles = new List<ClickableTile> ();
		pendingTiles.Add (origin);
		costMatrix [origin.GetTileCoordX (), origin.GetTileCoordY ()] = 0;

		heuristicMatrix = CreateHeuristicMatrix (destination);

		while (pendingTiles.Count > 0) 
		{
			ClickableTile currentTile = pendingTiles [0];
			for (int i = 0; i < pendingTiles.Count; i++) 
			{
				if (GetFCost (pendingTiles [i], heuristicMatrix) < GetFCost (currentTile, heuristicMatrix) ||
					GetFCost (pendingTiles [i], heuristicMatrix) == GetFCost (currentTile, heuristicMatrix) && heuristicMatrix [pendingTiles [i].GetTileCoordX(), pendingTiles [i].GetTileCoordY()] < heuristicMatrix [currentTile.GetTileCoordX(), currentTile.GetTileCoordY()]) 
				{
					currentTile = pendingTiles [i];
				}
			}
			pendingTiles.Remove (currentTile);
			alreadyInspectedTiles.Add (currentTile);

			if (currentTile == destination) 
			{
				RetracePath (origin, destination);
				return;
			}

			foreach (ClickableTile neighbor in currentTile.neighbors) 
			{
				if(alreadyInspectedTiles.Contains(neighbor))
				{
					continue;
				}

				int newMovementCostToNeighbor = costMatrix [currentTile.GetTileCoordX (), currentTile.GetTileCoordY ()] + neighbor.movementCost;
				if (newMovementCostToNeighbor < costMatrix [neighbor.GetTileCoordX (), neighbor.GetTileCoordY ()] ||
					pendingTiles.Contains (neighbor) == false) 
				{
					costMatrix [neighbor.GetTileCoordX (), neighbor.GetTileCoordY ()] = newMovementCostToNeighbor;
					neighbor.parent = currentTile;
					if (pendingTiles.Contains (neighbor) == false) 
					{
						pendingTiles.Add (neighbor);
					}
				}
			}
		}
	}

	int GetFCost(ClickableTile tile, int[,] heuristicMatrix){
		int cost = tile.movementCost + heuristicMatrix [tile.GetTileCoordX(), tile.GetTileCoordY()];
		return cost;
	}

	int[,] CreateHeuristicMatrix(ClickableTile destination)
	{
		int[,] heuristicMatrix = new int[mapWidth, mapHeight];

		for (int i = 0; i < mapWidth; i++) 
		{
			for (int j = 0; j < mapHeight; j++) 
			{
				heuristicMatrix [i, j] = Math.Abs (i - destination.GetTileCoordX()) + Math.Abs(j - destination.GetTileCoordY());
			}
		}

		return heuristicMatrix;
	}

	void RetracePath(ClickableTile origin, ClickableTile destination){
		List<ClickableTile> path = new List<ClickableTile> ();
		ClickableTile currentTile = destination;

		while (currentTile != origin) 
		{
			path.Add (currentTile);
			Debug.Log (currentTile.GetTileCoordX() + " " +  currentTile.GetTileCoordY());
			currentTile = currentTile.parent;
		}


	}

	/*class Location
	{
		public int X;
		public int Y;
		public int F;
		public int G;
		public int H;
		public Location Parent;
	}

	ClickableTile[] CalulateShortestPath(int x_origin, int y_origin, int x_destination, int y_destination){
		List<ClickableTile> path = new List<ClickableTile>();

		Location current = null;
		var start = new Location { X = x_origin, Y = y_origin };
		var target = new Location { X = x_destination, Y = y_destination };
		var openList = new List<Location>();
		var closedList = new List<Location>();
		int g = 0;

		// start by adding the original position to the open list
		openList.Add(start);

		while (openList.Count > 0)
		{
			// get the square with the lowest F score
			var lowest = openList.Min(l => l.F);
			current = openList.First(l => l.F == lowest);

			// add the current square to the closed list
			closedList.Add(current);

			// remove it from the open list
			openList.Remove(current);

			// if we added the destination to the closed list, we've found a path
			if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
				break;

			var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y);
			g++;

			foreach(var adjacentSquare in adjacentSquares)
			{
				// if this adjacent square is already in the closed list, ignore it
				if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
					&& l.Y == adjacentSquare.Y) != null)
					continue;

				// if it's not in the open list...
				if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
					&& l.Y == adjacentSquare.Y) == null)
				{
					// compute its score, set the parent
					adjacentSquare.G = g;
					adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
					// Modificación del algoritmo A* para que tenga en cuenta el tipo de terreno de cada celda
					adjacentSquare.F = adjacentSquare.G + adjacentSquare.H + tiles[adjacentSquare.X, adjacentSquare.Y].typeOfTerrain.GetMovementCost();
					adjacentSquare.Parent = current;

					// and add it to the open list
					openList.Insert(0, adjacentSquare);
				}
				else
				{
					// test if using the current G score makes the adjacent square's F score
					// lower, if yes update the parent because it means it's a better path
					if (g + adjacentSquare.H < adjacentSquare.F)
					{
						adjacentSquare.G = g;
						adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
						adjacentSquare.Parent = current;
					}
				}
			}
		}

		// Volcar el current y todos sus padres en la lista l
		while (current != null) {
			ClickableTile tile = new ClickableTile();
			tile.SetTileCoordinates (current.X, current.Y);
			path.Add(tile);
			current = current.Parent;
		}

		return path.ToArray();
	}

	List<Location> GetWalkableAdjacentSquares(int x, int y)
	{
		var proposedLocations = new List<Location>()
		{
			new Location { X = x, Y = y - 1 },
			new Location { X = x, Y = y + 1 },
			new Location { X = x - 1, Y = y },
			new Location { X = x + 1, Y = y },
		};

		return proposedLocations.Where(l => tiles[l.X, l.Y].typeOfTerrain.terrainName == TileType.typeOfTerrain.MOUNTAIN || 
			tiles[l.X, l.Y].typeOfTerrain.terrainName == TileType.typeOfTerrain.PLAINS ||
			tiles[l.X, l.Y].typeOfTerrain.terrainName == TileType.typeOfTerrain.SWAMP).ToList();
	}

	static int ComputeHScore(int x, int y, int targetX, int targetY)
	{
		return Math.Abs(targetX - x) + Math.Abs(targetY - y);
	}*/
		
}
