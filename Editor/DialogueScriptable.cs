using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogueScriptable : MonoBehaviour
{
    [MenuItem("Assets/Create/DialogueData")]
    /// <summary>
    /// Add dialogue data scriptable to create menu.
    /// </summary>
    public static void AddDialogueDataScriptable()
    {
        var asset = ScriptableObject.CreateInstance<DialogueData>();

        // if needs preconfiguration, add here.
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path += "/NewDialogue.asset";

        ProjectWindowUtil.CreateAsset(asset, path);
    }
}
