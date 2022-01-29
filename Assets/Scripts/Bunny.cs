using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    //References
    EnemySpawner enemySpawner;

    [SerializeField] AudioClip[] hitSFX;
    [SerializeField] AudioClip[] saveSFX;

    [SerializeField] int timeBeforeDestroy = 1;
    public int position;

    private void Awake()
    {
        //Set References
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        Debug.Log(enemySpawner);

        // Check for missing Audio
        foreach(AudioClip SFX in hitSFX) { if (!SFX) { Debug.Log("MISSING AUDIO to " + this); } }
    }

    void OnMouseDown()
    {
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        GetComponent<SpriteRenderer>().color = Color.red;

        PlayHitSFX();

        yield return new WaitForSeconds(timeBeforeDestroy);
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
