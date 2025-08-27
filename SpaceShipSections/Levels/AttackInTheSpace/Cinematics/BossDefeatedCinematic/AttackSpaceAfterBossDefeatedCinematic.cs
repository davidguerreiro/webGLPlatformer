using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackSpaceAfterBossDefeatedCinematic : MonoBehaviour
{
    [Header("Components")]
    public CinematicManager cinematicManager;
    public GameObject alert;
    public DarkPortal darkPortal;

    [Header("Settings")]
    public string sceneToLoad;

    [Header("Dialogue data EN")]
    public DialogueData jojonete1EN;
    public DialogueData jojonete2EN;
    public DialogueData ramiro1EN;
    public DialogueData ramiro2EN;
    public DialogueData ramiro3EN;

    [Header("Dialogue data ES")]
    public DialogueData jojonete1ES;
    public DialogueData jojonete2ES;
    public DialogueData ramiro1ES;
    public DialogueData ramiro2ES;
    public DialogueData ramiro3ES;

    private Coroutine _playCinematic;

    /// <summary>
    /// Play boss cinematic.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playCinematic == null)
        {
            _playCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }

    /// <summary>
    /// Play boss cinematic routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        string lang = PlayerPrefs.GetString("language", "english");

        yield return new WaitForSeconds(8f);

        cinematicManager.shipGameManager.inGamePlay = false;
        cinematicManager.shipGameManager.player.playerController.RestrictPlayerControl();

        cinematicManager.shipGameManager.StopLevelMusic();

        alert.SetActive(false);
        alert.SetActive(true);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(.5f);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(1f);

        DialogueData jojonete = (lang == "english") ? jojonete1EN : jojonete1ES;

        cinematicManager.shipGamePlayUI.dialogueBox.PlayFullDialogue(jojonete);

        while (cinematicManager.shipGamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData ramiro = (lang == "english") ? ramiro1EN : ramiro1ES;

        cinematicManager.shipGamePlayUI.dialogueBox.PlayFullDialogue(ramiro);

        while (cinematicManager.shipGamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.sounds.PlayCinematicSound(5, true);
        yield return new WaitForSeconds(.1f);

        darkPortal.Appear();
        yield return new WaitForSeconds(2f);

        DialogueData ramiro2 = (lang == "english") ? ramiro2EN : ramiro2ES;

        cinematicManager.shipGamePlayUI.dialogueBox.PlayFullDialogue(ramiro2);

        while (cinematicManager.shipGamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData jojonete2 = (lang == "english") ? jojonete2EN : jojonete2ES;

        cinematicManager.shipGamePlayUI.dialogueBox.PlayFullDialogue(jojonete2);

        while (cinematicManager.shipGamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData ramiro3 = (lang == "english") ? ramiro3EN : ramiro3ES;

        cinematicManager.shipGamePlayUI.dialogueBox.PlayFullDialogue(ramiro3);

        while (cinematicManager.shipGamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        darkPortal.Expand();
        yield return new WaitForSeconds(1f);

        cinematicManager.shipGameManager.gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(.5f);

        cinematicManager.sounds.GetAudioComponent().FadeOutSong();
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(sceneToLoad);
    }
}
