using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity; 

public enum GameState
{
    MainMenu,
    Tutorial, 
    WinMenu, 
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
    private int levelIdx = 0; 
    private static GameState[] LEVELS = {GameState.Tutorial, GameState.FireIsland, GameState.IceIsland}; 

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

    private void Start()
    {
        SetGameState(GameState.MainMenu); 
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

    public void nextLevel(){
        // Increment the current level 
        levelIdx += 1; 

        if (levelIdx == LEVELS.Length){
            Debug.Log("game is over"); 
            HandleOnGameStateChanged(GameState.WinMenu); // End the game 
        }
        else {
            Debug.Log("going to: " + levelIdx); 
            HandleOnGameStateChanged(LEVELS[levelIdx]); // Load the next level 
        }
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
            case GameState.Tutorial:
                SceneManager.LoadScene("Tutorial"); 
                break;
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
            case GameState.WinMenu: 
                SceneManager.LoadScene("WinMenu"); 
                break; 
        }
        currentGameState = newState; 
    }

    
    [YarnCommand("start_game")]
    public void StartGame(){
        nextLevel(); 
    }
}