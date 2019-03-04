using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Actions
{
    public class Action_MoveToFood_Chopper : GOAP.Action
    {
        private CreatureMemory m_memory;
        private bool m_atTarget;

        public Action_MoveToFood_Chopper(GameObject gameAgent) : base("MoveToFood", "SeeFood", "AtFood", 1)
        {
            m_memory = gameAgent.GetComponent<CreatureMemory>();
        }

        public override void DoReset()
        {
            m_atTarget = false;
        }

        public override bool DoWork(GameObject gameAgent)
        {
            if (m_memory.ClosestFood != null)
            {
                Vector3 lookAtGoal = new Vector3(m_memory.ClosestFood.transform.position.x, 0.0f, m_memory.ClosestFood.transform.position.z);
                Vector3 directionToTargetPosition = lookAtGoal - gameAgent.transform.position;

                gameAgent.transform.rotation = Quaternion.Slerp(gameAgent.transform.rotation, Quaternion.LookRotation(directionToTargetPosition), m_memory.RotSpeed * Time.deltaTime);

                if (Vector3.Distance(gameAgent.transform.position, lookAtGoal) > m_memory.CloseEnoughRadius)
                {
                    if (m_memory.State != Status.Running)
                    {
                        m_memory.State = Status.Running;
                        gameAgent.GetComponent<ChopperLogic>().ChopperAnimator.SetTrigger("isRunning");
                    }
                    //gameAgent.transform.Translate(0, 0, m_memory.Speed * Time.deltaTime);
                }
                else
                {
                    m_atTarget = true;

                    if (m_memory.State != Status.Idle)
                    {
                        m_memory.State = Status.Idle;
                        gameAgent.GetComponent<ChopperLogic>().ChopperAnimator.SetTrigger("isIdle");
                    }
                }

                return true;
            }
            else if (m_memory.State != Status.Idle)
            {
                m_memory.State = Status.Idle;
                gameAgent.GetComponent<ChopperLogic>().ChopperAnimator.SetTrigger("isIdle");
            }

            return false;
        }

        public override bool IsComplete()
        {
            if (m_atTarget)
            {
                //Debug.Log("At Target");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
