layui.use(['layer', 'form', 'element'], function () {
    var layer = layui.layer;
    var form = layui.form;
    var element = layui.element;
    var menujson;
    var vm;
    var layer_denglu_zhuce_id;
    var vm_send_message_component = "";
    var Interval_timeDowm_SendMessage;
    var Flag_zhuce_validate_true = false;//注册登录验证通过标记
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
                var loginCookie = GetCookie("HantLoginName");
                //if (loginCookie != null) {
                //    setInterval(SetCookie, 1000, "HantLoginName", loginCookie);
                //}
                var isDengLu = loginCookie == null ? false : true;

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
                        , isDengLu: isDengLu
                        , isSend: true
                    }
                });
                element.init();
                $("#RightContent").height($(document).height() - 160);
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
            async: false,
            jsonp: 'callback', //指定一个查询参数名称来覆盖默认的 jsonp 回调参数名 callback
            //jsonpCallback: 'handleResponse', //设置回调函数名
            url: ApiConfig.url + "account/GetImageCode",
            success: function (data) {
                var resultJSON = data;
                $("#image_code_01").attr("src", "data:image/jpg;base64," + data.base64Str);
                $("#hidimgcode").val(data.imgcode);
            },
            error: function (data) {
                layer.alert("请联系系统管理员!");
            }
        })
    }
    //监听注册、登录按钮
    $(document).on('click', '#zhuce_tijiao_1', function () {
        var $obj = $("#zhuce_name_01");
        var $obj2 = $("#zhuce_pwd_01");
        var $obj3 = $("#zhuce_pwd_02");
        var $obj4 = $("#zhuce_code_01");
        if (validateName($obj))
            if (validatePwd1($obj2))
                if (validatePwd2($obj3, $obj2))
                    if (validateImgCode($obj4)) {
                        var LoginName = $("#zhuce_name_01").val();
                        var Pwd = $("#zhuce_pwd_01").val();
                        $.ajax({
                            type: "GET",
                            data: { LoginName: LoginName, Pwd: Pwd },
                            dataType: "jsonp", // 返回的数据类型，设置为JSONP方式
                            jsonp: 'callback', //指定一个查询参数名称来覆盖默认的 jsonp 回调参数名 callback
                            url: ApiConfig.url + "account/SaveZhuCeInfoOfLoginName",
                            success: function (data) {
                                var resultJSON = data;
                                if (resultJSON.Result == "OK") {
                                    layer.alert("成功！");
                                }
                            },
                            error: function (data) {
                                layer.alert("请联系系统管理员!");
                            }
                        })
                    }
        return false;
    });
    $(document).on('click', '#zhuce_quxiao_1', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    $(document).on('click', '#zhuce_tijiao_2', function () {
        layer.msg('11');
        return false;
    });
    $(document).on('click', '#zhuce_quxiao_2', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    //
    $(document).on('click', '#denglu_tijiao_1', function () {
        var LoginName = $("#denglu_name_01");
        var Pwd = $("#denglu_pwd_01");
        var $imgCode = $("#denglu_code_01");
        if (LoginName.val() == "") {
            layer.tips("用户名不能为空！", LoginName, {
                tips: [2, '#FF5722'],
                time: 2000,
                closeBtn: 1
            });
        }
        else if (Pwd.val() == "") {
            layer.tips("密码不能为空！", Pwd, {
                tips: [2, '#FF5722'],
                time: 2000,
                closeBtn: 1
            });
        }
        else if ($imgCode.val() != $("#hidimgcode").val()) {
            validateImgCode($imgCode);
        }
        else {
            $.ajax({
                type: "GET",
                data: { LoginName: LoginName.val(), Pwd: Pwd.val() },
                dataType: "jsonp", // 返回的数据类型，设置为JSONP方式
                jsonp: 'callback', //指定一个查询参数名称来覆盖默认的 jsonp 回调参数名 callback
                jsonpCallback: 'handleResponse', //设置回调函数名
                url: ApiConfig.url + "account/ValidateDengLuByLoginName",
                success: function (data) {
                    var resultJSON = data;
                    var tip;
                    if (resultJSON.Result == "1") {
                        tip = "用户名不存在！";
                        layer.tips(tip, LoginName, {
                            tips: [2, '#FF5722'],
                            time: 2000,
                            closeBtn: 1
                        });
                    } else if (resultJSON.Result == "2") {
                        tip = "密码不正确！";
                        layer.tips(tip, Pwd, {
                            tips: [2, '#FF5722'],
                            time: 2000,
                            closeBtn: 1
                        });
                    }
                    else {
                        vm.isDengLu = true;
                        layer.msg('登录成功', {
                            icon: 1,
                            time: 1000
                        }, function () {
                            layer.close(layer_denglu_zhuce_id);
                            element.render('nav');
                            //保存Cookie
                            SetCookie("HantLoginName", LoginName.val());
                        });
                    }
                },
                error: function (data) {
                    layer.alert("请联系系统管理员!");
                }
            })
        }
        return false;
    });
    $(document).on('click', '#denglu_quxiao_1', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    $(document).on('click', '#denglu_tijiao_2', function () {
        layer.msg("11");
        return false;
    });
    $(document).on('click', '#denglu_quxiao_2', function () {
        layer.close(layer_denglu_zhuce_id);
    });
    //退出登录
    $(document).on('click', '#menu_right_02', function () {
        DeleteCookie("HantLoginName");
        layer.msg('退出成功', {
            icon: 1,
            time: 1000
        });
    });
    //发送短信
    $(document).on('click', '#zhuce_sendmessage_01,#denglu_sendmessage_01', function () {
        var $obj = $(this);
        var PhoneNum;
        if ($obj.prop("id") == "zhuce_sendmessage_01") {
            PhoneNum = $("#zhuce_phone_01").val();
        }
        else {
            PhoneNum = $("#denglu_sendmessage_01").val();
        }
        $.ajax({
            type: "GET",
            data: { PhoneNum: PhoneNum },
            dataType: "jsonp", // 返回的数据类型，设置为JSONP方式
            jsonp: 'callback', //指定一个查询参数名称来覆盖默认的 jsonp 回调参数名 callback
            //jsonpCallback: 'handleResponse', //设置回调函数名
            url: ApiConfig.url + "account/SendShortMessage",
            success: function (data) {
                var resultJSON = data;
                if (resultJSON.Result == "OK") {
                    $($obj).css("display", "none");
                    $($obj).next().css("display", "inline-block");
                    //发送验证码后倒计时
                    if (Interval_timeDowm_SendMessage != undefined) {
                        clearInterval(Interval_timeDowm_SendMessage);
                    }
                    var timedowm = 60;
                    $($obj).next().text("倒计时" + timedowm + "秒");
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
                }
            },
            error: function (data) {
                layer.alert("请联系系统管理员!");
            }
        })
    });
    //用户名输入失去焦点验证
    $(document).on('blur', '#zhuce_name_01,#zhuce_pwd_01,#zhuce_pwd_02', function () {//,#zhuce_code_01失去焦点验证码验证暂时取消
        var $obj = $(this);
        if ($obj.prop("id") == "zhuce_name_01") {
            validateName($obj);
        } else if ($obj.prop("id") == "zhuce_pwd_01") {
            validatePwd1($obj);
        } else if ($obj.prop("id") == "zhuce_pwd_02") {
            validatePwd2($obj, $("#zhuce_pwd_01"));
        }
        //else if ($obj.prop("id") == "zhuce_code_01") {
        //    validateImgCode($obj);
        //}

    });
    //用户名输入失去焦点验证方法
    var validateName = function (obj) {
        var tip;
        var reg = /^[\u4e00-\u9fa5|a-zA-Z]{1}[\u4e00-\u9fa5|a-zA-Z0-9_]{4,23}$/;
        var name = obj.val();
        if (name == "") {
            tip = "不能为空";
        } else if (!reg.test(name)) {
            tip = "用户名只能包括中文、英文、数字、下划线，且必须以中文或英文开始,长度大于4！";
        }
        else {
            $.ajax({
                type: "GET",
                data: { Name: obj.val() },
                dataType: "jsonp", // 返回的数据类型，设置为JSONP方式
                async: false,
                jsonp: 'callback', //指定一个查询参数名称来覆盖默认的 jsonp 回调参数名 callback
                jsonpCallback: 'handleResponse', //设置回调函数名
                url: ApiConfig.url + "account/ValidateNameIsExist",
                success: function (data) {
                    var resultJSON = data;
                    if (resultJSON.Result == "NO") {
                        tip = "用户名已存在！";
                        layer.tips(tip, obj, {
                            tips: [2, '#FF5722'],
                            time: 2000,
                            closeBtn: 1
                        });
                    }
                },
                error: function (data) {
                    layer.alert("请联系系统管理员!");
                }
            })
        }
        if (tip != undefined) {
            layer.tips(tip, obj, {
                tips: [2, '#FF5722'],
                time: 2000,
                closeBtn: 1
            });
            return false;
        } else {
            return true;
        }
    };
    //密码验证
    var validatePwd1 = function (obj) {
        var tip;
        var name = obj.val();
        if (name == "") {
            tip = "不能为空";
        } else if (name.length < 6) {
            tip = "密码长度必须大于等于6位！";
        }
        if (tip != undefined) {
            layer.tips(tip, obj, {
                tips: [2, '#FF5722'],
                time: 2000,
                closeBtn: 1
            });
            return false;
        } else {
            return true;
        }
    }
    var validatePwd2 = function (obj, obj2) {
        var tip;
        var name = obj.val();
        if (name == "") {
            tip = "不能为空";
        } else if (name != obj2.val()) {
            tip = "两次输入密码不一致！";
        }
        if (tip != undefined) {
            layer.tips(tip, obj, {
                tips: [2, '#FF5722'],
                time: 2000,
                closeBtn: 1
            });
            return false;
        } else {
            return true;
        }
    }
    //验证码验证
    var validateImgCode = function (obj) {
        var tip;
        var name = obj.val();
        if (name == "") {
            tip = "不能为空";
        } else if (name != $("#hidimgcode").val()) {
            tip = "验证码不正确！";
            getImageCode();
        }
        if (tip != undefined) {
            layer.tips(tip, obj, {
                tips: [1, '#FF5722'],
                time: 2000,
                closeBtn: 1
            });
            return false;
        } else {
            return true;
        }
    }
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
});