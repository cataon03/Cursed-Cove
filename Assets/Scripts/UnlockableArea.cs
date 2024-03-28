using Yarn.Unity; 
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnlockableArea : MonoBehaviour
{
    public Tilemap unlockableArea; 
    public Tilemap oldBoundary; 
    public Tilemap newBoundary; 
    
    [YarnCommand("unlock_area")]
    public void UnlockArea(){
        unlockableArea.enabled = true; 
        oldBoundary.enabled = false; 
        newBoundary.enabled = true; 
    }   
}