using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilGhostCinematicAppear : MonoBehaviour
{
    public CinematicManager cinematicManager;
    public EvilGhost boss;
    public DialogueData dialogueData;

    private Coroutine _playCinematic;

    /// <summary>
    /// Play boss cinematic at the start of the boss
    /// battle level.
    /// </summary>
    public void PlayBossAppearCinematic()
    {
        if (_playCinematic == null)
        {
            _playCinematic = StartCoroutine(PlayCinematicRoutine());
        }
    }
    
    /// <summary>
    /// Play boss cinematic at the start of the boss
    /// battle level coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator PlayCinematicRoutine()
    {
        DarkPortal villainDarkPortal = cinematicManager.objects.GetObject(0).GetComponent<DarkPortal>();
        DarkPortal bossDarkPortal = cinematicManager.objects.GetObject(1).GetComponent<DarkPortal>();
        GameObject villain = cinematicManager.objects.GetObject(2);

        cinematicManager.gameManager.inGamePlay = false;
        yield return new WaitForSeconds(1.5f);

        // play dialogue.
        cinematicManager.gameManager.gamePlayUI.dialogueBox.PlayFullDialogue(dialogueData);

        while (cinematicManager.gameManager.gamePlayUI.dialogueBox.playingFullDialogue != null)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(.5f);

        villainDarkPortal.Appear();
        yield return new WaitForSeconds(1f);
        villain.SetActive(true);
        villain.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        villainDarkPortal.Dissapear();
        yield return new WaitForSeconds(1f);

        bossDarkPortal.Appear();
        yield return new WaitForSeconds(1f);

        boss.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        bossDarkPortal.Dissapear();

        yield return new WaitForSeconds(1f);

        cinematicManager.gameManager.player.playerController.AllowControl();
        cinematicManager.gameManager.inGamePlay = true;
        cinematicManager.gameManager.PlayLevelMainTheme();

        cinematicManager.gameManager.isBossLevel = false;

        boss.StartBattle();
    }
}
