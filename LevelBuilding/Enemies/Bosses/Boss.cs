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

    [Header("Settings")]
    public float hitForceUp;
    public float hasMovingAnim;

    [Header("Events")]
    public UnityEvent onHit;
    public UnityEvent onDefeated;

    [HideInInspector]
    public bool isAlive;
    public bool isMoving;
    public Coroutine isBeingHit;

    protected AudioComponent _audio;
    protected Rigidbody2D _rigi;
    protected Animator _anim;

    // TODO: Add hit and destroy logic.

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
    private void Hit()
    {

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
