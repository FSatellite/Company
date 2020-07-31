using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Text.RegularExpressions;

using CreoIntegration;
using FMIModel;
using MessagePrinter;

using ArrayWindows;
using Knowing;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using FileViewTree;

namespace ToolResolver
{
    public partial class Form1 : Office2007Form
    {
        public Form1()
        {
            InitForm();
        }

        public void InitForm()
        {
            this.EnableGlass = false;
            InitializeComponent();
            
            //初始化消息打印窗口
            this.printer = MessagePrinter.MessagePrinter.GetInstance();
            this.splitContainer1.Panel2.Controls.Add(this.printer);
            this.printer.Dock = DockStyle.Fill;

        }

        private void Form1_Shown(Object sender, EventArgs e)
        {
            InitTaskTree();
            if (this.hiddenTask == true)
            {
                this.splitContainer2.Panel1Collapsed = true;
                this.ribbonBar4.Visible = false;
                LoadApp(this.filePath, this.guid);
            }
        }

        /// <summary>
        /// 平台数据、关联关系保存位置，以及所需组件拷贝存放位置
        /// </summary>
        public string filePath;

        /// <summary>
        /// 用户信息
        /// </summary>
        public string token;

        /// <summary>
        /// 是否隐藏任务栏
        /// </summary>
        public bool hiddenTask = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">平台数据保存位置</param>
        /// <param name="token">用户信息</param>
        public Form1(string filePath, string token)
        {
            this.filePath = filePath;
            this.token = token;
            InitForm();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">组件路径，包含guid</param>
        public Form1(string filePath,bool hiddenTask)
        {
            int nSign = filePath.LastIndexOf('\\');
            this.filePath = filePath.Substring(0, nSign + 1);
            this.guid = filePath.Substring(nSign + 1);
            this.hiddenTask = hiddenTask;

            InitForm();
            //if (this.hiddenTask == true)
            //{
            //    this.splitContainer2.Panel1Collapsed = true;
            //    this.ribbonBar4.Visible = false;
            //    LoadApp(this.filePath, this.guid);
            //}

            RunModelbuttonItem_Click(null,null);

            System.Environment.Exit(0);
        }

        /// <summary>
        /// 任务参数模型名称
        /// </summary>
        public string TaskTreeName;

        /// <summary>
        /// 工具参数模型名称
        /// </summary>
        public string ToolTreeName;

        /// <summary>
        /// 当前运行模型类型
        /// </summary>
        public ToolType toolType;

        /// <summary>
        /// 任务参数List
        /// </summary>
        public List<ScalarVariableModel> scalarVariableList = new List<ScalarVariableModel>();

        /// <summary>
        /// 工具参数List
        /// </summary>
        public List<CreoParamModel> paramList = new List<CreoParamModel>();

        /// <summary>
        /// 工具集成object
        /// </summary>
        public ProgramIntegration.ProgramIntegration programIntegration;

        /// <summary>
        /// Creo object
        /// </summary>
        public CreoIntegration.CreoIntegration creo;

        /// <summary>
        /// 当前工具GUID
        /// </summary>
        public String guid;

        /// <summary>
        /// 消息打印
        /// </summary>
        public MessagePrinter.MessagePrinter printer;

        /// <summary>
        /// fmi模型
        /// </summary>
        FMIModel.FMIModel fmiModel;

        private FileViewTree.OptXpertFiles ProjectFilesView = new OptXpertFiles();

        private void InitialProjectFilesView(SuperTabControlPanel panel)
        {
            panel.Controls.Clear();
            panel.Controls.Add(ProjectFilesView);
            ProjectFilesView.Dock = DockStyle.Fill;
            ProjectFilesView.InitialFileData(this.filePath + "\\" + this.guid, "", this.filePath + "\\" + this.guid);
        }

        /// <summary>
        /// 初始化任务数据树
        /// </summary>
        public void InitTaskTree()
        {
            //MessageBox.Show(this.filePath);
            if (!File.Exists(this.filePath + "\\modelDescription.xml"))
                return;
            InitialProjectFilesView(this.superTabControlPanel3);
            fmiModel = new FMIModel.FMIModel(this.filePath + "\\modelDescription.xml");
            //初始化任务树
            this.TaskTreeName = fmiModel.modelName;
            this.scalarVariableList = fmiModel.ScalarVariables;

            this.advTree_Task.Nodes.Clear();

            //添加根节点
            DevComponents.AdvTree.Node rootNode = new DevComponents.AdvTree.Node();
            rootNode.Name = "RootNode";
            rootNode.Text = fmiModel.modelName;
            rootNode.Tooltip = "任务参数";
            rootNode.DataKey = "RootNode";
            rootNode.Image = this.imageList3.Images[0];
            this.advTree_Task.Nodes.Add(rootNode);

            //添加输入输出节点
            DevComponents.AdvTree.Node inputNode = new DevComponents.AdvTree.Node();
            inputNode.Text = "输入参数";
            inputNode.Name = "输入参数";
            inputNode.DataKey = "inputNode";
            inputNode.Image = this.imageList3.Images[1];
            rootNode.Nodes.Add(inputNode);

            DevComponents.AdvTree.Node outputNode = new DevComponents.AdvTree.Node();
            outputNode.Text = "输出参数";
            outputNode.Name = "输出参数";
            outputNode.DataKey = "outputNode";
            outputNode.Image = this.imageList3.Images[2];
            rootNode.Nodes.Add(outputNode);

            //添加参数节点
            for (int i = 0; i < fmiModel.ScalarVariables.Count; i++)
            {
                DevComponents.AdvTree.Node paramNode = new DevComponents.AdvTree.Node();
                paramNode.Text = fmiModel.ScalarVariables[i].name;
                paramNode.Tooltip = "变量名称";
                paramNode.DataKey = "ParamNode";
                paramNode.Tag = fmiModel.ScalarVariables[i].id;
                paramNode.Image = this.imageList3.Images[3];

                //各节点列设置
                for (int j = 0; j < this.advTree_Tool.Columns.Count; j++)
                {
                    paramNode.Cells.Add(new DevComponents.AdvTree.Cell());
                    if (j == 2)
                    {
                        paramNode.Cells[j].Editable = true;
                        if (fmiModel.ScalarVariables[i].scalarVariableValue.declaredType == "double[]" ||
                            fmiModel.ScalarVariables[i].scalarVariableValue.declaredType == "double[][]" ||
                            fmiModel.ScalarVariables[i].scalarVariableValue.declaredType == "int[]" ||
                            fmiModel.ScalarVariables[i].scalarVariableValue.declaredType == "int[][]" ||
                            fmiModel.ScalarVariables[i].scalarVariableValue.declaredType == "string[]" ||
                            fmiModel.ScalarVariables[i].scalarVariableValue.declaredType == "string[][]")
                        {
                            paramNode.Cells[j].Editable = false;
                        }
                    }
                    else
                        paramNode.Cells[j].Editable = false;
                }
                paramNode.Cells[1].Text = fmiModel.ScalarVariables[i].displayName;
                paramNode.Cells[2].Text = fmiModel.ScalarVariables[i].scalarVariableValue.start;
                paramNode.Cells[3].Text = fmiModel.ScalarVariables[i].scalarVariableValue.unit;
                paramNode.Cells[4].Text = fmiModel.ScalarVariables[i].scalarVariableValue.declaredType;

                if (fmiModel.ScalarVariables[i].causality == "input")
                    inputNode.Nodes.Add(paramNode);
                else
                    outputNode.Nodes.Add(paramNode);
            }

            this.advTree_Task.ExpandAll();

            //判断指定目录下是否有组件
            if (fmiModel.toolId != "" && this.hiddenTask != true)
            {
                //MessageBox.Show(fmiModel.toolId);
                if (File.Exists(this.filePath + "\\" + fmiModel.toolId + "\\Program.xml"))
                {
                    LoadApp(this.filePath, fmiModel.toolId);
                }
                else
                    MessageBox.Show("组件" + fmiModel.toolId + "不存在，请到工具库中下载");
            }
        }

        private void RunModelbuttonItem_Click(object sender, EventArgs e)
        {
            try
            {
                //根据关联关系将任务参数值赋给工具参数
                List<DevComponents.AdvTree.Node> listNode = new List<DevComponents.AdvTree.Node>();
                TraversingNode(this.advTree_Task.Nodes, listNode);
                List<DevComponents.AdvTree.Node> listToolNode = new List<DevComponents.AdvTree.Node>();
                TraversingNode(this.advTree_Tool.Nodes, listToolNode);
                for (int i = 0; i < listNode.Count; i++)
                {
                    string toolNodeText = "";
                    if (listNode[i].Cells[5].Text != "")
                        toolNodeText = listNode[i].Cells[5].Text.Split('.')[1];
                    else
                        continue;
                    DevComponents.AdvTree.Node findNode = null;
                    foreach (var item in listToolNode)
                    {
                        if (item.Text == toolNodeText)
                        {
                            findNode = item;
                            break;
                        }
                    }
                    if (findNode != null && findNode.Parent.DataKey.ToString() == "inputNode")
                        findNode.Cells[2].Text = listNode[i].Cells[2].Text;
                }

                List<DevComponents.AdvTree.Node> taskNodeList = new List<DevComponents.AdvTree.Node>();
                TraversingNode(this.advTree_Task.Nodes, taskNodeList);
                List<DevComponents.AdvTree.Node> toolNodeList = new List<DevComponents.AdvTree.Node>();
                TraversingNode(this.advTree_Tool.Nodes, toolNodeList);
                //工具组件运行
                //获取工具参数
                Dictionary<string, string> paramValueDict = new Dictionary<string, string>();
                TraversingNodeValue(this.advTree_Tool.Nodes, paramValueDict);
                Dictionary<string, string> resultDict = null;
                if (this.toolType == ToolType.DLL || this.toolType == ToolType.StandardModel)
                {
                    if (programIntegration != null)
                    {
                        resultDict = programIntegration.AppRun(paramValueDict);
                    }
                }
                else if (this.toolType == ToolType.CREO)
                {
                    if (this.creo != null)
                    {
                        this.printer.PrintMessage("重生模型");
                         resultDict = creo.RegenerateModel(paramValueDict);
                    }
                }
                //输出结果赋值
                foreach (var item in resultDict)
                {
                    DevComponents.AdvTree.Node node = null;
                    foreach (var nodeItem in toolNodeList)
                    {
                        if (nodeItem.Text == item.Key)
                        {
                            node = nodeItem;
                            break;
                        }
                    }

                    node.Cells[2].Text = item.Value;

                    //任务参数赋值
                    //for (int i = 0; i < taskNodeList.Count; i++)
                    //{
                    //    if (taskNodeList[i].Cells[5].Text == "")
                    //        continue;
                    //    if (taskNodeList[i].Cells[5].Text.Split('.')[1] == item.Key)
                    //    {
                    //        taskNodeList[i].Cells[2].Text = item.Value;
                    //        break;
                    //    }
                    //}
                }

                UpdateTaskDataXml();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        public void UpdateTaskDataXml()
        {
            if (this.fmiModel != null)
            {
                //保存计算结果
                this.fmiModel.toolId = this.guid;
                this.fmiModel.toolType = GetAppTypeStr(this.toolType);

                List<DevComponents.AdvTree.Node> nodeList = new List<DevComponents.AdvTree.Node>();
                TraversingNode(this.advTree_Task.Nodes, nodeList);
                for (int i = 0; i < nodeList.Count; i++)
                {
                    for (int j = 0; j < this.fmiModel.ScalarVariables.Count; j++)
                    {
                        if (nodeList[i].Text == this.fmiModel.ScalarVariables[j].name)
                        {
                            this.fmiModel.ScalarVariables[j].scalarVariableValue.start = nodeList[i].Cells[2].Text;
                            break;
                        }
                    }
                }
                this.fmiModel.ExportFmiFile(this.filePath);
            }
        }

        /// <summary>
        /// 递归获取所有节点参数
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private void TraversingNodeValue(DevComponents.AdvTree.NodeCollection nodes, Dictionary<string,string> paramValueDict)
        {
            foreach (DevComponents.AdvTree.Node node in nodes)
            {
                if (node.DataKey.ToString() == "ParamNode")
                {
                    string strValue = "";
                    //if (node.Cells[2].Text != "")
                    //    strValue = Convert.ToDouble(node.Cells[2].Text).ToString();
                    strValue = Regex.Replace(node.Cells[2].Text, "", "");
                    paramValueDict.Add(node.Text, strValue);
                }
                if (node.HasChildNodes)
                {
                    TraversingNodeValue(node.Nodes, paramValueDict);
                }
            }
        }

        /// <summary>
        /// 递归获取所有节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private void TraversingNode(DevComponents.AdvTree.NodeCollection nodes, List<DevComponents.AdvTree.Node> listNode)
        {
            foreach (DevComponents.AdvTree.Node node in nodes)
            {
                if (node.DataKey.ToString() == "ParamNode")
                {
                    listNode.Add(node);
                }
                if (node.HasChildNodes)
                {
                    TraversingNode(node.Nodes, listNode);
                }
            }
        }

        private void buttonItem_Creo_Click(object sender, EventArgs e)
        {
            //CreoIntegration.CreoIntegration creo = new CreoIntegration.CreoIntegration(@"F:\2020工作\ToolResolver\prt0001.prt");
            
            //List<CreoParamModel> paramList = creo.GetMdlParam();
            //creo.CreoStop();
            //this.ToolTreeName = creo.modelName;
            //this.paramList = paramList;

            ////清除节点
            //this.advTree_Tool.Nodes.Clear();

            ////添加根节点
            //DevComponents.AdvTree.Node rootNode = new DevComponents.AdvTree.Node();
            //rootNode.Name = "RootNode";
            //rootNode.Text = creo.modelName;
            //rootNode.Tooltip = "模型名称";
            //rootNode.Image = this.imageList3.Images[0];
            //this.advTree_Tool.Nodes.Add(rootNode);

            ////添加参数节点
            //for (int i = 0; i < paramList.Count; i++)
            //{
            //    DevComponents.AdvTree.Node paramNode = new DevComponents.AdvTree.Node();
            //    paramNode.Text = paramList[i].paramName;
            //    paramNode.Tooltip = "参数名称";
            //    paramNode.DataKey = "ParamNode";
            //    paramNode.Image = this.imageList3.Images[3];

            //    //各节点列设置
            //    for (int j = 0; j < this.advTree_Tool.Columns.Count; j++)
            //    {
            //        paramNode.Cells.Add(new DevComponents.AdvTree.Cell());
            //        if (j == 2)
            //            paramNode.Cells[j].Editable = true;
            //        else
            //            paramNode.Cells[j].Editable = false;
            //    }
            //    paramNode.Cells[1].Text = paramList[i].paramDisplayName;
            //    paramNode.Cells[2].Text = paramList[i].paramValue;
            //    paramNode.Cells[3].Text = paramList[i].paramUnit;
            //    paramNode.Cells[4].Text = paramList[i].paramValueType;

            //    rootNode.Nodes.Add(paramNode);
            //}

            //this.advTree_Tool.ExpandAll();

            //this.toolType = ToolType.CREO;
        }

        private void buttonItem_RelationshipEdit_Click(object sender, EventArgs e)
        {
            if (this.advTree_Tool.Nodes.Count == 0)
            {
                MessageBox.Show("请先添加任务数据。");
                return;
            }
            RelationshipsEdit relationshipEdit = new RelationshipsEdit(this.TaskTreeName, this.ToolTreeName, this.advTree_Task, this.advTree_Tool,this);
            relationshipEdit.Show();
        }

        /// <summary>
        /// 编辑集成新建组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem_New_Click(object sender, EventArgs e)
        {
            //MessageBoxButtons msgButton = MessageBoxButtons.OKCancel;
            
            //if (MessageBox.Show("新建组件会将现有工具覆盖，确定新建吗？","新建组件" ,msgButton) == DialogResult.OK)
            //{
            //    //清空工具参数
            //    this.advTree_Tool.Nodes.Clear();

            //    programIntegration = new ProgramIntegration.ProgramIntegration(Application.StartupPath, ProgramIntegration.ProgramIntegration.StartCmd.NEW);
            //    ProgramIntegration.ProgramModel programModel = programIntegration.GetProgramModel();
            //    //MessageBox.Show(programModel.guid);
            //    this.printer.PrintMessage("->新建组件guid：" + programModel.guid);

            //    InitToolTreeWithAppData(programModel);

            //    this.toolType = ToolType.DLL;
            //}
            //else
            //{
            //    return;
            //}
        }

        /// <summary>
        /// 工具组件初始化工具数据
        /// </summary>
        /// <param name="programModel"></param>
        private void InitToolTreeWithAppData(Object model)
        {
            try
            {
                this.advTree_Tool.Nodes.Clear();
                if (model.GetType().Name == "ProgramModel")
                {
                    ProgramIntegration.ProgramModel programModel = (ProgramIntegration.ProgramModel)model;

                    //添加根节点
                    DevComponents.AdvTree.Node rootNode = new DevComponents.AdvTree.Node();
                    rootNode.Name = "RootNode";
                    rootNode.Text = programModel.modelIdentifier;
                    rootNode.Tooltip = "任务参数";
                    rootNode.DataKey = "RootNode";
                    rootNode.Image = this.imageList3.Images[0];
                    this.advTree_Tool.Nodes.Add(rootNode);

                    //添加输入输出节点
                    DevComponents.AdvTree.Node inputNode = new DevComponents.AdvTree.Node();
                    inputNode.Text = "输入参数";
                    inputNode.Name = "输入参数";
                    inputNode.DataKey = "inputNode";
                    inputNode.Image = this.imageList3.Images[1];
                    rootNode.Nodes.Add(inputNode);

                    DevComponents.AdvTree.Node outputNode = new DevComponents.AdvTree.Node();
                    outputNode.Text = "输出参数";
                    outputNode.Name = "输出参数";
                    outputNode.DataKey = "outputNode";
                    outputNode.Image = this.imageList3.Images[2];
                    rootNode.Nodes.Add(outputNode);

                    //添加参数节点
                    for (int i = 0; i < programModel.modelVariables.Count; i++)
                    {
                        DevComponents.AdvTree.Node paramNode = new DevComponents.AdvTree.Node();
                        paramNode.Text = programModel.modelVariables[i].name;
                        paramNode.Tooltip = "变量名称";
                        paramNode.DataKey = "ParamNode";
                        paramNode.Image = this.imageList3.Images[3];

                        //各节点列设置
                        for (int j = 0; j < this.advTree_Tool.Columns.Count; j++)
                        {
                            paramNode.Cells.Add(new DevComponents.AdvTree.Cell());
                            if (j == 2 && this.hiddenTask == true)
                            {
                                paramNode.Cells[j].Editable = true;
                                if (programModel.modelVariables[i].dataType == "double[]" ||
                                    programModel.modelVariables[i].dataType == "double[][]" ||
                                    programModel.modelVariables[i].dataType == "int[]" ||
                                    programModel.modelVariables[i].dataType == "int[][]" ||
                                    programModel.modelVariables[i].dataType == "string[]" ||
                                    programModel.modelVariables[i].dataType == "string[][]")
                                {
                                    paramNode.Cells[j].Editable = false;
                                }
                            }
                            else
                                paramNode.Cells[j].Editable = false;
                        }
                        paramNode.Cells[1].Text = programModel.modelVariables[i].displayName;
                        paramNode.Cells[2].Text = programModel.modelVariables[i].valueReference;
                        paramNode.Cells[3].Text = programModel.modelVariables[i].unit;
                        paramNode.Cells[4].Text = programModel.modelVariables[i].dataType;

                        if (programModel.modelVariables[i].dataFlow == "input")
                            inputNode.Nodes.Add(paramNode);
                        else
                            outputNode.Nodes.Add(paramNode);
                    }
                }

                if (model.GetType().Name == "StandardModel")
                {
                    ProgramIntegration.StandardModel standardModel = (ProgramIntegration.StandardModel) model;

                    //添加根节点
                    DevComponents.AdvTree.Node rootNode = new DevComponents.AdvTree.Node();
                    rootNode.Name = "RootNode";
                    rootNode.Text = standardModel.modelIdentifier;
                    rootNode.Tooltip = "任务参数";
                    rootNode.DataKey = "RootNode";
                    rootNode.Image = this.imageList3.Images[0];
                    this.advTree_Tool.Nodes.Add(rootNode);

                    //添加输入输出节点
                    DevComponents.AdvTree.Node inputNode = new DevComponents.AdvTree.Node();
                    inputNode.Text = "输入参数";
                    inputNode.Name = "输入参数";
                    inputNode.DataKey = "inputNode";
                    inputNode.Image = this.imageList3.Images[1];
                    rootNode.Nodes.Add(inputNode);

                    DevComponents.AdvTree.Node outputNode = new DevComponents.AdvTree.Node();
                    outputNode.Text = "输出参数";
                    outputNode.Name = "输出参数";
                    outputNode.DataKey = "outputNode";
                    outputNode.Image = this.imageList3.Images[2];
                    rootNode.Nodes.Add(outputNode);

                    for (int i = 0; i < standardModel.paramInList.Count; i++)
                    {
                        DevComponents.AdvTree.Node paramNode = new DevComponents.AdvTree.Node();
                        paramNode.Text = standardModel.paramInList[i].paramName;
                        paramNode.Tooltip = "变量名称";
                        paramNode.DataKey = "ParamNode";
                        paramNode.Image = this.imageList3.Images[3];

                        //各节点列设置
                        for (int j = 0; j < this.advTree_Tool.Columns.Count; j++)
                        {
                            paramNode.Cells.Add(new DevComponents.AdvTree.Cell());
                            if (this.hiddenTask == true)
                                paramNode.Cells[j].Editable = true;
                            else
                                paramNode.Cells[j].Editable = false;
                        }
                        paramNode.Cells[1].Text = standardModel.paramInList[i].paramDescription;
                        if (standardModel.paramInList[i].paramModel == "##rowModel")
                            paramNode.Cells[2].Text = standardModel.paramInList[i].paramValue;
                        else
                        {
                            List<string> tempList = new List<string>();
                            for (int j = 0; j < standardModel.paramInList[i].paramValueList.Count; j++)
                            {
                                string temp = string.Join(",", standardModel.paramInList[i].paramValueList[j].ToArray());
                                tempList.Add(temp);
                            }

                            paramNode.Cells[2].Text = string.Join(";", tempList.ToArray());
                        }
                        paramNode.Cells[3].Text = standardModel.paramInList[i].paramUnit;
                        paramNode.Cells[4].Text = standardModel.paramInList[i].paramType;

                        inputNode.Nodes.Add(paramNode);
                    }

                    for (int i = 0; i < standardModel.paramOutList.Count; i++)
                    {
                        DevComponents.AdvTree.Node paramNode = new DevComponents.AdvTree.Node();
                        paramNode.Text = standardModel.paramOutList[i].paramName;
                        paramNode.Tooltip = "变量名称";
                        paramNode.DataKey = "ParamNode";
                        paramNode.Image = this.imageList3.Images[3];

                        //各节点列设置
                        for (int j = 0; j < this.advTree_Tool.Columns.Count; j++)
                        {
                            paramNode.Cells.Add(new DevComponents.AdvTree.Cell());
                            if (this.hiddenTask == true)
                                paramNode.Cells[j].Editable = true;
                            else
                                paramNode.Cells[j].Editable = false;
                        }
                        paramNode.Cells[1].Text = standardModel.paramOutList[i].paramDescription;
                        if (standardModel.paramOutList[i].paramModel == "##rowModel")
                            paramNode.Cells[2].Text = standardModel.paramOutList[i].paramValue;
                        else
                        {
                            List<string> tempList = new List<string>();
                            for (int j = 0; j < standardModel.paramOutList[i].paramValueList.Count; j++)
                            {
                                string temp = string.Join(",", standardModel.paramOutList[i].paramValueList[j].ToArray());
                                tempList.Add(temp);
                            }

                            paramNode.Cells[2].Text = string.Join(";", tempList.ToArray());
                        }
                        paramNode.Cells[3].Text = standardModel.paramOutList[i].paramUnit;
                        paramNode.Cells[4].Text = standardModel.paramOutList[i].paramType;

                        outputNode.Nodes.Add(paramNode);
                    }
                }
                if (model.GetType().Name == "CreoModel")
                {
                    CreoIntegration.CreoModel creoModel = (CreoIntegration.CreoModel)model;

                    //添加根节点
                    DevComponents.AdvTree.Node rootNode = new DevComponents.AdvTree.Node();
                    rootNode.Name = "RootNode";
                    rootNode.Text = creoModel.modelName;
                    rootNode.Tooltip = "任务参数";
                    rootNode.DataKey = "RootNode";
                    rootNode.Image = this.imageList3.Images[0];
                    this.advTree_Tool.Nodes.Add(rootNode);

                    //添加参数节点
                    for (int i = 0; i < creoModel.paramList.Count; i++)
                    {
                        DevComponents.AdvTree.Node paramNode = new DevComponents.AdvTree.Node();
                        paramNode.Text = creoModel.paramList[i].paramName;
                        paramNode.Tooltip = "变量名称";
                        paramNode.DataKey = "ParamNode";
                        paramNode.Image = this.imageList3.Images[3];

                        //各节点列设置
                        for (int j = 0; j < this.advTree_Tool.Columns.Count; j++)
                        {
                            paramNode.Cells.Add(new DevComponents.AdvTree.Cell());
                            if (this.hiddenTask == true)
                                paramNode.Cells[j].Editable = true;
                            else
                                paramNode.Cells[j].Editable = false;
                        }
                        paramNode.Cells[1].Text = creoModel.paramList[i].paramDisplayName;
                        paramNode.Cells[2].Text = creoModel.paramList[i].paramValue;
                        paramNode.Cells[3].Text = creoModel.paramList[i].paramUnit;
                        paramNode.Cells[4].Text = creoModel.paramList[i].paramValueType;
                        rootNode.Nodes.Add(paramNode);
                    }
                }
                this.advTree_Tool.ExpandAll();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// 编辑集成打开组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem_Open_Click(object sender, EventArgs e)
        {
            //this.programIntegration = new ProgramIntegration.ProgramIntegration(Application.StartupPath, ProgramIntegration.ProgramIntegration.StartCmd.OPEN, "F41A47AF-71F4-480C-B76C-5ED5FB8AD642");
            //ProgramIntegration.ProgramModel programModel = programIntegration.GetProgramModel();
            ////MessageBox.Show(programModel.guid);
            //InitToolTreeWithAppData(programModel);

            ////获取已有关联关系
            //Dictionary<string, string> dictRelationship = new Dictionary<string, string>();
            //programIntegration.GetRelationship(dictRelationship);
            //List<DevComponents.AdvTree.Node> nodeList = new List<DevComponents.AdvTree.Node>();
            //TraversingNode(this.advTree_Task.Nodes, nodeList);
            //for (int i = 0; i < nodeList.Count; i++)
            //{
            //    nodeList[i].Cells[5].Text = dictRelationship[nodeList[i].Text];
            //}

            //    this.toolType = ToolType.DLL;
        }

        /// <summary>
        /// 模型类型
        /// </summary>
        public enum ToolType
        {
            DLL,//工具组件
            CREO,//Creo模型
            StandardModel//标准模型
        }

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadDataButtonItem_Click(object sender, EventArgs e)
        {
            //保存任务数据
            List<DevComponents.AdvTree.Node> taskNodeList = new List<DevComponents.AdvTree.Node>();
            TraversingNode(this.advTree_Task.Nodes, taskNodeList);
            for (int i = 0; i < fmiModel.ScalarVariables.Count; i++)
            {
                foreach (var item in taskNodeList)
                {
                    if (item.Text == fmiModel.ScalarVariables[i].name)
                        fmiModel.ScalarVariables[i].scalarVariableValue.start = item.Cells[2].Text;
                    break;
                }
            }
            fmiModel.toolType = this.toolType.ToString().ToLower();
            fmiModel.toolId = this.guid;
        }
        
        /// <summary>
        /// 工具库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolKitButtonItem_Click(object sender, EventArgs e)
        {
            if (this.creo != null)
                this.creo.CreoStop();

            try
            {
                //工具库路径
                this.printer.PrintMessage("启动工具库...");
                string shareWorksPath = Application.StartupPath;
                int nPos = shareWorksPath.LastIndexOf('\\');
                shareWorksPath = shareWorksPath.Substring(0,nPos);

                shareWorksPath = @"C:\ShareWorks";
                string appToolPath = shareWorksPath + "\\AppTools\\AppTools.exe";
                //string compPath = Application.StartupPath + "\\AppLib";
                Process p = new Process();
                p.StartInfo.FileName = appToolPath;
                p.StartInfo.Arguments = this.filePath + " 1";
                p.Start();
                p.WaitForExit();
                p.Close();

                //获取选择组件
                string guidFilePath = shareWorksPath + "\\AppTools\\GUID.txt";
                StreamReader sr = new StreamReader(guidFilePath);
                string guid = sr.ReadLine();
                sr.Close();
                LoadApp(this.filePath, guid);
                InitialProjectFilesView(this.superTabControlPanel4);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public void LoadApp(string filePath,string guid)
        {
            //获取组件信息
            this.programIntegration = new ProgramIntegration.ProgramIntegration(Application.StartupPath, filePath, ProgramIntegration.ProgramIntegration.StartCmd.OPEN, guid);
            //MessageBox.Show(programModel.guid);

            this.guid = guid;
            this.toolType = GetAppType(this.programIntegration.baseModel.applicationTypeCode);

            if (this.toolType == ToolType.DLL)
            {
                ProgramIntegration.ProgramModel programModel = (ProgramIntegration.ProgramModel)programIntegration.GetProgramModel();
                InitToolTreeWithAppData(programModel);
            }
            else if (this.toolType == ToolType.StandardModel)
            {
                ProgramIntegration.StandardModel standardModel = (ProgramIntegration.StandardModel)programIntegration.GetProgramModel();
                InitToolTreeWithAppData(standardModel);
            }
            else if (this.toolType == ToolType.CREO)
            {
                //判断是否配置Creo路径
                string creoPath = "";
                if (File.Exists(Application.StartupPath + "\\CreoConfig.txt"))
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\CreoConfig.txt");
                    creoPath = sr.ReadLine();
                    sr.Close();
                    if (!File.Exists(creoPath))
                    {
                        CreoConfig creoConfig = new CreoConfig();
                        creoConfig.ShowDialog();
                        sr = new StreamReader(Application.StartupPath + "\\CreoConfig.txt");
                        creoPath = sr.ReadLine();
                        sr.Close();
                    }
                }

                string modelPath = "";
                if (this.filePath.LastIndexOf('\\') != this.filePath.Length - 1)
                    modelPath = this.filePath + "\\" + this.guid + "\\Data\\" + this.programIntegration.baseModel.modelIdentifier;
                else
                    modelPath = this.filePath + this.guid + "\\Data\\" + this.programIntegration.baseModel.modelIdentifier;
                this.printer.PrintMessage("启动Creo中...");
                this.creo = new CreoIntegration.CreoIntegration(modelPath,creoPath);
                this.printer.PrintMessage("获取模型参数");
                CreoIntegration.CreoModel model = this.creo.GetCreoModel();
                InitToolTreeWithAppData(model);
            }

            //获取已有关联关系
            Dictionary<string, string> dictRelationship = new Dictionary<string, string>();
            this.printer.PrintMessage("获取关联关系");
            new ProgramIntegration.ProgramIntegration().GetRelationship(filePath, dictRelationship);
            //this.programIntegration.GetRelationship(dictRelationship);
            List<DevComponents.AdvTree.Node> nodeList = new List<DevComponents.AdvTree.Node>();
            TraversingNode(this.advTree_Task.Nodes, nodeList);
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (dictRelationship.ContainsKey(nodeList[i].Text))
                {
                    nodeList[i].Cells[5].Text = dictRelationship[nodeList[i].Text];
                }
            }
        }

        public ToolType GetAppType(string type)
        {
            if (type == "Proe")
                return ToolType.CREO;
            if (type == "Caculate")
                return ToolType.DLL;
            if (type == "StandardModel")
                return ToolType.StandardModel;

            return ToolType.DLL;
        }

        public string GetAppTypeStr(ToolType type)
        {
            if (type == ToolType.CREO)
                return "Proe";
            if (type == ToolType.DLL)
                return "Caculate";
            if (type == ToolType.StandardModel)
                return "StandardModel";

            return "Caculate";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.creo != null)
                this.creo.CreoStop();
        }

        private void advTree_Task_CellSelected(object sender, DevComponents.AdvTree.AdvTreeCellEventArgs e)
        {
            if (e.Cell.ColumnHeader.Text == "当前值" && (this.advTree_Task.SelectedNode.Cells[4].Text == "double[]" || this.advTree_Task.SelectedNode.Cells[4].Text == "double[][]"))
            {
                ArrayWindows.ParaArrayEditor arrayEditor = new ParaArrayEditor();

                Knowing.UserMath knowind = new UserMath();
                string paramType = this.advTree_Task.SelectedNode.Cells[4].Text;
                switch (paramType)
                {
                    case "double[]":
                        double[] paramValueSingle;
                        knowind.ArrayDessToArray(e.Cell.Text, out paramValueSingle);
                        arrayEditor.InitializeArray(paramValueSingle, this.advTree_Task.SelectedNode.Cells[4].Text);
                        if (arrayEditor.ShowDialog() == DialogResult.OK)
                        {
                            Object paramArray = arrayEditor.GetArray();
                            string strTemp = "";
                            knowind.ArrayDescrib(paramArray, out strTemp);
                            e.Cell.Text = strTemp;
                        }
                        break;
                    case "double[][]":
                        double[][] paramValueDouble;
                        knowind.ArrayDessToArray(e.Cell.Text, out paramValueDouble);
                        arrayEditor.InitializeArray(paramValueDouble, this.advTree_Task.SelectedNode.Cells[4].Text);
                        if (arrayEditor.ShowDialog() == DialogResult.OK)
                        {
                            Object paramArray = arrayEditor.GetArray();
                            string strTemp = "";
                            knowind.ArrayDescrib(paramArray, out strTemp);
                            e.Cell.Text = strTemp;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void advTree_Tool_CellSelected(object sender, AdvTreeCellEventArgs e)
        {
            if (e.Cell.ColumnHeader.Text == "当前值" && (this.advTree_Tool.SelectedNode.Cells[4].Text == "double[]" || this.advTree_Tool.SelectedNode.Cells[4].Text == "double[][]"))
            {
                ArrayWindows.ParaArrayEditor arrayEditor = new ParaArrayEditor();

                Knowing.UserMath knowind = new UserMath();
                string paramType = this.advTree_Tool.SelectedNode.Cells[4].Text;
                switch (paramType)
                {
                    case "double[]":
                        double[] paramValueSingle;
                        knowind.ArrayDessToArray(e.Cell.Text, out paramValueSingle);
                        arrayEditor.InitializeArray(paramValueSingle, this.advTree_Tool.SelectedNode.Cells[4].Text);
                        if (arrayEditor.ShowDialog() == DialogResult.OK)
                        {
                            Object paramArray = arrayEditor.GetArray();
                            string strTemp = "";
                            knowind.ArrayDescrib(paramArray, out strTemp);
                            e.Cell.Text = strTemp;
                        }
                        break;
                    case "double[][]":
                        double[][] paramValueDouble;
                        knowind.ArrayDessToArray(e.Cell.Text, out paramValueDouble);
                        arrayEditor.InitializeArray(paramValueDouble, this.advTree_Tool.SelectedNode.Cells[4].Text);
                        if (arrayEditor.ShowDialog() == DialogResult.OK)
                        {
                            Object paramArray = arrayEditor.GetArray();
                            string strTemp = "";
                            knowind.ArrayDescrib(paramArray, out strTemp);
                            e.Cell.Text = strTemp;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
