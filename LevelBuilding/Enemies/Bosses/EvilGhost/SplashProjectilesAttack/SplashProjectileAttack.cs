using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashProjectileAttack : MonoBehaviour
{
    [Header("Components")]
    public ObjectPool pool;
    public Transform center;
    public Transform[] movingPoints;

    [HideInInspector]
    public bool inAttack;

    private Coroutine _splashAttack;
    private AudioComponent _audio;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Trigger splash attack.
    /// </summary>
    public void TriggerSplashAttack()
    {
        if (_splashAttack == null)
        {
            _splashAttack = StartCoroutine(TriggerSplashAttackCoroutine());
        }
    }

    /// <summary>
    /// Trigger splash attack coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator TriggerSplashAttackCoroutine()
    {
        inAttack = true;

        _audio.PlaySound(0);
        EvilGhostProjectile[] projectiles = new EvilGhostProjectile[6];
        string direction = "left";

        for (int i = 0; i < projectiles.Length; i++)
        {
            GameObject projectileGameObject = pool.SpawnPrefab();

            if (projectileGameObject)
            {
                projectiles[i] = projectileGameObject.GetComponent<EvilGhostProjectile>();
                projectiles[i].gameObject.transform.position = movingPoints[i].transform.position;
                projectiles[i].Spawn();
            }
        }

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < projectiles.Length; i++)
        {
            if (projectiles[i] != null) { 
                projectiles[i].MoveToTarget(center);
            }
        }

        yield return new WaitForSeconds(.5f);

        _audio.PlaySound(1);
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (i >= 3)
            {
                direction = "right";
            }

            Vector2 randomForce = GetRandomForce(direction);
            projectiles[i].ApplyForce(randomForce);
        }

        yield return new WaitForSeconds(1f);

        inAttack = false;
        _splashAttack = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Vector2 GetRandomForce(string direction)
    {
        float forceSide = Random.Range(1f, 4f);
        float forceUp = Random.Range(1f, 6f);

        Vector2 force = Vector2.up * forceUp;

        if (direction == "left")
        {
            force += Vector2.left * forceSide;
        }
        else
        {
            force += Vector2.right * forceSide;
        }

        return force;
    }

    /// <summary>
    /// Stop attack.
    /// </summary>
    public void StopAttack()
    {
        StopCoroutine(_splashAttack);
        _splashAttack = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    private void Init()
    {
        _audio = GetComponent<AudioComponent>();
    }
}
