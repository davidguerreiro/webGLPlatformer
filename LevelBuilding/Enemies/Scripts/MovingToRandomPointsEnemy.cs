using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToRandomPointsEnemy : Enemy
{
    [Header("Moving Settings")]
    public float speed;

    [Header("MovingPoints")]
    public Transform[] movingPoints;

    private bool _canMove;
    private Animator _anim;
    private Coroutine _moveRoutine;
    private int _lastPoint;

    // Start is called before the first frame update
    void Start()
    {
        Init();   
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove && _moveRoutine == null)
        {
            _moveRoutine = StartCoroutine(Moving());
        }
    }

    /// <summary>
    /// Get next moving target
    /// Transform.
    /// </summary>
    /// <returns>Transform</returns>
    private Transform GetTarget()
    {
        int index;

        do
        {
            index = Random.Range(0, movingPoints.Length);
        } while (index == _lastPoint);

        _lastPoint = index;

        return movingPoints[index];
    }

    /// <summary>
    /// Move coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator Moving()
    {
        Transform target = GetTarget();
        CheckOrientation(target);

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _moveRoutine = null;
    }

    /// <summary>
    /// Check sprite orientation
    /// when moving to point.
    /// </summary>
    /// <param name="target">Transform</param>
    private void CheckOrientation(Transform target)
    {
        spriteRenderer.flipX = (target.position.x > transform.position.x) ? true : false;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private new void Init()
    {
        base.Init();

        _anim = GetComponent<Animator>();
        _canMove = true;
        _lastPoint = 0;
    }

    /// <summary>
    /// Defeated enemy.
    /// </summary
    public override void Defeated()
    {
        gameObject.tag = "Untagged";
        isAlive = false;

        _anim.enabled = false;
        _canMove = false;

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        base.Defeated();
    }
}
