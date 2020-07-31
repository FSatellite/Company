/**
 * Created by 82766 on 2018/12/9.
 */
var config = {
    /*客户端地址*/
    AppToolsClient_Addr: "http://localhost:8082/AppToolsClient",
    portal_Addr: "https://122.112.218.36:8081/portal",       //权限服务地址
    KnowledgeApp_Addr:"http://localhost:8020",
    /*前端地址*/
    smallProgramClient_Addr: "http://localhost:8082/smallProgramClient/",
    //调用对象  平台(ShareWorks)/工具解析器(ToolResolver)
    callTool:""
};
//是否显示
var pageNum = 1;
var pageSize = 10000;
var defaultSize = 3;
//全局token;
var x_auth_token = "";
//当前登录用户
var loginUser;
var toolsInfo = {
    // 专业
    "category": [
        {
            name: "总体专业",
            id: "Z"
        },
        {
            name: "弹道与制导专业",
            id: "B"
        },
        {
            name: "姿控专业",
            id: "K"
        },
        {
            name: "电气专业",
            id: "P"
        },
        {
            name: "动力专业",
            id: "D"
        },
        {
            name: "力学环境专业",
            id: "L"
        },
        {
            name: "热环境专业",
            id: "R"
        },
        {
            name: "结构与分离专业",
            id: "J"
        }
    ],
    //原理依据
    "principleType": [
        {
            name: "行业规范",
            id: "HYGF"
        },
        {
            name: "岗位手册",
            id: "GWSC"
        },
        {
            name: "工程经验",
            id: "GCJY"
        }
    ],
    //功能分类
    "applicationType": [
        {
            name: "数据计算",
            id: "Caculate"
        },
        {
            name: "数据查询",
            id: "Search"
        },
        {
            name: "报表类",
            id: "Report"
        },
        {
            name:"Proe模型",
            id:"Proe"
        }
    ],
    //程序分类
    "moduleCategory": [
        {
            name: "增压系统计算模块",
            id: "SuperChargerSystem"
        },
        {
            name: "需要增压压力计算模块",
            id: "NeedToAddPressure"
        },
        {
            name: "管路参数计算及重量估算",
            id: "EstimateWeightOfPipe"
        },
        {
            name: "气液管路流阻计算模块",
            id: "DragForceOfPipe"
        },
        {
            name: "孔板流量计算模块",
            id: "FlowOfPlate"
        },
        {
            name: "可靠性指标分配及评估模块",
            id: "ReliabilityAnalysis"
        },
        {
            name: "数据处理及分析模块",
            id: "DataAnalysis"
        },
        {
            name:"自研程序",
            id:"SelfDevelopedProcedure"
        },
        {
            name:"商软模型",
            id:"CommercialSoftModel"
        }
    ],
    //程序开发语言
    "generationTool": [
        {name: "C", id: "C"},
        {name: "C++", id: "C++"},
        {name: "C#", id: "C#"},
        {name: "Java", id: "Java"},
        {name: "Python", id: "Python"},
        {name: "Matlab", id: "Matlab"}
    ],
    //程序类型
    "programType": [
        {name: "exe", id: "exe"},
        {name: "dll", id: "dll"}
    ]
};

var htmlStr="";

