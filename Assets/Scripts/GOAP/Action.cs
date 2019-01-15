using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GOAP
{
    public abstract class Action : IAction
    {
        private string m_name;
        private string m_precondtion;
        private string m_effect;
        private int m_cost;

        public Action(string name, string preCondition, string effect, int cost)
        {
            m_name = name;
            m_precondtion = preCondition;
            m_effect = effect;
            m_cost = cost;
        }

        public string Name { get { return m_name; } }
        public string PreCondition { get { return m_precondtion; } }
        public string Effect { get { return m_effect; } }
        public int Cost { get { return m_cost; } }

        public bool HasPrecondition()
        {
            return m_precondtion != "none";
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsComplete()
        {
            throw new NotImplementedException();
        }

        public virtual void DoReset()
        {
            throw new NotImplementedException();
        }

        public virtual void DoWork(GameObject gameAgent)
        {
            throw new NotImplementedException();
        }
    }
}
