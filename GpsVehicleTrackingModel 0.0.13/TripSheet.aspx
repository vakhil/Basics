<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TripSheet.aspx.cs" Inherits="TripSheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="js/date.format.js" type="text/javascript"></script>
 <script src="js/JTemplate.js?v=1001" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
         .requiredcls
        {
            color:Red;
            font-size:20px;
            font-weight:bold;
        }
        .txtClass
        {
            display: block;
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
        .txtddlClass
        {
            display: block;
            width: 210px;
            height: 30px;
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
        .Spancontrol
        {
            display: block;
            width: 206px;
            height: 36px;
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

        $(function () {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
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
            FillVehickleNumbers();
        });
        function FillVehickleNumbers() {
            var data = { 'op': 'Initilizedatafortripsheet' };
            var s = function (msg) {
                if (msg) {
                    fillvehicles(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function fillvehicles(msg) {
            if (msg != "Error") {
                var vehicles = msg["vehicles"];
                var drivers = msg["Employee"];
                var routes = msg["routes"];
                var vehiclelst = document.getElementById('cmb_vehicles');
                document.getElementById('cmb_vehicles').options.length = null;
                var opt = document.createElement('option');
                opt.innerHTML = "Select Vehicle";
                opt.value = "Select Vehicle";
                vehiclelst.appendChild(opt);
                for (i = 0; i < vehicles.length; i++) {
                    if (vehicles[i].vehicletype == "Puff" || vehicles[i].vehicletype == "Tanker") {
                        var option = document.createElement('option');
                        option.innerHTML = vehicles[i].vehicleno;
                        option.value = vehicles[i].vehicleno;
                        vehiclelst.appendChild(option);
                    }
                }

                var driverlst = document.getElementById('cmb_driver');
                document.getElementById('cmb_driver').options.length = null;
                var Deiveroption = document.createElement('option');
                Deiveroption.innerHTML = "Select Driver";
                Deiveroption.value = "Select Driver";
                driverlst.appendChild(Deiveroption);
                var helperlst = document.getElementById('cmb_helper');
                document.getElementById('cmb_helper').options.length = null;
                var opt = document.createElement('option');
                opt.innerHTML = "Select Helper";
                opt.value = "Select Helper";
                helperlst.appendChild(opt);
                for (i = 0; i < drivers.length; i++) {
                    if (drivers[i].EmployeeType == "Driver") {
                        var option = document.createElement('option');
                        option.innerHTML = drivers[i].EmpName;
                        option.value = drivers[i].EmpID;
                        driverlst.appendChild(option);
                    }
                    else {
                        var option = document.createElement('option');
                        option.innerHTML = drivers[i].EmpName;
                        option.value = drivers[i].EmpID;
                        helperlst.appendChild(option);
                    }
                }

                var routelst = document.getElementById('cmb_routes');
                document.getElementById('cmb_routes').options.length = null;
                var routeoption = document.createElement('option');
                routeoption.innerHTML = "Select Route";
                routeoption.value = "Select Route";
                routelst.appendChild(routeoption);
                for (i = 0; i < routes.length; i++) {
                    var option = document.createElement('option');
                    option.innerHTML = routes[i].routesname;
                    option.value = routes[i].routesno;
                    routelst.appendChild(option);
                }
            }
            else {
                alert("Error,Please refresh this page");
            }
        }
        function ddlVehicleNoChange(ID) {
            var VehicleNo = ID.value;
            if (VehicleNo == "Select Vehicle") {
                document.getElementById('txtVehicleType').innerHTML = "";
                document.getElementById('txtMake').innerHTML = "";
                document.getElementById('txtModel').innerHTML = "";
                return;
            }
            var data = { 'op': 'GetVehicleDetails', 'VehicleNo': VehicleNo };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('txtVehicleType').innerHTML = msg[0].vehicletype;
                    document.getElementById('txtMake').innerHTML = msg[0].vehiclemake;
                    document.getElementById('txtModel').innerHTML = msg[0].vehiclemodel;
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function btnTripSheetSaveClick() {
            var TripDate = document.getElementById('txt_datetime').value;
            var vehicleNo = document.getElementById('cmb_vehicles').value;
            var RouteID = document.getElementById('cmb_routes').value;
            var driver = document.getElementById('cmb_driver').value;
            var helper = document.getElementById('cmb_helper').value;
            var VehicleStartReading = document.getElementById('txtVehicleStartReading').value;
            var HrReading = document.getElementById('txtHrReading').value;
            var FuelTank = document.getElementById('txtFuelTank').value;
            var load = document.getElementById('txt_load').value;
            var Qty = document.getElementById('txtQty').value;
            var tripstartfrom = document.getElementById('txt_tripstartfrom').value;
            if (vehicleNo == "Select Vehicle") {
                alert("Please select vehicle");
                return;
            }
            if (RouteID == "Select Route") {
                alert("Please select route");
                return;
            }
            if (driver == "Select Driver") {
                alert("Please select driver");
                return;
            }
            if (VehicleStartReading == "") {
                alert("Please enter vehicle start reading");
                return;
            }
            if (FuelTank == "") {
                alert("Please enter fuel value");
                return;
            }
            if (load == "") {
                alert("Please enter load type");
                return;
            }
            if (Qty == "") {
                alert("Please enter quantity");
                return;
            }
            if (helper == "Select Helper") {
                helper = 0;
            }
            var data = { 'op': 'btnTripSheetSaveClick', 'TripDate': TripDate, 'vehicleNo': vehicleNo, 'RouteID': RouteID, 'driver': driver, 'helper': helper, 'VehicleStartReading': VehicleStartReading, 'HrReading': HrReading, 'FuelTank': FuelTank, 'load': load, 'Qty': Qty, 'tripstartfrom': tripstartfrom };
            var s = function (msg) {
                if (msg) {
                    FillVehickleNumbers();
                    btnRefreshClick();
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
        function btnRefreshClick() {
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
            document.getElementById('cmb_vehicles').selectedIndex = 0;
            document.getElementById('cmb_routes').selectedIndex = 0;
            document.getElementById('cmb_driver').selectedIndex = 0;
            document.getElementById('cmb_helper').selectedIndex = 0;
            document.getElementById('txtVehicleStartReading').value = "";
            document.getElementById('txtHrReading').value = "";
            document.getElementById('txtFuelTank').value = "";
            document.getElementById('txt_load').value = "";
            document.getElementById('txtQty').value = "";
            document.getElementById('txt_tripstartfrom').value = "";
            document.getElementById('txtVehicleType').innerHTML = "";

        }
        //Function for only no
        $(document).ready(function () {
            $("#txtVehicleStartReading,#txtHrReading,#txtFuelTank,#txtQty").keydown(function (event) {
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
        $(function () {
            $('#divclose').click(function () {
                $('#divRouteAssign').css('display', 'none');
            });
        });
        function viewroute() {
            $('#divTrip').processTemplate(null);
            $('#divTrip').removeTemplate();
            $('#divRouteAssign').css("display", "block");
            var route = document.getElementById('cmb_routes');
            var RoteName = route[route.selectedIndex].innerHTML;
            if (route.selectedIndex != 0) {
                var data = { 'op': 'ddlRouteNameChange', 'RoteName': RoteName };
                var s = function (msg) {
                    if (msg) {
                        BindtoGrid(msg);
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                callHandler(data, s, e);
            }
        }
        function BindtoGrid(data) {
            $('#divTrip').setTemplateURL('Trip.htm');
            $('#divTrip').processTemplate(data);
        }
        function btnprintClick() {
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table align="center">
        <tr>
            <td colspan="2">
                <span style="text-align: center; font-size: 20px; color: orange;">Trip Sheet</span>
            </td>
        </tr>
    </table>
    <table align="center">
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <label>
                                Date</label>
                        </td>
                        <td>
                            <input id="txt_datetime" type="datetime-local" class="txtsize"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Vehicle No</label>
                        </td>
                        <td>
                            <select id="cmb_vehicles" class="txtddlClass" onchange="ddlVehicleNoChange(this);">
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Route</label>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <select id="cmb_routes" class="txtddlClass">
                                        </select>
                                    </td>
                                    <td>
                                    <input type="button" id="btnviewroute" value="View" onclick="viewroute();" class="SaveButton"
                                        style="width: 50px; height: 20px; font-size: 13px; font-weight: bold;" />
                                   </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr class="divDriver">
                        <td>
                            <label>
                                Driver Name</label>
                        </td>
                        <td>
                            <select id="cmb_driver" class="txtddlClass">
                            </select>
                            <%--  <input id="cmb_driver" type="text" class="txtClass" list="cmb_driver" placeholder="Enter Employee Name" />
                            <datalist id="cmb_driver"></datalist>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Helper Name</label>
                        </td>
                        <td>
                            <select id="cmb_helper" class="txtddlClass">
                            </select>
                            <%--  <input id="txt_helper" type="text" class="txtClass" list="cmb_helper" placeholder="Enter Helper" />
                            <datalist id="cmb_helper"></datalist>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Make</label>
                        </td>
                        <td>
                            <span id="txtMake" class="txtClass"></span>
                            <%--  <input id="txtMake" type="text" class="txtClass" placeholder="Enter Make" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Model</label>
                        </td>
                        <td>
                            <span id="txtModel" class="txtClass"></span>
                            <%--  <input id="txtModel" type="text" class="txtClass" placeholder="Enter Model" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Type</label>
                        </td>
                        <td>
                            <span id="txtVehicleType" class="txtClass"></span>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr class="divVSR">
                        <td>
                            <label>
                                Vehicle Start Reading</label>
                        </td>
                        <td>
                            <input id="txtVehicleStartReading" type="text" class="txtClass" placeholder="Enter Vehicle Start Reading" />
                        </td>
                    </tr>
                    <tr class="divHR">
                        <td>
                            <label>
                                Hour Reading</label>
                        </td>
                        <td>
                            <input id="txtHrReading" type="text" class="txtClass" placeholder="Enter  Hour Reading" />
                        </td>
                    </tr>
                    <tr class="divFuelTank">
                        <td>
                            <label for="lblPhoneNo">
                                Fuel Tank</label>
                        </td>
                        <td>
                            <input id="txtFuelTank" type="text" class="txtClass" placeholder="Enter Fuel Tank" />
                        </td>
                        <td>
                        Ltrs
                        </td>
                    </tr>
                    <tr class="divFuelTank">
                        <td>
                            <label for="lblPhoneNo">
                                Load Type</label>
                        </td>
                        <td>
                            <input id="txt_load" type="text" class="txtClass" placeholder="Enter Load" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Quantity</label>
                        </td>
                        <td>
                            <input id="txtQty" type="text" class="txtClass" placeholder="Enter Quantity" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Trip Start From</label>
                        </td>
                        <td>
                            <input id="txt_tripstartfrom" type="text" class="txtClass" placeholder="Trip Start From" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <input type="button" id="BtnSave" value="Save" onclick="btnTripSheetSaveClick();"
                    class="SaveButton" style="width: 100px; height: 30px; font-size: 20px;" />&nbsp&nbsp
                    <%--  <input type="button" id="Btnprint" value="Print" onclick="btnprintClick();" class="SaveButton"
                    style="width: 100px; height: 30px; font-size: 20px;" />&nbsp&nbsp--%>
                <input type="button" id="btnreset" value="Reset" onclick="btnRefreshClick();" class="SaveButton"
                    style="width: 100px; height: 30px; font-size: 20px;" />
            </td>
        </tr>
    </table>
    <div id="divRouteAssign" class="pickupclass" style="text-align: center; height: 100%;
        width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
        background: rgba(192, 192, 192, 0.7);">
        <div id="divRoute" style="border: 5px solid #A0A0A0; position: absolute; top: 10%;
            background-color: White; left: 20%; right: 20%; width: 50%; height: 80%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            border-radius: 10px 10px 10px 10px;">
            <br />
            <br />
            <div id="divTrip" style="height: 80%; overflow: auto;">
            </div>
        </div>
        <div id="divclose" style="width: 25px; top: 8.5%; right: 29%; position: absolute;
            z-index: 99999; cursor: pointer;">
            <img src="Images/Close.png" alt="close" />
        </div>
    </div>
</asp:Content>
