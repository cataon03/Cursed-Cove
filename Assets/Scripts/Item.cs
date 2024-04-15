using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject {

    [Header("Item")]
    public bool stackable = true;
    public Sprite image;
    public int price; 
    public string itemName; 
}