using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CargoMenu : MonoBehaviour
{
    [SerializeField]
    GameObject menuContainer;
    [SerializeField]
    List<GameObject> unitButtons;
    [SerializeField]
    List<Text> buttonTexts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateUnitCargoMenu(List<Unit> unitsToShow)
    {
        for(int x = 0; x < unitsToShow.Count; x++)
        {
            buttonTexts[x].text = unitsToShow[x].name;
            unitButtons[x].SetActive(true);
        }
    }

    public void DeactivateUnitCargoMenu()
    {
        for(int x = 0; x < unitButtons.Count; x++)
        {
            buttonTexts[x].text = "";
            unitButtons[x].SetActive(false);
        }
    }
}
