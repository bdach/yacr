using System;

namespace CryptoPals.Runnables
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RunnableAttribute : Attribute
    {
        public int Id { get; }

        public RunnableAttribute(int id)
        {
            Id = id;
        }

        public override bool Match(object? obj)
        {
            if (obj is int id)
                return Id == id;

            return base.Match(obj);
        }
    }
}