using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Powerup")]
public class Powerup : Item {
    [Header("Powerups")]
    public PowerupType powerupType;
    public float powerupValue; 
    public float timeForPowerup; 
}

public enum PowerupType {
    Speed, 
    Invisibility, 
    Invincibility
}
