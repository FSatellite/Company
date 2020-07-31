/**
 * 服务调用
 * Created by zyct-yt on 2018/3/2.
 */
var services = {
    call: function (type, url, data, success,serverError, complete) {
        var param ={
            type: type,   //访问WebService使用Post方式请求
            url: url,     //调用WebService
            dataType: 'JSON',
            data: data,
            // contentType:'application/json;charset=utf-8',
            headers: {
                'x-auth-token': x_auth_token
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            error: function (x, e) {
                if (serverError!=null) {
                    serverError({code: "error", msg: "接口调用异常，请联系管理员。异常信息：" + JSON.stringify(e), data: null});
                } else {
                    if(JSON.stringify(e).indexOf("登录")!=-1){
                        window.location.href = "login.html";
                    }
                    $.messager.show({
                        title: '服务提示',
                        msg: '接口调用异常，请联系管理员。异常信息：' + JSON.stringify(e),
                        timeout: 5000,
                        showType: 'slide',
                        height: 108
                    });
                }
            },
            success: function (data, textStatus) { //回调函数，result，返回值
                if ("success" == data.code) {
                    success(data);
                } else if (serverError!=null) {
                    serverError(data);
                } else {
                    if(data.msg.indexOf("登录")!=-1){
                        window.location.href = "login.html";
                    }
                    $.messager.show({
                        title: '服务提示',
                        msg: '服务器遇到异常，请联系管理员。异常信息：' + data.msg,
                        timeout: 5000,
                        showType: 'slide',
                        height: 108
                    });
                }
            },
            complete: function(XMLHttpRequest, status) { //请求完成后最终执行参数　
                if(complete){
                    complete(XMLHttpRequest, status);
                }
            }
        };
        $.ajax(param);
    },
    get:function (url,data,success,serverError) {
        this.getCommon(true,url,data,success,serverError);
    },
	getCommon:function(async,url,data,success,serverError){
		var param={
            url: url,
            type: "GET",
            data:data,
			async: async,
            dataType: 'JSON',
            success: function (result) {
                if ("success" == result.code) {
                    success(result);
                } else if (serverError!=null) {
                    serverError(result);
                } else {
                    $.messager.show({
                        title: '服务提示',
                        msg: '服务器遇到异常，请联系管理员。异常信息：' + result.msg,
                        timeout: 5000,
                        showType: 'slide',
                        height: 108
                    });
                }
            },error: function (x, e) {
                console.log(x);
                console.log(e);
                if (serverError!=null) {
                    serverError({code: "error", msg: "接口调用异常，请联系管理员。异常信息：" + JSON.stringify(e), data: null});
                } else {
                    if(JSON.stringify(e).indexOf("登录")!=-1){
                        window.location.href = "login.html";
                    }
                    $.messager.show({
                        title: '服务提示',
                        msg: '接口调用异常，请联系管理员。异常信息：' + JSON.stringify(e),
                        timeout: 5000,
                        showType: 'slide',
                        height: 108
                    });
                }
            },
        };
        $.ajax(param);
    },
    getAsync:function (url,data,success,serverError) {
        this.getCommon(true,url,data,success,serverError);
    },
	postCommon:function(ansys,url,data,success,serverError,contentType){
		var param={
            url: url,
            async: ansys,
            type: "POST",
            data:data,
            dataType: 'JSON',
            success: function (result) {
                if ("success" == result.code) {
                    success(result);
                } else if (serverError!=null) {
                    serverError(result);
                } else {
                    $.messager.show({
                        title: '服务提示',
                        msg: '服务器遇到异常，请联系管理员。异常信息：' + result.msg,
                        timeout: 5000,
                        showType: 'slide',
                        height: 108
                    });
                }
            }, error: function (x, e) {
                if (serverError!=null) {
                    serverError({code: "error", msg: "接口调用异常，请联系管理员。异常信息：" + JSON.stringify(e), data: null});
                } else {
                    if(JSON.stringify(e).indexOf("登录")!=-1){
                        window.location.href = "login.html";
                    }
                    $.messager.show({
                        title: '服务提示',
                        msg: '接口调用异常，请联系管理员。异常信息：' + JSON.stringify(e),
                        timeout: 5000,
                        showType: 'slide',
                        height: 108
                    });
                }
            },
        };
        if(contentType){
            delete param.dataType;
            param["contentType"]= "application/json";
        }
        $.ajax(param);
	},
    post:function (url,data,success,serverError,contentType) {
       this.postCommon(true,url,data,success,serverError,contentType);
    },
    postSync:function (url,data,success,serverError,contentType) {
        this.postCommon(false,url,data,success,serverError,contentType);
    },
    postAsync:function (url,data,success,serverError,contentType) {
       this.postCommon(true,url,data,success,serverError,contentType);
    },
    //获取url中的参数
    getQueryString: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); // 匹配目标参数
        var result = window.location.search.substr(1).match(reg); // 对querystring匹配目标参数
        if (result != null) {
            return decodeURIComponent(result[2]);
        } else {
            return null;
        }
    }
};