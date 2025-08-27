using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetSpawners : MonoBehaviour
{
    public bool destroySpaceEnemies;

    /// <summary>
    /// Collision with reset spawners logic.
    /// </summary>
    /// <param name="collision">collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ResetSpawn") || collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
        }

        if (destroySpaceEnemies)
        {
            if (collision.gameObject.CompareTag("SpaceEnemy"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
