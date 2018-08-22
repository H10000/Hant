layui.use(['layer', 'form'], function () {
    var layer = layui.layer;
    var layui_form = layui.form;
    var menujson;
    var vm;
    var layer_denglu_zhuce_id;
    var vm_send_message_component = "";
    var Interval_timeDowm_SendMessage;
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
                        , isSend: true
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
                    layer_denglu_zhuce_id = layer.open({
                        type: 1,
                        title: ['注册', 'font-size:18px;font-weight:bold;color:#c2c2c2;'],
                        content: content,
                        area: ['500px', '400px'],
                        shade: [0.8, '#393D49'],
                        shadeClose: true,
                        anim: 1,
                    });
                    getImageCode();
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
                    layer_denglu_zhuce_id = layer.open({
                        type: 1,
                        title: ['登录', 'font-size:18px;font-weight:bold;color:#c2c2c2;'],
                        content: content,
                        area: ['500px', '340px'],
                        shade: [0.8, '#393D49'],
                        shadeClose: true,
                        anim: 1,
                    });
                    getImageCode();
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
    //验证码图片刷新
    $(document).on('click', '#image_code_01', function () {
        getImageCode();
    });
    var getImageCode = function () {
        $.ajax({
            type: "GET",
            dataType: "jsonp", // 返回的数据类型，设置为JSONP方式
            jsonp: 'callback', //指定一个查询参数名称来覆盖默认的 jsonp 回调参数名 callback
            jsonpCallback: 'handleResponse', //设置回调函数名
            url: ApiConfig.url + "account/",
            success: function (data) {
                var resultJSON = data;
                $("#image_code_01").attr("src", "data:image/jpg;base64," + data.base64Str);
                $("#hidimgcode").attr("src", data.imgcode);
            },
            error: function (data) {
                layer.alert("请联系系统管理员!");
            }
        })
    }
    //监听注册、登录按钮
    $(document).on('click', '#zhuce_tijiao_1', function () {
        layer.msg('11');
        return false;
    });
    $(document).on('click', '#zhuce_quxiao_1', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    $(document).on('click', '#denglu_tijiao_1', function () {
        layer.msg("11");
        return false;
    });
    $(document).on('click', '#denglu_quxiao_1', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    $(document).on('click', '#zhuce_tijiao_2', function () {
        layer.msg('11');
        return false;
    });
    $(document).on('click', '#zhuce_quxiao_2', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    $(document).on('click', '#denglu_tijiao_2', function () {
        layer.msg("11");
        return false;
    });
    $(document).on('click', '#denglu_quxiao_2', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    //发送短信
    $(document).on('click', '#zhuce_sendmessage_01', function () {

        var $obj = $(this);
        $($obj).css("display", "none");
        $($obj).next().css("display", "inline-block");
        //发送验证码后倒计时
        if (Interval_timeDowm_SendMessage != undefined) {
            clearInterval(Interval_timeDowm_SendMessage);
        }
        var timedowm = 60;
        $("#zhuce_sendlable_01").text("倒计时" + timedowm + "秒");
        Interval_timeDowm_SendMessage = setInterval(function () {
            timedowm--;
            if (timedowm == 0) {
                $($obj).next().css("display", "none");
                $($obj).css("display", "inline-block");
                clearInterval(Interval_timeDowm_SendMessage);
            }
            else
                $($obj).next().text("倒计时" + timedowm + "秒");
        }, 1000);
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
    //生成guid
    function uuid(len, radix) {
        var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
        var uuid = [], i;
        radix = radix || chars.length;
        if (len) {
            for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
        } else {
            var r;
            uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
            uuid[14] = '4';
            for (i = 0; i < 36; i++) {
                if (!uuid[i]) {
                    r = 0 | Math.random() * 16;
                    uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
                }
            }
        }
        return uuid.join('');
    }
});