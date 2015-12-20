<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RouteDrawing.aspx.cs" Inherits="RouteDrawing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" href="Images/TitleLogo.png" type="image/x-icon" title="trackmyasset.in" />
    <title>Track my asset</title>
    <%--<script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&libraries=geometry"></script>--%>
    <script src="https://maps.googleapis.com/maps/api/js?v=3&sensor=false&libraries=geometry"></script>
    <script src="js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="js/jquery-1.4.4.js" type="text/javascript"></script>
    <style type="text/css">
        .wrapper
        {
            overflow: hidden;
            top: 0px;
        }
        html, body
        {
            margin-top: 1px;
            padding-top: 1px;
            margin-left: 0px;
            padding-left: 0px;
            height: 99.5%; /* needed for container min-height */
            width: 100%;
            overflow: hidden;
            position: fixed;
            background: url(CSS/cream_dust.png) repeat left top;
        }
        .inner
        {
            width: 100%;
            position: absolute;
            left: 0px;
            bottom: 0px;
            right: 0px;
            color: Black;
            z-index: 99999;
            height: 225px;
            overflow: auto;
        }
        .togglediv
        {
            position: absolute;
            width: 99.8%;
            left: 0px;
            bottom: 0px;
            right: 0px;
            color: Black;
            z-index: 99999;
            border: 1px solid #d5d5d5;
            height: 196px;
            background-color: #ffffff;
            opacity: 0.9;
            cursor: pointer;
        }
        .allbtncls
        {
            display: inline-block;
            min-width: 54px;
            border: 1px solid #dcdcdc;
            border: 1px solid rgba(0,0,0,0.1);
            text-align: center;
            color: #444;
            font-size: 12px;
            font-weight: bold;
            height: 23px;
            padding: 0 8px;
            line-height: 27px;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            -webkit-transition: all 0.218s;
            -moz-transition: all 0.218s;
            -o-transition: all 0.218s;
            transition: all 0.218s;
            background-color: #f5f5f5;
            background-image: -webkit-gradient(linear,left top,left bottom,from(#f5f5f5),to(#f1f1f1));
            background-image: -webkit-linear-gradient(top,#f5f5f5,#f1f1f1);
            width: 10px;
            opacity: 1.9;
        }
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
            top: 0;
            left: 0px;
            top: 30px;
            overflow: hidden;
            position: absolute;
        }
        .datebuttonsleft
        {
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf) );
            cursor: pointer;
            margin: 0px;
            border-radius: 3px 0px 0px 0px;
            width: 75px;
            height: 26px;
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
            width: 75px;
            height: 26px;
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
    </style>
    <script type="text/javascript">
        var map;
        $(document).ready(function () {
            function initialize() {
                var latlng = new google.maps.LatLng(18.33, 73.55);
                var myOptions = {
                    zoom: 6,
                    center: latlng,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                map = new google.maps.Map(document.getElementById("googleMap"), myOptions);
                var locationControlDiv = document.createElement('div');
                var locationControl = new LocationsControl(locationControlDiv, map);
                map.controls[google.maps.ControlPosition.TOP_RIGHT].push(locationControlDiv);

                var StpdmarkerControlDiv = document.createElement('div');
                var StpdmarkerControl = new StoppedmarkerControl(StpdmarkerControlDiv, map);
                map.controls[google.maps.ControlPosition.TOP_RIGHT].push(StpdmarkerControlDiv);

                var drawtimeDiv = document.createElement('div');
                var drawtimeControl = new drawtime(drawtimeDiv, map);
                map.controls[google.maps.ControlPosition.TOP_RIGHT].push(drawtimeDiv);
            }
            google.maps.event.addDomListener(window, "load", initialize);
        });
    </script>
    <script type="text/javascript">
        var interval;
        var data = [];
        var point;
        var flightPlanCoordinates = new Array();
        var map;
        var x;
        var maxID = 0;
        var Locationsdata;
        var showlocations = true;
        var addedlocations = [];
        function load() {
            if ($('#btnShow').val() == 'Draw') {
                btnstopclick();
                btnclearclick();
                deletelocationOverlays();
                addedlocations = [];
                deleteStoppedmarkerOverlays();
                allstoppedmarkers = [];
                for (var i = 0; i < stoppedmarkers.length; i++) {
                    stoppedmarkers[i].setMap(null);
                }
                stoppedmarkers = [];
                prevbranch = "";
                imgcnt = 1;
                //                    var imageDir = 'VehicleTypes/';                         
                //                    var images = ['greenCar4.png', 'greenCar5.png', 'greenCar6.png', 'greenCar7.png', 'greenCar8.png', 'greenCar9.png', 'greenCar10.png', 'greenCar11.png', 'greenCar12.png', 'greenCar13.png', 'greenCar14.png', 'greenCar15.png', 'greenCar16.png', 'greenCar1.png', 'greenCar2.png', 'greenCar3.png'];

                x = document.getElementById('example');
                $(function () {
                    var Username = '<%= Session["field1"] %>';
                    var data = { 'op': 'ShowMyLocations',  'Type': 'NOTALL' };
                    var s = function (msg) {
                        if (msg) {
                            Bindlocations(msg);
                        }
                        else {
                        }
                    };
                    var e = function (x, h, e) {
                        // $('#BookingDetails').html(x);
                    };
                    callHandler(data, s, e);
                });
                $(function () {
                    var data = { 'op': 'getdata' };
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
                function Bindlocations(data) {
                    Locationsdata = data;
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
                function BindResults(logdata) {
                    data = logdata;
                    interval = setInterval(function () { liveupdate() }, 1000);
                }
                $('#btnShow').val('Pause');
            }
            else if ($('#btnShow').val() == 'Pause') {
                clearInterval(interval);
                $('#btnShow').val('Resume');
            }
            else if ($('#btnShow').val() == 'Resume') {
                clearInterval(interval);
                var speed = $("#speedval").val();
                if (speed == "speed") {
                    speed = 3;
                }
                if (speed == 1) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 1200);
                }
                else if (speed == 2) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 1000);
                }
                else if (speed == 3) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 700);
                }
                else if (speed == 4) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 500);
                }
                else if (speed == 5) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 200);
                }
                else if (speed == 6) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 50);
                }
                $('#btnShow').val('Pause');
            }
        }

        // Add a Locations control that returns the user to London
        function LocationsControl(locationcontrolDiv, map) {
            locationcontrolDiv.style.padding = '5px 0px 0px 0px';
            var locationcontrolUI = document.createElement('div');
            locationcontrolUI.style.backgroundColor = 'rgb(255, 255, 255)';
            locationcontrolUI.style.border = '1px solid rgb(113, 123, 135)';
            locationcontrolUI.style.cursor = 'pointer';
            locationcontrolUI.style.textAlign = 'center';
            locationcontrolUI.style.width = '110px';
            locationcontrolUI.title = 'Hide Locations';
            locationcontrolUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            locationcontrolDiv.appendChild(locationcontrolUI);
            var locationcontrolText = document.createElement('div');
            locationcontrolText.style.fontFamily = 'Arial,sans-serif';
            locationcontrolText.style.fontSize = '12px';
            locationcontrolText.style.paddingLeft = '4px';
            locationcontrolText.style.paddingRight = '4px';
            locationcontrolText.style.height = '20px';
            locationcontrolText.style.paddingTop = '1px';
            locationcontrolText.innerHTML = '<b>Hide Locations<b>'
            locationcontrolUI.appendChild(locationcontrolText);

            // Setup click-event listener: simply set the map to London
            google.maps.event.addDomListener(locationcontrolUI, 'click', function () {
                if (!showlocations) {
                    showlocations = true;
                    locationcontrolText.innerHTML = '<b>Hide Locations<b>'
                    locationcontrolUI.title = "Hide Locations";
                    if (Locationsdata.length > 0) {
                        for (var vehicledata in addedlocations) {
                            var myCenter = new google.maps.LatLng(addedlocations[vehicledata].latitude, addedlocations[vehicledata].longitude);
                            placeMarkerlocations(myCenter, addedlocations[vehicledata].BranchName, addedlocations[vehicledata].mrkrcnt, addedlocations[vehicledata].radius); 
                        }
                        $(this).val("Show Locations");
                    }
                    else {
                        var Username = '<%= Session["field1"] %>';
                        var data = { 'op': 'ShowMyLocations',  'Type': 'NOTALL' };
                        var s = function (msg) {
                            if (msg) {
                                Bindlocations(msg);
                            }
                            else {
                            }
                        };
                        var e = function (x, h, e) {
                            // $('#BookingDetails').html(x);
                        };
                        callHandler(data, s, e);
                        $(this).val("Show Locations");
                    }
                }
                else {
                    showlocations = false;
                    locationcontrolText.innerHTML = '<b>Show Locations<b>'
                    locationcontrolUI.title = "Hide Locations";
                    deletelocationOverlays();
                }
            });
        }
        var showStoppedmarkers = true;
        function StoppedmarkerControl(StoppedmarkercontrolDiv, map) {
            StoppedmarkercontrolDiv.style.padding = '5px 0px 0px 0px';
            var StoppedmarkercontrolUI = document.createElement('div');
            StoppedmarkercontrolUI.style.backgroundColor = 'rgb(255, 255, 255)';
            StoppedmarkercontrolUI.style.border = '1px solid rgb(113, 123, 135)';
            StoppedmarkercontrolUI.style.cursor = 'pointer';
            StoppedmarkercontrolUI.style.textAlign = 'center';
            StoppedmarkercontrolUI.style.width = '110px';
            StoppedmarkercontrolUI.title = 'Hide Stoppages';
            StoppedmarkercontrolUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            StoppedmarkercontrolDiv.appendChild(StoppedmarkercontrolUI);
            var StoppedmarkercontrolText = document.createElement('div');
            StoppedmarkercontrolText.style.fontFamily = 'Arial,sans-serif';
            StoppedmarkercontrolText.style.fontSize = '12px';
            StoppedmarkercontrolText.style.paddingLeft = '4px';
            StoppedmarkercontrolText.style.paddingRight = '4px';
            StoppedmarkercontrolText.style.height = '20px';
            StoppedmarkercontrolText.style.paddingTop = '1px';
            StoppedmarkercontrolText.innerHTML = '<b>Hide Stoppages<b>'
            StoppedmarkercontrolUI.appendChild(StoppedmarkercontrolText);

            // Setup click-event listener: simply set the map to London
            google.maps.event.addDomListener(StoppedmarkercontrolUI, 'click', function () {
                if (!showStoppedmarkers) {
                    showStoppedmarkers = true;
                    StoppedmarkercontrolText.innerHTML = '<b>Hide Stoppages<b>'
                    StoppedmarkercontrolUI.title = "Hide Stoppages";
                    for (var i = 0; i < allstoppedmarkers.length; i++) {
                        allstoppedmarkers[i].setMap(map);
                    }
                }
                else {
                    showStoppedmarkers = false;
                    StoppedmarkercontrolText.innerHTML = '<b>Show Stoppages<b>'
                    StoppedmarkercontrolUI.title = "Show Stoppages";
                    deleteStoppedmarkerOverlays();
                }
            });
        }
        function deleteStoppedmarkerOverlays() {
            StoppedmarkerclearOverlays();
//            allstoppedmarkers = [];
        }

        // Sets the map on all markers in the array.
        function StoppedmarkersetAllMap(map) {
            for (var i = 0; i < allstoppedmarkers.length; i++) {
                allstoppedmarkers[i].setMap(map);
            }
        }

        // Removes the overlays from the map, but keeps them in the array.
        function StoppedmarkerclearOverlays() {
            StoppedmarkersetAllMap(null);
        }

        function drawtime(controlDiv, map) {
            controlDiv.style.padding = '5px 1px 0px 0px';
            var controlUI = document.createElement('input');
            controlUI.type = "text";
            controlUI.id = "txtdrawtime";
            controlUI.style.backgroundColor = 'rgb(255, 255, 255)';
            controlUI.style.border = '1px solid rgb(113, 123, 135)';
            controlUI.style.paddingLeft = '4px';
            controlUI.style.width = '30px';
            controlUI.style.height = '20px';
            controlUI.style.fontFamily = 'Arial,sans-serif';
            controlUI.style.fontSize = '12px';
            controlUI.title = 'Enter timegap in Min.';
            controlDiv.appendChild(controlUI);
        }

        var LocationsArray = new Array();
        function placeMarkerlocations(location, BranchName, mrkrcnt, radius) {
            var marker = new google.maps.Marker({
                position: location,
                map: map,
                center: location,
                title: BranchName,
                icon: "Images/greenmarker.png",
                zoom: 6
            });
            LocationsArray.push(marker);

            var rrr = parseInt(radius, rrr);
            var circle = new google.maps.Circle({
                map: map,
                zoom: 16,
                radius: rrr,    // 10 miles in metres
                strokeColor: "#FFffff",
                fillColor: "#FF0000",
                fillOpacity: 0.35,
                strokeWeight: 1,
                strokeOpacity: 0
            });
            circle.bindTo('center', marker, 'position');
            LocationsArray.push(circle);

            var content = "Location : " + BranchName;
            var infowindow = new google.maps.InfoWindow({
                content: content
            });

            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });
        }
        // Deletes all markers in the array by removing references to them.
        function deletelocationOverlays() {
            locationclearOverlays();
            LocationsArray = [];
        }

        // Sets the map on all markers in the array.
        function locationsetAllMap(map) {
            for (var i = 0; i < LocationsArray.length; i++) {
                LocationsArray[i].setMap(map);
            }
        }

        // Removes the overlays from the map, but keeps them in the array.
        function locationclearOverlays() {
            locationsetAllMap(null);
        }
        var count = 0;
        var markersArray = [];
        var polilinepath = [];
        var firstlog = false;
        var stoppedmarkers = [];
        var allstoppedmarkers = [];
        var flightPath;
        var marker;
        var imgcnt = 1;
        var prevbranch = "";
        function liveupdate() {
            if (data.length > 0 && data.length > count) {
                var vehicleno = data[count].vehicleno;
                var Latitude = data[count].latitude;
                var Longitude = data[count].longitude;
                var angle = data[count].direction;
                var speed = data[count].speed;
                speed = Math.round(speed);
                var odometer = data[count].odometer;
                var status = data[count].Status;
                var datetime = data[count].datetime;
                point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                if (markersArray.length > 0) {
                    if (!map.getBounds().contains(point)) {
                        map.panTo(point);
                    }
                }
                if (typeof Locationsdata === "undefined") {
                }
                else {
                    for (var cont = 0; cont < Locationsdata.length; cont++) {
                        var targetLoc = new google.maps.LatLng(Latitude, Longitude);
                        var center = new google.maps.LatLng(Locationsdata[cont].latitude, Locationsdata[cont].longitude);
                        var radius = Locationsdata[cont].radius;
                        var isinside = pointInCircle(targetLoc, radius, center);
                        function pointInCircle(point, radius, center) {
                            return (google.maps.geometry.spherical.computeDistanceBetween(point, center) <= radius)
                        }
                        if (isinside) {
                            if (prevbranch != Locationsdata[cont].BranchName) {
                                // To add the marker to the map, call setMap();
                                if (showlocations) {
                                    var marker = new google.maps.Marker({
                                        position: point,
                                        map: map,
                                        center: location,
                                        title: Locationsdata[cont].BranchName,
                                        icon: "Images/greenmarker.png",
                                        zoom: 6
                                    });
                                    var content = "Location : " + Locationsdata[cont].BranchName;
                                    var infowindow = new google.maps.InfoWindow({
                                        content: content
                                    });

                                    google.maps.event.addListener(marker, 'click', function () {
                                        infowindow.open(map, marker);
                                    });
                                    LocationsArray.push(marker);

                                    var rrr = parseInt(Locationsdata[cont].radius, rrr);
                                    var circle = new google.maps.Circle({
                                        map: map,
                                        zoom: 16,
                                        radius: rrr,    // 10 miles in metres
                                        strokeColor: "#FFffff",
                                        fillColor: "#FF0000",
                                        fillOpacity: 0.35,
                                        strokeWeight: 1,
                                        strokeOpacity: 0
                                    });
                                    circle.bindTo('center', marker, 'position');
                                    LocationsArray.push(circle);

                                    prevbranch = Locationsdata[cont].BranchName;
                                }
                                addedlocations.push({ latitude: Locationsdata[cont].latitude, longitude: Locationsdata[cont].longitude, BranchName: Locationsdata[cont].BranchName, mrkrcnt: imgcnt, radius: Locationsdata[cont].radius });
                                imgcnt = imgcnt + 1;
                            }
                        }
                    }
                }
                var startmarker;
                var endmarker;
                if (firstlog == false) {
                    var latlngbounds = new google.maps.LatLngBounds();
                    latlngbounds.extend(point);
                    map.fitBounds(latlngbounds);
                    map.setZoom(13);

                    $('#example').remove();

                    var divgrid = document.getElementById("divlogsgrid");
                    divgrid.innerHTML = '<table id="example" summary="Route Details" rules="groups" frame="hsides" border="2"  style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><caption>MODEL DAIRY GPS REPORT</caption> <tr><td><table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><caption>Route Map Summary</caption><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup> <tr style="border-bottom: 1px solid #d5d5d5; background-color: #d5d5d5; height: 30px;font-family: Arial; font-weight: normal; font-size: 12px;">    <th style="width: 150px;">TimeStamp </th> <th style="width: 400px;"> Address </th><th style="width: 100px;"> Speed </th> <th style="width: 100px;"> Status </th><th style="width: 100px;"> Odometer </th> <th style="width: 150px;"> Stopped Time </th></tr></table></td></tr><tr><td> <div id="divscroll"  style="width: 1050px; height: 123px; overflow: auto;"><table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><tr id="templateRow" style="display: none; cursor: pointer; border-bottom: 1px solid #d5d5d5;"  onclick="onrowclick(this);"><td style="width: 150px; text-align: center;border-bottom: 1px solid #d5d5d5;"></td><td style="width: 400px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td> <td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td><td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td><td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td> <td style="width: 150px; text-align: center;border-bottom: 1px solid #d5d5d5;"></td></tr></table></div></td></tr></table>';

                    startmarker = new google.maps.Marker({
                        position: point,
                        map: map,
                        center: location,
                        zoom: 6
                    });
                    stoppedmarkers.push(startmarker);
                    var Sinfowindow = new google.maps.InfoWindow({
                        content: "Start Point"
                    });

                    google.maps.event.addListener(startmarker, 'click', function () {
                        Sinfowindow.open(map, startmarker);
                    });
                    endmarker = new google.maps.Marker({
                        position: new google.maps.LatLng(data[data.length - 1].latitude, data[data.length - 1].longitude),
                        map: map,
                        center: location,
                        zoom: 6
                    });
                    stoppedmarkers.push(endmarker);
                    var Einfowindow = new google.maps.InfoWindow({
                        content: "End Point"
                    });

                    google.maps.event.addListener(endmarker, 'click', function () {
                        Einfowindow.open(map, endmarker);
                    });

                    var odometerfrom = data[0].odometer;
                    startodo = odometerfrom;
                    var odometerto = data[data.length - 1].odometer;
                    var TotalDistance = odometerto - odometerfrom;

                    document.getElementById('lbltripinfo').innerHTML = "Report From : " + data[0].datetime + "  To : " + data[data.length - 1].datetime + " and  TotalDistance Travelled : " + Math.round(TotalDistance) + "KMs Vehicle : " + vehicleno + "";
                }
                firstlog = true;

                var iconsrc;
                var VehicleType = data[count].vehicletype;
                if (VehicleType == 'Escavator') {
                    iconsrc = "VehicleTypes/" + VehicleType + ".png";
                }
                else
                    if (VehicleType == "Roller") {
                        if (speed == 0) {
                            VehicleType = "red" + VehicleType;
                            iconsrc = "VehicleTypes/" + VehicleType + ".png";
                        }
                        else {
                            VehicleType = "green" + VehicleType;
                            iconsrc = "VehicleTypes/" + VehicleType + ".png";
                        }
                    }
                    else {
                        if (VehicleType == "") {
                            VehicleType = "Car";
                        }
                        if (speed == 0) {
                            VehicleType = "red" + VehicleType;
                            //                            iconsrc = "VehicleTypes/" + VehicleType + ".png";
                        }
                        else {
                            var imgangle = 0;
                            VehicleType = "green" + VehicleType;
                        }
                        if (angle >= 0 && angle < 22.5) {
                            imgangle = 0;
                            //                    document.getElementById("ImgRotate").style.MozTransform = "rotate(" + 22.5 + "deg)";
                            //                    iconsrc = document.getElementById("ImgRotate").src;
                            iconsrc = "VehicleTypes/" + VehicleType + "4.png";
                        }
                        else if (angle >= 22.5 && angle < 45) {
                            imgangle = 22.5;
                            //                    document.getElementById("ImgRotate").style.MozTransform = "rotate(" + 45 + "deg)";
                            //                    iconsrc = document.getElementById("ImgRotate").src;
                            iconsrc = "VehicleTypes/" + VehicleType + "5.png";
                        }
                        else if (angle >= 45 && angle < 67.5) {
                            imgangle = 45.5;
                            //                    document.getElementById("ImgRotate").style.MozTransform = "rotate(" + 67.5 + "deg)";
                            //                    iconsrc = document.getElementById("ImgRotate").src;
                            iconsrc = "VehicleTypes/" + VehicleType + "6.png";
                        }
                        else if (angle >= 67.5 && angle < 90) {
                            imgangle = 67.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "7.png";
                        }
                        else if (angle >= 90 && angle < 112.5) {
                            imgangle = 90.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "8.png";
                        }
                        else if (angle >= 112.5 && angle < 135) {
                            imgangle = 112.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "9.png";
                        }
                        else if (angle >= 135 && angle < 157.5) {
                            imgangle = 135.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "10.png";
                        }
                        else if (angle >= 157.5 && angle < 180) {
                            imgangle = 157.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "11.png";
                        }
                        else if (angle >= 180 && angle < 202.5) {
                            imgangle = 180.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "12.png";
                        }
                        else if (angle >= 202.5 && angle < 225) {
                            imgangle = 202.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "13.png";
                        }
                        else if (angle >= 225 && angle < 247.5) {
                            imgangle = 225.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "14.png";
                        }
                        else if (angle >= 247.5 && angle < 270) {
                            imgangle = 247.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "15.png";
                        }
                        else if (angle >= 270 && angle < 292.5) {
                            imgangle = 270.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "17.png";
                        }
                        else if (angle >= 292.5 && angle < 315) {
                            imgangle = 292.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "1.png";
                        }
                        else if (angle >= 315 && angle < 337.5) {
                            imgangle = 315.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "2.png";
                        }
                        else if (angle >= 337.5 && angle < 360) {
                            imgangle = 337.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "3.png";
                        }
                        else if (angle >= 360) {
                            imgangle = 360;
                            iconsrc = "VehicleTypes/" + VehicleType + "3.png";
                        }
                        //                        }
                    }

                deleteOverlays();
                var image = new google.maps.MarkerImage(iconsrc,
                // This marker is 20 pixels wide by 32 pixels tall.
        null,
                // The origin for this image is 0,0.
        new google.maps.Point(0, 0),
                // The anchor for this image is the base of the flagpole at 0,32.
        new google.maps.Point(20, 20)
    );
                marker = new google.maps.Marker({
                    position: point,
                    map: map,
                    center: location,
                    zoom: 6,
                    icon: image
                });

                var timestamp = "";

                if (speed == 0) {
                    if (count < data.length) {

                        var _MS_PER_DAY = 86400000;
                        var _MS_PER_aaa = 3600000;
                        var _MS_PER_sss = 60000;
                        var _MS_PER_ddd = 1000;
                        var spantime = 0;


                        while (count < data.length && data[count].speed == 0) {
                            var prestime;
                            if (count < 1) {
                                prestime = data[count].datetime;
                            }
                            else {
                                prestime = data[count].datetime;
                            }
                            var upcmngdatetime = data[count+1].datetime;
                            var date = prestime.split(" ")[0];
                            var time = prestime.split(" ")[1];
                            var AMPM = prestime.split(" ")[2];

                            var datevalues = new Array();
                            var timevalues = new Array();
                            if (date.indexOf("-") != -1) {
                                datevalues = date.split('-');
                            }
                            else if (date.indexOf("/") != -1) {
                                datevalues = date.split('/');
                            }
                            timevalues = time.split(':');
                            var exacthours = timevalues[0];
                            if (AMPM == "PM") {
                                exacthours = parseInt(exacthours);
                                if (exacthours != "12") {
                                    exacthours = 12 + exacthours;
                                }
                            }
                            var updatetime = new Date(datevalues[2], datevalues[0] - 1, datevalues[1], exacthours, timevalues[1], timevalues[2]);

                            var upcmngdate = upcmngdatetime.split(" ")[0];
                            var upcmngtime = upcmngdatetime.split(" ")[1];
                            var upcmngAMPM = upcmngdatetime.split(" ")[2];

                            var upcmngdatevalues = new Array();
                            var upcmngtimevalues = new Array();
                            if (upcmngdate.indexOf("-") != -1) {
                                upcmngdatevalues = upcmngdate.split('-');
                            }
                            else if (upcmngdate.indexOf("/") != -1) {
                                upcmngdatevalues = upcmngdate.split('/');
                            }
                            upcmngtimevalues = upcmngtime.split(':');
                            var upcmngexacthours = upcmngtimevalues[0];
                            if (upcmngAMPM == "PM") {
                                upcmngexacthours = parseInt(upcmngexacthours);
                                if (upcmngexacthours != "12") {
                                    upcmngexacthours = 12 + upcmngexacthours;
                                }
                            }
                            var upcmngupdatetime = new Date(upcmngdatevalues[2], upcmngdatevalues[0] - 1, upcmngdatevalues[1], upcmngexacthours, upcmngtimevalues[1], upcmngtimevalues[2]);
                            //                            upcmngupdatetime = upcmngupdatetime.toString('dd-MM-yyyy HH:mm:ss');

                            spantime += (upcmngupdatetime - updatetime);
                            count++;
                        }
                        count--;
                        var days = Math.floor((spantime) / _MS_PER_DAY);
                        var hours = Math.floor((spantime) / _MS_PER_aaa);
                        if (hours > 24) {
                            hours = hours % 24;
                        }
                        var min = Math.floor((spantime) / _MS_PER_sss);
                        if (min > 60) {
                            min = min % 60;
                        }
                        var sec = Math.floor((spantime) / _MS_PER_ddd);
                        if (sec > 60) {
                            sec = sec % 60;
                        }
                      //  data[data.length - 1].latitude, data[data.length - 1].longitude
                        var mylocationurl = "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;'  onclick='addlocations(" + Latitude + "," + Longitude + ")'>Add to My Locations</a>";
                        if (days >= 1) {
                            timestamp = "Stopped for " + days + " days " + hours + " Hrs " + min + " Min" + mylocationurl;
                        }
                        else if (hours >= 1) {
                            timestamp = "Stopped for " + hours + " Hrs " + min + " Min" + mylocationurl;
                        }
                        else if (min >= 1) {
                            timestamp = "Stopped for " + min + " Min " + sec + " Sec" + mylocationurl;
                        }
                        else if (days < 1 && min > 1) {
                            timestamp = "Stopped for " + hours + " Hrs " + min + " Min" + mylocationurl;
                        }
                        markersArray.push(marker);
                        if (timestamp != null && timestamp != "" && showStoppedmarkers == true) {
                            var timevalue = document.getElementById('txtdrawtime');
                            if (timevalue) {
                                timevalue = document.getElementById('txtdrawtime').value;
                                if (timevalue != "" && timevalue != "0") {
                                    var inputspan = 0;
                                    inputspan = Math.floor((timevalue) * _MS_PER_sss);
                                    if (spantime > inputspan) {
                                        var stoppedmarker = new google.maps.Marker({
                                            position: point,
                                            map: map,
                                            title: timestamp,
                                            icon: "Images/ssmarker.png",
                                            zoom: 6
                                        });
                                        var content = timestamp;
                                        var infowindow = new google.maps.InfoWindow({
                                            content: content
                                        });

                                        google.maps.event.addListener(stoppedmarker, 'click', function () {
                                            infowindow.open(map, stoppedmarker);
                                        });
                                        allstoppedmarkers.push(stoppedmarker);
                                    }
                                }
                                else {
                                    var stoppedmarker = new google.maps.Marker({
                                        position: point,
                                        map: map,
                                        title: timestamp,
                                        icon: "Images/ssmarker.png",
                                        zoom: 6
                                    });
                                    var content = timestamp;
                                    var infowindow = new google.maps.InfoWindow({
                                        content: content
                                    });

                                    google.maps.event.addListener(stoppedmarker, 'click', function () {
                                        infowindow.open(map, stoppedmarker);
                                    });
                                    allstoppedmarkers.push(stoppedmarker);
                                }
                            }
                        }
                    }
                    else {
                        $('#btnShow').val('Draw');
                        markersArray.push(marker);
                    }
                }
                else {
                    markersArray.push(marker);
                }

                function getTemplateRow() {
                    var x = document.getElementById("templateRow").cloneNode(true);
                    x.id = Latitude + "_" + Longitude + "_" + count;
                    x.style.display = "";
                    //    x.innerHTML = x.innerHTML.replace(/{id}/, ++maxID);
                    x.cells[0].innerHTML = datetime;
                    x.cells[1].innerHTML = address;
                    x.cells[2].innerHTML = speed + " Kms/Hr ";
                    x.cells[3].innerHTML = status;
                    odometer = parseFloat(odometer);
                    startodo = parseFloat(startodo);
                    var diff = odometer - startodo;
                    diff = roundToTwo(diff)
                    x.cells[4].innerHTML = diff;
                    x.cells[5].innerHTML = timestamp;
                    if (status == "Stopped") {
                        x.style.backgroundColor = "#ffcccc";
                    }
                    return x;
                }
                $("#divscroll").scrollTop($("#divscroll").get(0).scrollHeight);
                var polylength = flightPlanCoordinates.length;
                flightPlanCoordinates[polylength] = point;
                polylength++;
                flightPath = new google.maps.Polyline({
                    path: flightPlanCoordinates,
                    strokeColor: "#0000CD",
                    strokeOpacity: 1.0,
                    strokeWeight: 2
                });
                //                var encodeString = google.maps.geometry.encoding.encodePath(flightPath);
                flightPath.setMap(map);

                polilinepath.push(flightPath);

                var address;
                geocoder = new google.maps.Geocoder();
                address = geocoder.geocode({ 'latLng': point }, function (results, addstatus) {
                    if (addstatus == google.maps.GeocoderStatus.OK) {
                        if (results.length > 0) {
                            if (results[0]) {
                                address = results[0].formatted_address;
                                var t = document.getElementById("example");
                                var rows = t.getElementsByTagName("tr");

                                var r = rows[rows.length - 1];
                                r.parentNode.insertBefore(getTemplateRow(), r);

                                var content = "<div>Time : " + datetime + "<br/>" + "Speed : " + speed + "<br/>" + "Address : " + address + "<br/>" + "Status : " + status + "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;'  onclick='addlocations(" + Latitude + "," + Longitude + ")'>Add to My Locations</a>";
                                createInfoWindow(flightPath, content);
                                function createInfoWindow(poly, content) {
                                    google.maps.event.addListener(poly, 'click', function (event) {
                                        infoWindow.content = content;
                                        infoWindow.position = event.latLng;
                                        infoWindow.open(map);
                                    });
                                }
                            }
                            else {
                                address = "No results";
                                var t = document.getElementById("example");
                                var rows = t.getElementsByTagName("tr");

                                var r = rows[rows.length - 1];
                                r.parentNode.insertBefore(getTemplateRow(), r);

                                var content = "<div>Time : " + datetime + "<br/>" + "Speed : " + speed + "<br/>" + "Address : " + address + "<br/>" + "Status : " + status + "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;'  onclick='addlocations(" + Latitude + "," + Longitude + ")'>Add to My Locations</a>";
                                createInfoWindow(flightPath, content);
                                function createInfoWindow(poly, content) {
                                    google.maps.event.addListener(poly, 'click', function (event) {
                                        infoWindow.content = content;
                                        infoWindow.position = event.latLng;
                                        infoWindow.open(map);
                                    });
                                }
                            }
                        }
                        else {
                            var t = document.getElementById("example");
                            var rows = t.getElementsByTagName("tr");

                            var r = rows[rows.length - 1];
                            r.parentNode.insertBefore(getTemplateRow(), r);

                            var content = "<div>Time : " + datetime + "<br/>" + "Speed : " + speed + "<br/>" + "Address : " + address + "<br/>" + "Status : " + status + "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;'  onclick='addlocations(" + Latitude + "," + Longitude + ")'>Add to My Locations</a>";
                            createInfoWindow(flightPath, content);
                            function createInfoWindow(poly, content) {
                                google.maps.event.addListener(poly, 'click', function (event) {
                                    infoWindow.content = content;
                                    infoWindow.position = event.latLng;
                                    infoWindow.open(map);
                                });
                            }
                        }
                    }
                    else {
                        var t = document.getElementById("example");
                        var rows = t.getElementsByTagName("tr");

                        var r = rows[rows.length - 1];
                        r.parentNode.insertBefore(getTemplateRow(), r);

                        var content = "<div>Time : " + datetime + "<br/>" + "Speed : " + speed + "<br/>" + "Address : " + address + "<br/>" + "Status : " + status + "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;'  onclick='addlocations(" + Latitude + "," + Longitude + ")'>Add to My Locations</a>";
                        createInfoWindow(flightPath, content);
                        function createInfoWindow(poly, content) {
                            google.maps.event.addListener(poly, 'click', function (event) {
                                infoWindow.content = content;
                                infoWindow.position = event.latLng;
                                infoWindow.open(map);
                            });
                        }
                    }
                });
                count++;
            }
            else {
                $('#btnShow').val('Draw');
                clearInterval(interval);
                if (data.length > 0) {
                    var startpoint = new google.maps.LatLng(data[0].latitude, data[0].longitude);
                    var endpoint = new google.maps.LatLng(data[data.length - 1].latitude, data[data.length - 1].longitude);
                    var latlngbounds = new google.maps.LatLngBounds();
                    latlngbounds.extend(startpoint);
                    latlngbounds.extend(endpoint);
                    map.fitBounds(latlngbounds);
                }
            }

            function deleteOverlays() {
                clearOverlays();
                markersArray = [];
                polilinepath = [];
                if (flightPath) {
                    flightPath.setMap(null);
                }
            }

            // Sets the map on all markers in the array.
            function setAllMap(map) {
                for (var i = 0; i < markersArray.length; i++) {
                    markersArray[i].setMap(map);
                }

                for (i = 0; i < polilinepath.length; i++) {
                    polilinepath[i].setMap(map); //or line[i].setVisible(false);
                }
            }

            // Removes the overlays from the map, but keeps them in the array.
            function clearOverlays() {
                setAllMap(null);
            }

        }

        function addlocations(lat, lng) {
            var mytext = lat + "," + lng;
            window.open('Mylocation.aspx?lat=' + lat + "&long=" + lng);
        }

        function speedrange(speed) {
            if (speed == 1) {
                clearInterval(interval);
                interval = setInterval(function () { liveupdate() }, 1200);
            }
            else if (speed == 2) {
                clearInterval(interval);
                interval = setInterval(function () { liveupdate() }, 1000);
            }
            else if (speed == 3) {
                clearInterval(interval);
                interval = setInterval(function () { liveupdate() }, 700);
            }
            else if (speed == 4) {
                clearInterval(interval);
                interval = setInterval(function () { liveupdate() }, 500);
            }
            else if (speed == 5) {
                clearInterval(interval);
                interval = setInterval(function () { liveupdate() }, 200);
            }
            else if (speed == 6) {
                clearInterval(interval);
                interval = setInterval(function () { liveupdate() }, 50);
            }
        }

        function btnstopclick() {
            clearInterval(interval);
            $("#speedval").val("speed");
            //            deletestoppedOverlays();
            firstlog = false;
            $('#btnShow').val('Draw');
            count = 0;
        }

        function btnclearclick() {
            deleteallOverlays();
            function deleteallOverlays() {
                clearallOverlays();
                markersArray = [];
                polilinepath = [];
                if (flightPath) {
                    flightPath.setMap(null);
                }
                flightPlanCoordinates = [];
            }

            // Sets the map on all markers in the array.
            function allsetAllMap(map) {
                for (var i = 0; i < markersArray.length; i++) {
                    markersArray[i].setMap(map);
                }
               
                for (i = 0; i < polilinepath.length; i++) {
                    polilinepath[i].setMap(map); //or line[i].setVisible(false);
                }
            }

            // Removes the overlays from the map, but keeps them in the array.
            function clearallOverlays() {
                allsetAllMap(null);
            }
        }

        var hidden = false;
        function clicked() {
            if (hidden) {
                $(".togglediv").stop().animate({ bottom: 0 }, 500);
                hidden = false;
                $("#btnshowhide").val("Hide");
            }
            else {
                $("#btnshowhide").val("Show");
                $(".togglediv").css('margin-left', 0);
                $(".togglediv").css('margin-right', 0);
                $(".togglediv").animate({ bottom: '-196px' }, 500);
                hidden = true;
            }
        }
    </script>
    <script type="text/javascript">
        var infoWindow = new google.maps.InfoWindow();
        function onrowclick(idval) {
            var positionarray = idval.id.split('_');
            //            var mainpowericon = vehicle.closest('tr').find('#imgpower');
            var table = $('#example');
            var tr = $('#example').find('tr');
            var timestamp;
            var speed;
            var vehstatus;
            var stoppedtime;
            var rowCellValue;

            for (var i = 3; i < tr.length; i++) {
                var rowid = tr[i].id;
                if (rowid == idval.id) {
                    timestamp = tr[i].cells[0].innerHTML;
                    speed = tr[i].cells[2].innerHTML;
                    vehstatus = tr[i].cells[3].innerHTML;
                    stoppedtime = tr[i].cells[5].innerHTML;
                    stoppedtime = stoppedtime.replace('Add to My Locations', '');
                }
            }
            var point = new google.maps.LatLng(positionarray[0], positionarray[1]);
            map.panTo(point);

            var address;
            geocoder = new google.maps.Geocoder();
            address = geocoder.geocode({ 'latLng': point }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        address = results[0].formatted_address;
                        infoWindow.close();
                        infoWindow.setOptions({
                            content: "<div>Time : " + timestamp + "<br/>" + "Speed : " + speed + "<br/>" + "Address : " + address + "<br/>" + "Status : " + vehstatus + "<br/>" + "Stopped Time : " + stoppedtime + "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;' onclick='addlocations(" + positionarray[0] + "," + positionarray[1] + ")'>Add to My Locations</a>",
                            position: point
                        });
                        infoWindow.open(map);
                    }
                    else {
                        address = "No results";
                        infoWindow.close();
                        infoWindow.setOptions({
                            content: "<div>Time : " + timestamp + "<br/>" + "Speed : " + speed + "<br/>" + "Address : " + address + "<br/>" + "Status : " + vehstatus + "<br/>" + "Stopped Time : " + stoppedtime + "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;' onclick='addlocations(" + positionarray[0] + "," + positionarray[1] + ")'>Add to My Locations</a>",
                            position: point
                        });
                        infoWindow.open(map);
                    }
                }
            });
        }

        function PrevValidating() {
            var speed = $("#speedval").val();
            if (speed == "speed") {
                speed = 3;
            }
            if (speed > 1) {
                speed--;
            }
            $("#speedval").val(speed);
            if ($('#btnShow').val() == 'Pause') {

                if (speed == 1) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 1200);
                }
                else if (speed == 2) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 1000);
                }
                else if (speed == 3) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 700);
                }
                else if (speed == 4) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 500);
                }
                else if (speed == 5) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 200);
                }
                else if (speed == 6) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 50);
                }
            }
            return false;
        }

        function NextValidating() {
            var speed = $("#speedval").val();
            if (speed == "speed") {
                speed = 3;
            }
            if (speed < 6) {
                speed++;
            }
            $("#speedval").val(speed);
            if ($('#btnShow').val() == 'Pause') {
                if (speed == 1) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 1200);
                }
                else if (speed == 2) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 1000);
                }
                else if (speed == 3) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 700);
                }
                else if (speed == 4) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 500);
                }
                else if (speed == 5) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 200);
                }
                else if (speed == 6) {
                    clearInterval(interval);
                    interval = setInterval(function () { liveupdate() }, 50);
                }
            }
            return false;
        }
    </script>
    <script type="text/javascript">
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
    <script type="text/javascript">
        function previewroute() {
            btnstopclick();
            btnclearclick();
            deletelocationOverlays();
            addedlocations = [];
            deleteStoppedmarkerOverlays();
            allstoppedmarkers = [];
            for (var i = 0; i < stoppedmarkers.length; i++) {
                stoppedmarkers[i].setMap(null);
            }
            stoppedmarkers = [];
            prevbranch = "";
            imgcnt = 1;

            x = document.getElementById('example');
            $(function () {
                var Username = '<%= Session["field1"] %>';
                var data = { 'op': 'ShowMyLocations',  'Type': 'NOTALL' };
                var s = function (msg) {
                    if (msg) {
                        Bindlocations(msg);
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                    // $('#BookingDetails').html(x);
                };
                callHandler(data, s, e);
            });
           
            function Bindlocations(data) {
                Locationsdata = data;
            }

            clearInterval(interval);
            $("#speedval").val("speed");
            //            deletestoppedOverlays();
            firstlog = false;
            $('#btnShow').val('Draw');
            count = 0;
            deleteallOverlays();
            function deleteallOverlays() {
                clearallOverlays();
                markersArray = [];
                stoppedmarkers = [];
                polilinepath = [];
                if (flightPath) {
                    flightPath.setMap(null);
                }
                flightPlanCoordinates = [];
            }

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
                var data = { 'op': 'getdata' };
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
                x = document.getElementById('example');

                $('#example').remove();

                var divgrid = document.getElementById("divlogsgrid");
                divgrid.innerHTML = '<table id="example" summary="Route Details" rules="groups" frame="hsides" border="2"  style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><caption>MODEL DAIRY GPS REPORT</caption> <tr><td><table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><caption>Route Map Summary</caption><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup><colgroup align="center"></colgroup> <tr style="border-bottom: 1px solid #d5d5d5; background-color: #d5d5d5; height: 30px;font-family: Arial; font-weight: normal; font-size: 12px;">    <th style="width: 150px;">TimeStamp </th> <th style="width: 400px;"> Address </th><th style="width: 100px;"> Speed </th> <th style="width: 100px;"> Status </th><th style="width: 100px;"> Odometer </th> <th style="width: 150px;"> Stopped Time </th></tr></table></td></tr><tr><td> <div id="divscroll"  style="width: 1050px; height: 123px; overflow: auto;"><table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;font-family:Arial; font-weight:normal;font-size:12px;" cellpadding="2px" cellspacing="0px"><tr id="templateRow" style="display: none; cursor: pointer; border-bottom: 1px solid #d5d5d5;"  onclick="onrowclick(this);"><td style="width: 150px; text-align: center;border-bottom: 1px solid #d5d5d5;"></td><td style="width: 400px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td> <td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td><td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td><td style="width: 100px; text-align: center;border-bottom: 1px solid #d5d5d5;"> </td> <td style="width: 150px; text-align: center;border-bottom: 1px solid #d5d5d5;"></td></tr></table></div></td></tr></table>';
                var startmarker = new google.maps.Marker({
                    position: new google.maps.LatLng(data[0].latitude, data[0].longitude),
                    map: map,
                    center: location,
                    zoom: 6
                });
                stoppedmarkers.push(startmarker);
                var Sinfowindow = new google.maps.InfoWindow({
                    content: "Start Point"
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
                    content: "End Point"
                });

                google.maps.event.addListener(endmarker, 'click', function () {
                    Einfowindow.open(map, endmarker);
                });

                var odometerfrom = data[0].odometer;
                startodo = odometerfrom;
                var odometerto = data[data.length - 1].odometer;
                var TotalDistance = odometerto - odometerfrom;

                document.getElementById('lbltripinfo').innerHTML = "Report From : " + data[0].datetime + "  To : " + data[data.length - 1].datetime + " and  TotalDistance Travelled : " + Math.round(TotalDistance) + "KMs ";

                var startpoint = new google.maps.LatLng(data[0].latitude, data[0].longitude);
                var endpoint = new google.maps.LatLng(data[data.length - 1].latitude, data[data.length - 1].longitude);
                var latlngbounds = new google.maps.LatLngBounds();
                latlngbounds.extend(startpoint);
                latlngbounds.extend(endpoint);
                map.fitBounds(latlngbounds);
                for (var count = 0; count < data.length; count++) {
                    var Latitude = data[count].latitude;
                    var Longitude = data[count].longitude;
                    var speed = data[count].speed;
                    speed = Math.round(speed);
                    var odometer = data[count].odometer;
                    var timestamp = "";
                    point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                    if (typeof Locationsdata === "undefined") {
                    }
                    else {
                        for (var cont = 0; cont < Locationsdata.length; cont++) {
                            var targetLoc = new google.maps.LatLng(Latitude, Longitude);
                            var center = new google.maps.LatLng(Locationsdata[cont].latitude, Locationsdata[cont].longitude);
                            var radius = Locationsdata[cont].radius;
                            var isinside = pointInCircle(targetLoc, radius, center);
                            function pointInCircle(point, radius, center) {
                                return (google.maps.geometry.spherical.computeDistanceBetween(point, center) <= radius)
                            }
                            if (isinside) {
                                if (prevbranch != Locationsdata[cont].BranchName) {
                                    // To add the marker to the map, call setMap();
                                    if (showlocations) {
                                        var marker = new google.maps.Marker({
                                            position: point,
                                            map: map,
                                            center: location,
                                            title: Locationsdata[cont].BranchName,
                                            icon: "Images/greenmarker.png",
                                            zoom: 6
                                        });
//                                        var content = "Location : " + Locationsdata[cont].BranchName;
//                                        var infowindow = new google.maps.InfoWindow({
//                                            content: content
//                                        });

//                                        google.maps.event.addListener(marker, 'click', function () {
//                                            infowindow.open(map, marker);
//                                        });
                                        LocationsArray.push(marker);

                                        var rrr = parseInt(Locationsdata[cont].radius, rrr);
                                        var circle = new google.maps.Circle({
                                            map: map,
                                            zoom: 16,
                                            radius: rrr,    // 10 miles in metres
                                            strokeColor: "#FFffff",
                                            fillColor: "#FF0000",
                                            fillOpacity: 0.35,
                                            strokeWeight: 1,
                                            strokeOpacity: 0
                                        });
                                        circle.bindTo('center', marker, 'position');
                                        LocationsArray.push(circle);

                                        prevbranch = Locationsdata[cont].BranchName;
                                    }
                                    addedlocations.push({ latitude: Locationsdata[cont].latitude, longitude: Locationsdata[cont].longitude, BranchName: Locationsdata[cont].BranchName, mrkrcnt: imgcnt, radius: Locationsdata[cont].radius });
                                    imgcnt = imgcnt + 1;
                                }
                            }
                        }
                    }
                    if (speed == 0) {
                        if (count < data.length) {

                            var _MS_PER_DAY = 86400000;
                            var _MS_PER_aaa = 3600000;
                            var _MS_PER_sss = 60000;
                            var _MS_PER_ddd = 1000;
                            var spantime = 0;


                             while (count < data.length && data[count].speed == 0){
                                var prestime;
                                if (count < 1) {
                                    prestime = data[count].datetime;
                                }
                                else {
                                    prestime = data[count].datetime;
                                }
                                var upcmngdatetime = data[count+1].datetime;
                                var date = prestime.split(" ")[0];
                                var time = prestime.split(" ")[1];
                                var AMPM = prestime.split(" ")[2];

                                var datevalues = new Array();
                                var timevalues = new Array();
                                if (date.indexOf("-") != -1) {
                                    datevalues = date.split('-');
                                }
                                else if (date.indexOf("/") != -1) {
                                    datevalues = date.split('/');
                                }
                                timevalues = time.split(':');
                                var exacthours = timevalues[0];
                                if (AMPM == "PM") {
                                    exacthours = parseInt(exacthours);
                                    if (exacthours != "12") {
                                        exacthours = 12 + exacthours;
                                    }
                                }
                                var updatetime = new Date(datevalues[2], datevalues[0] - 1, datevalues[1], exacthours, timevalues[1], timevalues[2]);

                                var upcmngdate = upcmngdatetime.split(" ")[0];
                                var upcmngtime = upcmngdatetime.split(" ")[1];
                                var upcmngAMPM = upcmngdatetime.split(" ")[2];

                                var upcmngdatevalues = new Array();
                                var upcmngtimevalues = new Array();
                                if (upcmngdate.indexOf("-") != -1) {
                                    upcmngdatevalues = upcmngdate.split('-');
                                }
                                else if (upcmngdate.indexOf("/") != -1) {
                                    upcmngdatevalues = upcmngdate.split('/');
                                }
                                upcmngtimevalues = upcmngtime.split(':');
                                var upcmngexacthours = upcmngtimevalues[0];
                                if (upcmngAMPM == "PM") {
                                    upcmngexacthours = parseInt(upcmngexacthours);
                                    if (upcmngexacthours != "12") {
                                        upcmngexacthours = 12 + upcmngexacthours;
                                    }
                                }
                                var upcmngupdatetime = new Date(upcmngdatevalues[2], upcmngdatevalues[0] - 1, upcmngdatevalues[1], upcmngexacthours, upcmngtimevalues[1], upcmngtimevalues[2]);
                                //                            upcmngupdatetime = upcmngupdatetime.toString('dd-MM-yyyy HH:mm:ss');

                                spantime += (upcmngupdatetime - updatetime);
                                count++;
                            }
                            count--;
                            var days = Math.floor((spantime) / _MS_PER_DAY);
                            var hours = Math.floor((spantime) / _MS_PER_aaa);
                            if (hours > 24) {
                                hours = hours % 24;
                            }
                            var min = Math.floor((spantime) / _MS_PER_sss);
                            if (min > 60) {
                                min = min % 60;
                            }
                            var sec = Math.floor((spantime) / _MS_PER_ddd);
                            if (sec > 60) {
                                sec = sec % 60;
                            }

                            var mylocationurl = "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;'  onclick='addlocations(" + Latitude + "," + Longitude + ")'>Add to My Locations</a>";

                            if (days >= 1) {
                                timestamp = "Stopped for " + days + " days " + hours + " Hrs " + min + " Min" + mylocationurl;
                            }
                            else if (hours >= 1) {
                                timestamp = "Stopped for " + hours + " Hrs " + min + " Min" + mylocationurl;
                            }
                            else if (min >= 1) {
                                timestamp = "Stopped for " + min + " Min " + sec + " Sec" + mylocationurl;
                            }
                            else if (days < 1 && min > 1) {
                                timestamp = "Stopped for " + hours + " Hrs " + min + " Min" + mylocationurl;
                            }
                            if (timestamp != null && timestamp != "" && showStoppedmarkers == true) {
                                var timevalue = document.getElementById('txtdrawtime');
                                if (timevalue) {
                                    timevalue = document.getElementById('txtdrawtime').value;
                                    if (timevalue != "" && timevalue != "0") {
                                        var inputspan = 0;
                                        inputspan = Math.floor((timevalue) * _MS_PER_sss);
                                        if (spantime > inputspan) {
                                            var stoppedmarker = new google.maps.Marker({
                                                position: point,
                                                map: map,
                                                title: timestamp,
                                                icon: "Images/ssmarker.png",
                                                zoom: 6
                                            });
//                                            var content = timestamp;
//                                            var infowindow = new google.maps.InfoWindow({
//                                                content: content
//                                            });

//                                            google.maps.event.addListener(stoppedmarker, 'click', function () {
//                                                infowindow.open(map, stoppedmarker);
//                                            });
                                            allstoppedmarkers.push(stoppedmarker);
                                        }
                                    }
                                    else {
                                        var stoppedmarker = new google.maps.Marker({
                                            position: point,
                                            map: map,
                                            title: timestamp,
                                            icon: "Images/ssmarker.png",
                                            zoom: 6
                                        });
//                                        var content = timestamp;
//                                        var infowindow = new google.maps.InfoWindow({
//                                            content: content
//                                        });

//                                        google.maps.event.addListener(stoppedmarker, 'click', function () {
//                                            infowindow.open(map, stoppedmarker);
//                                        });
                                        allstoppedmarkers.push(stoppedmarker);
                                    }
                                }
                            }
                        }
                    }
                    var status = data[count].Status;
                    var datetime = data[count].datetime;
                    var point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                    var polylength = flightPlanCoordinates.length;
                    flightPlanCoordinates[polylength] = point;
                    polylength++;
                    flightPath = new google.maps.Polyline({
                        path: flightPlanCoordinates,
                        strokeColor: "#0000CD",
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });
                    //                var encodeString = google.maps.geometry.encoding.encodePath(flightPath);
                    flightPath.setMap(map);

                    polilinepath.push(flightPath);


                    var usertype = '<%=Session["UserType"]%>';
                    var address = "Click here to get location.";
                    var content = "<div>Time : " + datetime + "<br/>" + "Speed : " + speed + "<br/>" + "Address : " + address + "<br/>" + "Status : " + status + "<br/><a style='text-decoration: underline;color:blue; cursor:pointer;'";

                    var t = document.getElementById("example");
                    var rows = t.getElementsByTagName("tr");

                    var r = rows[rows.length - 1];
                    r.parentNode.insertBefore(getTemplateRow(), r);
                   
                    createInfoWindow(flightPath, content);
                    function createInfoWindow(poly, content) {
                        google.maps.event.addListener(poly, 'click', function (event) {
                            infoWindow.content = content;
                            infoWindow.position = event.latLng;
                            infoWindow.open(map);
                        });
                    }

                    function getTemplateRow() {
                        var x = document.getElementById("templateRow").cloneNode(true);
                        x.id = Latitude + "_" + Longitude + "_" + count;
                        x.style.display = "";
                        //    x.innerHTML = x.innerHTML.replace(/{id}/, ++maxID);
                        x.cells[0].innerHTML = datetime;
                        x.cells[1].innerHTML = address;
                        x.cells[2].innerHTML = speed + " Kms/Hr ";
                        x.cells[3].innerHTML = status;
                        odometer = parseFloat(odometer);
                        startodo = parseFloat(startodo);
                        var diff = odometer - startodo;
                        diff = roundToTwo(diff)
                        x.cells[4].innerHTML = diff;
                        x.cells[5].innerHTML = timestamp;
                        if (status == "Stopped") {
                            x.style.backgroundColor = "#ffcccc";
                        }
                        return x;
                    }
                    $("#divscroll").scrollTop($("#divscroll").get(0).scrollHeight);
                }
              
            }
        }
        function roundToTwo(num) {
            return +(Math.round(num + "e+2") + "e-2");
        }
    </script>
</head>
<body id="bodyid">
    <form id="form1" runat="server">
    <div class="wrapper">
        <div style="top: 0px;">
            <div style="float: left; width: 250px; padding-left: 20px; padding-top: 0px;">
                <img src="Images/TitleLogo.png" />
                <label id="lblAsntech" style="font-family: Postmaster; font-size: 16px; font-weight: bold;">
                    Track My Asset</label>
            </div>
            <div>
                <span id="lbltripinfo" style="font-family: 'Trebuchet MS', arial, sans-serif; font-size: 12px;
                    width: 100%;"></span>
            </div>
            <div id="mapcontent">
                <div id="googleMap" style="width: 100%; height: 100%; position: relative; background-color: rgb(229, 227, 223);">
                </div>
            </div>
        </div>
        <div class="togglediv">
            <div class="inner">
                <input type="button" class="btntogglecls" value="Hide" id="btnshowhide" style="float: right;
                    background-color: Black; color: White;" onclick="clicked();" />
                <div id="divlogsgrid" style="height: 193px; cursor: auto; border: 1px solid #d5d5d5;
                    position: absolute; font-family: Arial; font-weight: normal; font-size: 12px;
                    top: 28px; float: left;">
                    <table id="example" style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff;
                        font-family: Arial; font-weight: normal; font-size: 12px;" cellpadding="2px"
                        cellspacing="0px" summary="Route Details" rules="groups" frame="hsides" border="2">
                        <tr>
                            <td>
                                <table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff; font-family: Arial;
                                    font-weight: normal; font-size: 12px;" cellpadding="2px" cellspacing="0px">
                                    <tr style="border-bottom: 1px solid #d5d5d5; background-color: #d5d5d5; height: 30px;
                                        font-family: Arial; font-weight: normal; font-size: 12px;">
                                        <th style="width: 150px;">
                                            TimeStamp
                                        </th>
                                        <th style="width: 400px;">
                                            Address
                                        </th>
                                        <th style="width: 100px;">
                                            Speed
                                        </th>
                                        <th style="width: 100px;">
                                            Odometer
                                        </th>
                                        <th style="width: 100px;">
                                            Status
                                        </th>
                                        <th style="width: 150px;">
                                            Stopped Time
                                        </th>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divscroll" style="width: 1050px; height: 123px; overflow: auto;">
                                    <table style="cursor: pointer; overflow: scroll; border: 1px solid #ffffff; font-family: Arial;
                                        font-weight: normal; font-size: 12px;" cellpadding="2px" cellspacing="0px">
                                        <tr id="templateRow" style="display: none; cursor: pointer; border-bottom: 1px solid #d5d5d5;"
                                            onclick="onrowclick(this);">
                                            <td style="width: 150px; text-align: center; border-bottom: 1px solid #d5d5d5;">
                                            </td>
                                            <td style="width: 400px; text-align: center; border-bottom: 1px solid #d5d5d5;">
                                            </td>
                                            <td style="width: 100px; text-align: center; border-bottom: 1px solid #d5d5d5;">
                                            </td>
                                            <td style="width: 100px; text-align: center; border-bottom: 1px solid #d5d5d5;">
                                            </td>
                                            <td style="width: 150px; text-align: center; border-bottom: 1px solid #d5d5d5;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: right; position: absolute; top: -3px; left: 50px; width: 280px;">
                    <table>
                        <tr>
                            <td>
                                <input type="button" class="btntogglecls" value="Preview Route" id="btnprevroute"
                                    style="width: 100px; background-color: #d5d5d5; color: #000000;" onclick="previewroute();" />
                            </td>
                            <td>
                                <input type="button" class="btntogglecls" value="Draw" id="btnShow" style="width: 90px;
                                    background-color: #d5d5d5; color: #000000;" onclick="load();" />
                            </td>
                            <td>
                                <input type="button" class="btntogglecls" value="Stop" id="btnstop" style="width: 90px;
                                    background-color: #d5d5d5; color: #000000;" onclick="btnstopclick();" />
                            </td>
                            <td>
                                <input type="button" class="btntogglecls" value="Clear" id="btnclear" style="width: 90px;
                                    background-color: #d5d5d5; color: #000000;" onclick="btnclearclick();" />
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0" style="border: 1px solid #d5d5d5; width: 55px;
                                    height: 22px; border-radius: 3px 3px 3px 3px;">
                                    <tr>
                                        <td>
                                            <button id="precday" class="datebuttonsleft" onclick="return PrevValidating();">
                                            </button>
                                        </td>
                                        <td>
                                            <input type="text" style="width: 40px; padding: .2em .0em; height: 21px; border-top: 0px solid #ffffff;
                                                text-align: center; border-bottom: 0px solid #ffffff; border-left: 1px solid #d5d5d5;
                                                border-right: 1px solid #d5d5d5; font-size: 13px;" readonly="readonly" id="speedval"
                                                value="speed" />
                                        </td>
                                        <td>
                                            <button id="nextday" class="datebuttonsright" onclick="return NextValidating();">
                                            </button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <input type="button" class="btntogglecls" style="height: 28px; opacity: 1.0; width: 100px;
                                    background-color: #d5d5d5; color: Blue;" onclick="tableToExcel('example', 'W3C Example Table')"
                                    value="Export to Excel">
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
