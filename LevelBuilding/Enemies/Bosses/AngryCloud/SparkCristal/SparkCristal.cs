using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkCristal : MonoBehaviour
{
    [Header("Components")]
    public GameObject particles;

    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        Init();   
    }

    /// <summary>
    /// Enable lighting.
    /// </summary>
    public void ShowLighting()
    {
        particles.SetActive(true);
        _anim.SetBool("Lighting", true);
    }

    /// <summary>
    /// Disable lighting.
    /// </summary>
    public void HideLighting()
    {
        particles.SetActive(false);
        _anim.SetBool("Lighting", false);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _anim = GetComponent<Animator>();
    }
}
