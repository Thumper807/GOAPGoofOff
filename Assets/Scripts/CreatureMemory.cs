using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMemory : MonoBehaviour
{
    public GameObject ClosestFood;
    
    [Header("Creature Attributes")]
    public DietType Type;

    [Range(0.1f, 5.0f)]
    public float Speed = 1.0f;

    [Range(4.0f, 7.0f)]
    public float RotSpeed = 5.0f;

    public float CloseEnoughRadius = 1.0f;
    public int Health;
    public int Vision;
    public Status State;

    // Start is called before the first frame update
    void Start()
    {
        Type = DietType.Vegetarian;
        Vision = 20;
        Speed = Random.Range(3.0f, 7.0f);
        RotSpeed = Random.Range(4.0f, 7.0f);
    }
}

public enum DietType { Vegetarian, Carnivore, Omnivore };
public enum Status { Idle, Walking, Running };

