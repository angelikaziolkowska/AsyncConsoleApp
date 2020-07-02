using System;
using System.Collections.Generic;
using System.Text;
using static System.Math; //static imports methods from one class
                          //as opposed to all classes from namespace

namespace AsyncConsoleApp
{
    internal class TelePrompterConfig
    {
        public int DelayInMilliseconds { get; private set; } = 200;
        public void UpdateDelay(int increment) // negative to speed up
        {
            var newDelay = Min(DelayInMilliseconds + increment, 1000);
            newDelay = Max(newDelay, 20);
            DelayInMilliseconds = newDelay;
        }
        public bool Done { get; private set; }
        public void SetDone()
        {
            Done = true;
        }
    }
    
}
