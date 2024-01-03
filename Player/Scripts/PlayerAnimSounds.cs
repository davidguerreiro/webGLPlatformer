using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimSounds : MonoBehaviour
{
    [Header("Components")]
    public PlayerController playerController;

    [Header("Sprites")]
    public Sprite idle;
    public Sprite jumping;

    [Header("Audio")]
    public AudioClip jumpSoundClip;

    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (playerController != null)
        {
            if (playerController.canMove)
            {
                MovingAnim();
            }

            EnterDoorAnim();
        }
    }

    /// <summary>
    /// Moving animation.
    /// </summary>
    public void MovingAnim()
    {
        if (playerController.xMove > 0f)
        {
            _spriteRenderer.flipX = false;
        }

        if (playerController.xMove < 0f)
        {
            _spriteRenderer.flipX = true;
        }

        if (playerController.xMove != 0f && playerController.isGrounded)
        {
            _anim.SetBool("IsMoving", true);
        } else
        {
            _anim.SetBool("IsMoving", false);
        }
    }

    /// <summary>
    /// Play jump sound.
    /// </summary>
    public void JumpSound()
    {
        if (_audio != null)
        {
            _audio.clip = jumpSoundClip;
            _audio.Play();
        }
    }

    /// <summary>
    ///  Trigger enter door animation.
    /// </summary>
    private void EnterDoorAnim()
    {
        if (playerController.enterDoor)
        {
            _anim.SetBool("EnterDoor", true);
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
