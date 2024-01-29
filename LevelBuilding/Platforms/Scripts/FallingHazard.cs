using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    public float gravityScale;

    private Rigidbody2D _rigi;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Stop gravity when this hazard is disabled.
    /// </summary>
    private void OnDisable()
    {
        StopDropping();
    }

    /// <summary>
    /// Drop hazard.
    /// </summary>
    public void Drop()
    {
        _rigi.bodyType = RigidbodyType2D.Dynamic;
        _rigi.gravityScale = gravityScale;
    }

    /// <summary>
    /// Stop falling.
    /// </summary>
    public void StopDropping()
    {
        _rigi.bodyType = RigidbodyType2D.Kinematic;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init()
    {
        _rigi = GetComponent<Rigidbody2D>();
    }
}
