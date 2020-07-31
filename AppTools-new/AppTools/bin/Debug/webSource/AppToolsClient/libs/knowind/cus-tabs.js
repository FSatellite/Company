/**
 * tabs 选项卡
 * Created by zyct-yt on 2017/8/15.
 */
(function ($) {
    $.fn.extend({
        KnoTabs: function (v1, v2) {
            this.each(function () {
                this.tabs = (this.tabs == null ? init(this) : this.tabs);
                // if (!this.selected) this["setSelected"](0);
                if ("string" == typeof(v1) || "number" == typeof(v1)) {
                    this[v1](v2);
                } else if ("object" == typeof(v1)) {
                    for (var key in v1) {
                        this[key] = v1[key];
                    }
                }
            });
        }
    });
    function openTab(obj) {
        if (!obj)return;
        var stab = this["getTabs"](obj.id);
        if (obj.id != stab) {
            this["setSelected"](stab);
            return
        }
        ;
        var ele = this;
        var ntab = CFactory.createNode({
            type: "div", attr: {"class": "cus-tab dis-inb cur-p h-con-title"}, child: [{
                type: "a",
                child: [{type: "span", text: obj.title}, {type: "i", attr: {"class": "icon12 my-icon-del1 ml5"}}]
            }]
        });
        ntab.obj = obj;
        if (obj.id != null && obj.id != undefined) $(ntab).attr({"id": obj.id});
        if (obj.classStyle) {
            $(ntab).addClass(obj.classStyle);
        }
        $(ntab[0][1]).hide();//隐藏掉关闭按钮
        $(ntab[0][1]).mousedown(function (e) {//点击关闭按钮
            ele["closeTab"](ntab);
            event.stopPropagation();
        });
        $(ntab).mouseover(function () {
            $(ntab[0][1]).show();
        });
        $(ntab).mouseout(function () {
            $(ntab[0][1]).hide();
        });
        this.appendChild(ntab);
        this.tabs.push(ntab);
        var tabSet = this;
        $(ntab).click(function () {    //选中tab
            // ele["setSelected"](this);
            $(ele).find("a").css({"border-bottom": "3px"});
            $(this).find("a").css({"border-bottom": "solid #1C9EFE 3px"});
            if (this != ele.selected && tabSet["onClick"]) {
                tabSet["onClick"](ntab.obj);
            }
            ele.selected = this;
        });
    }

    function closeTab(tab) {
        delTabObj(tab);
        tab = this["getTabs"](tab);
        this.tabs.delObj(tab);
        if (this.selected == tab) this["setSelected"](0);
        $(tab).remove();
    }

    function delTabObj(tab, openTabs_) {
        var openTabsNew;
        var openTabsString = sessionStorage.getItem("openTabs");
        if (openTabsString) {
            openTabsNew = JSON.parse(openTabsString);
        }
        openTabs_ = [];
        for (var i = 0; i < openTabsNew.length; i++) {
            if (tab.id != openTabsNew[i].id) {
                openTabs_.push(openTabsNew[i])
            }
        }
        sessionStorage.setItem("openTabs", JSON.stringify(openTabs_));
        console.log(openTabs_)
    }

    function init(ele) {
        //方法初始化
        ele["openTab"] = openTab;
        ele["closeTab"] = closeTab;
        ele["setSelected"] = setSelected;
        ele["getTabs"] = getTabs;
        //-------------------------
        var tabs = [];
        var chs = $(ele).children();
        for (var i = 0; i < chs.length; i++) {
            chs[i].obj = {id: chs[i].id, title: ""};
            tabs.push(chs[i]);
        }
        $(chs).click(function () {
            ele.selected = this;
            $(ele).find("a").css({"border-bottom": "3px"});
            $(this).find("a").css({"border-bottom": "solid #1C9EFE 3px"});
        });
        return tabs;
    }

    function setSelected(tab) { //设置选中tab
        tab = this["getTabs"](tab);
        if (tab != this.selected) tab.click();
    }

    function getTabs(tab) {    //获取tab
        if ("number" == typeof(tab)) return this.tabs[tab];//根据顺序获取tab
        if ("string" == typeof(tab)) {
            for (var i = 0; i < this.tabs.length; i++) {
                if (this.tabs[i].obj.id == tab) return this.tabs[i];
            }
        }
        return tab;
    }

    // $(function () {
    //     console.log("custab第一次");
    //     // $(".cus-tabs").KnoTabs();
    // })
})(jQuery);