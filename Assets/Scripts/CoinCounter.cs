using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Instance; 
    public TMP_Text cointText; 
    public int currentCoins = 0; 

    void Awake(){
        cointText = GetComponent<TMP_Text>();
        Instance = this; 
    }

    void Start(){
        cointText.text = "COINS: " + currentCoins.ToString(); 
    }

    public void IncreaseCoins(){
        currentCoins += 1; 
        cointText.text = "COINS: " + currentCoins.ToString(); 
    } 

    public void updateText(){
        cointText.text = "COINS: " + currentCoins.ToString(); 
    } 
}
