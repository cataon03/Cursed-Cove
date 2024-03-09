using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : DialogueItem
{
    public string name; 

    // TextArea(min, max)
    [TextArea(3, 10)]
    public string[] sentences; 

}
