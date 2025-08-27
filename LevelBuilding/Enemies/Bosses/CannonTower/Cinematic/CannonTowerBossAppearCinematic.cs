using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTowerBossAppearCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public CannonTower bossLeft;
    public CannonTower bossRight;

    [Header("Dialogue Data EN")]
    public DialogueData cannonTower1EN;
    public DialogueData cannonTower2EN;
    public DialogueData ramiroEN;

    [Header("Dialogue Data ES")]
    public DialogueData cannonTower1ES;
    public DialogueData cannonTower2ES;
    public DialogueData ramiroES;


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
    /// Play cinematic routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        string lang = PlayerPrefs.GetString("language", "english");

        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1f);

        // appear boss.
        cinematicManager.sounds.PlayCinematicSound(0);
        StartCoroutine(bossLeft.ShowsUp());
        StartCoroutine(bossRight.ShowsUp());

        yield return new WaitForSeconds(2.5f);

        // play dialogue.
        DialogueData cannonTower1 = (lang == "english") ? cannonTower1EN : cannonTower1ES;
        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(cannonTower1);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DialogueData ramiro = (lang == "english") ? ramiroEN : ramiroES;
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiro);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        DialogueData cannonTower2 = (lang == "english") ? cannonTower2EN : cannonTower2ES;
        cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.PlayFullDialogue(cannonTower2);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBoxSecondary.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        // start boss battle.
        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        bossLeft.StartBattle();
        bossRight.StartBattle();
    }
}
