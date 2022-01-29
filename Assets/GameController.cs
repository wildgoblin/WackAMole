using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    string hammerType;

    void Update()
    {
        Debug.Log("Hammer Type: " + hammerType);
    }

    public void SetHammerType(string type)
    {
        hammerType = type;
    }

    public string GetHammerType()
    {
        return hammerType;
    }

}
