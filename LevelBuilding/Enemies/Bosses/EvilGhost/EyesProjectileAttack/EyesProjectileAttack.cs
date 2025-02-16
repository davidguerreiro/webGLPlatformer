using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesProjectileAttack : MonoBehaviour
{
    [Header("Config")]
    public float toWaitBetweenShots;

    [Header("Components")]
    public ObjectPool pool;
    public Transform leftEye;
    public Transform rightEye;
    public Transform cristal;               // Only to be used by Scar boss.
    public Transform[] leftPoints;
    public Transform[] rightPoints;
    public Transform[] bottomPoints;        // Only to be used by Scar boss.
    public Transform leftHorizontal;
    public Transform rightHorizontal;

    [HideInInspector]
    public bool inAttack;

    private Coroutine _eyesProjectilesAttack;
    private AudioComponent _audio;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    
    /// <summary>
    /// Trigger eyes projectile attack.
    /// </summary>
    /// <param name="direction">string</param>
    public void TriggerEyesProjectileAttack(string direction)
    {
        if (_eyesProjectilesAttack == null)
        {
            _eyesProjectilesAttack = StartCoroutine(TriggerEyesProjectileAttackCoroutine(direction));
        }
    }
       
    /// <summary>
    /// Trigger eyes projectile horizontal attack.
    /// </summary>
    /// <param name="direction"></param>
    public void TriggerEyesProjectileHorizontalAttack(string direction)
    {
        if (_eyesProjectilesAttack == null)
        {
            _eyesProjectilesAttack = StartCoroutine(TriggerEyeProyectilesHorizontalAttackCoroutine(direction));
        }
    }

    /// <summary>
    /// Trigger eyes projectile attack coroutine.
    /// </summary>
    /// <param name="direction">string</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator TriggerEyesProjectileAttackCoroutine(string direction)
    {
        inAttack = true;
        int j = 0;
        int targetLeft = 0;
        int targetRight = 1;

        Transform[] targets = (direction == "left") ? leftPoints : rightPoints;
        EvilGhostProjectile[] projectiles = new EvilGhostProjectile[8];

        _audio.PlaySound(0);
        for (int i = 0; i < 4; i++)
        {
            GameObject projectileGameObjectLeft = pool.SpawnPrefab();
            GameObject projectileGameObjectRight = pool.SpawnPrefab();

            if (i > 0)
            {
                targetLeft = GetRandomTarget(targets.Length);
                targetRight = GetRandomTarget(targets.Length);
            }

            if (i == 3)
            {
                targetLeft = 0;
                targetRight = 1;
            }

            if (projectileGameObjectLeft && projectileGameObjectRight)
            {
                projectiles[j] = projectileGameObjectLeft.GetComponent<EvilGhostProjectile>();
                projectiles[j].gameObject.transform.position = leftEye.position;
                projectiles[j].Spawn();

                j++;

                projectiles[j] = projectileGameObjectRight.GetComponent<EvilGhostProjectile>();
                projectiles[j].gameObject.transform.position = rightEye.position;
                projectiles[j].Spawn();

                yield return new WaitForSeconds(1f);

                _audio.PlaySound(1);

                projectiles[j - 1].MoveToTarget(targets[targetLeft], true);
                projectiles[j].MoveToTarget(targets[targetRight], true);

                yield return new WaitForSeconds(toWaitBetweenShots);
            }
        }

        inAttack = false;
        pool.DisableAll();
        _eyesProjectilesAttack = null;
    }

    /// <summary>
    /// Get random target.
    /// </summary>
    /// <param name="max">int</param>
    /// <returns>int</returns>
    private int GetRandomTarget(int max)
    {
        return Random.Range(0, max);
    }

    /// <summary>
    /// Trigger eyes projectiles horizontally.
    /// </summary>
    /// <param name="direction">string</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator TriggerEyeProyectilesHorizontalAttackCoroutine(string direction)
    {
        inAttack = true;
        int j = 0;

        Transform target = (direction == "left") ? leftHorizontal : rightHorizontal;
        EvilGhostProjectile[] projectiles = new EvilGhostProjectile[10];

        _audio.PlaySound(0);
        for (int i = 0; i < 5; i++)
        {
            GameObject projectileGameObjectLeft = pool.SpawnPrefab();
            GameObject projectileGameObjectRight = pool.SpawnPrefab();
            
            if (projectileGameObjectLeft && projectileGameObjectRight)
            {
                projectiles[j] = projectileGameObjectLeft.GetComponent<EvilGhostProjectile>();
                projectiles[j].gameObject.transform.position = leftEye.position;
                projectiles[j].Spawn();

                j++;

                projectiles[j] = projectileGameObjectRight.GetComponent<EvilGhostProjectile>();
                projectiles[j].gameObject.transform.position = rightEye.position;
                projectiles[j].Spawn();

                yield return new WaitForSeconds(1f);

                _audio.PlaySound(1);

                projectiles[j - 1].MoveToTarget(target, true);
                projectiles[j].MoveToTarget(target, true);

                yield return new WaitForSeconds(toWaitBetweenShots);
            }
        }

        inAttack = false;
        pool.DisableAll();
        _eyesProjectilesAttack = null;
    }

    /// <summary>
    /// Eyes proyectile attack used only by Scar boss.
    /// </summary>
    /// <param name="direction">string</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator TriggerCristalProyectilesAttackCoroutine(string direction)
    {
        inAttack = true;

        if (direction == "right")
        {
            System.Array.Reverse(bottomPoints);
        }
        
        foreach (Transform target in bottomPoints)
        {
            GameObject projectileGameObject = pool.SpawnPrefab();

            if (projectileGameObject)
            {
                EvilGhostProjectile evilProyectile = projectileGameObject.GetComponent<EvilGhostProjectile>();
                evilProyectile.gameObject.transform.position = cristal.position;
                evilProyectile.Spawn();

                yield return new WaitForSeconds(0.5f);

                _audio.PlaySound(1);

                evilProyectile.MoveToTarget(target, true);

                yield return new WaitForSeconds(toWaitBetweenShots);
            }
        }

        yield return new WaitForSeconds(1f);

        pool.DisableAll();

        if (direction == "right")
        {
            System.Array.Reverse(bottomPoints);
        }

        inAttack = false;
        _eyesProjectilesAttack = null;
    }

    /// <summary>
    /// Stop attack.
    /// </summary>
    public void StopAttack()
    {
        pool.DisableAll();

        if (_eyesProjectilesAttack != null)
        {
            StopCoroutine(_eyesProjectilesAttack);
            _eyesProjectilesAttack = null;
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
