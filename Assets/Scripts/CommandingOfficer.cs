using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class CommandingOfficer : MonoBehaviour
{
    public AudioClip armyTheme;

    public int specialPowerSections = 6;
    public int minorPowerPercentage = 50;
    
    public int fundsPerPropierty = 1000;
    
    public List<Unit> landUnitCatalog = new List<Unit>();
    public List<Unit> airUnitCatalog = new List<Unit>();
    public List<Unit> seaUnitCatalog = new List<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
