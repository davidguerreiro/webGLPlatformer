using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShip : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int lifes;
    public int maxLifes;
    public float speed;

    [Header("Status")]
    public bool invincible;

    [Header("Events")]
    public UnityEvent getDamage;
    public UnityEvent updateLife;
    public UnityEvent onInvencible;
    public UnityEvent onDestroyed;

    [HideInInspector]
    public PlayerShipController playerController;

    [HideInInspector]
    public PlayerShipAudioAnims playerAudioAnims;

    [HideInInspector]
    public bool inDamage;

    [HideInInspector]
    public ShipGameManager gameManager;

    /// <summary>
    /// Update maxhealth.
    /// </summary>
    /// <param name="update"></param>
    public void UpdateHealth(int update)
    {
        health += update;

        if (health < 0)
        {
            health = 0;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    /// <summary>
    /// Restore players full health.
    /// </summary>
    public void RestoreFullHealth()
    {
        health = maxHealth;
    }

    /// <summary>
    /// Get current player's health.
    /// </summary>
    /// <returns>int</returns>
    public int GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Update player lifes.
    /// </summary>
    /// <param name="life">int</param>
    public void UpdateLife(int life)
    {
        lifes += life;

        if (lifes <= -1)
        {
            lifes = 0;
        }

        if (lifes > maxLifes)
        {
            lifes = maxLifes;
        }

        updateLife?.Invoke();
    }

    /// <summary>
    /// Get player current lifes.
    /// </summary>
    /// <returns>int</returns>
    public int GetLife()
    {
        return lifes;
    }

    /// <summary>
    /// Get damage.
    /// </summary>
    public void GetDamage()
    {
        inDamage = true;
        UpdateHealth(-1);


        if (health > 0)
        {
            getDamage?.Invoke();
            Invoke("AllowDamage", .5f);
        } else
        {
            Destroyed();
            inDamage = false;
        }

    }

    /// <summary>
    /// Set player invencible.
    /// Usually called after respawn.
    /// </summary>
    public void Invincible()
    {
        invincible = true;

        onInvencible?.Invoke();

        Invoke("AllowDamage", 1.5f);
    }

    /// <summary>
    /// Player destroyed.
    /// </summary>
    public void Destroyed()
    {
        UpdateLife(-1);
        playerController.RestrictControl();

        onDestroyed?.Invoke();
    }

    /// <summary>
    /// Allow to get damage from hazards and
    /// enemies.
    /// </summary>
    public void AllowDamage()
    {
        inDamage = false;
        invincible = false;
    }

    /// <summary>
    /// Triggers player enter collision logic.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerController.canMove && (collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("SpaceEnemy")) && !inDamage && !invincible)
        {
            GetDamage();
        }
    }

    /// <summary>
    /// Trigger player stay collsion logic.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerController.canMove && (collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("SpaceEnemy")) && !inDamage && !invincible)
        {
            GetDamage();
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void InitPlayer(ShipGameManager gameManager)
    {
        this.gameManager = gameManager;
        playerController = GetComponent<PlayerShipController>();
        playerController.Init(this);

        playerAudioAnims = GetComponent<PlayerShipAudioAnims>();
        playerAudioAnims.Init(this);
    }
}
