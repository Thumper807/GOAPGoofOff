using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GOAP
{
    public class ActionPlanner
    {
        private IAction[] m_actions;

        public ActionPlanner(IAction[] actions)
        {
            m_actions = actions;
        }

        public ActionPlan Plan(string desiredEffect)
        {
            foreach (IAction action in m_actions)
                action.DoReset();


            ActionPlan bestActionPlan = null;
            IAction[] desiredEffectActions = Array.FindAll(m_actions, a => a.Effect == desiredEffect);

            foreach (IAction action in desiredEffectActions)
            {
                ActionPlan actionPlan = new ActionPlan(action);

                if (action.HasPrecondition())
                    actionPlan.InsertActionPlan(Plan(action.PreCondition));
                else
                    actionPlan.Valid = true;

                if (bestActionPlan == null || (actionPlan.Valid && actionPlan.TotalCost < bestActionPlan.TotalCost))
                    bestActionPlan = actionPlan;
            }

            return bestActionPlan;
        }
    }
}
