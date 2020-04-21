using System;
using DesignPatternsTest.CreationalPatterns.FactoryMethod.Interfaces;

namespace DesignPatternsTest.CreationalPatterns.FactoryMethod.Implementations
{
    public class WinCrossplatformButton : ICrossplatformButton
    {
        public void Draw()
        {
            Console.WriteLine("Render a button in a Windows style.");
        }
    }
}
