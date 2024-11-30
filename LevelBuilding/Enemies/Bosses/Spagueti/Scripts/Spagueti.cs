using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spagueti : Boss
{
    [Header("Stats")]
    public float speedGoingUp;
    public float speedGoingDown;

    [Header("Battle config")]
    public float toWaitBetweenAppear;
    public float toWaitBetweenFiringSplashBalls;
    public int bossIncreasePhaseAtHits;

    [Header("SplashFireAttack")]
    public SplashFireBallLauncher fireLauncher;

    [Header("MovingPoints")]
    public Transform rightUpMovingPoint;
    public Transform rightDownMovingPoint;
    public Transform leftUpMovingPoint;
    public Transform leftDownMovingPoint;

    private string _position;
    private float _originalDownSpeed;
    private Coroutine _showUpRoutine;
    private Coroutine _showDownRoutine;
    private Coroutine _attackLoopBehaviour;

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
            if (_attackLoopBehaviour == null)
            {
                _attackLoopBehaviour = StartCoroutine(BattleLoop());
            }

            CheckForMovingAnim();
        }
    }

    /// <summary>
    /// Change boss position before performing attack.
    /// </summary>
    public void SwitchSide()
    {
        if (_position == "right")
        {
            transform.position = leftDownMovingPoint.position;
            _position = "left";
        } else
        {
            transform.position = rightDownMovingPoint.position;
            _position = "right";
        }
    }

    /// <summary>
    /// Boss battle loop.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator BattleLoop()
    {
        yield return new WaitForSeconds(toWaitBetweenAppear);

        _showUpRoutine = StartCoroutine(ShowsUp());

        while (_showUpRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        LaunchFireBalls();
        _showDownRoutine = StartCoroutine(GoDown());

        if (GetCurrentBossPhase() == 1)
        {
            yield return new WaitForSeconds(toWaitBetweenFiringSplashBalls);
            LaunchFireBalls();
        }

        while (_showDownRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        int random = Random.Range(0, 20);

        if (random <= 9)
        {
            SwitchSide();
        }

        _attackLoopBehaviour = null;
    }

    /// <summary>
    /// Shows boss up.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ShowsUp()
    {
        _audio.PlaySound(4);

        Transform upperPoint = _position == "right" ? rightUpMovingPoint : leftUpMovingPoint;

        while (Vector2.Distance(transform.position, upperPoint.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, upperPoint.position, speedGoingUp * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = upperPoint.position;

        _showUpRoutine = null;
    }

    /// <summary>
    /// Move boss down water.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator GoDown()
    {
        Transform lowerPoint = _position == "right" ? rightDownMovingPoint : leftDownMovingPoint;

        while (Vector2.Distance(transform.position, lowerPoint.position) >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, lowerPoint.position, speedGoingDown * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = lowerPoint.position;
        RestoreGoingDownSpeed();
     
        _showDownRoutine = null;
    }

    /// <summary>
    /// Launch fire balls.
    /// </summary>
    /// <param name="number">int</param>
    public void LaunchFireBalls(int number = 2)
    {
        string direction = (_position == "right") ? "left" : "right";

        for (int i = 0; i < number; i++)
        {
            _audio.PlaySound(5);
            fireLauncher.ShootSplashBall(direction);
        }
    }

    /// <summary>
    /// Go down fast.
    /// </summary>
    public void GoDownFast()
    {
        if (inBattleLoop)
        {
            if (_showUpRoutine != null)
            {
                StopCoroutine(_showUpRoutine);
                _showUpRoutine = null;
            }

            IncreaseGoingDownSpeed();
        }
    }

    /// <summary>
    /// Increase going down speed.
    /// Usually performed after a hit.
    /// </summary>
    public void IncreaseGoingDownSpeed()
    {
        speedGoingDown *= 15f;
    }

    /// <summary>
    /// Restore going down speed to its original
    /// value.
    /// </summary>
    public void RestoreGoingDownSpeed()
    {
        speedGoingDown = _originalDownSpeed;
    }

    /// <summary>
    /// Checks if boss phase has to be
    /// increased. Called from HIT Unity 
    /// event.
    /// </summary>
    public void CheckIFIncreaseBossPhase()
    {
        if (hitsToDestroy == bossIncreasePhaseAtHits)
        {
            IncreaseBossPhase();
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

        if (_showUpRoutine != null)
        {
            StopCoroutine(_showUpRoutine);
            _showUpRoutine = null;
        }

        if (_showDownRoutine != null)
        {
            StopCoroutine(_showDownRoutine);
            _showDownRoutine = null;
        }

        if (_attackLoopBehaviour != null)
        {
            StopCoroutine(_attackLoopBehaviour);
            _attackLoopBehaviour = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public new void Init()
    {
        base.Init();
        isMoving = true;
        _position = "right";
        _originalDownSpeed = speedGoingDown;
    }
}
