using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentConvert
{
    public class FMIModel : BaseModel
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        public List<ScalarVariableModel> ScalarVariables = new List<ScalarVariableModel>();
    }

    /// <summary>
    /// FMIModel变量模型
    /// </summary>
    public class ScalarVariableModel
    {
        /// <summary>
        /// 变量的标识
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 变量的英文名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 变量参考值
        /// </summary>
        public string valueReference { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string causality { get; set; }

        /// <summary>
        /// 变量连续性
        /// </summary>
        public string variability { get; set; }

        /// <summary>
        /// 变量描述信息
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 变量显示名称
        /// </summary>
        public string displayName { get; set; }

        /// <summary>
        /// 变量的索引文件
        /// </summary>
        public string attachFile { get; set; }

        /// <summary>
        /// 变量的版本
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 参数值类
        /// </summary>
        public ScalarVariableValue scalarVariableValue = new ScalarVariableValue();
    }

    /// <summary>
    /// 变量值
    /// </summary>
    public class ScalarVariableValue
    {
        /// <summary>
        /// 变量类型
        /// </summary>
        public string declaredType { get; set; }

        /// <summary>
        /// 变量初始值
        /// </summary>
        public string start { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public string derivative { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string reinit { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public string min { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public string max { get; set; }
    }
}
