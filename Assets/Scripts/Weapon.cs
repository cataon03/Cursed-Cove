using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Weapon")]
public class Weapon : Item {
    [Header("Weapon")]
    public float damage;
    public float knockback;
}
