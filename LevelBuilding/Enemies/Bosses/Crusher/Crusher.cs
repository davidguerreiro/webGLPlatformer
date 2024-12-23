using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : Boss
{
    [Header("Stats")]
    public float movingSpeed;
    public float returningSpeed;

    [Header("Falling Attack")]
    public float fallForce;
    public float secondsBeforeFall;
    public float secondsFloor;

    [Header("Failling Spikes Attack")]
    public int[] patternOne = new int[5];
    public int[] patternTwo = new int[5];
    public int[] patternThree = new int[5];
    public float waitBetweenPlatforms;

    [Header("Moving Points")]
    public Transform returningPoint;
    public Transform movingPointLeft;
    public Transform movingPointRight;
    public Vector2 movingSecondsRatio;

    [Header("Object pooling")]
    public ObjectPool[] faillingPlatformSpawners;

    [Header("Phase 2 Battle Settings")]
    public float secondPhaseMovingSpeed;

    private Coroutine _backToReturningPoint;
    private Coroutine _fallingAttack;
    private Coroutine _stayInFloor;
    private Coroutine _moveCoroutine;
    private Coroutine _patternAttack;
    private bool _canUseFallingSpikesAttack;

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
                _patternAttack = StartCoroutine(AttackPatternOne());
            }

            if (isMoving && _moveCoroutine == null)
            {
               _moveCoroutine = StartCoroutine(MoveSideToSide());
            }
        }

    }

    /// <summary>
    /// Crusher return to initial Y position after performing
    /// an attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ReturnToMovingPoint()
    {
        while (Vector2.Distance(transform.localPosition, returningPoint.localPosition) > 0.01f) {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, returningPoint.localPosition, returningSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.localPosition = returningPoint.localPosition;
        returningPoint.transform.parent = transform;

        yield return new WaitForSeconds(.5f);
        _backToReturningPoint = null;
    }

    /// <summary>
    /// Falling attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator FallingAttack()
    {
        returningPoint.transform.parent = transform.parent;

        yield return new WaitForSeconds(secondsBeforeFall);

        _rigi.bodyType = RigidbodyType2D.Dynamic;
        _rigi.gravityScale = fallForce;
        RemoveWeakPoints();
        yield return new WaitForSeconds(.2f);

        gameManager.cinematicManager.mainCamera.ShackeCamera(.5f);
        EnableWeakPoints();
      
        _stayInFloor = StartCoroutine(StayInFloor());

        while (_stayInFloor != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _fallingAttack = null;
    }

    /// <summary>
    /// Stay in floor mechanic logic.
    /// </summary>
    /// <returns>IEnumrator</returns>
    public IEnumerator StayInFloor()
    {
        _audio.PlaySound(1);

        // trigger throw failling spikes attack only once.
        if (_canUseFallingSpikesAttack)
        {
            StartCoroutine(ThrowFaillingSpikes());
            _canUseFallingSpikesAttack = false;
        }

        yield return new WaitForSeconds(.1f);
        _rigi.gravityScale = 0;
        _rigi.bodyType = RigidbodyType2D.Kinematic;
        yield return new WaitForSeconds(secondsFloor);

        _rigi.gravityScale = 0;
        _rigi.velocity = new Vector2(0f, 0f);


        _backToReturningPoint = StartCoroutine(ReturnToMovingPoint());

        while (_backToReturningPoint != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _stayInFloor = null;
    }

    /// <summary>
    /// Throw failling platforms.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ThrowFaillingSpikes()
    {
        int[] pattern = GetFaillingSpikesPattern();
        
        for (int i = 0; i < faillingPlatformSpawners.Length; i++)
        {
            GameObject faillingPlatform = faillingPlatformSpawners[pattern[i]].SpawnPrefab();
            faillingPlatform.transform.position = faillingPlatformSpawners[pattern[i]].transform.position;
            faillingPlatform.GetComponent<FallingHazard>().Drop();
            yield return new WaitForSeconds(waitBetweenPlatforms);
        }
      
    }

    /// <summary>
    /// Get failling spikes pattern used when
    /// performing this attack.
    /// </summary>
    /// <returns>int[]</returns>
    private int[] GetFaillingSpikesPattern()
    {
        int pattern = Random.Range(0, 2);
        int[] patternArray;

        switch (pattern)
        {
            case 0:
                patternArray = patternOne;
                break;
            case 1:
                patternArray = patternTwo;
                break;
            case 2:
                patternArray = patternThree;
                break;
            default:
                patternArray = patternOne;
                break;
        }

        return patternArray;
    }

    /// <summary>
    /// Move boss side to side before attacking.
    /// </summary>
    /// <returns>IEnumrator</returns>
    public IEnumerator MoveSideToSide()
    {
        Transform target = movingPointLeft;

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;
        target = movingPointRight;

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _moveCoroutine = null;
    }

    /// <summary>
    /// Get number of seconds the moving attack will be in place.
    /// </summary>
    /// <returns>float</returns>
    private float GetMovingSeconds()
    {
        return Random.Range(movingSecondsRatio.x, movingSecondsRatio.y);
    }

    /// <summary>
    /// Patter attack one.
    /// Used during boss phase 1.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator AttackPatternOne()
    {
        float secondsMoving = GetMovingSeconds();

        // move side to side.
        isMoving = true;
        yield return new WaitForSeconds(secondsMoving);

        isMoving = false;

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        _fallingAttack = StartCoroutine(FallingAttack());

        while (_fallingAttack != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _patternAttack = null;
    }

    /// <summary>
    /// Increase boss phase based on boss hits.
    /// Call this method in onHit boss Unity Event.
    /// </summary>
    public void IncreaseBossPhaseAfterHit()
    {
        if (hitsToDestroy == 2)
        {
            movingSpeed = secondPhaseMovingSpeed;
        }

        if (hitsToDestroy == 3 || hitsToDestroy == 2)
        {
            _canUseFallingSpikesAttack = true;
        }
    }

    /// <summary>
    /// Stop all attacks. This method
    /// will be normally called from OnDefeated Boss
    /// Unity Event.
    /// </summary>
    public void StopAllAttackCoroutines()
    {   
        if (_backToReturningPoint != null)
        {
            StopCoroutine(_backToReturningPoint);
            _backToReturningPoint = null;
        }

        if (_fallingAttack != null)
        {
            StopCoroutine(_fallingAttack);
            _fallingAttack = null;
        }

        if (_stayInFloor != null)
        {
            StopCoroutine(_stayInFloor);
            _stayInFloor = null;
        }

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        if (_patternAttack != null)
        {
            StopCoroutine(_patternAttack);
            _patternAttack = null;
        }

    }

}
