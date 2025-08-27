using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameIntroCinematic : MonoBehaviour
{
    public string sceneToLoadAfterCinematic;

    [Header("Components")]
    public CinematicManager cinematicManager;
    public Animator playerShip;
    public AudioComponent playerShipAudio;
    public Animator playerAnim;
    public GameObject alert;
    public FadeElement introText;
    public GameObject spacePirates;

    [Header("Dialogue Data EN")]
    public DialogueData jojonete1EN;
    public DialogueData jojonete2EN;
    public DialogueData jojonete3EN;
    public DialogueData ramiro1EN;
    public DialogueData ramiro2EN;

    [Header("Dialogue Data ES")]
    public DialogueData jojonete1ES;
    public DialogueData jojonete2ES;
    public DialogueData jojonete3ES;
    public DialogueData ramiro1ES;
    public DialogueData ramiro2ES;

    private Coroutine _playingCinematic;
    private SpriteRenderer playerSprite;

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

        playerSprite = playerAnim.gameObject.GetComponent<SpriteRenderer>();

        cinematicManager.cinematicOST.PlaySound(0);

        yield return new WaitForSeconds(1.5f);

        introText.FadeIn();
        yield return new WaitForSeconds(12f);
        introText.FadeOut();
        yield return new WaitForSeconds(2f);
            
        cinematicManager.gamePlayUI.cover.FadeOut();
        yield return new WaitForSeconds(4f);

        DialogueData jojoneteDialogue = (lang == "english") ? jojonete1EN : jojonete1ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(jojoneteDialogue);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData ramiroDialogue = (lang == "english") ? ramiro1EN : ramiro1ES;

        cinematicManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue);

        while (cinematicManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.cinematicOST.PauseAudio();
        yield return new WaitForSeconds(.1f);

        alert.SetActive(true);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(.5f);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(1f);

        jojoneteDialogue = (lang == "english") ? jojonete2EN : jojonete2ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(jojoneteDialogue);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.cinematicOST.PlaySound(1);
        spacePirates.SetActive(true);

        yield return new WaitForSeconds(3f);

        ramiroDialogue = (lang == "english") ? ramiro2EN : ramiro2ES;

        cinematicManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroDialogue);

        while (cinematicManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        jojoneteDialogue = (lang == "english") ? jojonete3EN : jojonete3ES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(jojoneteDialogue);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        playerShip.SetBool("MoveOut", true);
        playerShipAudio.PlaySound(0);

        yield return new WaitForSeconds(2f);

        /*
        DialogueData ramiroDialogue = (lang == "english") ? firstRamiroDialogueEN : firstRamiroDialogueES;
        DialogueData alertDialogue = (lang == "english") ? alertDialogueEN : alertDialogueES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiroDialogue);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        // stop background ost and display alert.
        cinematicManager.cinematicOST.PauseAudio();
        yield return new WaitForSeconds(.1f);

        alert.SetActive(true);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(.5f);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(1f);

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(alertDialogue);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);
        cinematicManager.cinematicOST.PlaySound(1);

        DialogueData evadeDialogue = (lang == "english") ? evadeEN : evadeES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(evadeDialogue);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        meteors.SetActive(true);
        playerShip.SetTrigger("Evade");

        cinematicManager.sounds.PlayCinematicSound(1);
        yield return new WaitForSeconds(2f);

        playerShip.SetTrigger("Evade");
        cinematicManager.sounds.PlayCinematicSound(1);
        yield return new WaitForSeconds(2f);

        foreach (MovingDecoration movingMeteor in movingMeteors)
        {
            movingMeteor.stopAtTheEnd = true;
        }

        playerShip.SetBool("Damaged", true);
        explosions.SetActive(true);
        cinematicManager.sounds.PlayCinematicSound(2);

        yield return new WaitForSeconds(1.5f);

        DialogueData shipLanding = (lang == "english") ? shipLandingEN : shipLandingES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(shipLanding);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData ramiroWorried = (lang == "english") ? ramiroWorriedEN : ramiroWorriedES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiroWorried);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.5f);

        planet.SetActive(true);
        yield return new WaitForSeconds(5f);

        DialogueData ramiroPlanet = (lang == "english") ? ramiroPlanetEN : ramiroPlanetES;

        cinematicManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroPlanet);

        while (cinematicManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        playerShip.SetTrigger("IntoPlanet");
        cinematicManager.sounds.PlayCinematicSound(1);
        yield return new WaitForSeconds(1f);

        playerShip.gameObject.SetActive(false);
        star.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        cinematicManager.gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(2.5f);

        cinematicManager.cinematicOST.audio.Stop();
        yield return new WaitForSeconds(.5f);

        cinematicManager.sounds.PlayCinematicSound(1);
        yield return new WaitForSeconds(1f);

        cinematicManager.sounds.PlayCinematicSound(3);
        yield return new WaitForSeconds(3.5f);

        cinematicManager.cinematicOST.PlaySound(2);
        yield return new WaitForSeconds(.5f);

        cinematicManager.ChangeLayoutStraigh(0, 1);
        yield return new WaitForSeconds(.1f);

        cinematicManager.gamePlayUI.cover.FadeOut();
        yield return new WaitForSeconds(2f);


        playerAnim.SetTrigger("StandUp");
        cinematicManager.sounds.PlayCinematicSound(4);
        yield return new WaitForSeconds(1.5f);

        playerSprite.flipX = false;
        yield return new WaitForSeconds(1f);

        DialogueData ramiroWhereIAm = (lang == "english") ? ramiroWhereIAmEN: ramiroWhereIAmES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiroWhereIAm);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        playerSprite.flipX = true;
        yield return new WaitForSeconds(1f);


        DialogueData ramiroAssesShip = (lang == "english") ? ramiroAssesShipEN: ramiroAssesShipES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiroAssesShip);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.cinematicOST.StopAudio();

        scarDarkPortal.Appear();
        yield return new WaitForSeconds(1f);
        scar.SetActive(true);
        yield return new WaitForSeconds(1f);
        scarDarkPortal.Dissapear();

        yield return new WaitForSeconds(1f);

        cinematicManager.cinematicOST.PlaySound(1);

        DialogueData scarAppear = (lang == "english") ? scarAppearEN : scarAppearES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarAppear);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData ramiroSeesScar = (lang == "english") ? ramiroSeesScarEN : ramiroSeesScarES;

        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiroSeesScar);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData scarSurprised = (lang == "english") ? scarSurprisedEN : scarSurprisedES;
        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarSurprised);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        shipDarkPortal.Appear();
        yield return new WaitForSeconds(1f);
        brokenShip.SetActive(false);
        shipDarkPortal.Dissapear();
        yield return new WaitForSeconds(1.5f);

        DialogueData ramiroLostShip = (lang == "english") ? ramiroLostShipEN : ramiroLostShipES;
        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiroLostShip);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData scarLeaves = (lang == "english") ? scarLeavesEN : scarLeavesES;
        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(scarLeaves);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        scarDarkPortal.Appear();
        yield return new WaitForSeconds(1f);
        scar.SetActive(false);
        scarDarkPortal.Dissapear();
        yield return new WaitForSeconds(1.5f);

        playerSprite.flipX = false;

        yield return new WaitForSeconds(1f);

        DialogueData ramiroStartsJourney = (lang == "english") ? ramiroStartsJourneyEN : ramiroStartsJourneyES;
        cinematicManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(ramiroStartsJourney);

        while (cinematicManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        */

        cinematicManager.gamePlayUI.cover.FadeIn();
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(sceneToLoadAfterCinematic);
    }
}
