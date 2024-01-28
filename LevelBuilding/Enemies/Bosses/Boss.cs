using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Boss : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Base Stats")]
    public int hitsToDestroy;

    [Header("Colliders")]
    public CircleCollider2D[] circleColliders;
    public BoxCollider2D[] boxColliders;
    public CapsuleCollider2D[] capsuleColliders;

    [Header("Other Components")]
    public GameObject[] hazardPoints;
    public GameObject[] destroyedExplosions;

    [Header("Settings")]
    public float hitForceUp;
    public float hasMovingAnim;

    [Header("Events")]
    public UnityEvent onHit;
    public UnityEvent onDefeated;

    [HideInInspector]
    public bool isAlive;
    public bool isMoving;
    public bool inBattleLoop;
    public Coroutine isBeingHit;
    public Coroutine isBeingDestroyed;

    protected AudioComponent _audio;
    protected Rigidbody2D _rigi;
    protected Animator _anim;

    // TODO: Continue implementing boss logic in Crusher class.

    /// <summary>
    /// Remove enemy colliders.
    /// </summary>
    private void RemoveColliders()
    {
        foreach (CircleCollider2D collider in circleColliders)
        {
            collider.enabled = false;
        }

        foreach (BoxCollider2D collider in boxColliders)
        {
            collider.enabled = false;
        }

        foreach (CapsuleCollider2D collider in capsuleColliders)
        {
            collider.enabled = false;
        }
    }

    /// <summary>
    /// Enable colliders
    /// </summary>
    private void EnableColliders()
    {
        foreach (CircleCollider2D collider in circleColliders)
        {
            collider.enabled = true;
        }

        foreach (BoxCollider2D collider in boxColliders)
        {
            collider.enabled = true;
        }

        foreach (CapsuleCollider2D collider in capsuleColliders)
        {
            collider.enabled = true;
        }

    }

    /// <summary>
    /// Remove hazard tags.
    /// </summary>
    private void RemoveHazardPoints()
    {
        foreach (GameObject hazard in hazardPoints)
        {
            if (hazard.activeSelf)
            {
                hazard.tag = "Untagged";
            }
        }
    }

    /// <summary>
    /// Enable hazard points.
    /// </summary>
    private void EnableHazardPoints()
    {
        foreach (GameObject hazard in hazardPoints)
        {
            if (hazard.activeSelf)
            {
                hazard.tag = "Hazard";
            }
        }
    }

    /// <summary>
    /// Get hit.
    /// </summary>
    /// <returns>IEnmumerator</returns>
    public IEnumerator Hit()
    {
        GetDamage();

        if (hitsToDestroy > 0)
        {
            onHit?.Invoke();

            RemoveColliders();
            _anim.SetTrigger("IsHit");
            _audio.PlaySound(2);
            yield return new WaitForSeconds(1f);

            EnableColliders();

        } else
        {
            // Boss defeated.
            isBeingDestroyed = StartCoroutine(Destroyed());

            while (isBeingDestroyed != null)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        isBeingHit = null;
    }

    /// <summary>
    /// Boss destroyed anim and logic.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator Destroyed()
    {
        onDefeated?.Invoke();

        isAlive = false;
        isMoving = false;
        inBattleLoop = false;

        _anim.SetBool("Destroy1", true);
        _audio.PlaySound(2);
        yield return new WaitForSeconds(1f);

        _anim.SetBool("Destroy2", true);
        _audio.SetLoop(true);
        _audio.PlaySound(3);
        StartCoroutine(EnableDestroyExplosions());
        yield return new WaitForSeconds(3.5f);

        DisableDestroyExplosions();
        _anim.SetBool("Destroy3", true);
        _audio.PlaySound(4);
        yield return new WaitForSeconds(1.5f);

        gameManager.DisplayKey();
        isBeingDestroyed = null;
    }


    /// <summary>
    /// Enable explision animations when boss
    /// is defeated.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator EnableDestroyExplosions()
    {
        foreach (GameObject explosion in destroyedExplosions)
        {
            explosion.SetActive(true);
            yield return new WaitForSeconds(.5f);
        }
    }

    /// <summary>
    /// Disable explosions displayed when boss is defeated in destroy2
    /// animation phase.
    /// </summary>
    private void DisableDestroyExplosions()
    {
        foreach (GameObject explosion in destroyedExplosions)
        {
            explosion.SetActive(false);
        }
    }

    /// <summary>
    /// Update hit count.
    /// </summary>
    private void GetDamage()
    {
        hitsToDestroy--;

        if (hitsToDestroy <= 0)
        {
            hitsToDestroy = 0;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    protected void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _rigi = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }
}
