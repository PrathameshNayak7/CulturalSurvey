//------Index----
$(document).ready(function () {

    LoadMasters("Department", $("#Department"), 'Select Department');
    LoadMasters("Location", $("#WorkLocation"), 'Select Location');
    LoadMasters("Levels", $("#Level"), 'Select Level');
    LoadMasters("Tenure", $("#Tenure"), 'Select Tenure');
    LoadMasters("Age", $("#Age"), 'Select Age');
    GetMainSaved();
    $("#user_active").val("pg_Selection");
    $("#selections").show();
});
function LoadMasters(MasterName, control, placeholder) {
    control.empty();
    $.ajax({
        beforeSend: function () {
            $('#loader').show();
        },
        complete: function () {
            $('#loader').hide();
        },
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Survey/GetMasterTables',
        data: { MasterName: MasterName },
        success: function (data) {
            if (data.list.length == 1) {
                $.each(data.list, function (i, obj) {
                    control.append('<option value="' + obj.FieldValue + '">' + obj.FieldText + '</option>');
                });
            }
            else if (data.list.length > 1) {
                control.append('<option value="">' + placeholder + '</option>');
                $.each(data.list, function (i, obj) {
                    control.append('<option value="' + obj.FieldValue + '">' + obj.FieldText + '</option>');
                });

            }
            else {
                error(MasterName + " are not Available ", "topCenter");

            }
        },
        error: function (ex) {
            error(" Failed to Load " + MasterName + ex.Message, "topCenter");
            $('#loader').hide();
        }
    });
}
function GetMainSaved() {
    $.ajax({
        beforeSend: function () {
            $('#loader').show();
        },
        complete: function () {
            $('#loader').hide();
        },
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Survey/Get_Response_Main',
        data: {},
        success: function (data) {
            if (data.list != null) {
                $("#Department").val(data.list.Department);
                $("#WorkLocation").val(data.list.Work_Location);
                $("#Level").val(data.list.Levels);
                $("#Tenure").val(data.list.Tenuer);
                $("#Age").val(data.list.Age);
                $('input[name="Gender"][value="' + data.list.Gender + '"]').attr('checked', true);
                $('#Language_Code').val(data.list.Language);

                //$("#Department option:contains(" + data.list.Department + ")").attr('selected', 'selected');              
                //$("#WorkLocation option:contains(" + data.list.Work_Location + ")").attr('selected', 'selected');
                //$("#Level option:contains(" + data.list.Levels + ")").attr('selected', 'selected');
                //$("#Tenure option:contains(" + data.list.Tenuer + ")").attr('selected', 'selected');
                //$("#Age option:contains(" + data.list.Age + ")").attr('selected', 'selected');
             
            }
        },
        error: function (ex) {
            error(" Failed to Get data " + ex.Message, "topCenter");
            $('#loader').hide();
        }
    });
}
$("#Language_Code").change(function () {
    if ($("#user_active").val() == "pg_Questions") {
        translator($("#Language_Code").val());
    }
    else if ($("#user_active").val() == "pg_Selection") {
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
//------------Selection Part-----------
$("#nextQuestion").click(function () {
    if (pg_select_validation() != 1) {
        var vmA = {
            'Department': $('#Department').val(),
            'Gender': $("input[name='Gender']:checked").val(),
            'Work_Location': $('#WorkLocation').val(),
            'Levels': $('#Level').val(),
            'Tenuer': $('#Tenure').val(),
            'Age': $('#Age').val(),
            'Language': $('#Language_Code').val()
        };

        $.ajax({
            beforeSend: function () {
                $('#loader').show();
            },
            complete: function () {
                $('#loader').hide();
            },
            error: function (ex) {
                error("Failed to Save Demograp" + ex.Message, "topCenter");
                $('#loader').hide();
            },
            type: 'POST',
            url: '/Survey/Save_Response_Main',
            dataType: 'json',
            async: false,
            data: { Main: vmA },
            success: function (data) {
                if (data.Status != false) {
                    success("welcome to survey", "topCenter");
                    $("#user_active").val("pg_Questions");
                    LoadQuestions();
                    $("#selections").hide();
                    $("#questions").show();
                }
            }
        });
    }
    else {

    }
});
function pg_select_validation() {
    var status = 0;
    if (($("#Department").val() == "") || ($("#Department").val() == undefined)) {
        error("select the Department", "topCenter");
        $("#Department").attr("style", "border:solid;border-color:orangered;");
        $("#Department").focus();
        status = 1;
        return status;
    }
    else if ($("input[name='Gender']:checked").val() == undefined) {
        error("select the Gender", "topCenter");
        $("#Gender").attr("style", "border:solid;border-color:orangered;");
        $('#Gender').focus();
        status = 1;
        return status;
    }
    else if (($("#WorkLocation").val() == "") || ($("#WorkLocation").val() == undefined)) {
        error("select the Work Location", "topCenter");
        $("#WorkLocation").attr("style", "border:solid;border-color:orangered;");
        $("#WorkLocation").focus();
        status = 1;
        return status;
    }
    else if (($("#Level").val() == "") || ($("#Level").val() == undefined)) {
        error("select the Level", "topCenter");
        $("#Level").attr("style", "border:solid;border-color:orangered;");
        $("#Level").focus();
        status = 1;
        return status;
    }
    else if (($("#Tenure").val() == "") || ($("#Tenure").val() == undefined)) {
        error("select the Tenure", "topCenter");
        $("#Tenure").attr("style", "border:solid;border-color:orangered;");
        $("#Tenure").focus();
        status = 1;
        return status;

    }
    else if (($("#Age").val() == "") || ($("#Age").val() == undefined)) {
        error("select the Age", "topCenter");
        $("#Age").attr("style", "border:solid;border-color:orangered;");
        $("#Age").focus();
        status = 1;
        return status;
    }
    else {
        return status;
    }
}
function onSelect(control) {
    //   $(control).removeAttr("style", "border:solid;border-color:orangered;");
    document.getElementById(control).removeAttribute("style", "border:solid;border-color:orangered;");

}