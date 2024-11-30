using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnContact : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;

    [Header("Events")]
    public UnityEvent onContact;

    private bool _triggered;

    /// <summary>
    /// Check if player enters contact zone to 
    /// trigger event.
    /// </summary>
    /// <param name="collision">Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (! _triggered)
        {
            if (gameManager.inGamePlay && collision.CompareTag("Player"))
            {
                onContact?.Invoke();
                _triggered = true;
            }
        }
    }

    /// <summary>
    /// Enable event if triggered.
    /// </summary>
    public void EnableEvent()
    {
        _triggered = false;
    }
}
