using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    [Header("Settings")]
    public float gravityScale;
    public bool hasAnim;
    public bool hasAudio;
    public bool playAnimBeforeFalling;
    public float secondsBeforeFalling;

    private Rigidbody2D _rigi;
    private Animator _anim;
    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Stop gravity when this hazard is disabled.
    /// </summary>
    private void OnDisable()
    {
        StopDropping();
    }

    /// <summary>
    /// Drop hazard.
    /// </summary>
    public void Drop()
    {
        _rigi.bodyType = RigidbodyType2D.Dynamic;
        _rigi.gravityScale = gravityScale;
    }

    /// <summary>
    /// Stop falling.
    /// </summary>
    public void StopDropping()
    {
        if (_rigi == null)
        {
            _rigi = GetComponent<Rigidbody2D>();
        }

        if (hasAudio && _audio != null)
        {
            _audio.PlaySound();
        }

        _rigi.bodyType = RigidbodyType2D.Kinematic;
    }


    /// <summary>
    /// Drop with animation.
    /// </summary>
    public void DropWithAnim()
    {
        StartCoroutine(DropWithAnimRoutine());
    }

    /// <summary>
    /// Drop with animtion routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator DropWithAnimRoutine()
    {
        if (hasAnim && playAnimBeforeFalling)
        {
            _anim.SetBool("animBeforeFalling", true);
            yield return new WaitForSeconds(secondsBeforeFalling);
            _anim.SetBool("animBeforeFalling", false);
        }

        Drop();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _rigi = GetComponent<Rigidbody2D>();

        if (hasAnim)
        {
            _anim = GetComponent<Animator>();
        }

        if (hasAudio)
        {
            _audio = GetComponent<AudioComponent>();
        }
    }
}
