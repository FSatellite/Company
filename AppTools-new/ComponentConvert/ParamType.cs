using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentConvert
{
    public class ParamType
    {
        public static string TypeConvert(string typeStr)
        {
            string type = "";
            if (typeStr.Contains("double"))
                type = "Real";
            if (typeStr.Contains("int"))
                type = "Integer";
            if (typeStr.Contains("string"))
                type = "String";
            if (typeStr.Contains("bool"))
                type = "Boolean";
            if (typeStr.Contains("Enumeration"))
                type = "Enumeration";

            return type;
        }
    }
}
