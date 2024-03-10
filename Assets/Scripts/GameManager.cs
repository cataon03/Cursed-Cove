using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    MainMenu,
    FireIsland,
    Home, 
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void OnGameStateChange(GameState newState);
    public static event OnGameStateChange gameStateChange;

    private GameState currentGameState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PickupableItem.FireIslandWin += handleOnFireIslandWin; 
        SetGameState(GameState.MainMenu);
    }
      void Update()
    {
        // Check if the Esc key was pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Load the Main Menu scene
            SceneManager.LoadScene("MainMenu");
        }
    }
    

    private void handleOnFireIslandWin()
    {
    }

    public void SetGameState(GameState newState)
    {
        currentGameState = newState;
       this. HandleOnGameStateChanged(newState); 
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

    private void HandleOnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                SceneManager.LoadScene("MainMenu"); 
                break;
            case GameState.FireIsland:
                SceneManager.LoadScene("FireIsland"); 

                break;
            case GameState.GameOver:
                Application.Quit(); 
                break;
        }
    }
}