using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyPlatform : MonoBehaviour
{
    /// <summary>
    /// Enter collision controller.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetPlayerIcy(collision.gameObject);
        }
    }

    /// <summary>
    /// Exit collision controller.
    /// </summary>
    /// <param name="collision">Collision2D</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnSetPlayerIcy(collision.gameObject);
        }
    }

    /// <summary>
    /// Set player moving state to moving over
    /// icy floor.
    /// </summary>
    /// <param name="playerCollided">GameObject</param>
    private void SetPlayerIcy(GameObject playerCollided)
    {
        PlayerController playerController = playerCollided.gameObject.GetComponent<PlayerController>();

        if (playerController.isGrounded)
        {
            playerController.SetInIcyFloor(true);
        }
    }

    /// <summary>
    /// Remove moving over icy floor state from player
    /// controller.
    /// </summary>
    /// <param name="playerCollided">GameObject</param>
    private void UnSetPlayerIcy(GameObject playerCollided)
    {
        PlayerController playerController = playerCollided.gameObject.GetComponent<PlayerController>();
        playerController.SetInIcyFloor(false);
    }
}
