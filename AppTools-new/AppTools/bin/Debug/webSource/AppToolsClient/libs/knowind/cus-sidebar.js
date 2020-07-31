/**
 * 滑动条
 * Created by zyct-yt on 2017/8/15.
 */
(function($){
    $.fn.extend({
        sidebar:function(v1,v2){
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
       ele.initwidth=$(ele).width();
       ele.state="expand";
       ele["expand"]=expand;
       ele["shrink"]=shrink;
       var but=$(ele).find(".cus-sidebar-but");
       $(but).click(function(){
            if(ele.state=="expand"){
                ele["shrink"]();
                $(but).html("<span style='color:#BEC5DA'>≡</span>");
            }else{
                ele["expand"]();
                $(but).html("<span style='color:#BEC5DA'>|||</span>");
            }
       });
   }
   function shrink(){
       $(".cus-center").css({"margin-left":"55px"});
       $(this).width(50);
       this.state="shrink";
   }
   function expand() {
       $(".cus-center").css({"margin-left":"160px"});
       $(this).width(this.initwidth);
       this.state="expand";
   }
    $(function(){
        $(".cus-sidebar").sidebar();
    })
})(jQuery)
