using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{ 

    [SerializeField] string type;

    //References
    GameController gameController;
    void Awake()
    {
        //Set References
         gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    }
    void OnMouseDown()
    {
        gameController.SetHammerType(type);
    }


}
