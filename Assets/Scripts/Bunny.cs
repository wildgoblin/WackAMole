using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] int timeBeforeDestroy = 1;
    public int position;
    

    void OnMouseDown()
    {
        StartCoroutine(Hit());        
    }

    private IEnumerator Hit()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(gameObject);
    }
    

}
