using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelDataScriptable : MonoBehaviour
{
    [MenuItem("Assets/Create/LevelData")]
    /// <summary>
    /// Add level data scriptable to create menu.
    /// </summary>
    public static void AddLevelDataScriptable ()
    {
        var asset = ScriptableObject.CreateInstance <LevelData>();

        // if needs preconfiguration, add here.
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path += "/NewLevelData.asset";

        ProjectWindowUtil.CreateAsset(asset, path);
    }

    [MenuItem("Assets/Create/LocalVars")]
    /// <summary>
    /// Add local vars scriptable to create menu.
    /// </summary>
    public static void AddLocalVarsScriptable()
    {
        var asset = ScriptableObject.CreateInstance<LocalVars>();

        // if needs preconfiguration, add here.
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path += "/NewLocalVars.asset";

        ProjectWindowUtil.CreateAsset(asset, path);
    }
}