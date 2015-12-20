<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="JobCard.aspx.cs" Inherits="JobCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/date.format.js" type="text/javascript"></script>
    <style>
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
        .Spancontrol
        {
            display: block;
            width: 206px;
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
            FillRegions();
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
        var Regions = "Fill Diesel,Check Air,Repair,Tyre Replacement,Tyre Rotation,Body Wash,Electrical Works,Nitrogen Fill,Minor Mechanical Works";
        function FillRegions() {
            document.getElementById('divWork').innerHTML = "";
            var Region = Regions.split(',');
            var data = "<table>";
            for (var i = 0; i <= Region.length; i++) {
                if (typeof (Region[i]) != "undefined") {
                    data += "<tr><td><input type='checkbox' name='checkbox' value='checkbox' onchange='ckb_onchange(this);' id = " + i + " class = 'chkclass'/><span>" + Region[i] + "</span></td><td><input type='text' value='' class = 'txtClass'/></td></tr>";
                }
            }
            data += "</table>";
            $('#divWork').append(data);
        }
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
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox') {
                    row.cells[0].childNodes[0].checked = false;
                    row.cells[0].childNodes[0].title = "";
                    row.cells[1].childNodes[0].value = "";
                }
            }
            
            var Jobcards = 'N';
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            for (i = 0; i < assignedtrips.length; i++) {
                if (assignedtrips[i].tripsno == tripsno) {
                    document.getElementById('txt_vehno').value = assignedtrips[i].vehicleno;
                    Jobcards = assignedtrips[i].Jobcards;
                    break;
                }
                else {
                    document.getElementById('txt_vehno').value = "";
                }
            }
            if (Jobcards == 'Y') {
                var data = { 'op': 'gettripjobcards', 'tripsno': tripsno };
                var s = function (msg) {
                    if (msg) {
                        for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                            if (row.cells[0].childNodes[0].type == 'checkbox') {
                                var labelval = row.cells[0].childNodes[1].innerHTML;
                                var txtval = row.cells[1].childNodes[0].value;
                                for (var jc = 0; jc < msg.length; jc++) {
                                    if (labelval == msg[jc].jobcard) {
                                        row.cells[0].childNodes[0].checked = true;
                                        row.cells[0].childNodes[0].title = msg[jc].jobcardstatus;
                                        row.cells[1].childNodes[0].value = msg[jc].jobcarddetails;
                                        break;
                                    }
                                }
                            }
                        }
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
        function BtnSaveClick() {
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            var jobcarddate = document.getElementById('txt_datetime').value;
            if (tripsno == "Select Trip") {
                alert("Please select trip to assign job cards");
                return false;
            }
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            var checkedjobcards = [];
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox' && row.cells[0].childNodes[0].checked == true) {
                    var labelval = row.cells[0].childNodes[1].innerHTML;
                    var txtval = row.cells[1].childNodes[0].value;
                    checkedjobcards.push({ 'jobtype': labelval, 'jobdetails': txtval });
                }
            }
           
            if (checkedjobcards.length == 0) {
                alert("Please select job cards");
                return false;
            }

            var data = { 'op': 'jobcardsaveclick', 'jobcarddate': jobcarddate, 'tripsno': tripsno, 'checkedjobcards': checkedjobcards };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    FillTripsheets();
                    btnRefreshClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
        }
        function btnRefreshClick() {
            document.getElementById('cmb_Tripsheets').selectedIndex = 0;
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
            document.getElementById('txt_vehno').value = "";
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            var ckdvlsdiv = document.getElementById('divWork').childNodes;
            for (var i = 0, row; row = ckdvlsdiv[0].rows[i]; i++) {
                if (row.cells[0].childNodes[0].type == 'checkbox') {
                    row.cells[0].childNodes[0].checked = false;
                    row.cells[0].childNodes[0].title = "";
                    row.cells[1].childNodes[0].value = "";
                }
            }
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
        }
        function ckb_onchange(id) {
            if (id.title == "C") {
                id.checked = true;
                alert("This jobcard already completed");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table align="center">
        <tr>
            <td colspan="2">
                <span style="text-align: center; font-size: 20px; color: orange;">Job Card</span>
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
                    Assigned Tripsheets</label>
            </td>
            <td>
            <select id="cmb_Tripsheets" class="txtClass" onchange="tripsheet_change();" style="width: 210px;"></select>
            </td>
        </tr>
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
                <input id="txt_vehno" type="text" class="txtClass" placeholder="Vehicle Nunber" disabled="disabled"/>
            </td>
        </tr>
        </table>
        </td>
        <td><table>
        <tr>
            <td colspan="2" align="center" style="text-align:left;">
                <div id="divWork" style="float: left; width: 450px; height: 100%; border: 1px solid gray;
                    overflow: auto; padding-left: 10%; border-radius: 7px 7px 7px 7px; padding-top: 3%;font-weight:bold;">
                </div>
            </td>
        </tr>
        </table></td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <input type="button" id="BtnSave" value="Save" onclick="BtnSaveClick();" class="SaveButton"
                    style="width: 137px; height: 30px; font-size: 20px;" />&nbsp&nbsp
                     <input type="button" id="btnreset" value="Reset" onclick="btnRefreshClick();" class="SaveButton"
                    style="width: 137px; height: 30px; font-size: 20px;" />
            </td>
        </tr>
    </table>
</asp:Content>
