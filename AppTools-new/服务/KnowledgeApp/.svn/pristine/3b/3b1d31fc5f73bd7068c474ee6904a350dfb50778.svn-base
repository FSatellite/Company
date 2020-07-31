var redis = require('redis');
var client = redis.createClient(6379,'127.0.0.1');
client.on('connect', function() {
    console.log('connected');
});
//string
client.set('framework', 'AngularJS');
client.get('framework',function(err,data){
    console.log(data);
});
//hash
client.hmset('frameworks', 'javascript', 'AngularJS', 'css', 'Bootstrap', 'node', 'Express');
client.hgetall('frameworks', function(err, object) {
    console.log(object);
});
//list
client.rpush(['framework-list', 'angularjs', 'backbone'], function(err, reply) {
    console.log(reply);//长度
});
client.lrange('framework-list', 0, -1, function(err, reply) {
    console.log(reply); //元素
});
//set
client.sadd(['tags', 'angularjs', 'backbonejs', 'emberjs'], function(err, reply) {
    console.log(reply); // 3
});
client.smembers('tags', function(err, reply) {
    console.log(reply);
});
//判断key是否存在
client.exists('key', function(err, reply) {
    if (reply === 1) {
        console.log('exists');
    } else {
        console.log('doesn\'t exist');
    }
});
//删除key
client.del('frameworks', function(err, reply) {
    console.log(reply);
});
//设定有效期
client.set('key1', 'val1');
client.expire('key1', 30);
//增加
client.set('key2', 10, function() {
    client.incr('key2', function(err, reply) {
        console.log(reply); // 11
    });
});