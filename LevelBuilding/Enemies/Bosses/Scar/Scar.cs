using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scar : Boss
{
    [Header("Battle Config")]
    public float movingSpeed;
    public float toWaitTeleporting;

    [Header("Boss Components")]
    public SpriteRenderer bossSprite;
    public DarkPortal darkPortal;
    public Transform cristal;

    [Header("Attacks")]
    public EyesProjectileAttack cristalProyectilesAttack;

    [Header("Moving points")]
    public Transform topMiddle;
    public Transform topLeft;
    public Transform topRight;
    public Transform middleLeft;
    public Transform middleRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    private Coroutine _teleportRoutine;
    private Coroutine _moveCoroutine;
    private Coroutine _patternAttack;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Teleport ghost to a moving point
    /// in battle scene.
    /// </summary>
    /// <param name="toMove">Transform</param>
    public void TeleportToPoint(Transform toMove)
    {
        if (_teleportRoutine == null)
        {
            _teleportRoutine = StartCoroutine(TeleportToPointCoroutine(toMove));
        }
    }

    /// <summary>
    /// Teleport boss to a moving point in
    /// battle scene.
    /// </summary>
    /// <param name="toMove">Transform</param>
    /// <param name="direction">string</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator TeleportToPointCoroutine(Transform toMove)
    {
        darkPortal.Appear();
        RemoveHazardPoints();
        RemoveWeakPoints();
        yield return new WaitForSeconds(.5f);

        bossSprite.gameObject.SetActive(false);
        yield return new WaitForSeconds(.1f);
        darkPortal.Dissapear();


        yield return new WaitForSeconds(toWaitTeleporting);

        gameObject.transform.position = toMove.position;

        darkPortal.Appear();
        yield return new WaitForSeconds(.5f);

        EnableHazardPoints();
        EnableWeakPoints();
        bossSprite.gameObject.SetActive(true);

        darkPortal.Dissapear();
        yield return new WaitForSeconds(.1f);

        _teleportRoutine = null;
    }

    /// <summary>
    /// Move to position.
    /// </summary>
    /// <param name="target">Transform</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator MoveToPoint(Transform target)
    {
        _audio.PlaySound(9);

        while (Vector2.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, movingSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        _moveCoroutine = null;
    }



}
