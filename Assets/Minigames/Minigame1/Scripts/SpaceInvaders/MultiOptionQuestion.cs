using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New question", menuName = "Scriptable objects/Question")]
public class MultiOptionQuestion : ScriptableObject
{
    [TextArea] public string sentence;
    [Range(0, 3)] public int correctId;
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;
}
