using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public static int MAX_HEALTH = 5; 

    public static PlayerHealthBar Instance; 

    public int health; 
    public int numOfHearts; 

    public Image[] hearts; 
    public Sprite fullHeart; 
    public Sprite emptyHeart; 

    void Awake(){
        health = 5; 
        numOfHearts = 5; 
        Instance = this; 
        DamageableCharacter.OnPlayerHit += handleOnPlayerHit;
    }
    
    void onDestory(){
        DamageableCharacter.OnPlayerHit -= handleOnPlayerHit;
    }

    private void handleOnPlayerHit(float health){
      
        this.health = (int) health; 

    }

    void Update(){
        for (int i = 0; i < hearts.Length; i++){
            if (i < health){
                hearts[i].sprite = fullHeart; 
            }
            else {
                hearts[i].sprite = emptyHeart; 
            }
            
            if (i < numOfHearts){
                hearts[i].enabled = true; 
            }
            else {
                hearts[i].enabled = false; 
            } 
        }
    }

}
