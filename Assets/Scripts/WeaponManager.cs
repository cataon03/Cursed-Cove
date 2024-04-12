using System.Collections;
using System.Collections.Generic;
using System.Text;
using System; 
using UnityEngine;
using UnityEditor.Animations;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public AnimationClip[] fireSwordAnimations; 
    public AnimationClip[] iceSwordAnimations; 
    public AnimationClip[] purpleSwordAnimations; 

    public AnimatorOverrideController animatorOverrideController;
    
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

   // Handle equipping weapons
   void HandleOnItemEquipped(string weaponName){
        Debug.Log(weaponName); 
        AnimationClip[]  clips; 
        string[] animationNames = { "player_attack", "waiting", "walking" };
        if (weaponName == "FireSword"){
            clips = fireSwordAnimations; 
        }
        else if (weaponName == "IceSword"){
            clips = iceSwordAnimations; 
        }
        else {
            clips = purpleSwordAnimations; 
        }

        for (int i = 0; i < animationNames.Length; i++){
            print(animationNames[i]);
            animatorOverrideController[animationNames[i]] = clips[i];
        }
           
    }
}
