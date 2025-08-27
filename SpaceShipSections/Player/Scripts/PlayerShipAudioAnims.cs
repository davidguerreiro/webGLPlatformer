using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipAudioAnims : MonoBehaviour
{
    [Header("Components")]
    public GameObject cabin;
    public GameObject explosion;
    public GameObject shipBackFire;

    private AudioComponent audio;
    private Animator anim;
    private SpriteRenderer renderer;
    private Sprite shipSprite;
    private PlayerShip player;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIFInvencible();
    }

    /// <summary>
    /// Shoot sound.
    /// </summary>
    public void ShootSound()
    {
        audio.PlaySound(0);
    }

    /// <summary>
    /// Check if player is invencible.
    /// </summary>
    public void CheckIFInvencible()
    {
        if (anim != null && anim.enabled)
        {
            anim.SetBool("Invencible", player.invincible);
        }
    }

    /// <summary>
    /// Player gets damage anim logic.
    /// </summary>
    public void DamageAnim()
    {
        audio.PlaySound(1);
        anim.SetTrigger("Damage");
    }

    /// <summary>
    /// Player destroyed.
    /// </summary>
    public void Destroyed()
    {
        StartCoroutine(DestroyedCoroutine());
    }

    /// <summary>
    /// Player destroyed coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator DestroyedCoroutine()
    {
        anim.enabled = false;
        renderer.sprite = null;
        shipBackFire.SetActive(false);
        cabin.SetActive(false);

        explosion.SetActive(true);
        audio.PlaySound(2);
        yield return new WaitForSeconds(.7f);
        explosion.SetActive(false);
    }

    /// <summary>
    /// Restore player on respawn.
    /// </summary>
    public void RestorePlayer()
    {
        anim.enabled = true;
        renderer.sprite = shipSprite;

        cabin.SetActive(true);
        shipBackFire.SetActive(true);
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    public void Init(PlayerShip player)
    {
        audio = GetComponent<AudioComponent>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        this.player = player;

        shipSprite = renderer.sprite;
    }
}
