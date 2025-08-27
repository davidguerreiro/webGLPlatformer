using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderSpaceShip : SpaceEnemy
{
    [Header("Boss Components")]
    public GameObject fire;
    public SpaceCannon cannon;
    public GameObject[] explosions;

    [Header("Boss Settings")]
    public int minShoots;
    public int maxShoots;
    public float toWaitBetweenShoots;
    public float toWaitToShootAgainMin;
    public float toWaitToShootAgainMax;
    public float destroyedAnimDuration;

    private bool inBattle;
    private Transform topMovingPoint;
    private Transform bottomMovingPoint;
    private Coroutine moveContinuousRoutine;
    private Coroutine shootRoutine;
    private Coroutine shooting;

    // Update is called once per frame
    void Update()
    {
       if (isAlive && inBattle)
        {
            if (moveContinuousRoutine == null)
            {
                moveContinuousRoutine = StartCoroutine(MoveUpAndDown());
            }

            if (shootRoutine == null)
            {
                shootRoutine = StartCoroutine(PerformShooting());
            }
        } 
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
    /// Perform shooting attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PerformShooting()
    {
        int shoots = Random.Range(minShoots, maxShoots + 1);
        float toWaitBeforTriggeringShootsAgain = Random.Range(toWaitToShootAgainMin, toWaitToShootAgainMax);

        for (int i = 0; i < shoots; i++)
        {
            shooting = StartCoroutine(Shoot());
            yield return shooting;
        }

        yield return new WaitForSeconds(toWaitBeforTriggeringShootsAgain);

        shootRoutine = null;
    }

    /// <summary>
    /// Move up and down continuously
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveUpAndDown()
    {
        Move(topMovingPoint);

        while (isMoving)
        {
            yield return new WaitForFixedUpdate();
        }

        Move(bottomMovingPoint);

        while (isMoving)
        {
            yield return new WaitForFixedUpdate();
        }

        moveContinuousRoutine = null;
    }

    /// <summary>
    /// Shoot coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator Shoot()
    {
        cannon.Shoot();
        audio.PlaySound(2);

        yield return new WaitForSeconds(toWaitBetweenShoots);
    }

    /// <summary>
    /// Trigger boss destroyed animation.
    /// </summary>
    public void BossDestroyed()
    {
        StartCoroutine(DestroyedRoutine());
        TriggerPostBossSequence();
    }

    /// <summary>
    /// Trigger after boss defeated cinematic.
    /// </summary>
    public void TriggerPostBossSequence()
    {
        GameObject.Find("BossDefeatedCinematic").GetComponent<AttackSpaceAfterBossDefeatedCinematic>().PlayBossAppearCinematic();
    }

    /// <summary>
    /// Starts boss battle.
    /// </summary>
    public void StartBattle()
    {
        inBattle = true;
    }

    /// <summary>
    /// Boss destroyed routine.
    /// </summary>
    /// <returns>IEnumetarot</returns>
    private IEnumerator DestroyedRoutine()
    {
        inBattle = false;
        anim.SetBool("Defeated", true);

        // TODO: set audio loop for explosions.
        foreach (GameObject explosion in explosions)
        {
            explosion.SetActive(true);
        }

        yield return new WaitForSeconds(destroyedAnimDuration);

        foreach (GameObject explosion in explosions)
        {
            explosion.SetActive(false);
        }

        HideSprite();

        explosion.SetActive(true);
        audio.PlaySound(1);
    }

    /// <summary>
    /// Stop all enemy behaviour coroutines.
    /// </summary>
    public new void StopEnemyCoroutines()
    {
        base.StopEnemyCoroutines();

        if (shootRoutine != null)
        {
            StopCoroutine(shooting);
            shooting = null;
        }

        if (moveContinuousRoutine != null)
        {
            StopCoroutine(moveContinuousRoutine);
            moveContinuousRoutine = null;
        }

        if (shooting != null)
        {
            StopCoroutine(shooting);
            shooting = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="objectPool">ObjectPool</param>
    /// <param name="topMovingPoint">Transform</param>
    /// <param name="bottomMovingPoint">Transform</param>
    public void Init(ObjectPool objectPool, Transform topMovingPoint, Transform bottomMovingPoint)
    {
        Init();

        this.topMovingPoint = topMovingPoint;
        this.bottomMovingPoint = bottomMovingPoint;

        cannon.Init(.1f, objectPool);
    }
}
