using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Army : MonoBehaviour {

    public CommandingOfficer COIdentity;
    public Color assignedColor;

	internal List<Unit> unitsInArmy;
    
    int warFunds = 0;
    [SerializeField]
    int startingFundsPerPowerSection = 9000;
    int currentMaxSpecialPower;
    [HideInInspector]
    public int mediumPowerThreshold;
    int maxPowerIncrement;
    int timesPowerWasUsed = 0;
    public int currentSpecialPower = 0;
    // armyPowerLevel variable: 0 = normal; 1 = normal power active; 2 = super power active
    public int armyPowerLevel = 0;

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
            UnitBaseModification(unit);
        }
        currentMaxSpecialPower = COIdentity.specialPowerSections * startingFundsPerPowerSection;
        mediumPowerThreshold = currentMaxSpecialPower * COIdentity.minorPowerPercentage / 100;
        maxPowerIncrement = startingFundsPerPowerSection * 20 / 100;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void StartNewTurn()
    {
        RefreshAllUnits();
        switch (armyPowerLevel)
        {
            case 1:
                DeactivatePower();
                armyPowerLevel = 0;
                break;
            case 2:
                DeactivateSuperPower();
                armyPowerLevel = 0;
                break;
            default:
                break;
        }
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

    public void AddUnitToArmy(Unit unit)
    {
        unitsInArmy.Add(unit);
        unit.SetPropietary(this);
        UnitBaseModification(unit);
        if(armyPowerLevel == 1)
        {
            UnitSpecialPowerModification(unit);
        }
        else if (armyPowerLevel == 2)
        {
            UnitSuperSpecialPowerModification(unit);
        }
    }

    public void AddFunds()
    {
        warFunds = warFunds + COIdentity.fundsPerPropierty;

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
        if (armyPowerLevel == 0)
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
    
    public void ActivatePower(int powerLevel)
    {
        armyPowerLevel = powerLevel;
        switch (armyPowerLevel)
        {
            case 1:
                currentSpecialPower -= mediumPowerThreshold;
                SpecialPower();
                break;
            case 2:
                currentSpecialPower = 0;
                SuperSpecialPower();
                break;
            default:
                break;
        }
        timesPowerWasUsed++;
        if (timesPowerWasUsed <= 10) {
            currentMaxSpecialPower = currentMaxSpecialPower + (maxPowerIncrement * COIdentity.specialPowerSections);
            mediumPowerThreshold = currentMaxSpecialPower * COIdentity.minorPowerPercentage / 100;
        }
        UI.instance.UpdatePowerDisplay();
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

    public void FillFactory(ClickableTile.factoryType factoryType)
    {
        List<Unit> catalogToShow = new List<Unit>();
        switch (factoryType)
        {
            case ClickableTile.factoryType.LAND:
                catalogToShow = landUnitCatalog;
                break;
            case ClickableTile.factoryType.AIR:
                catalogToShow = airUnitCatalog;
                break;
            case ClickableTile.factoryType.SEA:
                catalogToShow = seaUnitCatalog;
                break;
            default:
                Debug.Log("ERROR: Non-factory tile recognized as factory.");
                break;
        }

        for (int i = 0; i < catalogToShow.Count; i++)
        { 
            FactoryMenu.instance.FillOption(i, catalogToShow[i]);
        }
    }

    public abstract void UnitBaseModification(Unit unit);
    public abstract void UnitSpecialPowerModification(Unit unit);
    public abstract void UnitSuperSpecialPowerModification(Unit unit);
    public abstract void SpecialPower();
    public abstract void SuperSpecialPower();
    public abstract void DeactivatePower();
    public abstract void DeactivateSuperPower();
    
}
