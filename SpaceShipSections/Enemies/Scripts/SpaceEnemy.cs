using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SpaceEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float speed;

    [Header("Components")]
    public GameObject explosion;
    public SpriteRenderer spriteRenderer;
    public Animator anim;

    [Header("Events")]
    public UnityEvent onHit;
    public UnityEvent onDefeated;

    [Header("Settings")]
    public bool hideDestroyedAnimation;
    public float timeToDestroy;

    [HideInInspector]
    public bool isAlive;

    [HideInInspector]
    public bool isMoving;

    protected Coroutine moveCoroutine;

    protected AudioComponent audio;

    protected CircleCollider2D circleCollider;
    protected BoxCollider2D boxCollider;
    protected CapsuleCollider2D capsuleCollider;

    /// <summary>
    /// Remove enemy colliders. 
    /// </summary>
    protected void RemoveColliders()
    {
        if (circleCollider != null)
        {
            circleCollider.enabled = false;
        }

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }
    }

    /// <summary>
    /// Hide enemy sprite.
    /// </summary>
    protected void HideSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Triggered when enemy is hit by player
    /// proyectile.
    /// </summary>
    protected void Hit(GameObject proyectile)
    {
        proyectile.SetActive(false);

        UpdateHealth();

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            audio.PlaySound(0);

            onHit?.Invoke();
        } else
        {
            Death();
        }
    }

    /// <summary>
    /// Update enemy health.
    /// </summary>
    protected void UpdateHealth()
    {
        health--;

        if (health < 0)
        {
            health = 0;
        }
    }

    /// <summary>
    /// Called when enemy health reaches 0.
    /// </summary>
    protected void Death()
    {
        isAlive = false;

        RemoveColliders();

        if (hideDestroyedAnimation == false)
        {
            HideSprite();

            explosion.SetActive(true);
            audio.PlaySound(1);
        }

        onDefeated?.Invoke();

        Destroy(this.gameObject, timeToDestroy);
    }

    /// <summary>
    /// Manage hit collisions.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                Hit(collision.gameObject);
            }
        }
    }

    /// <summary>
    /// Move enemy to designated target.
    /// </summary>
    /// <param name="target">Transform</param>
    public void Move(Transform target)
    {
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(MoveRoutine(target));
        }
    }

    /// <summary>
    /// Move enemy to designated target coroutine.
    /// </summary>
    /// <param name="target">Transform</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveRoutine(Transform target)
    {
        isMoving = true;

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        isMoving = false;
        moveCoroutine = null;
    }

    /// <summary>
    /// Stop all base enemy coroutines.
    /// </summary>
    public void StopEnemyCoroutines()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    protected void Init()
    {
        audio = GetComponent<AudioComponent>();

        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        isAlive = true;
    }
}
