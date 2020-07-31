using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppTools.beans
{
    /// <summary>
    /// 程序包检查信息
    /// </summary>
    class PackageInfo
    {
        /// <summary>
        /// 包路径
        /// </summary>
        public string path
        {
            get;
            set;
        }
        /// <summary>
        /// 程序包
        /// </summary>
        public List<FileItem> package
        {
            get;
            set;
        }
        /// <summary>
        /// 程序信息
        /// </summary>
        public AppInfo appInfo
        {
            get;
            set;
        }
    }
}
