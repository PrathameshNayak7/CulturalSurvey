function LoadDepartment() {
    $("#Department").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        dataType: 'json',
        url: '/Master/Load_State',
        data: {},
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Department").append('<option value="' + obj.State_Id + '">' + obj.State_Name + '</option>');
                });
                LoadDistrict();
                $('#loader').hide();
            }
            else if (data.length > 1) {
                $("#Department").append('<option value="">Select State</option>');
                $.each(data, function (i, obj) {
                    $("#Department").append('<option value="' + obj.State_Id + '">' + obj.State_Name + '</option>');
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
$("#Department").change(function () {
});
function LoadWorkLocation() {
    $("#WorkLocation").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Master/Load_District',
        data: { State_Id: $("#State_Id").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#WorkLocation").append('<option value="' + obj.District_Id + '">' + obj.District_Name + '</option>');
                });
                LoadTaluka();
                $('#loader').hide();
            }
            else if (data.length >= 1) {
                $("#WorkLocation").append('<option value="">Select District</option>');
                $.each(data, function (i, obj) {
                    $("#WorkLocation").append('<option value="' + obj.District_Id + '">' + obj.District_Name + '</option>');
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
$("#WorkLocation").change(function () {

});
function LoadLevel() {

  

    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Master/Load_Taluka',
        data: { District_Id: $("#District_Id").val() },
        success: function (data) {
            if (data.length == 1) {
                $.each(data, function (i, obj) {
                    $("#Level").append('<option value="' + obj.Taluka_Id + '">' + obj.Taluka_Name + '</option>');
                });
                LoadVillage();
                $('#loader').hide();
            }
            else if (data.length >= 1) {
                $("#Level").append('<option value="">Select Taluka</option>');
                $.each(data, function (i, obj) {
                    $("#Level").append('<option value="' + obj.Taluka_Id + '">' + obj.Taluka_Name + '</option>');
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
$("#Level").change(function () {
    

});
function LoadTenure() {
    $("#Tenure").empty();
    $.ajax({
        cache: false,
        async: false,
        type: 'POST',
        url: '/Master/Load_Village',
        data: { Taluka_Id: $("#Taluka_Id").val() },
        success: function (data) {
            if (data.length >= 1) {
                $("#Tenure").append('<option value="">Select Village</option>');
                $.each(data, function (i, obj) {
                    $("#Tenure").append('<option value="' + obj.Village_Id + '">' + obj.Village_Name + '</option>');
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