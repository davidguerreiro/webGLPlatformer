using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkBallShooter : MonoBehaviour
{
    public ObjectPool pool;

    /// <summary>
    /// Spawn spark ball.
    /// </summary>
    public void ShootSparkBall()
    {
        GameObject sparkBall = pool.SpawnPrefab();
        
        if (sparkBall)
        {
            sparkBall.transform.position = gameObject.transform.position;
        }
    }
}
