using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsGameplayUI : MonoBehaviour
{
    public Text textComponent;
    private GameManager _gameManager;

    // Update is called once per frame
    void Update()
    {
        if (_gameManager != null)
        {
            UpdateCoinsValue();
        }
    }

    /// <summary>
    /// Update coins value.
    /// </summary>
    private void UpdateCoinsValue()
    {
        textComponent.text = _gameManager.GetCoinsLeftInLevel().ToString();
    }

    /// <summary>
    /// Init coins UI.
    /// </summary>
    /// <param name="gameManager"></param>
    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
}
