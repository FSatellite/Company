using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppTools.beans
{
    /// <summary>
    /// 
    /// </summary>
    public class FileItem
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string name
        {
            get;
            set;
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string path
        {
            get;
            set;
        }
        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool isFolder
        {
            get;
            set;
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string type
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string createDate
        {
            get;
            set;
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string updateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long size
        {
            get;
            set;
        }
        /// <summary>
        /// 子文件
        /// </summary>
        public List<FileItem> children
        {
            get;
            set;
        }
    }
}
