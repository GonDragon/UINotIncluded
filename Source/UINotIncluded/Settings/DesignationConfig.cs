using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UINotIncluded
{
    public enum DesignationConfig : byte
    {
        hidden,
        left,
        main,
        right
    }

    public static class DesignationConfigExtension
    {
        public static String ToStringHuman(this DesignationConfig designation)
        {
            switch (designation)
            {
                case DesignationConfig.hidden:
                    return "Hidden";
                case DesignationConfig.left:
                    return "Left Side";
                case DesignationConfig.main:
                    return "Main";
                case DesignationConfig.right:
                    return "Right Side";

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
