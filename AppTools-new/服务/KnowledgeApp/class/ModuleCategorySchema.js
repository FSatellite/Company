/**
 * 程序模块分类
 */
var mongoose = require('../modules/db.js'),
    Schema = mongoose.Schema;
var moduleCategorySchema = new Schema({
    moduleCategoryCode : {type:String,unique: true, required: true},
    moduleCategory : {type:String}
}, { collection: 'moduleCategory' });
/**
 * 添加程序模块分类
 * @param obj
 * @param callback
 */
moduleCategorySchema.methods.insert = function(obj, callback) {
    var instance = new ModuleCategory(obj);
    ModuleCategory.create(instance,function(err){
        callback(err);
    });
};
/**
 * 查询所有分类
 * @param query
 * @param callback
 */
moduleCategorySchema.methods.getAll = function (query,callback) {
    ModuleCategory.find(query,{_id:0},function (err,value) {
        callback(err,value);
    });
};
/**
 * 根据moduleCategoryCode查询分类
 * @param moduleCategoryCode
 * @param callback
 */
moduleCategorySchema.methods.getById = function (moduleCategoryCode,callback) {
    ModuleCategory.findOne({moduleCategoryCode:moduleCategoryCode},{_id:0},function (err,value) {
        callback(err,value);
    })
};
/**
 * 根据moduleCategory查询分类
 * @param moduleCategory
 * @param callback
 */
moduleCategorySchema.methods.getByName = function (moduleCategory,callback) {
    ModuleCategory.findOne({moduleCategory:moduleCategory},{_id:0},function (err,value) {
        callback(err,value);
    })
};
/**
 * 重名验证
 * @param option
 * @param callback
 */
moduleCategorySchema.methods.checkExist = function (option,callback) {
    ModuleCategory.find(option,{_id:0},function (err,value) {
        callback(err,value);
    })
};
/**
 * 删除分类
 * @param moduleCategoryCode
 * @param callback
 */
moduleCategorySchema.methods.deleteById = function (moduleCategoryCode,callback) {
    ModuleCategory.findOneAndDelete({moduleCategoryCode:moduleCategoryCode},{_id:0},function (err) {
        callback(err);
    })
};
/**
 * 修改分类
 * @param json
 * @param callback
 */
moduleCategorySchema.methods.update = function (json,callback) {
    ModuleCategory.findOneAndUpdate({moduleCategoryCode:json.moduleCategoryCode},{$set: json},{new: true},function (err) {
        callback(err,json);
    })
};
var ModuleCategory = mongoose.model('ModuleCategory',moduleCategorySchema);

module.exports = ModuleCategory;