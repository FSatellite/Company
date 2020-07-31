using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Reflection;
using AppTools.beans;
using AppTools.util;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AppTools.remote
{
    class UploadFile
    {
        public static MessageState uploadFile(FileItem fileItem,AppInfo appInfo, String url)
        {
            MessageState state = new MessageState();

            string LINEND = "\r\n";
            string MULTIPART_FROM_DATA = "multipart/form-data";
            string boundary = "-----" + DateTime.Now.Ticks.ToString("x");
            byte[] beginBoundary = Encoding.UTF8.GetBytes("--" + boundary + LINEND);
            byte[] endBoundary = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(SecurityValidate);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version11;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.Timeout = 100000;
            request.ContentType = MULTIPART_FROM_DATA + ";boundary=" + boundary + LINEND;
            //   webrequest.SendChunked = true;
            //   webrequest.TransferEncoding = Config.encode.BodyName;


            FileStream fileStream = new FileStream(fileItem.path, FileMode.Open, FileAccess.Read);

            string filePartHeader =
                  "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                  "Content-Type: application/octet-stream;chartset=" + Encoding.UTF8.BodyName + "\r\n\r\n";

            string fileHeader = string.Format(filePartHeader, "file", fileItem.name);
            byte[] fileHeaderBytes = Encoding.UTF8.GetBytes(fileHeader);

            MemoryStream memStream = new MemoryStream();
            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(fileHeaderBytes, 0, fileHeaderBytes.Length);

            var buffer = new byte[1024];
            int bytesRead;//=0
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            // memStream.Write(endBoundary, 0, endBoundary.Length);

            //Key-Value数据
            var stringKeyHeader = "\r\n--" + boundary +
                                  "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                  "\r\nContent-Type: text/plain; charset=" + Encoding.UTF8.BodyName + "\r\n" +
                                  "Content-Transfer-Encoding: 8bit\r\n" +
                                  "\r\n{1}";
            ///file
            PropertyInfo[] propertys = appInfo.GetType().GetProperties();
            foreach (PropertyInfo pinfo in propertys)
            {
                //Console.WriteLine(pinfo.Name + "=" + pinfo.GetValue(appInfo, null));
                var field = string.Format(stringKeyHeader, pinfo.Name, pinfo.GetValue(appInfo, null));

                var fieldBytes = Encoding.UTF8.GetBytes(field);
                //Console.WriteLine(fieldBytes.Length);
                memStream.Write(fieldBytes, 0, fieldBytes.Length);

            }
            ///user
            //propertys = user.GetType().GetProperties();
            //foreach (PropertyInfo pinfo in propertys)
            //{
            //    //Console.WriteLine(pinfo.Name + "=" + pinfo.GetValue(fileInfo, null));

            //    var field = string.Format(stringKeyHeader, pinfo.Name, pinfo.GetValue(user, null));
            //    Console.WriteLine(field);
            //    var fieldBytes = Config.encode.GetBytes(field);
            //    Console.WriteLine(fieldBytes.Length);

            //    memStream.Write(fieldBytes, 0, fieldBytes.Length);

            //}


            memStream.Write(endBoundary, 0, endBoundary.Length);

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            request.ContentLength = tempBuffer.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)request.GetResponse();
            string Content = null;
            using (Stream resStream = httpWebResponse.GetResponseStream())//得到回写的流
            {
                StreamReader newReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8);
                Content = newReader.ReadToEnd();
                newReader.Close();
            }
            //state=JSONUtil.parse<MessageState>(Content);
            state = new MessageState();
            //Console.WriteLine(state.data);
            if (Content.Contains("success"))
            {
                state.state = "success";
                state.msg = "上传成功";
            }
            else
            {
                state.state = "error";
                state.msg = "上传失败";

            }
            httpWebResponse.Close();
            request.Abort();
            Console.WriteLine("上传消息:" + state.msg);

            return state;
        }
        public static bool SecurityValidate(object sender,
                            X509Certificate certificate,
                            X509Chain chain,
                           SslPolicyErrors errors)
        {
            //直接确认，不然打不开，会出现超时错误
            return true;
        }
    }
}
