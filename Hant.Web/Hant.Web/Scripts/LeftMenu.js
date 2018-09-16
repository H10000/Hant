layui.use(['element'], function () {
    $(document).ready(function () {
        var Request = GetRequest();
        var RequestID = Request["id"].split('_')[2];
        var rightmenuhtml;
        var element = layui.element;
        $.ajax({
            type: "GET",
            data: {},
            datatype: "JSON",
            url: "/Config/Menu/menu.json",
            success: function (data) {
                var menujson = data;
                $.each(menujson.leftmenu, function (index, item) {
                    if (RequestID.indexOf(item.code) != -1) {
                        if (item.code == RequestID) {
                            if (item.leftchild != undefined && item.leftchild.length > 0) {
                                rightmenuhtml = "<ul class=\"layui-nav layui-nav-tree layui-inline\"  style=\"background-color: #fff;\" >";
                                $.each(item.leftchild, function (index2, item2) {

                                    if (index2 == 0) {
                                        rightmenuhtml += " <li class=\"layui-nav-item layui-this\">";
                                        rightmenuhtml += "<a id=\"menu_left_" + item2.code + "\" name=\"menu\" href=\"" + item2.url + "\">" + item2.name + "</a>";
                                        rightmenuhtml += "</li>";
                                    }
                                    else {
                                        rightmenuhtml += " <li class=\"layui-nav-item\">";
                                        rightmenuhtml += "<a  id=\"menu_left_" + item2.code + "\" name=\"menu\" href=\"" + item2.url + "\">" + item2.name + "</a>";
                                        rightmenuhtml += "</li>";
                                    }
                                });
                                rightmenuhtml += "</ul>";
                            }
                        }
                        else {
                            if (item.leftchild != undefined && item.leftchild.length > 0) {
                                rightmenuhtml = "<ul class=\"layui-nav layui-nav-tree layui-inline\"  style=\"background-color: #fff;\" >";
                                $.each(item.leftchild, function (index2, item2) {

                                    if (item2.code == RequestID) {
                                        rightmenuhtml += " <li class=\"layui-nav-item layui-this\">";
                                        rightmenuhtml += "<a  id=\"menu_left_" + item2.code + "\" name=\"menu\" href=\"" + item2.url + "\">" + item2.name + "</a>";
                                        rightmenuhtml += "</li>";
                                    }
                                    else {
                                        rightmenuhtml += " <li class=\"layui-nav-item\">";
                                        rightmenuhtml += "<a  id=\"menu_left_" + item2.code + "\" name=\"menu\" href=\"" + item2.url + "\">" + item2.name + "</a>";
                                        rightmenuhtml += "</li>";
                                    }
                                });
                                rightmenuhtml += "</ul>";
                            }
                        }
                    }
                });
                $("#left_menu").height($(document).height() - 160);
                $("#left_menu").html(rightmenuhtml);
                element.init();
            }
        });
    });
});