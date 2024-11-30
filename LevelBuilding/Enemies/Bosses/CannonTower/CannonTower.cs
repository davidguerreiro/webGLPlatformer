using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : Boss
{
    [Header("Stats")]
    public float speedGoingUp;

    [Header("Battle Config")]
    public int bossIncreasePhaseAtHits;

    [Header("Components")]
    public MovingCannon movingCannon;
    public Transform displayMovingPoint;

    [HideInInspector]
    public Coroutine showsUpRoutine;

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
            movingCannon.SetCannonActive(this);
            CheckForMovingAnim();
        }
    }

    /// <summary>
    /// Boss shows up.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator ShowsUp()
    {
        while (Vector2.Distance(transform.position, displayMovingPoint.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, displayMovingPoint.position, speedGoingUp * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = displayMovingPoint.position;

        showsUpRoutine = null;
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
    /// Init class method.
    /// </summary>
    public new void Init()
    {
        base.Init();
        isMoving = true;
    }
}
