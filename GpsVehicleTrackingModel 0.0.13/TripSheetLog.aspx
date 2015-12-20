<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TripSheetLog.aspx.cs" Inherits="TripSheetLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
 <script src="js/JTemplate.js?v=1001" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
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
        var LogsData = [];
        var ExpensesData = [];
        $(function () {
            FillTripsheets();
            LogsData = [];
            ExpensesData = [];
            for (var i = 1; i < 11; i++) {
                LogsData.push({ Sno: i });
            }
            for (var j = 1; j < 11; j++) {
                ExpensesData.push({ Sno: j });
            }
            $('#divFillScreen').removeTemplate();
            $('#divFillScreen').setTemplateURL('TripSheetLogs.htm');
            $('#divFillScreen').processTemplate(LogsData);
            $('#divTripExpenses').removeTemplate();
            $('#divTripExpenses').setTemplateURL('tripexpenses.htm');
            $('#divTripExpenses').processTemplate(ExpensesData);
            FillExpenses();
        });
        function FillExpenses() {
            var data = { 'op': 'GetTripExpenses' };
            var s = function (msg) {
                if (msg) {
                    BindExpenses(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        var expenses;
        function BindExpenses(msg) {
            expenses = msg;
            $(".ddlExpClass").each(function () {
                var ddlexp = $(this);
                ddlexp[0].options.length = null;
                var opt = document.createElement('option');
                opt.innerHTML = "select";
                ddlexp[0].appendChild(opt);
                for (var i = 0; i < msg.length; i++) {
                    if (msg[i].ExpenseName != null) {
                        var opt = document.createElement('option');
                        opt.innerHTML = msg[i].ExpenseName;
                        opt.value = msg[i].ExpenseName;
                        ddlexp[0].appendChild(opt);
                    }
                }
            });
        }
        function btnAddTripLogsRowClick() {
            LogsData = [];
            var txtsno = "";
            var txtTripDate = "";
            var txtKms = "";
            var txtPlace = "";
            var txtDetails = "";
            var txtAmount = "";
            var txtQty = "";
            var txtDiesel = "";
            var rows = $("#table_TripSheetDetails tr:gt(0)");
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtSNo').text() != "") {
                    txtsno = $(this).find('#txtSNo').text();
                    txtTripDate = $(this).find('#txtTripDate').val();
                    txtKms = $(this).find('#txtKms').val();
                    txtPlace = $(this).find('#txtPlace').val();
                    txtDetails = $(this).find('#txtDetails').val();
                    txtAmount = $(this).find('#txtAmount').val();
                    txtQty = $(this).find('#txtQty').val();
                    txtDiesel = $(this).find('#txtDiesel').val();
                    LogsData.push({ Sno: txtsno, TripDate: txtTripDate, Kms: txtKms, Place: txtPlace, Details: txtDetails, Amount: txtAmount, Qty: txtQty, Diesel: txtDiesel });
                }
            });
            var Sno = parseInt(txtsno) + 1;
            txtTripDate = "";
            txtKms = "";
            txtPlace = "";
            txtDetails = "";
            txtAmount = "";
            txtQty = "";
            txtDiesel = "";
            LogsData.push({ Sno: Sno, TripDate: txtTripDate, Kms: txtKms, Place: txtPlace, Details: txtDetails, Amount: txtAmount, Qty: txtQty, Diesel: txtDiesel });
            $('#divFillScreen').removeTemplate();
            $('#divFillScreen').setTemplateURL('TripSheetLogs.htm');
            $('#divFillScreen').processTemplate(LogsData);
        }
        function btnTripExpenseAddRowClick() {
            ExpensesData = [];
            var txtsno = "";
            var ddlExpenceType = "";
            var txtPlace = "";
            var txtAmount = "";
            var rows = $("#table_TripExpenses tr:gt(0)");
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtSNo').text() != "") {
                    txtsno = $(this).find('#txtSNo').text();
                    ddlExpenceType = $(this).find('#ddlExpenceType').val();
                    txtPlace = $(this).find('#txtPlace').val();
                    txtAmount = $(this).find('#txtAmount').val();
                    ExpensesData.push({ Sno: txtsno, ExpenceType: ddlExpenceType, Place: txtPlace, Amount: txtAmount });
                    FillExpenses();
                }
            });
            var Sno = parseInt(txtsno) + 1;
            ddlExpenceType = "";
            txtPlace = "";
            txtAmount = "";
            ExpensesData.push({ Sno: Sno, ExpenceType: ddlExpenceType, Place: txtPlace, Amount: txtAmount });
            $('#divTripExpenses').removeTemplate();
            $('#divTripExpenses').setTemplateURL('tripexpenses.htm');
            $('#divTripExpenses').processTemplate(ExpensesData);
            FillExpenses();
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
        function btnSaveTripsheetLogsClick() {
            var tripsno = document.getElementById('cmb_Tripsheets').value;
            if (tripsno == "Select Trip") {
                alert("Please select assign trip");
                return false;
            }
            var rows = $("#table_TripSheetDetails tr:gt(0)");
            var tripLogDetails = new Array();
            $(rows).each(function (i, obj) {
                if ($(this).find('#txtAmount').val() != "") {
                    tripLogDetails.push({ TripDate: $(this).find('#txtTripDate').val(), Kms: $(this).find('#txtKms').val(), Place: $(this).find('#txtPlace').val(), Details: $(this).find('#txtDetails').val(), Amount: $(this).find('#txtAmount').val(), Qty: $(this).find('#txtQty').val(), Diesel: $(this).find('#txtDiesel').val() });
                }
            });
            if (tripLogDetails.length == 0) {
                alert("Enter Trip locations");
                return false;
            }
            var Exprows = $("#table_TripExpenses tr:gt(0)");
            var tripExpDetails = new Array();
            $(Exprows).each(function (i, obj) {
                var tripexp = $(this).find('#ddlExp');
                if (tripexp[0].value != "select") {
                    tripExpDetails.push({ ExpenseType: tripexp[0].value, Place: $(this).find('#txtPlace').val(), Amount: $(this).find('#txtAmount').val() });
                }
            });
            var data = { 'op': 'btnSaveTripsheetLogsClick', 'tripsno': tripsno, 'tripLogDetails': tripLogDetails, 'tripExpDetails': tripExpDetails };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    FillTripsheets();
                    LogsData = [];
                    for (var i = 1; i < 11; i++) {
                        LogsData.push({ Sno: i });
                    }
                    $('#divFillScreen').removeTemplate();
                    $('#divFillScreen').setTemplateURL('TripSheetLogs.htm');
                    $('#divFillScreen').processTemplate(LogsData);
                    ExpensesData = [];
                    for (var j = 1; j < 11; j++) {
                        ExpensesData.push({ Sno: j });
                    }
                    $('#divTripExpenses').removeTemplate();
                    $('#divTripExpenses').setTemplateURL('tripexpenses.htm');
                    $('#divTripExpenses').processTemplate(ExpensesData);
                    FillExpenses();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            CallHandlerUsingJson(data, s, e);
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
  </script>
  <style type="text/css">
     .Spancontrol
        {
            display: block;
            width: 156px;
            height:28px;
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
         .datecss
        {
            display: block;
            width: 196px;
            height:28px;
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
        .ddlExpClass
        {
            display: block;
            width: 196px;
            height:28px;
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
  </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div align="center" style="height: 90px;">
        <span style="color: Red; font-size: 18px;">Trip Sheet Logs</span>
    </div>
    <table align="center">
        <tr>
            <td>
                <label>
                    Assigned Tripsheets</label>
            </td>
            <td>
                <select id="cmb_Tripsheets" class="Spancontrol" style="width: 210px;">
                </select>
            </td>
        </tr>
    </table>
    <div id="divFillScreen">
    </div>
    <div>
        <table style="width: 100%;">
            <tr>
                <td style="float: right;">
                    <input type="button" id="btnTripLogs" value="Add Locations" onclick="btnAddTripLogsRowClick();"
                        class="ContinueButton" style="width: 130px; height: 30px; font-size: 18px;" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divTripExpenses">
    </div>
    <table style="width: 100%;">
        <tr>
            <td style="padding-left: 45%;">
                <input type="button" id="Btnprint" value="Save" onclick="btnSaveTripsheetLogsClick();"
                    class="ContinueButton" style="width: 100px; height: 30px; font-size: 20px;" />&nbsp&nbsp
            </td>
          <%--  <td style="float: right;">
                <input type="button" id="btnTripExpense" value="Add Expenses" onclick="btnTripExpenseAddRowClick();" class="ContinueButton"
                    style="width: 130px; height: 30px; font-size: 18px;" />
            </td>--%>
        </tr>
    </table>
</asp:Content>

