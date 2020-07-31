using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using CreoIntegration;
using FMIModel;

using DevComponents.DotNetBar;

namespace ToolResolver
{
    public partial class RelationshipsEdit : Office2007Form
    {
        /// <summary>
        /// 父窗口句柄
        /// </summary>
        private Form1 parentForm = null;

        /// <summary>
        /// 任务参数模型名称
        /// </summary>
        private string TaskTreeName;

        /// <summary>
        /// 工具参数模型名称
        /// </summary>
        private string ToolTreeName;

        /// <summary>
        /// 任务参数树
        /// </summary>
        private DevComponents.AdvTree.AdvTree taskTree = null;

        /// <summary>
        /// 工具参数树
        /// </summary>
        private DevComponents.AdvTree.AdvTree toolTree = null;

        /// <summary>
        /// 任务参数选择节点
        /// </summary>
        private DevComponents.AdvTree.Node taskSelectNode;

        public RelationshipsEdit(string taskTreeName, string toolTreeName, DevComponents.AdvTree.AdvTree taskTree, DevComponents.AdvTree.AdvTree toolTree,Form1 parentForm)
        {
            this.EnableGlass = false;

            this.TaskTreeName = taskTreeName;
            this.ToolTreeName = toolTreeName;
            this.taskTree = taskTree;
            this.toolTree = toolTree;

            this.parentForm = parentForm;

            InitializeComponent();
            InitTaskTree();
            InitToolTree();
        }

        /// <summary>
        /// 建立关联关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Relate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.taskSelectNode.DataKey.ToString() == "ParamNode")
                {
                    DevComponents.AdvTree.Node toolNode = this.advTree_Tool.SelectedNode;
                    int nIndex = this.advTree_Tool.SelectedIndex;
                    if (nIndex != -1)
                    {
                        //获取工具参数信息
                        this.taskSelectNode.Cells[1].Text = "工具参数." + toolNode.Text;

                        //删除工具参数节点
                        //toolNode.Remove();
                    }
                }
            }
            catch (Exception ee)
            {

            }
        }

        /// <summary>
        /// 初始化任务数据树
        /// </summary>
        public void InitTaskTree()
        {
            this.advTree_Task.Nodes.Clear();

            foreach (DevComponents.AdvTree.Node node in this.taskTree.Nodes)
            {
                //避免绑定原构型树，需要做特殊处理
                DevComponents.AdvTree.Node geomNode = new DevComponents.AdvTree.Node();
                geomNode.Text = node.Text;
                geomNode.Name = node.Name;
                geomNode.Tag = node;
                geomNode.DataKey = node.DataKey;
                geomNode.Image = this.imageList3.Images[0];
                AddNode(geomNode, node);
                this.advTree_Task.Nodes.Add(geomNode);
            }

            this.advTree_Task.ExpandAll();
        }

        public void InitToolTree()
        {
            //清除节点
            this.advTree_Tool.Nodes.Clear();

            foreach (DevComponents.AdvTree.Node node in this.toolTree.Nodes)
            {
                //避免绑定原构型树，需要做特殊处理
                DevComponents.AdvTree.Node geomNode = new DevComponents.AdvTree.Node();
                geomNode.Text = node.Text;
                geomNode.Name = node.Name;
                geomNode.Tag = node;
                geomNode.DataKey = node.DataKey;
                geomNode.Image = this.imageList3.Images[0];
                AddNode(geomNode, node);
                this.advTree_Tool.Nodes.Add(geomNode);
            }

            this.advTree_Tool.ExpandAll();
        }

        /// <summary>
        /// 递归添加节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="tagNode"></param>
        private void AddNode(DevComponents.AdvTree.Node node, DevComponents.AdvTree.Node tagNode)
        {
            if (tagNode.HasChildNodes)
            {
                foreach (DevComponents.AdvTree.Node nodeChild in tagNode.Nodes)
                {
                    DevComponents.AdvTree.Node geomNode = new DevComponents.AdvTree.Node();
                    geomNode.Text = nodeChild.Text;
                    geomNode.Name = nodeChild.Name;
                    geomNode.Tag = nodeChild;
                    geomNode.DataKey = nodeChild.DataKey;

                    if (geomNode.DataKey.ToString() == "ParamNode")
                        geomNode.Image = this.imageList3.Images[3];
                    if (geomNode.DataKey.ToString() == "inputNode")
                        geomNode.Image = this.imageList3.Images[1];
                    if (geomNode.DataKey.ToString() == "outputNode")
                        geomNode.Image = this.imageList3.Images[2];

                    node.Nodes.Add(geomNode);

                    if (geomNode.DataKey.ToString() == "ParamNode")
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            geomNode.Cells.Add(new DevComponents.AdvTree.Cell());
                            geomNode.Cells[i].Editable = false;
                        }
                        if (nodeChild.Cells.Count == 6)
                            geomNode.Cells[1].Text = nodeChild.Cells[5].Text;
                        else
                            geomNode.Cells[1].Text = "";
                    }

                    AddNode(geomNode, nodeChild);
                }
            }
        }

        /// <summary>
        /// 任务数据节点点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTree_Task_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            try
            {
                this.taskSelectNode = this.advTree_Task.SelectedNode;
                if (this.advTree_Task.SelectedNode.DataKey.ToString() == "RootNode" ||
                    this.advTree_Task.SelectedNode.DataKey.ToString() == "inputNode" ||
                    this.advTree_Task.SelectedNode.DataKey.ToString() == "outputNode")
                    return;

                if (this.advTree_Task.SelectedNode.Text != "")
                {
                    List<DevComponents.AdvTree.Node> nodeList = new List<DevComponents.AdvTree.Node>();
                    TraversingNode(this.parentForm.advTree_Task.Nodes, nodeList);
                    DevComponents.AdvTree.Node nodeFind = null;
                    foreach (var item in nodeList)
                    {
                        if (item.Text == this.advTree_Task.SelectedNode.Text)
                        {
                            nodeFind = item;
                            break;
                        }
                    }
                    if (nodeFind != null)
                    {
                        this.textBox_TaskName.Text = nodeFind.Cells[1].Text;
                        this.textBox_TaskUnit.Text = nodeFind.Cells[3].Text;
                        this.textBox_TaskDataType.Text = nodeFind.Cells[4].Text;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        /// <summary>
        /// 工具数据节点点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTree_Tool_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
        {
            try
            {
                if (this.advTree_Tool.SelectedNode.DataKey.ToString() == "RootNode")
                    return;

                if (this.advTree_Tool.SelectedNode.Text != "")
                {
                    List<DevComponents.AdvTree.Node> nodeList = new List<DevComponents.AdvTree.Node>();
                    TraversingNode(this.parentForm.advTree_Tool.Nodes, nodeList);
                    DevComponents.AdvTree.Node nodeFind = null;
                    foreach (var item in nodeList)
                    {
                        if (item.Text == this.advTree_Tool.SelectedNode.Text)
                        {
                            nodeFind = item;
                            break;
                        }
                    }
                    if (nodeFind != null)
                    {
                        this.textBox_ToolName.Text = nodeFind.Cells[1].Text;
                        this.textBox_ToolUnit.Text = nodeFind.Cells[3].Text;
                        this.textBox_ToolDataType.Text = nodeFind.Cells[4].Text;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        /// <summary>
        /// 保存关联关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            //任务数据关联关系赋值
            List<DevComponents.AdvTree.Node> nodeList = new List<DevComponents.AdvTree.Node>();
            List<DevComponents.AdvTree.Node> parentNodeList = new List<DevComponents.AdvTree.Node>();
            TraversingNode(this.advTree_Task.Nodes,nodeList);
            TraversingNode(this.parentForm.advTree_Task.Nodes, parentNodeList);
            foreach (var item in nodeList)
            {
                DevComponents.AdvTree.Node nodeFind = null;
                foreach (var parentItem in parentNodeList)
                {
                    if (parentItem.Text == item.Text)
                    {
                        nodeFind = parentItem;
                        break;
                    }
                }
                nodeFind.Cells[5].Text = item.Cells[1].Text;
            }

            //保存关联关系到xml
            SaveRelationship(nodeList, this.parentForm.filePath + "\\Relationship.xml");

            //关闭对话框
            this.Dispose();
        }

        public void SaveRelationship(List<DevComponents.AdvTree.Node> nodeList,string savePath)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode declarationNode = doc.CreateXmlDeclaration("1.0", "utf-8", "");
            doc.AppendChild(declarationNode);

            XmlNode rootNode = doc.CreateElement("Relationships");
            doc.AppendChild(rootNode);

            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode node = doc.CreateNode(XmlNodeType.Element, "Param", null);
                XmlElement nodeElement = (XmlElement)node;
                nodeElement.SetAttribute("TaskParam", nodeList[i].Cells[0].Text);
                nodeElement.SetAttribute("ToolParam", nodeList[i].Cells[1].Text);
                rootNode.AppendChild(node);
            }
            doc.Save(savePath);
        }

        /// <summary>
        /// 递归获取所有参数节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private void TraversingNode(DevComponents.AdvTree.NodeCollection nodes,List<DevComponents.AdvTree.Node> nodeList)
        {
            foreach (DevComponents.AdvTree.Node node in nodes)
            {
                if (node.DataKey.ToString() == "ParamNode")
                {
                    nodeList.Add(node);
                }
                if (node.HasChildNodes)
                {
                    TraversingNode(node.Nodes,nodeList);
                }
            }
        }

        /// <summary>
        /// 清除单个关联关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ClearRelate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.advTree_Task.SelectedNode.DataKey.ToString() == "RootNode")
                    return;

                if (this.advTree_Task.SelectedNode.Text != "")
                {
                    this.advTree_Task.SelectedNode.Cells[1].Text = "";
                }
            }
            catch (Exception ee)
            {

            }
        }

        /// <summary>
        /// 清除关联关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Clear_Click(object sender, EventArgs e)
        {
            List<DevComponents.AdvTree.Node> nodeList = new List<DevComponents.AdvTree.Node>();
            TraversingNode(this.advTree_Task.Nodes, nodeList);
            for (int i = 0; i < nodeList.Count; i++)
            {
                nodeList[i].Cells[1].Text = "";
            }
        }
    }
}
