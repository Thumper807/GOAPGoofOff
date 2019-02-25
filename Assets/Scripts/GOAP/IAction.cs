using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GOAP
{
    public interface IAction
    {
        string Name { get; }
        string PreCondition { get; }
        string Effect { get; }
        int Cost { get; }

        bool HasPrecondition();
        bool IsComplete();
        void DoReset();
        bool DoWork(GameObject gameAgent);
    }
}
