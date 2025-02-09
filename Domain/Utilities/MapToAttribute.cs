using System;

namespace Lab1.Domain.Utilities
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MapToAttribute : Attribute
    {
        public string TargetProperty { get; }

        public MapToAttribute(string targetProperty)
        {
            TargetProperty = targetProperty;
        }
    }

}
