using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingForwardSpaceEnemy : SpaceEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Move();
        }    
    }

    /// <summary>
    /// Move straight left.
    /// </summary>
    public void Move()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
