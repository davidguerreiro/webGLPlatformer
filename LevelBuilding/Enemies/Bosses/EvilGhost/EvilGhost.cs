using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGhost : Boss
{
    [Header("Battle Config")]
    public float toWaitTeleporting;

    [Header("Boss Components")]
    public SpriteRenderer bossSprite;
    public DarkPortal darkPortal;
    public GameObject horns;
    public SpriteRenderer villainSprite;

    [Header("Eyes")]
    public Transform leftEye;
    public Transform rightEye;
    public Transform leftEyeLeft;
    public Transform leftEyeRight;
    public Transform rightEyeLeft;
    public Transform rightEyeRight;


    [Header("Attacks")]
    public SplashProjectileAttack splashProjectileAttack;
    public EyesProjectileAttack eyesProjectileAttack;

    [Header("Moving points")]
    public Transform topMiddle;
    public Transform topRight;
    public Transform topLeft;
    public Transform bottomRight;
    public Transform bottomLeft;

    private string _direction;
    private string _patternLeftToRight = "leftRoRight";
    private string _patternRightToLeft = "rightToLeft";
    private Coroutine _teleportRoutine;
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

            CheckForMovingAnim();
        }
    }

    /// <summary>
    /// Boss main pattern attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PatternAttack()
    {
        // teleport to start point.
        TeleportToPoint(topRight, "left");

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        // teleport to middle point.
        TeleportToPoint(topMiddle, "left");

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        // trigger splah attack.
        int triggerTimes = (GetCurrentBossPhase() >= 1) ? 3 : 2;

        for (int i = 0; i < triggerTimes; i++) {
            splashProjectileAttack.TriggerSplashAttack();

            while (splashProjectileAttack.inAttack)
            {
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(.1f);
        }

        // trigger up projectile attack.
        string projectilePattern = GetProjectilesAttacksPattern();

        if (projectilePattern == _patternLeftToRight)
        {
            TeleportToPoint(topLeft, "right");
        } else
        {
            TeleportToPoint(topRight, "left");
        }

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        string direction = (projectilePattern == _patternLeftToRight) ? "right" : "left";
        eyesProjectileAttack.TriggerEyesProjectileAttack(direction);

        while (eyesProjectileAttack.inAttack)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        // trigger horizontal projectile attack.
        if (projectilePattern == _patternLeftToRight)
        {
            TeleportToPoint(bottomRight, "left");
        }
        else
        {
            TeleportToPoint(bottomLeft, "right");
        }

        while (_teleportRoutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        direction = (projectilePattern == _patternLeftToRight) ? "left" : "right";
        eyesProjectileAttack.TriggerEyesProjectileHorizontalAttack(direction);

        while (eyesProjectileAttack.inAttack)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _patternAttack = null;
    }
    
    /// <summary>
    /// Teleport ghost to a moving point
    /// in battle scene.
    /// </summary>
    /// <param name="toMove">Transform</param>
    /// <param name="direction">string</param>
    public void TeleportToPoint(Transform toMove, string direction)
    {
        if (_teleportRoutine == null)
        {
            _teleportRoutine = StartCoroutine(TeleportToPointCoroutine(toMove, direction));
        }
    }

    /// <summary>
    /// Teleport ghost to a moving point in
    /// battle scene.
    /// </summary>
    /// <param name="toMove">Transform</param>
    /// <param name="direction">string</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator TeleportToPointCoroutine(Transform toMove, string direction)
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
        ChangeDirection(direction);

        darkPortal.Dissapear();
        yield return new WaitForSeconds(.1f);

        _teleportRoutine = null;
    }

    /// <summary>
    /// Change boss direction. 
    /// </summary>
    /// <param name="direction"></param>
    public void ChangeDirection(string direction)
    {
       if (direction == "left")
        {
            bossSprite.flipX = false;
            villainSprite.flipX = true;

            leftEye.transform.position = leftEyeLeft.position;
            rightEye.transform.position = rightEyeLeft.position;
        } else
        {
            bossSprite.flipX = true;
            villainSprite.flipX = false;

            leftEye.transform.position = leftEyeRight.position;
            rightEye.transform.position = rightEyeRight.position;
        }
    }

    /// <summary>
    /// Gets which projectiles attack boss will follow
    /// in current main attack pattern.
    /// </summary>
    /// <returns>string</returns>
    private string GetProjectilesAttacksPattern()
    {
        int rand = Random.Range(0, 10);

        if (rand < 5)
        {
            return _patternLeftToRight;
        }

        return _patternRightToLeft;
    }

    /// <summary>
    /// Logic to be called on boss hit.
    /// </summary>
    public void CalledOnHit()
    {
        StopAttacksAndPattern();

        if (hitsToDestroy <= 3)
        {
            IncreaseBossPhase();
        }
    }

    /// <summary>
    /// Disable animated red eyes
    /// before displaying defeated
    /// animation.
    /// </summary>
    public void CallBeforeDefeatedAnim()
    {
        leftEye.gameObject.SetActive(false);
        rightEye.gameObject.SetActive(false);
    }

    /// <summary>
    /// Stop all attacks and pattern
    /// attacks.
    /// </summary>
    public void StopAttacksAndPattern()
    {
        if (splashProjectileAttack.inAttack)
        {
            splashProjectileAttack.StopAttack();
        }

        if (eyesProjectileAttack)
        {
            eyesProjectileAttack.StopAttack();
        }

        if (_teleportRoutine != null)
        {
            StopCoroutine(_teleportRoutine);
            _teleportRoutine = null;
        }

        if (_patternAttack != null)
        {
            StopCoroutine(_patternAttack);
            _patternAttack = null;
        }
    }

    /// <summary>
    /// Stop all attacks. This method will normally be called
    /// from OnDefeated Boss Unity Event.
    /// </summary>
    public void StopAllAttackCoroutines()
    {
        isMoving = false;
        _anim.SetBool("IsMoving", false);

        StopAttacksAndPattern();
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
        _direction = "left";
    }
}
