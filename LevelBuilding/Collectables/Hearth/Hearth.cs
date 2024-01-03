using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour
{
    public int value;
    public GameObject sprite;

    private CircleCollider2D _collider;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Collision logic.
    /// </summary>
    /// <param name="collision">Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Collect(player);
        }
    }

    /// <summary>
    /// Collect hearth by player.
    /// </summary>
    /// <param name="player">Player</param>
    private void Collect(Player player)
    {
        _collider.enabled = false;
        sprite.SetActive(false);
        _audio.Play();

        player.UpdateHealth(value);

        Destroy(this, 3f);
    }


    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _collider = GetComponent<CircleCollider2D>();
        _audio = GetComponent<AudioSource>();
    }

}
