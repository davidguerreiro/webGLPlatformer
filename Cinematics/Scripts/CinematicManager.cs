using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CinematicManager : MonoBehaviour
{
    [Header("Components")]
    public GamePlayUI gamePlayUI;
    public ShipGamePlayUI shipGamePlayUI;
    public GameObject[] layouts;
    public MainCamera mainCamera;
    public CinematicSounds sounds;
    public CinematicObjects objects;
    public AudioComponent cinematicOST;

    [Header("Settings")]
    public bool autoInit;

    [Header("Events")]
    public UnityEvent afterInitEvent;

    [HideInInspector]
    public GameManager gameManager;

    [HideInInspector]
    public ShipGameManager shipGameManager;

    [HideInInspector]
    public bool changingLayouts;

    private void Start()
    {
        if (autoInit)
        {
            Init();
        }   
    }


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

    /// <summary>
    /// Init all cinematic dependencies for ship levels
    /// cinematics.
    /// </summary>
    /// <param name="shipGameManager"></param>
    public void Init(ShipGameManager shipGameManager)
    {
        this.shipGameManager = shipGameManager;

        // init main camera.
        mainCamera.Init();

        // init cinematic sounds.
        sounds.Init();
    }

    /// <summary>
    /// Init all dependencies without cinematic manager.
    /// </summary>
    public void Init()
    {
        // init main camera.
        mainCamera.Init();

        // init cinematic sounds.
        sounds.Init();

        // init gameplay UI.
        gamePlayUI.Init();

        // trigger scene cinematic.
        afterInitEvent?.Invoke();
    }
       
    /// <summary>
    /// Change cinematic layout.
    /// </summary>
    /// <param name="currentLayout">int</param>
    /// <param name="layoutToDisplay">int</param>
    /// <param name="toWaitFadeIn">int</param>
    /// <param name="toWaitFadeOut">int</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator ChangeLayout(int currentLayout, int layoutToDisplay, float toWaitFadeIn = 1f, float toWaitFadeOut = 1f)
    {
        changingLayouts = true;

        gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(toWaitFadeIn);

        layouts[currentLayout].SetActive(false);
        layouts[layoutToDisplay].SetActive(true);

        gamePlayUI.cover.FadeOut();
        yield return new WaitForSeconds(toWaitFadeOut);

        changingLayouts = false;
    }

    /// <summary>
    /// Change layouts without cover fade in/out.
    /// </summary>
    /// <param name="currentLayout">int</param>
    /// <param name="layoutToDisplay">int</param>
    public void ChangeLayoutStraigh(int currentLayout, int layoutToDisplay)
    {
        layouts[currentLayout].SetActive(false);
        layouts[layoutToDisplay].SetActive(true);
    }
}
