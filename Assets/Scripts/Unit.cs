using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    SpriteRenderer mySprite;
    Animator myAnimator;
    PlayerCursor cursor;
    [SerializeField]
    SpriteRenderer hpSprite;
    [SerializeField]
    Sprite[] hpNumbers;

    //TODO: This is public for debug purposes
    public Army propietary;

    //TODO: Delete this
    public int initialX;
    public int initialY;

    bool unitUsed = false;
    bool unitHasMoved = false;
    bool unitSelected = false;

    [SerializeField]
    bool ranged = false;
    [SerializeField]
    int minAttackRange = 1;
    [SerializeField]
    int maxAttackRange = 1;
    List<ClickableTile> attackSpots;
    List<ClickableTile> loadSpots;
    public List<ClickableTile> interactableObjectives;
    int objectiveIndex;
    [HideInInspector]
    public bool readyToAttack = false;

    [Header("Movement stuff")]
    public ClickableTile originTile;
    public ClickableTile possibleDestination;
    public int movementPoints = 5;
    [HideInInspector]
    public bool unitMoving = false;
    List<ClickableTile> path;
    float moveSpeed = 5f;
    public enum typeOfMovement
    {
        FOOT,
        VEHICLE,
        FLYING,
        WATER
    }
    public typeOfMovement movementType = typeOfMovement.FOOT;

    public int moneyValue = 1000;

    [Header("Unit stats")]
    int hp = 10;
    int maxHP = 10;
    //TODO: Attack should be based on a table. The attack is given face-value here for debugging purposes.
    [SerializeField]
    int attack = 60;
    [SerializeField]
    int attackMultiplier = 100;
    [SerializeField]
    int defenseMultiplier = 100;

    [Header("Capturing stuff")]
    [SerializeField]
    bool canCapture = false;
    int capturePoints = 0;
    bool decidedToCapture = false;

    [Header("Cargo stuff")]
    [HideInInspector]
    public bool transportUnit = false;
    [HideInInspector]
    public bool readyToLoad = false;


    // Use this for initialization
    void Start()
    {
        mySprite = gameObject.GetComponent<SpriteRenderer>();
        cursor = FindObjectOfType<PlayerCursor>();
        myAnimator = gameObject.GetComponent<Animator>();

        attackSpots = new List<ClickableTile>();
        loadSpots = new List<ClickableTile>();
        interactableObjectives = new List<ClickableTile>();

        if(GetComponent<Cargo>() != null)
        {
            transportUnit = true;
        }

        //TODO: This is only for debug
        originTile = Map.instance.GetTile(initialX, initialY);
        originTile.AssignUnit(this);
        gameObject.transform.position = new Vector3(originTile.transform.position.x, originTile.transform.position.y, gameObject.transform.position.z);
        ////////
    }

    // Update is called once per frame
    void Update()
    {
        if (unitMoving == true)
        {
            OnMyMerryWay();
        }
        if (GameManager.gameState == GameManager.state.SELECTING_OBJECTIVE && GameManager.instance.unitSelected == this)
        {
            SelectObjective();
        }
    }

    //Unit has been selected by the cursor
    public void UnitSelected()
    {
        if (propietary == GameManager.instance.activePlayer && unitUsed == false)
        {
            if (GameManager.gameState != GameManager.state.MOVING_UNIT)
            {
                unitSelected = true;
                GameManager.instance.unitSelected = this;
                Map.instance.ActivateMovementArea(originTile.GetTileCoordX(), originTile.GetTileCoordY(), movementPoints);
                GameManager.gameState = GameManager.state.MOVING_UNIT;
            }
            else
            {
                ArrivedAtDestination(originTile.GetTileCoordX(), originTile.GetTileCoordY());
            }
        }
        else
        {
            //TODO: What happens when you select a unit you cannot move (other player's unit or already used unit)
            GameManager.instance.unitSelected = this;
            Map.instance.ActivateAttackArea(originTile.GetTileCoordX(), originTile.GetTileCoordY(), movementPoints, ranged, minAttackRange, maxAttackRange);
            GameManager.gameState = GameManager.state.CHECKING_ENEMY_UNIT;
        }
    }

    //Unit has received orders to move
    public void StartMoving(List<ClickableTile> newPath)
    {
        if (newPath != null)
        {
            Map.instance.ReturnTilesToNormal();
            path = newPath;
            ChangeRunningAnimation(path[path.Count - 1].GetTileCoordX(), path[path.Count - 1].GetTileCoordY());
            unitMoving = true;
            unitHasMoved = true;
        }
    }

    //Unit is moving
    void OnMyMerryWay()
    {
        Vector3 destination = new Vector3(path[path.Count - 1].GetTileCoordX(), path[path.Count - 1].GetTileCoordY(), transform.position.z);
        if (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.position = new Vector3(path[path.Count - 1].GetTileCoordX(), path[path.Count - 1].GetTileCoordY(), gameObject.transform.position.z);
            path.RemoveAt(path.Count - 1);
            if (path.Count == 0)
            {
                ArrivedAtDestination((int)destination.x, (int)destination.y);
                unitMoving = false;
            }
            else
            {
                ChangeRunningAnimation(path[path.Count - 1].GetTileCoordX(), path[path.Count - 1].GetTileCoordY());
            }
        }
    }

    //Unit has finished moving
    public void ArrivedAtDestination(int x, int y)
    {
        if (myAnimator != null)
        {
            mySprite.flipX = false;
            //transform.localScale = new Vector3 (1, 1, 1);
            //hpSprite.transform.localScale = new Vector3 (1, 1, 1);
            myAnimator.SetTrigger("idleState");
        }
        possibleDestination = Map.instance.GetTile(x, y);
        cursor.TeleportCursorToTile(x, y);
        if (ranged == false || unitHasMoved == false)
        {
            List<ClickableTile> tilesInAttackRange = Map.instance.unitMovementManager.CalculateRangeMatrix(x, y, maxAttackRange, minAttackRange);
            foreach (ClickableTile tile in tilesInAttackRange)
            {
                tile.ActivateAttackOverlay();
                if (tile.GetUnitAssigned() != null && tile.GetUnitAssigned().propietary != propietary)
                {
                    attackSpots.Add(tile);
                    //attackSpots.Add(tile);
                    InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.ATTACK);
                }
            }
        }
        if (canCapture == true && possibleDestination.typeOfTerrain.capturable == true && possibleDestination.propietary != propietary)
        {
            InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.CAPTURE);
        }
        //Activate LOAD function in menu
        foreach(ClickableTile tile in possibleDestination.neighbors)
        {
            Unit neighboringUnit = tile.GetUnitAssigned();
            if (neighboringUnit != null && neighboringUnit.propietary == GameManager.instance.activePlayer && neighboringUnit.transportUnit)
            {
                Cargo unitCargo = neighboringUnit.GetComponent<Cargo>();
                if (unitCargo.HasFreeSlots())
                {
                    foreach (Unit.typeOfMovement movementClass in unitCargo.acceptedMovementTypes)
                    {
                        if (movementType == movementClass)
                        {
                            loadSpots.Add(tile);
                            InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.LOAD);
                        }
                    }
                }
            }
        }
        InGameMenu.inGameMenu.ActivateMenuOption(MenuOption.menuOptions.WAIT);
        InGameMenu.inGameMenu.ActivateMenu();
        GameManager.gameState = GameManager.state.NAVIGATING_MENU;
    }

    void ChangeRunningAnimation(float nextX, float nextY)
    {
        if (myAnimator != null)
        {
            if (nextY > transform.position.y)
            {
                myAnimator.SetTrigger("runningUp");
            }
            else if (nextY < transform.position.y)
            {
                myAnimator.SetTrigger("runningDown");
            }
            else
            {
                myAnimator.SetTrigger("runningSide");
                if (nextX < transform.position.x)
                {
                    mySprite.flipX = true;
                    //transform.localScale = new Vector3 (-1, 1, 1);
                    //hpSprite.transform.localScale = new Vector3 (-1, 1, 1);
                }
                else
                {
                    mySprite.flipX = false;
                    //transform.localScale = new Vector3 (1, 1, 1);
                    //hpSprite.transform.localScale = new Vector3 (1, 1, 1);
                }
            }
        }
    }

    //Player confirms the movement after the unit has moved and/or attacked
    public void EstablishNewTile()
    {
        ClickableTile newTile = possibleDestination;

        if (unitHasMoved == true && decidedToCapture == false)
        {
            capturePoints = 0;
        }

        gameObject.transform.position = new Vector3(newTile.GetTileCoordX(), newTile.GetTileCoordY(), gameObject.transform.position.z);
        originTile.UnassignUnit();
        newTile.AssignUnit(this);
        originTile = newTile;
        GameManager.instance.unitSelected = null;
        TireUnit();
        Map.instance.ReturnTilesToNormal();
    }

    //Player cancels the movement after the unit has moved
    public void ReturnBackToOrigin()
    {
        gameObject.transform.position = new Vector3(originTile.GetTileCoordX(), originTile.GetTileCoordY(), gameObject.transform.position.z);
        GameManager.instance.unitSelected = null;
        attackSpots.Clear();
        loadSpots.Clear();
        interactableObjectives.Clear();
        unitHasMoved = false;
        Map.instance.ReturnTilesToNormal();
        cursor.TeleportCursorToTile(originTile.GetTileCoordX(), originTile.GetTileCoordY());
    }

    //Unit has already moved and won't be used again this turn
    public void TireUnit()
    {
        GrayUnGray(true);
        unitUsed = true;
    }

    //New turn starts, unit is refreshed
    public void RefreshUnit()
    {
        attackSpots.Clear();
        loadSpots.Clear();
        interactableObjectives.Clear();
        GrayUnGray(false);
        unitHasMoved = false;
        unitUsed = false;
        decidedToCapture = false;
    }

    //Heal due to starting in a property
    public void HealByProperty()
    {
        if (hp < 10)
        {
            if (hp < 9 && propietary.GetFunds() >= (int)(moneyValue * 2 / 10))
            {
                ChangeHP(2);
                propietary.ChangeFunds((int)(-moneyValue * 2 / 10));
            }
            else if (propietary.GetFunds() >= (int)(moneyValue * 1 / 10))
            {
                ChangeHP(1);
                propietary.ChangeFunds((int)(-moneyValue * 1 / 10));
            }
        }
    }

    //Change the visual aspect of the unit to represent it already being used that turn
    void GrayUnGray(bool gray)
    {
        float h, s, v;
        Color.RGBToHSV(mySprite.color, out h, out s, out v);
        if (gray == true)
        {
            v = 0.5f;
        }
        else
        {
            v = 1f;
        }
        mySprite.color = Color.HSVToRGB(h, s, v);
    }

    public void PrepareToAttack()
    {
        objectiveIndex = 0;
        readyToAttack = true;
        interactableObjectives = attackSpots;
        //GetMyAttackRange();
        PinpointUnit(interactableObjectives[objectiveIndex]);
    }

    public void PrepareToLoad()
    {
        objectiveIndex = 0;
        readyToLoad = true;
        interactableObjectives = loadSpots;
        //GetMyAttackRange();
        PinpointUnit(interactableObjectives[objectiveIndex]);
    }

    void SelectObjective()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D))
        {
            if (objectiveIndex >= interactableObjectives.Count - 1)
            {
                objectiveIndex = 0;
            }
            else
            {
                objectiveIndex++;
            }
            PinpointUnit(interactableObjectives[objectiveIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
        {
            if (objectiveIndex <= 0)
            {
                objectiveIndex = interactableObjectives.Count - 1;
            }
            else
            {
                objectiveIndex--;
            }
            PinpointUnit(interactableObjectives[objectiveIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ArrivedAtDestination(possibleDestination.GetTileCoordX(), possibleDestination.GetTileCoordY());
            //Map.instance.ReturnTilesToNormal();
        }
    }
    
    void PinpointUnit(ClickableTile objective)
    {
        cursor.TeleportCursorToTile(objective.GetTileCoordX(), objective.GetTileCoordY());
    }

    public void AttackNow()
    {
        if (readyToAttack == true)
        {
            AttackUnit(interactableObjectives[objectiveIndex].GetUnitAssigned());
            EstablishNewTile();

            //TODO: Maybe should go to another state while the attack transpires before going back to moving the cursor
            GameManager.gameState = GameManager.state.MOVING_CURSOR;
        }
    }

    void AttackUnit(Unit enemy)
    {
        readyToAttack = false;
        float rawDamage = (((attack * attackMultiplier) / 100f) + Random.Range(0, 9)) * (hp / 10f) * ((200f - (enemy.defenseMultiplier + enemy.originTile.typeOfTerrain.defensiveStat * enemy.hp)) / 100f);
        int actualDamage = (int)rawDamage / 10;
        enemy.ChangeHP(-actualDamage);
        propietary.AddPower((int)((enemy.moneyValue / 10) * actualDamage) / 2);
        enemy.propietary.AddPower((int)(enemy.moneyValue / 10) * actualDamage);
        //Counter
        if (ranged == false && enemy != null && enemy.ranged == false)
        {
            rawDamage = (((enemy.attack * enemy.attackMultiplier) / 100f) + Random.Range(0, 9)) * (enemy.hp / 10f) * ((200f - (defenseMultiplier + originTile.typeOfTerrain.defensiveStat * hp)) / 100f);
            actualDamage = (int)rawDamage / 10;
            ChangeHP(-actualDamage);
            propietary.AddPower((int)(moneyValue / 10) * actualDamage);
            enemy.propietary.AddPower((int)((moneyValue / 10) * actualDamage) / 2);
        }
    }

    public void ChangeHP(int value)
    {
        hp = hp + value;
        Debug.Log("Unit " + name + " is at " + hp + " hp");
        if (hp <= 0)
        {
            hp = 0;
            DestroyMe();
        }
        else if (hp >= 10)
        {
            hp = 10;
            hpSprite.sprite = null;
        }
        else
        {
            hpSprite.sprite = hpNumbers[hp - 1];
        }
    }

    void DestroyMe()
    {
        //TODO: Destroy effect.
        propietary.EraseUnitFromArmy(this);
        Destroy(gameObject);
    }

    public void CaptureTile()
    {
        if (unitHasMoved == true)
        {
            capturePoints = 0;
        }
        decidedToCapture = true;
        capturePoints += hp;
        if (capturePoints >= 20)
        {
            possibleDestination.ChangePropietary(propietary);
        }
    }

    public void LoadIntoCargo()
    {
        Debug.Log("Cargo done");
    }
}
