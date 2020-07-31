var express = require('express');
var bodyParser = require("body-parser");
var DownInfo = require("../class/DownInfoSchema");
var mongoose = require('../modules/db.js');
var uuid = require('../modules/uuid');
var URL = require('url');
var router = express.Router();
router.use(bodyParser.urlencoded({extended: false}));
var downInfoInstance = new DownInfo();

/**
 * 查询用户的下载信息
 */
router.get("/getByUser/:userId", function (req, res, next) {
    var userId = req.params.userId;
    var query = {userId:userId};
    downInfoInstance.getAll(query,function(err,result) {
        if (err) {
            res.send({code: "error", msg: "用户的下载信息获取失败，原因："+err});
        } else {
            res.send({code: "success",data:result,msg:"用户的下载信息获取成功"});
        }
    })
});
/**
 * 根据id查询程序分类
 */
router.get("/getByModel/:modelId", function (req, res, next) {
    var modelId = req.params.modelId;
    var query = {modelId:modelId};
    downInfoInstance.getAll(query,function (err,result) {
        if (err) {
            res.send({code: "error", msg: "程序的下载信息获取失败，原因："+err});
        } else {
            res.send({code: "success",data:result,msg:"程序的下载信息获取成功"});
        }
    })
});
/**
 * 删除分类
 */
router.delete("/deleteByModel/:modelId", function (req, res, next) {
    var id = req.params.id;
    var modelId = req.params.modelId;
    var query = {modelId:modelId};
    downInfoInstance.delete(query,function (err) {
        if (err) {
            res.send({code: "error", msg: "分类删除失败，原因："+err});
        } else {
            res.send({code: "success",msg:"分类删除成功"});
        }
    })
});
module.exports = router;