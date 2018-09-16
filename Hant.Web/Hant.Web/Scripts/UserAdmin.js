layui.use(['element', 'table', 'form'], function () {
    var element = layui.element;
    var table = layui.table;
    var form = layui.form;
    $(document).ready(function () {
        table.render({
            elem: '#table1'
            , height: 312
            , url: ApiConfig.url + "admin/GetUserInfoList" //数据接口
            , page: true //开启分页
            , cols: [[ //表头
                { type: 'numbers' }
                , { type: 'checkbox' }
                , { field: 'ID', title: 'ID', hide: true }
                , { field: 'UserID', title: '用户唯一标示', hide: true }
                , { field: 'UserName', title: '用户名', align: 'center' }
                , { field: 'Mobile', title: '手机号', width: 150, align: 'center' }
                , {
                    field: 'Status', title: '账号状态', width: 95, templet: '#TplStatus'
                }
                , { field: 'right', width: 177, align: 'center', toolbar: '#Tplbar' }
            ]]
            , page: true
            , height: "full-265"
        });
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
            , url: ApiConfig.url + "admin/UpdateUserStatus"
            , success: function (data) {

            }
            , error: function (data) {

            }
        });
    });

    //监听工具条
    table.on('tool(table1)', function (obj) { //注：tool是工具条事件名，test是table原始容器的属性 lay-filter="对应的值"
        var data = obj.data; //获得当前行数据
        var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
        var tr = obj.tr; //获得当前行 tr 的DOM对象

        if (layEvent == "detail") { //查看
            alert(11);
        } else if (layEvent == 'del') { //删除
            layer.confirm('真的删除行么', function (index) {
                obj.del(); //删除对应行（tr）的DOM结构，并更新缓存
                layer.close(index);
                //向服务端发送删除指令
            });
        } else if (layEvent == 'edit') { //编辑
            //do something

            //同步更新缓存对应的值
            obj.update({
                username: '123'
                , title: 'xxx'
            });
        }
    });
    //监听搜索按钮
    form.on('submit(SearchButton)', function (data) {
        table.render({
            elem: '#table1'
            , height: 312
            , url: ApiConfig.url + "admin/GetUserInfoList" //数据接口
            , where: {
                SearchUserName: data.field.SearchUserName
                , SearchPhone: data.field.SearchPhone
                , SearchStatus: data.field.SearchStatus ? 1 : 0
            }
            , page: true //开启分页
            , cols: [[ //表头
                { type: 'numbers' }
                , { type: 'checkbox' }
                , { field: 'ID', title: 'ID', hide: true }
                , { field: 'UserID', title: '用户唯一标示', hide: true }
                , { field: 'UserName', title: '用户名', align: 'center' }
                , { field: 'Mobile', title: '手机号', width: 150, align: 'center' }
                , {
                    field: 'Status', title: '账号状态', width: 95, templet: '#TplStatus'
                }
                , { field: 'right', width: 177, align: 'center', toolbar: '#Tplbar' }
            ]]
            , page: true
            , height: "full-265"
        });
        return false;
    });
});