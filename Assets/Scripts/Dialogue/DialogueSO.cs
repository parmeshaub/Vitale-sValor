using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class DialogueSO : ScriptableObject
{
    [Header("General")]
    public DialogueType dialogueType;
    public AudioClip audioClip;
    [TextArea(4, 4)]
    public string dialogueText;
    public DialogueSO nextDialogue;

    [Header("Normal Dialogue")]
    public string speakerName;
    public string speakerRole;
    public float parsingSpeed;
    public bool isPlayerSpeaking;
    public AnimationEmotions emotionEnum;
    public List<DialogueChoices> choices;

    [Header("Cinematic Dialogue")]
    public float dialogueExitTime;
}

public enum AnimationEmotions
{
    HAPPY,
    SAD,
    ANGRY,
    SURPRISED,
    CONFUSED,
    EXCITED,
    SCARED,
    DISGUSTED,
    ANXIOUS,
    CALM,
    EMBARRASSED,
    CURIOUS,
    FRUSTRATED,
    RELIEVED,
    HOPEFUL,
    BORED,
    DETERMINED,
    SHOCKED,
    PROUD,
    NERVOUS,
    NONE
}

public enum DialogueType
{
    Normal,
    Cinematic
}

