using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] Enemys;
    public GameObject[] Loot;

    private Vector3 startPoint;
    public Vector3 endPoint;

    public float upMove = 10f;

    public int enemysToSpawn = 2;
    public int itemsToSpawn = 1;

    public LayerMask obj;

    private int upCount = 0;


    void Start()
    {
        startPoint = transform.position;
        SpawnEnemys();
    }

    void spawn(int xToSpawn, GameObject[] arr)
    {
        for(int i = 0; i < xToSpawn; i++)
        {
            float x = Random.Range(startPoint.x, endPoint.x);
            Vector2 spawnPoint = new Vector2(x, transform.position.y);

            transform.position = spawnPoint;

            int rand = Random.Range(0, arr.Length);

            Instantiate(arr[rand], transform.position, Quaternion.identity);
        }
        return;
    }

    public void SpawnEnemys()
    {
        Vector2 up = new Vector2(transform.position.x, transform.position.y + upMove);
        transform.position = up;

        spawn(enemysToSpawn, Enemys);
        spawn(itemsToSpawn, Loot);

  
        if (transform.position.y < endPoint.y)
        {
            SpawnEnemys();
        }
    }
}
