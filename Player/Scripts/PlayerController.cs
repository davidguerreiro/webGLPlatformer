using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    public bool canMove;
    public bool isGrounded;
    public bool enterDoor;

    [Header("Stats")]
    public float speed;
    public float jumpForce;

    [Header("Components")]
    public PlayerCheckGround rightPlayerCheckGround;
    public PlayerCheckGround leftPlayerCheckGround;

    [Header("Events")]
    public UnityEvent jumpEvent;

    [HideInInspector]
    public float xMove;

    private Rigidbody2D _rigibody;
    private BoxCollider2D _boxCollider;
    private CapsuleCollider2D _capsuleCollider;
    private float _baseGravityScale;

    private void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown("space"))
            {
                Jump();
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }

        UpdateGrounded();
    }

    /// <summary>
    /// Move character using
    /// basic input and rigibody.
    /// </summary>
    public void Move()
    {
        xMove = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(xMove, _rigibody.velocity.y);

        _rigibody.velocity = new Vector2(movement.x * speed, _rigibody.velocity.y);
    }

    /// <summary>
    /// Player jump logic.
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
        {
            _rigibody.velocity = new Vector2(_rigibody.velocity.x, jumpForce);

            jumpEvent?.Invoke();
        }
    }

    /// <summary>
    /// Player enemy defeated recoil.
    /// </summary>
    public void EnemyDefeatedRecoil()
    {
        float force = Input.GetKey("space") ? jumpForce * 1.3f : jumpForce / 2f;
        _rigibody.velocity = new Vector2(_rigibody.velocity.x, 0f);
        _rigibody.velocity = new Vector2(_rigibody.velocity.x, force);
    }

    /// <summary>
    /// Update player grounded status.
    /// </summary>
    public void UpdateGrounded()
    {
        if (leftPlayerCheckGround.GetGrounded() || rightPlayerCheckGround.GetGrounded())
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// Allow player control.
    /// </summary>
    public void AllowControl()
    {
        canMove = true;

        _rigibody.gravityScale = _baseGravityScale;
        _boxCollider.enabled = true;
        _capsuleCollider.enabled = true;
    }

    /// <summary>
    /// Restrict player control.
    /// </summary>
    public void RestrictControl()
    {
        canMove = false;

        _rigibody.velocity = new Vector2(0f, 0f);
        _rigibody.gravityScale = 0f;
        _boxCollider.enabled = false;
        _capsuleCollider.enabled = false;
    }

    public void EnterDoor()
    {
        RestrictControl();
        _rigibody.gravityScale = 0f;
        _boxCollider.enabled = false;
        _capsuleCollider.enabled = false;

        enterDoor = true;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _rigibody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        enterDoor = false;

        _baseGravityScale = 5;
    }
}
