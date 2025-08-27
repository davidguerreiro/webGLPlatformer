using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
    [Header("EnergyBars")]
    public EnergyBar[] energyBars;

    private PlayerShip player;

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            UpdateHealth();
        }
    }

    /// <summary>
    /// Update player's energy UI.
    /// </summary>
    private void UpdateHealth()
    {
        for (int i = energyBars.Length - 1; i >= 0; i--)
        {
            if (player.health > i)
            {
                energyBars[i].EnableBar();
            } else
            {
                energyBars[i].DisableBar();
            }
        }
    }

    /// <summary>
    /// Init energy bars script.
    /// </summary>
    private void InitEnergyBars()
    {
        foreach (EnergyBar energyBar in energyBars)
        {
            energyBar.Init();
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="player"></param>
    public void Init(PlayerShip player)
    {
        this.player = player;

        InitEnergyBars();
    }
}
