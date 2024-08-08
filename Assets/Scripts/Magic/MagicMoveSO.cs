using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicMoveSO : ScriptableObject
{
    public string moveName;
    public string moveDescription;
    public Sprite icon;
    public bool isUnlocked;
    public Sprite lockedIcon;
    public TypeOfSkill typeOfSkill;
    public float coolDownTiming = 4;

    public GameObject skillPrefab;

    public abstract void Activate();
    public abstract void Cast();

}

public enum TypeOfSkill
{
    INSTANT,
    CASTABLE
}
