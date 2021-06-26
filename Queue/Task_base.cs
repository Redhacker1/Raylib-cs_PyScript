using System;

namespace RaylibTest.Queue
{
    class Task_base
    {
        public enum Priority_enum
        {
            Very_High = 1,
            High = 2,
            Normal = 3,
            Low = 4,
            Lowest = 5
        }

        public dynamic Arguments = new dynamic[] { };
        public bool Permanent = false;
        public Priority_enum priority = Priority_enum.Normal;
        public string TaskType;

        public virtual dynamic Run_Task()
        {
            throw new NotImplementedException();
        }
    }
}