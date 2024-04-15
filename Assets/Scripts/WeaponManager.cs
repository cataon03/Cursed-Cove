using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public AnimationClip[] fireSwordAnimations; 
    public AnimationClip[] iceSwordAnimations; 
    public AnimationClip[] purpleSwordAnimations; 
    public AnimatorOverrideController animatorOverrideController;
    public SwordHitbox playerSwordHitbox; 
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        InventoryManager.OnWeaponEquipped += HandleOnWeaponEquipped;
        playerSwordHitbox = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordHitbox>(); 
    }

   // Handle equipping weapons
   void HandleOnWeaponEquipped(Weapon weapon){
        AnimationClip[]  clips; 
        string[] animationNames = { "player_attack", "waiting", "walking" };
        
        if (weapon.name == "Fire Sword"){
            clips = fireSwordAnimations; 
        }
        else if (weapon.name == "Ice Sword"){
            clips = iceSwordAnimations; 
        }
        else {
            clips = purpleSwordAnimations; 
        }

        for (int i = 0; i < animationNames.Length; i++){
            animatorOverrideController[animationNames[i]] = clips[i];
        }
        playerSwordHitbox.knockbackForce = weapon.knockback; 
        playerSwordHitbox.swordDamage = weapon.damage; 
    }
}
