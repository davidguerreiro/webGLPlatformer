using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGamePlayUI : MonoBehaviour
{
    [Header("Components")]
    public FadeElement cover;
    public EnergyUI energyUI;
    public LifesUI lifesUI;
    public GameOverUI gameOver;
    public DialogueBox dialogueBox;
    public DialogueBox dialogueBoxSecondary;
    public HelpBox helpbox;
    public PauseUI pauseUI;

    private ShipGameManager gameManager;

    /// <summary>
    /// Init ship gameplay UI method.
    /// </summary>
    /// <param name="shipGameManager">ShipGameManager</param>
    public void Init(ShipGameManager shipGameManager)
    {
        gameManager = shipGameManager;

        // init energy UI.
        energyUI.Init(gameManager.player);

        // init lifes UI.
        lifesUI.Init(gameManager.player);

        // init dialogue box.
        if (shipGameManager.hasCinematic)
        {
            dialogueBox.Init();
            dialogueBoxSecondary.Init();
        }
    }

    /// <summary>
    /// Display game over screen.
    /// </summary>
    public void DisplayGameOver()
    {
        gameManager.PlayGameOverMusic();
        gameOver.gameObject.SetActive(true);
        gameOver.EnableNavegable(gameManager);
    }
}
