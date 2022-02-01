using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;


    
    //Spawners
    List<int> spawnersAvailable = new List<int>();
    List<GameObject> spawners = new List<GameObject>();

    bool spawn = false;

    private void Start()
    {
        //Initilization
        GetSpawnLocations();
        GetSpawnersAvailable();
    }


    /// <summary>
    /// Get All Spawn Locations as part of the
    /// parent Spawner
    /// </summary>
    private void GetSpawnLocations()
    {
        foreach (Transform spawner in this.transform)
        {
            spawners.Add(spawner.gameObject);
        }
    }

    /// <summary>
    /// Get all spawners as a interger list to
    /// determine the location of each spawner
    /// </summary>
    private void GetSpawnersAvailable()
    {
        int spawnerNumber = 0;
        foreach (GameObject spawner in spawners)
        {            
            if (spawner.transform.childCount == 0)
            {
                spawnersAvailable.Add(spawnerNumber);
            }
            else
            {
                Debug.Log("Add Something here: " + spawnerNumber);
            }
            spawnerNumber++;
        }
    }

    private IEnumerator Spawn()
    {

        while (spawn)
        {

            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            if (spawnersAvailable.Count == 0) { Debug.Log("No Spawners Available"); }
            else if (!spawn) { break;  }
            else
            {
                //Choose Random Bunny to spawn
                int randomBunny = Random.Range(0, enemyPrefab.Length);

                //Get random spawn location out of AvailableSpawners
                int randomSpawner = Random.Range(0, spawnersAvailable.Count);
                int spawnerLocation = spawnersAvailable[randomSpawner];

                //Spawn bunny
                GameObject bunny = Instantiate(
                    enemyPrefab[randomBunny],
                    spawners[spawnerLocation].transform.position, // at random spawner location
                    transform.rotation, // Don't change rotation
                    spawners[spawnerLocation].transform); // Child new object under spawner

                bunny.GetComponent<Bunny>().position = spawnerLocation;
                //Update Available spawners
                RemoveFromAvailableSpawners(spawnerLocation);
            }
        }
    }

    private void RemoveFromAvailableSpawners(int spawnerLocation)
    {
        if(spawnersAvailable.Count != 0)
        {
            spawnersAvailable.Remove(spawnerLocation);
        }
        
    }

    public void AddToAvailableSpawners(int spawnerLocation)
    {
        spawnersAvailable.Add(spawnerLocation);

    }

    public void StartSpawning()
    {
        spawn = true;
        StartCoroutine(Spawn());
    }

    public void StopSpawning()
    {
        spawn = false;
    }

    public void RemoveAllSpawnedEnemies()
    {
        foreach (GameObject spawner in spawners)
        {
            foreach(Transform spawnerChild in spawner.transform)
                {
                Destroy(spawnerChild.gameObject);
            }
        }
    }

    public void UpdateMaxSpawnSpeed(float newSpeed)
    {
        maxSpawnDelay -= newSpeed;
    }
}
