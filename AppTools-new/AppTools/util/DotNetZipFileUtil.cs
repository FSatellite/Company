using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace AppTools.util
{
    class DotNetZipFileUtil
    {
        #region Ionic.Zip压缩文件
        ////压缩方法一
        //public static void ExeCompOne(string dirToZip, string zipedFileName)
        //{
        //    string FileName = DateTime.Now.ToString("yyMMddHHmmssff");
        //    //ZipFile实例化一个压缩文件保存路径的一个对象zip
        //    using (ZipFile zip = new ZipFile(@"E:\\yangfeizai\\"+ FileName+".zip",Encoding.Default))
        //    {
        //        //加密压缩
        //        //zip.Password = "123456";
        //        //将要压缩的文件夹添加到zip对象中去(要压缩的文件夹路径和名称)
        //        zip.AddDirectory(@"E:\\yangfeizai\\"+"12051214544443");
        //        //将要压缩的文件添加到zip对象中去,如果文件不存在抛错FileNotFoundExcept
        //        //zip.AddFile(@"E:\\yangfeizai\\12051214544443\\"+"Jayzai.xml");
        //        zip.Save();
        //    }
        //}
         //压缩方法二
        public static void ExeCompTwo(string dirToZip, string zipedFileName)
        {
            if (Path.GetExtension(zipedFileName) != ".zip")
            {
                zipedFileName = zipedFileName + ".zip";
            }
            //ZipFile实例化一个对象zip
            using (ZipFile zip = new ZipFile(System.Text.Encoding.UTF8))
            {
                //加密压缩
                //zip.Password = "123456";
               
                //将要压缩的文件夹添加到zip对象中去(要压缩的文件夹路径和名称)
                zip.AddDirectory(dirToZip);
                //将要压缩的文件添加到zip对象中去,如果文件不存在抛错FileNotFoundExcept
                //zip.AddFile(@"E:\\yangfeizai\\12051214544443\\"+"Jayzai.xml");
                //用zip对象中Save重载方法保存压缩的文件，参数为保存压缩文件的路径
                zip.Save(zipedFileName);
            }
        }
        #endregion
 
        #region //删除压缩包中的文件
        //3.从zip文件中删除一个文件,注意无法直接删除一个文件夹
        public void ExeDelete(string FileName)
        {
            using (ZipFile zip = ZipFile.Read(@"E:\\yangfeizai\\" + FileName + ".zip"))
            {
                //zip["Jayzai.xml"] = null;
                //删除zip对象中的一个文件
                zip.RemoveEntry("Jayzai.xml");
                zip.Save();
            }
        }
        #endregion

        //从zip文件中解压出一个文件
        public void ExeSingleDeComp(string FileName)
        {
            using (ZipFile zip = ZipFile.Read(@"E:\\yangfeizai\\"+FileName+ ".zip"))
            {
                zip.Password = "123456";//密码解压
                //Extract解压zip文件包的方法，参数是保存解压后文件的路基
                zip["Jayzai.xml"].Extract(@"E:\\yangfeizai\\Test");
            }
        }
 
        //从zip文件中解压全部文件
        public static void ExeAllDeComp(string zipFilePath, string unZipDir)
        {
            if (zipFilePath == string.Empty)
            {
                throw new Exception("压缩文件不能为空！");
            }
            if (!File.Exists(zipFilePath))
            {
                throw new FileNotFoundException("压缩文件不存在！");
            }
            //解压文件夹为空时默认与压缩文件同一级目录下，跟压缩文件同名的文件夹  
            if (unZipDir == string.Empty)
                unZipDir = zipFilePath.Replace(Path.GetFileName(zipFilePath), Path.GetFileNameWithoutExtension(zipFilePath));
            if (!unZipDir.EndsWith("/"))
                unZipDir += "/";
            if (!Directory.Exists(unZipDir))
                Directory.CreateDirectory(unZipDir);
            using (ZipFile zip = new ZipFile(zipFilePath, System.Text.Encoding.UTF8))
            {
                zip.ExtractAll(unZipDir, ExtractExistingFileAction.OverwriteSilently);
            }
            //using (ZipFile zip = ZipFile.Read(zipFilePath))
            //{
            //    //zip.Password = "123456";//密码解压
            //    foreach (ZipEntry entry in zip)
            //    {
            //        string name = entry.FileName;
            //        string a = "";
            //        //Extract解压zip文件包的方法，参数是保存解压后文件的路基
            //        entry.Extract(unZipDir);
            //    }
            //}
        }
    }
}
