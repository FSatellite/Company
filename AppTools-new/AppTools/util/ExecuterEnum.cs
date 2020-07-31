using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AppTools.util
{
    class ExecuterEnum
    {
        public static Hashtable table = new Hashtable();

        static ExecuterEnum()
        {
             table.Add("Search", "TSExcuter.Searcher");
             table.Add("Caculate", "TSExcuter.Caculater");
             table.Add("Report", "TSExcuter.Tcharts");
        }
    }
}
