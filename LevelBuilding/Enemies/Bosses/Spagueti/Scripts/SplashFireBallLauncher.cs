using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashFireBallLauncher : MonoBehaviour
{
    public ObjectPool pool;

    /// <summary>
    /// Instance splash fire ball from object pool and trigger
    /// physics.
    /// </summary>
    /// <param name="direction">string</param>
    public void ShootSplashBall(string direction)
    {
        GameObject splashBall = pool.SpawnPrefab();

        if (splashBall)
        {
            splashBall.transform.position = gameObject.transform.position;

            Rigidbody2D splashRigi = splashBall.GetComponent<Rigidbody2D>();

            splashRigi.velocity = Vector2.zero;

            // TODO: Change, test and/or randomize force values.
            if (direction == "left")
            {
                splashRigi.velocity = new Vector2(-5f, 5f);
            } else
            {
                splashRigi.velocity = new Vector2(5f, 5f);
            }                
        }
    }
}
