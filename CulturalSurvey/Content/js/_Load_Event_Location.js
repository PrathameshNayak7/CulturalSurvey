function LoadState() {
    $("#State_Id").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Event/Load_State',
        data: {},
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#State_Id").append('<option value="' + obj.State_Id + '">' + obj.State_Name + '</option>');
                });
                LoadDistrict();
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#State_Id").append('<option value="">Select State</option>');
                $.each(data, function (i, obj) {
                    $("#State_Id").append('<option value="' + obj.State_Id + '">' + obj.State_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("No States Available", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load State" + ex.Message, "topCenter");
        }
    });
}
$("#State_Id").change(function () {
    $("#Taluka_Id").empty();
    $("#Taluka_Id").empty();
    if ($("#isCamuRegistration").val() == "0") {
        LoadDistrict();
    }
    else {
        LoadCamuDistrict();
        LoadCamuColleges();
    }
    

});
function LoadDistrict() {
    $("#District_Id").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Event/Load_District',
        data: { State_Id: $("#State_Id").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#District_Id").append('<option value="' + obj.District_Id + '">' + obj.District_Name + '</option>');
                });
                LoadTaluka();
                $('#loader').hide();
            }
            else if (data.length >= 1) {
                $("#District_Id").append('<option value="">Select District</option>');
                $.each(data, function (i, obj) {
                    $("#District_Id").append('<option value="' + obj.District_Id + '">' + obj.District_Name + '</option>');
                });

                $('#loader').hide();
            }
            else {
                $('#loader').hide();
                warning("District Not Available", "topCenter");
            }
        },
        error: function (ex) {
            error("Failed to Load District" + ex.Message, "topCenter");
            $('#loader').hide();

        }
    });
}
$("#District_Id").change(function () {
    $("#Taluka_Id").empty();
    $("#Village_Id").empty();
    if ($("#isCamuRegistration").val() == "0") {
        LoadTaluka();
    }
    else {
        LoadCamuTaluka();
        LoadCamuColleges();
    }
    

});
function LoadTaluka() {

    $("#Village_Id").empty();
    $("#Taluka_Id").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Event/Load_Taluka',
        data: { District_Id: $("#District_Id").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Taluka_Id").append('<option value="' + obj.Taluka_Id + '">' + obj.Taluka_Name + '</option>');
                });
                LoadVillage();
                $('#loader').hide();
            }
            else if (data.length >= 1) {
                $("#Taluka_Id").append('<option value="">Select Taluka</option>');
                $.each(data, function (i, obj) {
                    $("#Taluka_Id").append('<option value="' + obj.Taluka_Id + '">' + obj.Taluka_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                warning("Talukas are not Available", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Taluka" + ex.Message, "topCenter");
            $('#loader').hide();

        }
    });
}
$("#Taluka_Id").change(function () {
    if ($("#isCamuRegistration").val() == "0") {
        LoadVillage();
    }
    else {
        LoadCamuColleges();
    }

});
function LoadVillage() {
    $("#Village_Id").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Event/Load_Village',
        data: { Taluka_Id: $("#Taluka_Id").val() },
        success: function (data) {
            if (data.length >= 1) {
                $("#Village_Id").append('<option value="">Select Village</option>');
                $.each(data, function (i, obj) {
                    $("#Village_Id").append('<option value="' + obj.Village_Id + '">' + obj.Village_Name + '</option>');
                });
            }
            else {

                warning("Villages are not available", "topCenter");
            }

        },
        error: function (ex) {

            error("Failed to Load Villages" + ex.Message, "topCenter");

        }
    });
}

function LoadQualification() {
    $("#Qualification").empty();
    var event_Id = $("#hdnEventId").val();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Event/Load_Qualification',
        data: { Event_Id: $("#hdnEventId").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Qualification").append('<option value="' + obj.Qualification_Id + '">' + obj.Qualification_Name + '</option>');
                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Qualification").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#Qualification").append('<option value="' + obj.Qualification_Id + '">' + obj.Qualification_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("No Qualification Available", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Qualification" + ex.Message, "topCenter");
        }
    });
}

function LoadCourseType() {
    $("#CourseType").empty();
    var event_Id = $("#hdnEventId").val();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Event/Load_CourseType',
        data: { Event_Id: $("#hdnEventId").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#CourseType").append('<option value="' + obj.CourseType_Id + '">' + obj.CourseType_Name + '</option>');
                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#CourseType").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#CourseType").append('<option value="' + obj.CourseType_Id + '">' + obj.CourseType_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("Configure the Course Type", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Course Type" + ex.Message, "topCenter");
        }
    });
}



// Camu State,District,Taluka,CollegeType,College

function LoadCamuState() {
    $("#State_Id").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuStates',
        data: {},
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#State_Id").append('<option value="' + obj.State_Id + '">' + obj.State_Name + '</option>');
                });
                if ($("#isCamuRegistration").val() == "0") {
                    LoadDistrict();
                }
                else {
                    LoadCamuDistrict();
                    LoadCamuColleges();
                }
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#State_Id").append('<option value="">Select State</option>');
                $.each(data, function (i, obj) {
                    $("#State_Id").append('<option value="' + obj.State_Id + '">' + obj.State_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("No States Available", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load State" + ex.Message, "topCenter");
        }
    });
}
function LoadCamuColleges() {
    $("#College_Id").empty();
    if ($("#State_Id").val() != '' && $("#District_Id").val() != '' & $("#Taluka_Id").val() != '' && $("#CourseType").val() != '') {
        $.ajax({
            cache: false,
            async: false,
            type: 'POST',
            url: '/Creg/GetCamuColleges',
            data: { State_Id: $("#State_Id").val(), District_Id: $("#District_Id").val(), Taluka_Id: $("#Taluka_Id").val(), CollegeType_Id: $("#CourseType").val() },
            success: function (data) {
                if (data.length == 1) {
                    $.each(data, function (i, obj) {
                        if (obj.status == 'There are no College List') {
                            warning("College Not Available", "topCenter");

                        }
                        else {
                            $("#College_Id").append('<option value="' + obj.College_Id + '">' + obj.College_Name + '</option>');
                        }

                    });
                    $('#loader').hide();
                }
                else if (data.length >= 1) {
                    $("#College_Id").append('<option value="">Select College</option>');
                    $.each(data, function (i, obj) {
                        $("#College_Id").append('<option value="' + obj.College_Id + '">' + obj.College_Name + '</option>');
                    });

                    $('#loader').hide();
                }
                else {
                    $('#loader').hide();
                    warning("College Not Available", "topCenter");
                }
            },
            error: function (ex) {
                error("Failed to Load College" + ex.Message, "topCenter");
                $('#loader').hide();

            }
        });
    }
    else {

    }

}
$("#CourseType").change(function () {
    $("#College_Id").empty();

    if ($("#isCamuRegistration").val() == "0") {
        //   LoadTaluka();
    }
    else {
        LoadCamuColleges();
    }

});
function LoadCamuDistrict() {
    $("#District_Id").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Creg/GetCamuDistrict',
        data: { State_Id: $("#State_Id").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#District_Id").append('<option value="' + obj.District_Id + '">' + obj.District_Name + '</option>');
                });
                LoadTaluka();
                $('#loader').hide();
            }
            else if (data.length >= 1) {
                $("#District_Id").append('<option value="">Select District</option>');
                $.each(data, function (i, obj) {
                    $("#District_Id").append('<option value="' + obj.District_Id + '">' + obj.District_Name + '</option>');
                });

                $('#loader').hide();
            }
            else {
                $('#loader').hide();
                warning("District Not Available", "topCenter");
            }
        },
        error: function (ex) {
            error("Failed to Load District" + ex.Message, "topCenter");
            $('#loader').hide();

        }
    });
}

function LoadCamuTaluka() {

    $("#Village_Id").empty();
    $("#Taluka_Id").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Creg/GetCamuTaluka',
        data: { District_Id: $("#District_Id").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Taluka_Id").append('<option value="' + obj.Taluka_Id + '">' + obj.Taluka_Name + '</option>');
                });
                LoadVillage();
                $('#loader').hide();
            }
            else if (data.length >= 1) {
                $("#Taluka_Id").append('<option value="">Select Taluka</option>');
                $.each(data, function (i, obj) {
                    $("#Taluka_Id").append('<option value="' + obj.Taluka_Id + '">' + obj.Taluka_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                warning("Talukas are not Available", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Taluka" + ex.Message, "topCenter");
            $('#loader').hide();

        }
    });
}

function LoadCamuCourseType() {
    $("#CourseType").empty();
   
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuCollegeType',
        data: { },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#CourseType").append('<option value="' + obj.CourseType_Id + '">' + obj.CourseType_Name + '</option>');
                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#CourseType").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#CourseType").append('<option value="' + obj.CourseType_Id + '">' + obj.CourseType_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("Configure the Course Type", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Course Type" + ex.Message, "topCenter");
        }
    });
}

function LoadCamuInstitute() {
    $("#Institute").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuInstitutions',
        data: {},
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Institute").append('<option value="' + obj.Institution_Id + '">' + obj.Institution_Name + '</option>');
                });
                LoadCamuProgram();
                LoadCamuAcademic();
                LoadCamuStaffs();
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Institute").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#Institute").append('<option value="' + obj.Institution_Id + '">' + obj.Institution_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("Configure the Institute", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Institute" + ex.Message, "topCenter");
        }
    });
}

$("#Institute").change(function () {
    $("#Program").empty();
    $("#Degree").empty();
    LoadCamuProgram();
    LoadCamuAcademic();
    LoadCamuStaffs();

});

function LoadCamuProgram() {
    $("#Program").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuProgram',
        data: { Institute_Id: $("#Institute").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Program").append('<option value="' + obj.Program_Id + '">' + obj.Program_Name + '</option>');
                });
                LoadCamuDegree();
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Program").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#Program").append('<option value="' + obj.Program_Id + '">' + obj.Program_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("Configure the Courses", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Courses" + ex.Message, "topCenter");
        }
    });
}

$("#Program").change(function () {
    LoadCamuDegree();
});

function LoadCamuDegree() {
    $("#Degree").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuDegree',
        data: { Institute_Id: $("#Institute").val(), Program_Id: $("#Program").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Degree").append('<option value="' + obj.Degree_Id + '">' + obj.Degree_Name + '</option>');
                });
                LoadCamuStaffs();
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Degree").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#Degree").append('<option value="' + obj.Degree_Id + '">' + obj.Degree_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("Configure the Fellowship", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Fellowship" + ex.Message, "topCenter");
        }
    });
}

function LoadCamuStaffs() {
    $("#Incharge").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuStaff',
        data: { Institute_Id: $("#Institute").val(), Program_Id: $("#Degree").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    if (obj.status == 'There are no Staff') {
                        $("#Incharge").append('<option value="">[Select]</option>');

                    }
                    else {
                        $("#Incharge").append('<option value="' + obj.Staff_Id + '">' + obj.Staff_Name + '</option>');
                    }

                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Incharge").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#Incharge").append('<option value="' + obj.Staff_Id + '">' + obj.Staff_Name + '</option>');
                });

                $('#loader').hide();
            }
            else {
                error("Configure the Staffs", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Staffs" + ex.Message, "topCenter");
        }
    });
}

function LoadCamuAcademic() {
    $("#AdmissionYear").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuAcademic',
        data: { Institute_Id: $("#Institute").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    if (obj.status == 'There are no Year') {
                        $("#AdmissionYear").append('<option value="">[Select]</option>');
                    }
                    else {
                        $("#AdmissionYear").append('<option value="' + obj.Academic_Id + '">' + obj.Academic_Code + '</option>');
                    }

                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#AdmissionYear").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#AdmissionYear").append('<option value="' + obj.Academic_Id + '">' + obj.Academic_Code + '</option>');
                });

                $('#loader').hide();
            }
            else {
                error("Configure the Admission Year", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Admission Year" + ex.Message, "topCenter");
        }
    });
}


function LoadCamuQualification() {
    $("#Qualification").empty();
    var event_Id = $("#hdnEventId").val();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/Load_Qualification',
        data: { Event_Id: $("#hdnEventId").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Qualification").append('<option value="' + obj.Qualification_Id + '">' + obj.Qualification_Name + '</option>');
                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Qualification").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#Qualification").append('<option value="' + obj.Qualification_Id + '">' + obj.Qualification_Name + '</option>');
                });
                $('#loader').hide();
            }
            else {
                error("No Qualification Available", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Qualification" + ex.Message, "topCenter");
        }
    });
}


function LoadCamuClusterHead() {
    $("#Cluster_Head").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuClusterHead',
        data: { Event_Id: $("#Event").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    if (obj.status == 'There are no Staff') {
                        $("#Cluster_Head").append('<option value="">[Select]</option>');

                    }
                    else {
                        $("#Cluster_Head").append('<option value="' + obj.Staff_Id + '">' + obj.Staff_Name + '</option>');
                    }

                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Cluster_Head").append('<option value="0">[All]</option>');
                $.each(data, function (i, obj) {
                    $("#Cluster_Head").append('<option value="' + obj.Staff_Id + '">' + obj.Staff_Name + '</option>');
                });

                $('#loader').hide();
            }
            else {
                error("Configure the Staffs", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Staffs" + ex.Message, "topCenter");
        }
    });
}
function LoadEventMaster() {
    $("#Event").empty();

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Creg/GetCamuEventMaster',
        data: {},
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    if (obj.status == 'There are no forms') {
                        $("#Event").append('<option value="">[Select]</option>');

                    }
                    else {
                        $("#Event").append('<option value="' + obj.Event_Id + '">' + obj.Event_Name + '</option>');
                    }

                });
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Event").append('<option value="">[Select]</option>');
                $.each(data, function (i, obj) {
                    $("#Event").append('<option value="' + obj.Event_Id + '">' + obj.Event_Name + '</option>');
                });

                $('#loader').hide();
            }
            else {
                error("Event forms not configured", "topCenter");
                $('#loader').hide();
            }
        },
        error: function (ex) {
            error("Failed to Load Events" + ex.Message, "topCenter");
        }
    });
}
//Report Page




