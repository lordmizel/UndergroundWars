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
    int specialPowerSections = 6;
    [SerializeField]
    int startingFundsPerPowerSection = 9000;
    int currentMaxSpecialPower;
    int maxPowerIncrement;
    int timesPowerWasUsed = 0;
    public int currentSpecialPower = 0;
    public bool poweredUp = false;
    
    public AudioClip armyTheme;

    Vector2 lastPlaceOfCursor = new Vector2();

    [SerializeField]
    List<Unit> landUnitCatalog = new List<Unit>();
    [SerializeField]
    List<Unit> airUnitCatalog = new List<Unit>();
    [SerializeField]
    List<Unit> seaUnitCatalog = new List<Unit>();

    // Use this for initialization
    void Start () 
	{
        unitsInArmy = new List<Unit>();
        foreach(Unit unit in GetComponentsInChildren<Unit>())
        {
            unitsInArmy.Add(unit);
        }
        currentMaxSpecialPower = specialPowerSections * startingFundsPerPowerSection;
        maxPowerIncrement = startingFundsPerPowerSection * 20 / 100;
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

    public void ChangeFunds(int amount)
    {
        warFunds += amount;
        if(warFunds < 0)
        {
            warFunds = 0;
        }
        UI.instance.UpdateFundsDisplay();
    }

    public void AddPower(int value)
    {
        if (poweredUp == false)
        {
            currentSpecialPower += value;
            if (currentSpecialPower > currentMaxSpecialPower)
            {
                currentSpecialPower = currentMaxSpecialPower;
            }
            UI.instance.UpdatePowerDisplay();
        }
    }

    public int GetPower()
    {
        return currentSpecialPower;
    }

    public int GetMaxPower()
    {
        return currentMaxSpecialPower;
    }

    //TODO: Check how to activate powers
    public void ActivatePower()
    {
        currentSpecialPower = 0;
        timesPowerWasUsed++;
        if (timesPowerWasUsed <= 10) {
            currentMaxSpecialPower = currentMaxSpecialPower + (maxPowerIncrement * specialPowerSections);
        }
        UI.instance.UpdatePowerDisplay();
        poweredUp = true;
    }

    public void SetLastPlaceOfCursor(int x, int y)
    {
        lastPlaceOfCursor.x = x;
        lastPlaceOfCursor.y = y;
    }

    public Vector2 GetPlaceOfCursor()
    {
        return lastPlaceOfCursor;
    }
}
