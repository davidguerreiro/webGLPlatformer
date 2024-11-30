using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Components")]
    public SpriteRenderer sprite;
    public GameObject particles;

    private BoxCollider2D _collider;
    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Trigger collision with player logic.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Collect(player);
        }
    }

    /// <summary>
    /// Collect key by player.
    /// </summary>
    /// <param name="player">Player</param>
    private void Collect(Player player)
    {
        _collider.enabled = false;
        sprite.enabled = false;
        particles.SetActive(false);
        player.playerController.TriggerCollectKeyVibration();

        player.GetKey();

        _audio.PlaySound(0);

        Destroy(this, 2f);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _collider = GetComponent<BoxCollider2D>();
        _audio = GetComponent<AudioComponent>();
    }
}
