using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : MovingSideToSideEnemy
{
    [Header("Explode Enemy")]
    public Sprite explodeSprite;
    public GameObject explosion;
    public int growingTimes;
    public float secondsBeforeGlowing;
    public float secondsBeforeExplosion;
    public float secondsExplosionDamageable;

    private Coroutine _explosionRoutine;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Defeated enemy.
    /// </summary
    public override void Defeated()
    {
        gameObject.tag = "Untagged";
        isAlive = false;

        _anim.SetBool("IsMoving", false);
        _anim.enabled = false;
        _canMove = false;

        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        RemoveHazardPoints();
        RemoveColliders();

        _audio.PlaySound(0);

        gameManager.player.playerController.EnemyDefeatedRecoil();

        // explode enemy.
        _explosionRoutine = StartCoroutine(ExplosionCoroutine());
    }
    
    /// <summary>
    /// Explosion after enemy is defeated.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ExplosionCoroutine()
    {
        spriteRenderer.sprite = explodeSprite;
        
        for (int i = 0; i < growingTimes; i++)
        {
            yield return new WaitForSeconds(secondsBeforeGlowing);

            _audio.PlaySound(1);
            _anim.enabled = true;
            _anim.SetTrigger("Growing");
        }

        yield return new WaitForSeconds(secondsBeforeExplosion);

        _anim.enabled = false;
        spriteRenderer.sprite = null;
        _audio.PlaySound(2);
        explosion.SetActive(true);
        yield return new WaitForSeconds(secondsExplosionDamageable);

        explosion.SetActive(false);

        Destroy(this.gameObject, timeToDestroy);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private new void Init()
    {
        base.Init();

        if (_anim == null)
        {
            _anim = GetComponent<Animator>();
            _anim.SetBool("IsMoving", true);
        }

        _canMove = true;
    }
}
