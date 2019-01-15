using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Actions
{
    public class Action_MoveToFood : GOAP.Action
    {
        private CreatureMemory m_memory;
        private bool m_atTarget;

        public Action_MoveToFood(GameObject gameAgent) : base("MoveToFood", "SeeFood", "AtFood", 1)
        {
            m_memory = gameAgent.GetComponent<CreatureMemory>();
        }

        public override void DoReset()
        {
            m_atTarget = false;
        }

        public override void DoWork(GameObject gameAgent)
        {
            if (m_memory.ClosestFood != null)
            {
                Vector3 directionToTargetPosition = m_memory.ClosestFood.transform.position - gameAgent.transform.position;
                float distThisFrame = 5 * Time.deltaTime;
                if (directionToTargetPosition.magnitude > distThisFrame)
                {
                    gameAgent.transform.Translate(directionToTargetPosition.normalized * distThisFrame, Space.World);
                }
                else
                {
                    m_atTarget = true;
                }
            }
        }

        public override bool IsComplete()
        {
            if (m_atTarget)
            {
                Debug.Log("At Target");
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool IsValid()
        {
            return m_memory.ClosestFood != null;
        }
    }
}
