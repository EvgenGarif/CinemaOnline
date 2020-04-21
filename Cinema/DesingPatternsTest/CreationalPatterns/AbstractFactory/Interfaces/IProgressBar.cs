﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsTest.CreationalPatterns.AbstractFactory.Interfaces
{
    public interface IProgressBar
    {
        void Draw();
        void SetProgress(int percents);
    }
}
