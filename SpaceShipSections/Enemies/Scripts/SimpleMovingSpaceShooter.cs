using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovingSpaceShooter : SpaceEnemy
{
    [Header("Shooter components")]
    public GameObject fire;
    public SpaceCannon cannon;

    [Header("Settings")]
    public int minShoots;
    public int maxShoots;
    public float toWaitBetweenShoots;
    public float toWaitBeforeMoving;

    private Transform[] movingPoints;
    private Coroutine shooting;
    private Coroutine moveAndShoot;

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            MoveAndShoot();
        }
    }

    /// <summary>
    /// Move and shoot attack;
    /// </summary>
    private void MoveAndShoot()
    {
        if (moveAndShoot == null)
        {
            moveAndShoot = StartCoroutine(MoveAndShootRoutine());
        }
    }

    /// <summary>
    /// Move and shoot attack routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveAndShootRoutine()
    {
        int indexMoving = Random.Range(0, movingPoints.Length - 1);
        int shoots = Random.Range(minShoots, maxShoots + 1);

        Move(movingPoints[indexMoving]);

        while (isMoving)
        {
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < shoots; i++)
        {
            shooting = StartCoroutine(Shoot());
            yield return shooting;
        }

        yield return new WaitForSeconds(toWaitBeforeMoving);

        moveAndShoot = null;
    }

    /// <summary>
    /// Shoot coroutine.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shoot()
    {
        cannon.Shoot();
        audio.PlaySound(2);

        yield return new WaitForSeconds(toWaitBetweenShoots);

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

        if (moveAndShoot != null)
        {
            StopCoroutine(moveAndShoot);
            moveAndShoot = null;
        }   
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="objectPool">ObjectPool</param>
    public void Init(ObjectPool objectPool, Transform[] movingPoints)
    {
        Init();

        this.movingPoints = movingPoints;
        cannon.Init(.1f, objectPool);
    }
}
