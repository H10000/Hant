layui.use(['layer', 'form'], function () {
    var layer = layui.layer;
    var layui_form = layui.form;
    var menujson;
    var vm;
    $(document).ready(function () {
        $.ajax({
            type: "GET",
            data: {},
            datatype: "JSON",
            url: "/Config/Menu/menu.json",
            success: function (data) {
                menujson = data;
                var leftmenuhtml = "";
                var rightmenuhtml = "";
                var navmenuhtml = "";
                //
                $.each(menujson.leftmenu, function (index, item) {
                    if (item.child.length > 0) {
                        leftmenuhtml += "<li class=\"layui-nav-item\">";
                        leftmenuhtml += "<a id=\"menu_left_" + item.code + "\" name=\"menu\" href=\"" + item.url + "\">" + item.name + "</a>";
                        leftmenuhtml += " <dl class=\"layui-nav-child\">";
                        $.each(item.child, function (index2, item2) {
                            leftmenuhtml += " <dd><a id=\"menu_left_" + item2.code + "\" name=\"menu\" href=\"" + item2.url + "\">" + item2.name + "</a></dd>";
                        });
                        leftmenuhtml += "</dl>";
                    }
                    else {
                        leftmenuhtml += " <li class=\"layui-nav-item\"><a id=\"menu_left_" + item.code + "\" name=\"menu\" href=\"" + item.url + "\">" + item.name + "</a></li>";
                    }
                });
                //
                $.each(menujson.rightmenu, function (index, item) {
                    if (item.child.length > 0) {
                        rightmenuhtml += " <li class=\"layui-nav-item\">";
                        rightmenuhtml += "<a id=\"menu_right_" + item.code + "\" name=\"menu\" href=\"" + item.url + "\"><img src=\"http://t.cn/RCzsdCq\" class=\"layui-nav-img\">" + item.name + "</a>";
                        rightmenuhtml += " <dl class=\"layui-nav-child\">";
                        $.each(item.child, function (index2, item2) {
                            rightmenuhtml += " <dd><a id=\"menu_right_" + item2.code + "\" name=\"menu\" href=\"" + item2.url + "\">" + item2.name + "</a></dd>";
                        });
                        rightmenuhtml += "</dl>";
                    }
                    else {
                        rightmenuhtml += " <li class=\"layui-nav-item\"><a id=\"menu_right_" + item.code + "\" name=\"menu\" href=\"" + item.url + "\">" + item.name + "</a></li>";
                    }
                });
                var Request = GetRequest();
                navmenuhtml = navmenuinit(Request["id"] == undefined ? "index_page_01" : String(Request["id"]));
                vm = new Vue({
                    el: "#menu",
                    data: {
                        leftmenuhtml: leftmenuhtml
                        , rightmenuhtml: rightmenuhtml
                        , navmenuhtml: navmenuhtml
                        , isDengLu: false
                    }
                });
                layui.use('element', function () {
                    var element = layui.element;
                })
            },
            error: function (data) {
                layer.alert("请联系系统管理员!");
            }
        });
    })

    //菜单点击事件
    $(document).on("click", 'a[name=\'menu\']', function () {
        var $obj = $(this);
        var id = $obj.prop("id");
        var content = "";
        if (id == "menu_zhuce_01") {
            $.ajax({
                type: "GET",
                datatype: "html",
                url: "/Config/Menu/zhuce.html",
                success: function (data) {
                    content = data;
                    var id = layer.open({
                        type: 1,
                        title: '注册',
                        content: content,
                        area: ['500px', '400px'],
                        shade: [0.8, '#393D49'],
                        shadeClose: true,
                        anim: 1,
                    });
                    //监听提交
                    $("menu").on('click', '#zhuce_zhuce', function () {
                        layer.msg('11');
                        return false;
                    });
                    $("menu").on('click', '#zhuce_quxiao', function () {
                        layer.close(id);
                    });
                },
                error: function (data) {
                    layer.alert("请联系系统管理员!");
                }
            })
        }
        else if (id == "menu_denglu_01") {
            var content = "";
            $.ajax({
                type: "GET",
                datatype: "html",
                url: "/Config/Menu/denglu.html",
                success: function (data) {
                    content = data;
                    var id = layer.open({
                        type: 1,
                        title: ['登录', 'font-size:18px;font-weight:bold;color:#c2c2c2;'],
                        content: content,
                        area: ['500px', '340px'],
                        shade: [0.8, '#393D49'],
                        shadeClose: true,
                        anim: 1,
                    });
                    //监听提交
                    $("menu").on('click', '#zhuce_tijiao', function () {
                        layer.msg("11");
                        return false;
                    });
                    $("menu").on('click', '#zhuce_quxiao', function () {
                        layer.close(id);
                    });
                },
                error: function (data) {
                    layer.alert("请联系系统管理员!");
                }
            })
        }
        else {
            var href = $obj.prop("href");
            window.location.href = $obj.prop("href") + "?id=" + id;
        }

        return false;
    });
    //加载导航栏菜单
    var navmenuinit = function (id) {
        var m1;
        var m2;
        function nav_menu(code, name, url) {
            this.code = code;
            this.name = name;
            this.url = url;
        };
        var code = id.split('_');
        if (code[1] == "left") {
            $.each(menujson.leftmenu, function (index, item) {
                if (code[2] == item.code) {
                    m1 = new nav_menu("nav_left_" + item.code, item.name, item.url);
                }
                else {
                    if (item.child.length > 0) {
                        $.each(item.child, function (index2, item2) {
                            if (code[2] == item2.code) {
                                m1 = new nav_menu("nav_left_" + item2.code, item2.name, item2.url);
                                m2 = new nav_menu("nav_left_" + item.code, item.name, item.url);
                            }
                        });
                    }
                }
            });
        }
        else if (code[1] == "right") {
            $.each(menujson.rightmenu, function (index, item) {
                if (code[2] == item.code) {
                    m1 = new nav_menu("nav_right_" + item.code, item.name, item.url);
                }
                else {
                    if (item.child.length > 0) {
                        $.each(item.child, function (index2, item2) {
                            if (code[2] == item2.code) {
                                m1 = new nav_menu("nav_right_" + item2.code, item2.name, item2.url);
                                m2 = new nav_menu("nav_right_" + item.code, item.name, item.url);
                            }
                        });
                    }
                }
            });
        }
        var html = "";
        html += "<span class=\"layui-breadcrumb\">";
        if (m1 == undefined) {
            html += "<a id=\"index_page_01\" name=\"menu\" href=\"../Menu/index\"><cite>首页</cite></a>";
        }
        else {
            html += "<a id=\"index_page_01\" name=\"menu\" href=\"../Menu/index\">首页</a>";
        }
        html += "<a name=\"menu\" href=\"#\"></a>";
        html += "</span>";
        if (m1 != undefined) {
            html += "<span class=\"layui-breadcrumb\" lay-separator=\"—\">";
            if (m2 != undefined) {
                html += "<a id=\"" + m2.code + "\" name=\"menu\" href=\"" + m2.url + "\">" + m2.name + "</a>";
            }
            html += "<a id=\"" + m1.code + "\" name=\"menu\" href=\"" + m1.url + "\"><cite>" + m1.name + "</cite></a>";
            html += "</span>";
        }
        return html;
    }
    //获取url参数
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串 
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
});