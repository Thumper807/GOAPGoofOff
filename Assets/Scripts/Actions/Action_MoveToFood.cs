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
                Vector3 lookAtGoal = new Vector3(m_memory.ClosestFood.transform.position.x, 0.5f, m_memory.ClosestFood.transform.position.z);
                Vector3 directionToTargetPosition = lookAtGoal - gameAgent.transform.position;

                gameAgent.transform.rotation = Quaternion.Slerp(gameAgent.transform.rotation, Quaternion.LookRotation(directionToTargetPosition), m_memory.RotSpeed * Time.deltaTime);

                if (Vector3.Distance(gameAgent.transform.position, lookAtGoal) > m_memory.CloseEnoughRadius)
                {
                    gameAgent.transform.Translate(0, 0, m_memory.Speed * Time.deltaTime);
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
