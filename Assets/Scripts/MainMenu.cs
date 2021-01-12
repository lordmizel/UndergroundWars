using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    CharacterHolder characterHolder;

    public void StartGame()
    {
        characterHolder.EstablishCharacters();
        //SceneManager.LoadScene("Test");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
