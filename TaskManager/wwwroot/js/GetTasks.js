$(function () {
    $('#editTask').click(function () {
        if ($("#selectedTask").val() != '') {
            let taskInfo = document.getElementById('taskInfo');
            let taskEditInfo = document.getElementById('taskEditInfo');
            let saveTask = document.getElementById('saveTask');
            let taskEditIDInput = document.getElementById('taskEditIDInput');
            let registrationDate = document.getElementById('registrationDate');
            taskInfo.setAttribute("hidden", "hidden");
            taskEditInfo.removeAttribute("hidden");
            saveTask.removeAttribute("hidden");
            $.ajax({
                type: "GET",
                url: "@Url.Action("GetTaskTypes")",
                dataType: "json",
                success: function (msg) {
                    for (var i = 0; i < msg.length; i++) {
                        var opt = document.createElement('option');
                        opt.value = msg[i];
                        opt.innerHTML = msg[i];
                        taskEditType.appendChild(opt);
                    }
                }
            })
            $.ajax({
                type: "GET",
                url: "@Url.Action("GetTask")",
                data: { id: $("#selectedTask").val() },
                dataType: "json",
                success: function (msg) {
                    $('#taskEditID').html('ID: ' + msg.id);
                    $('#taskEditIDInput').val(msg.id);
                    $('#taskEditName').val(msg.name);
                    $('#taskEditDescription').val(msg.description);
                    $('#taskEditType').val(msg.typeTask);
                    $('#taskEditExecutors').val(msg.executorsList);
                }
            })
        }

    });
    $('#deleteTask').click(function () {
        $.ajax({
            type: "POST",
            async: false,
            url: "@Url.Action("Delete")",
            data: { id: $("#selectedTask").val() }
        });
        $('#tree').jstree({
            'core': {
                'data': {
                    'url': function (node) {
                        return '/Task/GetNodes';
                    },
                    'data': function (node) {
                        return { 'id': node.id };
                    }
                }
            }
        });
        $('#tree').jstree(true).refresh();
    });

    $('#tree')
        .on('changed.jstree', function (e, data) {
            var i, j, r = [];
            for (i = 0, j = data.selected.length; i < j; i++) {
                $.ajax({
                    type: "GET",
                    url: "@Url.Action("GetTask")",
                    data: { id: data.instance.get_node(data.selected[i]).id },
                    dataType: "json",
                    success: function (msg) {
                        console.log(msg.registrationDate);
                        $('#selectedTask').val(msg.id);
                        $('#taskID').html('ID: ' + msg.id);
                        $('#taskName').html('Название: ' + msg.name);
                        $('#taskDescription').html('Описание: ' + msg.description);
                        $('#taskExecutors').html('Список исполнителей: ' + msg.executorsList);
                        $('#registrationDate').html('Дата регистрации: ' + new Date(msg.registrationDate));
                        if (msg.completionDate != null) {
                            $('#completionDate').html('Дата завершения: ' + new Date(msg.completionDate));
                        }
                        else {
                            $('#completionDate').html('');
                        }
                        $('#taskType').html('Статус: ' + msg.typeTask);
                    },
                    error: function (msg) {
                        console.log(msg);
                    }
                });
            }
        })
        .jstree({
            'core': {
                'data': {
                    'url': function (node) {
                        return '/Task/GetNodes';
                    },
                    'data': function (node) {
                        return { 'id': node.id };
                    }
                }
            }
        });

}); 