using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using FG.Assignment.Units;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private GameObject unitPreFab;
    [SerializeField] private float spawnDelay = 3; 
    
    public float nextSpawnTime;

   
    private void Update()
    {
        if (ShouldSpawn())
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(unitPreFab, transform.position, transform.rotation);
    }
    
    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
    
}
