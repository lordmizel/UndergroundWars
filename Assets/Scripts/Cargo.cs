using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    public List<Unit.typeOfMovement> acceptedMovementTypes;
    [SerializeField]
    int cargoSpace = 1;

    public Unit[] cargoSlots;

    CargoMenu cargoMenu;

    // Start is called before the first frame update
    void Start()
    {
        cargoSlots = new Unit[cargoSpace];
        cargoMenu = FindObjectOfType<CargoMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HasFreeSlots()
    {
        foreach(Unit slot in cargoSlots)
        {
            if(slot == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool HasUnitsLoaded()
    {
        foreach (Unit slot in cargoSlots)
        {
            if (slot != null)
            {
                return true;
            }
        }
        return false;
    }

    public void LoadUnit(Unit unit)
    {
        for(int i = 0; i < cargoSlots.Length; i++)
        {
            if (cargoSlots[i] == null)
            {
                cargoSlots[i] = unit;
                break;
            }
        }
    }

    public void BeginUnload()
    {
        cargoMenu.ActivateUnitCargoMenu(cargoSlots);
    }
}
