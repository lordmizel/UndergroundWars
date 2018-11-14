using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {

	Unit[] unitsInArmy;

	public Unit unitSelected;

	// Use this for initialization
	void Start () 
	{
		unitsInArmy = GetComponentsInChildren<Unit> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RefreshAllUnits()
	{
		foreach (Unit unit in unitsInArmy) 
		{
			unit.RefreshUnit ();
		}
	}
}
