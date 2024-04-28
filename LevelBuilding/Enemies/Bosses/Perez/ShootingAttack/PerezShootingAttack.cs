using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerezShootingAttack : MonoBehaviour
{
    private Animator _anim;
    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Show boss throught one of the tunnels.
    /// </summary>
    public void Show()
    {
        _anim.SetBool("Show", true);
    }

    /// <summary>
    /// Hide boss throught one of the tunnels.
    /// </summary>
    public void Hide()
    {
        _anim.SetBool("Show", false);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioComponent>();
    }
}
