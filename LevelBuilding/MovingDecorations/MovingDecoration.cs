using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDecoration : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public bool movingLeft;
    public float delay;
    public float completedDelay;
    public bool playSoundAtInitMovement;
    public bool stopAtTheEnd;

    [Header("Components")]
    public Transform leftTarget;
    public Transform rightTarget;

    private Coroutine _isMoving;
    private AudioComponent _audio;
    private GameManager _gameManager;

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (playSoundAtInitMovement)
        {

            if (_gameManager != null)
            {
                if (_gameManager.inGamePlay && _isMoving == null && !_gameManager.inGameOver)
                {
                    _isMoving = StartCoroutine(Moving());
                }


                if (_gameManager.inGameOver && _isMoving != null)
                {
                    StopCoroutine(_isMoving);
                }
            }
        } else
        {
            if (_isMoving == null)
            {
                _isMoving = StartCoroutine(Moving());
            }
        }
    }

    /// <summary>
    /// Moving decoration.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator Moving()
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        Transform target = (movingLeft) ? leftTarget : rightTarget;

        if (playSoundAtInitMovement)
        {
            _audio.PlaySound(0);
        }

        while (Vector2.Distance(transform.localPosition, target.localPosition) > 0.01f)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, target.localPosition, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.localPosition = target.localPosition;

        if (completedDelay > 0f)
        {
            yield return new WaitForSeconds(completedDelay);
        }

        if (movingLeft)
        {
            transform.localPosition = rightTarget.localPosition;
        } else
        {
            transform.localPosition = leftTarget.localPosition;
        }

        if (stopAtTheEnd)
        {
            speed = 0;
        }

        _isMoving = null;
    }

    /// <summary>
    /// Get game manager in current scene.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator GetGameManager()
    {
        yield return new WaitForSeconds(.1f);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        if (playSoundAtInitMovement)
        {
            _audio = GetComponent<AudioComponent>();
            StartCoroutine(GetGameManager());
        }

    }

}
