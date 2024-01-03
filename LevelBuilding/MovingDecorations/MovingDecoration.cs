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

    [Header("Components")]
    public Transform leftTarget;
    public Transform rightTarget;

    private Coroutine _isMoving;

    // Update is called once per frame
    void Update()
    {
        if (_isMoving == null)
        {
            _isMoving = StartCoroutine(Moving());
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

        _isMoving = null;
    }

}
