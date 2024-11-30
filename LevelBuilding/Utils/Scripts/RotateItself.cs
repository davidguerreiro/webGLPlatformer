using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItself : MonoBehaviour {
    public bool autoStart;
    public float speed = 10f;                                   // Rotate speed.
    public bool rotateX = true;
    public bool rotateY = true;
    public bool rotateZ = true;

    private bool _canMove;                                      // Flag used to stop movement.
    private Quaternion _originalRotation;

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update() {
        if (_canMove)
        {
            RotateObject();
        }
    }

    /// <summary>
    /// Rotate object over itself.
    /// </summary>
    private void RotateObject() {

        float rotationValue = speed * Time.deltaTime;
        float rotateXValue = ( this.rotateX ) ? rotationValue : 0f;
        float rotateYValue = ( this.rotateY ) ? rotationValue : 0f;
        float rotateZvalue = ( this.rotateZ ) ? rotationValue : 0f;

        transform.Rotate( new Vector3( rotateXValue, rotateYValue, rotateZvalue ) );
    }

    /// <summary>
    /// Set rotation in motion.
    /// </summary>
    public void StartRotation()
    {
        _canMove = true;
    }

    /// <summary>
    /// Stop rotation.
    /// </summary>
    /// <param name="resetRotation">bool</param>
    public void StopRotation(bool resetRotation = false) {
        _canMove = false;

        if (resetRotation)
        {
            transform.rotation = _originalRotation;
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _originalRotation = transform.rotation;

        if (autoStart)
        {
            StartRotation();
        }
    }
}
