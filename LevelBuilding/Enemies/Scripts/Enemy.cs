using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite defeatedSprite;

    [Header("Colliders")]
    public CircleCollider2D[] circleColliders;
    public BoxCollider2D[] boxColliders;
    public CapsuleCollider2D[] capsuleColliders;

    [Header("Settings")]
    public bool flipSpriteOnKill;
    public float defeatedForceUp;
    public float timeToDestroy;

    [HideInInspector]
    public bool isAlive;

    protected AudioSource _audio;
    protected Rigidbody2D _rigi;

    /// <summary>
    /// Remove enemy colliders.
    /// </summary>
    private void RemoveColliders()
    {
        foreach (CircleCollider2D collider in circleColliders)
        {
            collider.enabled = false;
        }

        foreach (BoxCollider2D collider  in boxColliders)
        {
            collider.enabled = false;
        }

        foreach (CapsuleCollider2D collider in capsuleColliders)
        {
            collider.enabled = false;
        }
    }

    /// <summary>
    /// Enemy killed by player.
    /// </summary>
    public virtual void Defeated()
    {
        RemoveColliders();
        SetDefeatedSprite();

        _rigi.isKinematic = false;
        _rigi.velocity = new Vector2(0f, defeatedForceUp);

        _audio.Play();

        gameManager.player.playerController.EnemyDefeatedRecoil();

        Destroy(this, timeToDestroy);
    }

    /// <summary>
    /// Set defeated enemy sprite.
    /// </summary>
    private void SetDefeatedSprite()
    {
        spriteRenderer.sprite = defeatedSprite;

        if (flipSpriteOnKill)
        {
            spriteRenderer.flipY = true;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    protected void Init()
    {
        isAlive = true;
        _audio = GetComponent<AudioSource>();
        _rigi = GetComponent<Rigidbody2D>();
    }
}
