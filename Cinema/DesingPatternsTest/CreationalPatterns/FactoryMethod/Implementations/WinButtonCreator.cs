using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatternsTest.CreationalPatterns.FactoryMethod.AbstractClasses;
using DesignPatternsTest.CreationalPatterns.FactoryMethod.Interfaces;

namespace DesignPatternsTest.CreationalPatterns.FactoryMethod.Implementations
{
    public class WinButtonCreator : ButtonCreator
    {
        public override ICrossplatformButton CreateButton()
        {
            return new WinCrossplatformButton();
        }
    }
}
