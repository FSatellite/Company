using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StandardModel
{
    class Program
    {
        class ParamIn
        {
            public double m;
            public double g;
            public double density;
            public double V;
            public double l;
            public double CL;
            public double rtRatio;
            public List<string> ma;//马赫数
            public List<List<string>> aerodynamic;//气动参数
            public ParamIn(double _m, double _g, double _density, double _V, double _l, double _CL, double _rtRatio,List<string> _ma,List<List<string>> _aerodynamic)
            {
                this.m = _m;
                this.g = _g;
                this.density = _density;
                this.V = _V;
                this.l = _l;
                this.CL = _CL;
                this.rtRatio = _rtRatio;
                this.ma = _ma;
                this.aerodynamic = _aerodynamic;
            }

            public object this[string index]
            {
                get
                {
                    if (index == "m")
                        return m;
                    if (index == "g")
                        return g;
                    if (index == "density")
                        return density;
                    if (index == "V")
                        return V;
                    if (index == "l")
                        return l;
                    if (index == "CL")
                        return CL;
                    if (index == "rtRatio")
                        return rtRatio;
                    return null;
                }
                set
                {
                    if (index == "m")
                        m = Convert.ToDouble(value);
                    if (index == "g")
                        g = Convert.ToDouble(value);
                    if (index == "density")
                        density = Convert.ToDouble(value);
                    if (index == "V")
                        V = Convert.ToDouble(value);
                    if (index == "l")
                        l = Convert.ToDouble(value);
                    if (index == "CL")
                        CL = Convert.ToDouble(value);
                    if (index == "rtRatio")
                        rtRatio = Convert.ToDouble(value);
                }
            }
        }
        class ParamOut
        {
            public double S;
            public double aspectRatio;
            public double b0;
            public double b1;
            public ParamOut(double _S, double _aspectRatio, double _b0, double _b1)
            {
                this.S = _S;
                this.aspectRatio = _aspectRatio;
                this.b0 = _b0;
                this.b1 = _b1;
            }
        }

        static void Main(string[] args)
        {
            try
            {
                //获取数据文件所在路径
                string basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Substring(0,AppDomain.CurrentDomain.SetupInformation.ApplicationBase.IndexOf("RunFile"));

                ParamIn paramIn = ReadParamIn(basePath + "Data\\params.in");
                ParamOut paramOut = new ParamOut(0, 0, 0, 0);
                //计算
                paramOut.S = 1e3 * paramIn.m * paramIn.g / (0.5 * paramIn.CL * paramIn.density * Math.Pow(paramIn.V, 2));
                paramOut.aspectRatio = Math.Pow(paramIn.l, 2) / paramOut.S;
                paramOut.b0 = (paramOut.S / paramIn.l) * (2 * paramIn.rtRatio / (1 + paramIn.rtRatio));
                paramOut.b1 = (paramOut.S / paramIn.l) * (2 / (1 + paramIn.rtRatio));

                WriteParamOut(basePath + "Data\\params.out", paramOut);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
                throw;
            }
        }

        static ParamIn ReadParamIn(string filePath)
        {
            try
            {
                ParamIn paramIn = new ParamIn(2.2, 9.8, 1.225, 238, 2.5, 0.8, 3, new List<string>(), new List<List<string>>());
                //读取输入文件中的参数
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line = sr.ReadLine();
                        int nFlag = 0; //标记是否遇到参数块
                        string paramModel = "";
                        while (line != null)
                        {
                            //遇到参数块时，nFlag++
                            if (line == "##rowModel" || line == "##matrixModel" || line == "##structModel")
                            {
                                paramModel = line;
                                nFlag++;
                                line = sr.ReadLine();
                                continue;
                            }

                            if (line == "" || line.IndexOf("//") == 0)
                            {
                                nFlag = 0;
                                paramModel = "";
                                line = sr.ReadLine();
                                continue;
                            }

                            //单变量
                            if (paramModel == "##rowModel")
                            {
                                if (nFlag > 1)
                                {
                                    string[] array = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                                    paramIn[array[0]] = array[2];
                                }
                            }

                            //数组变量
                            if (paramModel == "##matrixModel")
                            {
                                if (nFlag > 2)
                                {
                                    paramIn.ma.Add(line);
                                }
                            }

                            //结构数组
                            if (paramModel == "##structModel")
                            {
                                if (nFlag > 3)
                                {
                                    string[] array = Regex.Split(line, "\\s+", RegexOptions.IgnoreCase);
                                    for (int i = 0; i < array.Length; i++)
                                    {
                                        if (paramIn.aerodynamic.Count == 0)
                                        {
                                            for (int j = 0; j < array.Length; j++)
                                                paramIn.aerodynamic.Add(new List<string>());
                                        }
                                        paramIn.aerodynamic[i].Add(array[i]);
                                    }
                                }
                            }

                            nFlag++;
                            line = sr.ReadLine();
                        }
                    }
                }
                return paramIn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        static void WriteParamOut(string filePath,ParamOut paramOut)
        {
            //将计算结果写到输出参数文件中
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("##rowModel\n" +
                             "#paramName  #description  #value  #type  #unit\n" +
                             "S 翼面积 " + Math.Round(paramOut.S,3) + " double m2\n" +
                             "aspectRatio 展弦比 " + Math.Round(paramOut.aspectRatio,3) + " double /\n" +
                             "b0 翼根弦长 " + Math.Round(paramOut.b0,3) + " double m\n" +
                             "b1 翼尖弦长 " + Math.Round(paramOut.b1,3) + " double m");
                sw.Close();
            }
        }
    }
}
