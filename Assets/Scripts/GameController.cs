using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    string hammerType;
    [SerializeField] int lives = 3;

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
        lives--;
        Debug.Log("Lives left" + lives);
    }



}
