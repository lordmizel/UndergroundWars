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
    List<BattleMap>[] battleMapLists;
    [SerializeField]
    List<MapMenuOption> mapMenuEntries;
    int selectedMap = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        gameSpecs = FindObjectOfType<GameSpecifications>();
        currentNumberOfPlayers = minPlayers;
        battleMapLists = new List<BattleMap>[3] { maps2p, maps3p, maps4p};
        ChangePlayerNumber(0);
        mapMenuEntries[selectedMap].LightUp();
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
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSelectedMapOption(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeSelectedMapOption(1);
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
        SeedMapMenu(battleMapLists[currentNumberOfPlayers - 2]);
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

    void ChangeSelectedMapOption(int i)
    {
        mapMenuEntries[selectedMap].UnLight();
        selectedMap += i;
        if (selectedMap < 0)
        {
            selectedMap = battleMapLists[currentNumberOfPlayers - 2].Count - 1;
        }
        else if (selectedMap >= battleMapLists[currentNumberOfPlayers - 2].Count)
        {
            selectedMap = 0;
        }

        mapMenuEntries[selectedMap].LightUp();
    }
}
