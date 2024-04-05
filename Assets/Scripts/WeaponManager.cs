using System.Collections;
using System.Collections.Generic;
using System.Text;
using System; 
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public GameObject player; 
    
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

    private void Start() {
     
        InventoryManager.OnItemEquipped += HandleOnItemEquipped;
    }

   // Handle equipping weapons here. Probably good to start out with a prompt to ask if 
   // the player actually wants to change weapons. 
   void HandleOnItemEquipped(string weaponName){
        Debug.Log("Need to handle logic for implementing different weapons here!"); 
        Debug.Log(weaponName); 

        if (weaponName == "FireSword"){
            Debug.Log("Here's where we'd change the animation on the player!"); 
        }
        /*
        Changing between weapons includes things like changing the animation to 
        show the new weapon (for example, if the weaponName is "fire sword" or whatever, 
        you can change the gray parts on the sword in the character's animation to orange), 
        changing the attack damage/knockback values, and any other special abilities that
        come with the weapon (setting enemies on fire, making enemies slow, doing extra
        damage to certain enemies, etc.)
        */ 

    }
}
