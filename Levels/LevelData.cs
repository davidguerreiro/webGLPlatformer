using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public string levelName;
    public string levelIntro;
    public Sprite levelIcon;
    public string nextLevelName;
    public string levelToLoadOnRetry;
    public AudioClip levelMusic;
    public bool saveDataAtInitLevel;
    public string saveVariableName;
}
