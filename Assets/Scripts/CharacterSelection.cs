using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    GameSpecifications gameSpecs;
    int selectedSlot = 0;
    int selectedCO = 0;
    [SerializeField]
    CharacterPortraitSelector[] portraits = new CharacterPortraitSelector[4];
    public int[] selectedCharacters = new int[4] { 0, 1, 2, 3};
    public List<CommandingOfficer> characters = new List<CommandingOfficer>();

    // Start is called before the first frame update
    void Start()
    {
        gameSpecs = FindObjectOfType<GameSpecifications>();
        for(int x = 0; x < gameSpecs.playerNumber; x++)
        {
            portraits[x].gameObject.SetActive(true);
            portraits[x].portrait.sprite = characters[x].portrait;
        }
        portraits[selectedSlot].HighlightMe();
        //for(int x = 0; x < selectedCharacters.Length; x++)
        //{
        //    portraits[x].portrait.sprite = characters[x].portrait;
        //}
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
            selectedSlot = gameSpecs.playerNumber - 1;
        }
        else if (selectedSlot >= gameSpecs.playerNumber)
        {
            selectedSlot = 0;
        }
        portraits[selectedSlot].HighlightMe();
        selectedCO = selectedCharacters[selectedSlot];
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
        selectedCharacters[selectedSlot] = selectedCO;
        portraits[selectedSlot].portrait.sprite = characters[selectedCO].portrait;
    }
}
