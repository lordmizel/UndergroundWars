using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMovement : MonoBehaviour {

	Map map;

	// Use this for initialization
	void Start () 
	{
		map = gameObject.GetComponent<Map> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void CalculateMovementMatrix(int x, int y, int movementPoints)
	{
		int[,] costMatrix = new int[map.mapWidth, map.mapHeight];
		List<ClickableTile> pendingTiles = new List<ClickableTile>();
		List<ClickableTile> alreadyInspectedTiles = new List<ClickableTile>();
		costMatrix [x, y] = 0;
		pendingTiles.Add (map.GetTile(x, y));
		map.GetTile (x, y).ActivateMoveOverlay ();
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
}
