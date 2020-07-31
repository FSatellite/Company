var express = require('express');
const redis = require('redis');
var URL = require('url');
var path = require('path');
var favicon = require('serve-favicon');
var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var config = require("./modules/SysConfig");
var index = require('./routes/index');
var moduleCategory = require('./routes/moduleCategory');
var model = require('./routes/model');
var file = require('./routes/gridFile');
var downInfo = require('./routes/downInfo');
var authority = require('./routes/authority');
var app = express();

var client = redis.createClient(6379, "192.168.0.254",{auth_pass:"knowind"});
client.on("error", function (err) {
    console.log("Error " + err);
});
// view engine setup
app.set('views', path.join(__dirname, 'views'));
var ejs = require('ejs');
app.engine('html', ejs.__express);
app.set('view engine', 'html');
//---设置允许跨域请求
app.all('*', function (req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "x-requested-with,Authorization,x-auth-token,Set-Cookie");
    res.header("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS");
    //res.header("X-Powered-By", ' 3.2.1');
    res.header("Access-Control-Max-Age", "3600");
    res.header("Access-Control-Allow-Credentials","true");
    //res.header("Content-Type", "application/json;charset=utf-8");
    if (req.method == 'OPTIONS') {
        res.send(200);
    }
    else {
        next();
    }
});
// uncomment after placing your favicon in /public
//app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
app.use(logger('dev'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));
app.use(function (req, res, next) {
    //获取请求头中的信息
    var headers = req.headers;
    var x_auth_token;
    x_auth_token = headers["x-auth-token"];
    if(!x_auth_token){
        var referer = headers.referer;
        if(referer){
            var tokenIndex = referer.indexOf("x-auth-token=");
            if(tokenIndex>=0){
                x_auth_token=referer.substr(tokenIndex+13,36);
            }
        }
        //如果请求头中没有token，从地址栏中获取
        if(!x_auth_token){
            var param = URL.parse(req.url, true).query;
            x_auth_token = param["x-auth-token"];
        }
    }
    console.log("x_auth_token:" + x_auth_token);
    if (!x_auth_token) {
        res.send({code: "error", data: null, msg: "未登录，请登录"});
    } else {
        client.get(x_auth_token, function (err, value) {
            if(err){
                res.send({code: "error", data: null, msg: err});
            }
            if(value) {
                client.expire(x_auth_token, 18000, function(timeErr, isSuccess){
                    if (timeErr || !isSuccess) {
                    //your code
                } else {
                    console.log("重新设置时间");
                }
            });
                var index = value.indexOf("{");
                var jsonStr = value.substr(index);
                var json = eval('(' + jsonStr + ')');
                config.loginUser = {
                    user: {
                        id: json.id,
                        username: json.username,
                        truename: json.truename,
                        idCard: json.idCard,
                        address: json.address,
                        phone: json.phone,
                        telphone: json.telphone,
                        email: json.email,
                        security: json.security,
                        isDelete: json.isDelete,
                        isDisable: json.isDisable,
                        depId: json.depId,
                        depName: json.depName
                    },
                    roles: json.roles,
                    rights: json.rightsList
                };
                next();
            }else{
                res.send({code: "error", data: null, msg: "未登录或已过期，请重新登录"});
            }
        });
    }

});
app.use('/', index);
app.use('/moduleCategory',moduleCategory);
app.use('/model',model);
app.use('/file',file);
app.use('/downInfo',downInfo);
app.use('/authority',authority);
app.use('/swagger', express.static('public/swagger'));
// catch 404 and forward to error handler
app.use(function(req, res, next) {
  var err = new Error('Not Found');
  err.status = 404;
  next(err);
});

// error handler
app.use(function(err, req, res, next) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});

module.exports = app;
