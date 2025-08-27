using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProyectile : MonoBehaviour
{
    [Header("Settings")]
    public float speed;

    private Vector2 _direccion;
    private bool _canMove;

    private Transform initParent;

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            Move();
        }
    }

    private void OnDisable()
    {
       // ReassignParentOnDisable();
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

    /// <summary>
    /// Reasign original spawn parent. Useful if proyectile
    /// was part of an object pool.
    /// </summary>
    private void ReassignParentOnDisable()
    {
        transform.parent = initParent;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        //initParent = transform.parent;
    }
}
