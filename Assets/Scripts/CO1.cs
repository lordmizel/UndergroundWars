﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO1 : Army
{
    [SerializeField]
    int unitAtkMullSP = 10;
    [SerializeField]
    int unitDefMullSP = 10;

    public override void UnitModification()
    {
        //CO1 has normal units overall.
        throw new System.NotImplementedException();
    }

    public override void SpecialPower()
    {
        foreach (Unit unit in unitsInArmy)
        {
            unit.ChangeHP(2);
            unit.attackMultiplier += unitDefMullSP;
            unit.defenseMultiplier += unitDefMullSP;
        }
    }

    public override void SuperSpecialPower()
    {

    }

    public override void DeactivatePower()
    {
        foreach (Unit unit in unitsInArmy)
        {
            unit.ChangeHP(2);
            unit.attackMultiplier -= unitDefMullSP;
            unit.defenseMultiplier -= unitDefMullSP;
        }
    }
}
