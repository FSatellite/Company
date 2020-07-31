/**
 * Created by zyct-yt on 2017/8/15.
 */
(function($){
    $.fn.extend({
        accordion:function(v1,v2){
            this.each(function(){
                if(!this.state)init(this);
                if("string"==typeof(v1)){
                    this[v1](v2);
                }else if("object"==typeof(v1)){

                }
            });
        }
    });
    function init(ele) {
      var chs=$(ele).find(".cus-item");
      chs.each(function(){
          var ch=this;
          var menus=$(this).children();
         var attr=CFactory.toJSON($(this).attr("cus-data"));
         var title=CFactory.createNode({type:"div",attr:{class:"cus-acc-it",style:"margin-left: 17px;height: 35px;overflow: hidden"},
         child:[{type:"img",attr:{class:"cus-acc-img",src:"img/acc-close.png",width:"16",height:"16",style:"margin-right:8px;cursor: pointer"}},
             {type:"label",text:attr.title,attr:{style:"cursor: pointer"}}]});
         this.appendChild(title);
         var cont=CFactory.createNode({type:"div",attr:{class:"cus-acc-cont",style:"display:none"}})
          $(cont).append(menus)
          this.appendChild(cont);
          $(title).click(function(){
              if(!ch.expand){;
                  expand(ch);
              }else{
                  close(ch);
              }
          });
      });
        $(".cus-accordion").children(".cus-item").eq(0).children(".cus-acc-cont").css({"display":"block"});
    }
    function expand(it){
        it.expand=true;
        var img=$(it).find(".cus-acc-img");
        $(img).attr({"src":"img/acc-expand.png"});
        var cont=$(it).find(".cus-acc-cont");
        $(cont).css({"display":"block"});
    }
    function close(it){
        it.expand=false;
        var img=$(it).find(".cus-acc-img");
        var cont=$(it).find(".cus-acc-cont");
        $(cont).css({"display":"none"});
        $(img).attr({"src":"img/acc-close.png"});
    }
    function setcheck(it){

    }
    $(function(){
        $(".cus-accordion").accordion();
    })
})(jQuery)