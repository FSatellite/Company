#ifndef __ZhuQiVolume_h__
#define __ZhuQiVolume_h__

#include <string>
using namespace std;

char *DoubleToChar(double a);

//初始化函数
extern "C" __declspec(dllexport) bool __stdcall FmiInitializeSlave();
extern "C" __declspec(dllexport) bool __stdcall FmiInitializeSlaveWithFile(string file);

//设置参数值
extern "C" __declspec(dllexport) bool __stdcall FmiSetReal(char *name,double value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetBool(char *name, bool value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetString(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetInt(char *name, int value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetRealArray(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetStringArray(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetIntArray(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetRealArray2(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetIntArray2(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiSetStringArray2(char *name, char *value);

//获取参数值
extern "C" __declspec(dllexport) bool __stdcall FmiGetReal(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetBool(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetInt(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetString(char* name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetIntArray(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetRealArray(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetStringArray(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetIntArray2(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetStringArray2(char *name, char *value);
extern "C" __declspec(dllexport) bool __stdcall FmiGetRealArray2(char *name, char *value);

//计算
extern "C" __declspec(dllexport) bool __stdcall FMIRunSlave(char *info);

//显示图片
extern "C" __declspec(dllexport) bool __stdcall FMIShowPicture();

//结束运行
extern "C" __declspec(dllexport) bool __stdcall FMIFreeSlaveInstance();

//显示运行日志
extern "C" __declspec(dllexport) bool __stdcall FMIShowLog();

#endif
