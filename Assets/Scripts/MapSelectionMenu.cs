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

    [SerializeField]
    Text numberText;
    [SerializeField]
    List<BattleMap> maps2p, maps3p, maps4p;
    [SerializeField]
    List<MapMenuOption> mapMenuEntries;



    // Start is called before the first frame update
    void Start()
    {
        gameSpecs = FindObjectOfType<GameSpecifications>();
        currentNumberOfPlayers = minPlayers;
        SeedMapMenu(maps2p);
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

        numberText.text = currentNumberOfPlayers + " Players";
    }

    void SeedMapMenu(List<BattleMap> maps)
    {
        for(int x = 0; x < mapMenuEntries.Count; x++)
        {
            if (x >= maps.Count)
            {
                mapMenuEntries[x].gameObject.SetActive(false);
            }
            else
            {
                mapMenuEntries[x].gameObject.SetActive(true);
                mapMenuEntries[x].ChangeName(maps[x].mapName);
            }
        }
    }
}
