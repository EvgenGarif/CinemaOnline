using System;
using DesignPatternsTest.BehavioralPatterns.Strategy.Interfaces;

namespace DesignPatternsTest.BehavioralPatterns.Strategy.Implementations
{
    public class PetrolEngine : IMovable
    {
        public void Move()
        {
            Console.WriteLine("Moving car with petrol");
        }
    }
}
