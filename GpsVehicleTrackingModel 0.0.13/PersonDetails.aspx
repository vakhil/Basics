<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PersonDetails.aspx.cs" Inherits="PersonDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="jquery.jqGrid-4.5.2/ui.Jquery.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/jquery-ui-1.9.2.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../jquery.jqGrid-4.5.2/plugins/searchFilter.css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="jquery.jqGrid-4.5.2/src/i18n/grid.locale-en.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <script src="jquery.jqGrid-4.5.2/plugins/jquery.searchFilter.js" type="text/javascript"></script>
    <link href="jquery.jqGrid-4.5.2/js/Jquery.ui.css.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
     <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>

     <script type="text/javascript">
         $(function () {
             grid_persons();
             get_persondetails();
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
             $.ajax({
                 type: "GET",
                 url: "Bus.axd?json=",
                 dataType: "json",
                 contentType: "application/json; charset=utf-8",
                 data: JSON.stringify(d),
                 async: true,
                 cache: true,
                 success: s,
                 error: e
             });
         }

         function validateEmail(email) {
             var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
             if (reg.test(email)) {
                 return true;
             }
             else {
                 return false;
             }
         } 

         function forpesonsaving() {
             var name = document.getElementById('txt_persnname').value;
             var mobile = document.getElementById('txt_mobile').value;
             var mail = document.getElementById('txt_mail').value;
             var designation = document.getElementById('txt_designation').value;
             var btnval = document.getElementById('btn_save').value;
             var sno = document.getElementById('sno_lbl').innerHTML;

             if (!validateEmail(mail)) {
                 alert("Please Enter Proper Email ID");
                 return;
             }

             if (btnval == "SAVE") {
                 confi = confirm("Do you want to SAVE This Person Details????");
             }
             else {
                 confi = confirm("Do you want to EDIT This Person Details????");
             }
             if (confi) {
                 var Data = { 'op': 'Person_details_save', 'name': name, 'mobile': mobile, 'mail': mail, 'designation': designation, 'btnval': btnval, 'sno': sno };
                 var s = function (msg) {
                     if (msg) {
                         if (msg.length > 0) {
                             alert(msg);
                             get_persondetails();
                         }
                     }
                     else {
                     }
                 };
                 var e = function (x, h, e) {
                 };
                 callHandler(Data, s, e);
             }
         }
         function fordelete() {
             var sno = document.getElementById('sno_lbl').innerHTML;
             confi = confirm("Do you want to Delete This Person Details????");

             if (confi) {
                 var Data = { 'op': 'person_details_delete', 'sno': sno
                 };
                 var s = function (msg) {
                     if (msg) {
                         if (msg.length > 0) {
                             alert(msg);
                             get_persondetails();
                             forclear();

                         }
                     }
                     else {
                     }
                 };
                 var e = function (x, h, e) {
                 };
                 callHandler(Data, s, e);
             }
         }

         function grid_persons() {
             jQuery("#grid_persons").jqGrid({
                 datatype: "local",
                 height: '250px',
                 autowidth: true,
                 colNames: ['Person Name', 'Mobile', 'Email', 'Designation', 'sno'],
                 colModel: [
                        { name: 'PersonName', index: 'PersonName', width: 100 },
   		                { name: 'Mobile', index: 'Mobile', width: 150 },
                        { name: 'Email', index: 'Email', width: 300 },
                    	{ name: 'Designation', index: 'Designation', width: 150 },
   		                { name: 'Sno', index: 'Sno', width: 50, hidden: true },

   	                  ],
                 rowNum: 10,
                 gridview: true,
                 loadonce: true,
                 rowList: [10, 20, 30],
                 //sortname: 'Sno',
                 viewrecords: true,
                 sortorder: 'asc',
                 pager: '#pager',
                 rownumbers: true,
                 shrinkToFit: false,
                 // multiselect: true,
                 Find: "Find",
                 caption: "Vehicle Alerts",
                 onSelectRow: function (id) {

                     var PersonName = $('#grid_persons').jqGrid('getCell', id, 'PersonName');
                     var Mobile = $('#grid_persons').jqGrid('getCell', id, 'Mobile');
                     var Email = $('#grid_persons').jqGrid('getCell', id, 'Email');
                     var Designation = $('#grid_persons').jqGrid('getCell', id, 'Designation');
                     var Sno = $('#grid_persons').jqGrid('getCell', id, 'Sno');

                     $('#txt_persnname').val(PersonName);
                     $('#txt_mobile').val(Mobile);
                     $('#txt_mail').val(Email);
                     $('#txt_designation').val(Designation);
                     $('#sno_lbl').html(Sno);
                     document.getElementById('btn_save').value = "EDIT";

                 }

             }).jqGrid('navGrid', '#pager', { edit: false, add: false, del: false, refresh: false });
         }
         function get_persondetails() {

             var Data = { 'op': 'get_persondetails'
             };
             var s = function (msg) {
                 if (msg) {
                     if (msg.length > 0) {
                         fillgrid(msg);
                     }
                 }
                 else {
                 }
             };
             var e = function (x, h, e) {
             };
             callHandler(Data, s, e);

         }
         function fillgrid(databind) {
             $("#grid_persons").jqGrid("clearGridData");
             var newarray = [];
             for (var Booking in databind) {
                 newarray.push({ 'PersonName': databind[Booking].pname, 'Mobile': databind[Booking].phonenumber, 'Email': databind[Booking].emailid, 'Designation': databind[Booking].designation, 'Sno': databind[Booking].sno });
             }

             var mydata = newarray;
             for (var i = 0; i <= mydata.length; i++) {

                 jQuery("#grid_persons").jqGrid('addRowData', i + 1, mydata[i]);
             }
             forclear();
         }

         function forclear() {
             document.getElementById('txt_persnname').value = "";
             document.getElementById('txt_mobile').value = "";
             document.getElementById('txt_mail').value = "";
             document.getElementById('txt_designation').value = "";
             document.getElementById('btn_save').value = "SAVE";
             document.getElementById('sno_lbl').innerHTML = "";
             $('#grid_persons').jqGrid('resetSelection');
         }

     </script>

<div align="center">
<table>
<tr>
<td>
Person Name
</td>
<td>
    <input id="txt_persnname" type="text" />
</td>
<td>
Mobile Number
</td>
<td>
    <input id="txt_mobile" type="text" maxlength="10" />
</td>
</tr>
<tr>
<td>
EmailID
</td>
<td>
    <input id="txt_mail" type="text" />
</td>
<td>
Designation
</td>
<td>
    <input id="txt_designation" type="text" />
    <label style="display:none;" id="sno_lbl"></label>
</td>
</tr>
<tr>
<td colspan="10">
    <input id="btn_save" type="button" value="SAVE" onclick="forpesonsaving()"/>
    <input id="btn_delete" type="button" value="DELETE" onclick="fordelete()"/>
    <input id="btn_clr" type="button" value="CLEAR" onclick="forclear()"/>
</td>
</tr>
</table>
<div style="width:75%;">
<table id="grid_persons"></table>
<div id="pagers"></div>
</div>
</div>

</asp:Content>

