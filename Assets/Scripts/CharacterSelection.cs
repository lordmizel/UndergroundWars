using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    int selectedSlot = 0;
    [SerializeField]
    CharacterPortraitSelector[] portraits = new CharacterPortraitSelector[4];
    int[] selectedCharacters = new int[4] { 0, 1, 2, 3};
    [SerializeField]
    List<CommandingOfficer> characters = new List<CommandingOfficer>();

    // Start is called before the first frame update
    void Start()
    {
        portraits[selectedSlot].HighlightMe();
        for(int x = 0; x < selectedCharacters.Length; x++)
        {
            portraits[x].portrait.sprite = characters[x].portrait;
        }
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
        portraits[selectedSlot].UnHighlightMe();
        selectedSlot += next;
        if(selectedSlot < 0)
        {
            selectedSlot = portraits.Length - 1;
        }
        else if (selectedSlot >= portraits.Length)
        {
            selectedSlot = 0;
        }
        portraits[selectedSlot].HighlightMe();
    }
}
