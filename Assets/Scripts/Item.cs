using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject {

    [Header("Only gameplay")]
    //public TileBase tile;
    public ItemType type;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;

    [Header("Powerups")]
    public PowerupType powerupType;
    public float powerupValue; 
    public float timeForPowerup; 

}

public enum ItemType {
    Object, 
    Weapon, 
    Powerup
}

public enum PowerupType {
    Speed, 
    Damage, 
    Invincibility
}
