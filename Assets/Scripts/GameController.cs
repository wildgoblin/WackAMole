using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    string hammerType;
    [SerializeField] int lives = 3;
    [SerializeField] int hats = 9;

    [Header ("Lives")]
    [SerializeField] GameObject livesArea;
    [SerializeField] GameObject lifePrefab;
    [SerializeField] GameObject noLifePrefab;

    [Header ("Hats Play Space")]
    [SerializeField] GameObject hatsArea;
    [SerializeField] GameObject hat;
    [SerializeField] float revealTimeDelay = 1;

    List<int> spawnersAvailable = new List<int>();

    [SerializeField] GameObject leftCurtain;
    [SerializeField] GameObject rightCurtain;

    void Start()
    {
        SpawnLives();
        SpawnHats();
        StartCoroutine(RevealHats(revealTimeDelay));
        //START GAME (In Coroutine)

    }

    private void SpawnLives()
    {
        
        for (int i = 0; i < lives; i++)
        {
            Instantiate(lifePrefab, livesArea.transform);
            
        }
    }

    private void SpawnHats()
    {
        for (int i = 0; i < hats; i++)
        {
            // Spawn Hat
            Instantiate(hat, hatsArea.transform);
            spawnersAvailable.Add(i);
            //Hide Hat
            hatsArea.transform.GetChild(i).GetComponent<Image>().enabled = false;
        }        
    }


    private IEnumerator RevealHats(float timeDelay)
    {
        leftCurtain.GetComponent<Animator>().SetTrigger("openCurtain");
        rightCurtain.GetComponent<Animator>().SetTrigger("openCurtain");
        yield return new WaitForSeconds(3);
        int numberOfHats = hatsArea.transform.childCount;
        Debug.Log(hatsArea.transform.childCount);
        for (int i = 0; i < numberOfHats; i++)
        {
            int randomSpawner = Random.Range(0, spawnersAvailable.Count);
            int spawnerLocation = spawnersAvailable[randomSpawner];

            yield return new WaitForSeconds(timeDelay);
            hatsArea.transform.GetChild(spawnerLocation).GetComponent<Image>().enabled = true;
            RemoveFromAvailableSpawners(spawnerLocation);
        }

        StartGame();


    }



    private void StartGame()
    {
        
        EnemySpawner enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        enemySpawner.StartSpawning();
    }

    private void RemoveFromAvailableSpawners(int spawnerLocation)
    {
        if (spawnersAvailable.Count != 0)
        {
            spawnersAvailable.Remove(spawnerLocation);
        }

    }


    public void SetHammerType(string type)
    {
        hammerType = type;
    }

    public string GetHammerType()
    {
        return hammerType;
    }

    public void LoseALife()
    {
        if (lives > 0)
        {
            //Reference Variable
            Transform lifeToLose = livesArea.transform.GetChild(lives - 1);

            // Allowance Effects
            lifeToLose.GetComponent<Image>().color = Color.black;

            // Data Adjust
            lives--;
            Debug.Log("Lives left" + lives);
        }
    }

    


}
