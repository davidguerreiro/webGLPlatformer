using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWorldItem : MonoBehaviour
{
    [Header("LockedItems")]
    public GameObject lockedItem;
    public GameObject unLockedItem;

    [Header("Settings")]
    public string sceneToLoad;
    public bool unlocked;
    public string gameVarToUnlock;

    private AudioComponent _audio;
    private SelectWorld selectWorld;

    /// <summary>
    /// Unlock item if has been unlocked.
    /// </summary>
    public void UnLockItem()
    {
        lockedItem.SetActive(false);
        unLockedItem.SetActive(true);
        unlocked = true;
    }

    /// <summary>
    /// Trigger selected item. Usually called from
    /// menu item event.
    /// </summary>
    public void Selected()
    {
        if (unlocked)
        {
            _audio.PlaySound(0);
            selectWorld.LoadWorld(sceneToLoad);
        } else
        {
            _audio.PlaySound(1);
        }
    }


    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init(SelectWorld selectWorld)
    {
        _audio = GetComponent<AudioComponent>();
        this.selectWorld = selectWorld;

        unlocked = false;
    }

}
