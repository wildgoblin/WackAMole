using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int minSpawnDelay;
    [SerializeField] int maxSpawnDelay;

    List<int> spawnersAvailable = new List<int>();

    List<GameObject> spawners = new List<GameObject>();

    bool spawn = true;

    private void Start()
    {
        //Initilization
        GetSpawnLocations();
        GetSpawnersAvailable();

        //Start Game
        StartCoroutine(Spawn());
    }



    private void GetSpawnLocations()
    {
        foreach (Transform spawner in this.transform)
        {
            spawners.Add(spawner.gameObject);
        }
    }

    private void GetSpawnersAvailable()
    {
        int spawnerNumber = 0;
        foreach (GameObject spawner in spawners)
        {            
            if (spawner.transform.childCount == 0)
            {
                Debug.Log("Added: " + spawnerNumber);
                spawnersAvailable.Add(spawnerNumber);
            }
            else
            {
                Debug.Log("Something here: " + spawnerNumber);
            }
            spawnerNumber++;
        }
    }

    private IEnumerator Spawn()
    {

        while (spawn)
        {


            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            int spawner = Random.Range(0, spawners.Count); // Get random spawner
            Instantiate(
                enemyPrefab,
                spawners[spawner].transform.position, // at random spawner location
                transform.rotation, // Don't change rotation
                spawners[spawner].transform); // Child new object under spawner
        }
    }
}
