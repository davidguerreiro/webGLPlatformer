using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerHazardEvent : MonoBehaviour
{    
    public UnityEvent onContact;

    private bool _triggered;

    /// <summary>
    /// Check player collision to trigger hazard.
    /// </summary>
    /// <param name="collision">Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && ! _triggered)
        {
            onContact?.Invoke();
        }
    }

    /// <summary>
    /// Restore this event trigger.
    /// </summary>
    public void RestoreEvent()
    {
        _triggered = false;
    }
}
