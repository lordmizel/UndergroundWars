using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommandingOfficer : MonoBehaviour
{
    public AudioClip armyTheme;

    public int specialPowerSections = 6;
    public int minorPowerPercentage = 50;
    
    public int fundsPerPropierty = 1000;
    
    public List<Unit> landUnitCatalog = new List<Unit>();
    public List<Unit> airUnitCatalog = new List<Unit>();
    public List<Unit> seaUnitCatalog = new List<Unit>();
    
    public abstract void UnitBaseModification(Unit unit);
    public abstract void UnitSpecialPowerModification(Unit unit);
    public abstract void UnitSuperSpecialPowerModification(Unit unit);
    public abstract void SpecialPower();
    public abstract void SuperSpecialPower();
    public abstract void DeactivatePower();
    public abstract void DeactivateSuperPower();
}
