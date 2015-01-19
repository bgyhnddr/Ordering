$.ajaxSetup({
    dataType: "json",
    complete:
        function (req, state) {
            if (state === "parsererror") {
                var form = $(req.responseText).filter("form")[0];
                if (form) {
                    var redict = form.action;
                    if (redict.indexOf("Login.aspx") > 0) {
                        window.location = window.location.origin + "/Login.aspx";
                    }

                }
            }
        }
});

var order = {
    currentUser: "",
    auth: 0,
    delteSvg: '<svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 100 100"><line x1="75" y1="25" x2="25" y2="75" style="stroke:red;stroke-width:10"/><line x1="25" y1="25" x2="75" y2="75" style="stroke:red;stroke-width:10"/></svg>',
    ajax: function (url, success, fail, data) {
        var dat = {};
        if (data) {
            dat = data;
        }

        $.ajax({
            url: url,
            data: dat,
            success: function (result) {
                if (result.success) {
                    if ($.isFunction(success)) {
                        success(result.data, result.message);
                    }
                }
                else {
                    if ($.isFunction(fail)) {
                        fail(result.message);
                    }
                }
            },
            error: function (req, text) {
                if ($.isFunction(fail)) {
                    fail(text);
                }
            }
        });
    },
    getCurrentUser: function (success, fail) {
        order.ajax("ASHX/GetCurrentUserInfo.ashx", success, fail);
    },
    getOrderList: function (success, fail) {
        order.ajax("ASHX/GetOrderList.ashx", success, fail);
    },
    setPrice: function (success, fail, id, price) {
        order.ajax("ASHX/UpdatePrice.ashx", success, fail, { "id": id, "price": price });
    },
    setPay: function (success, fail, id, pay) {
        order.ajax("ASHX/UpdatePay.ashx", success, fail, { "id": id, "pay": pay });
    },
    setComment: function (success, fail, id, comment) {
        order.ajax("ASHX/UpdateComment.ashx", success, fail, { "id": id, "comment": comment });
    },
    deleteOrder: function (success, fail, id) {
        order.ajax("ASHX/DeleteOrder.ashx", success, fail, { "id": id });
    },
    setManager: function (success, fail, manager) {
        order.ajax("ASHX/SetManager.ashx", success, fail, { "manager": manager });
    },
    getMenu: function (success, fail, manager) {
        order.ajax("ASHX/GetMenu.ashx", success, fail);
    },
    addOrder: function (success, fail, store, food, price) {
        order.ajax("ASHX/AddOrder.ashx", success, fail, { store: store, food: food, price: price });
    }
};