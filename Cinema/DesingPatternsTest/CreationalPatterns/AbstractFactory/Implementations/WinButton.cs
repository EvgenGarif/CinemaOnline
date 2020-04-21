using System;
using DesignPatternsTest.CreationalPatterns.AbstractFactory.Interfaces;

namespace DesignPatternsTest.CreationalPatterns.AbstractFactory.Implementations
{
    public class WinButton : IButton
    {
        public void Draw()
        {
            Console.WriteLine("Render a button in a Windows style.");
        }
    }
}
