using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProyectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed;

    private Vector2 _direccion;
    private bool _canMove;

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            Move();
        }
    }

    /// <summary>
    /// Set proyectile axis X direction. Calling
    /// this method puts the proyectile in motion.
    /// </summary>
    /// <param name="direction">string</param>
    public void SetDirection(string direction)
    {
        _direccion = (direction == "right") ? Vector2.right : Vector2.left;
        _canMove = true;
    }

    /// <summary>
    /// Increase proyectile speed.
    /// </summary>
    /// <param name="multiplyer"></param>
    public void IncreaseSpeed(float multiplyer)
    {
        speed *= multiplyer;
    }

    /// <summary>
    /// Move proyectile. Call this method
    /// in Update or coroutine.
    /// </summary>
    private void Move()
    {
        transform.Translate(_direccion * speed * Time.deltaTime);
    }
}
