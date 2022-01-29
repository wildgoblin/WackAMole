using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int minSpawnDelay;
    [SerializeField] int maxSpawnDelay;

    List<GameObject> spawners = new List<GameObject>();

    bool spawn = true;

    private void Start()
    {
        GetSpawnLocations();

        StartCoroutine(Spawn());
    }

    private void GetSpawnLocations()
    {
        foreach (Transform spawner in this.transform)
        {
            spawners.Add(spawner.gameObject);
        }
    }

    private IEnumerator Spawn()
    {        
        while(spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            int spawner = Random.Range(0, spawners.Count);
            Instantiate(enemyPrefab, spawners[spawner].transform.position, transform.rotation, spawners[spawner].transform);
        }
    }
}
