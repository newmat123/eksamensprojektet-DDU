﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{

    public GameObject[] Tiles;

    private void Start()
    {
        int rand = Random.Range(0, Tiles.Length);
        GameObject instance = (GameObject)Instantiate(Tiles[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
    }

}
