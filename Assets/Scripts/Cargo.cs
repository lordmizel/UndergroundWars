﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    public List<Unit.typeOfMovement> acceptedMovementTypes;
    [SerializeField]
    int cargoSpace = 1;
    [SerializeField]
    Unit[] cargoSlots;

    // Start is called before the first frame update
    void Start()
    {
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
}
