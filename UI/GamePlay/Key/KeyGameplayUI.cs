using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGameplayUI : MonoBehaviour
{
    [Header("Components")]
    public Image spriteRenderer;
    public Sprite hasKeySprite;

    private bool _listeningForKey;
    private Player _player;

    // Update is called once per frame
    void Update()
    {
        if (_listeningForKey)
        {
            CheckForKeyObtained();
        }
    }

    /// <summary>
    /// Check if player has key to display
    /// key obtained sprite.
    /// </summary>
    private void CheckForKeyObtained()
    {
        if (_player.HasKey())
        {
            spriteRenderer.sprite = hasKeySprite;
            _listeningForKey = false;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="player">Player</param>
    public void Init(Player player)
    {
        _player = player;
        _listeningForKey = true;
    }
}
