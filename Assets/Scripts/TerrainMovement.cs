using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainMovement : MonoBehaviour {

	Map map;
	GameManager gameManager;

	int[,] movementCostMatrix = new int[,] { {1, 2, 1, 100},
		{1, 1, 1, 100},
		{2, 100, 1, 100},
		{2, 3, 1, 100},
		{2, 100, 1, 100},
		{1, 3, 1, 100},
		{100, 100, 1, 1},
		{100, 100, 1, 2}};

	// Use this for initialization
	void Start () 
	{
		gameManager = FindObjectOfType<GameManager> ();
		map = gameObject.GetComponent<Map> ();

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void ActivateMovementOverlay()
	{
		gameManager.activePlayer.unitSelected.originTile.ActivateMoveOverlay ();
	}

	public void ReturnTilesToNormal()
	{
		for (int x = 0; x < map.mapWidth; x++) 
		{
			for (int y = 0; y < map.mapHeight; y++) 
			{
				map.GetTile(x, y).DeactivateAllOverlays ();
			}
		}
	}

	public void CalculateMovementMatrix(int x, int y, int movementPoints)
	{
		int[,] costMatrix = new int[map.mapWidth, map.mapHeight];
		List<ClickableTile> pendingTiles = new List<ClickableTile>();
		List<ClickableTile> alreadyInspectedTiles = new List<ClickableTile>();
		costMatrix [x, y] = 0;
		pendingTiles.Add (map.GetTile(x, y));
		map.GetTile(x, y).ActivateMoveOverlay ();

		while (pendingTiles.Count > 0) 
		{
			foreach (ClickableTile tile in pendingTiles[0].neighbors) 
			{
				if (costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] == 0 || 
					costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] > costMatrix [pendingTiles[0].GetTileCoordX (), pendingTiles[0].GetTileCoordY ()] + GetCostForMovementType((int)gameManager.activePlayer.unitSelected.movementType, (int)tile.typeOfTerrain.terrainName)) 
				{
						costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] = costMatrix [pendingTiles [0].GetTileCoordX (), pendingTiles [0].GetTileCoordY ()] + GetCostForMovementType((int)gameManager.activePlayer.unitSelected.movementType, (int)tile.typeOfTerrain.terrainName);
					if (costMatrix [tile.GetTileCoordX (), tile.GetTileCoordY ()] <= movementPoints && 
						alreadyInspectedTiles.Contains(map.GetTile(tile.GetTileCoordX (), tile.GetTileCoordY ())) == false
						&& (map.GetTile(tile.GetTileCoordX (), tile.GetTileCoordY ()).GetUnitAssigned() == null
						|| map.GetTile(tile.GetTileCoordX (), tile.GetTileCoordY ()).GetUnitAssigned().propietary == gameManager.activePlayer.unitSelected.propietary)) 
					{
						pendingTiles.Add (map.GetTile(tile.GetTileCoordX (), tile.GetTileCoordY ()));
						map.GetTile(tile.GetTileCoordX (), tile.GetTileCoordY ()).ActivateMoveOverlay ();
					}
				}
			}
			alreadyInspectedTiles.Add (pendingTiles [0]);
			pendingTiles.RemoveAt (0);
		}
	}

	public List<ClickableTile> CalculateShortestPath(ClickableTile origin, ClickableTile destination)
	{
		List<ClickableTile> pendingTiles = new List<ClickableTile> ();
		List<ClickableTile> alreadyInspectedTiles = new List<ClickableTile> ();
		pendingTiles.Add (origin);

		while (pendingTiles.Count > 0) 
		{
			ClickableTile currentTile = pendingTiles [0];
			for (int i = 0; i < pendingTiles.Count; i++) 
			{
				if (pendingTiles[i].GetFCost() < currentTile.GetFCost() ||
					pendingTiles [i].GetFCost() == currentTile.GetFCost() && pendingTiles[i].hCost < currentTile.hCost) 
				{
					currentTile = pendingTiles [i];
				}
			}
			pendingTiles.Remove (currentTile);
			alreadyInspectedTiles.Add (currentTile);

			if (currentTile == destination) 
			{
				return RetracePath (origin, destination);
			}

			foreach (ClickableTile neighbor in currentTile.neighbors) 
			{
				if(alreadyInspectedTiles.Contains(neighbor) || 
					neighbor.GetUnitAssigned() != null && neighbor.GetUnitAssigned().propietary != gameManager.activePlayer.unitSelected.propietary)
				{
					continue;
				}

				int newMovementCostToNeighbor = currentTile.gCost + GetCostForMovementType((int)gameManager.activePlayer.unitSelected.movementType, (int)neighbor.typeOfTerrain.terrainName);
				if (newMovementCostToNeighbor < neighbor.gCost ||
					pendingTiles.Contains (neighbor) == false) 
				{
					neighbor.gCost = newMovementCostToNeighbor;
					neighbor.hCost = GetHCost (neighbor, destination);
					neighbor.parent = currentTile;
					if (pendingTiles.Contains (neighbor) == false) 
					{
						pendingTiles.Add (neighbor);
					}
				}
			}
		}

		//It should never get here
		Debug.LogError("Something went wrong with the A* algorithm");
		return null;
	}

	int GetHCost(ClickableTile origin, ClickableTile destination)
	{
		return Math.Abs (origin.GetTileCoordX() - destination.GetTileCoordX ()) + Math.Abs (origin.GetTileCoordY() - destination.GetTileCoordY ());
	}

	int GetCostForMovementType(int movementType, int terrainType)
	{
		return movementCostMatrix [terrainType, movementType];
	}

	List<ClickableTile> RetracePath(ClickableTile origin, ClickableTile destination){
		List<ClickableTile> path = new List<ClickableTile> ();
		ClickableTile currentTile = destination;

		while (currentTile != origin) 
		{
			path.Add (currentTile);
			currentTile = currentTile.parent;
		}

		return path;
	}
}
