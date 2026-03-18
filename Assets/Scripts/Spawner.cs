using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;

    float timeToNextSpawn;
    float timeSinceLastSpawn = 0.0f;

    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 3.0f;

    void Update()
    {
        {
            timeSinceLastSpawn = timeSinceLastSpawn + Time.deltaTime;
            if (timeSinceLastSpawn > timeToNextSpawn)
            {
                int selection = Random.Range(0, objectsToSpawn.Length);

                Instantiate(objectsToSpawn[selection], transform.position, Quaternion.identity);

                timeToNextSpawn = Random.Range(minSpawnTime, maxSpawnTime);
                timeSinceLastSpawn = 0.0f;
            }
        }
    }
}
