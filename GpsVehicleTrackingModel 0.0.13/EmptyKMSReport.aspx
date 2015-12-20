<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="EmptyKMSReport.aspx.cs" Inherits="EmptyKMSReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <link href="jquery.jqGrid-4.5.2/ui.Jquery.css" rel="stylesheet" type="text/css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <link href="jquery.jqGrid-4.5.2/js/i18n/jquery-ui-1.9.2.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../jquery.jqGrid-4.5.2/plugins/searchFilter.css" />
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/jquery.json-2.4.js" type="text/javascript"></script>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script type="text/javascript">

        $(function () { FillBranches(); });
        var rendererOptions = {
            draggable: true
        };
        var directionsDisplay = new google.maps.DirectionsRenderer(rendererOptions); ;
        var directionsService = new google.maps.DirectionsService();
        var map;

        var australia = new google.maps.LatLng(-25.274398, 133.775136);

        function initialize() {
            var mapOptions = {
                zoom: 7,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: australia
            };
            map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
            directionsDisplay.setMap(map);
            directionsDisplay.setPanel(document.getElementById('directionsPanel'));

            google.maps.event.addListener(directionsDisplay, 'directions_changed', function () {
                computeTotalDistance(directionsDisplay.directions);
            });
        }

        function computeTotalDistance(result) {
            var total = 0;
            var myroute = result.routes[0];
            for (var i = 0; i < myroute.legs.length; i++) {
                total += myroute.legs[i].distance.value;
            }
            total = total / 1000.
            googlekms = total;
        }
        google.maps.event.addDomListener(window, 'load', initialize);


        function FillBranches() {
            var data = { 'op': 'get_Routes' };
            var s = function (msg) {
                if (msg) {
                    fillroutes_divchklist(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
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

        var allbranchesdata;
        function fillroutes_divchklist(msg) {
            allbranchesdata = msg;
            var plants = [];
            var sel = document.getElementById('ddlselectplant');
            var opt = document.createElement('option');
            opt.innerHTML = "Select Plant";
            opt.value = "Select Plant";
            sel.appendChild(opt);
            for (var i = 0; i < allbranchesdata.length; i++) {
                if (typeof allbranchesdata[i] === "undefined" || allbranchesdata[i].PlantName == "" || allbranchesdata[i].PlantName == null) {
                }
                else {
                    if (plants.indexOf(allbranchesdata[i].PlantName) == -1) {
                        var plantname = allbranchesdata[i].PlantName;
                        var PlantSno = allbranchesdata[i].PlantSno;
                        plants.push(plantname);
                        var opt = document.createElement('option');
                        opt.innerHTML = plantname;
                        opt.value = PlantSno;
                        sel.appendChild(opt);
                    }
                }
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select All";
            opt.value = "Select All";
            sel.appendChild(opt);
        }

        var googlekms = 0;
        var kmstatus = '';
        function loopArray(list) {
            var i = 0;
            var loopArray1 = function (msg) {
                googlekms = 0;
                // call itself
                calcRoute(msg[i], function () {
                    if (kmstatus == "OK") {
                        // set x to next item
                        addRow('gridreport', msg[i].TripName, msg[i].emptykms, googlekms, msg[i].Latitude, msg[i].Longitude);
                        // any more items in array?
                        i++;
                        if (i < msg.length) {
                            loopArray1(msg);
                        }
                    }
                    else {
                        sleep(5000);
                        if (i < msg.length) {
                            loopArray1(msg);
                        }
                    }
                });
            }

            function sleep(milliseconds) {
                $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
                var start = new Date().getTime();
                for (var i = 0; i < 1e7; i++) {
                    if ((new Date().getTime() - start) > milliseconds) {
                        break;
                    }
                }
            }
            // start 'loop'
            loopArray1(list);
            function calcRoute(msg, callback) {
                var start = startlatlan;
                var end = msg.Latitude + "," + msg.Longitude;
                var request = {
                    origin: start,
                    destination: end,
                    travelMode: google.maps.DirectionsTravelMode.DRIVING
                };
                directionsService.route(request, function (response, status) {
                    kmstatus = status;
                    if (status == google.maps.DirectionsStatus.OK) {
                        directionsDisplay.setDirections(response);
                        callback();
                    }
                    else {
                        callback();
                    }
                });
            }
        }

        var startlatlan = '';
        function btnGenerateclick() {
            var table = document.getElementById('gridreport');
            $('#gridreport').css('display','block');
            var selectedplant = document.getElementById('ddlselectplant');
            var plantsno = selectedplant.value;
            if (plantsno == "Select Plant") {
                alert("Please select Plant Name");
                return false;
            }
            for (var i = 0; i < allbranchesdata.length; i++) {
                if (allbranchesdata[i].id === plantsno) {
                    startlatlan = allbranchesdata[i].latitude + ',' + allbranchesdata[i].longitude;
                }
            }
            $("#gridreport").find("tr:gt(0)").remove();
            var data = { 'op': 'get_routes_trips', 'Plantid': plantsno };
            var s = function (msg) {
                if (msg) {
                    loopArray(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }

        function addRow(tableID, TripName, Emptykms, googlekms, lat, lng) {
            var table = document.getElementById(tableID);
            var rowCount = table.rows.length;
            var row = table.insertRow(rowCount);
            var _sno = row.insertCell(0);
            _sno.innerHTML = rowCount;
            var _TripName = row.insertCell(1);
            _TripName.innerHTML = TripName;
            var _Emptykms = row.insertCell(2);
            _Emptykms.innerHTML = Emptykms;
            var _googlekms = row.insertCell(3);
            _googlekms.innerHTML = googlekms;
            var _diffkms = row.insertCell(4);
            var diffkms = parseFloat(googlekms) - parseFloat(Emptykms);
            diffkms = diffkms.toFixed(2);
            _diffkms.innerHTML = diffkms;
            var _lng = row.insertCell(5);
            var element2 = document.createElement("input");
            element2.type = "button";
            element2.value = "Check on Map";
            element2.onclick = function () {
                window.open('GetDirections.aspx?Locations=' + startlatlan + '@' + lat + ',' + lng + '');
                return true;
            };
            _lng.appendChild(element2);
        }

        var tableToExcel = (function () {
            var uri = 'data:application/vnd.ms-excel;base64,'
    , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
    , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
    , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
            return function (table, name) {
                if (!table.nodeType) table = document.getElementById(table)
                var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
                window.location.href = uri + base64(format(template, ctx))
            }
        })()

    </script>
    <style type="text/css">
        .tableth
        {
            width:100px;
            border-right:1px solid #d5d5d5;
            border-bottom:1px solid #d5d5d5;
        }
    </style>
    <table>
        <tr>
            <td style="padding-left: 300px">
                <span>Select Plant</span>
                <select id="ddlselectplant" style="width: 170px;">
                </select>
            </td>
            <td>
                <input type="button" value="Generate" id="btnGenerate" onclick="btnGenerateclick();"
                    class="ContinueButton" />
            </td>
            <td>
            <input type="button" class="btntogglecls" style="height: 28px; opacity: 1.0; width: 100px;
                                    background-color: #d5d5d5; color: Blue;" onclick="tableToExcel('gridreport', 'W3C Example Table')"
                                    value="Export to Excel">
            </td>
        </tr>
        <tr>
            <td colspan="3" style="padding-left:30%;">
                <table id='gridreport' style="display: none; text-align:center; border:1px solid #d5d5d5;">
                    <tr>
                        <th class="tableth">
                            SNo
                        </th>
                        <th class="tableth">
                            Trip Name
                        </th>
                        <th class="tableth">
                            Empty KMs
                        </th>
                        <th class="tableth">
                            Google KMs
                        </th>
                        <th class="tableth">
                            Diff in Kms
                        </th>
                        <th class="tableth">
                            Check On Map
                        </th>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="display: none;">
        <div id="mapcontent">
            <div id="googleMap" style="width: 70%; height: 100%; position: relative; background-color: rgb(229, 227, 223);">
            </div>
        </div>
    </div>
</asp:Content>
