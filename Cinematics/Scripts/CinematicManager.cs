using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    [Header("Components")]
    public MainCamera mainCamera;
    public CinematicSounds sounds;
    public CinematicObjects objects;

    [HideInInspector]
    public GameManager gameManager;

    /// <summary>
    /// Init all cinematic dependencies.
    /// </summary>
    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;

        // init main camera.
        mainCamera.Init();

        // init cinematic sounds.
        sounds.Init();
    }
}
