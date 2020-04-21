using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatternsTest.BehavioralPatterns.Strategy.Interfaces;

namespace DesignPatternsTest.BehavioralPatterns.Strategy.Implementations
{
    public class HybridCar
    {
        public IMovable Strategy { get; set; }

        public HybridCar(IMovable strategy)
        {
            Strategy = strategy;
        }

        public void Move()
        {
            Strategy.Move();
        }
    }
}
