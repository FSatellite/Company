using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentConvert
{
    /// <summary>
    /// 标准模型
    /// </summary>
    public class StandardModel : BaseModel
    {
        /// <summary>
        /// 输入参数
        /// </summary>
        public List<StandardModelParam> paramInList = new List<StandardModelParam>();
        public Dictionary<string, StandardModelParam> paramInDict = new Dictionary<string, StandardModelParam>();

        /// <summary>
        /// 输出参数
        /// </summary>
        public List<StandardModelParam> paramOutList = new List<StandardModelParam>();
        public Dictionary<string, StandardModelParam> paramOutDict = new Dictionary<string, StandardModelParam>();
    }

    public class StandardModelParam
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        public string paramName { get; set; }

        /// <summary>
        /// 参数描述
        /// </summary>
        public string paramDescription { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string paramValue { get; set; }

        /// <summary>
        /// 参数块名称
        /// </summary>
        public string paramModel { get; set; }

        /// <summary>
        /// 参数维度
        /// </summary>
        public string paramMatrix { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public string paramUnit { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string paramType { get; set; }

        /// <summary>
        /// 参数所在文件
        /// </summary>
        public string paramFile { get; set; }

        public List<List<string>> paramValueList = new List<List<string>>();
    }
}
