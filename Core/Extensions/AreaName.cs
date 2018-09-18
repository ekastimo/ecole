using System;

namespace Core.Extensions
{
    [AttributeUsage(AttributeTargets.Class |AttributeTargets.Struct)]
    public class AreaName : Attribute
    {
        private readonly string _name;
        public double Version;

        public AreaName(string name)
        {
            _name = name;
            Version = 1.0;
        }

        public string GetName()
        {
            return _name;
        }
    }
}
