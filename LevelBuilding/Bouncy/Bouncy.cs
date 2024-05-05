using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    public Sprite baseSprite;
    public Sprite actionSprite;

    private AudioComponent _audio;
    private SpriteRenderer _renderer;
    private Coroutine _bouncePlayer;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Trigger 2D collisions for this
    /// collider.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _bouncePlayer == null)
        {
            _bouncePlayer = StartCoroutine(BouncePlayer(collision.gameObject));
        }
    }

    /// <summary>
    /// Trigger bounce player and bounce animation.
    /// </summary>
    /// <param name="player">GameObject</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator BouncePlayer(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        _audio.PlaySound();
        playerController.Bounce();

        _renderer.sprite = actionSprite;
        yield return new WaitForSeconds(.1f);
        _renderer.sprite = baseSprite;

        _bouncePlayer = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _renderer = GetComponent<SpriteRenderer>();
    }
}
