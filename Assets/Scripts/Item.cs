using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject {

    [Header("Only gameplay")]
    //public TileBase tile;
    public ItemType type;
    public bool stackable = true;
    public Sprite image;

    [Header("Shop Item Price")]
    public int price; 


}

public enum ItemType {
    Object, 
    Weapon, 
    Powerup, 
    ShopItem
}
