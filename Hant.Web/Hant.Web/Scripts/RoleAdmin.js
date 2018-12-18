layui.use(['element', 'table', 'form', 'layer', 'laytpl'], function () {
    var element = layui.element;
    var table = layui.table;
    var form = layui.form;
    var layer = layui.layer;
    var laytpl = layui.laytpl;
    var layerOpen;
    $(document).ready(function () {
        var obj = { SearchUserName: "", SearchStatus: 1 }
        init(obj);
        element.init();
    });

    //监听启用禁用操作
    form.on('switch(TplStatus)', function (obj) {
        var ID = obj.value;
        var Status = obj.elem.checked ? 1 : 0;
        $.ajax({
            type: "GET"
            , dataType: "JSON"
            , data: { ID: ID, Status: Status }
            , url: ApiConfig.url + "admin/UpdateRoleStatus"
            , success: function (data) {

            }
            , error: function (data) {

            }
        });
    });
    //加载表格
    var init = function (obj) {
        table.render({
            elem: '#table1'
            , url: ApiConfig.url + "admin/GetRoleInfoList" //数据接口
            , where: {
                SearchUserName: obj.SearchUserName
                , SearchStatus: obj.SearchStatus
            }
            , page: { limits: [5, 10, 15, 20, 25] } //开启分页
            , cols: [[ //表头
                { type: 'numbers' }
                , { type: 'checkbox' }
                , { field: 'ID', title: 'ID', hide: true }
                , { field: 'RoleID', title: '用户唯一标示', hide: true }
                , { field: 'RoleName', title: '角色名', align: 'center' }
                , {
                    field: 'Status', title: '角色状态', width: 95, templet: '#TplStatus'
                }
                , { field: 'right', width: 177, align: 'center', toolbar: '#Tplbar' }
            ]]
            , height: "full-280"
            , toolbar: '#Tpltoolbar'
            , defaultToolbar: ['filter', 'print', 'exports']//自由配置头部工具栏右侧的图标 filter: 显示筛选图标 exports: 显示导出图标 print: 显示打印图标

        });
    }
    //监听行工具条
    table.on('tool(table1)', function (obj) { //注：tool是工具条事件名，table1是table原始容器的属性 lay-filter="对应的值"
        var rowobj = obj;
        var datas = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象

        if (layEvent == "detail") { //查看
            var data = {
                title: '查看'
                , type: 'add'
                , list: [{ title: '名称', name: 'name', value: datas.RoleName, type: 'text' }]
                , id: datas.ID
            }
            laytpl($('#TplView').html()).render(data, function (html) {
                layerOpen = layer.open({
                    type: 1,
                    title: ['' + data.title + '', 'font-size:18px;font-weight:bold;color:#c2c2c2;'],
                    content: html,
                    area: ['400px', '120px'],
                    shade: [0.8, '#393D49'],
                    shadeClose: true,
                    anim: 1,
                });
            });
        } else if (layEvent == 'del') { //删除
            layer.confirm('真的删除么', function (index) {
                //obj.del(); //删除对应行（tr）的DOM结构，并更新缓存
                layer.close(index);
                //向服务端发送删除指令
                var id = new Array();
                id[0] = datas.ID;
                $.ajax({
                    type: "POST"
                    , datatype: "JSON"
                    , data: { "": id }
                    , url: ApiConfig.url + "admin/DeleteRoleInfo" //数据接口
                    , traditional: true//允许参数为数组
                    , success: function (data) {
                        var obj = { SearchUserName: $("input[name='SearchUserName']").val(), SearchStatus: $("input[name='SearchStatus']").attr("checked") ? 1 : 0 }
                        table.reload('table1', {
                            where: {
                                SearchUserName: obj.SearchUserName
                                , SearchStatus: obj.SearchStatus
                            }
                        });
                    }
                    , error: function (data) {
                        layer.alert("请联系系统管理员!");
                    }
                })
            });
        } else if (layEvent == 'edit') { //编辑
            var data = {
                title: '编辑'
                , type: 'add'
                , list: [{ title: '名称', name: 'name', value: datas.RoleName, type: 'text' }]
                , id: datas.ID
            }
            laytpl($('#TplEdit').html()).render(data, function (html) {
                layerOpen = layer.open({
                    type: 1,
                    title: ['' + data.title + '', 'font-size:18px;font-weight:bold;color:#c2c2c2;'],
                    content: html,
                    area: ['400px', '200px'],
                    shade: [0.8, '#393D49'],
                    shadeClose: true,
                    anim: 1,
                });
            });

        }
    });
    //监听表格工具条
    table.on('toolbar(table1)', function (obj) {
      
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                var data = {
                    title: '添加'
                    , type: 'add'
                    , list: [{ title: '名称', name: 'name', value: '', type: 'text' }]
                    , id: ''
                }
                laytpl($('#TplAdd').html()).render(data, function (html) {
                    layerOpen = layer.open({
                        type: 1,
                        title: ['' + data.title + '', 'font-size:18px;font-weight:bold;color:#c2c2c2;'],
                        content: html,
                        area: ['400px', '200px'],
                        shade: [0.8, '#393D49'],
                        shadeClose: true,
                        anim: 1,
                    });
                });
                break;
            case 'delete':
                layer.confirm('真的删除么', function (index) {
                    layer.close(index);
                var checkStatus = table.checkStatus(obj.config.id);
                var ids = new Array();
                for (var i = 0; i < checkStatus.data.length; i++) {
                    ids[i] = checkStatus.data[i].ID;
                }
                $.ajax({
                    type: "POST"
                    , datatype: "JSON"
                    , data: { "": ids }
                    , url: ApiConfig.url + "admin/DeleteRoleInfo" //数据接口
                    , traditional: true//允许参数为数组
                    , success: function (data) {
                        var obj = { SearchUserName: $("input[name='SearchUserName']").val(), SearchStatus: $("input[name='SearchStatus']").attr("checked") ? 1 : 0 }
                        table.reload('table1', {
                            where: {
                                SearchUserName: obj.SearchUserName
                                , SearchStatus: obj.SearchStatus
                            }
                        });
                    }
                    , error: function (data) {
                        layer.alert("请联系系统管理员!");
                    }
                })
            })
                break;
            case 'update':
                layer.msg('刷新');
                break;
        };
    });
    //监听搜索按钮
    form.on('submit(SearchButton)', function (data) {
        var obj = { SearchUserName: data.field.SearchUserName, SearchStatus: data.field.SearchStatus ? 1 : 0 }
        init(obj);
        return false;
    });
    //监听添加提交按钮
    form.on('submit(AddButton)', function (data) {
        var name = data.field.name;
        $.ajax({
            type: "GET"
            , datatype: "JSON"
            , data: { name: name }
            , url: ApiConfig.url + "admin/SaveRoleInfo" //数据接口
            , success: function (data) {
                if (data.Result == "OK") {
                    var obj = { SearchUserName: "", SearchStatus: 1 }
                    init(obj);
                    layer.close(layerOpen);
                }
                else {
                    layer.alert(data.Result);
                }
            }
            , error: function (data) {
                layer.alert("请联系系统管理员!");
            }
        })
        return false;
    });
    //监听编辑提交按钮
    form.on('submit(EditButton)', function (data) {
        var name = data.field.name;
        var id = data.field.ID;
        $.ajax({
            type: "GET"
            , datatype: "JSON"
            , data: { name: name, id: id }
            , url: ApiConfig.url + "admin/UpdateRoleInfo" //数据接口
            , success: function (data) {
                if (data.Result == "OK") {
                    layer.close(layerOpen);
                    $(".layui-laypage-btn").click();//刷新当前页
                }
                else {
                    layer.alert(data.Result);
                }
            }
            , error: function (data) {
                layer.alert("请联系系统管理员!");
            }
        })
        return false;
    });

    //监听添加取消按钮
    $(document).on('click', '#add_quxiao_1', function () {
        layer.close(layerOpen);
    });

});