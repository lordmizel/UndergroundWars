using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameSpecifications characterHolder;

    void Start()
    {
        characterHolder = FindObjectOfType<GameSpecifications>();
    }

    public void StartGame()
    {
        characterHolder.EstablishCharacters();
        SceneManager.LoadScene(characterHolder.mapToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
