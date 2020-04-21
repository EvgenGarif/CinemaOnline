using System;
using DesignPatternsTest.CreationalPatterns.AbstractFactory.Interfaces;

namespace DesignPatternsTest.CreationalPatterns.AbstractFactory.Implementations
{
    public class OsxButton : IButton
    {
        public void Draw()
        {
            Console.WriteLine("Render a button in OSX style.");
        }
    }
}
