using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonSwitch : MonoBehaviour
{
    public Sprite activatedSprite;
    public UnityEvent activated;

    private SpriteRenderer _spriteRenderer;
    private AudioComponent _audioComponent;
    private bool isActivated;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Check if player enters collision.
    /// </summary>
    /// <param name="collision">Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (! isActivated && collision.CompareTag("Player"))
        {
            ActivateSwitch();
        }
    }

    /// <summary>
    /// Activate switch.
    /// </summary>
    private void ActivateSwitch()
    {
        isActivated = true;

        _spriteRenderer.sprite = activatedSprite;
        _audioComponent.PlaySound();

        activated?.Invoke();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioComponent = GetComponent<AudioComponent>();
    }
}
