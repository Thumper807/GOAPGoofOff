using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using System;

public class Creature_BT_Tasks : MonoBehaviour
{
    private Animator m_animator;
    private CreatureMemory m_memory;

    private const int VEGETATIONMASK = 1 << 9;
    private const int CREATUREMASK = 1 << 10;

    private int m_layerMaskToUse;

    private GameObject m_closestFoodToEat;


    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_memory = this.GetComponent<CreatureMemory>();

        switch (m_memory.Type)
        {
            case DietType.Vegetarian:
                m_layerMaskToUse = VEGETATIONMASK;
                break;
            case DietType.Carnivore:
                m_layerMaskToUse = CREATUREMASK;
                break;
        }

        m_animator.SetFloat("cycleOffset", UnityEngine.Random.Range(0.0f, 1.0f));
    }

    [Task]
    void FindFood()
    {
        Collider[] foodToEat = Physics.OverlapSphere(this.transform.position, m_memory.Vision, m_layerMaskToUse);

        float minDistance = float.MaxValue;

        foreach (Collider food in foodToEat)
        {
            // Exclude yourself from the potential food list.
            if (food == this.GetComponent<Collider>())
                continue;

            float distance = Vector3.Distance(food.transform.position, this.transform.position);

            if (m_closestFoodToEat == null || distance < minDistance)
            {
                minDistance = distance;
                m_closestFoodToEat = food.gameObject;
            }
        }

        m_memory.ClosestFood = m_closestFoodToEat;

        Task.current.Complete(m_memory.ClosestFood != null);
    }

    [Task]
    void MoveToFood()
    {
        try
        {
            Vector3 targetPosition = new Vector3(m_memory.ClosestFood.transform.position.x, 0.0f, m_memory.ClosestFood.transform.position.z);
            float distance = Vector3.Distance(this.transform.position, targetPosition);

            if (distance > m_memory.CloseEnoughRadius)
            {
                Vector3 directionToTargetPosition = targetPosition - this.transform.position;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(directionToTargetPosition), m_memory.RotSpeed * Time.deltaTime);

                m_animator.SetBool("isMoving", true);
            }
            else
            {
                m_animator.SetBool("isMoving", false);
                Task.current.Succeed();
            }

        }
        catch (Exception ex)
        {
            m_animator.SetBool("isMoving", false);
            Task.current.Fail();
        }
    }

    [Task]
    void EatFood()
    {
        GameObject food = m_memory.ClosestFood;
        GameObject.Destroy(food);
        Task.current.Succeed();
    }
}
