using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCannon : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public string direction;            // Accepted values: "right" or "left"
    public float toWaitBeforeShooting;
    public float toWaitAfterShooting;
    public string initialPosition;      // Accepted values: "top" or "bottom"
    public int minMoveTimes;
    public int maxMoveTimes;

    [Header("Components")]
    public Transform movingPointUp;
    public Transform movingPointDown;
    public GameObject fireParticles;
    public ObjectPool objectPool;

    private string _currentPosition;
    private AudioComponent _audioComponent;
    private Coroutine _moveRoutine;
    private Coroutine _shootRoutine;
    private Coroutine _patternRoutine;
    private int _timesToMove;
    private bool _active;
    private CannonTower _parentBoss;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            if (_patternRoutine == null)
            {
                _patternRoutine = StartCoroutine(CannonPattern());
            }
        }
    }

    /// <summary>
    /// Cannon movement and attack routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator CannonPattern()
    {
        _timesToMove = GetMoveTimes();

        // move up - down.
        for (int i = 0; i < _timesToMove; i++)
        {
            _moveRoutine = StartCoroutine(MoveToPosition());

            while(_moveRoutine != null)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        // shoot cannon.
        _shootRoutine = StartCoroutine(ShootCannonBall());

        while (_shootRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _patternRoutine = null;
    }

    /// <summary>
    /// Move cannon to up or down
    /// position.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveToPosition()
    {
        Transform target = (_currentPosition == "top") ? movingPointDown : movingPointUp;

        while (Vector2.Distance(transform.position, target.position) > 0.01f) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;
        _currentPosition = (_currentPosition == "top") ? "bottom" : "top";

        _moveRoutine = null;
    }

    /// <summary>
    /// Shoot cannon ball.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ShootCannonBall()
    {
        yield return new WaitForSeconds(toWaitBeforeShooting);

        GameObject ball = objectPool.SpawnPrefab();

        if (ball)
        {
            ball.transform.position = transform.position;

            CannonProyectile proyectile = ball.GetComponent<CannonProyectile>();
            proyectile.SetDirection(direction);
            _audioComponent.PlaySound();
        }

        yield return new WaitForSeconds(toWaitAfterShooting);

        _shootRoutine = null;
    }

    /// <summary>
    /// Set cannon active.
    /// </summary>
    /// <param name="boss">CannonTower</param>
    public void SetCannonActive(CannonTower boss)
    {
        _active = true;
        _parentBoss = boss;
    }

    /// <summary>
    /// Stop cannnon, usually called
    /// when boss is defeated.
    /// </summary>
    public void StopCannon()
    {
        _active = false;
        fireParticles.SetActive(false);
        StopCoroutines();
    }

    /// <summary>
    /// Stop all cannon coroutines.
    /// </summary>
    public void StopCoroutines()
    {
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        if (_shootRoutine != null)
        {
            StopCoroutine(_shootRoutine);
            _shootRoutine = null;
        }

        if (_patternRoutine != null)
        {
            StopCoroutine(_patternRoutine);
            _patternRoutine = null;
        }
    }

    /// <summary>
    /// Return move times for this run.
    /// </summary>
    /// <returns>int</returns>
    private int GetMoveTimes()
    {
        return Random.Range(minMoveTimes, maxMoveTimes);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _currentPosition = initialPosition;
        _audioComponent = GetComponent<AudioComponent>();
    }
}
