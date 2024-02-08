using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSpawners : MonoBehaviour
{
    /// <summary>
    /// Collision with reset spawners logic.
    /// </summary>
    /// <param name="collision">collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ResetSpawn") || collision.gameObject.CompareTag("Hazard"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
