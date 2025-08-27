using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherAppearCinematic : MonoBehaviour
{
    [Header("Components")]
    public CinematicManager cinematicManager;
    public Crusher boss;
    public Transform bossMovingPoint;
    public GameObject explosion;

    [Header("Dialogues")]
    public DialogueData crusherAppearEN;
    public DialogueData crusherAppearES;
    public DialogueData ramiroAnswerEN;
    public DialogueData ramiroAnswerES;
    public DialogueData crusherEndEN;
    public DialogueData crusherEndES;

    [Header("Settins")]
    public float bossAscendSpeed;

    private Coroutine _playingCinematic;

    /// <summary>
    /// Play Crusher enter scene cinematic.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playingCinematic == null)
        {
            _playingCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }

    /// <summary>
    /// Crusher enter scene cinematic.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        string lang = PlayerPrefs.GetString("language", "english");

        boss.RemoveColliders();
        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1.5f);

        // boss appear.
        cinematicManager.mainCamera.ShackeCamera(2.5f, true);
        yield return new WaitForSeconds(2.8f);

        explosion.SetActive(true);

        Transform bossPosition = boss.gameObject.transform;

        while (Vector2.Distance(bossPosition.position, bossMovingPoint.position) > 0.01f)
        {
            bossPosition.position = Vector2.MoveTowards(bossPosition.position, bossMovingPoint.position, bossAscendSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        bossPosition.position = bossMovingPoint.position;
        yield return new WaitForSeconds(1.5f);
        explosion.SetActive(false);

        // play dialogue.
        DialogueData crusherEnter = (lang == "english") ? crusherAppearEN : crusherAppearES;
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(crusherEnter);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData ramiroAnswers = (lang == "english") ? ramiroAnswerEN : ramiroAnswerES;
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(ramiroAnswers);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        DialogueData crusherEnd = (lang == "english") ? crusherEndEN: crusherEndES;
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(crusherEnd);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        // start boss battle.
        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
