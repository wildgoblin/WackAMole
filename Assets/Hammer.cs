using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, IHammer
{
    
    [SerializeField] public string hammerType { get; set; }

    public void Initialize()
    {
        hammerType = hammerType;
    }
}
