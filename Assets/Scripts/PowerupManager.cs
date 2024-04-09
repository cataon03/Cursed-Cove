using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;
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
     
        InventoryManager.OnPowerupEquipped += HandleOnPowerupEquipped;
    }

   // Handle equipping weapons here. Probably good to start out with a prompt to ask if 
   // the player actually wants to change weapons. 
   void HandleOnPowerupEquipped(Item powerup){

        Debug.Log("Need to handle logic for powerup"); 
      

        if (powerup.powerupType == PowerupType.Speed){
            
        }

    }
}
