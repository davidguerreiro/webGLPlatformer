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

    // TODO: Write logic for failing spikes attack.
    // TOOD: Write logic for pattern two.
    // TODO: Write logic to use patterns in Update.
    // TODO: Assign phase two method to onHit event.
    // TODO: Create init boss animation.
    // TOOD: Add start battle method.

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
    /// Crusher return to initial Y position after performing
    /// an attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ReturnToMovingPoint()
    {
        while (Vector2.Distance(transform.position, returningPoint.position) < 0.01f) {
            Vector2.MoveTowards(transform.position, returningPoint.position, returningSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.localPosition = returningPoint.position;
        returningPoint.transform.parent = transform.parent;

        yield return new WaitForSeconds(.5f);
        _backToReturningPoint = null;
    }

    /// <summary>
    /// Falling attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator FallingAttack()
    {
        returningPoint.transform.parent = null;

        yield return new WaitForSeconds(secondsBeforeFall);

        _rigi.bodyType = RigidbodyType2D.Dynamic;
        _rigi.gravityScale = fallForce;

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

        _rigi.gravityScale = 0;
        _rigi.bodyType = RigidbodyType2D.Kinematic;

        _rigi.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(secondsFloor);

        _backToReturningPoint = StartCoroutine(ReturnToMovingPoint());

        while (_backToReturningPoint != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _stayInFloor = null;
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

        _moveCoroutine = StartCoroutine(MoveSideToSide());
        yield return new WaitForSeconds(secondsMoving);

        _fallingAttack = StartCoroutine(FallingAttack());

        while (_fallingAttack != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _patternAttack = null;
    }

    /// <summary>
    /// Change moving speed in boss phase two.
    /// </summary>
    public void IncreaseMovingSpeed()
    {
        if (hitsToDestroy == 1)
        {
            IncreaseBossPhase();
            movingSpeed = secondPhaseMovingSpeed;
        }
    }

}
