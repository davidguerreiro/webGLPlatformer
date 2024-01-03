using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSideToSideEnemy : Enemy
{
    [Header("Moving Settings")]
    public float speed;

    [Header("Moving Points")]
    public Transform leftPoint;
    public Transform rightPoint;

    private bool _canMove;
    private Animator _anim;
    private Coroutine _moveRoutine;

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
    /// Init class method.
    /// </summary>
    private new void Init()
    {
        base.Init();

        _anim = GetComponent<Animator>();
        _canMove = true;
    }

    /// <summary>
    /// Moving enemy side to side.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator Moving()
    {
        Transform target = leftPoint;

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;
        target = rightPoint;
        spriteRenderer.flipX = !spriteRenderer.flipX;

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;
        spriteRenderer.flipX = !spriteRenderer.flipX;

        _moveRoutine = null;
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
