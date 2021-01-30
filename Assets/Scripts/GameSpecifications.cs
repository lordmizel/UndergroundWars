using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpecifications : MonoBehaviour
{
    public int playerNumber = 2;
    public CommandingOfficer[] officers = new CommandingOfficer[4];
    [SerializeField]
    CharacterSelection selectionManager;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void EstablishCharacters()
    {
        for(int x = 0; x < selectionManager.characters.Count; x++)
        {
            officers[x] = Instantiate(selectionManager.characters[selectionManager.selectedCharacters[x]], this.transform);
        }
    }
}
