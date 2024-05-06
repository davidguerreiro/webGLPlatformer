using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProyectile : MonoBehaviour
{
    public string tagToCompare = "ResetProyectile";

    /// <summary>
    /// Check object collision.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagToCompare))
        {
            gameObject.SetActive(false);
        }
    }
}
