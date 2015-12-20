<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StatusReport.aspx.cs" Inherits="StatusReport" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
<script type="text/javascript">
    function GenerateClick() {
        document.getElementById("divStatus").innerHTML = "";
        var Fromdate = document.getElementById('txtFromDate').value;
        var todate = document.getElementById('txtToDate').value;
        if (Fromdate == "" ) {
            alert("Please Select Fromdate");
            return false;
        }
        if (todate == "") {
            alert("Please Select Todate");
            return false;
        }
        var data = { 'op': 'BtnGenerateclick', 'Fromdate': Fromdate, 'todate': todate };
        var s = function (msg) {
            if (msg) {
                if (msg == "No data were found") {
                    $('#divStatus').css('display', 'none');
                    alert("No data were found");
                }
                else {
                    $('#divStatus').css('display', 'block');
                    BindtoTables(msg)
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
    function BindtoTables(msg) {
        for (var i = 0; i < msg.length; i++) {
            var TripText = document.createElement('span');
            TripText.innerHTML = "Trip Name:";
            $('#divStatus').append(TripText);
            if (typeof msg[i] === "undefined") {
            }
            else {
                var TripName = document.createElement('span');
                TripName.innerHTML = msg[i].TripName;
                TripName.style.color = "#FF0000";
                $('#divStatus').append(TripName);
            }

            var VehicleNoText = document.createElement('span');
            VehicleNoText.innerHTML = "Vehicle No:";
            $('#divStatus').append(VehicleNoText);
            if (typeof msg[i] === "undefined") {
            }
            else {
                var VehicleNo = document.createElement('span');
                VehicleNo.innerHTML = msg[i].VehicleNo;
                VehicleNo.style.color = "#FF0000";
                $('#divStatus').append(VehicleNo);
            }

            var TripStartTimeText = document.createElement('span');
            TripStartTimeText.innerHTML = "Trip Start Time:";
            $('#divStatus').append(TripStartTimeText);
            if (typeof msg[i] === "undefined") {
            }
            else {
                var TripStartTime = document.createElement('span');
                TripStartTime.style.color = "#FF0000";
                TripStartTime.innerHTML = msg[i].Assigndate;
                $('#divStatus').append(TripStartTime);
            }

            var RouteNameText = document.createElement('span');
            RouteNameText.innerHTML = "Route Name:";
            $('#divStatus').append(RouteNameText);
            if (typeof msg[i] === "undefined") {
            }
            else {
                var RouteName = document.createElement('span');
                RouteName.innerHTML = msg[i].RouteName;
                RouteName.style.color = "red";
                $('#divStatus').append(RouteName);
            }
//            var divText = document.createElement('span');
//            divText.innerHTML = "Status:";
//            $('#divStatus').append(divText);
//            if (typeof msg[i] === "undefined") {
//            }
//            else {
//                var divstatus = document.createElement('span');
//                divstatus.innerHTML = msg[i].EndTime;
//                divstatus.style.color = "red";
//                $('#divStatus').append(divstatus);
//            }

            var table = document.createElement('Table');
            if (!table.tHead) {
                var header = table.createTHead();
                var rowx = header.insertRow(0);
                var cell1 = rowx.insertCell(0);
                var cell2 = rowx.insertCell(0);
                var cell3 = rowx.insertCell(0);
                cell3.innerHTML = "<b>Sno</b>";
                cell2.innerHTML = "<b>Location Name</b>";
                cell1.innerHTML = "<b>Enter Time</b>";
            }
            table.id = 'table' + i;
            table.style.height = '400px';
            table.style.width = '560px';
            table.border = "2";
            table.style.border = "1px solid #000"
            table.style.borderWidth = "1px";
            table.style.borderColor = "#000";
            table.style.borderStyle = "solid";
            var completedcount1 = 0;
            for (var j = msg[i].SubTriplist.length; j--; ) {
                //                if (document.getElementById('txtVehicleNo1').innerHTML == MsgData[j].VehicleNo) {
                var row = table.insertRow(1);
                var cell1 = row.insertCell(0);
                var cell2 = row.insertCell(1);
                var cell3 = row.insertCell(2);
                cell1.innerHTML = j + 1;
                cell2.innerHTML = msg[i].SubTriplist[j].LocationName;
                cell3.innerHTML = msg[i].SubTriplist[j].EnterTime;
                if (msg[i].SubTriplist[j].EnterTime != "") {
                    row.style.backgroundColor = "#FFFFCC";
                    completedcount1++;
                }
            }
            $('#divStatus').append(table);
            var brelem = document.createElement('br');
            $('#divStatus').append(brelem);
        }
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
</script>
<br />
        <br /><br />
        <br />
 <div class="shell" style="padding-left:120px;">
        
    <div style="width:100%;height:100%;">
    <div>
        <table>
            <tr>
                <td>
                    <label>
                        From Date</label>
                </td>
                <td>
                    <input id="txtFromDate" type="datetime-local" class="txtsize"/>
                </td>
                <td>
                    <label>
                        To Date</label>
                </td>
                <td>
                    <input id="txtToDate" type="datetime-local" class="txtsize"/>
                </td>
                <td>
                    <input type="button" id="btnGenerate" value="Generate" class="ContinueButton" onclick="GenerateClick();"/>
                </td>
            </tr>
        </table>
        </div>
        <div id="divStatus">
        </div>
    </div>
    <br />
  </div>
</asp:Content>
