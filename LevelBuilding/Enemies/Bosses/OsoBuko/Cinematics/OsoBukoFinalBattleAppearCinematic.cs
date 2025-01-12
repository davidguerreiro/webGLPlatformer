using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsoBukoFinalBattleAppearCinematic : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public OsoBuko boss;
    public DialogueData dialogueData;

    [Header("Settings")]
    public float moveSpeed;

    private Coroutine _playCinematic;

    /// <summary>
    /// Play boss cinematic at the start of the
    /// boss battle level.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playCinematic == null)
        {
            _playCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }

    private IEnumerator PlayCinematicRoutine()
    {
        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(2.5f);

        Transform middlePoint = cinematicManager.objects.GetObject(0).transform;

        cinematicManager.gameManager.PlayLevelMainTheme();

        // boss going down.
        while (Vector2.Distance(boss.transform.position, middlePoint.position) > 0.01f)
        {
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, middlePoint.position, moveSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        boss.transform.position = middlePoint.transform.position;

        yield return new WaitForSeconds(1.5f);

        // play dialogue.
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(dialogueData);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        // start boss battle.
        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
