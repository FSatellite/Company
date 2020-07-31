var mongoose = require('../modules/db.js'),
    Schema = mongoose.Schema;
var modelSchema = new Schema({
    id: {type: String, unique: true, required: true},//id
    modelname: {type: String},//程序名
    fmiVersion: {type: String},//版本
    modelIdentifier: {type: String},//程序标识
    guid: {type: String},//guid
    description: {type: String},//程序的简要概述
    authorId: {type: String},//作者id
    author: {type: String},//作者名
    generatationTool: {type: String},//开发语言
    programType: {type: String},//程序类型：exe、 dll
    categoryCode: {type: String},//专业分类代号
    category: {type: String},//专业分类名
    principleType: {type: String},//原理依据类型：行业规范、岗位手册、工程经验
    principleTypeCode: {type: String},//原理依据类型代号
    applicationType: {type: String},//功能应用类型：数据计算、数据检查、数据查找类
    applicationTypeCode: {type: String},//功能应用类型代号:
    moduleCategory: {type: String},//程序模块分类
    moduleCategoryCode: {type: String},//程序模块分类代号
    copyright: {type: String},//版权
    remark: {type: String},//备注
    keyword: {type: String},//关键词
    downloadNumber: {type: Number},//版权
    applicationSize: {type: String},//程序大小
    publishTime: {type: String},//发布时间
    updateTime: {type: String},//更新时间
    //applicationPermissions: {type: String},//程序权限：部分，公开
    objectId: {type: String} //objectId：数据库中存储的文件的objectId
}, {collection: 'model'});
/**
 * 查询所有程序：支持分页和根据名称模糊查询
 * @param options
 * @param callback
 */
modelSchema.methods.getAll = function (options, callback) {
    var query = Model.find({});
    var page = options.page;
    var pageSize = options.pageSize;
    if (page && pageSize) {
        query.skip((page - 1) * pageSize);
        query.limit(pageSize);
        delete options.page;
        delete options.pageSize;
    }
    if (options) {
        query.where(options).sort({publishTime: -1});
    }
    query.exec(function (err, rs) {
        if (err) {
            callback(err, null);
        } else {
            Model.find(options, {}, function (err1, value) {
                if (page && pageSize) {
                    var jsonArray = {list: rs, total: value.length, page: page, pageSize: pageSize};
                    callback(err1, jsonArray);
                } else {
                    callback(err1, rs);
                }

            })
        }

    })
};

/**
 * 查询所有程序：
 * @param options
 * @param callback
 */
modelSchema.methods.getAll2 = function (applicationTypeCode, authorId, guids, callback) {
    var options = {};
    if (applicationTypeCode) {
        options = {$and: [{applicationTypeCode: applicationTypeCode}, {$or: [{authorId: authorId}, {$and: [{authorId: {$ne: authorId}}, {guid: {$in: guids}}]}]}]};
    } else {
        options = {$and: [{$or: [{authorId: authorId}, {$and: [{authorId: {$ne: authorId}}, {guid: {$in: guids}}]}]}]};
    }

    Model.find(options, function (err, rs) {
        callback(err, rs);
    });
};
/**
 * 查询单个程序
 * @param options
 * @param callback
 */
modelSchema.methods.getOne = function (options, callback) {
    Model.findOne(options, {_id: 0}, function (err, value) {
        callback(err, value);
    })
};
/**
 * 添加程序
 * @param obj
 * @param callback
 */
modelSchema.methods.insert = function (obj, callback) {
    var instance = new Model(obj);
    Model.create(instance, function (err) {
        callback(err);
    });
};

/**
 * 修改分类
 * @param json
 * @param callback
 */
modelSchema.methods.update = function (json, callback) {
    Model.findOneAndUpdate({id: json.id}, {$set: json}, {new: true}, function (err) {
        callback(err);
    })
};
/**
 * 删除一个程序
 * @param options
 * @param callback
 */
modelSchema.methods.delete = function (options, callback) {
    Model.findOneAndDelete(options, function (err, value) {
        callback(err, value);
    })
};
var Model = mongoose.model('Model', modelSchema);
module.exports = Model;