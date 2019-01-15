using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMemory : MonoBehaviour
{
    public GameObject ClosestFood;
    public CreatureType Type;
    public int Speed;
    public int Health;
    public int Vision;

    // Start is called before the first frame update
    void Start()
    {
        Type = CreatureType.Vegetarian;
        Vision = 20;
    }
}

public enum CreatureType { Vegetarian, Carnivore, Omnivore };

