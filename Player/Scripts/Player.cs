using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public int maxHealth;
    public int coins;
    public int maxCoins;
    public bool key;

    [Header("Status")]
    public bool invincible;

    [Header("Events")]
    public UnityEvent getDamage;
    public UnityEvent onInvincible;


    [HideInInspector]
    public PlayerController playerController;

    [HideInInspector]
    public bool inDamage;

    private Rigidbody2D rigi;

    /// <summary>
    /// Update player's health.
    /// </summary>
    /// <param name="update">int</param>
    public void UpdateHealth(int update)
    {
        health += update;

        if (health < -1)
        {
            health = -1;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    /// <summary>
    /// Get current player health.
    /// </summary>
    /// <returns>int</returns>
    public int GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Update player coins.
    /// </summary>
    /// <param name="update">int</param>
    public void UpdateCoins(int update)
    {
        coins += update;

        if (coins < 0)
        {
            coins = 0;
        }

        if (coins > maxCoins)
        {
            coins = maxCoins;
        }
    }

    /// <summary>
    /// Get current player coins.
    /// </summary>
    /// <returns>int</returns>
    public int GetCoins()
    {
        return coins;
    }

    /// <summary>
    /// Get key logic.
    /// </summary>
    public void GetKey()
    {
        key = true;
    }

    /// <summary>
    /// Checks if player has obtained
    /// current level key.
    /// </summary>
    /// <returns>bool</returns>
    public bool HasKey()
    {
        return key;
    }

    /// <summary>
    /// Get damage.
    /// </summary>
    public void GetDamage()
    {
        inDamage = true;

        playerController.RestrictControl();
        playerController.TriggerHitVibration();
        UpdateHealth(-1);

        ResetPlayerPhysics();

        getDamage?.Invoke();
    }

    /// <summary>
    /// Set player invencible.
    /// </summary>
    public void Invincible()
    {
        invincible = true;

        onInvincible?.Invoke();

        Invoke("AllowDamage", 1.5f);
    }

    /// <summary>
    /// Allow to get damage from level
    /// hazards.
    /// </summary>
    public void AllowDamage()
    {
        inDamage = false;
        invincible = false;
    }

    /// <summary>
    /// Triggers player collision.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerController.canMove && ! invincible && collision.gameObject.CompareTag("Hazard") && ! inDamage)
        {
            GetDamage();
        }
    }

    /// <summary>
    /// Reset player physics, usually after death.
    /// </summary>
    private void ResetPlayerPhysics()
    {
        rigi.velocity = Vector2.zero;
        rigi.angularVelocity = 0f;
        rigi.simulated = false;
    }

    /// <summary>
    /// Enable player physics, usually disabled after death.
    /// </summary>
    public void ReEnablePlayerPhysics()
    {
        rigi.simulated = true;
    }

    /// <summary>
    /// Init player.
    /// </summary>
    public void InitPlayer()
    {
        // init playercontroller.
        playerController = GetComponent<PlayerController>();
        rigi = GetComponent<Rigidbody2D>();

        playerController.Init();

        key = false;
        inDamage = false;
    }
}
