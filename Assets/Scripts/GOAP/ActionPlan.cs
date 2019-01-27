using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GOAP
{
    public class ActionPlan
    {
        public List<IAction> Plan;
        public int TotalCost;
        public bool Valid;  // Are all Preconditions met (if there is one, did it find an action to satisfy)

        public ActionPlan(IAction rootAction)
        {
            Plan = new List<IAction>();
            AddAction(rootAction);
        }

        public void AddAction(IAction action)
        {
            Plan.Add(action);
            TotalCost += action.Cost;
        }

        public void InsertActionPlan(ActionPlan actionPlan)
        {
            if (actionPlan == null)
            {
                TotalCost = int.MaxValue;
                Valid = false;
            }
            else
            {
                Plan.InsertRange(0, actionPlan.Plan);
                TotalCost += actionPlan.TotalCost;
                Valid = actionPlan.Valid;
            }
        }

        public bool ExecuteActionPlan(GameObject go)
        {
            if (!Plan[0].IsValid())
                return false;

            Plan[0].DoWork(go);

            if (Plan[0].IsComplete())
                Plan.RemoveAt(0);

            return true;
        }
    }
}
