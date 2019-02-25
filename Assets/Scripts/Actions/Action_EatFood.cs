using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class Action_EatFood : GOAP.Action
    {
        CreatureMemory m_memory;
        private bool m_foodEaten;

        public Action_EatFood(GameObject gameAgent) : base("EatFood", "AtFood", "FoodEaten", 1)
        {
            m_memory = gameAgent.GetComponent<CreatureMemory>();
        }

        public override void DoReset()
        {
            m_foodEaten = false;
        }

        public override bool DoWork(GameObject gameAgent)
        {
            GameObject food = m_memory.ClosestFood;
            GameObject.Destroy(food);
            m_foodEaten = true;

            return true;
        }

        public override bool IsComplete()
        {
            if (m_foodEaten == true)
            {
                Debug.Log("Food Eaten");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
