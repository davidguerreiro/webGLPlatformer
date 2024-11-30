using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : Boss
{
    [Header("Stats")]
    public float movingSpeed;
    public float movingUpSpeed;
    public float fallingDownSpeed;

    [Header("Battle config")]
    public int bossIncreasePhaseAtHits;

    [Header("Projectiles Spawner Attack")]
    public SkyProjectileSpawner[] spawners;
    public float timeBetweenProjectile;
    public int rounds;

    [Header("MovingPoints")]
    public Transform rightMovingPoint;
    public Transform leftMovingPoint;
    public Transform rightUpPoint;
    public Transform leftUpPoint;

    private string _direction;
    private Coroutine _movingRoutine;
    private Coroutine _movingUpDownRoutine;
    private Coroutine _projectilesAttack;
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
    /// Moving boss side to side.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MovingSideToSide()
    {
        ChangeDirection();

        Transform target = (_direction == "left") ? leftMovingPoint : rightMovingPoint;

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _movingRoutine = null;
    }

    /// <summary>
    /// Moving boss up/down routine.
    /// </summary>
    /// <param name="isUp">bool</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator MovingUpDown(bool isUp = true)
    {
        Transform target;
        float speed = (isUp) ? movingUpSpeed : fallingDownSpeed;

        if (_direction == "left")
        {
            target = (isUp) ? leftUpPoint : leftMovingPoint;
        } else
        {
            target = (isUp) ? rightUpPoint : rightMovingPoint;
        }

        if (isUp)
        {
            _audio.PlaySound(7);
        }
        else
        {
            _audio.PlaySound(8);
        }

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _movingUpDownRoutine = null;
    }

    /// <summary>
    /// Spawn projectiles from the sky.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ProjectilesAttack()
    {
        for (int i = 0; i < rounds; i++)
        {
            spawners.Shuffle();

            foreach (SkyProjectileSpawner spawner in spawners)
            {
                spawner.SpawnSkyProjectile();
                yield return new WaitForSeconds(timeBetweenProjectile);
            }
        }

        _projectilesAttack = null;
    }

    /// <summary>
    /// Increase movements speed.
    /// Called at AtHit boss event.
    /// </summary>
    public void IncreaseMovementSpeed()
    {
        if (hitsToDestroy == bossIncreasePhaseAtHits)
        {
            movingSpeed += 3f;
            rounds += 1;
        }
    }

    /// <summary>
    /// Boss main pattern attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PatternAttack()
    {
        if (_movingRoutine == null)
        {
            _movingRoutine = StartCoroutine(MovingSideToSide());
        }

        while (_movingRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        int randomNumber = Random.Range(0, 10);

        if (randomNumber > 5)
        {
            if (_movingRoutine == null)
            {
                _movingRoutine = StartCoroutine(MovingSideToSide());
            }

            while (_movingRoutine != null)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        if (_movingUpDownRoutine == null)
        {
            _movingUpDownRoutine = StartCoroutine(MovingUpDown());

            while (_movingUpDownRoutine != null)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        if (_projectilesAttack == null)
        {
            _projectilesAttack = StartCoroutine(ProjectilesAttack());

            while (_projectilesAttack != null)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForSeconds(1f);

        randomNumber = Random.Range(0, 10);
        
        if (randomNumber > 5)
        {
            ChangeDirection();
        }

        if (_movingUpDownRoutine == null)
        {
            _movingUpDownRoutine = StartCoroutine(MovingUpDown(false));

            while (_movingUpDownRoutine != null)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        _patternAttack = null;
    }

    /// <summary>
    /// Change movement direction.
    /// </summary>
    private void ChangeDirection()
    {
        if (_direction == "left")
        {
            sprite.flipX = true;
            _direction = "right";
        } else
        {
            sprite.flipX = false;
            _direction = "left";
        }
    }

    /// <summary>
    /// Check for moving animation.
    /// </summary>
    public void CheckForMovingAnim()
    {
        if (isMoving)
        {
            _anim.SetBool("IsMoving", true);
        }
        else
        {
            _anim.SetBool("IsMoving", false);
        }
    }

    /// <summary>
    /// Stop all attacks. This method
    /// will be normally called from OnDefeated Boss
    /// Unity Event.
    /// </summary>
    public void StopAllAttackCoroutines()
    {
        isMoving = false;
        _anim.SetBool("IsMoving", false);

        if (_patternAttack != null)
        {
            StopCoroutine(_patternAttack);
            _patternAttack = null;
        }

        if (_movingRoutine !=  null)
        {
            StopCoroutine(_movingRoutine);
            _movingRoutine = null;
        }

        if (_movingUpDownRoutine != null)
        {
            StopCoroutine(_movingUpDownRoutine);
            _movingUpDownRoutine = null;
        }

        if (_projectilesAttack != null)
        {
            StopCoroutine(_projectilesAttack);
            _projectilesAttack = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public new void Init()
    {
        base.Init();
        isMoving = true;
        _direction = "right";
    }
}
