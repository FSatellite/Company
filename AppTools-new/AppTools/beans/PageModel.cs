using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppTools.beans
{
    class PageModel<T>
    {
        public int count
        {
            get;
            set;
        }
        public int totalPage
        {
            get;
            set;
        }
        public int pageSize
        {
            get;
            set;
        }
        public int pageNum
        {
            get;
            set;
        }
        public List<T> list
        {
            get;
            set;
        }
    }
}
