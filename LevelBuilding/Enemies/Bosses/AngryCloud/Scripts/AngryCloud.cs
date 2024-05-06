using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryCloud : Boss
{
    [Header("Stats")]
    public float movingSpeed;

    [Header("Spark Tackle Attack")]
    public float startSparkTackleAttackSpeed;
    public float spartkTackleAttackSpeed;

    [Header("Moving Points")]
    public Transform[] movingPoints;

    private Coroutine _movingRoutine;
    
    // TODO: Program electric proyectiles object pool and attack.
    // TODO: Program spark tackle attack.

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Move boss point to point while shooting
    /// electric bombs.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MovePointToPoint()
    {
        for (int i = 0; i < movingPoints.Length; i++)
        {
            int nextIndex = i + 1;
            Transform target = (nextIndex == movingPoints.Length) ? movingPoints[0] : movingPoints[nextIndex];

            while (Vector2.Distance(transform.position, target.position) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }

            transform.position = target.position;

            // TODO: Trigger shoot electric bomb.
            
        }

        _movingRoutine = null;
    }

    /// <summary>
    /// Increase boss phase based on boss hits.
    /// Call this method in onHit boss Unity Event.
    /// </summary>
    public void IncreaseBossPhaseAfterHit()
    {
        if (hitsToDestroy == 2)
        {
            movingSpeed += 1.5f;
        }
    }

    /// <summary>
    /// Stop all attacks. This method
    /// will be normally called from OnDefeated Boss
    /// Unity Event.
    /// </summary>
    public void StopAllAttackCoroutines()
    {
        if (_movingRoutine != null)
        {
            StopCoroutine(_movingRoutine);
            _movingRoutine = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public new void Init()
    {
        base.Init();
        isMoving = true;
    }
}
