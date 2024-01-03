using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;

    [Header("Components")]
    public SpriteRenderer sprite;

    private CircleCollider2D _collider;
    private AudioSource _audioSource;
    private bool _picked;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Trigger collision between the player
    /// and the coin.
    /// </summary>
    /// <param name="collision">Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_picked && collision.gameObject.CompareTag("Player"))
        {
            _picked = true;
            Player player = collision.gameObject.GetComponent<Player>();
            Collect(player);
        }
    }

    /// <summary>
    /// Collect coin.
    /// </summary>
    private void Collect(Player player)
    {
        sprite.enabled = false;
        _collider.enabled = false;

        _audioSource.Play();

        player.UpdateCoins(value);

        Invoke("Remove", .5f);
    }

    /// <summary>
    /// Remove coin gameobject from game
    /// scene.
    /// </summary>
    private void Remove()
    {
        Destroy(this);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _collider = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }
}
