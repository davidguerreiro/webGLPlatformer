using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    [Header("Components")]
    public MainCamera mainCamera;
    public CinematicSounds sounds;
    public CinematicObjects objects;

    /// <summary>
    /// Init all cinematic dependencies.
    /// </summary>
    public void Init()
    {
        // init main camera.
        mainCamera.Init();

        // init cinematic sounds.
        sounds.Init();
    }
}
