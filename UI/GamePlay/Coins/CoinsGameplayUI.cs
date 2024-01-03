using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsGameplayUI : MonoBehaviour
{
    public Text textComponent;
    private Player _player;

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            UpdateCoinsValue();
        }
    }

    /// <summary>
    /// Update coins value.
    /// </summary>
    private void UpdateCoinsValue()
    {
        textComponent.text = _player.GetCoins().ToString();
    }

    /// <summary>
    /// Init coins UI.
    /// </summary>
    /// <param name="player"></param>
    public void Init(Player player)
    {
        _player = player;
    }
}
