using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class Action_FindFood : GOAP.Action
    {
        private const int VEGETATIONMASK = 1 << 9;
        private const int CREATUREMASK = 1 << 10;

        private int m_layerMaskToUse;
        private CreatureMemory m_memory;

        private GameObject m_closestFoodToEat;

        public Action_FindFood(GameObject gameAgent) : base("FindFood", "none", "SeeFood", 1)
        {
            m_memory = gameAgent.GetComponent<CreatureMemory>();
        }

        public override void DoReset()
        {
            m_closestFoodToEat = null;
        }

        public override void DoWork(GameObject gameAgent)
        {
            switch (m_memory.Type)
            {
                case CreatureType.Vegetarian:
                    m_layerMaskToUse = VEGETATIONMASK;
                    break;
                case CreatureType.Carnivore:
                    m_layerMaskToUse = CREATUREMASK;
                    break;
            }

            Collider[] foodToEat = Physics.OverlapSphere(gameAgent.transform.position, m_memory.Vision, m_layerMaskToUse);

            float minDistance = float.MaxValue;

            foreach (Collider food in foodToEat)
            {
                // Exclude yourself from the potential food list.
                if (food == gameAgent.GetComponent<Collider>())
                    continue;

                float distance = Vector3.Distance(food.transform.position, gameAgent.transform.position);

                if (m_closestFoodToEat == null || distance < minDistance)
                {
                    minDistance = distance;
                    m_closestFoodToEat = food.gameObject;
                }
            }

            m_memory.ClosestFood = m_closestFoodToEat;
        }

        public override bool IsComplete()
        {
            if (m_closestFoodToEat != null)
            {
                Debug.Log("Found Food");
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
