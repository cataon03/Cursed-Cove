using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public static int MAX_HEALTH = 5; 

    public static PlayerHealthBar Instance; 

    public float health = 5; 
    private int numOfHearts; 

    public Image[] hearts; 
    public Sprite fullHeart; 
    public Sprite emptyHeart; 
    public Sprite halfHeart; 

    void Awake(){
        Instance = this; 
        DamageableCharacter.OnPlayerHit += handleOnPlayerHit;
    }

    void Start(){
        numOfHearts = hearts.Length - 1;  
        for (int i = 0; i < hearts.Length; i++){
            hearts[i].sprite = fullHeart; 
        }
    }
    
    void onDestory(){
        DamageableCharacter.OnPlayerHit -= handleOnPlayerHit;
    }
    private void handleOnPlayerHit(float health) {
        int fullHearts = (int)health; // Number of full hearts is the integer part of health
        bool hasHalfHeart = (health % 1) >= 0.5; // Check if there is a half heart

        for (int i = 0; i < hearts.Length; i++) {
            if (i < fullHearts) {
                hearts[i].sprite = fullHeart; // Set full heart
            } else if (i == fullHearts && hasHalfHeart) {
                hearts[i].sprite = halfHeart; // Set half heart if applicable
            } else {
                hearts[i].sprite = emptyHeart; // Set empty heart
            }
        }
    }

}
