$(document).ready(
    $('.detalhar').click(function () {

        //$('#detalhes').modal('show');
    })
);


function retornoLog(html) {
    var m = $('.detalhes');
    m.find('.modal-body').html(html.trim())
    m.modal('show');
}



$(function () {
    $("#aluno").change(function () {
        $("#disciplina").empty();
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Notas/carregaDisciplinas",
            data: { aluno: $("#aluno").val() },
            success: function (dados) {
                $("#disciplina").append("<option value='0'>Selecione...</option>");
                $(dados).each(function (i) {
                    $("#disciplina").append("<option value='" + dados[i].Value + "'>" + dados[i].Text + "</option>");
                });
            }
        });
    });
});


$(function () {
    $("#universidade").change(function () {
        $("#curso").empty();
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Aluno/carregaCurso",
            data: { universidade: $("#universidade").val() },
            success: function (dados) {
                $("#curso").append("<option value='0'>Selecione...</option>");
                $(dados).each(function (i) {
                    $("#curso").append("<option value='" + dados[i].Value + "'>" + dados[i].Text + "</option>");
                });
            }
        });
    });
});



function mascaraData(campoData) {
    var data = campoData.value;
    if (data.length == 2) {
        data = data + '/';
        document.forms[0].data.value = data;
        return true;
    }
    if (data.length == 5) {
        data = data + '/';
        document.forms[0].data.value = data;
        return true;
    }
}
