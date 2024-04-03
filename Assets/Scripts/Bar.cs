using Pathfinding;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.InputSystem;
using Pathfinding.Util;


public class Bar : MonoBehaviour
{
    [field: SerializeField]
    public int MaxValue { get;  set; }
    
    [field: SerializeField]
    public int Value {get; set; }
    
    [SerializeField]
    private RectTransform _topBar; 

    [SerializeField]
    private RectTransform _bottomBar; 

    [SerializeField] 
    private float _animationSpeed = 10f; 

    private Coroutine _adjustBarWidthCouroutine; 

    private float _fullWidth; 
    private float TargetWidth => Value * _fullWidth / MaxValue; 

    private void Start(){
        _fullWidth = _topBar.rect.width; 
    }

    private IEnumerator AdjustBarWidth(int amount){
        var suddenChangeBar = amount >= 0 ? _bottomBar : _topBar; 
        var slowChangeBar = amount >= 0 ? _topBar : _bottomBar; 

        suddenChangeBar.SetWidth(TargetWidth); 
        while (Mathf.Abs(suddenChangeBar.rect.width - slowChangeBar.rect.width) > 1f){
            slowChangeBar.SetWidth(Mathf.Lerp(slowChangeBar.rect.width, TargetWidth, Time.deltaTime * _animationSpeed)); 
            yield return null; 
        }
        slowChangeBar.SetWidth(TargetWidth); 
    }

    public void Change(int amount){
        Debug.Log(amount); 
        Value = Mathf.Clamp(Value - amount, 0, MaxValue); 
        if (_adjustBarWidthCouroutine != null){
            StopCoroutine(_adjustBarWidthCouroutine); 
        }
        _adjustBarWidthCouroutine = StartCoroutine(AdjustBarWidth(amount)); 
    }

    /*
    private void Update(){
       if (Mouse.current.leftButton.wasPressedThisFrame){
        Change(20); 
       }
       if (Mouse.current.rightButton.wasPressedThisFrame){
        Change(-20); 
       }
    } */ 
}