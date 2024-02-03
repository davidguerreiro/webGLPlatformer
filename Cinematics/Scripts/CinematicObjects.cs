using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicObjects : MonoBehaviour
{
    public GameObject[] objects;

    /// <summary>
    /// Enable cinematic object.
    /// </summary>
    /// <param name="index">int</param>
    public void EnableObject(int index)
    {
        objects[index].SetActive(true);
    }

    /// <summary>
    /// Disable cinematic object.
    /// </summary>
    /// <param name="index"></param>
    public void DisableObject(int index)
    {
        objects[index].SetActive(false);
    }

    /// <summary>
    /// Enable all cinematic objects.
    /// </summary>
    public void EnableAll()
    {
        foreach (GameObject cinematicObject in objects) 
        {
            cinematicObject.SetActive(true);
        }
    }

    /// <summary>
    /// Disable all cinematic objects.
    /// </summary>
    public void DisableAll()
    {
        foreach (GameObject cinematicObject in objects)
        {
            cinematicObject.SetActive(false);
        }
    }

    /// <summary>
    /// Get cinematic object.
    /// </summary>
    /// <param name="index">int</param>
    /// <returns>GameObject</returns>
    public GameObject GetObject(int index)
    {
        if (index < objects.Length)
        {
            return objects[index];
        }

        return null;
    }

    /// <summary>
    /// Add cinematic object to objects
    /// array.
    /// </summary>
    /// <param name="cinematicObject">GameObject</param>
    /// <param name="index">int</param>
    public void SetObject(GameObject cinematicObject, int index)
    {
        if (index < objects.Length)
        {
            objects[index] = cinematicObject;
        }
    }
}
