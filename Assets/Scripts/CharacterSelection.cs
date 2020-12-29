using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    int selectedCharacter = 0;
    [SerializeField]
    CharacterPortraitSelector[] portraits = new CharacterPortraitSelector[4];
    [SerializeField]
    List<CommandingOfficer> characters = new List<CommandingOfficer>();

    // Start is called before the first frame update
    void Start()
    {
        portraits[selectedCharacter].HighlightMe();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeSelectedPortrait(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeSelectedPortrait(-1);
        }
    }

    void ChangeSelectedPortrait(int next)
    {
        portraits[selectedCharacter].UnHighlightMe();
        selectedCharacter += next;
        if(selectedCharacter < 0)
        {
            selectedCharacter = portraits.Length - 1;
        }
        else if (selectedCharacter >= portraits.Length)
        {
            selectedCharacter = 0;
        }
        portraits[selectedCharacter].HighlightMe();
    }
}
