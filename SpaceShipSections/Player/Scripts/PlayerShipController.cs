using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

public class PlayerShipController : MonoBehaviour
{
    [Header("Status")]
    public bool canMove;

    [Header("Stats")]
    public float fireCadence;

    [Header("Components")]
    public SpaceCannon cannon;

    [Header("Events")]
    public UnityEvent atShoot;

    [HideInInspector]
    public float xMove;

    [HideInInspector]
    public float yMove;

    [HideInInspector]
    public Rewired.Player rewiredPlayer;

    private PlayerShip player;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rigibody;

    // Update is called once per frame
    void Update()
    {
        if (canMove && (rewiredPlayer.GetButtonDown("Jump") || rewiredPlayer.GetButtonDown("Cancel")))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    /// <summary>
    /// Move ship.
    /// </summary>
    public void Move()
    {
        xMove = rewiredPlayer.GetAxis("MoveHorizontal");
        yMove = rewiredPlayer.GetAxis("MoveVertical");

        Vector2 movement = new Vector2(xMove, yMove);

        rigibody.velocity = new Vector2(movement.x * player.speed, movement.y * player.speed);   
    }

    /// <summary>
    /// Restore speed.
    /// </summary>
    public void RestoreSpeed()
    {
        rigibody.velocity = new Vector2(0, 0);
    }

    /// <summary>
    /// Shoot proyectile.
    /// </summary>
    public void Shoot()
    {
        cannon.Shoot();

        atShoot?.Invoke();
    }

    /// <summary>
    /// Allow player ship control.
    /// </summary>
    public void AllowControl()
    {
        canMove = true;

        circleCollider.enabled = true;
    }

    /// <summary>
    /// Remove player ship control.
    /// </summary>
    public void RestrictControl()
    {
        canMove = false;

        circleCollider.enabled = false;

        RestoreSpeed();
    }

    /// <summary>
    /// Allow player input for playing.
    /// </summary>
    public void AllowPlayerInput()
    {
        canMove = true;
    }

    /// <summary>
    /// Restrict player input for playing.
    /// </summary>
    public void RestrictPlayerControl()
    {
        canMove = false;
        RestoreSpeed();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="player">PlayerShip</param>
    public void Init(PlayerShip player)
    {
        this.player = player;

        rigibody = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        cannon.Init(fireCadence);

        rewiredPlayer = ReInput.players.GetPlayer(0); 
    }
}
