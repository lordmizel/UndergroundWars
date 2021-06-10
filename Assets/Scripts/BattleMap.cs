using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattleMap : ScriptableObject
{
    public string mapName = "MapName";
    public int playerNumber = 2;
    public Sprite mapImage;
}
