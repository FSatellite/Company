#include "ZhuQiVolume.h"

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
	char chParam[256];
	strncpy(chParam, name, strlen(name) + 1);
	if (strcmp(chParam,"k") == 0)
	{
		paramStruct.k = value;
	}
	if (strcmp(chParam,"m") == 0)
	{
		paramStruct.m = value;
	}
	if (strcmp(chParam,"Densityq") == 0)
	{
		paramStruct.Densityq = value;
	}
	if (strcmp(chParam,"M") == 0)
	{
		paramStruct.M = value;
	}
	if (strcmp(chParam,"Pj") == 0)
	{
		paramStruct.Pj = value;
	}
	if (strcmp(chParam,"Pt") == 0)
	{
		paramStruct.Pt = value;
	}
	if (strcmp(chParam,"R") == 0)
	{
		paramStruct.R = value;
	}
	if (strcmp(chParam,"Tj") == 0)
	{
		paramStruct.Tj = value;
	}
	if (strcmp(chParam,"Tt") == 0)
	{
		paramStruct.Tt = value;
	}
	if (strcmp(chParam,"Vt") == 0)
	{
		paramStruct.Vt = value;
	}
	if (strcmp(chParam,"Vq") == 0)
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
    //计算分子
	double dNumerator = (paramStruct.Pt * paramStruct.Vt * paramStruct.M) / (paramStruct.R * paramStruct.Tt) + paramStruct.m;
	//计算分母
	double dDenominator = paramStruct.Densityq - (paramStruct.Pj * paramStruct.M) / (paramStruct.R * paramStruct.Tj);
	//计算贮气容积
	paramStruct.Vq = dNumerator / dDenominator * paramStruct.k;
	return true;
}

//返回计算结果
bool __stdcall FmiGetReal(char *name, char *value)
{
	char chParam[256];
	strncpy(chParam, name, strlen(name) + 1);
	if(strcmp(chParam,"k") == 0)
	{
		char *temp = DoubleToChar(paramStruct.k);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"m") == 0)
	{
		char *temp = DoubleToChar(paramStruct.m);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"Densityq") == 0)
	{
		char *temp = DoubleToChar(paramStruct.Densityq);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"M") == 0)
	{
		char *temp = DoubleToChar(paramStruct.M);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"Pj") == 0)
	{
		char *temp = DoubleToChar(paramStruct.Pj);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"Pt") == 0)
	{
		char *temp = DoubleToChar(paramStruct.Pt);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"R") == 0)
	{
		char *temp = DoubleToChar(paramStruct.R);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"Tj") == 0)
	{
		char *temp = DoubleToChar(paramStruct.Tj);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"Tt") == 0)
	{
		char *temp = DoubleToChar(paramStruct.Tt);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"Vt") == 0)
	{
		char *temp = DoubleToChar(paramStruct.Vt);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"Vq") == 0)
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
	char *c = (char *)malloc(64 * sizeof(char)); //double 64位
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