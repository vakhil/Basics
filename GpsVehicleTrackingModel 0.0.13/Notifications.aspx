<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Notifications.aspx.cs" MasterPageFile="~/MasterPage.master"
    Inherits="Notifications" %>

<asp:Content ID="headcontecnt" ContentPlaceHolderID="HeadContent" runat="server">
 <script src="js/JTemplate.js?v=1001" type="text/javascript"></script>
    <link href="CSS/jquery-ui-1.10.3.custom.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
    <script src="DropDownCheckList.js?v=1001" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <style type="text/css">
     html, body
        {
            margin: 0;
            padding: 0;
            height: 100%;
        }
        .vehiclecls
        {
            cursor:pointer;
            height:30px;
            background-color:#ffffcc;
            border:solid 1px #d5d5d5;
            text-align:center;
        }
        .vehiclelogcls
        {
            background-color:#fffffc;
            border:solid 1px #d5d5d5;
            text-align:center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var Username = '<%= Session["field1"] %>';
            var data = { 'op': 'getnotificationvehicles', Username: Username };
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
        });

        function fillvehicles(msg) {
            var maindiv = document.getElementById('divvehicles');
            for (var i = 0; i < msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].vehicleno == "" || msg[i].vehicleno == null) {
                }
                else {
                    var vehnumber = msg[i].vehicleno;
                    var iDiv = document.createElement('div');
                    iDiv.id = vehnumber;
                    iDiv.className = 'vehiclecls';
                    iDiv.innerHTML = vehnumber;
                    iDiv.onclick = function () { vehicleonclick(this); };
                    maindiv.appendChild(iDiv);
                }
            }
        }

        function vehicleonclick(clickedveh) {
            var Username = '<%= Session["field1"] %>';
            var vehno = clickedveh.innerHTML;
            var data = { 'op': 'getvehnotification', VehicleNo: vehno };
            var s = function (msg) {
                if (msg) {
                    fillvehlogs(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }

        function fillvehlogs(msg) {
            var maindiv = document.getElementById('divvehlogs');
            maindiv.innerHTML = "";
            for (var i = 0; i < msg.length; i++) {
                if (typeof msg[i] === "undefined" || msg[i].vehicleid == "" || msg[i].vehicleid == null) {
                }
                else {
                    var iDiv = document.createElement('div');
                    iDiv.className = 'vehiclelogcls';
                    var vehicleid = msg[i].vehicleid;
                    var aletdt = msg[i].aletdt;
                    var alertinfo = msg[i].alertinfo;
                    var alerttype = msg[i].alerttype;
                    iDiv.innerHTML = 'Vehicle Number : ' + vehicleid + '<br />Date : ' + aletdt + '<br /><span style="text-decoration:underline;">Notification</span><br />' + alertinfo + '<br />Notification Type : ' + alerttype + '<br />';
                    iDiv.onclick = function () { vehicleonclick(this); };
                    maindiv.appendChild(iDiv);
                }
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="height:100%;width:100%;">
        <div id="divvehicles" style="width:30%; float:left; border: 1px solid #d5d5d5;top: 57px;bottom:0px; position:absolute;overflow:auto;">
        </div>
        <div id="divvehlogs" style="width:69.5%; float:right;border: 1px solid #d5d5d5;top: 57px;bottom:0px; position:absolute;overflow:auto;left: 30%;">
        </div>
    </div>
</asp:Content>
