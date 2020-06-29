
formatJsonDateString = function (value) {
    var a;
    if (typeof value == 'string') {
        a = /\/Date\((\d*)\)\//.exec(value);
        if (a) {
            var dateVal = new Date(+a[1]);
            var month = dateVal.getMonth() + 1;
            var day = dateVal.getDate();
            var year = dateVal.getFullYear();
            return month + "/" + day + "/" + year;
        }
    }
    return value;
}

getProjectManagerByProjectID = function () {
    $("#projectManagerLabel").empty();
    if ($("#ProjectID").val().length > 0) {
        $.ajax({
            url: rootUrl + "Project/GetProjectManagerName",
            type: "POST",
            data: { projectID: $("#ProjectID").val() },
            success: function (data) {
                $("#projectManagerLabel").text(data);
            },
            error: function (xhr, status) {
                alert("Error has occured while getting the project manager name");
            }
        });
    }
}

function getEmployeePercentageOfAllocation() {
    if ($("#EmployeeID").val().length > 0) {
        var v_data = {};
        v_data.empID = $("#EmployeeID").val();
        v_data.prjID = $("#ProjectID").val();

        $.ajax({
            url: rootUrl + "Allocation/GetPercentageOfAllocation",
            type: "POST",
            data: JSON.stringify(v_data),
            contentType: "application/json",
            success: function (data) {
                $("#howMuchOccupied").text(data);
            },
            error: function () {
                alert("Error has occured while loading employee details");
            }
        });
    }
}

function getEmployeeOtherProjectAllocations() {
    if ($("#EmployeeID").val().length > 0) {
        $.ajax({
            url: rootUrl + "Allocation/EmployeeAllocations",
            cache: false,
            type: "GET",
            contentType: "application/html; charset=utf-8",
            data: { empID: $("#EmployeeID").val() },
            success: function (data) {
                $("#employeeAllocationDiv").html(data);
            },
            error: function (xhr, status) {
                alert("Error has occured while loading employee allocation details");
            }
        });
    }
}

function loadEmployeeDetailsForAllocationEditPage() {
    if ($("#EmployeeID").val().length > 0) {
        $.ajax({
            url: rootUrl + "Employee/GetEmployeeDetails",
            type: "POST",
            data: { id: $("#EmployeeID").val() },
            success: function (data) {
                $("#employeeID").text(data["EmployeeID"]);
                $("#employeeType").text(data["EmploymentTypeName"]);
                $("#primarySkills").text(data["PrimarySkills"]);
            },
            error: function () {
                alert("Error has occured while loading project details");
            }
        });
    }
}

function updateProjectDetailsSectionOnAllocationPage() {
    if ($("#ProjectID").val().length > 0) {
        $.ajax({
            url: rootUrl + "Allocation/GetProjectDetails",
            type: "POST",
            data: { projectID: $("#ProjectID").val() },
            success: function (data) {
                var sDate = formatJsonDateString(data["StartDate"]);
                var eDate = formatJsonDateString(data["EndDate"]);

                // updating project details section
                $("#pmName").text(data["ProjectManagerName"]);
                $("#projectStartDate").text(sDate);
                $("#projectEndDate").text(eDate);
                $("#projectType").text(data["ProjectTypeName"]);
            },
            error: function () {
                alert("Error has occured while loading project details");
            }
        });
    }
}

function getPracticeManagerNameForSubPracticePage() {
    if ($("#PracticeID").val().length > 0) {
        $.ajax({
            url: rootUrl + "SubPractice/GetPracticeName",
            type: "POST",
            data: { id: $("#PracticeID").val() },
            success: function (data) {
                if (data.length == 0) {
                    $("#practiceManagerName").text("  Practice Manager: None");
                }
                else {
                    $("#practiceManagerName").text("  Practice Manager: " + data);
                }
            },
            error: function () {
                alert("Unable to retrieve the Pratice Manager Name");
            }
        });
    }
}

function generateEmployeeIDForEmpPage() {
    if ($("#EmploymentTypeID").val().length > 0) {
        $.ajax({
            url: rootUrl + "Employee/GenerateNewEmployeeID",
            type: "POST",
            data: { id: $("#EmploymentTypeID").val() },
            success: function (data) {
                $("#EmployeeID").val(data);
                $("#empID").val(data);
            },
            error: function (eid) { alert("Error while generating new Employee ID"); }
        });
    }
}

function getEmailIDOfEmployee() {
    if ($("#EmployeeID").val().length > 0) {
        $.ajax({
            url: rootUrl + "Employee/GetEmailID",
            type: "POST",
            data: { id: $("#EmployeeID").val() },
            success: function (data) {
                $("#Email").val(data);
                if ($("#Email").val().length != 0) {
                    //$("#Email").attr('disabled', 'disabled');
                }
                else {
                    //$("#Email").removeAttr('disabled', 'disabled');
                }
            },
            error: function (eid) { alert("Error while retrieving Email ID"); }
        });
    }
}

function loadSubPracticeDropDownForEmpPage() {
    if ($("#PracticeID").val().length > 0) {
        $.ajax({
            url: rootUrl + "Employee/GetSubPracticeList",
            type: 'POST',
            data: { id: $("#PracticeID").val() },
            success: function (data) {
                $('#SubPracticeID').empty();
                $("#SubPracticeID").append($("<option></option>").val(0).text("Please Select"));
                $.each(data, function () {
                    $("#SubPracticeID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                });
            },
            error: function (xhr) { alert('Error while loading the Sub Practice list'); }
        });
    }
}

function getPracticeManagerNameForEmployeePage() {
    $("#txtPracticeManager").text("");
    $("#txtSubPracticeManager").text("");

    if ($("#PracticeID").val().length > 0) {
        $.ajax({
            url: rootUrl + "SubPractice/GetPracticeName",
            type: "POST",
            data: { id: $("#PracticeID").val() },
            success: function (data) {
                if (data.length == 0) {
                    $("#txtPracticeManager").text("Not assigned yet");
                }
                else {
                    $("#txtPracticeManager").text(data);
                }
            },
            error: function () {
                alert("Unable to retrieve the Pratice Manager Name");
            }
        });
    }
}

function getSubPracticeManagerNameForEmployeePage() {
    if ($("#SubPracticeID").val().length > 0) {

        $("#txtSubPracticeManager").text("");

        $.ajax({
            url: rootUrl + "SubPractice/GetSubPracticeManagerName",
            type: "POST",
            data: { id: $("#SubPracticeID").val() },
            success: function (data) {
                if (data.length == 0) {
                    $("#txtSubPracticeManager").text("Not assigned yet");
                }
                else {
                    $("#txtSubPracticeManager").text(data);
                }
            },
            error: function () {
                alert("Unable to retrieve the Pratice Manager Name");
            }
        });
    }
}

function loadBuLevelDropDownListForEmpPage() {
    if ($("#BusinessUnitID").val().length == 0) return;
    $("#Level1ID").empty();
    $("#Level1ID").append($("<option></option>").val(0).text("Please Select"));

    $("#Level2ID").empty();
    $("#Level2ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level3ID").empty();
    $("#Level3ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level4ID").empty();
    $("#Level4ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level5ID").empty();
    $("#Level5ID").append($("<option></option>").val(0).text("Please Select"));

    $.ajax({
        url: rootUrl + "BuLevel/GetBuLevelsByBuID",
        type: "POST",
        data: { buID: $("#BusinessUnitID").val() },
        success: function (data) {

            $.each(data, function () {
                $("#Level1ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));

                if ($("#BusinessUnitID").val() == 2) {
                    $("#Level2ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                    $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                    $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                    $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                }
            });
        },
        error: function () {
            alert("Error has occured while loading the BU Levels for the selected BU");
        }
    });
}

function loadBuLevelDropDownListForEmpPageFromLevel2() {
    if ($("#BusinessUnitID").val().length == 0) return;
    $("#Level2ID").empty();
    $("#Level2ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level3ID").empty();
    $("#Level3ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level4ID").empty();
    $("#Level4ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level5ID").empty();
    $("#Level5ID").append($("<option></option>").val(0).text("Please Select"));

    $.ajax({
        url: rootUrl + "BuLevel/GetBuLevelsByBuID",
        type: "POST",
        data: { buID: $("#BusinessUnitID").val() },
        success: function (data) {

            $.each(data, function () {
                $("#Level2ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the BU Levels for the selected BU");
        }
    });
}

function loadResourceLevelDropDownListForEmpPage() {
    if ($("#Level1ID").val().length == 0) return;


    $.ajax({
        url: rootUrl + "RLevel/GetResourceLevelsByBuID",
        type: "POST",
        data: { levelID: $("#Level1ID").val() },
        success: function (data) {

            $.each(data, function () {
                $("#Level2ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));

                if ($("#BusinessUnitID").val() == 1) {
                    $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                    $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                    $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                }
            });
        },
        error: function () {
            alert("Error has occured while loading the Resource Levels for the selected BU Level");
        }
    });
}

function loadAccountsDropDownListForEmpPage() {
    $("#Level1ID").empty();
    $("#Level2ID").empty();
    $("#Level2ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level3ID").empty();
    $("#Level3ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level4ID").empty();
    $("#Level4ID").append($("<option></option>").val(0).text("Please Select"));
    $("#Level5ID").empty();
    $("#Level5ID").append($("<option></option>").val(0).text("Please Select"));

    $.ajax({
        url: rootUrl + "Accounts/GetAccountsListItems",
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level1ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function loadAccountsListForEmpPageWithRmgEarked() {
    $("#Level3ID").empty();
    $("#Level4ID").empty();
    $("#Level4ID").append($("<option></option>").val(0).text("Please Select"));

    $.ajax({
        url: rootUrl + "Accounts/GetAccountsListItems",
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function loadProjectsDropDownListForEmpPageWithRmgEarmarked() {
    $("#Level4ID").empty();

    $.ajax({
        url: rootUrl + "Project/GetProjectsListItems",
        data: { accountID: $("#Level3ID").val() },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function GetLevel5ListForEmpPageWithRmGEarmarked() {
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 27 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function GetBillableTypeListForEmpPage() {
    $("#Level3ID").empty();
    $("#Level4ID").empty();
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 23 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function GetBillableTypeListForLevel45InEmpPage() {
    $("#Level4ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 23 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function GetBillableTypeListForLevel5InEmpPage() {
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 27 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}


function GetBOLevel2ListForInEmpPageWithAdmin() {
    $("#Level2ID").empty();
    $("#Level3ID").empty();
    $("#Level4ID").empty();
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 29 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level2ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function GetBOLevel2ListForInEmpPageWithFinance() {
    $("#Level2ID").empty();
    $("#Level3ID").empty();
    $("#Level4ID").empty();
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 28 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level2ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function GetLevel4NonBillableListForEmpPage() {
    $("#Level4ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 24 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function GetLevel5NonBillableListForEmpPage() {
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 25 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the list");
        }
    });
}

function loadProjectsDropDownListForEmpPage() {
    $("#Level2ID").empty();

    $.ajax({
        url: rootUrl + "Project/GetProjectsListItems",
        data: { accountID: $("#Level1ID").val() },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level2ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function loadLevel2ListOnRmgGrowthBenchForEmpPage() {
    $("#Level2ID").empty();
    $("#Level3ID").empty();
    $("#Level4ID").empty();
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 26 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level2ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function loadLevel3ListOnRmgGrowthBenchForEmpPage() {
    $("#Level3ID").empty();
    $("#Level4ID").empty();
    $("#Level5ID").empty();

    $.ajax({
        url: rootUrl + "SubCategory/GetSubCategoriesByCategory",
        data: { categoryID: 26 },
        type: "POST",
        success: function (data) {

            $.each(data, function () {
                $("#Level3ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level4ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
                $("#Level5ID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Accounts list");
        }
    });
}

function loadPracticeDropDownListForEmpPage() {
    if ($("#BusinessUnitID").val().length == 0) return;
    $("#PracticeID").empty();
    $("#PracticeID").append($("<option></option>").val(0).text("Please Select"));
    $("#SubPracticeID").empty();
    $("#SubPracticeID").append($("<option></option>").val(0).text("Please Select"));

    $.ajax({
        url: rootUrl + "Practice/GetPracticesByBuID",
        type: "POST",
        data: { buID: $("#BusinessUnitID").val() },
        success: function (data) {

            $.each(data, function () {
                $("#PracticeID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Practices for the selected BU");
        }
    });
}

function loadPracticeDropDownListForProjectPage() {
    if ($("#BusinessUnitID").val().length == 0) return;
    $("#PracticeID").empty();
    $("#PracticeID").append($("<option></option>").val(0).text("Please Select"));

    $.ajax({
        url: rootUrl + "Practice/GetPracticesByBuID",
        type: "POST",
        data: { buID: $("#BusinessUnitID").val() },
        success: function (data) {

            $.each(data, function () {
                $("#PracticeID").append($("<option></option>").val(parseInt(this['Value'])).text(this['Text']));
            });
        },
        error: function () {
            alert("Error has occured while loading the Practices for the selected Business Unit");
        }
    });
}

//$(document).ajaxStart(function () {
//    $("#loading").show();
//});

//$(document).ajaxComplete(function () {
//    $("#loading").hide();
//});

