using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBlock : MonoBehaviour
{
    public GameObject explosion;
    
    private SpriteRenderer _spriteRenderer;
    private AudioComponent _audioComponent;
    private BoxCollider2D _boxCollider2D;
    private bool exploded;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Explode block.
    /// </summary>
    public void Explode()
    {
        if (! exploded)
        {
            exploded = false;
            StartCoroutine(ExplodeRoutine());
        }
    }

    /// <summary>
    /// Explode block routine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ExplodeRoutine()
    {
        explosion.SetActive(true);
        _audioComponent.PlaySound();
        _boxCollider2D.enabled = false;

        yield return new WaitForSeconds(.5f);
        _spriteRenderer.sprite = null;

        yield return new WaitForSeconds(.15f);
        explosion.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        Destroy(this);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioComponent = GetComponent<AudioComponent>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }
}
