using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsoBuko : Boss
{
    [Header("Battle Config")]
    public float movingSpeed;
    public float secondsWithFlame;
    public float secondsWithOutFlame;

    [Header("Boss Components")]
    public EternityFlame flame;

    [Header("Moving Points")]
    public Transform topRight;
    public Transform topMiddle;
    public Transform topLeft;
    public Transform middle;
    public Transform bottomRight;
    public Transform bottomMiddle;
    public Transform bottomLeft;

    [Header("Top Proyectile Spawners")]
    public SkyProjectileSpawner[] proyectileSpawners;

    private bool _flameIn;
    private Coroutine _moveCoroutine;
    private Coroutine _flameCounterRoutine;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        // TODO: Add battle patterns.
        // TODO: Add falling proyectiles attack.
    }

    // Update is called once per frame
    void Update()
    {
        if (inBattleLoop)
        {
            if (_flameCounterRoutine == null)
            {
                _flameCounterRoutine = StartCoroutine(CountFlame());
            }

            // Aadd attack patterns here.
            CheckForMovingAnim();
        } 
    }

    /// <summary>
    /// Show eternity flame.
    /// </summary>
    public void ShowFlame()
    {
        boxColliders[0].enabled = true;
        flame.Appear();

        _flameIn = true;
    }

    /// <summary>
    /// Hide flame.
    /// </summary>
    public void HideFlame()
    {
        boxColliders[0].enabled = false;
        flame.Dissapear();

        _flameIn = false;
    }

    /// <summary>
    /// Move to position.
    /// </summary>
    /// <param name="target">Transform</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator MoveToPoint(Transform target)
    {
        isMoving = true;

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _moveCoroutine = null;
    }

    /// <summary>
    /// Count when flame has to appear
    /// or dissapear.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator CountFlame()
    {
        float secondsToWait = _flameIn ? secondsWithOutFlame : secondsWithFlame;

        yield return new WaitForSeconds(secondsToWait);

        if (_flameIn)
        {
            HideFlame();
        } else
        {
            ShowFlame();
        }

        _flameCounterRoutine = null;
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
    /// Init class method.
    /// </summary>
    public new void Init()
    {
        base.Init();
        isMoving = true;
        _flameIn = true;
    }

}
