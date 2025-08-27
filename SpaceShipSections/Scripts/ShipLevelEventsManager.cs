using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipLevelEventsManager : MonoBehaviour
{
    [Header("Components")]
    public ShipGameManager gameManager;
    public GameObject enemies;

    [Header("Spawners")]
    public SpaceEnemySpawner topSpawner;
    public SpaceEnemySpawner middleTopSpawner;
    public SpaceEnemySpawner middleSpawner;
    public SpaceEnemySpawner middleBottomSpawner;
    public SpaceEnemySpawner bottomSpawner;

    [Header("Settings")]
    public float toWaitBeforeSpawning;
    public float toWaitBetweenSpawning;

    [Header("Envents")]
    public UnityEvent afterSpawnAllSwarms;

    /// <summary>
    /// Type of enemies available to spawn.
    /// </summary>
    public enum Enemies
    {
        Asteroid,
        BlueShip,
        KamikazeShip,
        OrangeShip,
    };

    /// <summary>
    /// Spawner used to spawn enemy in the scene.
    /// </summary>
    public enum Spawners
    {
        Top,
        MiddleTop,
        Middle,
        MiddleBottom,
        Bottom,
    };

    /// <summary>
    /// Spawn rows available in game map.
    /// </summary>
    public enum SpawnRows
    {
        Left,
        Middle,
        Right,
    }
   

    [System.Serializable]
    public struct Swarm
    {
        public Enemies enemy;
        public Spawners spawner;
        public SpawnRows row;

        [Range(0, 4)]
        public int position;        // Position in the enemy positions matrix
    }
    [System.Serializable]
    public class ListWrapper
    {
        public List<Swarm> items = new List<Swarm>();
    }

    [Header("Swarms")]
    public List<ListWrapper> swarms;

    private Coroutine spawnLoopCoroutine;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.inGamePlay && spawnLoopCoroutine == null)
        {
            spawnLoopCoroutine = StartCoroutine(SpawnSwarms());
        }
    }

    /// <summary>
    /// Spawns swarms of enemies during level gameplay.
    /// </summary>
    /// <returns>IEnumerator</returns>
    public IEnumerator SpawnSwarms()
    {
        yield return new WaitForSeconds(toWaitBeforeSpawning);
        
        foreach (ListWrapper swarmList in swarms)
        {
            SpawnSwarmEnemies(swarmList.items);

            yield return StartCoroutine(CheckForSwarmAlive());
            yield return new WaitForSeconds(toWaitBetweenSpawning);
        }

        afterSpawnAllSwarms?.Invoke();
    }

    /// <summary>
    /// Spawn all the enemies from a swarm in the game level scene.
    /// </summary>
    /// <param name="enemies">List<Swarm></param>
    public void SpawnSwarmEnemies(List<Swarm> enemies)
    {
        foreach (Swarm swarm in enemies)
        {
            SpaceEnemySpawner spawner = GetSpawner(swarm.spawner);
            string row = GetRow(swarm.row);

            // spawn asteroid.
            if (swarm.enemy == Enemies.Asteroid)
            {
                spawner.SpawnAsteroidSlow();
            }

            // spawn basic blue ship enemy.
            if (swarm.enemy == Enemies.BlueShip)
            {
                spawner.SpawnSpaceShooterBasic(row, swarm.position);
            }

            // spawn kamikaze enemy.
            if (swarm.enemy == Enemies.KamikazeShip)
            {
                spawner.SpawnKamikazeEnemy(row, swarm.position);
            }

            // spawn orange moving shooter.
            if (swarm.enemy == Enemies.OrangeShip)
            {
                spawner.SpawnMovingShooterEnemy();
            }
        }
    }

    /// <summary>
    /// Resolve to which row the enemy is spawned in the map.
    /// </summary>
    /// <param name="row">SpawRows</param>
    /// <returns>string</returns>
    private string GetRow(SpawnRows row)
    {
        string strRow = "";

        switch (row)
        {
            case SpawnRows.Right:
                strRow = "right";
                break;
            case SpawnRows.Middle:
                strRow = "middle";
                break;
            case SpawnRows.Left:
                strRow = "left";
                break;
            default:
                strRow = "right";
                break;
        }

        return strRow;
    }

    /// <summary>
    /// Resolve to which spawner the enemy is spawned.
    /// </summary>
    /// <param name="spawner">Spawners</param>
    /// <returns>SpaceEnemySpawner</returns>
    private SpaceEnemySpawner GetSpawner(Spawners spawner)
    {
        SpaceEnemySpawner resolved;

        switch (spawner)
        {
            case Spawners.Top:
                resolved = topSpawner;
                break;
            case Spawners.MiddleTop:
                resolved = middleTopSpawner;
                break;
            case Spawners.Middle:
                resolved = middleSpawner;
                break;
            case Spawners.MiddleBottom:
                resolved = middleBottomSpawner;
                break;
            case Spawners.Bottom:
                resolved = bottomSpawner;
                break;
            default:
                resolved = middleSpawner;
                break;
        }

        return resolved;
    }

    /// <summary>
    /// Checks if current swarm enemies are still alive.
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator CheckForSwarmAlive()
    {
        while (enemies.transform.childCount > 0)
        {
            yield return new WaitForFixedUpdate();
        }
    }
}
