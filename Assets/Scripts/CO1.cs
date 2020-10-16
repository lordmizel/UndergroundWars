using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CO1 : CommandingOfficer
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
        unit.attackMultiplier += unitAtkMul_P;
        unit.defenseMultiplier += unitDefMul_P;
        unit.poweredUpUnit = true;
    }

    public override void UnitSuperSpecialPowerModification(Unit unit)
    {
        unit.attackMultiplier += unitAtkMul_SP;
        unit.defenseMultiplier += unitDefMul_SP;
        unit.movementPoints += 1;
        unit.poweredUpUnit = true;
    }

    public override void SpecialPower()
    {
        foreach (Unit unit in GameManager.instance.activePlayer.unitsInArmy)
        {
            unit.ChangeHP(2);
            UnitSpecialPowerModification(unit);
        }
    }

    public override void SuperSpecialPower()
    {
        foreach (Unit unit in GameManager.instance.activePlayer.unitsInArmy)
        {
            unit.ChangeHP(5);
            UnitSuperSpecialPowerModification(unit);
            unit.poweredUpUnit = true;
        }
    }

    public override void DeactivatePower()
    {
        foreach (Unit unit in GameManager.instance.activePlayer.unitsInArmy)
        {
            UnitBaseModification(unit);
            unit.poweredUpUnit = false;
        }
    }

    public override void DeactivateSuperPower()
    {
        foreach (Unit unit in GameManager.instance.activePlayer.unitsInArmy)
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
