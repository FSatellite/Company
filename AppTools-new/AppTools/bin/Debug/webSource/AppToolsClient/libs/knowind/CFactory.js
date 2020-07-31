/**
 * Created by zyct-yt on 2017/8/15.
 */

Date.prototype.Format = function (fmt) { //author: meizz
    var o = {
        "M+": this.getMonth() + 1, //月份
        "d+": this.getDate(), //日
        "h+": this.getHours(), //小时
        "m+": this.getMinutes(), //分
        "s+": this.getSeconds(), //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

//时间比较（yyyy-MM-dd HH:mm:ss）
function compareTime(startTime,endTime) {
    var startTimes = startTime.substring(0, 10).split('-');
    var endTimes = endTime.substring(0, 10).split('-');
    startTime = startTimes[1] + '-' + startTimes[2] + '-' + startTimes[0] + ' ' + startTime.substring(10, 19);
    endTime = endTimes[1] + '-' + endTimes[2] + '-' + endTimes[0] + ' ' + endTime.substring(10, 19);
    var thisResult = (Date.parse(endTime) - Date.parse(startTime)) / 3600 / 1000;
    return thisResult;
}

(function(){

    function moduleFactory(){
        var moduleF=this;
        this.createComponentByDOM=function (dom, obj) {
            var comp=obj;
            comp.autoRun=function(){
                this.comp=dom;
                if(this.comp==null)return;
                // var compList=$(this.comp).find("[knoComp]");
                // for(var i=0;i<compList.length;i++){
                //     var name=$(compList[i]).attr("knoComp");
                //     this[name]=compList[i];
                // }
                moduleF.findComp(this.comp,this);
                if(comp.init){
                    comp.init();
                }
            };
            comp.autoRun(comp);
            return comp;
        };
        this.createComponents=function(obj){
            var comps=$("[compName="+obj.compName+"]");
            var list=[];
            for(var i=0;i<comps.length;i++){
                var nobj=$.extend(true, {}, obj);
                var comp=comps[i];
                nobj.comp=comp;
                list.push(nobj);
                if(comp==null)return;
                // var compList=$(comp).find("[knoComp]");
                // for(var i=0;i<compList.length;i++){
                //     var name=$(compList[i]).attr("knoComp");
                //     this[name]=compList[i];
                // }
                moduleF.findComp(nobj.comp,nobj);
                if(comp.init){
                    comp.init();
                }
            }
            return list;
        };
        this.createComponent=function(obj){
            var comp=obj;
            comp.autoRun=function(compName){
                this.comp=$("[compName="+compName+"]")[0];
                if(this.comp==null)return;
                moduleF.findComp(this.comp,this);
                if(comp.init){
                    comp.init();
                }
            };
            comp.autoRun(comp.compName);
            return comp;
        };
        this.createNode=function (obj){
            var mycompent=createChildNode(obj);
            return	mycompent;
        };
        this.findComp=function(comp,obj){
            var compList=$(comp).find("[knoComp]");
            for(var i=0;i<compList.length;i++){
                var name=$(compList[i]).attr("knoComp");
                obj[name]=compList[i];
            }
            var knoArray=$(comp).find("[knoArray]");
            for(var i=0;i<knoArray.length;i++){
                var name=$(knoArray[i]).attr("knoArray");
                var code=$(knoArray[i]).attr("code");
                if(!obj[name]){
                    obj[name]=[];
                }
                obj[name].push(knoArray[i]);
                if(code!=null){
                    obj[name][code]=knoArray[i];
                }
            }
        };
        //调用发发创建节点
        function createChildNode(obj){
            //万能构造
            var compent=document.createElement(obj.type);
            //采用回调的方法增加子节点
            for(var i=0;obj.child&&obj.child[i];i++){
                var childNode=createChildNode(obj.child[i]);
                compent.appendChild(childNode);
                if(obj.child[i].name)compent[obj.child[i].name]=childNode;
                compent[i]=childNode;
            }
            createAttrNode(obj,compent);
            return compent;
        }
        //创建节点属性
        function createAttrNode(obj,compent){
            for(var key in obj.attr){
                var objattr;
                try{
                    objattr=document.createAttribute(key);
                    objattr.value=obj.attr[key];
                    compent.setAttributeNode(objattr);
                }catch(error){
                    //FF
                    if(typeof(error.stack)!=undefined){
                        console.log(error.message);
                    }else{
                        // $.messager.alert("提示","抱歉！程序遇到异常请联系管理员\n" +
                        //     "异常信息:抓捕到的关键属性为"+key+",属性值"+attr[key]+"\n" +
                        //     "文件名:"+error.description+"\n" +
                        //     "行号："+error.number,"warning");
                        console.log(error.message);
                    }
                }
            };
            //设定样式
            for(var key in obj.style){
                compent.style[key]=obj.style[key];
            }
            //设定响应事件
            for(var key in obj.events){
                compent[key]=obj.events[key];
            }
            //如果存在文本节点
            if(obj.text){
                compent.appendChild(document.createTextNode(obj.text));
            }
            if(obj.innerHTML!=null){
                compent.innerHTML=obj.innerHTML;
            }
            //设定执行事件
            if(obj.callback){
                obj.callback(compent);
            }
        }
        //创建组件
        this.createTextlabel=function (label,inputText,customstyle){
            if(!label.style){
                label.style={marginRight:"10px"};
            }
            if(!inputText){
                inputText={};
            }
            if(!customstyle){
                customstyle={attr:{},style:{}};
            }

            var span=this.createNode({
                type:"span",
                style:customstyle.style,
                attr:customstyle.attr
            });
            var mylabel=this.createNode({
                type:"span",
                text:label.text,
                attr:label.attr,
                style:label.style
            });
            var input=this.createNode({
                type:"input",
                attr:inputText.attr,
                style:inputText.style
            });
            span.appendChild(mylabel);
            span.appendChild(input);
            span["label"]=label;
            span["input"]=input;
            return span;
        };
        /**
         * 该方法暂时只能处理一层对象，不能处理数组
         * @param v
         * @returns {Object}
         */
        this.toJSON=function(v){
            if(!v)return;
            var json=new Object();
            var kvs=v.split(",");
            for(var i in kvs){
                if(typeof(kvs[i])!="string")continue;
                var kv=kvs[i].split(":");
                json[kv[0]]=(kv[1]).replace("'","").replace("'","");
            }
            return json;
        }
    };

    function ComponentFactory(obj){

    }
    //-----------------------------------------------------------------
//----------------------------扩展数组方法-------------------------
    /**
     * 根据下标删除
     * @param index
     * @returns {boolean}
     */
    Array.prototype.del=function(index){
        if(isNaN(index)||index>=this.length){
            return false;
        }
        this.splice(index,1);
        return;
    };
//直接删除对象
    Array.prototype.delObj=function(obj){
        if(!obj||this.length==0){
            return false;
        }
        for(var i=0,n=0;i<this.length;i++){
            if(this[i]==obj){
                this.splice(i,1);
                return;
            }
        }
    };
//直接删除对象
    Array.prototype.delById=function(obj){
        if(!obj||this.length==0){
            return false;
        }
        for(var i=0,n=0;i<this.length;i++){
            if(this[i].index=obj.index){
                this.splice(i,1);
                return;
            }
        }
    };
    //获取对象数组下标
    Array.prototype.getIndex = function (obj) {
        if (!obj || this.length == 0) {
            return -1;
        }
        for (var i = 0; i < this.length; i++) {
            if (this[i] == obj) return +i;
        }
        return -1;
    };

    window.CFactory=new moduleFactory();

})();
