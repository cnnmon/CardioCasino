using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Flashback", menuName = "Flashback")]
public class Flashback : ScriptableObject
{
    public TextAsset storyJSON;
    public Sprite bgSprite;
    public Sprite[] choicesLeft;
    public Sprite[] choicesRight;
}
