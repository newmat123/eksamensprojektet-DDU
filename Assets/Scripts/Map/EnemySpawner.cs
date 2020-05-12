using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] Enemys;

    private Vector3 startPoint;
    public Vector3 endPoint;

    public float upMove = 10f;

    public int enemysToSpawn = 3;

    public LayerMask obj;



    void Start()
    {
        startPoint = transform.position;
        SpawnEnemys();
    }


    public void SpawnEnemys()
    {
        Vector2 up = new Vector2(transform.position.x, transform.position.y + upMove);
        transform.position = up;
        int spawned = 0;

        while (spawned < enemysToSpawn)
        {
            float x = Random.Range(startPoint.x, endPoint.x);
            Vector2 spawnPoint = new Vector2(x, transform.position.y);

            transform.position = spawnPoint;

            Collider2D objectDetction = Physics2D.OverlapCircle(transform.position, 1, obj);
            if (objectDetction = null)
            {
                int rand = Random.Range(0, Enemys.Length);
                Instantiate(Enemys[rand], transform.position, Quaternion.identity);
                spawned++;
            }
        }
        if (transform.position.y < endPoint.y)
        {
            SpawnEnemys();
        }
    }
}
