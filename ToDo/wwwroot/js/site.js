//#region DisplayView

$(document).ready(function () {
    var date = new Date();
    date = date.getTime() + (date.getTimezoneOffset() * 60000) + (3600000 * 6);
    const dateTimeNow = new Date(date).toISOString().slice(0, 16);
    $('#date').val(dateTimeNow);
});

$(document).ready(function () {

    //LockUnlockFormProject
    $('#lock-form-project-link').click(function () {
        if (!$('#edit-project-savebtn').prop('disabled')) {
            $('#edit-project-savebtn').prop('disabled', true);
            $(this).removeClass('bi-unlock unlock-link').addClass('bi-lock lock-link');
        }
        else {
            $('#edit-project-savebtn').prop('disabled', false);
            $(this).removeClass('bi-lock lock-link').addClass('bi-unlock unlock-link');
        }
    });

    $('#show-all-information-project-link').click(function () {
        $('.additional-info').toggleClass('info-hide');
    });

    //ModalLink
    $(".modal-link").click(function () {
        $.get(this.href, function (data) {
            $('#modal-dialog').html(data);
        });
    });

    //HideShowProjects
    $('#hidden-projects-link').click(function () {
        var hiddenVal = true;
        if ($('#hidden-projects-checkbox').is(':checked')) {
            $('#hidden-projects-link').removeClass('bi-eye show-link').addClass('bi-eye-slash hide-link');
            $('.project-hidden').removeClass('project-hidden-hide').addClass('project-hidden-show');
            $('#hidden-projects-checkbox').prop('checked', false);
            hiddenVal = false;
        }
        else {
            $('.project-hidden').removeClass('project-hidden-show').addClass('project-hidden-hide');
            $('#hidden-projects-link').removeClass('bi-eye-slash hide-link').addClass('bi-eye show-link');
            $('#hidden-projects-checkbox').prop('checked', true);
            hiddenVal = true;
        }
        //$.ajax({
        //    type: 'GET',
        //    url: "Home/_GetProjects",
        //    data: { hidden: hiddenVal },
        //    success: function (data) {
        //        $(".projects-list").html(data);
        //    }
        //});
    });

    //ShowDescriptionTask
    $('.taskTitle').click(function () {
        var taskId = $(this).data("taskid");
        var descrId = $('#description-' + taskId);
        var textAreaId = $('#textarea-task-description-' + taskId);
        if (descrId.hasClass('tr-nonactive')) {
            descrId.removeClass('tr-nonactive').addClass('tr-active');
        }
        else {
            descrId.removeClass('tr-active').addClass('tr-nonactive');
            textAreaId.prop('disabled', true);
        }
    });

    //LockUnlockDescriptionTask
    $('.lock-description-task').click(function () {
        var taskId = $(this).data("taskid");
        var descrBtn = $(`#description-${taskId} :button`);
        var descrTextArea = $('textarea', `#description-${taskId}`);
        if (descrTextArea.prop('disabled')) {
            $(this).removeClass('bi-lock lock-link').addClass('bi-unlock unlock-link');
            descrTextArea.prop('disabled', false);
            descrBtn.prop('disabled', false);
        }
        else {
            $(this).removeClass('bi-unlock unlock-link').addClass('bi-lock lock-link');
            descrTextArea.prop('disabled', true);
            descrBtn.prop('disabled', true);
        }
    });
});

//#endregion
