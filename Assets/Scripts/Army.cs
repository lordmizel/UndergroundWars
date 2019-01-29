using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {

    public Color assignedColor;

	List<Unit> unitsInArmy;

	public Unit unitSelected;
    
    int warFunds = 0;
    [SerializeField]
    int fundsPerPropierty = 1000;

	// Use this for initialization
	void Start () 
	{
        unitsInArmy = new List<Unit>();
        foreach(Unit unit in GetComponentsInChildren<Unit>())
        {
            unitsInArmy.Add(unit);
        }
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

    public void EraseUnitFromArmy(Unit unit)
    {
        unitsInArmy.Remove(unit);
    }

    public void AddFunds()
    {
        warFunds = warFunds + fundsPerPropierty;
    }
}
