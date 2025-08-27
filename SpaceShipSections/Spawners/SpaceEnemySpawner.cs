using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEnemySpawner : MonoBehaviour
{
    [Header("MovingPoints")]
    public Transform[] rowLeft;
    public Transform[] rowMiddle;
    public Transform[] rowRight;

    [Header("Components")]
    public GameObject enemiesWrapper;

    // NOTE: Add other enemies when they are created.
    [Header("Enemies")]
    public GameObject asteriodSlow;
    public GameObject spaceShooterBasic;
    public GameObject kamikazeEnemy;
    public GameObject spaceMovingShooterBasic;
    public GameObject commanderSpaceShip;

    // NOTE: Add other enemy proyectiles object pool when created.
    [Header("Proyectiles Object Pools")]
    public ObjectPool enemyBulletProyectileObjectPool;

    /// <summary>
    /// Instance asteroid slow enemy into the level.
    /// </summary>
    public void SpawnAsteroidSlow()
    {
        GameObject spawn = Instantiate(asteriodSlow);
        InitalizeSpawn(spawn);
    }

    /// <summary>
    /// Instance simple shooter space enemy.
    /// </summary>
    /// <param name="row">string</param>
    /// <param name="position">int</param>
    public void SpawnSpaceShooterBasic(string row = "right", int position = 0)
    {
        GameObject spawn = Instantiate(spaceShooterBasic);
        InitalizeSpawn(spawn);
        
        SimpleShooterSpaceEnemy enemy = spawn.GetComponent<SimpleShooterSpaceEnemy>();

        if (enemy)
        {
            enemy.Init(enemyBulletProyectileObjectPool);

            Transform[] target = GetMovingRow(row);
            enemy.Move(target[position]);
        }
    }

    /// <summary>
    /// Instance kamikaze enemey in game screen.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="position"></param>
    public void SpawnKamikazeEnemy(string row = "right", int position = 0)
    {
        GameObject spawn = Instantiate(kamikazeEnemy);
        InitalizeSpawn(spawn);

        KamikazeSpaceEnemy enemy = spawn.GetComponent<KamikazeSpaceEnemy>();

        if (enemy)
        {
            enemy.Init();

            Transform[] target = GetMovingRow(row);
            enemy.Move(target[position]);
        }
        
    }
    
    /// <summary>
    /// Instance moving shooter enemy in game scene.
    /// </summary>
    public void SpawnMovingShooterEnemy()
    {
        GameObject spawn = Instantiate(spaceMovingShooterBasic);
        InitalizeSpawn(spawn);

        SimpleMovingSpaceShooter enemy = spawn.GetComponent<SimpleMovingSpaceShooter>();

        if (enemy)
        {
            Transform[] movingPoints = new Transform[5];

            // define moving points from the moving points matrix.
            movingPoints[0] = rowLeft[2];
            movingPoints[1] = rowMiddle[0];
            movingPoints[2] = rowMiddle[2];
            movingPoints[3] = rowMiddle[4];
            movingPoints[4] = rowRight[2];

            enemy.Init(enemyBulletProyectileObjectPool, movingPoints);
        }
    }

    /// <summary>
    /// Spawn Commander SpaceShip enemy in game scene.
    /// </summary>
    public void SpawnCommanderSpaceShip()
    {
        GameObject spawn = Instantiate(commanderSpaceShip);
        InitalizeSpawn(spawn);

        CommanderSpaceShip enemy = spawn.GetComponent<CommanderSpaceShip>();

        if (enemy)
        {
            enemy.Init(enemyBulletProyectileObjectPool, rowRight[0], rowRight[4]);
            enemy.Move(rowRight[2]);
        }
    }

    /// <summary>
    /// Spawn Commander SpaceShip enemy in game scene.
    /// </summary>
    /// <returns>CommanderSpaceShip</returns>
    public CommanderSpaceShip SpawnCommanderSpaceShipAndReturn()
    {
        GameObject spawn = Instantiate(commanderSpaceShip);
        InitalizeSpawn(spawn);

        CommanderSpaceShip enemy = spawn.GetComponent<CommanderSpaceShip>();

        if (enemy)
        {
            enemy.Init(enemyBulletProyectileObjectPool, rowRight[0], rowRight[4]);
            enemy.Move(rowRight[2]);
        }

        return enemy;
    }

    // TODO: Add here instantiate other enemy types.

    /// <summary>
    /// Initialize spawn instance position and parent.
    /// </summary>
    /// <param name="spawn">GameObject</param>
    private void InitalizeSpawn(GameObject spawn)
    {
        spawn.SetActive(true);
        spawn.transform.position = transform.position;
        spawn.transform.parent = enemiesWrapper.transform;
    }

    private Transform[] GetMovingRow(string row)
    {
        Transform[] target;

        switch (row)
        {
            case "left":
                target = rowLeft;
                break;
            case "middle":
                target = rowMiddle;
                break;
            case "right":
                target = rowRight;
                break;
            default:
                target = rowRight;
                break;
        }

        return target;
    }
}
