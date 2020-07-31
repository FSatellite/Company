var express = require('express');
var bodyParser = require("body-parser");
var ModuleCategory = require("../class/ModuleCategorySchema");
var mongoose = require('../modules/db.js');
var uuid = require('../modules/uuid');
var URL = require('url');
var router = express.Router();
router.use(bodyParser.urlencoded({extended: false}));
var moduleCategoryInstance = new ModuleCategory();
var app =require("../app");
/**
 * 添加程序分类
 */
router.post("/insert", function (req, res, next) {
    var json = req.body;
    moduleCategoryInstance.insert(json, function (err) {
        if (err) {
            res.send({code: "error", msg: "分类保存失败，原因："+err});
        } else {
            res.send({code: "success",data:json,msg:"分类保存成功"});
        }
    });
   /* if (json.moduleCategoryCode) {
        //update
        moduleCategoryInstance.update(json,function (err) {
            if (err) {
                res.send({code: "error", msg: "分类更新失败，原因："+err});
            } else {
                res.send({code: "success",data:json,msg:"分类更新成功"});
            }
        })
    } else {
        //insert
        json.categoryId = Math.uuidFast();
        moduleCategoryInstance.insert(json, function (err) {
            if (err) {
                res.send({code: "error", msg: "分类保存失败，原因："+err});
            } else {
                res.send({code: "success",data:json,msg:"分类保存成功"});
            }
        });
    }*/

});
/**
 * 编辑程序分类
 */
router.post("/update", function (req, res, next) {
    var json = req.body;
    moduleCategoryInstance.update(json,function (err) {
        if (err) {
            res.send({code: "error", msg: "分类更新失败，原因："+err});
        } else {
            res.send({code: "success",data:json,msg:"分类更新成功"});
        }
    })
});
/**
 * 查询所有分类
 */
router.get("/getAll", function (req, res, next) {
    var query = {};
    console.log("//////////////////////////");
    moduleCategoryInstance.getAll(query,function(err,result) {
        console.log("//////////////////////////");
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因："+err});
        } else {
            res.send({code: "success",data:result,msg:"分类获取成功"});
        }
    })
});
/**
 * 根据moduleCategoryCode查询程序分类
 */
router.get("/getByCode/:moduleCategoryCode", function (req, res, next) {
    var moduleCategoryCode = req.params.moduleCategoryCode;
    moduleCategoryInstance.getById(moduleCategoryCode,function (err,result) {
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因："+err});
        } else {
            res.send({code: "success",data:result,msg:"分类获取成功"});
        }
    })
});
/**
 * 根据moduleCategory查询程序分类
 */
router.get("/getByName/:moduleCategory", function (req, res, next) {
    var moduleCategory = req.params.moduleCategory;
    moduleCategoryInstance.getByName(moduleCategory,function (err,result) {
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因："+err});
        } else {
            res.send({code: "success",data:result,msg:"分类获取成功"});
        }
    })
});
/**
 * 根据moduleCategory查询程序分类
 */
router.get("/checkExist", function (req, res, next) {
    var option = req.query;
    var query={$or:[{moduleCategoryCode:option.moduleCategoryCode},{moduleCategory:option.moduleCategory}]};
    moduleCategoryInstance.checkExist(query,function (err,result) {
        if (err) {
            res.send({code: "error", msg: "分类获取失败，原因："+err});
        } else {
            res.send({code: "success",data:result,msg:"分类获取成功"});
        }
    })
});

/**
 * 删除分类
 */
router.get("/deleteByCode/:moduleCategoryCode", function (req, res, next) {
    var moduleCategoryCode = req.params.moduleCategoryCode;
    moduleCategoryInstance.deleteById(moduleCategoryCode,function (err) {
       if (err) {
           res.send({code: "error", msg: "分类删除失败，原因："+err});
       } else {
           res.send({code: "success",msg:"分类删除成功"});
       }
   })
});
module.exports = router;