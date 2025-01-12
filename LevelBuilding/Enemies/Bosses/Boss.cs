using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Boss : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Base Stats")]
    public int hitsToDestroy;

    [Header("Components")]
    public SpriteRenderer sprite;

    [Header("Colliders")]
    public CircleCollider2D[] circleColliders;
    public BoxCollider2D[] boxColliders;
    public CapsuleCollider2D[] capsuleColliders;

    [Header("Other Components")]
    public GameObject[] hazardPoints;
    public GameObject[] weakPoints;
    public GameObject[] destroyedExplosions;
    public GameObject[] levelEnemiesAndHazards;

    [Header("Settings")]
    public float hitForceUp;
    public bool hasMovingAnim;
    public bool showDestroyedSprite;
    public Sprite destroyedSprite;
    public bool displayKeyWhenDefeated;

    [Header("Events")]
    public UnityEvent onHit;
    public UnityEvent onDefeated;
    public UnityEvent onDefeatedAfterAnim;

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
    public void RemoveColliders()
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
    public void EnableColliders()
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
    public void RemoveHazardPoints()
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
    public void EnableHazardPoints()
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
    /// Remove all boss weak points.
    /// </summary>
    public void RemoveWeakPoints()
    {
        foreach (GameObject weakPoint in weakPoints)
        {
            weakPoint.SetActive(false);
        }
    }

    /// <summary>
    /// Enable all boss weak poinsts.
    /// </summary>
    public void EnableWeakPoints()
    {
        foreach (GameObject weakPoint in weakPoints)
        {
            weakPoint.SetActive(true);
        }
    }

    /// <summary>
    ///  Remove all enemies and hazards from
    ///  current level when Boss has been defeated.
    /// </summary>
    public void RemoveAllEnemiesAndHazards()
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
    /// Boss hit on weak point.
    /// </summary>
    public void Hit()
    {
        if (isBeingHit == null)
        {
            isBeingHit = StartCoroutine(HitCoroutine());
        }
    }

    /// <summary>
    /// Get hit.
    /// </summary>
    /// <returns>IEnmumerator</returns>
    private IEnumerator HitCoroutine()
    {
        GetDamage();
        Debug.Log("Hit coroutine");
        if (hitsToDestroy > 0)
        {
            onHit?.Invoke();

            RemoveColliders();
            _anim.SetTrigger("IsHit");
            _audio.PlaySound(2);

            gameManager.player.playerController.TriggerHitVibration();
            gameManager.player.playerController.EnemyDefeatedRecoil();

            yield return new WaitForSeconds(1f);

            EnableColliders();

        } else
        {
            // Boss defeated.
            gameManager.player.playerController.EnemyDefeatedRecoil();
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

        RemoveColliders();
        RemoveHazardPoints();
        RemoveAllEnemiesAndHazards();

        if (showDestroyedSprite)
        {
            sprite.sprite = destroyedSprite;
        }

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
        _audio.SetLoop(false);
        _audio.PlaySound(4);
        yield return new WaitForSeconds(1.5f);
        
        if (displayKeyWhenDefeated)
        {
            gameManager.DisplayKey();
        }

        onDefeatedAfterAnim?.Invoke();

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
    /// Set sprite rendering order.
    /// </summary>
    /// <param name="order">int</param>
    public void SetSpriteRendererOrder(int order = 0)
    {
        sprite.sortingOrder = order;
    }

    /// <summary>
    /// Start boss battle.
    /// </summary>
    public void StartBattle()
    {
        EnableColliders();
        EnableHazardPoints();

        isAlive = true;
        inBattleLoop = true;
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
