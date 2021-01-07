using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    int selectedSlot = 0;
    int selectedCO = 0;
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
        else if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSelectedCO(1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeSelectedCO(-1);
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

    void ChangeSelectedCO(int next)
    {
        selectedCO += next;
        if (selectedCO < 0)
        {
            selectedCO = characters.Count - 1;
        }
        else if (selectedCO >= characters.Count)
        {
            selectedCO = 0;
        }
        portraits[selectedSlot].portrait.sprite = characters[selectedCO].portrait;
    }
}
