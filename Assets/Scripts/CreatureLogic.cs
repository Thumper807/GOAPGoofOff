using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GOAP;
using Assets.Scripts.Actions;

public class CreatureLogic : MonoBehaviour
{
    private Animator m_animator;
    private CreatureMemory m_memory;
    private ActionPlanner m_actionPlanner;
    private ActionPlan m_actionPlan;
    private Goal m_goal;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_memory = this.GetComponent<CreatureMemory>();

        List<IAction> actions = new List<IAction>()
        {
            new Action_EatFood(gameObject),
            new Action_MoveToFood(gameObject),
            new Action_FindFood(gameObject)
        };

        m_actionPlanner = new ActionPlanner(actions.ToArray());

        m_goal = new Goal("FoodEaten");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (m_actionPlan != null && !m_actionPlan.IsComplete && m_actionPlan.StillValid)
        {
            m_actionPlan.ExecuteActionPlan(this.gameObject);
        }
        else
        {
            m_actionPlan = m_actionPlanner.Plan(m_goal.DesiredEffect);
        }
    }
}
