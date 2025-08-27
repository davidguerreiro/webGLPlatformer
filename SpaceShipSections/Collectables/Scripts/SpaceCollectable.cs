using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCollectable : MonoBehaviour
{
    [Header("Configuration")]
    public float speed;
    public bool recoversEnergy;
    public bool recoversLife;

    [Header("Components")]
    public GameObject sprite;

    private AudioComponent audio;
    private CircleCollider2D circleCollider;
    private bool canMove;
    private bool canBeCollected;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            MoveForward();
        }
    }

    /// <summary>
    /// Collect collectable by player.
    /// </summary>
    /// <param name="player">PLayerShip</param>
    public void Collected(PlayerShip player)
    {
        canBeCollected = false;

        audio.PlaySound(0);

        canMove = false;
        sprite.SetActive(false);
        circleCollider.enabled = false;

        if (recoversEnergy)
        {
            player.UpdateHealth(1);
        }

        if (recoversLife)
        {
            player.UpdateLife(1);
        }

        Destroy(this, 3f);
    }

    /// <summary>
    /// Trigger collision controler.
    /// </summary>
    /// <param name="collision">Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBeCollected)
        {
            Collected(collision.gameObject.GetComponent<PlayerShip>());
        }
    }

    /// <summary>
    /// Move collectable forward in game scene.
    /// </summary>
    private void MoveForward()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        audio = GetComponent<AudioComponent>();
        circleCollider = GetComponent<CircleCollider2D>();
        canMove = true;
        canBeCollected = true;
    }


}
