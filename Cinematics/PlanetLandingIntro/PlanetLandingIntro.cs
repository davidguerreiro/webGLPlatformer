using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetLandingIntro : MonoBehaviour
{
    [Header("Components")]
    public CinematicManager cinematicManager;
    public GameObject brokenShip;
    public GameObject ramiro;
    public GameObject shipExplosion;

    [Header("Settings")]
    public string sceneToLoadAfter;

    [Header("Dialogue Data EN")]
    public DialogueData jojonete1EN;
    public DialogueData jojonete2EN;
    public DialogueData ramiro1EN;
    public DialogueData ramiro2EN;
    public DialogueData ramiro3EN;
    public DialogueData ramiro4EN;
    public DialogueData ramiro5EN;

    [Header("Dialogue Data ES")]
    public DialogueData jojonete1ES;
    public DialogueData jojonete2ES;
    public DialogueData ramiro1ES;
    public DialogueData ramiro2ES;
    public DialogueData ramiro3ES;
    public DialogueData ramiro4ES;
    public DialogueData ramiro5ES;

    private Coroutine _playingCinematic;

    /// <summary>
    /// Play game cinematic.
    /// </summary>
    public void PlayCinematic()
    {
        if (_playingCinematic == null)
        {
            _playingCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }

    /// <summary>
    /// Play game cinematic routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        string lang = PlayerPrefs.GetString("language", "english");

        SpriteRenderer ramiroSprite = ramiro.GetComponent<SpriteRenderer>();
        Animator ramiroAnim = ramiro.GetComponent<Animator>();

        yield return new WaitForSeconds(1f);

        cinematicManager.sounds.PlayCinematicSound(3);
        yield return new WaitForSeconds(3.5f);

        cinematicManager.gamePlayUI.cover.FadeOut();
        cinematicManager.cinematicOST.PlaySound(2);
        yield return new WaitForSeconds(2.5f);

        ramiroAnim.SetTrigger("StandUp");
        cinematicManager.sounds.PlayCinematicSound(4);
        yield return new WaitForSeconds(1.5f);

        ramiroSprite.flipX = false;
        yield return new WaitForSeconds(1.5f);

        DialogueData ramiro1 = (lang == "english") ? ramiro1EN: ramiro1ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiro1);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.5f);

        ramiroSprite.flipX = true;
        yield return new WaitForSeconds(1f);

        DialogueData ramiro2 = (lang == "english") ? ramiro2EN : ramiro2ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiro2);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData jojonete1 = (lang == "english") ? jojonete1EN : jojonete1ES;

        cinematicManager.gamePlayUI.dialogueBox.PlayFullDialogue(jojonete1);

        while (cinematicManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.cinematicOST.PlaySound(3);
        yield return new WaitForSeconds(1f);

        DialogueData ramiro3 = (lang == "english") ? ramiro3EN : ramiro3ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiro3);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData jojonete2 = (lang == "english") ? jojonete2EN : jojonete2ES;

        cinematicManager.gamePlayUI.dialogueBox.PlayFullDialogue(jojonete2);

        while (cinematicManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        brokenShip.SetActive(false);
        shipExplosion.SetActive(true);
        yield return new WaitForSeconds(.9f);
        shipExplosion.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        DialogueData ramiro4 = (lang == "english") ? ramiro4EN : ramiro4ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiro4);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.5f);

        ramiroSprite.flipX = false;
        yield return new WaitForSeconds(1f);

        DialogueData ramiro5 = (lang == "english") ? ramiro5EN : ramiro5ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiro5);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(sceneToLoadAfter);
    }
}
