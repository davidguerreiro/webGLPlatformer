using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotativePlatform : MonoBehaviour
{
    [Header("Config")]
    public float toWaitBeforeRotation;
    public float rotateDuration;

    [Header("Components")]
    public GameManager gameManager;
    public BoxCollider2D boxCollider;
    public RotateItself rotateItself;

    private AudioComponent _audio;
    private Coroutine _rotatePlatform;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.inGamePlay) { 
            if (_rotatePlatform == null)
            {
                _rotatePlatform = StartCoroutine(RotatePlatform());
            }
        }
    }

    /// <summary>
    /// Rotate platform coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator RotatePlatform()
    {
        yield return new WaitForSeconds(toWaitBeforeRotation);

        rotateItself.StartRotation();
        _audio.PlaySound(0);
        boxCollider.enabled = false;
        yield return new WaitForSeconds(rotateDuration);

        rotateItself.StopRotation(true);
        boxCollider.enabled = true;

        _rotatePlatform = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
