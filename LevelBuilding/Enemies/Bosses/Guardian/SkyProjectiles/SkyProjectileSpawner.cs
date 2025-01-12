using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyProjectileSpawner : MonoBehaviour
{
    public ObjectPool pool;

    [Header("Settings")]
    public float forceDown;
    public bool spawnByTouch;
    public string tagToCheckForEnable;

    private AudioComponent _audio;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Spawn sky projectile into boss
    /// game scene.
    /// </summary>
    public void SpawnSkyProjectile()
    {
        GameObject projectile = pool.SpawnPrefab();

        if (projectile)
        {
            projectile.transform.position = gameObject.transform.position;

            Rigidbody2D projectileRigi = projectile.GetComponent<Rigidbody2D>();

            projectileRigi.velocity = Vector2.zero;
            projectileRigi.AddForce(Vector2.down * forceDown, ForceMode2D.Impulse);

            _audio.PlaySound();
        }
    }

    /// <summary>
    /// Trigger enter collision detect to spawn
    /// proyectile.
    /// </summary>
    /// <param name="collision">Collider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spawnByTouch)
        {
            if (collision.gameObject.CompareTag(tagToCheckForEnable))
            {
                SpawnSkyProjectile();
            }
        }
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }

}
