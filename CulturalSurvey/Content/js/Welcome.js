$(document).ready(function () {
    $("#user_active").val("pg_Welcome");
    LoadWelcomeInstruction();

});
function LoadWelcomeInstruction() {
    $.ajax(
        {
            beforeSend: function () {
                $('#loader').show();
            },
            complete: function () {
                $('#loader').hide();
            },
            type: 'POST',
            cache: false,
            async: false,
            dataType: 'JSON',
            url: '/Survey/GetWelcomeInstruction',
            data: {},
            contentType: 'application/json; charset=utf-8',
            success: function (res) {
                if (res.Status) {
                    if (res.list != undefined && res.list != null && res.list.isEnabled != 0) {

                        $('#lblSurvey_Name').text(res.list.Survey_Name);
                        $('#lblKan_Survey_Name').text(res.list.Kan_Survey_Name);
                        $('#lblTel_Survey_Name').text(res.list.Tel_Survey_Name);
                        $('#pnlEng_welcome').append(res.list.Eng_Welcome);
                        $('#pnlkan_welcome').append(res.list.Kan_Welcome);
                        $('#pnlTel_welcome').append(res.list.Tel_Welcome);
                        $('#pnlEng_instruction').append(res.list.Eng_Instruction);
                        $('#pnlkan_instruction').append(res.list.Kan_Instruction);
                        $('#pnlTel_instruction').append(res.list.Tel_Instruction);
                        $("#ftr_welcome").show();
                    }
                    else {
                        error("Survey Is Not Available", "topCenter");
                    }
                }
            },
            error: function (response) {
                alert(response);
            }
        });
}
$("#btnInstruction").click(function () {
    $("#user_active").val("pg_Instruction");
    $("#pg_Instruction").fadeIn(500);
    $("#pg_Welcome").fadeOut(500);
});
$("#btnBackWelcome").click(function () {
    $("#user_active").val("pg_Welcome");
    $("#pg_Welcome").fadeIn(500);
    $("#pg_Instruction").fadeOut(500);
});
$("#Language_Code").change(function () {
    var tt = $("#user_active").val();
    if ($("#user_active").val() == "pg_Welcome") {
        translator($("#Language_Code").val());
    }
    else if ($("#user_active").val() == "pg_Instruction") {
        translator($("#Language_Code").val());
    }
});
function translator(LanguageCode) {
    if (LanguageCode == "English") {
        $('.eng').show();
        $('.kan').hide();
        $('.tel').hide();
    }
    else if (LanguageCode == "Kannada") {
        $('.eng').hide();
        $('.kan').show();
        $('.tel').hide();
    }
    else if (LanguageCode == "Telugu") {
        $('.eng').hide();
        $('.kan').hide();
        $('.tel').show();
    }
    else {
        $('.eng').show();
        $('.kan').hide();
        $('.tel').hide();
    }
}