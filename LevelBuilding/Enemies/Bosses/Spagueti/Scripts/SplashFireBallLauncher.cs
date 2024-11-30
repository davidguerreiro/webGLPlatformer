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

            Vector2 force = GetRandForce(direction);
            splashRigi.AddForce(force, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Get force for splash ball.
    /// </summary>
    /// <param name="direction">string</param>
    /// <returns>Vector2</returns>
    private Vector2 GetRandForce(string direction)
    {
        float forceSide = Random.Range(4f, 6f);
        float forceUp = Random.Range(9f, 11f);

        Vector2 force = Vector2.up * forceUp;

        if (direction == "left")
        {
            force += Vector2.left * forceSide;
        } else
        {
            force += Vector2.right * forceSide;
        }

        return force;
    }
}
