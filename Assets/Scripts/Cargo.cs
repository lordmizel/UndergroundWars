using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    Unit mainUnitController;

    public List<Unit.typeOfMovement> acceptedMovementTypes;
    [SerializeField]
    int cargoSpace = 1;

    public bool unloadingUnit = false;

    
    public Unit[] cargoSlots;
    

    // Start is called before the first frame update
    void Start()
    {
        mainUnitController = gameObject.GetComponent<Unit>();
        cargoSlots = new Unit[cargoSpace];
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

    public void UnloadSelected()
    {
        unloadingUnit = true;
        CargoMenu.instance.ActivateUnitCargoMenu(cargoSlots);
    }

    public List<ClickableTile> GetUnloadArea(int unitIndex)
    {
        List<ClickableTile> validUnloadTiles = new List<ClickableTile>();
        foreach (ClickableTile tile in mainUnitController.possibleDestination.neighbors)
        {
            validUnloadTiles.Add(tile);
        }
        return validUnloadTiles;
    }

    public void UnloadUnitInTile()
    {
        Unit unitUnloaded = cargoSlots[CargoMenu.instance.buttonIndex];
        cargoSlots[CargoMenu.instance.buttonIndex] = null;
        unitUnloaded.gameObject.SetActive(true);
        unitUnloaded.transform.position = new Vector3(mainUnitController.GetSelectedTile().GetTileCoordX(), mainUnitController.GetSelectedTile().GetTileCoordY(), 0);
    }
}
