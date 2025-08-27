using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderShipAppearCinematic : MonoBehaviour
{
    [Header("Components")]
    public CinematicManager cinematicManager;
    public SpaceEnemySpawner centralSpawner;
    public GameObject alert;

    [Header("Dialogue data EN")]
    public DialogueData jojoneteEN;
    public DialogueData ramiroEN;
    public DialogueData commandShipEN;

    [Header("Dialogue data ES")]
    public DialogueData jojoneteES;
    public DialogueData ramiroES;
    public DialogueData commandShipES;

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

        yield return new WaitForSeconds(5.5f);

        cinematicManager.shipGameManager.inGamePlay = false;
        cinematicManager.shipGameManager.player.playerController.RestrictPlayerControl();

        cinematicManager.shipGameManager.StopLevelMusic();

        alert.SetActive(true);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(.5f);

        cinematicManager.sounds.PlayCinematicSound(0);
        yield return new WaitForSeconds(1f);

        DialogueData jojonete = (lang == "english") ? jojoneteEN : jojoneteES;

        cinematicManager.shipGamePlayUI.dialogueBox.PlayFullDialogue(jojonete);

        while (cinematicManager.shipGamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        CommanderSpaceShip boss = centralSpawner.SpawnCommanderSpaceShipAndReturn();
        yield return new WaitForSeconds(3f);

        DialogueData ramiro = (lang == "english") ? ramiroEN : ramiroES;

        cinematicManager.shipGamePlayUI.dialogueBox.PlayFullDialogue(ramiro);

        while (cinematicManager.shipGamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData commanderShip = (lang == "english") ? commandShipEN : commandShipES;

        cinematicManager.shipGamePlayUI.dialogueBoxSecondary.PlayFullDialogue(commanderShip);

        while (cinematicManager.shipGamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        cinematicManager.shipGameManager.PlayBossBattleMusic();
        cinematicManager.shipGameManager.inGamePlay = true;
        cinematicManager.shipGameManager.player.playerController.AllowControl();

        boss.StartBattle();
    }
}
