using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mg_entitySpawner : MonoBehaviour {

    float spawnTimer = 7f;
    float arenaRange = 8f;
    public GameObject[] entityPrefabs;

    void Update() {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0) {
            spawnTimer = 7f;
            SpawnEntity();
        }
    }

    void SpawnEntity () {
        float randomXPos = Random.Range(-arenaRange, arenaRange);
        float randomZPos = Random.Range(-arenaRange, arenaRange);
        int randomEntityIndex = Random.Range(0, entityPrefabs.Length);
        Instantiate(entityPrefabs[randomEntityIndex], new Vector3(randomXPos, entityPrefabs[randomEntityIndex].transform.position.y, randomZPos), Quaternion.identity);
    }
}
