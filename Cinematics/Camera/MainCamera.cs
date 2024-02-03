using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [HideInInspector]
    public Coroutine shakeRoutine;
    private Animator _anim;
    private AudioComponent _audioComponent;

    /// <summary>
    /// Shake main camera.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="playSound"></param>
    public void ShackeCamera(float duration = 3f, bool playSound = true)
    {
        if (shakeRoutine == null)
        {
            shakeRoutine = StartCoroutine(ShakeCameraRoutine(duration, playSound));
        }
    }

    /// <summary>
    /// Shacke main camera.
    /// </summary>
    /// <param name="duration">float</param>
    /// <param name="playSound">bool</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator ShakeCameraRoutine(float duration = 3f, bool playSound = true)
    {
        _anim.SetBool("Shackle", true);

        if (playSound)
        {
            _audioComponent.PlaySound(0);
            _audioComponent.SetLoop(true);
        }

        yield return new WaitForSeconds(duration);

        _anim.SetBool("Shackle", false);
        _audioComponent.SetLoop(false);
        _audioComponent.StopAudio();

        shakeRoutine = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _anim = GetComponent<Animator>();
        _audioComponent = GetComponent<AudioComponent>();
    }
}
