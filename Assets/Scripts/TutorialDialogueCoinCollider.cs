using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialDialogueCoinCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DialogueManager.instance.StartDialogue("BasicFighting");
            Destroy(gameObject);
        }
    }
    
}
