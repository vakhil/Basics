<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AssignAlerts.aspx.cs" Inherits="AssignAlerts" %>

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

     <style type="text/css">

#ldiv {
    height: 100%;
    width: 35%;
    position:relative;
    float:left;
    background-color:#fff;
    color:#6D5353;
    
   margin-left: 2%;
   overflow:auto;
}


#rdiv {
    vertical-align: top; position: relative;width: 50%;float: left;
}
@media only screen and (max-width: 960px) 
{
    #ldiv {
    height: 100%;
    width: 35%;
    position:relative;
    float:left;
    background-color:#fff;
    color:#ccc;
    
   margin-left: 2%;
}


#rdiv {
    vertical-align: top; position: relative;width: 50%;float: left;
}

}

    
    .divselectedclass
    {
        border:1px solid gray;
        padding-top:2px;
        padding-bottom:2px;
    }
     .back-red { background-color: #ffffcc; }
     .back-white { background-color: #ffffff; }
     
     .unitline
        {
            font: inherit;
            width: 120px;
        }
        .iconminus
        {
            float: right;
            width: 20px;
            height: 20px;
            margin: 2px 0 0 0;
            background: url("Images/minus.png") no-repeat;
            border-radius: 2px 2px 2px 2px;
        }
        .titledivcls
        {
            height: 70px;
        }
        .divcategory
        {
            border-bottom-style: dashed;
            border-bottom-color: #D6D6D6;
            border-bottom-width: 1px;
        }
           .activeanchor
        {
            text-decoration: none;
            color: #000000;
        }
       #tablestpinpoishow tr:hover {
    background-color: #ccc;

}
    
    </style>



    <script type="text/javascript">
        $(function () {
            getallvehicles();
            get_persondetails();
            retrievealrtnme();
            grid_vehicelsass();
            get_assignalerts();

        });
        // $('#tablestpinpoishow').css('cursor', 'pointer');

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
        function minusclick(thisid) {
            var prntdiv = $(thisid).parents(".titledivcls");
            ul = prntdiv.next("ul");
            if (thisid.title == "Hide") {
                ul.slideUp("slow");
                $(thisid).attr('title', "Show");
                $(thisid).css("background", "url('Images/plus.png') no-repeat");
            }
            else {
                ul.slideDown("slow");
                $(thisid).attr('title', "Hide");
                $(thisid).css("background", "url('Images/minus.png') no-repeat");
            }
        }

        function getallvehicles() {
            var data = { 'op': 'getvehicles' };
            var s = function (msg) {
                if (msg) {
                    getvehicleno(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(data, s, e);
        }
        var allvehdata;

        function getvehicleno(msg) {
            allvehdata = msg;
            //        var selectedplant = document.getElementById('divvehicles').value
            //        if (selectedplant == "") {
            //            selectedplant = "Select All";
            //        }
            document.getElementById('divvehicles').innerHTML = "";
            var plants = [];
            $("#divvehicles").append("<div id='VehicleNumber' class='divcategory'>");
            //        for (var i = 0; i < allvehdata.length; i++) {
            //            if (typeof allvehdata[i] === "undefined" || allvehdata[i].VehicleNumber == "" || allvehdata[i].VehicleNumber == null) {
            //            }
            //            else {
            //                var tVehicleNumber = allvehdata[i].VehicleNumber;
            //                var tSno = allvehdata[i].Sno;

            //                //                if (selectedplant != "Select All") {
            //                //                    if (tSno == selectedplant) {
            //                tVehicleNumber = tVehicleNumber.replace(/[^a-zA-Z0-9]/g, '');
            //                var exists = plants.indexOf(tVehicleNumber);
            //                if (exists == -1) {
            //                    var VehicleNumber = allvehdata[i].VehicleNumber;
            //                    VehicleNumber = VehicleNumber.replace(/[^a-zA-Z0-9]/g, '');
            //                    plants.push(VehicleNumber);
            //                    $("#divvehicles").append("<div id='div" + VehicleNumber + "' class='divcategory'>");
            //                }
            //                //                    }
            //                //                }
            //                //                        else {
            //                tVehicleNumber = tVehicleNumber.replace(/[^a-zA-Z0-9]/g, '');
            //                var exists = plants.indexOf(tVehicleNumber);
            //                if (exists == -1) {
            //                    var VehicleNumber = allvehdata[i].VehicleNumber;
            //                    VehicleNumber = VehicleNumber.replace(/[^a-zA-Z0-9]/g, '');
            //                    plants.push(VehicleNumber);
            //                    $("#divvehicles").append("<div id='div" + VehicleNumber + "' class='divcategory'>");
            //                }
            //            }
            //            //                    }
            //        }
            $("#VehicleNumber").append("<div class='titledivcls'><table style='width:100%;'><tr><td style='width: 120px;'><h2 class='unitline'> Vehicle Numbers </h2></td><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr><tr><td style='width: 120px;'><input id='allchk' type='checkbox' onchange='vehonchange()'/>Select All</td></tr></table></div>");
            $("#VehicleNumber").append("<ul id='ulAllVehicles' class='ulclass'>");
            for (var p = 0; p < allvehdata.length; p++) {
                //            for (var i = 0; i < allvehdata.length; i++) {
                //                var tVehicleNumber = allvehdata[i].VehicleNumber;
                //                tVehicleNumber = tVehicleNumber.replace(/[^a-zA-Z0-9]/g, '');
                if (typeof allvehdata[p] === "undefined" || allvehdata[p].VehicleNumber == "" || allvehdata[p].VehicleNumber == null) {
                }
                else {
                    //                    if (allvehdata[p] == tVehicleNumber) {
                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = allvehdata[p].Sno;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    checkbox.className = 'chkclass';
                    checkbox.onclick = 'checkclick();';
                    document.getElementById('ulAllVehicles').appendChild(checkbox);
                    label.innerHTML = allvehdata[p].VehicleNumber;
                    document.getElementById('ulAllVehicles').appendChild(label);
                    document.getElementById('ulAllVehicles').appendChild(hidden);
                    document.getElementById('ulAllVehicles').appendChild(document.createElement("br"));
                    //                    }
                    //                }
                }
            }
            get_persondetails();
        }
        function retrievealrtnme() {
            var data = { 'op': 'retrieve_alrtname' };
            var s = function (msg) {
                if (msg) {
                    if (msg.length > 0) {
                        fillalertslct(msg);
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

        function fillalertslct(msg) {
            allbranchesdata = msg;
            var plants = [];
            var sel = document.getElementById('slct_alrtgrp');
            var opt = document.createElement('option');
            opt.innerHTML = "Select Alert";
            opt.value = "Select Alert";
            sel.appendChild(opt);
            for (var i = 0; i < msg.length; i++) {

                if (msg[i].alert_name != null && msg[i].alert_name != "") {
                    var alert_name = allbranchesdata[i].alert_name;
                    var alert_sno = allbranchesdata[i].alert_sno;
                    // plants.push(alert_name);
                    var opt = document.createElement('option');
                    opt.innerHTML = alert_name;
                    opt.value = alert_sno;
                    sel.appendChild(opt);
                }

            }
        }

        function grid_vehicelsass() {

            jQuery("#grid_vehicelsass").jqGrid({
                datatype: "local",
                height: '250px',
                autowidth: true,
                colNames: ['AssignName', 'Alert Group', 'Vehicles', 'Assigned Persons', 'Sno', 'vehsno', 'persno'],
                colModel: [
                        { name: 'AssignName', index: 'AssignName', width: 150 },
   		                { name: 'AlertGroup', index: 'AlertGroup', width: 150 },
                   		{ name: 'Vehicles', index: 'Vehicles', width: 200 },
                   		{ name: 'Persons', index: 'Persons', width: 200 },
   		                { name: 'Sno', index: 'Sno', width: 50, hidden: true },
   		                { name: 'vehsno', index: 'vehsno', width: 50, hidden: true },
   		                { name: 'persno', index: 'persno', width: 50, hidden: true },

   	                  ],
                rowNum: 10,
                gridview: true,
                //loadonce: true,
                rowList: [10, 20, 30,40,50],
                //sortname: 'Sno',
                viewrecords: true,
                sortorder: 'asc',
                pager: '#pager',
                rownumbers: true,
                shrinkToFit: true,
                // multiselect: true,
                Find: "Find",
                caption: "Vehicle Alerts",
                ondblClickRow: function (id) {
                    $('.chkclass').each(function () {
                        $(this).attr("checked", false);
                    });
                    var AssignName = $('#grid_vehicelsass').jqGrid('getCell', id, 'AssignName');
                    var AlertGroup = $('#grid_vehicelsass').jqGrid('getCell', id, 'AlertGroup');
                    var Vehicles = $('#grid_vehicelsass').jqGrid('getCell', id, 'Vehicles');
                    var Persons = $('#grid_vehicelsass').jqGrid('getCell', id, 'Persons');
                    var Sno = $('#grid_vehicelsass').jqGrid('getCell', id, 'Sno');
                    var vehsno = $('#grid_vehicelsass').jqGrid('getCell', id, 'vehsno');
                    var persno = $('#grid_vehicelsass').jqGrid('getCell', id, 'persno');

                    $("select#slct_alrtgrp option").each(function () { this.selected = (this.text == AlertGroup); });
                    $('#txt_assignmentnme').val(AssignName);
                    $('#sno_lbl').html(Sno);
                    $('#vehsno_lbl').html(vehsno);
                    $('#persno_lbl').html(persno);
                    //$('#tdsfor_txt').val(TAXPer);
                    //$('#status_slct').val(Status);
                    var vehiclesarr = [];
                    var persnsarr = [];
                    vehiclesarr = vehsno.split(",");
                    persnsarr = persno.split(",");
                    if (vehiclesarr[0] != "") {
                        for (var i = 0; i < vehiclesarr.length; i++) {
                            $('.chkclass').each(function () {
                                if ($(this).next().next().val() == vehiclesarr[i]) {
                                    $(this).attr("checked", true);
                                    $(this).attr("disabled", false);
                                }
                            });
                        }
                    }
                    if (persnsarr[0] != "") {
                        for (var i = 0; i < persnsarr.length; i++) {
                            $('.chkclass').each(function () {
                                if ($(this).next().next().val() == persnsarr[i]) {
                                    $(this).attr("checked", true);
                                    $(this).attr("disabled", false);
                                }
                            });
                        }
                    }


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
                        persondata(msg);
                    }
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(Data, s, e);

        }

        var allpersondata;
        function persondata(msg) {
            allpersondata = msg;
            document.getElementById('persondetails').innerHTML = "";
            var plants = [];
            $("#persondetails").append("<div id='persons' class='divcategory'>");

            $("#persons").append("<div class='titledivcls'><table style='width:100%;'><tr><td style='width: 120px;'><h2 class='unitline'> Person Details </h2></td><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr><tr><td style='width: 120px;'><input id='allchkper' type='checkbox' onchange='peronchange()'/>Select All</td></tr></table></div>");
            $("#persons").append("<ul id='ulAllPersons' class='ulclass'>");
            for (var p = 0; p < allpersondata.length; p++) {

                if (typeof allpersondata[p] === "undefined" || allpersondata[p].pname == "" || allpersondata[p].pname == null) {
                }
                else {

                    var label = document.createElement("span");
                    var hidden = document.createElement("input");
                    hidden.type = "hidden";
                    hidden.name = "hidden";
                    hidden.value = allpersondata[p].sno;
                    var checkbox = document.createElement("input");
                    checkbox.type = "checkbox";
                    checkbox.name = "checkbox";
                    checkbox.value = "checkbox";
                    checkbox.id = "checkbox";
                    checkbox.className = 'chkclass';
                    checkbox.onclick = 'checkclick();';
                    document.getElementById('ulAllPersons').appendChild(checkbox);
                    label.innerHTML = allpersondata[p].pname;
                    document.getElementById('ulAllPersons').appendChild(label);
                    document.getElementById('ulAllPersons').appendChild(hidden);
                    document.getElementById('ulAllPersons').appendChild(document.createElement("br"));

                }
            }
        }

        function assignalerts_save() {
            var vehicleids = $("#divvehicles input:checkbox:checked").map(function () {
                return $(this).next().next().val();
            }).toArray();
            var personids = $("#persondetails input:checkbox:checked").map(function () {
                return $(this).next().next().val();
            }).toArray();

            var assgnnme = document.getElementById('txt_assignmentnme').value;
            var t = document.getElementById('slct_alrtgrp');
            var alrtgrp = t.options[t.selectedIndex].value;
            var btnval = document.getElementById('btn_save').value;
            var sno = document.getElementById('sno_lbl').innerHTML;
            var persno = document.getElementById('persno_lbl').innerHTML;
            var vehsno = document.getElementById('vehsno_lbl').innerHTML;
            //var sno = document.getElementById('txt_assignmentnme').value;
            var confi;
            if (btnval == "SAVE") {
                confi = confirm("Do you want to SAVE This Alert Group????");
            }
            else {
                confi = confirm("Do you want to EDIT This Alert Group????");
            }
            if (confi) {
                var Data = { 'op': 'assignalerts_save', 'assgnnme': assgnnme, 'alrtgrp': alrtgrp, 'vehicleids': vehicleids, 'personids': personids, 'btnval': btnval, 'sno': sno, 'persno': persno, 'vehsno': vehsno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            alert(msg);
                            get_assignalerts();
                            forclear();
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                CallHandlerUsingJson(Data, s, e);
            }

        }

        function get_assignalerts() {
            var Data = { 'op': 'get_assignalerts' };
            var s = function (msg) {
                if (msg) {

                    fillgrid(msg);

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(Data, s, e);

        }


        function fillgrid(databind) {
            $("#grid_vehicelsass").jqGrid("clearGridData");
            var newarray = [];
            for (var Booking in databind) {
                newarray.push({ 'AssignName': databind[Booking].alertassignmentName, 'AlertGroup': databind[Booking].alert_name, 'Vehicles': databind[Booking].vehicle, 'Persons': databind[Booking].person, 'Sno': databind[Booking].sno, 'vehsno': databind[Booking].vehicle_sno, 'persno': databind[Booking].person_sno });
            }

            var mydata = newarray;
            for (var i = 0; i <= mydata.length; i++) {

                jQuery("#grid_vehicelsass").jqGrid('addRowData', i + 1, mydata[i]);
            }
            disablechecks();
        }
        function fordelete() {
            var sno = document.getElementById('sno_lbl').innerHTML;
            var Data = { 'op': 'assignvehper_del', 'sno': sno };
            var s = function (msg) {
                if (msg) {

                    alert(msg);
                    get_assignalerts();
                    forclear();

                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(Data, s, e);
        }

        function forclear() {
            document.getElementById('txt_assignmentnme').value = "";
            document.getElementById('slct_alrtgrp').value = "Select Alert";
            document.getElementById('btn_save').value = "SAVE";
            document.getElementById('persno_lbl').innerHTML = "";
            document.getElementById('vehsno_lbl').innerHTML = "";
            document.getElementById('sno_lbl').innerHTML = "";
            document.getElementById('allchk').checked = false;
            document.getElementById('allchkper').checked = false;
            $('#grid_vehicelsass').jqGrid('resetSelection');
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            disablechecks();
        }

        function vehonchange() {
            if (document.getElementById('allchk').checked == true) {
                $('div#divvehicles input[type=checkbox]').each(function () {
                    $(this).attr("checked", true);
                });
            }
            else {
                $('div#divvehicles input[type=checkbox]').each(function () {
                    $(this).attr("checked", false);
                });
            }
        }
        function peronchange() {
            if (document.getElementById('allchkper').checked == true) {
                $('div#persondetails input[type=checkbox]').each(function () {
                    $(this).attr("checked", true);
                });
            }
            else {
                $('div#persondetails input[type=checkbox]').each(function () {
                    $(this).attr("checked", false);
                });
            }
        }


        function disablechecks() {
            var ids = $('#grid_vehicelsass').jqGrid('getRowData');
            var vehids = "";
            var perids = "";
            for (var i = 0; i < ids.length; i++) {
                vehids += ids[i].vehsno + ",";
                perids += ids[i].persno + ",";
            }
            vehids = vehids.substring(0, vehids.length - 1);
            perids = perids.substring(0, perids.length - 1);
            var veh = vehids.split(",");
            var per = perids.split(",");
            if (veh.length != 0) {
                for (var i = 0; i < veh.length; i++) {
                    $('.chkclass').each(function () {
                        if ($(this).next().next().val() == veh[i]) {
                            $(this).attr("disabled", true);
                        }
                    });
                }
            }
            if (per.length != 0) {
                for (var i = 0; i < per.length; i++) {
                    $('.chkclass').each(function () {
                        if ($(this).next().next().val() == per[i]) {
                            $(this).attr("disabled", true);
                        }
                    });
                }
            }
        }


    </script>



    <div align="center">
        <div>
            <table>
                <tr>
                <td>
                Assignment Name
                </td>
                <td>
                    <input id="txt_assignmentnme" type="text" />
                </td>
                    <td>
                        Alert Group
                    </td>
                    <td>
                        <select id="slct_alrtgrp" style="width: 150px;">
                       
                        </select>
                         <label id="persno_lbl" style="display:none;"></label>
                        <label id="vehsno_lbl" style="display:none;"></label>
                        <label id="sno_lbl" style="display:none;"></label>
                    </td>
                    
                </tr>
                <tr>
                <td colspan="10" align="center">
                <input id="btn_save" type="button" value="SAVE" onclick="assignalerts_save()"/>
                <input id="btn_delete" type="button" value="DELETE" onclick="fordelete()"/>
                <input id="btn_clear" type="button" value="CLEAR" onclick="forclear()"/>
                </td>
                </tr>
            </table>
            
        </div>
    </div>

    <div style="width:100%;height:100%;">
    <div id="ldiv">
    <div id="divvehicles" style="float: left; width: 200px;height:530px; border: 1px solid gray;overflow:auto;">
                        </div>
                         <div id="persondetails" style="float: left; width: 200px;height:530px; border: 1px solid gray;overflow:auto;">
                        </div>
    </div>

    <div id="rdiv">
    <table id="grid_vehicelsass"></table>
    <div id="pager"></div>
    </div>

    </div>

</asp:Content>

