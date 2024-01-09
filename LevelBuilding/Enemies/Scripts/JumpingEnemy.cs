using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    [Header("Jumping Settings")]
    public float secondsBeforeJumping;
    public float secondsAfterJumping;
    public float jumpForce;

    private bool _canMove;
    private Animator _anim;
    private Coroutine _jumpRoutine;

    // Start is called before the first frame update
    void Start()
    {
        Init();   
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.inGamePlay && _canMove && _jumpRoutine == null)
        {
            _jumpRoutine = StartCoroutine(Jump());
        }
    }

    /// <summary>
    /// Enemy Jump.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator Jump()
    {
        yield return new WaitForSeconds(secondsBeforeJumping);

        _audio.PlaySound(1);
        _rigi.velocity = new Vector2(0f, jumpForce);
        yield return new WaitForSeconds(secondsAfterJumping);

        _jumpRoutine = null;
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

        if (_jumpRoutine != null)
        {
            StopCoroutine(_jumpRoutine);
            _jumpRoutine = null;
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
        _canMove = true;
    }
}
