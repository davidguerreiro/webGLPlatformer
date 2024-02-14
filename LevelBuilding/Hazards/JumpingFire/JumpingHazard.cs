using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingHazard : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Jumping Settings")]
    public float secondsBeforeJumping;
    public float secondsAfterJumping;
    public float jumpForce;

    private Rigidbody2D _rigi;
    private Coroutine _jumpRoutine;
    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.inGamePlay && _jumpRoutine == null)
        {
            _jumpRoutine = StartCoroutine(Jump());
        }
    }

    /// <summary>
    /// Hazarz jump.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator Jump()
    {
        yield return new WaitForSeconds(secondsBeforeJumping);

        _audio.PlaySound(0);
        _rigi.velocity = new Vector2(0f, jumpForce);
        yield return new WaitForSeconds(secondsAfterJumping);

        _jumpRoutine = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _rigi = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioComponent>();
    }
}
