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
    public bool overIcyFloor;

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

        if (overIcyFloor)
        {
            //float icySpeed = speed * 0.05f;
            _rigibody.AddForce(new Vector2(movement.x * speed, _rigibody.velocity.y));
        } else
        {
            _rigibody.velocity = new Vector2(movement.x * speed, _rigibody.velocity.y);
        }
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
    /// Player bounce.
    /// 
    /// Usually triggered from a bouncy
    /// item placed in map.
    /// </summary>
    public void Bounce()
    {
        _rigibody.velocity = new Vector2(_rigibody.velocity.x, jumpForce * 1.5f);
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
    /// Get if player is walking over icy
    /// floor.
    /// </summary>
    /// <returns>bool</returns>
    public bool IsInIcyFloor()
    {
        return overIcyFloor;
    }

    /// <summary>
    /// Set player walking over icy
    /// floor value.
    /// </summary>
    /// <param name="isIcy">bool</param>
    public void SetInIcyFloor(bool isIcy)
    {
        overIcyFloor = isIcy;
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

    /// <summary>
    /// Restrict player intpu only.
    /// </summary>
    public void RestrictPlayerInput()
    {
        canMove = false;
    }

    /// <summary>
    /// Allow player input.
    /// </summary>
    public void AllowPlayerInput()
    {
        canMove = true;
    }

    /// <summary>
    /// Enter door after collecting key.
    /// </summary>
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
