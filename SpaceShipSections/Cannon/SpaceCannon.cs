using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCannon : MonoBehaviour
{
    [Header("Settings")]
    public string direction;

    [Header("Components")]
    public ObjectPool objectPool;

    private Coroutine shooting;
    private float cadence;

    /// <summary>
    /// Shoot proyectile from this cannnon.
    /// </summary>
    public void Shoot()
    {
        if (shooting == null)
        {
            shooting = StartCoroutine(ShootRoutine());
        }
    }

    /// <summary>
    /// Shoot proyectile from this cannon coroutine.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator ShootRoutine()
    {
        GameObject spawn = objectPool.SpawnPrefab();

        if (spawn)
        {
            spawn.transform.position = transform.position;
            spawn.transform.parent = null;

            CannonProyectile proyectile = spawn.GetComponent<CannonProyectile>();
            proyectile.SetDirection(direction);
        }

        yield return new WaitForSeconds(cadence);

        shooting = null;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="cadence"></param>
    public void Init(float cadence)
    {
        this.cadence = cadence;
    }

    /// <summary>
    /// Init class method.
    /// </summary>
    /// <param name="cadence">float</param>
    /// <param name="objectPool">ObjectPool</param>
    public void Init(float cadence, ObjectPool objectPool)
    {
        this.cadence = cadence;
        this.objectPool = objectPool;
    }
}
