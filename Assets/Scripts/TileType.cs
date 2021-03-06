﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TileType : ScriptableObject{

	public enum typeOfTerrain{
		PLAINS,
		ROAD,
		RIVER,
		FOREST,
		MOUNTAIN,
		SAND,
		WATER,
		REEF,
        PORT
	}
    
	public typeOfTerrain terrainName = typeOfTerrain.PLAINS;
    
    [Header("Public tile data")]
    public int defensiveStat = 1;

    public bool capturable = false;

	public int GetMovementCost() {
		switch (terrainName) {
		case typeOfTerrain.PLAINS:
			return 1;
		case typeOfTerrain.FOREST:
			return 2;
		case typeOfTerrain.MOUNTAIN:
			return 3;
		default:
			return 1;
		}
	}
}
