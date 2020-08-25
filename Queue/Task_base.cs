using System;
using System.Collections.Generic;
using System.Text;

namespace RaylibTest.Queue
{
    class Task_base
    {
        public string TaskType;
        public dynamic Arguments = new dynamic[] { };
        public Priority_enum priority = Priority_enum.Normal;
        public bool Permanent = false;
        public enum Priority_enum
        {
            Very_High = 1,
            High = 2,
            Normal = 3,
            Low = 4,
            Lowest = 5
        }

        public virtual dynamic Run_Task() => throw new NotImplementedException();



    }
}
