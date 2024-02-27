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
        SetGameState(GameState.MainMenu);
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
                Debug.Log("Switched to MainMenu state");
                SceneManager.LoadScene("MainMenu"); 
                break;
            case GameState.FireIsland:
                SceneManager.LoadScene("FireIsland"); 

                break;
            case GameState.GameOver:
                Debug.Log("Switched to GameOver state");
                Application.Quit(); 
                break;
        }
    }
}