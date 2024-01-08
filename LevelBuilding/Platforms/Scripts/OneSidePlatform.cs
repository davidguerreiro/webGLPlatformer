using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSidePlatform : MonoBehaviour
{
    public Player player;

    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPosition();
    }

    /// <summary>
    /// Check if collider is enabled based on wheter the player
    /// is below the platform or not.
    /// </summary>
    public void CheckPlayerPosition()
    {
        if (player.gameObject.transform.position.y < this.gameObject.transform.position.y)
        {
            _collider.enabled = false;
        } else
        {
            _collider.enabled = true;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    
}
