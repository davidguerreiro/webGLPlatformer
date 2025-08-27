using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPortal : MonoBehaviour
{
    private AudioComponent _audio;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Appear dark portal anim.
    /// </summary>
    public void Appear()
    {
        _audio.PlaySound(0);
        _anim.SetBool("Appear", true);
    }

    /// <summary>
    /// Close dark portal anim.
    /// </summary>
    public void Dissapear()
    {
        _audio.PlaySound(1);
        _anim.SetBool("Appear", false);
    }

    /// <summary>
    /// Expand dark portal.
    /// </summary>
    public void Expand()
    {
        _audio.PlaySound(0);
        _anim.SetBool("Expand", true);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _audio = GetComponent<AudioComponent>();
        _anim = GetComponent<Animator>();
    }
}
