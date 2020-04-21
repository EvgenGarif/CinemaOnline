using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatternsTest.CreationalPatterns.FactoryMethod.Interfaces;

namespace DesignPatternsTest.CreationalPatterns.FactoryMethod.AbstractClasses
{
    public abstract class ButtonCreator
    {
        public abstract ICrossplatformButton CreateButton();

        public ICrossplatformButton RenderButton()
        {
            var button = CreateButton();
            button.Draw();
            return button;
        }
    }
}
