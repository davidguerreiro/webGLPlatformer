using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class NavegableItem : MonoBehaviour
{
    [Header("Item settings")]
    public string label;
    public GameObject cursor;
    public bool selected;

    [Header("Events")]
    public UnityEvent onSelected;

    private Navegable _parentNavegable;

    private void Update()
    {
        if (_parentNavegable && _parentNavegable.isNavegable)
        {
            CheckSelected();
        }
    }

    /// <summary>
    /// Check if current item is selected.
    /// </summary>
    public void CheckSelected()
    {
        if (selected)
        {
            cursor.SetActive(true);
        } else
        {
            cursor.SetActive(false);
        }
    }

    /// <summary>
    /// Update this navegable item selected
    /// value.
    /// </summary>
    /// <param name="value">bool</param>
    public void SetSelected(bool value)
    {
        selected = value;
    }

    /// <summary>
    /// Trigger selected event when selected.
    /// </summary>
    public void Select()
    {
        onSelected?.Invoke();
    }

    /// <summary>
    /// Hide cursor visually, without affecting
    /// seleted status.
    /// </summary>
    public void HideCursor()
    {
        if (cursor.activeSelf) {
            cursor.SetActive(false);
        }
    }

    /// <summary>
    /// Show cursor visually, without affecting
    /// selected item.
    /// </summary>
    public void ShowCursor()
    {
        cursor.SetActive(true);
    }

    /// <summary>
    /// Set parent navegable.
    /// Usually called in parent navegable Init.
    /// </summary>
    /// <param name="navegable">Navegable</param>
    public void SetParentNavegable(Navegable navegable)
    {
        _parentNavegable = navegable;
    }
}
