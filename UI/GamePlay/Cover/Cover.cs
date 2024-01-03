using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public bool displayed;
    private Animator _anim;

    /// <summary>
    /// Display cover.
    /// </summary>
    public void Display()
    {
        _anim.SetBool("Display", true);
        displayed = true;
    }

    /// <summary>
    /// Hide cover.
    /// </summary>
    public void Hide()
    {
        _anim.SetBool("Display", false);
        displayed = false;
    }

    /// <summary>
    /// Init method.
    /// </summary>
    public void Init()
    {
        _anim = GetComponent<Animator>();
    }
}
