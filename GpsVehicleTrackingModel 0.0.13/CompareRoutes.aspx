<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CompareRoutes.aspx.cs" Inherits="CompareRoutes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckList" Namespace="UNLV.IAP.WebControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .btntogglecls:hover
        {
            cursor: pointer;
            -moz-box-shadow: 0px 0px 10px 0px #000000;
            -webkit-box-shadow: 0px 0px 10px 0px #000000;
            box-shadow: 0px 0px 10px 0px #000000;
        }
        .btntogglecls
        {
            display: inline-block;
            min-width: 54px;
            border: 1px solid rgba(0,0,0,0.1);
            text-align: center;
            color: #444;
            font-size: 12px;
            font-weight: bold;
            height: 28px;
            border-radius: 2px 2px 0px 0px;
            width: 10px;
            cursor: pointer;
            opacity: 2.0;
        }
        div#mapcontent
        {
            right: 0;
            bottom: 0;
            left: 0px;
            top: 48px;
            overflow: hidden;
            position: absolute;
        }
        .inner
        {
            width: 337px;
            position: absolute;
            left: 0px;
            bottom: 0px;
            color: Black;
            z-index: 99900;
            height: 100%;
            top: -2px;
            opacity: 0.9;
        }
        .togglediv
        {
            position: absolute;
            width: 318px;
            color: Black; /*z-index: 99900;*/
            border: 2px solid #d5d5d5;
            background-color: #ffffff;
            cursor: pointer;
            top: 57px;
            bottom: 0px;
            left: 0px;
            opacity: 0.9;
            z-index: 99999;
        }
        .datebuttonsleft
        {
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            cursor: pointer;
            margin: 0px;
            border-radius: 3px 0px 0px 0px;
            width: 35px;
            height: 30px;
            border: 0px solid #ffffff;
            background: #d5d5d5 url(Images/Left.png) no-repeat center;
        }
        .datebuttonsright
        {
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            cursor: pointer;
            margin: 0px;
            border-radius: 0px 3px 0px 0px;
            width: 35px;
            height: 30px;
            border: 0px solid #ffffff;
            background: #d5d5d5 url(Images/Right.png) no-repeat center;
        }
        .datebuttonsleft:hover
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background: #f5f5f5 url(Images/Left.png) no-repeat center;
            background-color: #dfdfdf;
        }
        .datebuttonsright:hover
        {
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #dfdfdf), color-stop(1, #ededed) );
            background: -moz-linear-gradient( center top, #dfdfdf 5%, #ededed 100% );
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#dfdfdf', endColorstr='#ededed');
            background: #f5f5f5 url(Images/Right.png) no-repeat center;
            background-color: #dfdfdf;
        }
        
        #slider_preview .ui-slider-range
        {
            background: #ef2929;
        }
        #slider_preview .ui-slider-handle
        {
            border-color: #ef2929;
        }
    </style>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&libraries=geometry"></script>
    <%-- <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>--%>
    <script src="Newjs/infobox.js" type="text/javascript"></script>
    <script type="text/javascript" src="DropDownCheckList.js"></script>
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
    <link href="js/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="font/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var map;
        function initialize() {
            var latlng = new google.maps.LatLng(18.33, 73.55);
            var myOptions = {
                zoom: 6,
                center: latlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("googleMap"), myOptions);
        }
        google.maps.event.addDomListener(window, "load", initialize);

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

    </script>
    <script type="text/javascript">
        $(function () {
            var fromtime1 = getParameterByName('fromtime1');
            document.getElementById("txtFromDate").value = fromtime1;
            var totime1 = getParameterByName('totime1');
            document.getElementById("txtToDate").value = totime1;
            var vehicle1 = getParameterByName('vehicle1');
            document.getElementById("txt_vehicles").value = vehicle1;

            var fromtime2 = getParameterByName('fromtime2');
            document.getElementById("txtFromDate1").value = fromtime2;
            var totime2 = getParameterByName('totime2');
            document.getElementById("txtToDate1").value = totime2;
            var vehicle2 = getParameterByName('vehicle2');
            document.getElementById("txt_vehicles1").value = vehicle2;

            var data = { 'op': 'InitilizeVehicles' };
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

        var vehicletypes;
        function fillvehicles(data) {
            vehicletypes = data;
            var vehiclenos = new Array();
            var options = '';
            for (var item in data) {
                options += '<option value="' + data[item].vehicleno + '" />';
            }
            document.getElementById('lst_vehicles').innerHTML = options;
            document.getElementById('lst_vehicles1').innerHTML = options;
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
        $(function () {
            $("#slider_preview").slider();
            var hidden = false;
            $("#btnClose").click(function () {
                if (hidden) {
                    $(".togglediv").stop().animate({ left: 0 }, 500);
                    hidden = false;
                    $("#btnClose").attr('title', "Hide");
                    $("#btnClose").attr('src', "Images/bigleft.png");
                }
                else {
                    $(".togglediv").css('margin-left', 0);
                    $(".togglediv").css('margin-right', 0);
                    $(".togglediv").animate({ left: '-320px' }, 500);
                    $("#btnClose").attr('title', "Show");
                    $("#btnClose").attr('src', "Images/bigright.png");
                    hidden = true;
                }
            });
        });
    </script>
    <script type="text/javascript">
        var Locationsdata = [];
        var polilinepath = [];
        var vehiclecnt = 1;
        function previewroute() {
            if (vehiclecnt == 1) {
                document.getElementById('lbl_veh1').innerHTML = "";
                document.getElementById('lbl_veh1_kms').innerHTML = "";
                document.getElementById('lbl_veh2').innerHTML = "";
                document.getElementById('lbl_veh2_kms').innerHTML = "";
                initialize();
            }
            var flightPlanCoordinates = new Array();
            addedlocations = [];
            allstoppedmarkers = [];
            var stoppedmarkers = [];
            for (var i = 0; i < stoppedmarkers.length; i++) {
                stoppedmarkers[i].setMap(null);
            }
            stoppedmarkers = [];
            prevbranch = "";
            imgcnt = 1;

            firstlog = false;
            count = 0;

            // Sets the map on all markers in the array.
            function allsetAllMap(map) {
                for (var i = 0; i < markersArray.length; i++) {
                    markersArray[i].setMap(map);
                }
                for (var i = 0; i < stoppedmarkers.length; i++) {
                    stoppedmarkers[i].setMap(map);
                }
                for (i = 0; i < polilinepath.length; i++) {
                    polilinepath[i].setMap(map); //or line[i].setVisible(false);
                }
            }

            // Removes the overlays from the map, but keeps them in the array.
            function clearallOverlays() {
                allsetAllMap(null);
            }


            $(function () {
                var startdt = "";
                var enddt = "";
                var checkedvehicle = "";
                if (vehiclecnt == 1) {
                    startdt = document.getElementById("txtFromDate").value;
                    enddt = document.getElementById("txtToDate").value;
                    checkedvehicle = document.getElementById("txt_vehicles").value;
                }
                else {
                    startdt = document.getElementById("txtFromDate1").value;
                    enddt = document.getElementById("txtToDate1").value;
                    checkedvehicle = document.getElementById("txt_vehicles1").value;
                }
                var data = { 'op': 'getdata_compare', 'startdt': startdt, 'enddt': enddt, 'checkedvehicle': checkedvehicle };
                var s = function (msg) {
                    if (msg) {
                        BindResults(msg);
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                    // $('#BookingDetails').html(x);
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

            var startodo = 0;
            function BindResults(logdata) {
                data = logdata;
                //                x = document.getElementById('example');
                //                $('#example').remove();
                //                var divgrid = document.getElementById("divlogsgrid");
                //                divgrid.innerHTML = '<table id="example" summary="Route Details" rules="groups" frame="hsides" border="2"  style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><caption>MODEL DAIRY GPS REPORT</caption> <tr><td><table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><caption>Route Map Summary</caption><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup> <tr style="border-bottom: 1px solid #d5d5d5; background-color: #d5d5d5; height: 30px;font-family: Arial; font-weight: normal; font-size: 12px;">    <th style="width: 150px;">TimeStamp </th> <th style="width: 400px;"> Address </th><th style="width: 100px;"> Speed </th> <th style="width: 100px;"> Status </th><th style="width: 100px;"> Odometer </th> <th style="width: 150px;"> Stopped Time </th></tr></table></td></tr><tr><td> <div id="divscroll"  style="width: 1050px; height: 123px; overflow: auto;"><table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><tr id="templateRow" style="display: none; cursor: pointer; border-bottom: 1px solid #d5d5d5;"  onclick="onrowclick(this);"><td style="width: 150px; text-align: center;border-bottom: 1px solid #d5d5d5;"></td><td style="width: 400px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td> <td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td><td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td><td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td> <td style="width: 150px; text-align: center;border-bottom: 1px solid #d5d5d5;"></td></tr></table></div></td></tr></table>';
                var startmarker = new google.maps.Marker({
                    position: new google.maps.LatLng(data[0].latitude, data[0].longitude),
                    map: map,
                    center: location,
                    zoom: 6
                });
                stoppedmarkers.push(startmarker);
                var Sinfowindow = new google.maps.InfoWindow({
                    content: "Start Point (" + data[0].vehicleno + ")"
                });

                google.maps.event.addListener(startmarker, 'click', function () {
                    Sinfowindow.open(map, startmarker);
                });
                var endmarker = new google.maps.Marker({
                    position: new google.maps.LatLng(data[data.length - 1].latitude, data[data.length - 1].longitude),
                    map: map,
                    center: location,
                    zoom: 6
                });
                stoppedmarkers.push(endmarker);
                var Einfowindow = new google.maps.InfoWindow({
                    content: "End Point (" + data[0].vehicleno + ")"
                });

                google.maps.event.addListener(endmarker, 'click', function () {
                    Einfowindow.open(map, endmarker);
                });

                var odometerfrom = data[0].odometer;
                startodo = odometerfrom;
                var odometerto = data[data.length - 1].odometer;
                var TotalDistance = odometerto - odometerfrom;
                if (vehiclecnt == 1) {
                    document.getElementById('lbl_veh1').innerHTML = data[0].vehicleno;
                    document.getElementById('lbl_veh1_kms').innerHTML = " : " + TotalDistance.toFixed(2) + "KMs ";
                }
                else {
                    document.getElementById('lbl_veh2').innerHTML = data[0].vehicleno;
                    document.getElementById('lbl_veh2_kms').innerHTML = " : " + TotalDistance.toFixed(2) + "KMs ";
                }
                var startpoint = new google.maps.LatLng(data[0].latitude, data[0].longitude);
                var endpoint = new google.maps.LatLng(data[data.length - 1].latitude, data[data.length - 1].longitude);
                if (vehiclecnt == 2) {
                    var latlngbounds = new google.maps.LatLngBounds();
                    latlngbounds.extend(startpoint);
                    latlngbounds.extend(endpoint);
                    map.fitBounds(latlngbounds);
                }
                for (var count = 0; count < data.length; count++) {
                    var vehicleno = data[count].vehicleno;
                    var Latitude = data[count].latitude;
                    var Longitude = data[count].longitude;
                    var speed = data[count].speed;
                    speed = Math.round(speed);
                    var odometer = data[count].odometer;
                    var timestamp = "";
                    var status = data[count].Status;
                    var datetime = data[count].datetime;
                    var point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                    var polylength = flightPlanCoordinates.length;
                    flightPlanCoordinates[polylength] = point;
                    polylength++;
                    var color = "#0000CD";
                    if (vehiclecnt == 2) {
                        color = "red";
                    }
                    flightPath = new google.maps.Polyline({
                        path: flightPlanCoordinates,
                        strokeColor: color,
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });
                    flightPath.setMap(map);
                    polilinepath.push(flightPath);
                }
                if (vehiclecnt == 1) {
                    vehiclecnt++;
                    previewroute();
                }
                else {
                    vehiclecnt = 1;
                }
            }
        }
        function roundToTwo(num) {
            return +(Math.round(num + "e+2") + "e-2");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 100%; height: 100%;">
        <div class="togglediv" id="divtoggle">
            <div class="inner">
                <img id="btnClose" alt="" src="Images/bigleft.png" title="Hide" style="float: right;
                    border: 1px solid #d5d5d5; width: 17px; height: 20px; background-color: #ffffff;" />
                <div style="width: 100%;">
                    <table style="position: absolute;">
                        <tr>
                            <td id="cell" valign="top" style="border: 1px solid #d5d5d5; background-color: #f4f4f4;
                                height: 650px;">
                                <table>
                                    <tr>
                                        <td>
                                            <span id="Span1" style="color: Blue; font-size: 15px; font-weight: bold;">Vehicle 1</span>
                                            <table style="width: 98%; padding: 5px; border: 1px solid #d5d5d5; border-radius: 5px;
                                                height: 50%;">
                                                <tr>
                                                    <td style="width: 80px; color: Black; font-weight: bold;">
                                                        From Date
                                                    </td>
                                                    <td>
                                                        <input id="txtFromDate" type="datetime-local" style="width: 200px; height: 22px;
                                                            font-size: 13px; padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="color: Black; font-weight: bold;">
                                                        To Date
                                                    </td>
                                                    <td>
                                                        <input id="txtToDate" type="datetime-local" style="width: 200px; height: 22px; font-size: 13px;
                                                            padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Vehicle
                                                    </td>
                                                    <td>
                                                        <input id="txt_vehicles" type="text" class="TextBox" style="padding: 5px; width: 200px;
                                                            font-size: 12px;" list="lst_vehicles" value="">
                                                        <datalist id="lst_vehicles">
                                                        </datalist>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span id="Span2" style="color: Red; font-size: 15px; font-weight: bold;">Vehicle 2</span>
                                            <table style="width: 98%; padding: 5px; border: 1px solid #d5d5d5; border-radius: 5px;
                                                height: 50%;">
                                                <tr>
                                                    <td style="width: 80px; color: Black; font-weight: bold;">
                                                        From Date
                                                    </td>
                                                    <td>
                                                        <input id="txtFromDate1" type="datetime-local" style="width: 200px; height: 22px;
                                                            font-size: 13px; padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="color: Black; font-weight: bold;">
                                                        To Date
                                                    </td>
                                                    <td>
                                                        <input id="txtToDate1" type="datetime-local" style="width: 200px; height: 22px; font-size: 13px;
                                                            padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Vehicle
                                                    </td>
                                                    <td>
                                                        <input id="txt_vehicles1" type="text" class="TextBox" style="padding: 5px; width: 200px;
                                                            font-size: 12px;" list="lst_vehicles1" value="">
                                                        <datalist id="lst_vehicles1">
                                                        </datalist>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input type="button" class="btntogglecls" value="Preview Route" id="btnprevroute"
                                                style="width: 100px; background-color: #d5d5d5; color: #000000; float: right;"
                                                onclick="previewroute();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <span id="lbl_veh1" style="color: Blue; font-size: 15px; font-weight: bold;"></span>
                                                    </td>
                                                    <td>
                                                        <span id="lbl_veh1_kms" style="color: Blue; font-size: 15px; font-weight: bold;">
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="lbl_veh2" style="color: Red; font-size: 15px; font-weight: bold;"></span>
                                                    </td>
                                                    <td>
                                                        <span id="lbl_veh2_kms" style="color: Red; font-size: 15px; font-weight: bold;">
                                                        </span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="mapcontent">
            <div id="googleMap" style="width: 100%; height: 100%; position: relative; background-color: rgb(229, 227, 223);">
            </div>
        </div>
    </div>
</asp:Content>
