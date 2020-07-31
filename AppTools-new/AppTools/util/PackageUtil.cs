using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AppTools.beans;

namespace AppTools.util
{
    class PackageUtil
    {   
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="aimPath"></param>
        public static void copyDir(string srcPath, string aimPath)
        {
                if (!System.IO.Directory.Exists(aimPath))
                {
                    System.IO.Directory.CreateDirectory(aimPath);
                }
                //令目标路径为aimPath\srcPath
                string srcdir =System.IO.Path.Combine(aimPath, System.IO.Path.GetFileName(srcPath));
                //如果源路径是文件夹，则令目标目录为aimPath\srcPath\
                if (Directory.Exists(srcPath))
                    srcdir += Path.DirectorySeparatorChar;
                // 如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(srcdir))
                {
                    System.IO.Directory.CreateDirectory(srcdir);
                }
                //获取源文件下所有的文件
                String[] files = Directory.GetFileSystemEntries(srcPath);
                foreach (string element in files)
                {
                    //如果是文件夹，循环
                    if (Directory.Exists(element))
                        copyDir(element, srcdir);
                    else
                        File.Copy(element, srcdir + Path.GetFileName(element), true);
                }
        }
        /// <summary>
        /// 查找目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="list"></param>
        public static void findFiles(string path, List<FileItem> list)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(path);
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                FileItem item = new FileItem();
                item.name = NextFolder.Name;
                item.path = NextFolder.FullName;
                item.isFolder = true;
                item.createDate = NextFolder.CreationTime.ToString();
                item.updateDate = NextFolder.LastWriteTime.ToString();
                item.children = new List<FileItem>();
                findFiles(NextFolder.FullName, item.children);
                list.Add(item);
            }
            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                FileItem item = new FileItem();
                item.name = NextFile.Name;
                item.path = NextFile.FullName;
                item.size = NextFile.Length;
                item.isFolder = false;
                item.createDate = NextFile.CreationTime.ToString();
                item.updateDate = NextFile.LastWriteTime.ToString();
                item.type = NextFile.Extension;
                list.Add(item);
            }
        }
    }
}
