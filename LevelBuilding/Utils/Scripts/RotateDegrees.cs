using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDegrees : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public float degrees;
    public float toWaitBetweenRotations;
    public bool playSoundAtRotation;

    [Header("Components")]
    public GameManager gameManager;

    private Coroutine _rotateObjectRoutine;
    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.inGamePlay)
        {
            if (_rotateObjectRoutine == null)
            {
                _rotateObjectRoutine = StartCoroutine(RotateObject());
            }
        }
    }

    /// <summary>
    /// Rotate object at given degrees in Z axis at given speed.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator RotateObject()
    {
        yield return new WaitForSeconds(toWaitBetweenRotations);

        if (playSoundAtRotation)
        {
            _audio.PlaySound();
        }

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + degrees);

        while (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.rotation = targetRotation;

        _rotateObjectRoutine = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        if (playSoundAtRotation)
        {
            _audio = GetComponent<AudioComponent>();
        }
    }
}
