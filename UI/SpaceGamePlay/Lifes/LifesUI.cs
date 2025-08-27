using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifesUI : MonoBehaviour
{
    public TextComponent lifesNumberText;

    private PlayerShip player;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            UpdatePlayerLifes();
        }   
    }

    /// <summary>
    /// Update player lifes on screen.
    /// </summary>
    private void UpdatePlayerLifes()
    {
        if (lifesNumberText != null)
        {
            lifesNumberText.UpdateContent(player.lifes.ToString());
        } 
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="player">PlayerShip</param>
    public void Init(PlayerShip player)
    {
        this.player = player;
    }
}
