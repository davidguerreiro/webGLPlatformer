using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicSounds : MonoBehaviour
{
    private AudioComponent _audioComponent;

    /// <summary>
    /// Play cinematic sound.
    /// </summary>
    /// <param name="sound"></param>
    /// <param name="loop"></param>
    public void PlayCinematicSound(int sound = 0, bool loop = false)
    {
        _audioComponent.SetLoop(loop);
        _audioComponent.PlaySound(sound);
    }

    /// <summary>
    /// Stops cinematic sound or music.
    /// </summary>
    public void StopLevelMusic()
    {
        _audioComponent.SetLoop(false);
        _audioComponent.StopAudio();
    }

    /// <summary>
    /// Expose audio component for further interaction
    /// in cinematics.
    /// </summary>
    /// <returns>AudioComponent</returns>
    public AudioComponent GetAudioComponent()
    {
        return _audioComponent;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _audioComponent = GetComponent<AudioComponent>();
    }
}
