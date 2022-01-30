using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    //References
    EnemySpawner enemySpawner;
    GameController gameController;
    SpriteRenderer spriteRenderer;

    [SerializeField] string type;
    [SerializeField] string oppositeType;
    [SerializeField] AudioClip[] hitSFX;
    [SerializeField] AudioClip[] saveSFX;

    [SerializeField] SpriteRenderer bunnyHead;
    [SerializeField] SpriteRenderer bunnyHands;

    [SerializeField] float timeBeforeDestroy = 1;
    [SerializeField] float failedHitTimeBeforeDestroy = 1;
    [SerializeField] float timeBeforeDisappear = 5;
    [SerializeField] float hitAllowanceTime = 0;
    bool availableToHit = true;
    public int position;



    private void Awake()
    {
        //Set References
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        // Check for missing Audio
        foreach (AudioClip SFX in hitSFX) { if (!SFX) { Debug.Log("MISSING AUDIO to " + this); } }
        bunnyHead = bunnyHead.GetComponent<SpriteRenderer>();
    }


    void Start()
    {
        StartCoroutine(TimeBeforeDisappear());
    }

    IEnumerator TimeBeforeDisappear()
    {
        yield return new WaitForSeconds(timeBeforeDisappear);
        if (type == "") {  }
        else { gameController.LoseALife(); }
        CleanUpAndDestroyObject();
    }

    /// <summary>
    /// When Clicking a bunny. Determine Type and execute
    /// According to that type.
    /// </summary>
    void OnMouseDown()
    {
        Debug.Log("TYPE: " + type);
        StopCoroutine(TimeBeforeDisappear());
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {

        if(availableToHit != false)
        {
            availableToHit = false;
            
            
            //FAILHIT If NO type then play fail routine
            if (type == "")
            {
                
                //Play Allowance Effects
                StartCoroutine(ChangeColor(Color.red));
                if (gameController.GetHammerType() == "fire")
                {
                    GetComponent<Animator>().SetTrigger("setFire");
                }
                if (gameController.GetHammerType() == "ice")
                {
                    GetComponent<Animator>().SetTrigger("setIce");
                }

                gameController.LoseALife();


                yield return new WaitForSeconds(failedHitTimeBeforeDestroy);
                CleanUpAndDestroyObject();
            }
            
            //SUCCESSHIT If the Type is the opposite play the SUCCEESS Hit routine
            else if (gameController.GetHammerType() == oppositeType)
            {
                //Play Allowance Effects
                StartCoroutine(ChangeColor(Color.green));
                GetComponent<Animator>().SetTrigger("saved");
                PlayHitSFX();

                //Wait for time then cleanup and destroy
                yield return new WaitForSeconds(timeBeforeDestroy);
                CleanUpAndDestroyObject();
            }

            //FAIL HIT If Type is different
            else if (gameController.GetHammerType() == type)
            {
                //Play Allowance Effects
                StartCoroutine(ChangeColor(Color.red));

                gameController.LoseALife();

                //Wait for time then cleanup and destroy
                yield return new WaitForSeconds(failedHitTimeBeforeDestroy);
                CleanUpAndDestroyObject();
            }

        }


    }

    private IEnumerator ChangeColor(Color chosenColor)
    {
        bunnyHead.color = chosenColor;
        bunnyHands.color = chosenColor;
        yield return new WaitForSeconds(hitAllowanceTime);
        bunnyHead.color = Color.white;
        bunnyHands.color = Color.white;
    }

    public void CleanUpAndDestroyObject()
    {
        enemySpawner.AddToAvailableSpawners(position);
        Destroy(gameObject);
    }

    private void PlayHitSFX()
    {
        if (hitSFX.Length > 0)
        {
            PlayAudioClipSFX(hitSFX[Random.Range(0, hitSFX.Length)]);
            PlayAudioClipSFX(saveSFX[Random.Range(0, saveSFX.Length)]);
        }
        else
        {
            Debug.Log("MISSING AUDIO FROM " + this);
        }
    }

    private void PlayAudioClipSFX(AudioClip SFX)
    {
        if(!SFX) { Debug.Log("MISSING AUDIO from " + this);  return; }
        
        AudioSource audio = GetComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(SFX, Camera.main.transform.position);
    }
    

}
