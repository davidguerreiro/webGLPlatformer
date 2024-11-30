using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableGameObjects : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject[] gameObjects;

    /// <summary>
    /// Enable all gameobjects in array.
    /// </summary>
    public void EnableAll()
    {
        foreach (GameObject gm in gameObjects)
        {
            gm.SetActive(true);
        }
    }

    /// <summary>
    /// Disable all gameObejcts in array.
    /// </summary>
    public void DisableAll()
    {
        foreach (GameObject gm in gameObjects)
        {
            gm.SetActive(false);
        }
    }
}
