(function ($) {
    $.fn.gridTable = function (options) {
        var table = this;
        table.empty();

        var opts = {
            fk: "id",
            columns: [],
            showfk: false,
            dataSource: {},
            rowInsertCallback: null,
            cellInsertCallback:null,
        };
        $.extend(true, opts, options);

        var thead = $("<thead></thead>");
        var theadtr = $("<tr>").appendTo(thead);
        $.each(opts.columns, function (index, col) {
            if (opts.fk != col) {
                theadtr.append("<th>" + col + "</th>");
            }
        });

        table.append(thead);
        var tbody = $("<tbody></tbody>").appendTo(table);

        var refresh = function (data) {
            tbody.empty();
            $.each(data, function (index, row) {
                var tr = $("<tr></tr>").appendTo(tbody);
                tr.data(opts.fk, row[opts.fk]);
                for (var n in row) {
                    if (n == opts.fk && opts.showfk == false) {
                        continue;
                    }
                    else {
                        var td = $("<td></td>");
                        td.html(row[n]).appendTo(tr);

                        if ($.isFunction(opts.cellInsertCallback)) {
                            opts.cellInsertCallback(td, n, row[n], row);
                        }
                    }
                }
                if ($.isFunction(opts.rowInsertCallback)) {
                    opts.rowInsertCallback(tr, row);
                }
            });
        }
        refresh(opts.dataSource);
        var func = {
            refresh: refresh
        }

        table.data("table", func);
    }

})(jQuery)