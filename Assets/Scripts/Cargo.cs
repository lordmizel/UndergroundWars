using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    Unit mainUnitController;

    public List<Unit.typeOfMovement> acceptedMovementTypes;
    [SerializeField]
    int cargoSpace = 1;

    
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
        CargoMenu.instance.ActivateUnitCargoMenu(cargoSlots);
    }

    public void GetUnloadArea(int unitIndex)
    {
        List<ClickableTile> validUnloadTiles = new List<ClickableTile>();
        foreach (ClickableTile tile in mainUnitController.possibleDestination.neighbors)
        {
            validUnloadTiles.Add(tile);
        }
        int objectiveIndex = 0;
        //readyToLoad = true;
        //interactableObjectives = validUnloadTiles;
        //GetMyAttackRange();
        PlayerCursor.instance.PinPointTile(validUnloadTiles[objectiveIndex]);
        GameManager.gameState = GameManager.state.SELECTING_OBJECTIVE;
    }
}
