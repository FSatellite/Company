#include "WingsGeomParamCal.h"

#define DOUBLE_PRECISION 0.000001//浮点型数据与0作比较

//小程序的接口参数结构体，其子成员是每个接口参数（根据小程序配置器自动生成）
struct ParamStruct
{
	double m;//导弹质量
	double g;//重力加速度
	double density;//空气密度
	double V;//远场速度
	double l;//翼展
	double CL;//升力系数
	double rtRatio;//梢根比
	double S;//翼面积
	double aspectRatio;//展弦比
	double b0;//翼根弦长
	double b1;//翼尖弦长
};

//接口参数结构体实例化对象
ParamStruct paramStruct;

//程序初始化
bool __stdcall FmiInitializeSlave()
{
	paramStruct.m = 2.2;
	paramStruct.g = 9.8;
	paramStruct.density = 1.225;
	paramStruct.V = 238;
	paramStruct.l = 2.5;
	paramStruct.CL = 0.8;
	paramStruct.rtRatio = 3;
	paramStruct.S = 0.78;
	paramStruct.aspectRatio = 8;
	paramStruct.b0 = 0.47;
	paramStruct.b1 = 0.17;
	return true;
}

//参数赋值
bool __stdcall FmiSetReal(char *name,double value)
{
	char chParam[256];
	strncpy(chParam, name, strlen(name) + 1);
	if (strcmp(chParam,"m") == 0)
	{
		paramStruct.m = value;
	}
	if (strcmp(chParam,"g") == 0)
	{
		paramStruct.g = value;
	}
	if (strcmp(chParam,"density") == 0)
	{
		paramStruct.density = value;
	}
	if (strcmp(chParam,"V") == 0)
	{
		paramStruct.V = value;
	}
	if (strcmp(chParam,"l") == 0)
	{
		paramStruct.l = value;
	}
	if (strcmp(chParam,"CL") == 0)
	{
		paramStruct.CL = value;
	}
	if (strcmp(chParam,"rtRatio") == 0)
	{
		paramStruct.rtRatio = value;
	}
	if (strcmp(chParam,"S") == 0)
	{
		paramStruct.S = value;
	}
	if (strcmp(chParam,"aspectRatio") == 0)
	{
		paramStruct.aspectRatio = value;
	}
	if (strcmp(chParam,"b0") == 0)
	{
		paramStruct.b0 = value;
	}
	if (strcmp(chParam,"b1") == 0)
	{
		paramStruct.b1 = value;
	}
	return true;
}

//计算函数（此函数为主要的计算公式函数）
	/************************************************************************/
	
	/************************************************************************/
bool __stdcall FMIRunSlave(char *info)
{
        //获取dll执行路径
        char filePath[255];
        GetModuleFileNameA(GetModuleHandleA("WingsGeomParamCal.dll"),filePath,255);
        int nStrNum = 0;
        char *resultStr[128] = {0};
	nStrNum = split(filePath,'\\',resultStr);
         //split(filePath,"\\",resultStr,&nStrNum);
         for (int i = 0; i < nStrNum - 1; i++)
        {
            char tempStr[128];
            strcpy(tempStr,resultStr[i]);
            if (i == 0)
            {
                strcpy(filePath,tempStr);
            }
            else
            {
                strcat(filePath,"\\");
                strcat(filePath,tempStr);
            }
        }
        //写输入文件
        char fileInputPath[255];//输入文件路径
        strcpy(fileInputPath,filePath);
        strcat(fileInputPath,"\\paras.in");
        FILE *file = NULL;
        file = fopen(fileInputPath,"r");
    
	return true;
}

int split(char *src, char c, char **res)
{
	int i, len, j, k;
	len = strlen(src);
	char tmp;
	for(k = j = i = 0;i < len;i++)
	{
		if(c == src[i])
		{
			res[k] = (char *)malloc(i-j+1);
			tmp = src[i];
			src[i] = 0;
			strcpy(res[k++], src+j);
			j = i+1;
			src[i] = tmp;
		}
	}
	res[k] = (char *)malloc(i-j+1);
	strcpy(res[k++], src+j);
	return k;
}

//返回计算结果
bool __stdcall FmiGetReal(char *name, char *value)
{
	char chParam[256];
	strncpy(chParam, name, strlen(name) + 1);
	if(strcmp(chParam,"m") == 0)
	{
		char *temp = DoubleToChar(paramStruct.m);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"g") == 0)
	{
		char *temp = DoubleToChar(paramStruct.g);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"density") == 0)
	{
		char *temp = DoubleToChar(paramStruct.density);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"V") == 0)
	{
		char *temp = DoubleToChar(paramStruct.V);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"l") == 0)
	{
		char *temp = DoubleToChar(paramStruct.l);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"CL") == 0)
	{
		char *temp = DoubleToChar(paramStruct.CL);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"rtRatio") == 0)
	{
		char *temp = DoubleToChar(paramStruct.rtRatio);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"S") == 0)
	{
		char *temp = DoubleToChar(paramStruct.S);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"aspectRatio") == 0)
	{
		char *temp = DoubleToChar(paramStruct.aspectRatio);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"b0") == 0)
	{
		char *temp = DoubleToChar(paramStruct.b0);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"b1") == 0)
	{
		char *temp = DoubleToChar(paramStruct.b1);
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