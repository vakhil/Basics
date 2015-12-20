<%@ Page Title="" Language="C#" MasterPageFile="~/PlantsMaster.master" AutoEventWireup="true"
    CodeFile="PlantsForm.aspx.cs" Inherits="PlantsForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/JTemplate.js?v=1001" type="text/javascript"></script>
    <link href="CSS/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="DropDownCheckList.js?v=1001" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        #maindivcontainer
        {
            /*position: relative;*/
            width: 100%;
            height: 100%;
        }
        #div1, #div2, #div3, #div4, #div5, #div6
        {
            position: absolute;
            width: 16.5%;
            height: 100%;
            z-index: 99999;
        }
        #div1
        {
            left: 0;
        }
        #div2
        {
            left: 16.5%;
        }
        #div3
        {
            left: 33%;
        }
        #div4
        {
            left: 49.5%;
        }
        #div5
        {
            left: 66%;
        }
         #div6
        {
            left: 82.5%;
        }
        .innerdiv
        {
            border: 5px solid #d5d5d5;
            height: 18%;
            text-align: center;
            background-color: #ffffff;
            box-shadow: inset 0 0 1em olive;
        }
         .clickdiv
        {
            border: 5px solid #d5d5d5;
            height: 18%;
            text-align: center;
            cursor: pointer;
            background-color: #ffffff;
            box-shadow: inset 0 0 1em rgb(185, 185, 174);
            cursor:pointer;
        }
    </style>
    <script type="text/javascript">
        var plants = [];
        $(function () {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            var Username = '<%= Session["field1"] %>';
            if (Username == '') {
                window.location.assign("Login.aspx");
                return;
            }
            var data = { 'op': 'ShowMyplants', 'Username': Username };
            var s = function (data) {
                if (data) {
                    assighplants(data);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        });
        function callHandler(d, s, e) {
            $.ajax({
                url: 'Bus.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function assighplants(data) {
            var ServerDt = data.ServerDt;
            var allworkingveh = 0;
            var allnonworkingveh = 0;
            for (var vehicledata in data.vehiclesupdatelist) {
                var found = false;
                for (i = 0; i < plants.length && !found; i++) {
                    if (plants[i].BranchName === data.vehiclesupdatelist[vehicledata].PlantName) {
                        found = true;
                    }
                }
                if (!found) {
                    var workingveh = 0;
                    var nonworkingveh = 0;
                    for (var vehstts in data.vehiclesupdatelist) {
                        if (data.vehiclesupdatelist[vehicledata].PlantName == data.vehiclesupdatelist[vehstts].PlantName) {
                            var vehno = data.vehiclesupdatelist[vehstts].vehiclenum;
                            var updatedata = data.vehiclesupdatelist[vehstts].Datetime;

                            var date = updatedata.split(" ")[0];
                            var time = updatedata.split(" ")[1];
                            var datevalues = new Array();
                            var timevalues = new Array();
                            datevalues = date.split('/');
                            timevalues = time.split(':');
                            var updatetime = new Date(datevalues[2], datevalues[1] - 1, datevalues[0], timevalues[0], timevalues[1], timevalues[2]);
                            var stodaydate = ServerDt;
                            var sdate = stodaydate.split(" ")[0];
                            var stime = stodaydate.split(" ")[1];
                            var sdatevalues = new Array();
                            var stimevalues = new Array();
                            sdatevalues = sdate.split('/');
                            stimevalues = stime.split(':');
                            var todaydate = new Date(sdatevalues[2], sdatevalues[1] - 1, sdatevalues[0], stimevalues[0], stimevalues[1], stimevalues[2]);
                            var _MS_PER_DAY = 86400000;
                            var _MS_PER_aaa = 3600000;
                            var _MS_PER_sss = 60000;
                            var _MS_PER_ddd = 1000;
                            var days = Math.floor((todaydate - updatetime) / _MS_PER_DAY);
                            var hours = Math.floor((todaydate - updatetime) / _MS_PER_aaa);
                            if (hours > 24) {
                                hours = hours % 24;
                            }
                            var min = Math.floor((todaydate - updatetime) / _MS_PER_sss);
                            if (min > 60) {
                                min = min % 60;
                            }
                            var sec = Math.floor((todaydate - updatetime) / _MS_PER_ddd);
                            if (sec > 60) {
                                sec = sec % 60;
                            }
                            if (hours > 1) {
                                nonworkingveh++;
                                allnonworkingveh++;
                            }
                            else {

                                workingveh++;
                                allworkingveh++;
                            }
                        }
                    }
                    plants.push({ BranchName: data.vehiclesupdatelist[vehicledata].PlantName, workingvehicles: workingveh, nonworkingvehicles: nonworkingveh });
                }
            }

            var Usertype = '<%= Session["UserType"] %>';
            if (Usertype === 'Admin') {
                document.getElementById('lbl_plant0').innerHTML = "ALL PLANTS";
                document.getElementById('lbl_workingveh0').innerHTML = allworkingveh;
                document.getElementById('lbl_nonworkingveh0').innerHTML = allnonworkingveh;
                for (var i = 1; i <= plants.length; i++) {
                    if (typeof document.getElementById('lbl_plant' + i + '') === "undefined") {
                    }
                    else {
                        document.getElementById('lbl_plant' + i + '').innerHTML = plants[i - 1].BranchName;
                        document.getElementById('lbl_workingveh' + i + '').innerHTML = plants[i - 1].workingvehicles;
                        document.getElementById('lbl_nonworkingveh' + i + '').innerHTML = plants[i - 1].nonworkingvehicles;
                    }
                }
            }
            else {
                for (var i = 0; i < plants.length; i++) {
                    if (typeof document.getElementById('lbl_plant' + i + '') === "undefined") {
                    }
                    else {
                        document.getElementById('lbl_plant' + i + '').innerHTML = plants[i].BranchName;
                        document.getElementById('lbl_workingveh' + i + '').innerHTML = plants[i].workingvehicles;
                        document.getElementById('lbl_nonworkingveh' + i + '').innerHTML = plants[i].nonworkingvehicles;
                    }
                }
            }
        }
        $(function () {
            $('.clickdiv').click(function () {
                var plantname = $(this);
                var plant = plantname[0].innerText;
                if (plantname[0].innerText.length > 1) {

                    var data = { 'op': 'loginauthorisation', 'plant': plant };
                    var s = function (msg) {
                        window.location.assign("Default.aspx");
                    };
                    var e = function (x, h, e) {
                    };
                    callHandler(data, s, e);
                    //                    //                    $("#divloginform").css("display", "block");
                    //                    //                    $("#txt_username").val("");
                    //                    //                    $("#txt_password").val("");
                    //                    //                    $("#lbl_hdnplntname").val(plantname[0].innerText);
                }
            });
        });
        function btnloginclick() {
            var username = $("#txt_username");
            var password = $("#txt_password");
            var plantname = $("#lbl_hdnplntname");
            var data = { 'op': 'loginauthorisation', 'username': username[0].value, 'password': password[0].value, 'plantname': plantname[0].value };
            var s = function (msg) {
                if (msg) {
                    if (msg == "success") {
                        $("#divloginform").css("display", "none");
                        //                    $('#maindiv').removeTemplate('LoginPlants.htm');
                        //                    $('#maindiv').css('display', 'none');
                        //                    InitilizeVehicles();
                        //                    filvehdiv(Totalstring);
                        //                    var myVar = setInterval(function () { liveupdate() }, 20000);
                        window.location.assign("Default.aspx");
                    }
                    else if (msg == "Plantnotmatch") {
                        alert("Authorisation Failed\nThis user not associated with this Plant");
                    }
                    else {
                        alert("Authorisation Failed\nPlease enter authorised username and password");
                    }
                }
                else {
                    alert("Authorisation Failed");
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btnlogincancelclick() {
            $("#divloginform").css("display", "none");
        }
        $(function () {
            $('#div_loginclose').click(function () {
                $('#divloginform').css('display', 'none');
            });
        });
    </script>
    <div>
        <div id="maindivcontainer">
            <div id='div1'>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div123" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant0" style="font-size: 15px; font-weight: bold; color: Red; text-align: center;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="labelwv" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh0" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label id="Label3" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td>
                                            <label id="lbl_nonworkingveh0" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div7" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant1" style="font-size: 15px; font-weight: bold; color: Red; text-align: center;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label2" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh1" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label5" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh1" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div8" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant2" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label7" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh2" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label9" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh2" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div9" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant3" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label11" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh3" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label13" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh3" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div10" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant4" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="lalel6" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh4" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label17" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh4" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id='div2'>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div11" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant5" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label19" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh5" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label21" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh5" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div12" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant6" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label24" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh6" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label25" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh6" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div14" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant7" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label27" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh7" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label29" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh7" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div13" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant8" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label31" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh8" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label33" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh8" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div15" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant9" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label35" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh9" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label37" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh9" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id='div3'>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div16" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant10" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label39" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh10" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label41" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh10" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div17" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant11" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label4" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh11" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label12" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh11" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div18" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant12" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label43" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh12" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label45" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh12" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div19" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant13" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label47" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh13" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label49" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh13" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div20" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant14" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label51" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh14" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label53" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh14" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id='div4'>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div21" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant15" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label55" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh15" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label57" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh15" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div22" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant16" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label59" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh16" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label61" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh16" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div23" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant17" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label63" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh17" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label65" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh17" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div24" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant18" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label67" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh18" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label69" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh18" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div25" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant19" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label71" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh19" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label73" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh19" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id='div5'>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div26" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant20" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label75" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh20" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label77" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh20" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div27" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant21" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label79" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh21" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label81" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh21" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div28" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant22" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label83" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh22" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label85" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh22" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div29" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant23" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label87" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh23" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label89" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh23" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div30" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant24" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label91" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh24" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label93" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh24" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id='div6'>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div32" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant25" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label8" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh25" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label14" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh25" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div33" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant26" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label18" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh26" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label22" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh26" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div34" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant27" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label28" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh27" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label32" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh27" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div35" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant28" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label38" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh28" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label42" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh28" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="innerdiv">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <div id="div36" style="cursor: pointer; text-align: center;">
                                    <img src="Images/plantimg.png" alt="close" height="30px" width="30px">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="clickdiv">
                                <label id="lbl_plant29" style="font-size: 15px; font-weight: bold; color: Red; text-align: left;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="label48" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                WorkingVehicles :</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_workingveh29" style="font-size: 18px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%;">
                                            <label id="Label52" style="font-size: 12px; font-weight: bold; color: #999191; text-align: center;">
                                                NonWorkingVehicles:</label>
                                        </td>
                                        <td style="width: 50%;">
                                            <label id="lbl_nonworkingveh29" style="font-size: 12px; font-weight: bold; color: Green;
                                                text-align: center;">
                                            </label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div id="divloginform" style="display: none; text-align: center; height: 100%; width: 100%;
        position: absolute; left: 0%; top: 0%; z-index: 99999; background-color: rgba(192, 192, 192, 0.701961);
        background-position: initial initial; background-repeat: initial initial;">
        <div style="margin: 5% auto; top: 20%; height: 200px; width: 400px; background-color: #ffffcc;
            text-align: left; border: 2px solid #d5d5d5; border-radius: 5px 5px 5px 5px;">
            <table>
                <tr>
                    <td>
                        <br />
                    </td>
                    <td>
                    </td>
                    <td>
                        <table style="width: 300px;">
                            <tr>
                                <td style="text-align: center;">
                                    <label id="Label1" style="font-size: 15px; font-weight: bold; color: Red; text-align: center;">
                                        LOGIN</label>
                                </td>
                                <td>
                                    <div id="div_loginclose" style="cursor: pointer; text-align: right;">
                                        <img src="Images/Close.png" alt="close">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <span>UserName</span>
                    </td>
                    <td>
                        <input type="text" id="txt_username" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <br />
                        <span>Password</span>
                    </td>
                    <td>
                        <input type="password" id="txt_password" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <input type="button" id="btn_login" value="Login" onclick="btnloginclick();" style="height: 30px;
                            width: 100px; font-size: 14px;" />
                        <input type="button" id="btn_logincancel" value="Cancel" onclick="btnlogincancelclick();"
                            style="height: 30px; width: 100px; font-size: 14px;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <label id="lbl_hdnplntname" style="visibility: hidden;">
    </label>
</asp:Content>
