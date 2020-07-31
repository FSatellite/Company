#include "ZhuQiVolume.h"
#include <cstring>

#define DOUBLE_PRECISION 0.000001//浮点型数据与0作比较

//小程序的接口参数结构体，其子成员是每个接口参数（根据小程序配置器自动生成）
struct ParamStruct
{
	double k;//余量系数
	double m;//贮箱外排气体总质量
	double Densityq;//贮气密度
	double M;//增压气体介质分子量
	double Pj;//贮气装置余压
	double Pt;//贮箱最高气枕压力
	double R;//气体常数
	double Tj;//贮气装置温度
	double Tt;//贮箱气枕平均温度
	double Vt;//推进剂加注量
	double Vq;//贮气装置容积
};

//接口参数结构体实例化对象
ParamStruct paramStruct;

//程序初始化
bool __stdcall FmiInitializeSlave()
{
	paramStruct.k = 4;
	paramStruct.m = 0;
	paramStruct.Densityq = 33.367;
	paramStruct.M = 4;
	paramStruct.Pj = 5000000;
	paramStruct.Pt = 400000;
	paramStruct.R = 8314;
	paramStruct.Tj = 300;
	paramStruct.Tt = 120;
	paramStruct.Vt = 3.8;
	paramStruct.Vq = 0.29;
	return true;
}

//参数赋值
bool __stdcall FmiSetReal(char *name,double value)
{
	string paramName = name;
	if (paramName == "k")
	{
		paramStruct.k = value;
	}
	if (paramName == "m")
	{
		paramStruct.m = value;
	}
	if (paramName == "Densityq")
	{
		paramStruct.Densityq = value;
	}
	if (paramName == "M")
	{
		paramStruct.M = value;
	}
	if (paramName == "Pj")
	{
		paramStruct.Pj = value;
	}
	if (paramName == "Pt")
	{
		paramStruct.Pt = value;
	}
	if (paramName == "R")
	{
		paramStruct.R = value;
	}
	if (paramName == "Tj")
	{
		paramStruct.Tj = value;
	}
	if (paramName == "Tt")
	{
		paramStruct.Tt = value;
	}
	if (paramName == "Vt")
	{
		paramStruct.Vt = value;
	}
	if (paramName == "Vq")
	{
		paramStruct.Vq = value;
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
	if (paramName == "k")
	{
		char *temp = DoubleToChar(paramStruct.k);
		strcpy(value, temp);
	}
	else if (paramName == "m")
	{
		char *temp = DoubleToChar(paramStruct.m);
		strcpy(value, temp);
	}
	else if (paramName == "Densityq")
	{
		char *temp = DoubleToChar(paramStruct.Densityq);
		strcpy(value, temp);
	}
	else if (paramName == "M")
	{
		char *temp = DoubleToChar(paramStruct.M);
		strcpy(value, temp);
	}
	else if (paramName == "Pj")
	{
		char *temp = DoubleToChar(paramStruct.Pj);
		strcpy(value, temp);
	}
	else if (paramName == "Pt")
	{
		char *temp = DoubleToChar(paramStruct.Pt);
		strcpy(value, temp);
	}
	else if (paramName == "R")
	{
		char *temp = DoubleToChar(paramStruct.R);
		strcpy(value, temp);
	}
	else if (paramName == "Tj")
	{
		char *temp = DoubleToChar(paramStruct.Tj);
		strcpy(value, temp);
	}
	else if (paramName == "Tt")
	{
		char *temp = DoubleToChar(paramStruct.Tt);
		strcpy(value, temp);
	}
	else if (paramName == "Vt")
	{
		char *temp = DoubleToChar(paramStruct.Vt);
		strcpy(value, temp);
	}
	else if (paramName == "Vq")
	{
		char *temp = DoubleToChar(paramStruct.Vq);
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