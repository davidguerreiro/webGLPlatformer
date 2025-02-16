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
    public ParticleSystem topProyectilesParticles;

    [Header("Moving Points")]
    public Transform topRight;
    public Transform topMiddle;
    public Transform topLeft;
    public Transform middle;
    public Transform bottomRight;
    public Transform bottomMiddle;
    public Transform bottomLeft;

    [Header("Top Proyectile Spawners")]
    public SkyProjectileSpawner[] topProyectileSpawners;

    [Header("Right Proyectile Spawners")]
    public SkyProjectileSpawner[] rightProyectileSpawners;

    [Header("Left Proyectile Spawners")]
    public SkyProjectileSpawner[] leftProyectileSpawners;

    private bool _flameIn;
    private Coroutine _moveCoroutine;
    private Coroutine _flameCounterRoutine;
    private Coroutine _moveProyectileAttack;
    private Coroutine _middleSkyProyectleAttack;
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
            if (_flameCounterRoutine == null)
            {
                _flameCounterRoutine = StartCoroutine(CountFlame());
            }

            if (_patternAttack == null)
            {
                SelectAttackPattern();
            }
        }

        CheckForMovingAnim();
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
        _audio.PlaySound(9);

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
        float secondsToWait = _flameIn ? secondsWithFlame : secondsWithOutFlame;

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
    /// Enable right sky proyectile spawners.
    /// </summary>
    public void EnableRightSkyProyectiles()
    {
        foreach (SkyProjectileSpawner spawner in rightProyectileSpawners)
        {
            spawner.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Disable right sky proyectile spawners.
    /// </summary>
    public void DisableRightSkyProyectiles()
    {
        foreach (SkyProjectileSpawner spawner in rightProyectileSpawners)
        {
            spawner.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Enable left sky proyectile spawners.
    /// </summary>
    public void EnableLeftSkyProyectiles()
    {
        foreach (SkyProjectileSpawner spawner in leftProyectileSpawners)
        {
            spawner.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Disable left sky proyectile spawners.
    /// </summary>
    public void DisableLeftSkyProyectiles()
    {
        foreach (SkyProjectileSpawner spawner in leftProyectileSpawners)
        {
            spawner.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Spawn top sky proyectiles attack.
    /// </summary>
    public void TriggerTopSkyProyectiles()
    {
        foreach(SkyProjectileSpawner spawner in topProyectileSpawners)
        {
            spawner.SpawnSkyProjectile();
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
    /// Move proyectiles attack. Boss moves top right to top left
    /// throwing proyectiles when moving. Then moves back to top
    /// right throwing proyectiles again.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MoveProyectilesAttack()
    {
        _moveCoroutine = StartCoroutine(MoveToPoint(topRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        EnableRightSkyProyectiles();

        _moveCoroutine = StartCoroutine(MoveToPoint(topLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DisableRightSkyProyectiles();
        EnableLeftSkyProyectiles();

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(topRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DisableLeftSkyProyectiles();

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(middle));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _moveProyectileAttack = null;
    }

    /// <summary>
    /// Trigger middle top sky proyectiles
    /// attack.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MiddleTopSkyProyectilesAttack()
    {
        yield return new WaitForSeconds(.5f);

        topProyectilesParticles.Play();
        _audio.PlaySound(9);
        yield return new WaitForSeconds(.1f);

        TriggerTopSkyProyectiles();
        yield return new WaitForSeconds(1.5f);

        _middleSkyProyectleAttack = null;
    }

    /// <summary>
    /// Pattern attack one.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator PatternAttackOne()
    {
        // Pattern:
        // Move top right.
        // Move bottom right.
        // Wait
        // Move left, then right again.
        // Wait.
        // Move middle.
        // Trigger middle sky proyectiles attack.
        // Trigger top sky proyectiles attack.
        // Move middle.

        _moveCoroutine = StartCoroutine(MoveToPoint(topRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(middle));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(topRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveProyectileAttack = StartCoroutine(MoveProyectilesAttack());

        while (_moveProyectileAttack != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);


        _moveCoroutine = StartCoroutine(MoveToPoint(middle));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _middleSkyProyectleAttack = StartCoroutine(MiddleTopSkyProyectilesAttack());

        while (_middleSkyProyectleAttack != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _patternAttack = null;
    }

    public IEnumerator patternAttackTwo()
    {
        // Pattern:
        // Move top right.
        // Perform sky proyectiles attack.
        // Perform sky proyectoles attack again.
        // Move bottom right.
        // Perform bottom dash attack.
        // Perform bottom dash attack.
        // Move top right.
        // Move middle.
        // Perform middle top sky proyectiles attack.

        _moveProyectileAttack = StartCoroutine(MoveProyectilesAttack());

        while (_moveProyectileAttack != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(topLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(middle));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _middleSkyProyectleAttack = StartCoroutine(MiddleTopSkyProyectilesAttack());

        while (_middleSkyProyectleAttack != null)
        {
            yield return new WaitForFixedUpdate();
        }

        _patternAttack = null;
    }

    public IEnumerator PatternAttackThree()
    {
        // Pattern:
        // Move top right
        // Move bottom right
        // Perform dash attack
        // Perform dash attack again
        // Move top right
        // Move Middle
        // Perform middle top proyectiles attack
        // Move top right
        // Perform top proyectiles attack
        // Move middle

        _moveCoroutine = StartCoroutine(MoveToPoint(topRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomLeft));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveCoroutine = StartCoroutine(MoveToPoint(bottomRight));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(middle));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        _moveProyectileAttack = StartCoroutine(MoveProyectilesAttack());

        while (_moveProyectileAttack != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _moveCoroutine = StartCoroutine(MoveToPoint(middle));

        while (_moveCoroutine != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        _patternAttack = null;
    }

    /// <summary>
    /// Select pattern attack to be performed
    /// by boss.
    /// </summary>
    private void SelectAttackPattern()
    {
        float random = Random.Range(0, 99);

        if (random < 33f)
        {
            _patternAttack = StartCoroutine(PatternAttackOne());
        } else if (random < 66f)
        {
            _patternAttack = StartCoroutine(patternAttackTwo());
        } else
        {
            _patternAttack = StartCoroutine(PatternAttackThree());
        }
    }

    /// <summary>
    /// Stop all coroutines.
    /// </summary>
    public void StopAllAttackCoroutines()
    {
        isMoving = false;
        _anim.SetBool("IsMoving", false);

        if (_patternAttack != null)
        {
            StopCoroutine(_patternAttack);
            _patternAttack = null;
        }

        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        if (_flameCounterRoutine!= null)
        {
            StopCoroutine(_flameCounterRoutine);
            _flameCounterRoutine = null;
        }

        if (_moveProyectileAttack != null)
        {
            StopCoroutine(_moveProyectileAttack);
            _moveProyectileAttack = null;
        }

        if (_middleSkyProyectleAttack != null)
        {
            StopCoroutine(_middleSkyProyectleAttack);
            _middleSkyProyectleAttack = null;
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

        DisableLeftSkyProyectiles();
        DisableRightSkyProyectiles();
    }

}
