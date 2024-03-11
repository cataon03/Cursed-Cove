using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{

    //global variables
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public static HealthSystem Instance;


    //once program is started
    void Awake(){
        health = 5;
        numOfHearts = 5;
        Instance = this;
        DamageableShip.OnPlayerHit += handleOnPlayerHit;
    }

    void onDestroy()
    {
        DamageableShip.OnPlayerHit -= handleOnPlayerHit;
    }

    private void handleOnPlayerHit(float health)
    {
        this.health = (int)health;
    }   

    // Update is called once per frame
    void Update()
    {
       
        for(int i=0; i<numOfHearts; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }


            if(i<numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
