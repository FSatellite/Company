var mongoose = require('../modules/db.js'),
    Schema = mongoose.Schema;
var downInfoSchema = new Schema({
    id : {type:String,unique: true, required: true},
    userId : {type:String},
    userName : {type:String},
    modelId : {type:String},
    modelname : {type:String},
    downloadTime : {type:String}

}, { collection: 'downInfo' });
/**
 * 添加下载信息
 * @param obj
 * @param callback
 */
downInfoSchema.methods.insert = function(obj, callback) {
    var instance = new DownInfo(obj);
    DownInfo.create(instance,function(err){
        callback(err);
    });
};
/**
 * 查询所有下载信息
 * @param query
 * @param callback
 */
downInfoSchema.methods.getAll = function (query,callback) {
    DownInfo.find(query,{_id:0},function (err,value) {
        callback(err,value);
    });
};
/**
 * 删除下载信息
 * @param options(参数json)
 * @param callback
 */
downInfoSchema.methods.delete = function (options,callback) {
    DownInfo.deleteMany(options,function (err) {
        callback(err);
    })
};
var DownInfo = mongoose.model('DownInfo',downInfoSchema);

module.exports = DownInfo;