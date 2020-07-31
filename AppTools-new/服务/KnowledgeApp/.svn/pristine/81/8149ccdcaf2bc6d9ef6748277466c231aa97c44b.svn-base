var express = require('express');
var router = express.Router();
var bodyParser = require("body-parser");
var URL = require('url');
var fs = require("fs");
var path = require('path');
var mongoose = require('../modules/db');
var multiparty = require('multiparty');
router.use(bodyParser.urlencoded({extended: false}));
var GridFile = require("../class/GridFileSchema");
var gridFileInstance = new GridFile();
/**
 * 上传文件
 */
router.post("/upload", function (req, res, next) {
    var Attachment = mongoose.getAttachment();
    // console.log(Attachment);
    console.log("开始上传文件");
    // 解析一个文件上传
    var form = new multiparty.Form();

    form.encoding = 'utf-8';
    form.uploadDir = path.join(__dirname, '../public') + "/tmp";
    form.keepExtensions = true;
    //上传后处理
    form.parse(req, function (err, fields, files) {
        var objectId = "";
        if (err) {
            console.log('parse error:' + err);
            res.send({code: "error", data: [], msg: "文件上传失败！"});
            return;
        } else {
            //fields 是获取表单的数据
            //files是文件上传成功返回的地址信息
            console.log("上传文件表单");
            console.log(fields);
            var name = "";
            if (fields.FileName) {
                name = fields.FileName[0];
            }
            console.log("///////");
            console.log(files);
            console.log(files.file);
            if (files && files.file[0]) {
                var oldPath = files.file[0].path;
                var fileType = oldPath.substring(oldPath.lastIndexOf('.'), oldPath.length);
                var filename = name + fileType;
                var readStream = fs.createReadStream(oldPath);
                var options = {
                    filename: filename
                }
                Attachment.write(options, readStream, function (error, f) {
                    if (error) {
                        console.log(error);
                        res.send({code: "error", data: [], msg: "文件上传失败！"});
                    } else {
                        console.log(f);
                        var objectId = f._id;
                        console.log("上传成功");
                        // mongoose.upload(filename,readStream);
                        fs.unlink(oldPath, function (err) {
                            if (err) throw err;
                            console.log('成功')
                            res.send({code: "success", data: [], msg: "文件上传成功！"});
                        })
                    }
                })
            }
            // res.send({code: "success", data: [], msg: "文件上传成功！"});
        }
    })

});
/**
 * 下载文件
 */
router.get("/download", function (req, res, next) {
    var Attachment = mongoose.getAttachment();
    var param = URL.parse(req.url, true).query;
    var oId = param.objectId;
    var name = param.name;
    var objectId = mongoose.Types.ObjectId(oId);

    var readStream = Attachment.readById(objectId);
    readStream.on('error', function (err) {
        res.send({code: "error", data: null, msg: "文件不存在!"});
    });
    var p = path.join(__dirname, '../public') + "/tmp/" + oId + ".pdf";
    readStream.on('data', function (fileConent) {
        fs.appendFile(p, fileConent, function (err1) {
            if (err1) {
                console.error(err1);
            } else {
                console.log('写入成功');
            }
        });
    });
    readStream.on('close', function () {
        var file = path.join(__dirname, '../public') + "/tmp/" + oId + ".pdf";
        fs.exists(file, function (exist) {
            if (exist) {
                res.set({
                    "Content-type": "application/octet-stream",
                    "Content-Disposition": "attachment;filename=" + encodeURI(name)
                });
                var fReadStream = fs.createReadStream(file);
                fReadStream.on("data", function (chunk) {
                    res.write(chunk, "binary");
                });
                fReadStream.on("end", function () {
                    fs.unlink(file, function (err) {
                        if (err) throw err;
                        console.log('成功')
                    })
                    res.end();
                });
            } else {
                res.send({code: "error", data: null, msg: "文件不存在!"});
                res.end();
            }
        });
    });
});
/**
 * 删除文件
 */
router.get("/delete/:objectId", function (req, res, next) {
    var objectId = req.params.objectId;
    var id = mongoose.Types.ObjectId(objectId);
    var Attachment = mongoose.getAttachment();
    Attachment.unlink({_id:id}, function (err) {
        if (err) {
            res.send({code: "error", msg: "文件删除失败，原因：" + err});
        } else {

            res.send({code: "success", msg: "文件成功！"});

        }
    })

});
router.get("/getFile/:id",function (req,res,next) {
    var id = req.params.id;
    gridFileInstance.getById(id,function (err,value) {
        if (err) {
            res.send({code: "error", msg: "文件查询失败，原因：" + err});
        } else {
            console.log(value);
            res.send({code: "success",data:value, msg: "文件查询成功！"});

        }
    })
})
module.exports = router;