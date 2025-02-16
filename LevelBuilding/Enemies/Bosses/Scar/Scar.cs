using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scar : Boss
{
    [Header("Battle Config")]
    public float movingSpeed;
    public float toWaitTeleporting;
    public int increaseBossPhaseAt;

    [Header("Boss Components")]
    public SpriteRenderer bossSprite;
    public DarkPortal darkPortal;
    public GameObject face;
    public GameObject cristal;

    [Header("Attacks")]
    public EyesProjectileAttack cristalProyectilesAttack;

    [Header("Moving points")]
    public Transform topMiddle;
    public Transform topLeft;
    public Transform topRight;
    public Transform middleLeft;
    public Transform middleRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    private Coroutine _teleportRoutine;
    private Coroutine _moveCoroutine;
    private Coroutine _patternAttack;

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
                _patternAttack = StartCoroutine(PatternAttack());
            }
        }
    }

    /// <summary>
    /// Teleport ghost to a moving point
    /// in battle scene.
    /// </summary>
    /// <param name="toMove">Transform</param>
    public void TeleportToPoint(Transform toMove)
    {
        if (_teleportRoutine == null)
        {
            _teleportRoutine = StartCoroutine(TeleportToPointCoroutine(toMove));
        }
    }

    /// <summary>
    /// Teleport boss to a moving point in
    /// battle scene.
    /// </summary>
    /// <param name="toMove">Transform</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator TeleportToPointCoroutine(Transform toMove)
    {
        darkPortal.Appear();
        RemoveHazardPoints();
        RemoveWeakPoints();
        yield return new WaitForSeconds(.5f);

        bossSprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        darkPortal.Dissapear();


        yield return new WaitForSeconds(toWaitTeleporting);

        gameObject.transform.position = toMove.position;

        darkPortal.Appear();
        yield return new WaitForSeconds(.5f);

        EnableHazardPoints();
        EnableWeakPoints();
        bossSprite.gameObject.SetActive(true);

        darkPortal.Dissapear();
        yield return new WaitForSeconds(.1f);

        _teleportRoutine = null;
    }

    /// <summary>
    /// Move to position.
    /// </summary>
    /// <param name="target">Transform</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator MoveToPoint(Transform target)
    {
        // _audio.PlaySound(9);

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _moveCoroutine = null;
    }

    /// <summary>
    /// Boss main pattern attack coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PatternAttack()
    {
        yield return new WaitForSeconds(1f);

        StartCoroutine(cristalProyectilesAttack.TriggerCristalProyectilesAttackCoroutine("left"));

        while (cristalProyectilesAttack.inAttack)
        {
            yield return new WaitForFixedUpdate();
        }

        
        if (GetCurrentBossPhase() >= 1)
        {
            yield return new WaitForSeconds(.5f);
            StartCoroutine(cristalProyectilesAttack.TriggerCristalProyectilesAttackCoroutine("right"));

            while (cristalProyectilesAttack.inAttack)
            {
                yield return new WaitForFixedUpdate();
            }
        }

        yield return new WaitForSeconds(.5f);

        TeleportToPoint(middleRight);

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(middleLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        TeleportToPoint(bottomLeft);

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(topRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        TeleportToPoint(bottomRight);

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(topLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        TeleportToPoint(topRight);

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        TeleportToPoint(topMiddle);

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _patternAttack = null;
    }

    /// <summary>
    /// Logic to be called on boss hit.
    /// </summary>
    public void CalledOnHit()
    {
        if (hitsToDestroy <= increaseBossPhaseAt)
        {
            IncreaseBossPhase();
        }
    }

    /// <summary>
    /// Enable boss sprite.
    /// </summary>
    public void EnableBossSprite()
    {
        bossSprite.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disable boss sprite.
    /// </summary>
    public void DisableBossSprite()
    {
        bossSprite.gameObject.SetActive(false);
    }

    /// <summary>
    /// Disable face and cristal after defeated
    /// anim.
    /// </summary>
    public void RemoveFaceAndCristal()
    {
        face.SetActive(false);
        cristal.SetActive(false);
    }

    /// <summary>
    /// Stop all behaviour coroutines.
    /// </summary>
    public void StopAllAttackCoroutines()
    {
        isMoving = false;
        _anim.SetBool("IsMoving", false);

        if (_teleportRoutine != null)
        {
            StopCoroutine(_teleportRoutine);
            _teleportRoutine = null;
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
