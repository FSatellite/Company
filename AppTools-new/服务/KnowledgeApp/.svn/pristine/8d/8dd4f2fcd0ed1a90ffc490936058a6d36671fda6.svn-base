var mongoose = require('../modules/db.js'),
    Schema = mongoose.Schema;
var gridFileSchema = new Schema({

}, { collection: 'knowledgeApp.files' });

/**
 * 根据id查询分类
 * @param id
 * @param callback
 */
gridFileSchema.methods.getById = function (id,callback) {
   try{
       GridFile.findOne({_id:mongoose.Types.ObjectId(id)},{_id:0},function (err,value) {
           callback(err,value);
       })
   }catch (e){
       callback(e,null);
   }
};

var GridFile = mongoose.model('GridFile',gridFileSchema);

module.exports = GridFile;