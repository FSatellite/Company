var mongoose = require('../modules/db.js'),
    Schema = mongoose.Schema;
var authoritySchema = new Schema({
    id : {type:String,unique: true, required: true},
    guid : {type:String},
    deptId : {type:String},
    authorId:{type:String},
}, { collection: 'authority' });
/**
 * 添加程序权限
 * @param obj
 * @param callback
 */
authoritySchema.methods.insert = function(obj, callback) {
    var instance = new Authority(obj);
    Authority.create(instance,function(err){
        callback(err);
    });
};
/**
 * 批量添加权限
 * @param arr
 * @param callback
 */
authoritySchema.methods.insertMany = function (arr,callback) {
    var objs = [];
    for(var i=0;i<arr.length;i++){
        objs.push(new Authority(arr[i]));
    }
    Authority.insertMany(objs,function (err,value) {
        callback(err,value);
    })


}
/**
 * 查询所有权限信息
 * @param query
 * @param callback
 */
authoritySchema.methods.getAll = function (query,callback) {
    Authority.find(query,{_id:0},function (err,value) {
        callback(err,value);
    });
};
/**
 * 删除权限信息（部分改为公开）
 * @param options
 * @param callback
 */
authoritySchema.methods.deleteMany = function (options,callback) {
    Authority.deleteMany(options,function (err) {
        callback(err);
    })
};
/**
 * 删除一条权限信息（部分时选中或取消某个部门）
 * @param options
 * @param callback
 */
authoritySchema.methods.deleteOne = function (options,callback) {
    Authority.findOneAndDelete(options,function (err) {
        callback(err);
    })
};
var Authority = mongoose.model('Authority',authoritySchema);

module.exports = Authority;