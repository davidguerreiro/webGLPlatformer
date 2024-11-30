using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyBlocky : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public float waitBetweenMovements;
    public bool playSound;

    [Header("Components")]
    public GameManager gameManager;
    public Transform[] movingPoints;

    private AudioComponent _audio;
    private Coroutine _movingRoutine;

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
            if (_movingRoutine == null)
            {
                _movingRoutine = StartCoroutine(MovePointToPoint());
            }
        }
    }
    
    /// <summary>
    /// Move hazard to each point in endless loop.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator MovePointToPoint()
    {
        for (int i = 0; i < movingPoints.Length; i++)
        {
            int nextIndex = i + 1;
            Transform target = (nextIndex == movingPoints.Length) ? movingPoints[0] : movingPoints[nextIndex];

            while (Vector2.Distance(transform.position, target.position) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }

            if (playSound)
            {
                _audio.PlaySound();
            }

            transform.position = target.position;
            yield return new WaitForSeconds(waitBetweenMovements);
        }

        _movingRoutine = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
