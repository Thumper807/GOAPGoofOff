using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMemory : MonoBehaviour
{
    public GameObject ClosestFood;
    
    [Header("Creature Attributes")]
    public CreatureType Type;

    [Range(0.1f, 5.0f)]
    public float Speed = 1.0f;

    [Range(0.1f, 180.0f)]
    public float RotSpeed = 5.0f;

    public float CloseEnoughRadius = 5.0f;
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

