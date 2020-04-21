using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatternsTest.CreationalPatterns.AbstractFactory.Interfaces;

namespace DesignPatternsTest.CreationalPatterns.AbstractFactory.Implementations
{
    public class OsxProgressBar : IProgressBar
    {
        public void Draw()
        {
            Console.WriteLine("Render a Progress Bar in OSX style.");
        }

        public void SetProgress(int percent)
        {
            Console.WriteLine($"Osx progress bar value {percent}%.");
        }
    }
}
