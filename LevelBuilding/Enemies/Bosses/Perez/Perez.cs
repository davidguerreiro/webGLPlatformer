using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perez : Boss
{
    [Header("Stats")]
    public float movingSpeed;
    public float speedIncrementedAfterHit;

    [Header("Eneimes to spawn")]
    public GameObject[] enemiesRight;
    public GameObject[] enemiesLeft;

    [Header("BattleFlow")]
    public PerezShootingAttack shootingAttackRight;
    public PerezShootingAttack shootingAttackLeft;
    public float toWaitInTube;
    public bool goingLeft;

    [Header("Moving Points")]
    public Transform tubeRight;
    public Transform tubeLeft;

    private int _enemiesSpawnCounter;
    private Coroutine _moveToTube;
    private Coroutine _patternAttack;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (inBattleLoop)
        {
            if (_patternAttack == null)
            {
                _patternAttack = StartCoroutine(PatternAttack());
            }

            CheckForMovingAnim();
        }
    }
      
    /// <summary>
    /// Move to tube.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MoveToTube()
    {
        yield return new WaitForSeconds(toWaitInTube);

        Transform destination = (goingLeft) ? tubeLeft : tubeRight;

        while (Vector2.Distance(transform.position, destination.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination.position, movingSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();            
        }

        transform.position = destination.position;

        ChangeMovingDirection();

        _moveToTube = null;
    }

    /// <summary>
    /// Change enemy direction.
    /// </summary>
    public void ChangeMovingDirection()
    {
        transform.Rotate(0f, 180f, 0f);
        goingLeft = !goingLeft;
    }

    /// <summary>
    /// Increment speed.
    /// Call this method on Hit event.
    /// </summary>
    public void IncrementSpeed()
    {
        movingSpeed += speedIncrementedAfterHit;
    }

    /// <summary>
    /// Spawn enemies in battle arena.
    /// </summary>
    public void SpawnEnemyGroup()
    {
        if (_enemiesSpawnCounter <= 3)
        {
            enemiesLeft[_enemiesSpawnCounter].SetActive(true);
            enemiesRight[_enemiesSpawnCounter].SetActive(true);

            _enemiesSpawnCounter++;
        }
    }

    /// <summary>
    /// Main pattern attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PatternAttack()
    {
        // move to tube.
        _moveToTube = StartCoroutine(MoveToTube());

        while (_moveToTube != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _patternAttack = null;
    }

    /// <summary>
    /// Check for moving animation.
    /// </summary>
    public void CheckForMovingAnim()
    {
        if (isMoving)
        {
            _anim.SetBool("IsMoving", true);
        } else
        {
            _anim.SetBool("IsMoving", false);
        }
    }

    /// <summary>
    /// Stop all battle coroutines.
    /// Call this method in onDefeated 
    /// boss battle event.
    /// </summary>
    public void StopAllBattleCoroutines()
    {
        isMoving = false;
        _anim.SetBool("IsMoving", false);

        if (_moveToTube != null)
        {
            StopCoroutine(_moveToTube);
            _moveToTube = null;
        }

        if (_patternAttack != null)
        {
            StopCoroutine(_patternAttack);
            _patternAttack = null;
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
