using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    [Header("Settings")]
    public float secondsBeforeBreak;
    public float secondsBeforeReappear;

    private Animator _anim;
    private AudioComponent _audioComponent;
    private BoxCollider2D _boxCollider;
    private Coroutine _destroyAndReapearRoutine;

    // Start is called before the first frame update
    void Start()
    {
        Init();   
    }

    /// <summary>
    /// Check when a collider enters the collision.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckIfPlayerIsAbovePlatform(collision.gameObject);
        }
    }

    /// <summary>
    /// Checks when a collider stays in collision.
    /// </summary>
    /// <param name="collision">Coillision2D</param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckIfPlayerIsAbovePlatform(collision.gameObject);
        }
    }

    /// <summary>
    /// Check if player is above the platform
    /// and the platform is not in the process of
    /// breaking and reappearing.
    /// </summary>
    /// <param name="player">GameObject</param>
    private void CheckIfPlayerIsAbovePlatform(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController.isGrounded && _destroyAndReapearRoutine == null)
        {
            _destroyAndReapearRoutine = StartCoroutine(DestroyAndReappear());
        }
    }

    /// <summary>
    /// Destroy and reapear platform logic.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator DestroyAndReappear()
    {
        _audioComponent.PlaySound(0);
        _anim.SetBool("Trembling", true);
        yield return new WaitForSeconds(secondsBeforeBreak);

        _boxCollider.enabled = false;
        _audioComponent.PlaySound(1);
        _anim.SetBool("Trembling", false);
        yield return new WaitForSeconds(secondsBeforeReappear);

        _audioComponent.PlaySound(2);
        _anim.SetTrigger("Appear");
        _boxCollider.enabled = true;

        _destroyAndReapearRoutine = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _anim = GetComponent<Animator>();
        _audioComponent = GetComponent<AudioComponent>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }
}
