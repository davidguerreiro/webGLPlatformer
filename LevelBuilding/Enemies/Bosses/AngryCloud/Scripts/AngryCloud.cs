using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryCloud : Boss
{
    [Header("Stats")]
    public float movingSpeed;

    [Header("Spark Tackle Attack")]
    public GameObject sparking;
    public float startSparkTackleAttackSpeed;
    public float spartkTackleAttackSpeed;
    public float waitBeforeAttack;
    public float waitBeforeReturningBack;
    public Transform originLeft;
    public Transform destinyLeft;
    public Transform origingRight;
    public Transform destinyRight;

    [Header("Spark Ball Shooter Attack")]
    public SparkBallShooter sparkBallShooter;

    [Header("Random Spark Attack")]
    public float timeSparkEnabled;
    public float toWaitBetweenSparks;

    [Header("Hazards")]
    public SparkCristal leftSparkCristal;
    public SparkCristal rightSparkCristal;

    [Header("Moving Points")]
    public Transform[] movingPoints;

    [Header("Face")]
    public GameObject redBack;
    public GameObject blueBack;

    private bool _inSparkAttack;
    private bool _canRandomSpark;
    private Coroutine _movingRoutine;
    private Coroutine _sparkAttackRoutine;
    private Coroutine _randomSparkAttackRoutine;

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
            if (_movingRoutine == null && _sparkAttackRoutine == null && ! _inSparkAttack)
            {
                _movingRoutine = StartCoroutine(MovePointToPoint());
            }

            if (_canRandomSpark && _randomSparkAttackRoutine == null && _sparkAttackRoutine == null && !_inSparkAttack)
            {
                _randomSparkAttackRoutine = StartCoroutine(RandomSpark());
            }

            CheckForMovingAnim();
        }
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

            ShootSparkBall();
        }

        _movingRoutine = null;
    }

    /// <summary>
    /// Spark attack coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PerformSparkAttack()
    {
        RemoveWeakPoints();

        int rand = Random.Range(0, 9);
        Transform origin;
        Transform target;
        Transform returnBack = movingPoints[0];

        if (rand < 4)
        {
            origin = originLeft;
            target = destinyRight;
        }
        else
        {
            origin = origingRight;
            target = destinyLeft;
        }

        EnableSparking();

        // enable sparking cristals.
        leftSparkCristal.ShowLighting();
        rightSparkCristal.ShowLighting();

        // move to origin.
        while (Vector2.Distance(transform.position, origin.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, origin.position, startSparkTackleAttackSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = origin.position;

        yield return new WaitForSeconds(waitBeforeAttack);

        // perform attack.
        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, spartkTackleAttackSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;
        yield return new WaitForSeconds(waitBeforeReturningBack);

        // return back.
        while (Vector2.Distance(transform.position, returnBack.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, returnBack.position, startSparkTackleAttackSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = returnBack.position;
        DisableSparking();

        yield return new WaitForSeconds(.1f);

        // disable spark cristals.
        leftSparkCristal.HideLighting();
        rightSparkCristal.HideLighting();

        _inSparkAttack = false;
        EnableWeakPoints();

        _sparkAttackRoutine = null;
    }

    /// <summary>
    /// Trigger spark attack during battle.
    /// </summary>
    public void TriggerSparkAttack()
    {
        if (hitsToDestroy > 0)
        {
            _inSparkAttack = true;

            if (_movingRoutine != null)
            {
                StopCoroutine(_movingRoutine);
                _movingRoutine = null;
            }

            _sparkAttackRoutine = StartCoroutine(PerformSparkAttack());
        }
    }

    /// <summary>
    /// Random Spark attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator RandomSpark()
    {
        EnableSparking();
        yield return new WaitForSeconds(timeSparkEnabled);

        DisableSparking();
        yield return new WaitForSeconds(toWaitBetweenSparks);

        _randomSparkAttackRoutine = null;
    }

    /// <summary>
    /// Enable sparking hazard.
    /// </summary>
    public void EnableSparking()
    {
        sparking.SetActive(true);
        EnableBlueFace();
    }

    /// <summary>
    /// Disable sparking hazard.
    /// </summary>
    public void DisableSparking()
    {
        sparking.SetActive(false);
        DisableBlueFace();
    }

    /// <summary>
    /// Shoot spark ball.
    /// </summary>
    public void ShootSparkBall()
    {
        _audio.PlaySound(5);
        sparkBallShooter.ShootSparkBall();
    }

    /// <summary>
    /// Increase boss phase based on boss hits.
    /// Call this method in onHit boss Unity Event.
    /// </summary>
    public void IncreaseBossPhaseAfterHit()
    {
        if (hitsToDestroy == 2)
        {
            _canRandomSpark = true;
        }
    }

    /// <summary>
    /// Enable blue back face animation.
    /// </summary>
    public void EnableBlueFace()
    {
        blueBack.SetActive(true);
    }

    /// <summary>
    /// Disable blue back face animation.
    /// </summary>
    public void DisableBlueFace()
    {
        blueBack.SetActive(false);
    }

    /// <summary>
    /// Disable both face animation.
    /// </summary>
    public void DisableBothFace()
    {
        DisableBlueFace();
        redBack.SetActive(false);
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

        // disable spark cristals.
        leftSparkCristal.HideLighting();
        rightSparkCristal.HideLighting();

        if (_movingRoutine != null)
        {
            StopCoroutine(_movingRoutine);
            _movingRoutine = null;
        }

        if (_sparkAttackRoutine != null)
        {
            StopCoroutine(_sparkAttackRoutine);
            _sparkAttackRoutine = null;
        }

        if (_randomSparkAttackRoutine != null)
        {
            StopCoroutine(_randomSparkAttackRoutine);
            _sparkAttackRoutine = null;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public new void Init()
    {
        base.Init();
        isMoving = true;
        _canRandomSpark = false;
    }
}
