using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     public void PlayGame(){
          GameManager gameManager = FindObjectOfType<GameManager>();
          gameManager.SetGameState(GameState.FireIsland);
     }

     public void QuitGame(){
          GameManager gameManager = FindObjectOfType<GameManager>();
          gameManager.SetGameState(GameState.GameOver);
     }
}
