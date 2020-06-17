using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO2 : Army
{
    [SerializeField]
    int unitAtkMul_P = 20;
    [SerializeField]
    int unitDefMul_P = 10;
    [SerializeField]
    int unitAtkMul_SP = 40;
    [SerializeField]
    int unitDefMul_SP = 10;

    public override void UnitBaseModification(Unit unit)
    {
        if(unit.ranged)
        {
            unit.attackMultiplier = 90;
            if (!unit.poweredUpUnit)
            {
                unit.maxAttackRange -= 1;
            }
        }
        else if (!unit.canCapture)
        {
            unit.attackMultiplier = 120;
        }
        unit.defenseMultiplier = 100;
    }

    public override void UnitSpecialPowerModification(Unit unit)
    {
        if(!unit.ranged && !unit.canCapture)
        {
            unit.attackMultiplier += unitAtkMul_P;
        }
        unit.defenseMultiplier += unitDefMul_P;
        unit.poweredUpUnit = true;
    }

    public override void UnitSuperSpecialPowerModification(Unit unit)
    {
        if (!unit.ranged && !unit.canCapture)
        {
            unit.attackMultiplier += unitAtkMul_SP;
        }
        unit.defenseMultiplier += unitDefMul_P;
        unit.poweredUpUnit = true;
    }

    public override void SpecialPower()
    {
        foreach (Unit unit in unitsInArmy)
        {
            UnitSpecialPowerModification(unit);
        }
    }

    public override void SuperSpecialPower()
    {
        foreach (Unit unit in unitsInArmy)
        {
            UnitSuperSpecialPowerModification(unit);
        }
    }
    
    public override void DeactivatePower()
    {
        foreach (Unit unit in unitsInArmy)
        {
            UnitBaseModification(unit);
            unit.poweredUpUnit = false;
        }
    }

    public override void DeactivateSuperPower()
    {
        foreach (Unit unit in unitsInArmy)
        {

            UnitBaseModification(unit);
            unit.poweredUpUnit = false;
        }
    }
}
