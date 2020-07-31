var express = require('express');
var bodyParser = require("body-parser");
var Authority = require("../class/AuthoritySchema");
var mongoose = require('../modules/db.js');
var uuid = require('../modules/uuid');
var config = require('../modules/SysConfig');
var URL = require('url');
var router = express.Router();
router.use(bodyParser.urlencoded({extended: false}));
var authorityInstance = new Authority();
var app = require("../app");
/*/!**
 * 添加程序分类
 *!/
router.post("/insert", function (req, res, next) {
    var json = req.body;
    moduleCategoryInstance.insert(json, function (err) {
        if (err) {
            res.send({code: "error", msg: "分类保存失败，原因：" + err});
        } else {
            res.send({code: "success", data: json, msg: "分类保存成功"});
        }
    });
});*/
/**
 * 批量添加
 */
router.post("/insertMany",function (req,res,next) {
    var json = req.body;
    console.log(json);
    var authorId = config.loginUser.user.id;
    var guid = json.guid;
    var depts = json["deptIds[]"];
    var objs = [];
    // console.log(depts);
    for(var i = 1;i<depts.length;i++){
        objs.push({id:Math.uuidFast(),guid:guid,deptId:depts[i],authorId:authorId})
    }
    authorityInstance.insertMany(objs,function (err,succ) {
        if (err){
            console.log(err);
            res.send({code: "error", msg: "程序权限设置失败，原因：" + err});
        }else{
            // console.log(succ);
            res.send({code: "success",data:succ, msg: "程序权限设置成功"});
        }
    })

});
/**
* 删除权限
*/
router.get("/deleteByModel/:guid", function (req, res, next) {
    var guid = req.params.guid;
    authorityInstance.deleteMany({guid:guid}, function (err) {
        if (err) {
            res.send({code: "error", msg: "权限删除失败，原因：" + err});
        } else {
            res.send({code: "success", msg: "权限删除成功"});
        }
    })
});
/**
* 根据程序查询所有权限
*/
router.get("/getByModel/:guid", function (req, res, next) {
    var guid = req.params.guid;
    var query = {guid:guid};
    authorityInstance.getAll(query, function (err, result) {
        if (err) {
            res.send({code: "error", msg: "权限查询失败，原因：" + err});
        } else {
            res.send({code: "success", data: result, msg: "权限查询成功"});
        }
    })
});
/**
 * 根据部门查询所有权限
 */
router.get("/getByDept/:deptId", function (req, res, next) {
    var deptId = req.params.deptId;
    var query = {deptId:deptId};
    authorityInstance.getAll(query, function (err, result) {
        if (err) {
            res.send({code: "error", msg: "权限查询失败，原因：" + err});
        } else {
            res.send({code: "success", data: result, msg: "权限查询成功"});
        }
    })
});
/*/!**
 * 编辑程序分类
 *!/
router.post("/update", function (req, res, next) {
    var json = req.body;
    moduleCategoryInstance.update(json, function (err) {
        if (err) {
            res.send({code: "error", msg: "分类更新失败，原因：" + err});
        } else {
            res.send({code: "success", data: json, msg: "分类更新成功"});
        }
    })
});
/!**
 * 查询所有分类
 *!/
router.get("/getAll", function (req, res, next) {
    var query = {};
    moduleCategoryInstance.getAll(query, function (err, result) {
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因：" + err});
        } else {
            res.send({code: "success", data: result, msg: "分类获取成功"});
        }
    })
});
/!**
 * 根据moduleCategoryCode查询程序分类
 *!/
router.get("/getByCode/:moduleCategoryCode", function (req, res, next) {
    var moduleCategoryCode = req.params.moduleCategoryCode;
    moduleCategoryInstance.getById(moduleCategoryCode, function (err, result) {
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因：" + err});
        } else {
            res.send({code: "success", data: result, msg: "分类获取成功"});
        }
    })
});
/!**
 * 根据moduleCategory查询程序分类
 *!/
router.get("/getByName/:moduleCategory", function (req, res, next) {
    var moduleCategory = req.params.moduleCategory;
    moduleCategoryInstance.getByName(moduleCategory, function (err, result) {
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因：" + err});
        } else {
            res.send({code: "success", data: result, msg: "分类获取成功"});
        }
    })
});
/!**
 * 根据moduleCategory查询程序分类
 *!/
router.get("/checkExist", function (req, res, next) {
    var option = req.query;
    var query = {$or: [{moduleCategoryCode: option.moduleCategoryCode}, {moduleCategory: option.moduleCategory}]};
    moduleCategoryInstance.checkExist(query, function (err, result) {
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因：" + err});
        } else {
            res.send({code: "success", data: result, msg: "分类获取成功"});
        }
    })
});

/!**
 * 删除分类
 *!/
router.get("/deleteByCode/:moduleCategoryCode", function (req, res, next) {
    var moduleCategoryCode = req.params.moduleCategoryCode;
    moduleCategoryInstance.deleteById(moduleCategoryCode, function (err) {
        if (err) {
            res.send({code: "error", msg: "分类删除失败，原因：" + err});
        } else {
            res.send({code: "success", msg: "分类删除成功"});
        }
    })
});*/
module.exports = router;