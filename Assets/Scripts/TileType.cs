using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileType : ScriptableObject{

	public enum typeOfTerrain{
		PLAINS,
		SWAMP,
		MOUNTAIN
	}

	public typeOfTerrain terrainName;

	public GameObject tileVisualPrefab;

	public int GetMovementCost() {
		switch (terrainName) {
		case typeOfTerrain.PLAINS:
			return 1;
		case typeOfTerrain.SWAMP:
			return 2;
		case typeOfTerrain.MOUNTAIN:
			return 3;
		default:
			return 1;
		}
	}
}
