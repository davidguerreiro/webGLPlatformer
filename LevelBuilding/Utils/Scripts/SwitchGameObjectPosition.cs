using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGameObjectPosition : MonoBehaviour
{
    public GameObject[] toSwitch;
    public GameObject toMove;

    /// <summary>
    /// Switch gameobjct from to switch array in position
    /// arrays by index.
    /// </summary>
    /// <param name="index">int</param>
    public void SwitchPosition(int index)
    {
        toSwitch[index].transform.position = toMove.transform.position;
    }
}
