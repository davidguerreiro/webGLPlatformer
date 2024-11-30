using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;

public class Navegable : MonoBehaviour
{
    public NavegableItem[] items;

    [Header("Settings")]
    public bool enableAtStart;
    public bool cancellable;
    public float waitBetweenNewSelected;
    public bool unsetNavegableOnSelection;

    [Header("Events")]
    public UnityEvent onCancel;
    public UnityEvent onNavigatingAbove;
    public UnityEvent onNavigatingBelow;

    [HideInInspector]
    public bool isNavegable;

    [HideInInspector]
    public Rewired.Player rewiredPlayer;

    private AudioComponent _audio;
    private int _index;
    private Coroutine _movingCursor;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isNavegable)
        {
            ListenForUserInput();
        }
    }

    /// <summary>
    /// Listen for using intpu while
    /// navegable.
    /// </summary>
    private void ListenForUserInput()
    {
        if (rewiredPlayer.GetButtonDown("Jump"))
        {
            SelectOption();
        }

        if (cancellable && rewiredPlayer.GetButtonDown("Cancel"))
        {
            Cancel();
        }


        if (_movingCursor == null)
        {
            float direction = rewiredPlayer.GetAxis("MoveVertical");

            if (direction > 0.2f)
            {
                _movingCursor = StartCoroutine(MoveCursor("up"));
            }

            if (direction < -0.2f)
            {
                _movingCursor = StartCoroutine(MoveCursor("down"));
            }
        }
    }

    /// <summary>
    /// Select option in current navegable.
    /// </summary>
    private void SelectOption()
    {
        _audio.PlaySound(1);

        items[_index].Select();

        if (unsetNavegableOnSelection)
        {
            UnSetNavegable();
        }

    }

    /// <summary>
    /// Move cursor coroutine.
    /// </summary>
    /// <param name="direction">string</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveCursor(string direction)
    {
        int newIndex = (direction == "up") ? -1 : 1;

        UnSelectAllItems();
        UpdateIndex(newIndex);
        _audio.PlaySound(0);

        items[_index].SetSelected(true);

        yield return new WaitForSeconds(waitBetweenNewSelected);

        _movingCursor = null;
    }

    /// <summary>
    /// Update index value.
    /// </summary>
    /// <param name="value">int</param>
    private void UpdateIndex(int value)
    {
        _index += value;

        if (_index < 0)
        {
            _index = items.Length - 1;
            onNavigatingAbove?.Invoke();
        }

        if (_index >= items.Length)
        {
            _index = 0;
            onNavigatingBelow?.Invoke();
        }
    }

    /// <summary>
    /// Set this menu navegable for user.
    /// </summary>
    public void SetNavegable()
    {
        isNavegable = true;
        ShowSelectedCursor();
    }

    /// <summary>
    /// Unset this menu navegable for user.
    /// </summary>
    public void UnSetNavegable()
    {
        HideSelectedCursor();
        isNavegable = false;
    }

    /// <summary>
    /// Get selected item label.
    /// </summary>
    /// <returns>string</returns>
    public string GetSelectedLabel()
    {
        foreach (NavegableItem item in items)
        {
            if (item.selected)
            {
                return item.label;
            }
        }

        return "";
    }

    /// <summary>
    /// Get settings selected item.
    /// Use this method when the menu has settings
    /// items associated.
    /// </summary>
    /// <returns>string</returns>
    public string GetSettingsItemSelectedLabel()
    {
        foreach (NavegableItem item in items)
        {
            SettingsItem settingsItem = item.GetComponent<SettingsItem>();

            if (settingsItem)
            {
                if (settingsItem.selected)
                {
                    return item.label;
                }
            }
        }

        return "";
    }

    /// <summary>
    /// Cancel menu interaction.
    /// </summary>
    public void Cancel()
    {
        UnSetNavegable();
        onCancel?.Invoke();
    }

    /// <summary>
    /// Unselect all items in menu.
    /// </summary>
    public void UnSelectAllItems()
    {
        foreach (NavegableItem item in items)
        {
            item.SetSelected(false);
        }
    }

    /// <summary>
    /// Hide selected cursor.
    /// </summary>
    public void HideSelectedCursor()
    {
        foreach (NavegableItem item in items)
        {
            item.HideCursor();
        } 
    }

    /// <summary>
    /// Set selectable. Usually called from
    /// external scripts or events.
    /// </summary>
    /// <param name="selectedLabel">string</param>
    public void SetSelectable(string selectedLabel)
    {
        UnSelectAllItems();

        foreach (NavegableItem item in items)
        {
            if (item.label == selectedLabel)
            {
                item.SetSelected(true);
                break;
            }
        }
    }

    /// <summary>
    /// Show selected cursor.
    /// </summary>
    public void ShowSelectedCursor()
    {
        foreach (NavegableItem item in items)
        {
            if (item.selected)
            {
                item.ShowCursor();
                break;
            }
        }
    }

    /// <summary>
    /// Init navegable items.
    /// </summary>
    public void InitItems()
    {
        foreach (NavegableItem item in items)
        {
            item.SetParentNavegable(this);
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
        rewiredPlayer = ReInput.players.GetPlayer(0);
        _index = 0;

        InitItems();

        if (enableAtStart)
        {
            SetNavegable();
        }
    }
}
