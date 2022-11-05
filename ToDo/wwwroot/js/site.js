//#region DisplayView

//DisableUnDisableInput
$(document).ready(function () {
    $('#edit-project-link').click(function () {
        if (!$('#edit-project-form :input').prop('disabled')) {
            $('#edit-project-form :input').prop('disabled', true);
        }
        else {
            $('#edit-project-form :input').prop('disabled', false);
        }
    });
});

$(document).ready(function () {
    $(".create-link").click(function (e) {
        $.get(this.href, function (data) {
            $('#create-dialog').html(data);
        });
    });
});
$(document).ready(function () {
    $(".edit-link").click(function (e) {
        $.get(this.href, function (data) {
            $('#edit-dialog').html(data);
        });
    });
});
//$(document).ready(function () {
//    $(".delete-link").click(function (e) {
//        $.get(this.href, function (data) {
//            $('#delete-dialog').html(data);
//        });
//    });
//});


//DeleteTask
$(document).ready(function () {
    $(".delete-task-link").click(function () {
        var taskid = $(this).data("taskId");
        $('#delete-task-id').val(taskid);
    });
});

//#endregion 