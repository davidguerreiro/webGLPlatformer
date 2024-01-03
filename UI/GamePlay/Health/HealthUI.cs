using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] containers = new Image[3];

    [Header("Sprites")]
    public Sprite filledHearthSprite;
    public Sprite emptyHearthSprite;

    private Player _player;

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            UpdateContainers();
        }
    }

    /// <summary>
    /// Update hearth containers in the UI.
    /// </summary>
    private void UpdateContainers()
    {
        foreach (Image container in containers)
        {
            container.sprite = emptyHearthSprite;
        }

        for (int j = 0; j < _player.GetHealth(); j++)
        {
            containers[j].sprite = filledHearthSprite;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="player"></param>
    public void Init(Player player)
    {
        _player = player;
    }
}
