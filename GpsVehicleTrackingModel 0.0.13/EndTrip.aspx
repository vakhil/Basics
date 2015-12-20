<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EndTrip.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="EndTrip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/date.format.js" type="text/javascript"></script>
      <style type="text/css">
        .txtClass
        {
            width: 186px;
            height: 28px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #555;
            vertical-align: middle;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,0.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,0.075);
            -webkit-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        }
        .divstyle
        {
            width: 100%;
            background: linear-gradient(to bottom,#f8f8f8,#eee);
            height: 30px;
            text-align: center;
            padding-top: 10px;
            font-size: 15px;
            font-weight: bold;
            border-bottom: 1px #ddd solid;
            
        }
        .SaveButton
        {
            background: #2a81d7;
            background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
            background: -webkit-gradient(linear,0% 0,0% 100%,from(#3e9ee5),to(#206bcb));
            border-top: 1px solid #2a73a6;
            border-right: 1px solid #165899;
            border-bottom: 1px solid #07428f;
            border-left: 1px solid #165899;
            -moz-box-shadow: inset 0 1px 0 0 #62b1e9;
            -webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
            box-shadow: inset 0 1px 0 0 #62b1e9;
            color: #FFF;
            cursor: pointer;
            text-decoration: none;
            text-shadow: 0 -1px 1px #1d62ab;
            border-radius: 4px 4px 4px 4px;
        }
        .SaveButton:hover
        {
            background: #0066CC;
            background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
            background: -webkit-gradient(linear,0% 0,0% 100%,from(#0066CC),to(#0066CC));
            border-top: 1px solid #2a73a6;
            border-right: 1px solid #165899;
            border-bottom: 1px solid #07428f;
            border-left: 1px solid #165899;
            -moz-box-shadow: inset 0 1px 0 0 #62b1e9;
            -webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
            box-shadow: inset 0 1px 0 0 #0066CC;
            color: #FFF;
            cursor: pointer;
            text-decoration: none;
            text-shadow: 0 -1px 1px #1d62ab;
            border-radius: 4px 4px 4px 4px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            FillTripsheets();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txt_datetime').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
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
        function CallHandlerUsingJson(d, s, e) {
            d = JSON.stringify(d);
            d = d.replace(/&/g, '\uFF06');
            d = d.replace(/#/g, '\uFF03');
            d = d.replace(/\+/g, '\uFF0B');
            d = d.replace(/\=/g, '\uFF1D');
            $.ajax({
                type: "GET",
                url: "Bus.axd?json=",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: d,
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
        function FillTripsheets() {
            var data = { 'op': 'getassignedtrips' };
            var s = function (msg) {
                if (msg) {
                    filltrips(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        var assignedtrips;
        function filltrips(msg) {
            assignedtrips = msg;
            var triplist = document.getElementById('cmb_Tripsheets');
            document.getElementById('cmb_Tripsheets').options.length = null;
            var opt = document.createElement('option');
            opt.innerHTML = "Select Trip";
            opt.value = "Select Trip";
            triplist.appendChild(opt);
            for (i = 0; i < msg.length; i++) {
                var option = document.createElement('option');
                option.innerHTML = msg[i].tripno;
                option.value = msg[i].tripsno;
                triplist.appendChild(option);
            }
        }
        function tripsheet_change() {
            document.getElementById('div_jobcards').innerHTML = null;
            document.getElementById('lbl_startdate').innerHTML = "";
            document.getElementById('lbl_vehno').innerHTML = "";
            document.getElementById('lbl_route').innerHTML = "";
            document.getElementById('lbl_Driver').innerHTML = "";
            document.getElementById('lbl_startodometer').innerHTML = "0";
            document.getElementById('lbl_startfuel').innerHTML = "0";
            document.getElementById('lbl_tripkms').innerHTML = "0";
            document.getElementById('lbl_gpskms').innerHTML = "0";
            document.getElementById('lbl_fuelconsumption').innerHTML = "0";
            document.getElementById('lbl_tripmileage').innerHTML = "0";
            document.getElementById('lbl_diffbtengpsandmanualkms').innerHTML = "0";
            document.getElementById('txt_endodometerrdng').value = "0";
            document.getElementById('txt_endfuelrdng').value = "0";
            document.getElementById('txt_endhourmtrrdng').value = "0";
            var TripDate = document.getElementById('txt_datetime').value;
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            if (tripsno == "Select Trip") {
                return;
            }
            var data = { 'op': 'gettripalldetails', 'tripsno': tripsno, 'TripDate': TripDate };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('lbl_startdate').innerHTML = msg.Tripdate;
                    document.getElementById('lbl_vehno').innerHTML = msg.Vehicleno;
                    document.getElementById('lbl_route').innerHTML = msg.RouteName;
                    document.getElementById('lbl_Driver').innerHTML = msg.Drivername;
                    document.getElementById('lbl_startodometer').innerHTML = msg.StrartReading;
                    document.getElementById('lbl_startfuel').innerHTML = msg.StrartFuel;
                    var gpskms = msg.gpskms;
                    gpskms = parseFloat(gpskms).toFixed(2);
                    document.getElementById('lbl_gpskms').innerHTML = gpskms;

                    var data = "<table style='margin: 10px;color: #777777 !important;font-weight: bold;font-family: sans-serif;'>";
                    for (var i = 0; i < msg.jobcards.length; i++) {
                        var status = "";
                        var jobcardsts = msg.jobcards[i].status;
                        if (jobcardsts == "A") {
                            status = "Assigned";
                        }
                        else {
                            status = "Completed";
                        }
                        data += "<tr><td style='color: #080A89'><span>" + msg.jobcards[i].jobcardname + "</span></td><td style='width:20px;'></td><td><span>" + status + "</span></td></tr>";
                    }
                    data += "</table>";
                    $('#div_jobcards').append(data);
                }
                else {
                }
            }
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function totalCalculation() {
            var srartodordng = document.getElementById('lbl_startodometer').innerHTML;
            var srartfuelrdng = document.getElementById('lbl_startfuel').innerHTML;
            var endodordng = document.getElementById('txt_endodometerrdng').value;
            var endfuelrdng = document.getElementById('txt_endfuelrdng').value;
            var tripkms = 0;
            var fuelconsumption = 0;
            if (srartodordng != "" && endodordng != "") {
                srartodordng = parseFloat(srartodordng)
                endodordng = parseFloat(endodordng)
                tripkms = (endodordng - srartodordng).toFixed(2);
            }
            if (srartfuelrdng != "" && endfuelrdng != "") {
                srartfuelrdng = parseFloat(srartfuelrdng)
                endfuelrdng = parseFloat(endfuelrdng)
                fuelconsumption = (endfuelrdng - srartfuelrdng).toFixed(2);
            }
            document.getElementById('lbl_tripkms').innerHTML = tripkms;
            document.getElementById('lbl_fuelconsumption').innerHTML = fuelconsumption;
            tripkms = parseFloat(tripkms);
            fuelconsumption = parseFloat(fuelconsumption);
            var mileage = 0;
            if (tripkms != 0 && fuelconsumption != 0) {
                mileage = (tripkms / fuelconsumption);
            }
            document.getElementById('lbl_tripmileage').innerHTML = parseFloat(mileage).toFixed(2);
            var gpskms = document.getElementById('lbl_gpskms').innerHTML;
            gpskms = parseFloat(gpskms);
            var diffbtwngpsandmanual = tripkms - gpskms;
            diffbtwngpsandmanual = parseFloat(diffbtwngpsandmanual).toFixed(2);
            document.getElementById('lbl_diffbtengpsandmanualkms').innerHTML = diffbtwngpsandmanual;
        }

        function btnRefreshClick() {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var hrs = today.getHours();
            var mnts = today.getMinutes();
            $('#txt_datetime').val(yyyy + '-' + mm + '-' + dd + 'T' + hrs + ':' + mnts);
            document.getElementById('div_jobcards').innerHTML = null;
            document.getElementById('lbl_startdate').innerHTML = "";
            document.getElementById('lbl_vehno').innerHTML = "";
            document.getElementById('lbl_route').innerHTML = "";
            document.getElementById('lbl_Driver').innerHTML = "";
            document.getElementById('lbl_startodometer').innerHTML = "0";
            document.getElementById('lbl_startfuel').innerHTML = "0"; 
            document.getElementById('lbl_tripkms').innerHTML = "0";
            document.getElementById('lbl_gpskms').innerHTML = "0";
            document.getElementById('lbl_fuelconsumption').innerHTML = "0"; 
            document.getElementById('lbl_tripmileage').innerHTML = "0";
            document.getElementById('lbl_diffbtengpsandmanualkms').innerHTML = "0";
            document.getElementById('txt_endodometerrdng').value="0";
            document.getElementById('txt_endfuelrdng').value = "0";
            document.getElementById('txt_endhourmtrrdng').value = "0";
            document.getElementById('cmb_Tripsheets').selectedIndex = 0;
        }
        function BtnSaveClick() {
            var endodordng = document.getElementById('txt_endodometerrdng').value;
            var endfuelrdng = document.getElementById('txt_endfuelrdng').value;
            var endhourmtrrdng = document.getElementById('txt_endhourmtrrdng').value;
            var gpskms = document.getElementById('lbl_gpskms').innerHTML;
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            var TripDate = document.getElementById('txt_datetime').value;
            if (endodordng == "") {
                alert("Please Enter Odometer Reading");
                return;
            }
            if (endfuelrdng == "") {
                alert("Please Enter Fuel Value");
                return;
            }
            if (endhourmtrrdng == "") {
                alert("Please enter Hour Meter Reading");
                return;
            }
            if (tripsno == "Select Trip") {
                alert("Please select Trip");
                return;
            }

            var data = { 'op': 'btnTripendSaveClick', 'TripDate': TripDate, 'tripsno': tripsno, 'endodordng': endodordng, 'endfuelrdng': endfuelrdng, 'endhourmtrrdng': endhourmtrrdng, 'gpskms': gpskms };
            var s = function (msg) {
                if (msg) {
                    if (msg == "Trip ended successfully") {
                        btnRefreshClick();
                        FillTripsheets();
                    }
                    alert(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        //Function for only no
        $(document).ready(function () {
            $("#txt_endodometerrdng,#txt_endhourmtrrdng,#txt_endfuelrdng").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, and enter
                if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 || event.keyCode == 190 ||
                // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table align="center">
        <tr>
            <td colspan="2">
                <span style="text-align: center; font-size: 20px; color: orange;">End Tripsheet</span>
            </td>
        </tr>
    </table>
    <table align="center">
        <tr>
            <td>
                <label>
                    Assigned Tripsheets</label>
            </td>
            <td>
                <select id="cmb_Tripsheets" class="txtClass" onchange="tripsheet_change();" style="width: 210px;">
                </select>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td>
            <div align="center">
             <table style='margin: 10px;color: #777777 !important;font-weight: bold;font-family: sans-serif;'>
                         <tr>
                                <td style="color: #080A89">
                                Trip End Date
                                </td>
                                <td style="color: #080A89;font-size: 20px;">
                                <input id="txt_datetime" type="datetime-local" class="txtsize"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #080A89">
                                End Odometer Reading
                                </td>
                                <td>
                                <input id="txt_endodometerrdng" type="text" class="txtClass" onkeyup="totalCalculation()"/>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #080A89">
                                End Hour Meter Reading
                                </td>
                                <td>
                                <input id="txt_endhourmtrrdng" type="text" class="txtClass" onkeyup="totalCalculation()"/>
                                </td>
                            </tr>
                             <tr>
                                <td style="color: #080A89">
                                Fuel Value
                                </td>
                                <td>
                                <input id="txt_endfuelrdng" type="text" class="txtClass" onkeyup="totalCalculation()"/>
                                </td>
                            </tr>
                            <tr><td style="height:20px;"></td></tr>
                             <tr>
                                <td style="color: #080A89">
                                Fuel Consumption
                                </td>
                                <td style="color: Red;font-size: 25px;">
                                <span id="lbl_fuelconsumption">0</span>
                                </td>
                            </tr>
                            <tr><td style="height:20px;"></td></tr>
                             <tr>
                                <td style="color: #080A89">
                                Trip KMS
                                </td>
                                <td style="color: Red;font-size: 25px;">
                                <span id="lbl_tripkms">0</span>
                                </td>
                            </tr>
                            <tr><td style="height:20px;"></td></tr>
                                <tr>
                                <td style="color: #080A89">
                                GPS KMS
                                </td>
                                <td style="color: Red;font-size: 25px;">
                                <span id="lbl_gpskms">0</span>
                                </td>
                            </tr>
                            <tr><td style="height:20px;"></td></tr>
                             <tr>
                                <td style="color: #080A89">
                                Trip Mileage
                                </td>
                                <td style="color: Red;font-size: 25px;">
                                <span id="lbl_tripmileage">0</span>
                                </td>
                            </tr>
                               <tr><td style="height:20px;"></td></tr>
                             <tr>
                                <td style="color: #080A89">
                                Difference between GPS and Manual KMS
                                </td>
                                <td style="color: Red;font-size: 25px;">
                                <span id="lbl_diffbtengpsandmanualkms">0</span>
                                </td>
                            </tr>
                            
                        </table>
              </div>
            </td>
            <td width="400px">
                <div style="border: solid 1px #d5d5d5;">
                    <div class="divstyle">
                        <span style="text-align: center; font-size: 20px; color: orange;">Trip Details</span>
                    </div>
                    <div id="div_tripdetails">
                        <table style='margin: 10px;color: #777777 !important;font-weight: bold;font-family: sans-serif;'>
                         <tr>
                                <td style="color: #080A89;width: 150px;">
                                Start Date
                                </td>
                                <td>
                                <span id="lbl_startdate"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #080A89;width: 150px;">
                                Vehicle Number
                                </td>
                                <td>
                                <span id="lbl_vehno"></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: #080A89;width: 150px;">
                                Route
                                </td>
                                <td>
                                <span id="lbl_route"></span>
                                </td>
                            </tr>
                             <tr>
                                <td style="color: #080A89;width: 150px;">
                                Driver Name
                                </td>
                                <td>
                                <span id="lbl_Driver"></span>
                                </td>
                            </tr>
                             <tr>
                                <td style="color: #080A89;width: 150px;">
                                Odometer Reading
                                </td>
                                <td>
                                <span id="lbl_startodometer">0</span>
                                </td>
                            </tr>
                               <tr>
                                <td style="color: #080A89;width: 150px;">
                                Fuel at start (Ltrs)
                                </td>
                                <td>
                                <span id="lbl_startfuel">0</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="divstyle">
                        <span style="text-align: center; font-size: 20px; color: orange;">Job Cards</span>
                    </div>
                    <div id="div_jobcards">
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <table style="width:100%;">
    <tr><td style="text-align:center;">
      <input type="button" id="BtnSave" value="Save" onclick="BtnSaveClick();" class="SaveButton"
                    style="width: 137px; height: 30px; font-size: 20px;" />&nbsp&nbsp
                     <input type="button" id="btnreset" value="Reset" onclick="btnRefreshClick();" class="SaveButton"
                    style="width: 137px; height: 30px; font-size: 20px;" />
    </td></tr>
    </table>
</asp:Content>
