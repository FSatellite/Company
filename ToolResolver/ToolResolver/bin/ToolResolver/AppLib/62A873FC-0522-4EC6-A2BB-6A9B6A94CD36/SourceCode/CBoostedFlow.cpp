#include "CBoostedFlow.h"
#include <cstring>

#define DOUBLE_PRECISION 0.000001//浮点型数据与0作比较

//小程序的接口参数结构体，其子成员是每个接口参数（根据小程序配置器自动生成）
struct ParamStruct
{
	double Pz;//贮箱气枕压力
	double Tz;//贮箱气枕温度
	double Qt;//发动机消耗的推进剂流量
	double R;//气体常数
	double M;//增压气体介质的分子量
	double q;//增压流量
};

//接口参数结构体实例化对象
ParamStruct paramStruct;

//程序初始化
bool __stdcall FmiInitializeSlave()
{
	paramStruct.Pz = 0;
	paramStruct.Tz = 0;
	paramStruct.Qt = 0;
	paramStruct.R = 8314;
	paramStruct.M = 0;
	paramStruct.q = 0;
	return true;
}

//参数赋值
bool __stdcall FmiSetReal(char *name,double value)
{
	string paramName = name;
	if (paramName == "Pz")
	{
		paramStruct.Pz = value;
	}
	if (paramName == "Tz")
	{
		paramStruct.Tz = value;
	}
	if (paramName == "Qt")
	{
		paramStruct.Qt = value;
	}
	if (paramName == "R")
	{
		paramStruct.R = value;
	}
	if (paramName == "M")
	{
		paramStruct.M = value;
	}
	if (paramName == "q")
	{
		paramStruct.q = value;
	}
	return true;
}

//计算函数（此函数为主要的计算公式函数）
	/************************************************************************/
	
	/************************************************************************/
bool __stdcall FMIRunSlave(char *info)
{
	return true;
}

//返回计算结果
bool __stdcall FmiGetReal(char *name, char *value)
{
	string paramName = name;
	if (paramName == "Pz")
	{
		char *temp = DoubleToChar(paramStruct.Pz);
		strcpy(value, temp);
	}
	else if (paramName == "Tz")
	{
		char *temp = DoubleToChar(paramStruct.Tz);
		strcpy(value, temp);
	}
	else if (paramName == "Qt")
	{
		char *temp = DoubleToChar(paramStruct.Qt);
		strcpy(value, temp);
	}
	else if (paramName == "R")
	{
		char *temp = DoubleToChar(paramStruct.R);
		strcpy(value, temp);
	}
	else if (paramName == "M")
	{
		char *temp = DoubleToChar(paramStruct.M);
		strcpy(value, temp);
	}
	else if (paramName == "q")
	{
		char *temp = DoubleToChar(paramStruct.q);
		strcpy(value, temp);
	}
	else
	{
		return false;
	}
	return true;
}

//释放当前小程序实例
bool __stdcall FMIFreeSlaveInstance()
{
	return true;
}

//将Double数据转为字符型
char *DoubleToChar(double a)
{
	char *c=new char[64]; //double 64位
	int b=(int)a;
	int i=0;
	int flag=1;
	char z;     //处理整数部分
	if(b!=0)
	{
		if(b<0)
		{
			c[i++]='-';
			b=0-b;
			flag=-1;
		}
		while(b)
		{
			c[i++]=b%10+'0';
			b/=10;
		}
		int j=0;
		if(flag==-1)
		{
			j=1;
		}
		int k=0;  //控制负数
		for(;j<i/2;j++,k++)
		{
			z=c[j];
			c[j]=c[i-k-1];
			c[i-k-1]=z;
		}
	}
	else
		c[i++]='0';
	c[i++]='.';     //处理小数部分  8位小数

	if(flag==-1)
	{
		a=0-a;
	}

	double d=a-(int)a;
	int t=0;
	int n=0;
	if(d!=0.0)
	{
		while((d<1)&&(n<8))    //小数位数留8位
		{
			d *= 10;
			t=int(d);
			c[i++]=t+'0';
			d=d-t;
			n++;
		}
		c[i++]=d;
	}
	else if(0.0==d)
		c[i++]='0';
	c[i]='\0';
	return c;
}