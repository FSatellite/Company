using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentConvert
{
    /// <summary>
    /// 基类
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// FMI版本
        /// </summary>
        public string fmiVersion { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string modelName { get; set; }

        /// <summary>
        /// 程序的Guid编码
        /// </summary>
        public string guid { get; set; }

        /// <summary>
        /// 模型的简要概况
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 作者的名称或组织
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 模型的可选版本
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// 有关此FMU的知识产权版权的可选信息
        /// </summary>
        public string copyright { get; set; }

        /// <summary>
        /// 有关此FMU的知识产权许可的可选信息
        /// </summary>
        public string license { get; set; }

        /// <summary>
        /// 生成XML文件的工具的可选名称
        /// </summary>
        public string generationTool { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string generationDateAndTime { get; set; }

        /// <summary>
        /// 枚举变量。-flat：扁平化变量名称-structured：带有“.”的分层结构变量名称。
        /// </summary>
        public string VariableNamingConvention { get; set; }

        /// <summary>
        /// 工具类型
        /// </summary>
        public string toolType { get; set; }

        /// <summary>
        /// 工具guid
        /// </summary>
        public string toolId { get; set; }
    }
}
