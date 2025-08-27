using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Settings")]
    public float speed;

    [Header("Components")]
    public Transform[] movingPoints;
    public Sprite[] sprites;

    [Header("Other components and gameobjects")]
    public GameObject[] otherComponents;
    
    [Header("State")]
    public bool isMoving;

    private SpriteRenderer renderer;
    private Animator anim;

    private Coroutine moveToPoint;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Flip actor sprite in X axis.
    /// </summary>
    /// <param name="value">bool</param>
    public void FlipXAxis(bool value)
    {
        renderer.flipX = value;
    }

    public void InitMovingAnim()
    {
        anim.SetBool("IsMoving", true);
    }

    public void StopMovingAnim()
    {
        anim.SetBool("IsMoving", false);
    }

    /// <summary>
    /// Change actor sprite.
    /// </summary>
    /// <param name="spriteIndex">int</param>
    public void ChangeSprite(int spriteIndex)
    {
        renderer.sprite = sprites[spriteIndex];
    }

    /// <summary>
    /// Move actor.
    /// </summary>
    /// <param name="targetIndex">int</param>
    /// <param name="moveAnim">bool</param>
    public void MoveActor(int targetIndex, bool moveAnim = true)
    {
        if (moveToPoint == null)
        {
            moveToPoint = StartCoroutine(MoveToPoint(targetIndex, moveAnim));
        }
    }
    
    /// <summary>
    /// Move actor to point.
    /// </summary>
    /// <param name="targetIndex">int</param>
    /// <param name="moveAnim">bool</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveToPoint(int targetIndex, bool moveAnim = true)
    {
        isMoving = true;

        if (moveAnim)
        {
            InitMovingAnim();
        }

        Transform target = movingPoints[targetIndex];

        while (Vector2.Distance(transform.position, target.position) > 0.01f) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }

        transform.position = target.position;

        if (moveAnim)
        {
            StopMovingAnim();
        }

        isMoving = false;
        moveToPoint = null;
    }

    /// <summary>
    /// Disable animator component.
    /// </summary>
    public void DisableAnimComponent()
    {
        anim.enabled = false;
    }

    /// <summary>
    /// Enable animator component.
    /// </summary>
    public void EnableAnimComponent()
    {
        anim.enabled = true;
    }

    /// <summary>
    /// Enable other components.
    /// </summary>
    /// <param name="index">int</param>
    public void EnableOtherComponents(int index)
    {
        otherComponents[index].SetActive(true);
    }

    /// <summary>
    /// Disable other components.
    /// </summary>
    /// <param name="index">int</param>
    public void DisableOtherComponents(int index)
    {
        otherComponents[index].SetActive(false);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
}
