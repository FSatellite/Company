#include "LiquidLevelCal.h"

#define DOUBLE_PRECISION 0.000001//浮点型数据与0作比较

//小程序的接口参数结构体，其子成员是每个接口参数（根据小程序配置器自动生成）
struct ParamStruct
{
	double s1_Volume;//芯一级氧箱体积
	double s2_Volume;//芯一级燃箱体积
	double s3_Volume;//芯二级氧箱体积
	double s4_Volume;//芯二级燃箱体积
	double s5_Volume;//助推氧箱
	double s6_Volume;//助推燃箱
	double s1_Height;//高度1
	double s2_Height;//高度2
	double s3_Height;//高度3
	double s4_Height;//高度4
	double s5_Height;//高度5
	double s6_Height;//高度6
};

//接口参数结构体实例化对象
ParamStruct paramStruct;

//程序初始化
bool __stdcall FmiInitializeSlave()
{
	paramStruct.s1_Volume = 30000;
	paramStruct.s2_Volume = 30000;
	paramStruct.s3_Volume = 30000;
	paramStruct.s4_Volume = 40000;
	paramStruct.s5_Volume = 50000;
	paramStruct.s6_Volume = 60000;
	paramStruct.s1_Height = 0;
	paramStruct.s2_Height = 0;
	paramStruct.s3_Height = 0;
	paramStruct.s4_Height = 0;
	paramStruct.s5_Height = 0;
	paramStruct.s6_Height = 0;
	return true;
}

//参数赋值
bool __stdcall FmiSetReal(char *name,double value)
{
	char chParam[256];
	strncpy(chParam, name, strlen(name) + 1);
	if (strcmp(chParam,"s1_Volume") == 0)
	{
		paramStruct.s1_Volume = value;
	}
	if (strcmp(chParam,"s2_Volume") == 0)
	{
		paramStruct.s2_Volume = value;
	}
	if (strcmp(chParam,"s3_Volume") == 0)
	{
		paramStruct.s3_Volume = value;
	}
	if (strcmp(chParam,"s4_Volume") == 0)
	{
		paramStruct.s4_Volume = value;
	}
	if (strcmp(chParam,"s5_Volume") == 0)
	{
		paramStruct.s5_Volume = value;
	}
	if (strcmp(chParam,"s6_Volume") == 0)
	{
		paramStruct.s6_Volume = value;
	}
	if (strcmp(chParam,"s1_Height") == 0)
	{
		paramStruct.s1_Height = value;
	}
	if (strcmp(chParam,"s2_Height") == 0)
	{
		paramStruct.s2_Height = value;
	}
	if (strcmp(chParam,"s3_Height") == 0)
	{
		paramStruct.s3_Height = value;
	}
	if (strcmp(chParam,"s4_Height") == 0)
	{
		paramStruct.s4_Height = value;
	}
	if (strcmp(chParam,"s5_Height") == 0)
	{
		paramStruct.s5_Height = value;
	}
	if (strcmp(chParam,"s6_Height") == 0)
	{
		paramStruct.s6_Height = value;
	}
	return true;
}

//计算函数（此函数为主要的计算公式函数）
	/************************************************************************/
	
	/************************************************************************/
bool __stdcall FMIRunSlave(char *info)
{
        //获取DLL执行路径
        char filePath[255];
        GetModuleFileNameA(GetModuleHandleA("LiquidLevelCal.dll"),filePath,255);
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
        //修改输入文件
        char fileInputPath[255];//输入文件路径
        strcpy(fileInputPath,filePath);
        strcat(fileInputPath,"\\r.txt");
    
        char tempFileInputPath[255];//临时输入文件地址
        strcpy(tempFileInputPath,filePath);
        strcat(tempFileInputPath,"\\tmp.txt");
    
        char linedata[256] = {0};
        FILE *fp = fopen(fileInputPath,"r");
	FILE *fpw = fopen(tempFileInputPath,"w");
	int nFlag = 0;
	while (fgets(linedata,sizeof(linedata) - 1,fp) != NULL)
	{
		int strLength = UTF8ToGBK(linedata,NULL,NULL);
		char *templinedata = (char *)malloc(strLength + 1);
		UTF8ToGBK(linedata,templinedata,strLength);
		char *tempStr = (char *)malloc(strlen(templinedata) + 32);

		if (nFlag == 0)
		{
			strcpy(tempStr,templinedata);
		}
		else
		{
			int nStrNum = 0;
			char *resultStr[128] = {0};
			//split(templinedata,",",resultStr,&nStrNum);
			nStrNum = split(templinedata,',',resultStr);
			for (int i = 0; i < nStrNum - 1; i++)
			{
				if (i == 0)
					strcpy(tempStr,resultStr[i]);
				else
				{
					strcat(tempStr,",");
					strcat(tempStr,resultStr[i]);
				}
			}
			strcat(tempStr,",");
                        if (nFlag == 1)
                            strcat(tempStr,DoubleToChar(paramStruct.s1_Volume));
                        if (nFlag == 2)
                            strcat(tempStr,DoubleToChar(paramStruct.s2_Volume));
                        if (nFlag == 3)
                            strcat(tempStr,DoubleToChar(paramStruct.s3_Volume));
                        if (nFlag == 4)
                            strcat(tempStr,DoubleToChar(paramStruct.s4_Volume));
                        if (nFlag == 5)
                            strcat(tempStr,DoubleToChar(paramStruct.s5_Volume));
                        if (nFlag == 6)
                            strcat(tempStr,DoubleToChar(paramStruct.s6_Volume));
			strcat(tempStr,"\n");
		}

		//写入文件
		strLength = GBKToUTF8(tempStr,NULL,NULL);
		char *utf8Str = (char *)malloc(strLength + 1);
		GBKToUTF8(tempStr,utf8Str,strLength + 1);
		fputs(utf8Str,fpw);
		nFlag++;
        }
        //关闭文件
	fclose(fp);
	fclose(fpw);
	remove(fileInputPath);
	rename(tempFileInputPath,fileInputPath);
        
        //运行程序
        char exeFilePath[255];//运行文件路径
        strcpy(exeFilePath,filePath);
        strcat(exeFilePath,"\\V-H.exe");
    
        char resultFilePath[255];//输出文件路径
        strcpy(resultFilePath,filePath);
        strcat(resultFilePath,"\\result.txt");
    
        char cmdStr[1024];
        strcpy(cmdStr,exeFilePath);
        strcat(cmdStr," ");
        strcat(cmdStr,fileInputPath);
        strcat(cmdStr," ");
        strcat(cmdStr,resultFilePath);
        system(cmdStr);
        
        //读取输出结果
        FILE *fpRead = fopen(resultFilePath,"r");
        nFlag = 0;
        while (fgets(linedata,sizeof(linedata) - 1,fpRead) != NULL)
	{
            int strLength = UTF8ToGBK(linedata,NULL,NULL);
            char *templinedata = (char *)malloc(strLength + 1);
            UTF8ToGBK(linedata,templinedata,strLength);

            int nStrNum = 0;
            char *resultStr[128] = {0};
			nStrNum = split(templinedata,',',resultStr);
            //split(templinedata,",",resultStr,&nStrNum);
            for (int i = 0; i < nStrNum; i++)
            {
                if (nFlag == 0)
                    paramStruct.s1_Height = strtod(resultStr[i],NULL);
                if (nFlag == 1)
                    paramStruct.s2_Height = strtod(resultStr[i],NULL);
                if (nFlag == 2)
                    paramStruct.s3_Height = strtod(resultStr[i],NULL);
                if (nFlag == 3)
                    paramStruct.s4_Height = strtod(resultStr[i],NULL);
                if (nFlag == 4)
                    paramStruct.s5_Height = strtod(resultStr[i],NULL);
                if (nFlag == 5)
                    paramStruct.s6_Height = strtod(resultStr[i],NULL);
            }
			nFlag++;
        }
        fclose(fpRead);
        
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

 //UTF8编码转换到GBK编码
int UTF8ToGBK(char * lpUTF8Str,char * lpGBKStr,int nGBKStrLen)
{
	wchar_t * lpUnicodeStr = NULL;
	int nRetLen = 0;

	if(!lpUTF8Str)  //如果UTF8字符串为NULL则出错退出
		return 0;

	nRetLen = ::MultiByteToWideChar(CP_UTF8,0,(char *)lpUTF8Str,-1,NULL,NULL);  //获取转换到Unicode编码后所需要的字符空间长度
	lpUnicodeStr = new WCHAR[nRetLen + 1];  //为Unicode字符串空间
	nRetLen = ::MultiByteToWideChar(CP_UTF8,0,(char *)lpUTF8Str,-1,lpUnicodeStr,nRetLen);  //转换到Unicode编码
	if(!nRetLen)  //转换失败则出错退出
		return 0;

	nRetLen = ::WideCharToMultiByte(CP_ACP,0,lpUnicodeStr,-1,NULL,NULL,NULL,NULL);  //获取转换到GBK编码后所需要的字符空间长度

	if(!lpGBKStr)  //输出缓冲区为空则返回转换后需要的空间大小
	{
		if(lpUnicodeStr)
			delete []lpUnicodeStr;
		return nRetLen;
	}

	if(nGBKStrLen < nRetLen)  //如果输出缓冲区长度不够则退出
	{
		if(lpUnicodeStr)
			delete []lpUnicodeStr;
		return 0;
	}

	nRetLen = ::WideCharToMultiByte(CP_ACP,0,lpUnicodeStr,-1,(char *)lpGBKStr,nRetLen,NULL,NULL);  //转换到GBK编码

	if(lpUnicodeStr)
		delete []lpUnicodeStr;

	return nRetLen;
}

int GBKToUTF8(char * lpGBKStr,char * lpUTF8Str,int nUTF8StrLen)
{
	wchar_t * lpUnicodeStr = NULL;
	int nRetLen = 0;

	if(!lpGBKStr)  //如果GBK字符串为NULL则出错退出
		return 0;

	nRetLen = ::MultiByteToWideChar(CP_ACP,0,(char *)lpGBKStr,-1,NULL,NULL);  //获取转换到Unicode编码后所需要的字符空间长度
	lpUnicodeStr = new WCHAR[nRetLen + 1];  //为Unicode字符串空间
	nRetLen = ::MultiByteToWideChar(CP_ACP,0,(char *)lpGBKStr,-1,lpUnicodeStr,nRetLen);  //转换到Unicode编码
	if(!nRetLen)  //转换失败则出错退出
		return 0;

	nRetLen = ::WideCharToMultiByte(CP_UTF8,0,lpUnicodeStr,-1,NULL,0,NULL,NULL);  //获取转换到UTF8编码后所需要的字符空间长度

	if(!lpUTF8Str)  //输出缓冲区为空则返回转换后需要的空间大小
	{
		if(lpUnicodeStr)
			delete []lpUnicodeStr;
		return nRetLen;
	}

	if(nUTF8StrLen < nRetLen)  //如果输出缓冲区长度不够则退出
	{
		if(lpUnicodeStr)
			delete []lpUnicodeStr;
		return 0;
	}

	nRetLen = ::WideCharToMultiByte(CP_UTF8,0,lpUnicodeStr,-1,(char *)lpUTF8Str,nUTF8StrLen,NULL,NULL);  //转换到UTF8编码

	if(lpUnicodeStr)
		delete []lpUnicodeStr;

	return nRetLen;
}

//返回计算结果
bool __stdcall FmiGetReal(char *name, char *value)
{
	char chParam[256];
	strncpy(chParam, name, strlen(name) + 1);
	if(strcmp(chParam,"s1_Volume") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s1_Volume);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s2_Volume") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s2_Volume);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s3_Volume") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s3_Volume);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s4_Volume") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s4_Volume);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s5_Volume") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s5_Volume);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s6_Volume") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s6_Volume);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s1_Height") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s1_Height);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s2_Height") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s2_Height);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s3_Height") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s3_Height);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s4_Height") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s4_Height);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s5_Height") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s5_Height);
		strcpy(value, temp);
	}
	else if(strcmp(chParam,"s6_Height") == 0)
	{
		char *temp = DoubleToChar(paramStruct.s6_Height);
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