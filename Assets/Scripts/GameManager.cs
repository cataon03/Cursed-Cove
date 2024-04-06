using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    MainMenu,
    FireIsland,
    IceIsland,
    Home, 
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isGameStart = true; 
    public delegate void OnGameStateChange(GameState newState);
    public static event OnGameStateChange gameStateChange;
    private GameState currentGameState;
    public GameState previousGameState; 

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

    public void restartLevel(){
        HandleOnGameStateChanged(currentGameState); 
    }

    public void setIsGameStart(bool isGameStart){
        this.isGameStart = isGameStart; 
    }
    public bool getIsGameStart(){
        return isGameStart; 
    }

    public GameState getPrevGameState(){
        return previousGameState; 
    }


    public GameState getCurrentGameState(){
        return currentGameState; 
    }
    private void Start()
    {
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
        previousGameState = currentGameState; 
        switch (newState)
        {
            case GameState.MainMenu:
                SceneManager.LoadScene("MainMenu"); 
                break;
            case GameState.FireIsland:
                SceneManager.LoadScene("FireIsland");
                break;
            case GameState.IceIsland: 
                SceneManager.LoadScene("IceIsland"); 
                break; 
            case GameState.GameOver:
                Application.Quit(); 
                break;
        }
        currentGameState = newState; 
    }
}