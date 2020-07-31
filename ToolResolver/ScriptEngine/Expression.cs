using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptInterface;
namespace ScriptEngine
{
    /// <summary>
    /// 表达式实现类
    /// </summary>
  public   class Expression:OptInterface .IExpression 
    {

        public Expression()
        {
            script = "";
            bandingParaName = "";
            actived = true;
        }
        /// <summary>
        /// 表达式
        /// </summary>
        public string script
        {
            get;
            set;
        }
        /// <summary>
        /// 表达式所针对的变量的输入输出类型
        /// </summary>
        public string bandingParaName
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有效
        /// </summary>
       public bool actived
        {
            get;
            set;
        }
    }
}
