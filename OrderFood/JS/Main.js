$(function () {
    if (!main.IsPC())
    {
        $("body").addClass("phone");
    }

    $("#userInfo").html("系统载入中");
    
    order.getCurrentUser(function (data) {
        order.currentUser = data[0].Account;
        order.auth = data[0].auth;
        main.buildTopBar(order.currentUser, order.auth);
        main.refreshOrderList();

    }, function (err) {
        $("#userInfo").html(err);
        main.buildRefreshButton();
        main.buildRefreshMenuButton();
        main.buildDownLoadMenuButton();
        main.buildLogOutButton();
    });

    main.bindSerchInputEvent();



    main.refreshMenu();
});

var main = {
    buildLogOutButton: function () {
        var button = $('<button class="logout">注销</button>');
        button.click(function () {
            window.location = "ASHX/LogOut.ashx";
        });
        $("#userInfo").append(button);
    },
    buildRefreshButton: function () {
        var button = $("<button>刷新</button>");
        button.click(function () {
            $("#order").html("刷新中");
            main.refreshOrderList();
        });
        $("#userInfo").append(button);

    },
    buildSetManagerButton: function () {
        $("#order").empty();
        var button = $('<button>点我成为点餐负责人</button>');
        button.click(function () {
            $("#order").html("刷新中");
            main.setManager();
        });
        $("#order").append(button);
    },
    buildRefreshMenuButton: function () {
        var button = $('<button>刷新菜单</button>');
        button.click(function () {
            main.refreshMenu();
        });
        $("#userInfo").append(button);
    },
    buildDownLoadMenuButton: function () {
        var button = $('<button>下载菜单</button>');
        button.click(function () {
            window.open("PUBLIC\\menu.xls");
        });
        $("#userInfo").append(button);
    },
    buildUploadButton: function () {
        new CFileUpload($("#userInfo")[0], "xls", "ASHX/UploadMenu.ashx", "菜单上传");
    },
    buildTopBar: function (name) {
        $("#userInfo").empty();
        var label = $("<label></label>");
        label.html(name);
        $("#userInfo").append(label);
        main.buildRefreshButton();
        main.buildRefreshMenuButton();

        main.buildDownLoadMenuButton();
        if (order.auth > 1) {
            main.buildUploadButton();
        }

        main.buildLogOutButton();
    },
    buildOrderList: function (table, data) {
        table.gridTable({
            fk: "id",
            columns: ["id", "名称", "餐类", "店编号", "餐名", "价格", "已付", "找零", "点餐时间", "备注"],
            showfk: false,
            dataSource: data,
            rowInsertCallback: function (tr, row) {
                if (order.auth > 1 || order.currentUser == row["Name"]) {
                    var td = $('<td>删除</td>').appendTo(tr);
                    td.addClass("clickable");
                    td.click(function () {
                        if (confirm("是否删除")) {
                            main.deleteClick(row["id"]);
                        }
                    });
                }
            },
            cellInsertCallback: function (td, name, value, row) {
                switch (name) {
                    case "Money":
                        if (order.auth > 1) {
                            td.addClass("clickable");
                            td.click(function () {
                                var temp = prompt("输入价格", row[name]);
                                try {
                                    var price = parseFloat(temp);
                                    if (!isNaN(price)) {
                                        main.priceClick(row["id"], price);
                                    }
                                }
                                catch (e) {
                                    alert(e.message);
                                }
                            });
                        }
                        break;
                    case "Comment":
                        if (order.auth > 1 || order.currentUser == row["Name"]) {
                            td.addClass("clickable");
                            td.click(function () {
                                var temp = prompt("输入备注", row[name]);
                                try {
                                    if (temp != null && temp != undefined) {
                                        main.commentClick(row["id"], temp);
                                    }
                                }
                                catch (e) {
                                    alert(e.message);
                                }
                            });
                        }
                        break;
                    case "Pay":
                        if (order.auth > 1) {
                            td.addClass("clickable");
                            td.click(function () {
                                var temp = prompt("输入付款", row[name]);
                                try {
                                    var pay = parseFloat(temp);
                                    if (!isNaN(pay)) {
                                        main.payClick(row["id"], pay);
                                    }
                                }
                                catch (e) {
                                    alert(e.message);
                                }
                            });
                        }
                        break;
                }
            }
        });
        table.before("<h1>午餐</h1>")
    },
    buildSumList: function (table, data) {
        table.gridTable({
            fk: "id",
            columns: ["餐类", "负责人", "店编号", "付款"],
            showfk: false,
            dataSource: data
        });
    },
    refreshOrderList: function () {
        order.getOrderList(
            function (data) {
                if (data.Table2.length == 0) {
                    main.buildSetManagerButton();
                }
                else {
                    var table = $("#order").find("table.orderList");
                    if (table.length == 0) {
                        $("#order").empty();
                        table = $('<table cellspacing=5 class="orderList"/>').appendTo($("#order"));
                        data.Table1;
                        main.buildOrderList(table, data.Table1);
                    }
                    else {
                        table.data("table").refresh(data.Table1);
                    }

                    var table2 = $("#order").find("table.sumList");
                    if (table2.length == 0) {
                        table2 = $('<table cellspacing=5 class="sumList"/>').appendTo($("#order"));
                        data.Table1;
                        main.buildSumList(table2, data.Table2);
                    }
                    else {
                        table2.data("table").refresh(data.Table2);
                    }
                }
            },
            function (err) {
                $("#order").html("载入列表失败:" + err);
            });
    },
    priceClick: function (id, price) {
        order.setPrice(function () {
            main.refreshOrderList();
        }, function (err) { alert(err); }, id, price);
    },
    payClick: function (id, pay) {
        order.setPay(function () {
            main.refreshOrderList();
        }, function (err) { alert(err); }, id, pay);
    },
    commentClick: function (id, comment) {
        order.setComment(function () {
            main.refreshOrderList();
        }, function (err) { alert(err); }, id, comment);
    },
    deleteClick: function (id) {
        order.deleteOrder(function () {
            main.refreshOrderList();
        }, function (err) { alert(err); }, id);
    },
    setManager: function () {
        order.setManager(
            function () {
                main.refreshOrderList();
            },
            function (err) {
                $("#order").html(err);
            });
    },
    addOrder: function (store, food, price) {
        order.addOrder(
            function () {
                main.refreshOrderList();
            },
            function (err) { alert(err); },
            store, food, price
            );

    },
    refreshMenu: function () {
        $("#menu").html("载入中");
        order.getMenu(
            function (data) {
                $("#menu").empty();
                if (data.type.length > 0) {
                    $.each(data.type, function (index, row) {
                        var div = $('<div class="type" name="' + row.code + '"></div>');
                        div.append("<h3>" + row.name + " 电话：" + row.phone + "</h3>").appendTo($("#menu"));
                    });

                    $.each(data.menu, function (index, row) {
                        var div = $('<div></div>');
                        div.data("row", row);
                        div.html(row.food + " " + row.price);
                        div.appendTo($("#menu").find('div[name="' + row.type + '"]'));
                        div.click(function (e) {
                            var data = $(e.currentTarget).data("row");
                            main.addOrder(data.type, data.food, data.price);
                        });
                    });
                }
            },
            function (err) {
                $("#menu").html(err);
            }
            );
    },
    bindSerchInputEvent: function () {
        $("#search").bind("input", function (e) {
            var value = e.currentTarget.value;
            $('#menu div.type>div:contains("' + value + '")').show();
            $('#menu div.type>div:not(:contains("' + value + '"))').hide();
        });
    },
    IsPC: function () {
        var userAgentInfo = navigator.userAgent;
        var Agents = new Array("Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod");
        var flag = true;
        for (var v = 0; v < Agents.length; v++) {
            if (userAgentInfo.indexOf(Agents[v]) > 0) { flag = false; break; }
        }
        return flag;
    }
};

