using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TerrainMovement : MonoBehaviour {

	Map map;
	GameManager gameManager;

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
						alreadyInspectedTiles.Contains(map.GetTile(tile.GetTileCoordX (), tile.GetTileCoordY ())) == false
						&& map.GetTile(tile.GetTileCoordX (), tile.GetTileCoordY ()).GetUnitAssigned() == null) 
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
		List<ClickableTile> path = new List<ClickableTile>();
		int[,] heuristicMatrix = new int[map.mapWidth, map.mapHeight];
		int[,] costMatrix = new int[map.mapWidth, map.mapHeight];
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
				return RetracePath (origin, destination);
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

		//It should never get here
		Debug.LogError("Something went wrong with the A* algorithm");
		return RetracePath (origin, destination);
	}

	int GetFCost(ClickableTile tile, int[,] heuristicMatrix){
		int cost = tile.movementCost + heuristicMatrix [tile.GetTileCoordX(), tile.GetTileCoordY()];
		return cost;
	}

	int[,] CreateHeuristicMatrix(ClickableTile destination)
	{
		int[,] heuristicMatrix = new int[map.mapWidth, map.mapHeight];

		for (int i = 0; i < map.mapWidth; i++) 
		{
			for (int j = 0; j < map.mapHeight; j++) 
			{
				heuristicMatrix [i, j] = Math.Abs (i - destination.GetTileCoordX()) + Math.Abs(j - destination.GetTileCoordY());
			}
		}

		return heuristicMatrix;
	}

	List<ClickableTile> RetracePath(ClickableTile origin, ClickableTile destination){
		List<ClickableTile> path = new List<ClickableTile> ();
		ClickableTile currentTile = destination;

		while (currentTile != origin) 
		{
			path.Add (currentTile);
			Debug.Log (currentTile.GetTileCoordX() + " " +  currentTile.GetTileCoordY());
			currentTile = currentTile.parent;
		}

		return path;
	}
}
