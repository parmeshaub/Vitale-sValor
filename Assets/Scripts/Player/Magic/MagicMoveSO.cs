using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicMoveSO : ScriptableObject
{
    public string moveName;
    public string moveDescription;
    public Sprite icon;

    public abstract void Activate();
}
