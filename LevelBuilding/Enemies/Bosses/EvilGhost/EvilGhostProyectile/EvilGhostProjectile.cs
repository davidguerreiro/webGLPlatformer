using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGhostProjectile : MonoBehaviour
{
    [Header("Stats")]
    public float movingSpeed;
    public float shootSpeed;

    [HideInInspector]
    public bool isMoving;
    
    private Rigidbody2D _rigi;
    private Animator _anim;
    private Coroutine _movingRoutine;

    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// On disable unity event.
    /// </summary>
    private void OnDisable()
    {
        if (_movingRoutine != null)
        {
            StopCoroutine(_movingRoutine);
            _movingRoutine = null;
        }
    }

    /// <summary>
    /// Moving projectile towards target.
    /// </summary>
    /// <param name="target">Transform</param>
    /// <param name="isShoot"><bool/param>
    public void MoveToTarget(Transform target, bool isShoot = false)
    {
        if (_movingRoutine == null)
        {
            _movingRoutine = StartCoroutine(MoveToTargetRoutine(target, isShoot));
        }
    }

    /// <summary>
    /// Moving proyectile towards target coroutine.
    /// </summary>
    /// <param name="target">Transform</param>
    /// <param name="isShoot">bool</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator MoveToTargetRoutine(Transform target, bool isShoot = false)
    {
        isMoving = true;
        float speed = (isShoot) ? shootSpeed : movingSpeed; 

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _movingRoutine = null;
    }

    /// <summary>
    /// Spawn logic.
    /// </summary>
    public void Spawn()
    {
        if (_anim == null)
        {
            _anim = GetComponent<Animator>();
        }

        if (_rigi == null)
        {
            _rigi = GetComponent<Rigidbody2D>();
        }

        _anim.SetBool("Spawn", false);

        _rigi.gravityScale = 0;
        _anim.SetBool("Spawn", true);
    }

    /// <summary>
    /// Apply force to this proyectile.
    /// </summary>
    /// <param name="force">Vector2</param>
    public void ApplyForce(Vector2 force)
    {
        _rigi.gravityScale = 1;
        _rigi.AddForce(force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _rigi = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
}
