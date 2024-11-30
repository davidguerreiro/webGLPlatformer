using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternityFlame : MonoBehaviour
{
    private Animator _animator;
    private AudioComponent _audioComponent;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Display eternal flame.
    /// </summary>
    public void Appear()
    {
        _animator.SetBool("Appear", true);
        _audioComponent.PlaySound();
    }

    /// <summary>
    /// Hide eternal flame.
    /// </summary>
    public void Dissapear()
    {
        _animator.SetBool("Dissapear", true);
        _audioComponent.PlaySound();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _animator = GetComponent<Animator>();
        _audioComponent = GetComponent<AudioComponent>();
    }
}
