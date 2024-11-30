using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingPlayerEnemy : Enemy
{
    [Header("Settings")]
    public float speed;
    public float awaitBeforeChase;
    public float awaitAfterChase;

    private Coroutine _isChasingRoutine;
    private Coroutine _appearCoroutine;
    protected bool _canMove;
    protected Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            CheckSpriteFlip();

            if (_isChasingRoutine == null)
            {
                _isChasingRoutine = StartCoroutine(ChasePlayer());
            }
        }
    }

    /// <summary>
    /// Appear enemy in game scene.
    /// </summary>
    public void Appear()
    {
        if (_appearCoroutine == null)
        {
            _appearCoroutine = StartCoroutine(AppearRoutine());
        }
    }

    /// <summary>
    /// Appear enemy in game scene coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator AppearRoutine()
    {
        _anim.SetBool("Appear", true);
        _audio.PlaySound(1);

        yield return new WaitForSeconds(awaitBeforeChase);

        EnableColliders();
        _canMove = true;

        _appearCoroutine = null;
    }

    /// <summary>
    /// Chase player.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ChasePlayer()
    {
        // TODO: Add play chase sound.
        Transform playerTransform = gameManager.player.gameObject.transform;

        while (Vector2.Distance(transform.position, playerTransform.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(awaitAfterChase);

        _isChasingRoutine = null;
    }

    /// <summary>
    /// Flips sprite in X axis based on player position.
    /// </summary>
    private void CheckSpriteFlip()
    {
        Transform playerTransform = gameManager.player.gameObject.transform;

        if (playerTransform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        } else
        {
            spriteRenderer.flipX = false;
        }
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

        if (_isChasingRoutine != null)
        {
            StopCoroutine(_isChasingRoutine);
            _isChasingRoutine = null;
        }

        if (_appearCoroutine != null)
        {
            StopCoroutine(_appearCoroutine);
            _appearCoroutine = null;
        }

        base.Defeated();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private new void Init()
    {
        base.Init();

        _anim = GetComponent<Animator>();
        RemoveColliders();
    }
}
