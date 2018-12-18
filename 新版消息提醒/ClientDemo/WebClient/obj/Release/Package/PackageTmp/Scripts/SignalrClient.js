
var userID;
$(document).ready(function () {
    layui.use(['mobile'], function () {
        //var element = layui.element;
        var mobile = layui.mobile;
        var layim = mobile.layim;
        var layer = mobile.layer;
        //模拟登陆
        var Request = GetRequest();
        var userName = Request["userName"];
        userID = Request["userID"];
        layui.sessionData("messageinfo", null)
        //var userID = prompt('Enter your userID:', '');
        //var userName = prompt('Enter your userName:', '');
        var friendlist = [];
        var userlist = [];
        var ouID;
        var ouName;
        $.ajax({
            type: 'POST',
            url: "/Chat/GetUserInfo",
            dataType: "JSON",
            success: function (data) {
                var datas = JSON.parse(data);
                for (var i = 0; i < datas.length; i++) {
                    if (i == 0) {
                        userlist[0] = { username: datas[i].UserName, id: datas[i].UserID, avatar: "../Content/layui/src/images/head/User.png", sign: "身无彩凤双飞翼", status: "online" };
                    } else {
                        if (ouID == datas[i].OUID) {
                            userlist[userlist.length] = { username: datas[i].UserName, id: datas[i].UserID, avatar: "../Content/layui/src/images/head/User.png", sign: "身无彩凤双飞翼", status: "online" };

                        }
                        else {
                            friendlist[friendlist.length] = { groupname: ouName, id: ouID, list: userlist };
                            userlist = [];
                            userlist[0] = { username: datas[i].UserName, id: datas[i].UserID, avatar: "../Content/layui/src/images/head/User.png", sign: "身无彩凤双飞翼", status: "online" };
                        }
                    }
                    ouID = datas[i].OUID;
                    ouName = datas[i].OUName;
                    if (i == datas.length - 1) {
                        friendlist[friendlist.length] = { groupname: ouName, id: ouID, list: userlist };
                    }
                }
                //聊天信息配置
                layim.config({

                    // voice: false,
                    //上传图片接口
                    uploadImage: {
                        url: '/Chat/UploadMessageFile' //（返回的数据格式见下文）
                      , type: '' //默认post
                    }

                    //上传文件接口
            , uploadFile: {
                url: '/Chat/UploadMessageFile' //（返回的数据格式见下文）
                , type: '' //默认post
            },

                    //可同时配置多个
                    tool: [{
                        alias: 'history' //工具别名
                      , title: '历史记录' //工具名称
                      , iconUnicode: '' //图标字体的unicode，可不填
                      , iconClass: 'layui-icon-log' //图标字体的class类名
                    }]

        , init: {
            //我的信息
            mine: {
                "username": userName //我的昵称
              , "id": userID //我的ID
              , "avatar": "../Content/layui/src/images/head/User.png" //我的头像
              , "sign": "斗破苍穹"
            }
            //我的好友列表
          , friend: friendlist
        }


        , isNewFriend: false //是否开启“新的朋友”
        , isgroup: false //是否开启“群聊”
        , chatTitleColor: '#1E9FFF' //顶部Bar颜色
        , title: '消息中心' //应用名，默认：我的IM
                });
            }
        });



        var departmentID = "001";
        var deviceType = "2";
        //连接服务
        var connection;

        connection = $.hubConnection("http://192.168.3.143:8889/signalr/hubs", { qs: { UserID: userID, UserName: escape(userName), DepartmentID: departmentID, DeviceType: deviceType } });
        var contosoChatHubProxy = connection.createHubProxy('SignalRHub');
        connection.start().done(function () {

        }).fail(function () {
            //layer.alter("连接服务器失败");
        });
        contosoChatHubProxy.on('receive', function (data) {
            //通知android收到消息
            if (!isPc()) {
                try {
                    window.hello.showAndroid(data.content);
                }
                catch (e) {

                }

            }

            //设置头像
            var headimgurl = "../Content/layui/src/images/head/User.png";
            userConfig.forEach(function (item, index) {
                if (item.cid == data.id) {
                    headimgurl = item.url;
                }
            });
            //收到一条好友消息
            layim.getMessage({
                username: data.username
              , avatar: headimgurl
              , id: data.id
              , type: 'friend'
              , cid: Math.random() * 100000 | 0 //模拟消息id，会赋值在li的data-cid上，以便完成一些消息的操作（如撤回），可不填
              , content: data.content
            });

            //通知消息缓存
            var myDate = new Date();//获取系统当前时间
            var year = myDate.getFullYear();
            var month = myDate.getMonth();
            var day = myDate.getDate();
            var hour = myDate.getHours();
            var minutes = myDate.getMinutes();
            var seconds = myDate.getSeconds();
            var ms = { SendTime: year + '-' + month + '-' + day + ' ' + hour + ':' + minutes + ':' + seconds, MessageContent: data.content, MessageType: data.type, ReceiveId: data.receiveId, SendId: data.id };
            var messgeinfo = layui.sessionData('messageinfo')[data.id];
            if (messgeinfo == undefined) {
                messgeinfo = [ms];
            }
            else {
                messgeinfo[messgeinfo.length] = ms;
            }
            messgeinfo = { key: data.id, value: messgeinfo };
            layui.sessionData("messageinfo", messgeinfo);
        });

        //监听发送消息
        layim.on('sendMessage', function (data) {
            var To = data.to;
            var s = { SendId: data.mine.id, SendName: data.mine.username, ReceiveId: data.to.id, MessageContent: data.mine.content, MessageType: "friend" };
            contosoChatHubProxy.invoke("ClientSendMessage", s).done(function () {

            });

        });
        //监听查看更多记录
        layim.on('chatlog', function (data, ul) {
            $.ajax({
                type: 'POST',
                url: "/Chat/GetCurrentMessage",
                dataType: "JSON",
                data: { sendID: currentCID, receiveID: userID, messageType: 'friend', sendStatus: 2 },
                success: function (data) {
                    layim.panel({
                        title: currentName //标题
                      , tpl: $("#tpl_history").html() //模版
                      , data: JSON.parse(data)
                    });
                }
            });
        });

        //监听自定义工具栏点击，以上述扩展的工具为例
        layim.on('tool(history)', function (insert, send, obj) { //事件中的tool为固定字符，而code则为过滤器，对应的是工具别名（alias）
            $.ajax({
                type: 'POST',
                url: "/Chat/GetMessageHistory",
                dataType: "JSON",
                data: { sendID: currentCID, receiveID: userID, messageType: 'friend', sendStatus: 2 },
                success: function (data) {
                    layim.panel({
                        title: currentName //标题
                      , tpl: $("#tpl_history").html() //模版
                      , data: JSON.parse(data)
                    });
                }
            });
        });

        //
        $('#chatHistoryLog').live('click', function () {
            var type;
            userConfig.forEach(function (item, index) {
                if (item.cid == currentCID) {
                    type = item.type;
                }
            });
            if (currentCID != undefined) {
                $.ajax({
                    type: 'POST',
                    url: "/Chat/GetCurrentMessage",
                    dataType: "JSON",
                    data: { sendID: currentCID, receiveID: userID, messageType: type, sendStatus: 2 },
                    success: function (data) {
                        layim.panel({
                            title: currentName //标题
                          , tpl: historyTpl //模版
                          , data: JSON.parse(data)
                        });
                    }
                });
            }
        });

        //语音
        //var audio_context;
        //var recorder;
        //function startUserMedia(stream) {
        //    var input = audio_context.createMediaStreamSource(stream);

        //    recorder = new Recorder(input);
        //}
        //try {
        //    // webkit shim
        //    window.AudioContext = window.AudioContext || window.webkitAudioContext;
        //    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia;
        //    window.URL = window.URL || window.webkitURL;

        //    //audio_context = new AudioContext;
        //} catch (e) {
        //    alert('No web audio support in this browser!');
        //}

        //navigator.getUserMedia({ audio: true }, startUserMedia, function (e) {
        //});
      
        ////语音录入
        //var type = false;//开始true,停止false
        //$('#btn_input_type').live('click', function () {
        //    if (type) {
        //        recorder && recorder.stop().then(function (blob, buffer) {
        //            var formData = new FormData();
        //            formData.append("file", blob, Date.parse(new Date()) + ".wav");
        //            $.ajax({
        //                url: "/Chat/UploadMessageFile"
        //                , type: "POST"
        //                , data: formData
        //                , contentType: false
        //                , processData: false
        //                , dataType: 'json'
        //                //, headers: options.headers || {}
        //                , success: function (res) {
        //                }
        //               , error: function () {
        //                   that.msg('请求上传接口出现异常');
        //               }
        //            });

        //            recorder.clear();
        //        });
        //    }
        //    else {
        //        recorder && recorder.record();
        //    }

        //});

        //定时连接服务器
        setInterval(function () {
            if (connection != undefined && connection.state == 4) {
                connection = undefined;
                //signalrConnection();
            }
        }, 5000);
    });
});
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串 
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

var userAgentInfo = navigator.userAgent;
var isAndroid = userAgentInfo.indexOf('Android') > -1 || userAgentInfo.indexOf('Adr') > -1;//android终端
var isiOS = !!userAgentInfo.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/); //ios终端
var isPc = function IsPC() {//是否是PC

    var Agents = ["Android", "iPhone",
        "SymbianOS", "Windows Phone",
        "iPad", "iPod"];
    var isPC = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            isPC = false;
            break;
        }
    }
    return isPC;
};
var currentCID;
var currentName;
var historyTpl;
var openSystem = function (cid, name, tpl2, tpl3) {
    layui.use('mobile', function () {
        var mobile = layui.mobile;
        var layim = mobile.layim;
        historyTpl = tpl3;
        currentCID = cid;
        currentName = name;
        var type;
        userConfig.forEach(function (item, index) {
            if (item.cid == currentCID) {
                type = item.type;
            }
        });
        var data = layui.sessionData('messageinfo')[cid];
        if (data == undefined) {
            $.ajax({
                type: 'POST',
                url: "/Chat/GetCurrentMessage",
                dataType: "JSON",
                data: { sendID: currentCID, receiveID: userID, messageType: type, sendStatus: 2 },
                success: function (data) {
                    layim.panel({
                        title: currentName //标题
                      , tpl: tpl3 //模版
                      , data: JSON.parse(data)
                    });
                }
            });
        }
        else {
            layim.panel({
                title: currentName //标题
                  , tpl: tpl2 //模版
                  , data: data
            });
        }
    });
}
var userConfig = [{ cid: 'Sys_001', type: 'all', userName: '系统提示', url: '../Content/layui/src/images/head/system.png' }];

