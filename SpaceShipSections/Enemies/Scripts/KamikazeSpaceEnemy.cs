using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeSpaceEnemy : SpaceEnemy
{
    [Header("Enemy components")]
    public GameObject fire;

    [Header("Settings")]
    public float ToWaitBeforeAttack;
    public float speedAttack;

    private Coroutine attackRoutine;
    private bool attacking;

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (! isMoving && attackRoutine == null)
            {
                attackRoutine = StartCoroutine(SetAttack());
            }

            if (attacking)
            {
                KamikazeAttack();
            }
        }
    }

    /// <summary>
    /// Time to wait before performing kamikaze attack;
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator SetAttack()
    {
        yield return new WaitForSeconds(ToWaitBeforeAttack);
        Attack();
    }

    /// <summary>
    /// Trigger kamikaze attack.
    /// </summary>
    private void Attack()
    {
        attacking = true;
        audio.PlaySound(2);
    }

    /// <summary>
    /// Perform kamikaze attack.
    /// </summary>
    private void KamikazeAttack()
    {
        transform.Translate(Vector2.left * speedAttack * Time.deltaTime);
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
    /// Stop all enemy behaviours coroutines.
    /// </summary>
    public new void StopEnemyCoroutines()
    {
        base.StopEnemyCoroutines();

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public new void Init()
    {
        base.Init();
    }
}
