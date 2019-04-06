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
    private GameObject m_attention;

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

        m_attention = this.transform.Find("Attention").gameObject;
    }

    [Task]
    void FindFood()
    {
        Collider[] foodToEat = Physics.OverlapSphere(this.transform.position, m_memory.Vision, m_layerMaskToUse);

        float closestDistance = float.MaxValue;
        GameObject closestFood = null;

        foreach (Collider food in foodToEat)
        {
            // Exclude yourself from the potential food list.
            if (food == this.GetComponent<Collider>())
                continue;

            float distance = Vector3.Distance(food.transform.position, this.transform.position);

            if (CheckIfBlocked(food.gameObject))
            {
                m_attention.SetActive(true);
                //Debug.Log(string.Format("{0}: Blocked while finding food", this.name));

                continue;
            }

            if (closestFood == null || distance < closestDistance)
            {
                closestDistance = distance;
                closestFood = food.gameObject;
            }
        }

        if (closestFood != null)
        {
            m_memory.ClosestFood = closestFood;
            m_attention.SetActive(false);
            Task.current.Succeed();
        }
        else
        {
            m_memory.ClosestFood = null;
            m_attention.SetActive(true);
            Task.current.Fail();
        }
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

                if (CheckIfBlocked(m_memory.ClosestFood))
                {
                    m_animator.SetBool("isMoving", false);
                    m_attention.SetActive(true);

                    //Debug.Log(string.Format("{0}: Blocked while moving", this.name));

                    Task.current.Fail();
                    return;
                }

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(directionToTargetPosition), m_memory.RotSpeed * Time.deltaTime);

                if (m_animator.GetBool("isMoving") != true)
                {
                    m_animator.SetBool("isMoving", true);
                    m_attention.SetActive(false);
                }
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

    [Task]
    private bool CheckIfBlocked(GameObject target)
    {
        if (target == null)
            return false;

        Vector3 targetPosition = new Vector3(target.transform.position.x, 0.0f, target.transform.position.z);
        float distance = Vector3.Distance(this.transform.position, targetPosition);

        Vector3 directionToTargetPosition = targetPosition - this.transform.position;
        Debug.DrawRay(this.transform.position, directionToTargetPosition, Color.magenta);

        RaycastHit rayHitInfo;

        if (Physics.Raycast(this.transform.position, directionToTargetPosition, out rayHitInfo, distance, CREATUREMASK))
        {
            CreatureMemory otherCreatureMemory = rayHitInfo.transform.GetComponent<CreatureMemory>();
            return otherCreatureMemory.ClosestFood == target;
        }

        return false;
    }
}
