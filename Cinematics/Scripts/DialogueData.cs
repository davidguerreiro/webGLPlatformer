using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : ScriptableObject
{
    public Sprite speakerImageSprite;

    [TextArea]
    public string[] dialogue;

    /// <summary>
    /// Gets dialogue from this
    /// scriptable.
    /// </summary>
    /// <returns>string[]</returns>
    public string[] GetDialogue()
    {
        return dialogue;
    }
}
