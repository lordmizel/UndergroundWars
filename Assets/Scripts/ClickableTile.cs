using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{

    int coordX;
    int coordY;
    public TileType typeOfTerrain;
    public string visibleName = "";
    public Sprite tileVisualPrefab;
    public enum factoryType
    {
        NONE,
        LAND,
        AIR,
        SEA
    }
    public factoryType unitCreation = factoryType.NONE;
    Unit unitAssigned;

    [SerializeField]
    GameObject moveColorOverlay;
    [SerializeField]
    GameObject attackColorOverlay;

    public Army propietary = null;
    [SerializeField]
    SpriteRenderer propietaryOverlay = null;

    //Pathfinding stuff
    public int movementCost = 1;
    public List<ClickableTile> neighbors;
    public ClickableTile parent;
    [HideInInspector]
    public int gCost;
    public int hCost;

    private void Start()
    {
        if(propietary != null)
        {
            propietaryOverlay.color = propietary.assignedColor;
        }
    }

    public void SetTileCoordinates(int x, int y)
    {
        coordX = x;
        coordY = y;
    }

    public int GetTileCoordX()
    {
        return coordX;
    }

    public int GetTileCoordY()
    {
        return coordY;
    }

    public void AssignUnit(Unit unit)
    {
        unitAssigned = unit;
    }

    public void UnassignUnit()
    {
        unitAssigned = null;
    }

    public Unit GetUnitAssigned()
    {
        return unitAssigned;
    }

    public void TileSelected()
    {
        switch (GameManager.gameState)
        {
            case GameManager.state.MOVING_CURSOR:
                if (unitAssigned != null)
                {
                    unitAssigned.UnitSelected();
                }
                else
                {
                    if(unitCreation != factoryType.NONE && propietary == GameManager.instance.activePlayer)
                    {
                        FactoryMenu.instance.ActivateFactoryMenu(unitCreation);
                    }
                    else
                    {
                        InGameMenu.inGameMenu.ShowNeutralMenu();
                    }
                }
                break;

            case GameManager.state.MOVING_UNIT:
                if (GameManager.instance.unitSelected.originTile == this)
                {
                    Map.instance.ReturnTilesToNormal();
                    GameManager.instance.unitSelected.ArrivedAtDestination(coordX, coordY);
                }
                else
                {
                    if ((unitAssigned == null || unitAssigned == GameManager.instance.unitSelected) && GameManager.instance.unitSelected.unitMoving == false)
                    {
                        GameManager.instance.unitSelected.StartMoving(Map.instance.unitMovementManager.CalculateShortestPath(GameManager.instance.unitSelected.originTile, this));
                    }
                }
                break;
        }
    }

    public void ActivateMoveOverlay()
    {
        moveColorOverlay.SetActive(true);
    }

    public void ActivateAttackOverlay()
    {
        attackColorOverlay.SetActive(true);
    }

    public void DeactivateAllOverlays()
    {
        moveColorOverlay.SetActive(false);
        attackColorOverlay.SetActive(false);
    }

    public bool IsReachableByMovement()
    {
        return moveColorOverlay.activeSelf;
    }

    public int GetFCost()
    {
        return gCost + hCost;
    }

    public void ChangePropietary(Army newPropietary)
    {
        propietary = newPropietary;
        propietaryOverlay.color = newPropietary.assignedColor;
    }
}
