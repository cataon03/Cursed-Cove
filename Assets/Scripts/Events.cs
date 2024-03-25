using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Events 
{
    public static event Action<bool> OnCharacterFreeze; 
    public static event Action<bool> OnPlayerAttackDisabled;
    public static event Action OnBossEnabled;  

}