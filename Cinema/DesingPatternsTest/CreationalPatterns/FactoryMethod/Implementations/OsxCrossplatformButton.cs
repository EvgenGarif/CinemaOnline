using System;
using DesignPatternsTest.CreationalPatterns.FactoryMethod.Interfaces;

namespace DesignPatternsTest.CreationalPatterns.FactoryMethod.Implementations
{
    public class OsxCrossplatformButton : ICrossplatformButton
    {
        public void Draw()
        {
            Console.WriteLine("Render a button in OSX style.");
        }
    }
}
