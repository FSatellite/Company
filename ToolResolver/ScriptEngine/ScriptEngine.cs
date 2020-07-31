using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OptInterface;
using IronPython;
using Microsoft.Scripting.Hosting.Providers;
using Microsoft.Scripting.Hosting;
using System.IO;
using System.Xml;

namespace ScriptEngine
{
    /// <summary>
    /// 表达式执行引擎类
    /// </summary>
    public partial class ScriptEngine : UserControl, OptInterface.IExpressEngine
    {
        public ScriptEngine()
        {
            InitializeComponent();
            engine = IronPython.Hosting.Python.CreateEngine();
            scriptFile = "";
           // this.textEditorControl1 .ActiveTextAreaControl .MouseClick +=new MouseEventHandler(ActiveTextAreaControl_MouseClick);
        }
        #region
        private Microsoft.Scripting.Hosting.ScriptEngine engine;
        private ScriptScope ssb;// 

        private OptInterface.IWrapComponent wrapComponentIns;

        private DevComponents.AdvTree.Node inputParasNode = new DevComponents.AdvTree.Node("输入参数");
        private DevComponents.AdvTree.Node outputParasNode = new DevComponents.AdvTree.Node("输出参数");
        private string scriptFile;
        private string baseCode = "#Python \r\nimport sys\r\nimport math\r\nsys.path.append(\"C:\\Python27\\Lib\")\r\nimport os\r" +
                "\nsys.setdefaultencoding(\'utf-8\')\r\n\r\n# please write you code from here ";

        #endregion
        /// <summary>
        /// 设置组件实例给引擎
        /// </summary>
        /// <param name="wrapComponent">组件实例对象</param>
        /// <param name="file">脚本文件</param>
        public void SetWrapComponent(IWrapComponent wrapComponent, string file)
        {
            wrapComponentIns = wrapComponent;
            scriptFile = file;
            if (wrapComponentIns != null)//根据组件对象，对变量进行处理，识别变量的表达式集合
            {
                ReadFile(scriptFile);
                this.itemsTreeModel.Nodes.Clear();
                this.inputParasNode.Nodes.Clear();
                this.outputParasNode.Nodes.Clear();
                ///
                this.itemsTreeModel.Nodes.Add(inputParasNode);
                this.itemsTreeModel.Nodes.Add(outputParasNode);
                //
                foreach (IParameter para in this.wrapComponentIns.GetInputParas())
                {
                    DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                    node.Text = para.name;
                    node.Name = para.name;
                    node.Tag = para;
                    node.ImageIndex = 0;
                    node.Cells.Add(new DevComponents.AdvTree.Cell(para.dataType));//数据类型
                    string datatype = para.dataType;
                    Knowing.UserMath knowing = new Knowing.UserMath();
                    if (para.currentValue == null)
                    {
                        para.currentValue = 0;
                    }
                    switch (datatype)
                    {
                        case "double":
                            string s_temp;
                            knowing.DataTranslate(para.currentValue, out s_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(s_temp));//当前值
                            break;
                        case "int":
                            string i_temp;
                            knowing.DataTranslate(para.currentValue, out i_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(i_temp));//当前值
                            break;
                        case "int[]":
                            string ia_temp;
                            knowing.ArrayDescrib(para.currentValue, out ia_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(ia_temp));//当前值
                            break;
                        case "int[][]":
                            string ia2_temp;
                            knowing.ArrayDescrib(para.currentValue, out ia2_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(ia2_temp));//当前值
                            break;
                        case "double[]":
                            string da_temp;
                            knowing.ArrayDescrib(para.currentValue, out da_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(da_temp));//当前值
                            break;
                        case "double[][]":
                            string da2_temp;
                            knowing.ArrayDescrib(para.currentValue, out da2_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(da2_temp));//当前值
                            break;
                        case "string[]":
                            string sa_temp;
                            knowing.ArrayDescrib(para.currentValue, out sa_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(sa_temp));//当前值
                            break;
                        case "string[][]":
                            string sa2_temp;
                            knowing.ArrayDescrib(para.currentValue, out sa2_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(sa2_temp));//当前值
                            break;
                        default:
                            node.Cells.Add(new DevComponents.AdvTree.Cell(para.currentValue.ToString()));//当前值
                            break;
                    }

                    if (para.expression != null)//表达式内容
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell(para.expression.script));
                    }
                    else
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell(""));//
                    }
                    node.Cells.Add(new DevComponents.AdvTree.Cell(para.description));//参数描述信息
                    if (para.expression != null)
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell());//表达式是否生效
                        node.Cells[5].CheckBoxVisible = true;
                        node.Cells[5].Checked = para.expression.actived;

                    }
                    else
                    {

                        node.Cells.Add(new DevComponents.AdvTree.Cell());//表达式是否生效
                        node.Cells[5].CheckBoxVisible = true;
                    }
                    inputParasNode.Nodes.Add(node);
                    inputParasNode.ExpandAll();
                }

                foreach (IParameter para in this.wrapComponentIns.GetOutputParas())
                {
                    DevComponents.AdvTree.Node node = new DevComponents.AdvTree.Node();
                    node.Text = para.name;
                    node.Name = para.name;
                    node.Tag = para;
                    node.ImageIndex = 1;
                    node.Cells.Add(new DevComponents.AdvTree.Cell(para.dataType));//数据类型
                    string datatype = para.dataType;
                    Knowing.UserMath knowing = new Knowing.UserMath();
                    if (para.currentValue == null)
                    {
                        para.currentValue = 0;
                    }
                    switch (datatype)
                    {
                        case "double":
                            string s_temp;
                            knowing.DataTranslate(para.currentValue, out s_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(s_temp));//当前值
                            break;
                        case "int":
                            string i_temp;
                            knowing.DataTranslate(para.currentValue, out i_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(i_temp));//当前值
                            break;
                        case "int[]":
                            string ia_temp;
                            knowing.ArrayDescrib(para.currentValue, out ia_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(ia_temp));//当前值
                            break;
                        case "int[][]":
                            string ia2_temp;
                            knowing.ArrayDescrib(para.currentValue, out ia2_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(ia2_temp));//当前值
                            break;
                        case "double[]":
                            string da_temp;
                            knowing.ArrayDescrib(para.currentValue, out da_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(da_temp));//当前值
                            break;
                        case "double[][]":
                            string da2_temp;
                            knowing.ArrayDescrib(para.currentValue, out da2_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(da2_temp));//当前值
                            break;
                        case "string[]":
                            string sa_temp;
                            knowing.ArrayDescrib(para.currentValue, out sa_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(sa_temp));//当前值
                            break;
                        case "string[][]":
                            string sa2_temp;
                            knowing.ArrayDescrib(para.currentValue, out sa2_temp);
                            node.Cells.Add(new DevComponents.AdvTree.Cell(sa2_temp));//当前值
                            break;
                        default:
                            node.Cells.Add(new DevComponents.AdvTree.Cell(para.currentValue.ToString()));//当前值
                            break;
                    }
                    if (para.expression != null)//表达式内容
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell(para.expression.script));
                    }
                    else
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell(""));//
                    }
                    node.Cells.Add(new DevComponents.AdvTree.Cell(para.description));//参数描述信息
                    if (para.expression != null)
                    {
                        node.Cells.Add(new DevComponents.AdvTree.Cell());//表达式是否生效
                        node.Cells[5].CheckBoxVisible = true;
                        node.Cells[5].Checked = para.expression.actived;

                    }
                    else
                    {

                        node.Cells.Add(new DevComponents.AdvTree.Cell());//表达式是否生效
                        node.Cells[5].CheckBoxVisible = true;
                    }

                    this.outputParasNode.Nodes.Add(node);
                    outputParasNode.ExpandAll();
                }
                ///
                inputParasNode.Visible = inputCheckBox.Checked;
                outputParasNode.Visible = outPutCheckBox.Checked;
                ///

            }
        }
        /// <summary>
        /// 运行表达式，返回执行结果
        /// </summary>
        /// <param name="expression">表达式对象</param>
        /// <param name="result">返回结果</param>
        /// <returns>是否成功</returns>
        public bool RunExpress(IParameter para, out string result)
        {
            result = "";
            if (para.expression == null)
            {
                
                return false;
            }
            SetParaMap();//映射变量参数给脚本引擎

            IExpression expression = para.expression;
            ////////////////////////////////////////////////////
            string content = baseCode + "\r\n" + expression.script;
            ScriptSource ssc = engine.CreateScriptSourceFromString(content);
            try
            {

                dynamic runResult = engine.Execute(content, ssb);

                if (runResult != null)
                {
                    result = runResult().ToString();
                    messageBox.Text = result.ToString();
                }
                dynamic value = ssb.GetVariable<Object>(expression.bandingParaName);
                if (value != null)
                {

                    
                }

                RefreshParaTree();
               
                return true;
            }
            catch (Exception ee)
            {
                result = "运行错误->" + ee.ToString();
                messageBox.Text = result;
                return false;
            }


            return true;
        }
        /// <summary>
        /// 运行组件的所有表达式
        /// </summary>
        /// <param name="result">返回结果</param>
        /// <param name="inputOrOutputParas">执行输入参数还是输出参数的表达式计算</param>
        /// <returns>返回结果是否成功</returns>
        public bool RunAllExpress(out string result, string inputOrOutputParas)
        {
            result = "";
            if (inputOrOutputParas.Equals("input"))
            {
                string expression = "";
                if (this.wrapComponentIns != null)
                {
                    foreach (IParameter para in this.wrapComponentIns.GetInputParas())
                    {
                        if (para.expression != null)
                        {
                            if (string.IsNullOrEmpty(para.expression.script) == false)
                            {
                                expression += para.expression.script + "\r\n";
                            }
                        }
                    }
                }
                /////

                SetParaMap();//映射变量参数给脚本引擎
                ScriptSource ssc = engine.CreateScriptSourceFromString(expression);
                try
                {
                    result = ssc.Execute(ssb);

                    RefreshParaTree();
                }
                catch (Exception ee)
                {
                    result = ee.ToString();
                    return false;
                }

            }
            else if (inputOrOutputParas.Equals("output"))
            {
                string expression = "";
                if (this.wrapComponentIns != null)
                {
                    foreach (IParameter para in this.wrapComponentIns.GetOutputParas())
                    {
                        if (para.expression != null)
                        {
                            if (string.IsNullOrEmpty(para.expression.script) == false)
                            {
                                expression += para.expression.script + "\r\n";
                            }
                        }
                    }
                }
                /////

                SetParaMap();//映射变量参数给脚本引擎
                ScriptSource ssc = engine.CreateScriptSourceFromString(expression);
                try
                {
                    result = ssc.Execute(ssb);

                    RefreshParaTree();
                }
                catch (Exception ee)
                {
                    result = ee.ToString();
                    return false;
                }

            }

            return true;
        }
        /// <summary>
        /// 将表达式内容输出到记录文件
        /// </summary>
        /// <param name="file">存储文件</param>
        /// <returns></returns>
        public bool WriteFile(string file)
        {
            if (this.wrapComponentIns == null)
            {
                return false;
            }

            //describFile = this.wrapPath + @"\" + this.wrapID + ".mi";

            ///XML文件创建
            XmlDocument xml_doc = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xml_doc.CreateXmlDeclaration("1.0", "utf-8", null);
            xml_doc.AppendChild(xmldecl);

            XmlElement root = xml_doc.CreateElement("Expressions");
            xml_doc.AppendChild(root);
            foreach (IParameter para in this.wrapComponentIns.GetInputParas())
            {
                if (para.expression != null)
                {
                    if (string.IsNullOrEmpty(para.expression.script) == false)
                    {
                        try
                        {
                            XmlElement expressionNode = xml_doc.CreateElement("expression");
                            XmlAttribute paraNameAttrib = xml_doc.CreateAttribute("paraName");
                            paraNameAttrib.Value = para.name;
                            XmlAttribute scriptAttrib = xml_doc.CreateAttribute("script");
                            scriptAttrib.Value = para.expression.script;
                            XmlAttribute activeAttrib = xml_doc.CreateAttribute("actived");
                            activeAttrib.Value = para.expression.actived.ToString();
                            expressionNode.Attributes.Append(paraNameAttrib);
                            expressionNode.Attributes.Append(scriptAttrib);
                            expressionNode.Attributes.Append(activeAttrib);
                            root.AppendChild(expressionNode);
                        }
                        catch (Exception ee)
                        {

                        }



                    }
                }
            }

            foreach (IParameter para in this.wrapComponentIns.GetOutputParas())
            {
                if (para.expression != null)
                {
                    if (string.IsNullOrEmpty(para.expression.script) == false)
                    {
                        try
                        {
                            XmlElement expressionNode = xml_doc.CreateElement("expression");
                            XmlAttribute paraNameAttrib = xml_doc.CreateAttribute("paraName");
                            paraNameAttrib.Value = para.name;
                            XmlAttribute scriptAttrib = xml_doc.CreateAttribute("script");
                            scriptAttrib.Value = para.expression.script;
                            XmlAttribute activeAttrib = xml_doc.CreateAttribute("actived");
                            activeAttrib.Value = para.expression.actived.ToString();
                            expressionNode.Attributes.Append(paraNameAttrib);
                            expressionNode.Attributes.Append(scriptAttrib);
                            expressionNode.Attributes.Append(activeAttrib);
                            root.AppendChild(expressionNode);
                        }
                        catch (Exception ee)
                        {

                        }



                    }
                }
            }
            xml_doc.Save(file);

            return true;
        }
        /// <summary>
        /// 读取组件存储的表达式文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool ReadFile(string file)
        {
            if (File.Exists(file))
            {
                if (this.wrapComponentIns == null)
                {
                    return false;
                }
                ////////////////////
                ///定义XML文档并以UTF8编码格式加载文档
                XmlDocument xmlfile = new XmlDocument();
                using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                {
                    xmlfile.Load(sr);
                }
                xmlfile.Normalize();

                ///读取XML文档的根目录(ModelDescription)
                XmlElement rootEle = xmlfile.DocumentElement;
                foreach (XmlElement node in rootEle.SelectNodes("expression"))
                {
                    string paraName = node.Attributes["paraName"].Value.ToString();
                    string script = node.Attributes["script"].Value.ToString();
                    string actived = node.Attributes["actived"].Value.ToString();
                    ///匹配输出参数
                    IParameter para = this.wrapComponentIns.GetOutputParas().Find(x => x.name.Equals(paraName));
                    if (para != null)
                    {
                        para.expression = new Expression();
                        para.expression.bandingParaName = paraName;
                        para.expression.script = script;
                        if (actived.Equals("true"))
                        {
                            para.expression.actived = true;
                        }
                        else
                        {
                            para.expression.actived = false;
                        }

                    }
                    else
                    {
                        para = this.wrapComponentIns.GetInputParas().Find(x => x.name.Equals(paraName));
                        if (para != null)
                        {
                            para.expression = new Expression();
                            para.expression.bandingParaName = paraName;
                            para.expression.script = script;
                            if (actived.Equals("true"))
                            {
                                para.expression.actived = true;
                            }
                            else
                            {
                                para.expression.actived = false;
                            }

                        }
                    }

                }
            }
            return true;
        }
        /// <summary>
        /// 显示表达式编辑界面
        /// </summary>
        public void ShowExpressDialog()
        {
            Form form = new Form();
            form.Controls.Add(this);
            form.Text = "组件变量表达式编辑器";
            this.Dock = DockStyle.Fill;
            form.Width = 850;
            form.Height = 500;

            form.ShowDialog();
        }
        /// <summary>
        /// 将组件的变量全部映射给脚本引擎
        /// </summary>
        private void SetParaMap()
        {
            ssb = engine.CreateScope();

            foreach (IParameter para in this.wrapComponentIns.GetOutputParas())
            {

                ssb.SetVariable(para.name, para.currentValue);

            }
            foreach (IParameter para in this.wrapComponentIns.GetInputParas())
            {
                ssb.SetVariable(para.name, para.currentValue);

            }

        }
        /// <summary>
        /// 根据脚本计算结果，获取脚本变量值返回给参数变量，刷新变量树列表
        /// </summary>
        private void RefreshParaTree()
        {
            foreach (DevComponents.AdvTree.Node node in this.inputParasNode.Nodes)
            {
                if (node.Tag != null)
                {
                    try
                    {
                        IParameter para = (IParameter)node.Tag;
                        string temp;
                        Knowing.UserMath knowing = new Knowing.UserMath();
                        knowing.ArrayDescrib(para.currentValue, out temp);
                        node.Cells[2].Text = temp;
                        if (para.expression != null)
                        {
                            node.Cells[3].Text = para.expression.script;
                            if (ssb.ContainsVariable(para.name))
                            {
                                dynamic value = ssb.GetVariable<Object>(para.name);
                                knowing.ArrayDescrib(para.currentValue, out temp);
                                node.Cells[2].Text = temp;
                                switch (para.dataType)
                                {
                                    case "int":
                                        int i_value;
                                        knowing.DataTranslate(value, out i_value);
                                        para.currentValue = i_value;
                                        node.Cells[2].Text = para.currentValue.ToString();
                                        break;
                                    case "double":
                                        double d_value;
                                        knowing.DataTranslate(value, out d_value);
                                        para.currentValue = d_value;
                                        node.Cells[2].Text = para.currentValue.ToString();
                                        break;
                                    case "double[]":
                                        double[] da_value;
                                        TranslateListToArray(para, value,out da_value );////////转换list变量
                                        if (da_value != null)
                                        {
                                            para.currentValue = da_value;
                                            string da_string;
                                            knowing.ArrayDescrib(da_value, out da_string);
                                            node.Cells[2].Text = da_string;
                                        }
                                       
                                        break;
                                    case "int[]":
                                        int[] ia_value;
                                        TranslateListToArray(para, value,out ia_value );////////转换list变量
                                        if (ia_value != null)
                                        {
                                            para.currentValue = ia_value;
                                            string ia_string;
                                            knowing.ArrayDescrib(ia_value, out ia_string);
                                            node.Cells[2].Text = ia_string;
                                        }
                                        break;
                                    case "int[][]":
                                        int[][] ia2_value;
                                         TranslateListToArray(para, value,out ia2_value );////////转换list变量
                                         if (ia2_value != null)
                                         {
                                             para.currentValue = ia2_value;
                                             string ia2_string;
                                             knowing.ArrayDescrib(ia2_value, out ia2_string);
                                             node.Cells[2].Text = ia2_string;
                                         }
                                        break;
                                    case "double[][]":
                                        double[][] da2_value;
                                       
                                        TranslateListToArray(para, value,out da2_value );////////转换list变量
                                        if (da2_value != null)
                                        {
                                            para.currentValue = da2_value;
                                            string da2_string;
                                            knowing.ArrayDescrib(da2_value, out da2_string);
                                            node.Cells[2].Text = da2_string;
                                        }
                                        break;
                                    case "string[][]":
                                        string[][] sa2_value;
                                        TranslateListToArray(para, value,out sa2_value );////////转换list变量
                                        if (sa2_value != null)
                                        {
                                            para.currentValue = sa2_value;
                                            string sa2_string;
                                            knowing.ArrayDescrib(sa2_value, out sa2_string);
                                            node.Cells[2].Text = sa2_string;
                                        }
                                        break;
                                    case "string[]":
                                        string[] sa_value;
                                        TranslateListToArray(para, value,out sa_value );////////转换list变量
                                        if (sa_value != null)
                                        {
                                            para.currentValue = sa_value;
                                            string sa_string;
                                            knowing.ArrayDescrib(sa_value, out sa_string);
                                            node.Cells[2].Text = sa_string;
                                        }
                                        break;

                                    default:
                                        para.currentValue = value;
                                        node.Cells[2].Text = para.currentValue.ToString();
                                        break;
                                }


                                node.Cells[5].Checked = para.expression.actived;
                            }
                        }

                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            foreach (DevComponents.AdvTree.Node node in this.outputParasNode.Nodes)
            {
                if (node.Tag != null)
                {
                    try
                    {
                        IParameter para = (IParameter)node.Tag;
                        string temp;
                        Knowing.UserMath knowing = new Knowing.UserMath();
                        knowing.ArrayDescrib(para.currentValue, out temp);
                        node.Cells[2].Text = temp;
                        if (para.expression != null)
                        {
                            node.Cells[3].Text = para.expression.script;
                            if (ssb.ContainsVariable(para.name))
                            {
                                dynamic value = ssb.GetVariable<Object>(para.name);
                                knowing.ArrayDescrib(para.currentValue, out temp);
                                node.Cells[2].Text = temp;
                                switch (para.dataType)
                                {
                                    case "int":
                                        int i_value;
                                        knowing.DataTranslate(value, out i_value);
                                        para.currentValue = i_value;
                                        node.Cells[2].Text = para.currentValue.ToString();
                                        break;
                                    case "double":
                                        double d_value;
                                        knowing.DataTranslate(value, out d_value);
                                        para.currentValue = d_value;
                                        node.Cells[2].Text = para.currentValue.ToString();
                                        break;
                                    case "double[]":
                                        double[] da_value;
                                        TranslateListToArray(para, value, out da_value);////////转换list变量
                                        if (da_value != null)
                                        {
                                            para.currentValue = da_value;
                                            string da_string;
                                            knowing.ArrayDescrib(da_value, out da_string);
                                            node.Cells[2].Text = da_string;
                                        }

                                        break;
                                    case "int[]":
                                        int[] ia_value;
                                        TranslateListToArray(para, value, out ia_value);////////转换list变量
                                        if (ia_value != null)
                                        {
                                            para.currentValue = ia_value;
                                            string ia_string;
                                            knowing.ArrayDescrib(ia_value, out ia_string);
                                            node.Cells[2].Text = ia_string;
                                        }
                                        break;
                                    case "int[][]":
                                        int[][] ia2_value;
                                        TranslateListToArray(para, value, out ia2_value);////////转换list变量
                                        if (ia2_value != null)
                                        {
                                            para.currentValue = ia2_value;
                                            string ia2_string;
                                            knowing.ArrayDescrib(ia2_value, out ia2_string);
                                            node.Cells[2].Text = ia2_string;
                                        }
                                        break;
                                    case "double[][]":
                                        double[][] da2_value;

                                        TranslateListToArray(para, value, out da2_value);////////转换list变量
                                        if (da2_value != null)
                                        {
                                            para.currentValue = da2_value;
                                            string da2_string;
                                            knowing.ArrayDescrib(da2_value, out da2_string);
                                            node.Cells[2].Text = da2_string;
                                        }
                                        break;
                                    case "string[][]":
                                        string[][] sa2_value;
                                        TranslateListToArray(para, value, out sa2_value);////////转换list变量
                                        if (sa2_value != null)
                                        {
                                            para.currentValue = sa2_value;
                                            string sa2_string;
                                            knowing.ArrayDescrib(sa2_value, out sa2_string);
                                            node.Cells[2].Text = sa2_string;
                                        }
                                        break;
                                    case "string[]":
                                        string[] sa_value;
                                        TranslateListToArray(para, value, out sa_value);////////转换list变量
                                        if (sa_value != null)
                                        {
                                            para.currentValue = sa_value;
                                            string sa_string;
                                            knowing.ArrayDescrib(sa_value, out sa_string);
                                            node.Cells[2].Text = sa_string;
                                        }
                                        break;

                                    default:
                                        para.currentValue = value;
                                        node.Cells[2].Text = para.currentValue.ToString();
                                        break;
                                }


                                node.Cells[5].Checked = para.expression.actived;
                            }
                        }

                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }
        /// <summary>
        /// 应用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Applybutton_Click(object sender, EventArgs e)
        {
            if (this.itemsTreeModel.SelectedNode != null)
            {
                if (this.itemsTreeModel.SelectedNode.Tag != null)
                {
                    IParameter para = (IParameter)this.itemsTreeModel.SelectedNode.Tag;
                    if (para.expression == null)
                    {
                        para.expression = new Expression();
                        para.expression.bandingParaName = para.name;
                    }
                    para.expression.script = this.textEditorControl1.Text;
                    string result = "";

                    bool r = RunExpress(para, out result);
                    WriteFile(scriptFile);
                    if (r == false)
                    {

                    }
                }
            }
        }

        private void inputCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.inputParasNode.Visible = inputCheckBox.Checked;

        }

        private void outPutCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.outputParasNode.Visible = this.outPutCheckBox.Checked;
        }
        /// <summary>
        /// 鼠标选择变量列表对象事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemsTreeModel_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Tag != null)
                {
                    IParameter para = (IParameter)e.Node.Tag;
                    if (para.expression != null)
                    {
                        this.textEditorControl1.Text = para.expression.script;
                    }
                    else
                    {
                        this.textEditorControl1.Text = "";
                    }
                }
            }
            else
            {
                this.textEditorControl1.Text = "";

            }
            this.textEditorControl1.Refresh();
        }

        private void Clearbutton_Click(object sender, EventArgs e)
        {
            this.textEditorControl1.Text = "";

        }

        private void label2_DoubleClick(object sender, EventArgs e)
        {

        }

        private string TranslateListToArray(IParameter para, dynamic value, out double[] d_array)
        {
            string temp="";
            d_array = null;
            Knowing.UserMath knowing = new Knowing.UserMath();
            IronPython.Runtime.List list;
            try
            {
                list = value;

            }
            catch (Exception e)
            {
                return "";
            }
            switch (para.dataType)
            {
                case "double[]":
                   
                    object[] valueT = list.ToArray();
                    d_array = new double[valueT.Length];
                    for (int i = 0; i < valueT.Length; i++)
                    {
                        d_array[i] = Convert.ToDouble(valueT[i]);
                    }
                    knowing.ArrayDescrib(d_array, out temp);
                    break;
                case "string[]":
                    object[] s_valueT = list.ToArray();
                    string[] s_array = new string[s_valueT.Length];
                    for (int i = 0; i < s_valueT.Length; i++)
                    {
                        s_array[i] = Convert.ToString (s_valueT[i]);
                    }
                    knowing.ArrayDescrib(s_array, out temp);
                    break;
                case "int[]":
                    object[] i_valueT = list.ToArray();
                    int[] i_array = new int[i_valueT.Length];
                    for (int i = 0; i < i_valueT.Length; i++)
                    {
                        i_array[i] = Convert.ToInt32 (i_valueT[i]);
                    }
                    knowing.ArrayDescrib(i_array, out temp);
                    break;
            }
            return temp;
        }
        private string TranslateListToArray(IParameter para, dynamic value, out int[] i_array)
        {
            string temp = "";
            i_array = null;
            Knowing.UserMath knowing = new Knowing.UserMath();
            IronPython.Runtime.List list;
            try
            {
                list = value;

            }
            catch (Exception e)
            {
                return "";
            }
            switch (para.dataType)
            {
                
                case "string[]":
                    object[] s_valueT = list.ToArray();
                    string[] s_array = new string[s_valueT.Length];
                    for (int i = 0; i < s_valueT.Length; i++)
                    {
                        s_array[i] = Convert.ToString(s_valueT[i]);
                    }
                    knowing.ArrayDescrib(s_array, out temp);
                    break;
                case "int[]":
                    object[] i_valueT = list.ToArray();
                     i_array = new int[i_valueT.Length];
                    for (int i = 0; i < i_valueT.Length; i++)
                    {
                        i_array[i] = Convert.ToInt32(i_valueT[i]);
                    }
                    knowing.ArrayDescrib(i_array, out temp);
                    break;
            }
            return temp;
        }
        private string TranslateListToArray(IParameter para, dynamic value, out string[] s_array)
        {
            string temp = "";
            s_array = null;
            Knowing.UserMath knowing = new Knowing.UserMath();
            IronPython.Runtime.List list;
            try
            {
                list = value;

            }
            catch (Exception e)
            {
                return "";
            }
            switch (para.dataType)
            {

                case "string[]":
                    object[] s_valueT = list.ToArray();
                     s_array = new string[s_valueT.Length];
                    for (int i = 0; i < s_valueT.Length; i++)
                    {
                        s_array[i] = Convert.ToString(s_valueT[i]);
                    }
                    knowing.ArrayDescrib(s_array, out temp);
                    break;
               
            }
            return temp;
        }

        private string TranslateListToArray(IParameter para, dynamic value, out double[][] d2_array)
        {
            string temp = "";
            d2_array = null;
            Knowing.UserMath knowing = new Knowing.UserMath();
            IronPython.Runtime.List list;
            try
            {
                 list = value;

            }
            catch (Exception e)
            {
                return "";
            }
            switch (para.dataType)
            {
                case "double[][]":

                    object[] valueT = list.ToArray();
                    d2_array = new double[valueT.Length][];

                    for (int i = 0; i < valueT.Length; i++)
                    {
                        IronPython.Runtime.List sub_list =(IronPython.Runtime.List) valueT[i];
                        object[] sub_obj = sub_list.ToArray();
                         d2_array[i]=new double [sub_obj .Length ];
                        for (int j = 0; j < sub_obj.Length; j++)
                        {
                            d2_array[i][j] = Convert.ToDouble(sub_obj[j]);
                        }
                        

                    }
                    knowing.ArrayDescrib(d2_array, out temp);
                    break;
              
            }
            return temp;
        }
        private string TranslateListToArray(IParameter para, dynamic value, out int[][] i2_array)
        {
            string temp = "";
           i2_array = null;
            Knowing.UserMath knowing = new Knowing.UserMath();
            IronPython.Runtime.List list;
            try
            {
                list = value;

            }
            catch (Exception e)
            {
                return "";
            }
            switch (para.dataType)
            {
                case "int[][]":

                    object[] valueT = list.ToArray();
                    i2_array = new int[valueT.Length][];

                    for (int i = 0; i < valueT.Length; i++)
                    {
                        IronPython.Runtime.List sub_list = (IronPython.Runtime.List)valueT[i];
                        object[] sub_obj = sub_list.ToArray();
                        i2_array[i] = new int[sub_obj.Length];
                        for (int j = 0; j < sub_obj.Length; j++)
                        {
                            i2_array[i][j] = Convert.ToInt32 (sub_obj[j]);
                        }


                    }
                    knowing.ArrayDescrib(i2_array, out temp);
                    break;

            }
            return temp;
        }
        private string TranslateListToArray(IParameter para, dynamic value, out string[][] s2_array)
        {
            string temp = "";
            s2_array = null;
            Knowing.UserMath knowing = new Knowing.UserMath();
            IronPython.Runtime.List list;
            try
            {
                list = value;

            }
            catch (Exception e)
            {
                return "";
            }
            switch (para.dataType)
            {
                case "string[][]":

                    object[] valueT = list.ToArray();
                    s2_array = new string[valueT.Length][];

                    for (int i = 0; i < valueT.Length; i++)
                    {
                        IronPython.Runtime.List sub_list = (IronPython.Runtime.List)valueT[i];
                        object[] sub_obj = sub_list.ToArray();
                        s2_array[i] = new string [sub_obj.Length];
                        for (int j = 0; j < sub_obj.Length; j++)
                        {
                            s2_array[i][j] = Convert.ToString (sub_obj[j]);
                        }


                    }
                    knowing.ArrayDescrib(s2_array, out temp);
                    break;

            }
            return temp;
        }

        private void MaxbuttonItem_Click(object sender, EventArgs e)
        {
            this.textEditorControl1.ActiveTextAreaControl.TextArea.InsertString("max()");

        }

        private void MinbuttonItem_Click(object sender, EventArgs e)
        {
            this.textEditorControl1.ActiveTextAreaControl.TextArea.InsertString("min()");
        }

        private void SumbuttonItem_Click(object sender, EventArgs e)
        {
            this.textEditorControl1.ActiveTextAreaControl.TextArea.InsertString("sum()");
        }
        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveTextAreaControl_MouseClick(object sender, MouseEventArgs e)
        {
           
        }
        /// <summary>
        /// 插入变量名称到编辑代码区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParaHelpItem_Click(object sender,EventArgs  e)
        {
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                this.textEditorControl1.ActiveTextAreaControl.TextArea.InsertString(item.Text);
            }
            catch (Exception ee)
            {

            }
        }

        private void textEditorControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void textEditorControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ////

            }
        }

        private void ParasbuttonItem_Click(object sender, EventArgs e)
        {

            ParacontextMenuStrip.Items.Clear();
            foreach (IParameter para in wrapComponentIns.GetInputParas())
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = para.name;
                item.ToolTipText = para.description;
                this.ParacontextMenuStrip.Items.Add(item);
                item.Click += new EventHandler(ParaHelpItem_Click);
            }
            foreach (IParameter para in wrapComponentIns.GetOutputParas())
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = para.name;
                item.ToolTipText = para.description;
                this.ParacontextMenuStrip.Items.Add(item);
                item.Click += new EventHandler(ParaHelpItem_Click);
            }
            this.ParacontextMenuStrip.Show(MousePosition);
        }

      
    }
}
