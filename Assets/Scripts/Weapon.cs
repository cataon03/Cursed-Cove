using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Weapon")]
public class Weapon : Item {
    [Header("Weapon")]
    public string weaponName; 
    public float damage;
    public float knockback;
}
