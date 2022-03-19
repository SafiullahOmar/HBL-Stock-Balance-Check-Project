// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });

    $('#tbl').dataTable({
        "paging": true,
        dom: 'Bfrtip',
        buttons: [
            'pageLength', 'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
});
showIn = (url, title) => {
  //  alert(url);

    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            //console.log(res);
            $('#form-modal').modal('show');

                $('.date').datepicker({
                    format: 'mm/dd/yyyy',
                    autoclose: true,
                    orientation: 'bottom left'
                });
            $('.select').select2();


            // .datepicker("setDate", new Date());
            // to make popup draggable
            //$('.modal-dialog').draggable({
            //    handle: ".modal-header"
            //});
        }
    })
}
showInPopup = (url, title) => {

    //alert(url);

    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            //console.log(res);
            $('#form-modal').modal('show');

            $('.date').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true,
                orientation: 'bottom left'
            });

            $('.select').select2();
            // .datepicker("setDate", new Date());
            // to make popup draggable
            //$('.modal-dialog').draggable({
            //    handle: ".modal-header"
            //});
        }
    })
}
jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all1').html(res.html);
                    //$('#view-all1').find('a.lnkAdd').bind('click', function () {
                    //   // $("#Id").val('0');
                    //    showIn("/Admin/ProductOrOrder/orderAddOrEdit/0", "tile");
                    //});
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    //$('.lnkAdd').prop("href", showInPopup('/Admin/ProductOrOrder/orderAddOrEdit/0', 'New Purchase/Sell Order'));
                    $('#form-modal').modal('hide');

                    $.notify("Submitted Successfully ", { globalPosition: 'top center', className: 'success' });

                    // $("#Id").val('0');
                    $("a[name='add']").on('click', function () {
                        showIn("/Admin/ProductOrOrder/orderAddOrEdit/0", "Add Order");
                    });
                   // alert(res.update);
                    if (res.update) {
                        location.reload();
                    }
                    $('#tbl').dataTable({
                    
                        dom: 'Bfrtip',
                        buttons: [
                            'pageLength', 'copy', 'csv', 'excel', 'pdf', 'print'
                        ]
                    });
                    $('.select').select2();
                    
                }
                else {
                  //  alert(res.isValid);
                   // alert(res.html);
                    $('#form-modal .modal-body').html(res.html);

                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
jQueryAjaxDelete = form => {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all1').html(res.html);
                    $.notify("Removed Successfully ", { globalPosition: 'top center', className: 'success' });

                    $('#tbl').dataTable({
                        
                        dom: 'Bfrtip',
                        buttons: [
                            'pageLength', 'copy', 'csv', 'excel', 'pdf', 'print'
                        ]
                    });
                    $('.select').select2();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}