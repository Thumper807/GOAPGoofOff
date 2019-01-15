using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GOAP
{
    public class Goal
    {
        private string m_desiredEffect;

        public Goal(string desiredEffect)
        {
            m_desiredEffect = desiredEffect;
        }

        public string DesiredEffect { get { return m_desiredEffect; } }
    }
}
