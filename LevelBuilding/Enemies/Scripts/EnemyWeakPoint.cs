using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWeakPoint : MonoBehaviour
{
    public Enemy enemy;
    public Boss boss;
    public UnityEvent hitEvent;

    /// <summary>
    /// Checks player hits enemy.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemy != null)
        {
            if (enemy.isAlive && collision.gameObject.CompareTag("Player"))
            {
                hitEvent?.Invoke();
            }
        }

        if (boss != null)
        {
            if (boss.isAlive && boss.inBattleLoop && boss.isBeingHit == null && collision.gameObject.CompareTag("Player"))
            {
                hitEvent?.Invoke();
            }
        }
    }
}
