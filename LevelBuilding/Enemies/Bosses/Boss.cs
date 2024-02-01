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
    public GameObject[] levelEnemiesAndHazards;

    [Header("Settings")]
    public float hitForceUp;
    public bool hasMovingAnim;

    [Header("Events")]
    public UnityEvent onHit;
    public UnityEvent onDefeated;

    [HideInInspector]
    public bool isAlive;

    [HideInInspector]
    public bool isMoving;

    [HideInInspector]
    public bool inBattleLoop;

    [HideInInspector]
    public Coroutine isBeingHit;

    [HideInInspector]
    public Coroutine isBeingDestroyed;

    private int _bossPhase;

    protected AudioComponent _audio;
    protected Rigidbody2D _rigi;
    protected Animator _anim;

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
    ///  Remove all enemies and hazards from
    ///  current level when Boss has been defeated.
    /// </summary>
    private void RemoveAllEnemiesAndHazards()
    {
        foreach (GameObject enemyOrHazard in levelEnemiesAndHazards)
        {
            if (enemyOrHazard != null && enemyOrHazard.activeSelf)
            {
                enemyOrHazard.SetActive(false);
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

        RemoveAllEnemiesAndHazards();

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
    /// Get current boss phase number.
    /// </summary>
    /// <returns></returns>
    public int GetCurrentBossPhase()
    {
        return _bossPhase;
    }
    
    /// <summary>
    /// Increase current boss phase.
    /// </summary>
    public void IncreaseBossPhase()
    {
        _bossPhase++;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    protected void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _rigi = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _bossPhase = 0;
    }
}
