using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckGround : MonoBehaviour
{
    private RaycastHit2D hit;
    private bool _grounded;

    private void FixedUpdate()
    {
        CheckGround();
    }

    /// <summary>
    /// Check object below player.
    /// </summary>
    private void CheckGround()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, .5f);

        if (hit.collider == null)
        {
            _grounded = false;
        } else
        {
            if (hit.collider.CompareTag("IsGround"))
            {
                _grounded = true;
            } else
            {
                _grounded = false;
            }
        }
    }

    /// <summary>
    /// Get grounded value.
    /// </summary>
    /// <returns>bool</returns>
    public bool GetGrounded()
    {
        return _grounded;
    }
}
