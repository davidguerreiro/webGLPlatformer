using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Components")]
    public GameManager gameManager;
    public ShipGameManager shipGameManager;
    public bool useShipManager;
    public string[] textToDisplayEn;
    public string[] textToDisplayEs;

    [Header("Settings")]
    public float timeBeforeDisplaying;
    public float timeBetweenTexts;

    private bool displayed;
    

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null)
        {
            if (!displayed)
            {
                if (gameManager.inGamePlay && gameManager.player.playerController.canMove)
                {
                    StartCoroutine(TriggerTutorialHelpBoxes());
                }
            }
        }

        if (shipGameManager != null && useShipManager)
        {
            if (!displayed)
            {
                if (shipGameManager.inGamePlay && shipGameManager.player.playerController.canMove)
                {
                    StartCoroutine(TriggerTutorialHelpBoxes());
                }
            }
        }
    }

    /// <summary>
    /// Trigger tutorial help boxes.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator TriggerTutorialHelpBoxes()
    {
        displayed = true;

        HelpBox helpBox = GetHelpBox();

        string lang = PlayerPrefs.GetString("language", "english");
        string[] textToDisplay = (lang == "english") ? textToDisplayEn : textToDisplayEs;

        yield return new WaitForSeconds(timeBeforeDisplaying);

        foreach (string text in textToDisplay)
        {
            helpBox.Display(text);

            while (helpBox.displayed)
            {
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(timeBetweenTexts);
        }
    }

    /// <summary>
    /// Get helpbox from gameplay UI.
    /// </summary>
    /// <returns>HelpBox</returns>
    private HelpBox GetHelpBox()
    {
        if (useShipManager)
        {
            return shipGameManager.gamePlayUI.helpbox;
        }

        return gameManager.gamePlayUI.helpbox;
    }
}
