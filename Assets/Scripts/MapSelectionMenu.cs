using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectionMenu : MonoBehaviour
{
    GameSpecifications gameSpecs;
    int maxPlayers = 4;
    int minPlayers = 2;
    int currentNumberOfPlayers;



    // Start is called before the first frame update
    void Start()
    {
        gameSpecs = FindObjectOfType<GameSpecifications>();
        currentNumberOfPlayers = minPlayers;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangePlayerNumber(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangePlayerNumber(-1);
        }
    }

    void ChangePlayerNumber(int i)
    {
        currentNumberOfPlayers += i;
        if(currentNumberOfPlayers < minPlayers)
        {
            currentNumberOfPlayers = maxPlayers;
        }
        else if (currentNumberOfPlayers > maxPlayers)
        {
            currentNumberOfPlayers = minPlayers;
        }


    }
}
