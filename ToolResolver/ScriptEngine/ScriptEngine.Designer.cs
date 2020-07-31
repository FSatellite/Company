namespace ScriptEngine
{
    partial class ScriptEngine
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptEngine));
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.styleManager2 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.itemsTreeModel = new DevComponents.AdvTree.AdvTree();
            this.columnHeader1 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader2 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader3 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader4 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader5 = new DevComponents.AdvTree.ColumnHeader();
            this.columnHeader6 = new DevComponents.AdvTree.ColumnHeader();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.inputCheckBox = new System.Windows.Forms.CheckBox();
            this.outPutCheckBox = new System.Windows.Forms.CheckBox();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.label2 = new System.Windows.Forms.Label();
            this.Applybutton = new System.Windows.Forms.Button();
            this.Clearbutton = new System.Windows.Forms.Button();
            this.messageBox = new System.Windows.Forms.RichTextBox();
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.MaxbuttonItem = new DevComponents.DotNetBar.ButtonItem();
            this.MinbuttonItem = new DevComponents.DotNetBar.ButtonItem();
            this.SumbuttonItem = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem4 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem5 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem6 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem7 = new DevComponents.DotNetBar.ButtonItem();
            this.power = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem9 = new DevComponents.DotNetBar.ButtonItem();
            this.ParacontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ParasbuttonItem = new DevComponents.DotNetBar.ButtonItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.itemsTreeModel)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Blue;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // styleManager2
            // 
            this.styleManager2.ManagerStyle = DevComponents.DotNetBar.eStyle.Office2010Blue;
            this.styleManager2.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // itemsTreeModel
            // 
            this.itemsTreeModel.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.itemsTreeModel.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.itemsTreeModel.BackgroundStyle.Class = "TreeBorderKey";
            this.itemsTreeModel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemsTreeModel.Columns.Add(this.columnHeader1);
            this.itemsTreeModel.Columns.Add(this.columnHeader2);
            this.itemsTreeModel.Columns.Add(this.columnHeader3);
            this.itemsTreeModel.Columns.Add(this.columnHeader4);
            this.itemsTreeModel.Columns.Add(this.columnHeader5);
            this.itemsTreeModel.Columns.Add(this.columnHeader6);
            this.itemsTreeModel.ImageList = this.imageList1;
            this.itemsTreeModel.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.itemsTreeModel.Location = new System.Drawing.Point(3, 35);
            this.itemsTreeModel.Name = "itemsTreeModel";
            this.itemsTreeModel.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.itemsTreeModel.NodesConnector = this.nodeConnector1;
            this.itemsTreeModel.NodeStyle = this.elementStyle1;
            this.itemsTreeModel.PathSeparator = ";";
            this.itemsTreeModel.Size = new System.Drawing.Size(794, 185);
            this.itemsTreeModel.Styles.Add(this.elementStyle1);
            this.itemsTreeModel.TabIndex = 4;
            this.itemsTreeModel.Text = "advTree1";
            this.itemsTreeModel.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.itemsTreeModel_NodeClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Name = "columnHeader1";
            this.columnHeader1.Text = "变量名称";
            this.columnHeader1.Width.Absolute = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Name = "columnHeader2";
            this.columnHeader2.Text = "数据类型";
            this.columnHeader2.Width.Absolute = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Name = "columnHeader3";
            this.columnHeader3.Text = "值";
            this.columnHeader3.Width.Absolute = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Name = "columnHeader4";
            this.columnHeader4.Text = "表达式";
            this.columnHeader4.Width.Absolute = 200;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Name = "columnHeader5";
            this.columnHeader5.Text = "描述";
            this.columnHeader5.Width.Absolute = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Name = "columnHeader6";
            this.columnHeader6.Text = "生效";
            this.columnHeader6.Width.Absolute = 60;
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "参数过滤";
            // 
            // inputCheckBox
            // 
            this.inputCheckBox.AutoSize = true;
            this.inputCheckBox.Location = new System.Drawing.Point(83, 11);
            this.inputCheckBox.Name = "inputCheckBox";
            this.inputCheckBox.Size = new System.Drawing.Size(72, 16);
            this.inputCheckBox.TabIndex = 6;
            this.inputCheckBox.Text = "输入变量";
            this.inputCheckBox.UseVisualStyleBackColor = true;
            this.inputCheckBox.CheckedChanged += new System.EventHandler(this.inputCheckBox_CheckedChanged);
            // 
            // outPutCheckBox
            // 
            this.outPutCheckBox.AutoSize = true;
            this.outPutCheckBox.Location = new System.Drawing.Point(190, 10);
            this.outPutCheckBox.Name = "outPutCheckBox";
            this.outPutCheckBox.Size = new System.Drawing.Size(72, 16);
            this.outPutCheckBox.TabIndex = 7;
            this.outPutCheckBox.Text = "输出变量";
            this.outPutCheckBox.UseVisualStyleBackColor = true;
            this.outPutCheckBox.CheckedChanged += new System.EventHandler(this.outPutCheckBox_CheckedChanged);
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textEditorControl1.IsReadOnly = false;
            this.textEditorControl1.Location = new System.Drawing.Point(5, 253);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.Size = new System.Drawing.Size(792, 127);
            this.textEditorControl1.TabIndex = 8;
            this.textEditorControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textEditorControl1_MouseClick);
            this.textEditorControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textEditorControl1_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "表达式编辑";
            this.label2.DoubleClick += new System.EventHandler(this.label2_DoubleClick);
            // 
            // Applybutton
            // 
            this.Applybutton.Location = new System.Drawing.Point(86, 386);
            this.Applybutton.Name = "Applybutton";
            this.Applybutton.Size = new System.Drawing.Size(75, 23);
            this.Applybutton.TabIndex = 10;
            this.Applybutton.Text = "应用";
            this.Applybutton.UseVisualStyleBackColor = true;
            this.Applybutton.Click += new System.EventHandler(this.Applybutton_Click);
            // 
            // Clearbutton
            // 
            this.Clearbutton.Location = new System.Drawing.Point(5, 386);
            this.Clearbutton.Name = "Clearbutton";
            this.Clearbutton.Size = new System.Drawing.Size(75, 23);
            this.Clearbutton.TabIndex = 11;
            this.Clearbutton.Text = "删除";
            this.Clearbutton.UseVisualStyleBackColor = true;
            this.Clearbutton.Click += new System.EventHandler(this.Clearbutton_Click);
            // 
            // messageBox
            // 
            this.messageBox.BackColor = System.Drawing.SystemColors.MenuBar;
            this.messageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.messageBox.Location = new System.Drawing.Point(222, 389);
            this.messageBox.Name = "messageBox";
            this.messageBox.Size = new System.Drawing.Size(564, 21);
            this.messageBox.TabIndex = 12;
            this.messageBox.Text = "";
            // 
            // itemPanel1
            // 
            this.itemPanel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.itemPanel1.BackgroundStyle.Class = "ItemPanel";
            this.itemPanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.DragDropSupport = true;
            this.itemPanel1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ParasbuttonItem,
            this.MaxbuttonItem,
            this.MinbuttonItem,
            this.SumbuttonItem,
            this.buttonItem4,
            this.buttonItem5,
            this.buttonItem6,
            this.buttonItem7,
            this.power,
            this.buttonItem9});
            this.itemPanel1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.itemPanel1.Location = new System.Drawing.Point(234, 226);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(544, 21);
            this.itemPanel1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.itemPanel1.TabIndex = 13;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // MaxbuttonItem
            // 
            this.MaxbuttonItem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.MaxbuttonItem.Image = ((System.Drawing.Image)(resources.GetObject("MaxbuttonItem.Image")));
            this.MaxbuttonItem.Name = "MaxbuttonItem";
            this.MaxbuttonItem.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor();
            this.MaxbuttonItem.Text = "max";
            this.MaxbuttonItem.Tooltip = "求一维数组里面的最大值";
            this.MaxbuttonItem.Click += new System.EventHandler(this.MaxbuttonItem_Click);
            // 
            // MinbuttonItem
            // 
            this.MinbuttonItem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.MinbuttonItem.Image = ((System.Drawing.Image)(resources.GetObject("MinbuttonItem.Image")));
            this.MinbuttonItem.Name = "MinbuttonItem";
            this.MinbuttonItem.Text = "min";
            this.MinbuttonItem.Tooltip = "求一维数组里的最小值";
            this.MinbuttonItem.Click += new System.EventHandler(this.MinbuttonItem_Click);
            // 
            // SumbuttonItem
            // 
            this.SumbuttonItem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.SumbuttonItem.Image = ((System.Drawing.Image)(resources.GetObject("SumbuttonItem.Image")));
            this.SumbuttonItem.Name = "SumbuttonItem";
            this.SumbuttonItem.Text = "sum";
            this.SumbuttonItem.Tooltip = "求一维数组所有元素的累加和";
            this.SumbuttonItem.Click += new System.EventHandler(this.SumbuttonItem_Click);
            // 
            // buttonItem4
            // 
            this.buttonItem4.Name = "buttonItem4";
            this.buttonItem4.Text = "sin";
            // 
            // buttonItem5
            // 
            this.buttonItem5.Name = "buttonItem5";
            this.buttonItem5.Text = "cos";
            // 
            // buttonItem6
            // 
            this.buttonItem6.Name = "buttonItem6";
            this.buttonItem6.Text = "tan";
            // 
            // buttonItem7
            // 
            this.buttonItem7.Name = "buttonItem7";
            this.buttonItem7.Text = "log";
            // 
            // power
            // 
            this.power.Name = "power";
            this.power.Text = "power";
            // 
            // buttonItem9
            // 
            this.buttonItem9.Name = "buttonItem9";
            this.buttonItem9.Text = "sqrt";
            // 
            // ParacontextMenuStrip
            // 
            this.ParacontextMenuStrip.Name = "ParacontextMenuStrip";
            this.ParacontextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // ParasbuttonItem
            // 
            this.ParasbuttonItem.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.ParasbuttonItem.Image = ((System.Drawing.Image)(resources.GetObject("ParasbuttonItem.Image")));
            this.ParasbuttonItem.Name = "ParasbuttonItem";
            this.ParasbuttonItem.Text = "参数";
            this.ParasbuttonItem.Click += new System.EventHandler(this.ParasbuttonItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "11-输入变量.png");
            this.imageList1.Images.SetKeyName(1, "12-输出变量.png");
            // 
            // ScriptEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Controls.Add(this.itemPanel1);
            this.Controls.Add(this.messageBox);
            this.Controls.Add(this.Clearbutton);
            this.Controls.Add(this.Applybutton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.outPutCheckBox);
            this.Controls.Add(this.inputCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.itemsTreeModel);
            this.Name = "ScriptEngine";
            this.Size = new System.Drawing.Size(800, 424);
            ((System.ComponentModel.ISupportInitialize)(this.itemsTreeModel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.StyleManager styleManager2;
        private DevComponents.AdvTree.AdvTree itemsTreeModel;
        private DevComponents.AdvTree.ColumnHeader columnHeader1;
        private DevComponents.AdvTree.ColumnHeader columnHeader2;
        private DevComponents.AdvTree.ColumnHeader columnHeader3;
        private DevComponents.AdvTree.ColumnHeader columnHeader4;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox inputCheckBox;
        private System.Windows.Forms.CheckBox outPutCheckBox;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Applybutton;
        private System.Windows.Forms.Button Clearbutton;
        private DevComponents.AdvTree.ColumnHeader columnHeader5;
        private DevComponents.AdvTree.ColumnHeader columnHeader6;
        private System.Windows.Forms.RichTextBox messageBox;
        private DevComponents.DotNetBar.ItemPanel itemPanel1;
        private DevComponents.DotNetBar.ButtonItem MaxbuttonItem;
        private DevComponents.DotNetBar.ButtonItem MinbuttonItem;
        private DevComponents.DotNetBar.ButtonItem SumbuttonItem;
        private DevComponents.DotNetBar.ButtonItem buttonItem4;
        private DevComponents.DotNetBar.ButtonItem buttonItem5;
        private DevComponents.DotNetBar.ButtonItem buttonItem6;
        private DevComponents.DotNetBar.ButtonItem buttonItem7;
        private DevComponents.DotNetBar.ButtonItem power;
        private DevComponents.DotNetBar.ButtonItem buttonItem9;
        private System.Windows.Forms.ContextMenuStrip ParacontextMenuStrip;
        private DevComponents.DotNetBar.ButtonItem ParasbuttonItem;
        private System.Windows.Forms.ImageList imageList1;
    }
}
