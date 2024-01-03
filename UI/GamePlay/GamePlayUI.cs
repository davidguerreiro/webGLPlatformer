using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    [Header("Components")]
    public Cover cover;
    public CoinsGameplayUI coinsUI;
    public KeyGameplayUI keyUI;
    public HealthUI healthUI;

    /// <summary>
    /// Init gameplay UI method.
    /// </summary>
    public void Init(GameManager gameManager)
    {
        // init cover.
        cover.Init();

        // init coins UI.
        coinsUI.Init(gameManager.player);

        // init key UI.
        keyUI.Init(gameManager.player);

        // init health UI.
        healthUI.Init(gameManager.player);
    }
}
