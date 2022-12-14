using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyToSpawn;
    private float xPos, zPos;
    public int enemyNumber;
    public float timeBetweenSpawn = 0.01f;
    private int spawnCount;
    private bool alreadySpawned;

    private List<GameObject> enemyInRoom = new();
    public int enemiesInRoom { get { return enemyInRoom.Count; } }

    public Vector3 deltaSpawn;

    private float xMin, xMax, zMin, zMax;
    public bool randomSpawn;

    private bool isPlayerInside = false;

    private Vector3 minPosition;
    private Vector3 maxPosition;

    private RoomProperties room;

    public delegate void OnRoomCleared();
    public OnRoomCleared onRoomCleared;

    void Start()
    {
        alreadySpawned = false;
        float x, y, z,dx,dz;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        dx = Mathf.Clamp(deltaSpawn.x, 0, transform.localScale.x / 2);
        dz = Mathf.Clamp(deltaSpawn.z, 0, transform.localScale.z / 2);

        xMin = x - dx;
        xMax = x + dx;
        zMin = z - dz;
        zMax = z + dz;

        Bounds bounds = GetComponent<BoxCollider>().bounds;

        minPosition = bounds.min;
        maxPosition = bounds.max;

        room = GetComponent<RoomProperties>();
        room.onEnterDialoguePlayed += SpawnEnemies;

    }

    IEnumerator EnemyDrop()
    {
        while (spawnCount < enemyNumber)
        { 
            xPos = Random.Range(xMin, xMax);
            zPos = Random.Range(zMin, zMax);
            Instantiate(enemyToSpawn, new Vector3(xPos, 0, zPos),Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenSpawn);
            spawnCount ++;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (alreadySpawned)
            {
                SpawnEnemies();
            }

        }
        else if (other.CompareTag("Enemy"))
        {
            if (!isPlayerInside)
            {
                other.gameObject.SetActive(false);
            }
            if (!other.gameObject.GetComponent<Enemy>().addedToList)
            {
                other.gameObject.GetComponent<Enemy>().InitiateProperties(enemyInRoom.Count, RemoveEnnemyFromList,IsInRoom);
                enemyInRoom.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            foreach ( GameObject enemy in enemyInRoom)
            {
                enemy.SetActive(false);
            }
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().ResetPosition();
        }
    }

    private void RemoveEnnemyFromList(int number)
    {
        enemyInRoom.RemoveAt(number);
        for (int i=number;i<enemyInRoom.Count; i++)
        {
            enemyInRoom[i].GetComponent<Enemy>().number = i;
        }

        if (enemyInRoom.Count == 0)
        {
            onRoomCleared();
        }
    }

    public void SpawnEnemies()
    {
        if (!alreadySpawned)
        {
            if (randomSpawn)
            {
                StartCoroutine(EnemyDrop());
            }
            alreadySpawned = true;
        }

        foreach (GameObject enemy in enemyInRoom)
        {
            enemy.SetActive(true);
        }
    }

    public bool IsInRoom(Vector3 position)
    {
        bool isIn = true;

        for (int i=0;i<3; i++)
        {
            if (position[i] < minPosition[i] || position[i] > maxPosition[i])
                isIn = false;
        }
        return isIn;
    }
}
