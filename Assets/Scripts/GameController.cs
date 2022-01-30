using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    string hammerType;
    
    [SerializeField] int lives = 3;
    [SerializeField] int hats = 9;
    int remainingLives;

    [Header ("Lives")]
    [SerializeField] GameObject livesArea;
    [SerializeField] GameObject lifePrefab;
    [SerializeField] Sprite noLifePrefab;

    [Header ("Hats Play Space")]
    [SerializeField] GameObject hatsArea;
    [SerializeField] GameObject hat;
    [SerializeField] float revealTimeDelay = 1;
    [SerializeField] GameObject CloudFX;

    [SerializeField] GameObject leftCurtain;
    [SerializeField] GameObject rightCurtain;
     float curtainDelayTime = 3; // Keep at 3, Animation is 3 seconds.

    [SerializeField] GameObject mainStartScreen;

    List<int> spawnersAvailable = new List<int>();

    bool gameStarted = false;
    EnemySpawner enemySpawner;

    void Start()
    {
        //References
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        // Initialize
        gameStarted = false;
        mainStartScreen.SetActive(true);
    }
    public void StartGame()
    {


        //Initialize
        remainingLives = lives;             
        
        gameStarted = true;
        mainStartScreen.SetActive(false);
        StartCoroutine(LoadMainGame(revealTimeDelay));
        //START GAME (In Coroutine)
    }

    private void Update()
    {
        //Check Lose Condition
        if(remainingLives <= 0 && gameStarted)
        {
            StartCoroutine(LoseGameCleanUp());
        }

    }

    private IEnumerator LoseGameCleanUp()
    {
        //Do Lose condition
        // Stop Spawn
        StopSpawning();
        // Clear Board
        enemySpawner.RemoveAllSpawnedEnemies();
        // Close curtains
        leftCurtain.GetComponent<Animator>().SetTrigger("closeCurtain");
        rightCurtain.GetComponent<Animator>().SetTrigger("closeCurtain");
        yield return new WaitForSeconds(curtainDelayTime);
        leftCurtain.GetComponent<Animator>().SetTrigger("stayOpen");

        gameStarted = false;
        mainStartScreen.SetActive(true);
        //Reset Lives
        CleanUpLives();        
        
        //Reset Hats
        CleanUpHats();
        // Show Main screen
        
        
    }

    private void SpawnLives()
    {        
        
        for (int i = 0; i < remainingLives; i++)
        {
            //Spawn Lives
            Instantiate(lifePrefab, livesArea.transform);
            // Hide lives
            livesArea.transform.GetChild(i).GetComponent<Image>().enabled = false;
        }
    }

    private void CleanUpLives()
    {
        foreach(Transform life in livesArea.transform)
        {
            Destroy(life.gameObject);
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

    private void CleanUpHats()
    {
        foreach (Transform hat in hatsArea.transform)
        {
            Destroy(hat.gameObject);
        }
    }


    private IEnumerator LoadMainGame(float timeDelay)
    {
        //Reveal Lives

        /*int numberOfLives = livesArea.transform.childCount;
        for (int i = 0; i < numberOfLives; i++)
        {
            Instantiate(CloudFX, livesArea.transform.GetChild(i).transform);
            yield return new WaitForSeconds(timeDelay);
            livesArea.transform.GetChild(i).GetComponent<Image>().enabled = true;
            Destroy(livesArea.transform.GetChild(i).GetChild(0).gameObject);
        }
        */
        SpawnLives();
        SpawnHats();

        //Reveal Lives
        foreach (Transform life in livesArea.transform)
        {
            Instantiate(CloudFX, life.transform);
            yield return new WaitForSeconds(timeDelay);
            life.GetComponent<Image>().enabled = true;
            foreach(Transform child in life.transform)
            {
                Destroy(life.GetChild(0).gameObject);
            }
                
        }

        //Open Curtains
        leftCurtain.GetComponent<Animator>().SetTrigger("openCurtain");
        rightCurtain.GetComponent<Animator>().SetTrigger("openCurtain");
        yield return new WaitForSeconds(curtainDelayTime);



        //Reveal Hats
        int numberOfHats = hatsArea.transform.childCount;
        for (int i = 0; i < numberOfHats; i++)
        {
            int randomSpawner = Random.Range(0, spawnersAvailable.Count);
            int spawnerLocation = spawnersAvailable[randomSpawner];

            Instantiate(CloudFX, hatsArea.transform.GetChild(spawnerLocation).transform);
            yield return new WaitForSeconds(timeDelay);
            hatsArea.transform.GetChild(spawnerLocation).GetComponent<Image>().enabled = true;
            RemoveFromAvailableSpawners(spawnerLocation);
            Destroy(hatsArea.transform.GetChild(spawnerLocation).GetChild(0).gameObject);
        }

        StartSpawning();


    }



    private void StartSpawning()
    {     
        
        enemySpawner.StartSpawning();
    }

    private void StopSpawning()
    {
        enemySpawner.StopSpawning();
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
        if (remainingLives > 0 && gameStarted)
        {
            //Reference Variable
            Transform lifeToLose = livesArea.transform.GetChild(remainingLives-1);

            // Allowance Effects
            lifeToLose.GetComponent<Image>().sprite = noLifePrefab;

            // Data Adjust
            remainingLives--;
            Debug.Log("Lives left" + remainingLives);
        }
    }

    public bool GetGameStartState()
    {
        return gameStarted;
    }

    


}
