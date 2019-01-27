using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.GOAP;
using Assets.Scripts.Actions;

public class CreatureLogic : MonoBehaviour
{
    private CreatureMemory m_memory;
    private ActionPlanner m_actionPlanner;
    private ActionPlan m_actionPlan;
    private Goal m_goal;

    // Start is called before the first frame update
    void Start()
    {
        m_memory = this.GetComponent<CreatureMemory>();

        List<IAction> actions = new List<IAction>();
        actions.Add(new Action_EatFood(gameObject));
        actions.Add(new Action_MoveToFood(gameObject));
        actions.Add(new Action_FindFood(gameObject));

        m_actionPlanner = new ActionPlanner(actions.ToArray());

        m_goal = new Goal("FoodEaten");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_actionPlan == null || m_actionPlan.Plan.Count == 0 || !m_actionPlan.Valid)
        {
            m_actionPlan = m_actionPlanner.Plan(m_goal.DesiredEffect);
        }
        else
        {
            m_actionPlan.ExecuteActionPlan(this.gameObject);
        }
    }
}
