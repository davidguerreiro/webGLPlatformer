using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;
    public SpriteRenderer headerSprite;
    public SpriteRenderer bodySprite;

    [Header("Animation")]
    public Sprite headerOpenedSprite;
    public Sprite bodyOpenedSprite;

    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // trigger open door only if in gameplay and player has key.
        if (gameManager.inGamePlay && collision.gameObject.CompareTag("Player") && gameManager.player.HasKey())
        {
            OpenDoor();
        }
    }

    /// <summary>
    /// Open door animation and logic.
    /// </summary>
    private void OpenDoor()
    {
        headerSprite.sprite = headerOpenedSprite;
        bodySprite.sprite = bodyOpenedSprite;

        _audio.Play();

        gameManager.EndLevel();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioSource>();
    }
}
