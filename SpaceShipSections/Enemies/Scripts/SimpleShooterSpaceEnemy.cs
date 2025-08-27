using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShooterSpaceEnemy : SpaceEnemy
{
    [Header("Shooter components")]
    public GameObject fire;
    public SpaceCannon cannon;

    [Header("State")]
    public bool ableToShoot;

    [Header("Settings")]
    public float minCadence;
    public float maxCadence;

    private Coroutine shooting;

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            EnemyPerformGameplayActions();  
        }
    }

    /// <summary>
    /// Enemy battle AI used during gampeplay.
    /// Usually called from Upate() when alive.
    /// </summary>
    private void EnemyPerformGameplayActions()
    {
        if (isMoving)
        {
            PreventShooting();
        } else
        {
            AllowShooting();
        }

        if (ableToShoot && shooting == null)
        {
            shooting = StartCoroutine(Shoot());
        }
    }

    /// <summary>
    /// Shoot coroutine.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Shoot()
    {
        float toWait = Random.Range(minCadence, maxCadence);

        cannon.Shoot();
        audio.PlaySound(2);

        yield return new WaitForSeconds(toWait);

        shooting = null;
    }

    /// <summary>
    /// Removes fire from spaceship. Usually called in
    /// onDefeat() Unity event.
    /// </summary>
    public void RemoveFire()
    {
        fire.SetActive(false);
    }

    /// <summary>
    /// Allows enemy to shoot.
    /// </summary>
    public void AllowShooting()
    {
        ableToShoot = true;
    }

    /// <summary>
    /// Restricts enemy to shoot.
    /// </summary>
    public void PreventShooting()
    {
        ableToShoot = false;
    }

    /// <summary>
    /// Stop all enemy behaviour coroutines.
    /// </summary>
    public new void StopEnemyCoroutines()
    {
        base.StopEnemyCoroutines();

        if (shooting != null)
        {
            StopCoroutine(shooting);
            shooting = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="objectPool"></param>
    public void Init(ObjectPool objectPool)
    {
        Init();
        cannon.Init(.1f, objectPool);
    }
}
