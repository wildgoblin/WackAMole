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
    [SerializeField] AudioClip[] hitSFX;
    [SerializeField] AudioClip[] saveSFX;

    [SerializeField] int timeBeforeDestroy = 1;
    public int position;

    private void Awake()
    {
        //Set References
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Check for missing Audio
        foreach (AudioClip SFX in hitSFX) { if (!SFX) { Debug.Log("MISSING AUDIO to " + this); } }
    }

    void OnMouseDown()
    {
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        // If NO type then play fail routine
        if (gameController.GetHammerType() == null)
        {
            spriteRenderer.color = Color.red;
        }
        // If the Type is the same play the success Hit routine
        else if (gameController.GetHammerType() == type)
        {
            spriteRenderer.color = Color.green;

            PlayHitSFX();

            yield return new WaitForSeconds(timeBeforeDestroy);
            enemySpawner.AddToAvailableSpawners(position);
            Destroy(gameObject);
        }
        // If Type is different
        else if (gameController.GetHammerType() != type)
        {
            spriteRenderer.color = Color.blue;
        }
        

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
