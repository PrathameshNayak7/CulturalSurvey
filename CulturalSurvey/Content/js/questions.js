$("#user_active").val("pg_Questions");
function LoadQuestions() {
    $('#tblQuestions tbody').empty();
    var table = "";
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
        url: '/Survey/GetMasterQuestions',
        data: {},
        success: function (data) {
            if (data.list.length > 0) {
                for (var i = 0; i < data.list.length; i++) {
                    table += '<tr class=' + data.list[i]["Page_Group"] + '>';
                    table += '<td>' + (i + 1) + ' ' + (data.list[i]["isSaved"] == 1 ? "<img src='../../Content/images/Completed.png' id='img" + data.list[i]["QSlno"] + "'  style='width: 24px; height: 24px;display:block;' />" : "<img src='../../Content/images/Completed.png' id='img" + data.list[i]["QSlno"] + "'  style='width: 24px; height: 24px;display:none;' />") + ' </td>';
                    table += '<td><input type="hidden" id="QSlno" value=' + data.list[i]["QSlno"] + '><input type="hidden" id="isOptions" value=' + data.list[i]["isOptions"] + '><input type="hidden" id="RSlno" value=' + data.list[i]["RSlno"] + '>';

                    table += '<div  id="pnlrdo' + data.list[i]["QSlno"] + '" style="padding:5px;" class="model-body"><h4 style="font-family:Muli;letter-spacing:2px;" class="eng"><lable>' + data.list[i]["Q_Code_Display"] + ' ' + data.list[i]["English"] + '&nbsp;<span style="color:red;">* </span></lable></h4>';
                    table += '<h4 style="display:none;font-family:Muli;letter-spacing:2px;" class="kan"><lable>' + data.list[i]["Q_Code_Display"] + ' ' + data.list[i]["kannada"] + '&nbsp;<span style="color:red;">* </span></lable></h4>';
                    table += '<h4 style="display:none;font-family:Muli;letter-spacing:2px;" class="tel"><lable>' + data.list[i]["Q_Code_Display"] + ' ' + data.list[i]["Telugu"] + '&nbsp;<span style="color:red;">* </span></lable></h4>';
                    if (data.list[i]["isOptions"] == 1) {
                        table += '<span style="font-size: large;"> <input ' + (data.list[i]["Rating"] == 5 ? "checked" : "") + ' type="radio"  name="Rating' + data.list[i]["QSlno"] + '" id=' + data.list[i]["QSlno"] + ' onclick="ValidateRemark(' + data.list[i]["QSlno"] + ',this.value,' + data.list[i]["RSlno"] + ')"  value="5" ><label  class="eng">&nbsp; Strongly Agree</label><label class="kan" style="display:none;">&nbsp; ಬಲವಾಗಿ ಒಪ್ಪುತ್ತೇನೆ</label><label class="tel" style="display:none;">&nbsp; బలంగా నమ్ముతున్నాను</label>  <br />';
                        table += ' <input ' + (data.list[i]["Rating"] == 4 ? "checked" : "") + ' type="radio" name="Rating' + data.list[i]["QSlno"] + '" id=rdo' + data.list[i]["QSlno"] + ' onclick="ValidateRemark(' + data.list[i]["QSlno"] + ',this.value,' + data.list[i]["RSlno"] + ')" value="4"><label class="eng">&nbsp; Agree</label><label class="kan" style="display:none;">&nbsp; ಒಪ್ಪುತ್ತೇನೆ</label><label class="tel" style="display:none;">&nbsp; అంగీకరిస్తున్నారు</label><br />';
                        table += ' <input ' + (data.list[i]["Rating"] == 3 ? "checked" : "") + ' type="radio"  name="Rating' + data.list[i]["QSlno"] + '" id=' + data.list[i]["QSlno"] + '  onclick="ValidateRemark(' + data.list[i]["QSlno"] + ',this.value,' + data.list[i]["RSlno"] + ')" value="3"><label class="eng" >&nbsp; Somewhat Agree</label><label class="kan" style="display:none;">&nbsp; ಸ್ವಲ್ಪಮಟ್ಟಿಗೆ ಒಪ್ಪುತ್ತೇನೆ</label><label class="tel" style="display:none;">&nbsp; కొంతవరకు అంగీకరిస్తున్నారు</label><br />';
                        table += ' <input ' + (data.list[i]["Rating"] == 2 ? "checked" : "") + ' type="radio"  name="Rating' + data.list[i]["QSlno"] + '" id=' + data.list[i]["QSlno"] + '  onclick="ValidateRemark(' + data.list[i]["QSlno"] + ',this.value,' + data.list[i]["RSlno"] + ')"  value="2"><label class="eng" >&nbsp; Disagree</label><label class="kan" style="display:none;">&nbsp; ಒಪ್ಪುವುದಿಲ್ಲ</label><label class="tel" style="display:none;">&nbsp; అంగీకరించలేదు</label><br />';
                        table += ' <input ' + (data.list[i]["Rating"] == 1 ? "checked" : "") + ' type="radio"  name="Rating' + data.list[i]["QSlno"] + '"  id=' + data.list[i]["QSlno"] + '  onclick="ValidateRemark(' + data.list[i]["QSlno"] + ',this.value,' + data.list[i]["RSlno"] + ')" value="1"><label class="eng"> &nbsp; Strongly Disagree</label><label class="kan" style="display:none;">&nbsp; ಖಂಡಿತವಾಗಿ ಒಪ್ಪುವುದಿಲ್ಲ</label><label class="tel" style="display:none;">&nbsp; తీవ్రంగా విభేదిస్తున్నారు</label></span></div>';
                        table += '<div id="pnlRemark' + data.list[i]["QSlno"] + '"  ' + (data.list[i]["Rating"] == 1 || data.list[i]["Rating"] == 2 ? (data.list[i]["Comments"] == undefined || data.list[i]["Comments"] == '' ? "style='display: block;background-color:#fee9e9;'" : "style='display: block;'") : "style='display: none;'") + '  ><br /> <label class="eng">' + data.list[i]["Q_Code_Display"] + ' ' + data.list[i]["EQR"] + ' </label><label class="kan" style="display:none;">' + data.list[i]["Q_Code_Display"] + ' ' + data.list[i]["KQR"] + ' </label><label class="tel" style="display:none;">' + data.list[i]["Q_Code_Display"] + ' ' + data.list[i]["TQR"] + ' </label><textarea  ' + (data.list[i]["Comments"] == undefined || data.list[i]["Comments"] == '' ? "style='display:border:solid;border-color:#e46d6d;'" : "") + '  rows="3"   class="form-control z-depth-1"  id=Remark' + data.list[i]["QSlno"] + ' name="Rating_Remark" onfocusout="commentupdate(' + data.list[i]["QSlno"] + ',this.name,' + data.list[i]["RSlno"] + ')" onkeypress="RemarkColor(this.id)" onkeyup="RemarkColor(this.id)" >' + data.list[i]["Comments"] + '</textarea></br></div><br />';
                        var a = data.list[i]["Rating"];
                        var ss = data.list[i]["Comments"];
                        if (data.list[i]["Rating"] == 1 || data.list[i]["Rating"] == 2) {

                            var ss = "#Remark" + data.list[i]["QSlno"];
                       
                            $("#Remark" + data.list[i]["QSlno"]).show();
                        }
                        else {

                            $("#pnlRemark" + data.list[i]["QSlno"]).hide();
                        }

                    }
                    else {
                        table += '<div id="pnlComment' + data.list[i]["QSlno"] + '" ><textarea name="comment" rows="3"  ' + (data.list[i]["Comments"] == undefined || data.list[i]["Comments"] == '' ? "style='display:border:solid;border-color:#e46d6d;'" : "") + '    class="form-control z-depth-1"  id=Comment' + data.list[i]["QSlno"] + '  onfocusout="commentupdate(' + data.list[i]["QSlno"] + ',this.name,' + data.list[i]["RSlno"] + ');" onkeypress="RemarkColor(this.id)" onkeyup="RemarkColor(this.id)"  >' + data.list[i]["Comments"] + '</textarea></br></div></div><br />';
                    }

                    table += '</td>';
                    table += '</tr>';
                }
                $('#tblQuestions tbody').html(table);

                $(".pg_1").show();
                $(".pg_2").hide();
                $(".pg_3").hide();
                $(".pg_4").hide();
                $(".pg_5").hide();
                $('#pnlNext').html("");
                $('#pnlPrevious').html("");
                /* $("#qtnNext").on('click', { pg_next: "2", pg_Validate: "pg_1" }, Pagination);*/
                $('#pnlNext').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(2, ' + "'pg_1'" + ',1)">Next <i class="fa fa-chevron-right"></i></a>');

                translator($("#Language_Code").val());
                /*  $('#loader').hide();*/
            }
            else {
                table = '<tr style="background-color:brown;color:white;"><td colspan=5><center>No Questions Found</center></td></tr>';
                $('#tblQuestions tbody').html(table);
                /* $('#loader').hide();*/
            }

        },
        error: function (ex) {
            error("Failed to load" + ex.Message, "topCenter");
            $("#tblQuestions tbody").empty();
            table = '<tr style="background-color:brown;color:white;"><td colspan=5><center>Error Occured</center></td></tr>';
            $('#tblQuestions tbody').html(table);
            /*   $('#loader').hide();*/
        }
    });
}



function paging(paging) {
    var pg = "pg_";
    var p = "p";

    /*   $("." + pg).fadeIn();*/
    for (let i = 1; i <= 5; i++) {
        if (i == paging) {
            $("." + pg + i).fadeIn();
        }
        else {
            $("." + pg + i).fadeOut();
        }

    }
    if (paging == 5) {
        $("#pnlFinal").show();
    }
    else {
        $("#pnlFinal").hide();
    }

}



$("#hm").click(function () {
    $("#selections").show();
    $("#questions").hide();
});



function Next_Pagination(pg_next, pg_Validate, Type) {

    var Validate = pg_Validate;
    var next = pg_next;
    var v = 0;
    if (Type == 1) {
        if ($("." + Validate).length > 0) {
            v = PagingValidation(Validate);
            if (v == 0) {
                // $("." + pg_class).fadeIn();
                common_Pagination(next);
            }
        }
    }
    else if (Type == 0) {
        common_Pagination(next);
    }
}

function common_Pagination(next) {
    if (next == 1) {
        $('#pnlNext').html("");
        $('#pnlNext').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(2,' + "'pg_1'" + ',1)">Next <i class="fa fa-chevron-right"></i></a>');
        $('#pnlPrevious').html("");
        paging(next);
    }
    else if (next == 2) {
        $('#pnlNext').html("");
        $('#pnlNext').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(3,' + "'pg_2'" + ',1)">Next <i class="fa fa-chevron-right"></i></a>');

        $('#pnlPrevious').html("");
        $('#pnlPrevious').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(1,' + "'pg_2'" + ',0)"><i class="fa fa-chevron-left"></i> back</a>');

        paging(next);
    }
    else if (next == 3) {
        $('#pnlNext').html("");
        $('#pnlNext').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(4,' + "'pg_3'" + ',1)">Next <i class="fa fa-chevron-right"></i></a>');

        $('#pnlPrevious').html("");
        $('#pnlPrevious').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(2,' + "'pg_3'" + ',0)"><i class="fa fa-chevron-left"></i> back</a>');


        paging(next);
    }
    else if (next == 4) {
        $('#pnlNext').html("");
        $('#pnlNext').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(5,' + "'pg_4'" + ',1)">Next <i class="fa fa-chevron-right"></i></a>');

        $('#pnlPrevious').html("");
        $('#pnlPrevious').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(3,' + "'pg_4'" + ',0)"><i class="fa fa-chevron-left"></i> back</a>');
        paging(next);
    }
    else if (next == 5) {
        $('#pnlNext').html("");
        $('#pnlPrevious').html("");
        $('#pnlPrevious').append('<a id="btnNext" class="btn btn-primary" onclick="Next_Pagination(4,' + "'pg_5'" + ',0)"><i class="fa fa-chevron-left"></i> back</a>');
        paging(next);
    }
}
$("#Final_Save").click(function () {

    if (PagingValidation("pg_5") == 0) {
        $("#pop_Confirm").modal("show");
    }

});
$("#Final_Submission").click(function () {

    $.ajax({
        beforeSend: function () {
            $('#loader').show();
        },
        complete: function () {
            $('#loader').hide();
        },
        type: 'POST',
        url: '/Survey/Submit_Final_Survey',
        dataType: 'json',
        async: false,
        data: {},
        success: function (data) {
            if (data.Status == true) {
                if (data.Message == "updated") {
                    success("Save Successfully", "topCenter");
                    window.location.replace("/Survey/Thankyou");
                }
            }
        },
        error: function (ex) {
            error("Failed to Submit the survey" + ex.Message, "topCenter");
            $('#loader').hide();
        }
    });
});


function ResponseValidation() {
    var status = 0;
    $('#tblQuestions > tbody  > tr').each(function () {
        var $fieldset = $(this);
        var isOptions = $('input[id="isOptions"]', $fieldset).val();
        var id = $('input[id="QSlno"]', $fieldset).val();
        var Rating = $(this).find("input[name='Rating" + id + "']:checked").val();
        var Remark = "Remark" + id;
        var pnlrdo = "pnlrdo" + id;
        if (isOptions == 1) {
            var Rating_Remark = $("#" + Remark).val();
            if ((Rating == 2 || Rating == 1) && (Rating_Remark == "" || Rating_Remark == undefined)) {
                $("#" + Remark).focus();

                status = 1;
                return status;
                return false;

            }
            if (Rating == "" || Rating == undefined) {


                $("#" + pnlrdo).attr("style", "background-color:#fee9e9;padding:5px;");
                $("#" + pnlrdo).focus();
                status = 1;
                return status;
                return false;
            }
        }
        else {
            var pnlComment = "pnl" + id;
            var Comment = "Comment" + id;
            var t = $("#" + Comment).val();
            if ($("#" + Comment).val() == "" || $("#" + Comment).val() == undefined) {

                $("#" + pnlComment).attr("style", "background-color:#fee9e9;");
                $("#" + Comment).attr("style", "border:solid;border-color:#e46d6d;");
                $("#" + Comment).focus();
                status = 1;
                return status;
                return false;
            }
            else {
                $("#" + pnlComment).removeAttr("style", "background-color:#fee9e9;");
                $("#" + Comment).removeAttr("style", "border:solid;border-color:#e46d6d;");

            }
        }
    });

    return status;
}

function PagingValidation(pg_class) {
    var status = 0;
    $('.' + pg_class).each(function () {
        var $fieldset = $(this);
        var isOptions = $('input[id="isOptions"]', $fieldset).val();
        var id = $('input[id="QSlno"]', $fieldset).val();
        var Rating = $(this).find("input[name='Rating" + id + "']:checked").val();
        var Remark = "Remark" + id;
        var pnlrdo = "pnlrdo" + id;
        if (isOptions == 1) {
            var Rating_Remark = $("#" + Remark).val();
            if ((Rating == 2 || Rating == 1) && (Rating_Remark == "" || Rating_Remark == undefined)) {
                error("Enter The remark", "topCenter");
                $("#" + Remark).focus();

                status = 1;

                return false;

            }
            if (Rating == "" || Rating == undefined) {
                $("#" + pnlrdo).attr("style", "background-color:#fee9e9;padding:5px;");
                error("Select The Rating", "topCenter");
                $("#" + pnlrdo).focus();
                status = 1;

                return false;
            }
        }
        else {
            var pnlComment = "pnl" + id;
            var Comment = "Comment" + id;
            var t = $("#" + Comment).val();
            if ($("#" + Comment).val() == "" || $("#" + Comment).val() == undefined) {

                $("#" + pnlComment).attr("style", "background-color:#fee9e9;");
                $("#" + Comment).attr("style", "border:solid;border-color:#e46d6d;");
                error("Enter The Comments", "topCenter");
                $("#" + Comment).focus();
                status = 1;

                return false;
            }
            else {
                $("#" + pnlComment).removeAttr("style", "background-color:#fee9e9;");
                $("#" + Comment).removeAttr("style", "border:solid;border-color:#e46d6d;");

            }
        }
    });
    return status;

}
function ValidateRemark(id, value, slno) {
    var pnlRemark = "pnlRemark" + id;
    var Remark = "Remark" + id;
    var pnlrdo = "pnlrdo" + id;
    var img = "img" + id;
    if ($("#" + Remark).val() == "" || $("#" + Remark).val() == undefined) {
        $("#" + pnlRemark).attr("style", "background-color:#fee9e9;");
        $("#" + Remark).attr("style", "border:solid;border-color:#e46d6d;");
    }
    else {
        $("#" + pnlRemark).removeAttr("style", "background-color:#fee9e9;");
        $("#" + Remark).removeAttr("style", "border:solid;border-color:#e46d6d;");
    }
    if (value == 1 || value == 2) {

        $("#" + pnlRemark).show();
    }
    else {

        $("#" + pnlRemark).hide();
    }
    $("#" + pnlrdo).removeAttr("style", "background-color:#fee9e9;padding:5px;");
    $.ajax({
        error: function (ex) {
            error("Failed to Save" + ex.Message, "topCenter");
        },
        type: 'POST',
        url: '/Survey/Auto_Save_Rating',
        dataType: 'json',
        async: false,
        data: { slno: slno, Rating: value, Remark: $("#" + Remark).val() },
        success: function (data) {
            if (data.Status != false) {
                if (value == 1 || value == 2) {

                    $("#" + img).hide();
                }
                else {

                    $("#" + img).show();
                }


            }
        }
    });
}

function commentupdate(id, source, slno) {
    var Remark = "Remark" + id;
    var comment = "Comment" + id;
    var img = "img" + id;

    if (source == "Rating_Remark" && $("#" + Remark).val() != "") {
        $.ajax({
            error: function (ex) {
                error("Failed to Save" + ex.Message, "topCenter");
            },
            type: 'POST',
            url: '/Survey/Auto_Save_Comment',
            dataType: 'json',
            async: false,
            data: { slno: slno, Comment: $("#" + Remark).val() },
            success: function (data) {
                if (data.Status != false) {
                    $("#" + img).show();
                }
            }
        });
    }
    else if (source == "comment") {
        $.ajax({
            error: function (ex) {
                error("Failed to Save" + ex.Message, "topCenter");
            },
            type: 'POST',
            url: '/Survey/Auto_Save_Comment',
            dataType: 'json',
            async: false,
            data: { slno: slno, Comment: $("#" + comment).val() },
            success: function (data) {
                if (data.Status != false) {
                    $("#" + img).show();
                }
            }
        });

    }
}
function RemarkColor(id) {
    var pnlRemark = "pnl" + id;
    var Remark = id;

    var t = $("#" + Remark).val();
    if ($("#" + Remark).val() == "" || $("#" + Remark).val() == undefined) {
        $("#" + pnlRemark).attr("style", "background-color:#fee9e9;");
        $("#" + Remark).attr("style", "border:solid;border-color:#e46d6d;");
    }
    else {
        $("#" + pnlRemark).removeAttr("style", "background-color:#fee9e9;");
        $("#" + Remark).removeAttr("style", "border:solid;border-color:#e46d6d;");
    }
}

function CommentColor(id) {
    var pnlComment = "pnl" + id;
    var Comment = id;

    var t = $("#" + Comment).val();
    if ($("#" + Comment).val() == "" || $("#" + Comment).val() == undefined) {
        $("#" + pnlComment).attr("style", "background-color:#fee9e9;");
        $("#" + Comment).attr("style", "border:solid;border-color:#e46d6d;");
    }
    else {
        $("#" + pnlComment).removeAttr("style", "background-color:#fee9e9;");
        $("#" + Comment).removeAttr("style", "border:solid;border-color:#e46d6d;");
    }
}