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

    [Header("Events")]
    public UnityEvent getDamage;


    [HideInInspector]
    public PlayerController playerController;

    [HideInInspector]
    public bool inDamage;

    private Coroutine _damageRoutine;

    /// <summary>
    /// Update player's health.
    /// </summary>
    /// <param name="update">int</param>
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
        UpdateHealth(-1);

        getDamage?.Invoke();
    }

    /// <summary>
    /// Allow to get damage from level
    /// hazards.
    /// </summary>
    public void AllowDamage()
    {
        inDamage = false;
    }

    /// <summary>
    /// Triggers player collision.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerController.canMove && collision.gameObject.CompareTag("Hazard") && ! inDamage)
        {
            GetDamage();
        }
    }

    /// <summary>
    /// Init player.
    /// </summary>
    public void InitPlayer()
    {
        // init playercontroller.
        playerController = GetComponent<PlayerController>();
        playerController.Init();

        key = false;
    }
}
