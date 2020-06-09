﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO1 : Army
{
    [SerializeField]
    int unitAtkMul_P = 10;
    [SerializeField]
    int unitDefMul_P = 10;
    [SerializeField]
    int unitAtkMul_SP = 30;
    [SerializeField]
    int unitDefMul_SP = 10;

    public override void UnitBaseModification(Unit unit)
    {
        unit.attackMultiplier = 100;
        unit.defenseMultiplier = 100;
    }

    public override void UnitSpecialPowerModification(Unit unit)
    {
        unit.attackMultiplier += unitDefMul_P;
        unit.defenseMultiplier += unitDefMul_P;
    }

    public override void UnitSuperSpecialPowerModification(Unit unit)
    {
        unit.attackMultiplier += unitDefMul_SP;
        unit.defenseMultiplier += unitDefMul_SP;
        unit.movementPoints += 1;
    }

    public override void SpecialPower()
    {
        foreach (Unit unit in unitsInArmy)
        {
            unit.ChangeHP(2);
            UnitSpecialPowerModification(unit);
            unit.poweredUpUnit = true;
        }
    }

    public override void SuperSpecialPower()
    {
        foreach (Unit unit in unitsInArmy)
        {
            unit.ChangeHP(5);
            UnitSuperSpecialPowerModification(unit);
            unit.poweredUpUnit = true;
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
            Debug.Log(unit.name);
            if (unit.poweredUpUnit)
            {
                Debug.Log(unit.name);
                UnitBaseModification(unit);
                unit.movementPoints -= 1;
                unit.poweredUpUnit = false;
            }
        }
    }
}