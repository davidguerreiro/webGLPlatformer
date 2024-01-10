using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlockEnemy : Enemy
{
    [Header("Failling Block Components")]
    public Transform movingPoint;

    [Header("Failing Block Settings")]
    public float speedBack;
    public float fallForce;
    public float secondsAbove;
    public float secondsFloor;

    private bool _canMove;
    private bool _isUp;
    private Coroutine _fallRoutine;
    private Coroutine _moveRoutine;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove && gameManager.inGamePlay && _fallRoutine == null && _isUp)
        {
            _fallRoutine = StartCoroutine(FallDown());
        }
    }

    /// <summary>
    /// Fall down.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator FallDown()
    {
        _isUp = false;
        yield return new WaitForSeconds(secondsAbove);

        _rigi.bodyType = RigidbodyType2D.Dynamic;
        _rigi.gravityScale = fallForce;

        _fallRoutine = null;
    }

    /// <summary>
    /// Return up to initial failling point.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ReturnUp()
    {
        _rigi.gravityScale = 0;
        _rigi.bodyType = RigidbodyType2D.Kinematic;

        _rigi.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(secondsFloor);

        while (Vector2.Distance(transform.position, movingPoint.position) > 0.01f) 
        {
            transform.position = Vector2.MoveTowards(transform.position, movingPoint.position, speedBack * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = movingPoint.position;
        _isUp = true;
        _moveRoutine = null;
    }

    /// <summary>
    /// On collision enter 2D.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("IsGround"))
        {
            if (!_isUp && _moveRoutine == null)
            {
                _moveRoutine = StartCoroutine(ReturnUp());
            }
        }
    }
    /// <summary>
    /// Defeated enemy.
    /// </summary
    public override void Defeated()
    {
        gameObject.tag = "Untagged";
        isAlive = false;
     
        _canMove = false;

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        if (_fallRoutine != null)
        {
            StopCoroutine(_fallRoutine);
            _fallRoutine = null;
        }

        _rigi.gravityScale = 1;
        base.Defeated();
    }


    /// <summary>
    /// Init class method.
    /// </summary>
    private new void Init()
    {
        base.Init();

        _canMove = true;
        _isUp = true;
    }
}
