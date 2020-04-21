using System;
using DesignPatternsTest.BehavioralPatterns.Strategy.Interfaces;

namespace DesignPatternsTest.BehavioralPatterns.Strategy.Implementations
{
    public class ElectricEngine: IMovable
    {
        public void Move()
        {
            Console.WriteLine("Moving car with electricity");
        }
    }
}
