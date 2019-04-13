using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour {

    public Color assignedColor;

	List<Unit> unitsInArmy;
    
    int warFunds = 0;
    [SerializeField]
    int fundsPerPropierty = 1000;

    [SerializeField]
    int maxSpecialPower;
    int currentSpecialPower = 0;
    
    public AudioClip armyTheme;

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

        UI.instance.UpdateFundsDisplay();
    }

    public int GetFunds()
    {
        return warFunds;
    }

    public void AddPower(int value)
    {
        currentSpecialPower += value;
        if(currentSpecialPower > maxSpecialPower)
        {
            currentSpecialPower = maxSpecialPower;
        }
        UI.instance.UpdatePowerDisplay();
    }

    public int GetPower()
    {
        return currentSpecialPower;
    }

    public int GetMaxPower()
    {
        return maxSpecialPower;
    }

    //TODO: Check how to activate powers
    public void ActivatePower()
    {
        currentSpecialPower = 0;
        UI.instance.UpdatePowerDisplay();
    }
}
