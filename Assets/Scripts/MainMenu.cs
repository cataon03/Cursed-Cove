using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
     GameManager gameManager; 
     public TextMeshProUGUI startResumeButtonTxt;

     public void Start(){
          gameManager = FindObjectOfType<GameManager>();
          if (gameManager.getIsGameStart()){
               startResumeButtonTxt.text = "Start Game!"; 
          }
          else {
               startResumeButtonTxt.text = "Resume"; 
          }
     }

     public void PlayGame(){
          if (gameManager.getIsGameStart()){
               gameManager.setIsGameStart(false); 
               gameManager.SetGameState(GameState.IceIsland); // Set default start at fire island 
          }
          else {
               gameManager.SetGameState(gameManager.getPrevGameState()); // Otherwise, set to last scene played
          }
     }

     public void QuitGame(){
          GameManager gameManager = FindObjectOfType<GameManager>();
          gameManager.SetGameState(GameState.GameOver);
     }
}
