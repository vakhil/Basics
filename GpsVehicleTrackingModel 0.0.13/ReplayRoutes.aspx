<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="ReplayRoutes.aspx.cs" Inherits="ReplayRoutes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
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
            width: 320px;
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
            width: 300px;
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
            width: 35px;
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
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script src="Newjs/infobox.js" type="text/javascript"></script>
    <script type="text/javascript">
        // Add a Home control that returns the user to London
        function HomeControl(controlDiv, map) {
            controlDiv.style.padding = '5px 1px 0px 0px';
            var controlUI = document.createElement('div');
            controlUI.style.backgroundColor = 'rgb(255, 255, 255)';
            controlUI.style.border = '1px solid rgb(113, 123, 135)';
            controlUI.style.cursor = 'pointer';
            controlUI.style.textAlign = 'center';
            controlUI.style.width = '300px';
            controlUI.title = 'Update Time';
            controlUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            controlDiv.appendChild(controlUI);
            var controlText = document.createElement('div');
            controlText.style.fontFamily = 'Arial,sans-serif';
            controlText.style.fontSize = '30px';
            controlText.style.color = 'Green';
            controlText.style.paddingLeft = '4px';
            controlText.style.paddingRight = '4px';
            controlText.style.paddingTop = '4px';
            controlText.style.height = '30px';
            controlText.style.paddingTop = '15px';
            controlText.innerHTML = 'Update Time';
            controlText.id = 'lbl_logtime';
            controlUI.appendChild(controlText);
        }

        function DrawControl(controlDiv, map) {
            controlDiv.style.padding = '5px 1px 0px 0px';
            var controlUI = document.createElement('div');
            controlUI.style.backgroundColor = 'rgb(255, 255, 255)';
            controlUI.style.border = '1px solid rgb(113, 123, 135)';
            controlUI.style.cursor = 'pointer';
            controlUI.style.textAlign = 'center';
            controlUI.style.width = '90px';
            controlUI.title = 'Pause';
            controlUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            controlDiv.appendChild(controlUI);
            var controlText = document.createElement('div');
            controlText.style.fontSize = '20px';
            controlText.style.color = 'Green';
            controlText.style.paddingLeft = '4px';
            controlText.style.paddingRight = '4px';
            controlText.style.paddingTop = '4px';
            controlText.style.height = '20px';
            controlText.style.paddingTop = '4px';
            controlText.innerHTML = 'Pause';
            controlText.id = 'btnShow';
            controlUI.appendChild(controlText);

            // Setup click-event listener: simply set the map to London
            google.maps.event.addDomListener(controlUI, 'click', function () {
                load();
            });
        }
        function StopControl(controlDiv, map) {
            controlDiv.style.padding = '5px 1px 0px 0px';
            var controlUI = document.createElement('div');
            controlUI.style.backgroundColor = 'rgb(255, 255, 255)';
            controlUI.style.border = '1px solid rgb(113, 123, 135)';
            controlUI.style.cursor = 'pointer';
            controlUI.style.textAlign = 'center';
            controlUI.style.width = '90px';
            controlUI.title = 'Stop';
            controlUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            controlDiv.appendChild(controlUI);
            var controlText = document.createElement('div');
            controlText.style.fontSize = '20px';
            controlText.style.color = 'Green';
            controlText.style.paddingLeft = '4px';
            controlText.style.paddingRight = '4px';
            controlText.style.paddingTop = '4px';
            controlText.style.height = '20px';
            controlText.style.paddingTop = '4px';
            controlText.innerHTML = 'Stop';
            controlText.id = 'btnstop';
            controlUI.appendChild(controlText);

            // Setup click-event listener: simply set the map to London
            google.maps.event.addDomListener(controlUI, 'click', function () {
                btnstopclick();
            });
        }
        function ClearControl(controlDiv, map) {
            controlDiv.style.padding = '5px 1px 0px 0px';
            var controlUI = document.createElement('div');
            controlUI.style.backgroundColor = 'rgb(255, 255, 255)';
            controlUI.style.border = '1px solid rgb(113, 123, 135)';
            controlUI.style.cursor = 'pointer';
            controlUI.style.textAlign = 'center';
            controlUI.style.width = '90px';
            controlUI.title = 'Clear';
            controlUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            controlDiv.appendChild(controlUI);
            var controlText = document.createElement('div');
            controlText.style.fontSize = '20px';
            controlText.style.color = 'Green';
            controlText.style.paddingLeft = '4px';
            controlText.style.paddingRight = '4px';
            controlText.style.paddingTop = '4px';
            controlText.style.height = '20px';
            controlText.style.paddingTop = '4px';
            controlText.innerHTML = 'Clear';
            controlText.id = 'btnclear';
            controlUI.appendChild(controlText);

            // Setup click-event listener: simply set the map to London
            google.maps.event.addDomListener(controlUI, 'click', function () {
                btnclearclick();
            });
        }

        $(function () {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            var todaydate = new Date();
            var datetdy = todaydate.getFullYear() + "-" + (todaydate.getMonth() + 1) + "-" + todaydate.getDate() + "T00:00";
            $("#txtFromDate").val(datetdy);
            var data = { 'op': 'getplantsandtrips', 'plant': 'ALL' };
            var s = function (msg) {
                if (msg) {
                    Bindplants(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        });
        function Bindplants(data) {
            var select = document.getElementById('ddl_plants');
            var opt = document.createElement('option');
            opt.value = "Select Plant";
            opt.innerHTML = "Select Plant";
            select.appendChild(opt);
            for (var j = 0; j < data.plants.length; j++) {
                var opt = document.createElement('option');
                var plantstring = data.plants[j];
                opt.value = plantstring.split('@')[1];
                opt.innerHTML = plantstring.split('@')[0];
                select.appendChild(opt);
            }

            var divtrips = document.getElementById('divtrips');
            $("#divtrips").append("<table><tr><td><input autocomplete='off' class='checkinput' type='checkbox' title='SelectAll' onclick='checkclick(this);'/><span class='livalue'>Select All</span></td></tr></table>");
            for (var j = 0; j < data.routes.length; j++) {
                $("#divtrips").append("<table><tr><td><input autocomplete='off' class='checkinput' type='checkbox' onclick='checkclick(this);' title=" + data.routes[j] + "/><span class='livalue'>" + data.routes[j] + "</span></td></tr></table>");
            }
        }

        function plantsselectchange() {
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.options[selected.selectedIndex].text;
            if (plantname == "Select Plant") {
                alert("Please select Plant");
                return;
            }
            var data = { 'op': 'getplantsandtrips', 'plant': plantname };
            var s = function (msg) {
                if (msg) {
                    Bindroutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function Bindroutes(data) {
            document.getElementById('divtrips').innerHTML = "";
            var divtrips = document.getElementById('divtrips');
            $("#divtrips").append("<table><tr><td><input autocomplete='off' class='checkinput' type='checkbox' title='SelectAll' onclick='checkclick(this);'/><span class='livalue'>Select All</span></td></tr></table>");
            for (var j = 0; j < data.routes.length; j++) {
                $("#divtrips").append("<table><tr><td><input autocomplete='off' class='checkinput' type='checkbox' onclick='checkclick(this);' title=" + data.routes[j] + "/><span class='livalue'>" + data.routes[j] + "</span></td></tr></table>");
            }
        }
        function checkclick(checkedvalue) {
            var checkinputs = $('#divtrips').find('.checkinput');
            if (checkedvalue.title == "SelectAll") {
                if (checkedvalue.checked == true) {
                    checkinputs.each(function (list) {
                        var checkbox = checkinputs[list];
                        checkbox.checked = true;
                    });
                }
                else {
                    checkinputs.each(function (list) {
                        var checkbox = checkinputs[list];
                        checkbox.checked = false;
                    });
                }
            }
            else {
                if (checkedvalue.checked == false) {
                    checkinputs.each(function (list) {
                        var checkbox = checkinputs[list];
                        if (checkbox.title == "SelectAll") {
                            checkbox.checked = false;
                        }
                    });
                }
            }
        }
        $(function () {
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
                    $(".togglediv").animate({ left: '-305px' }, 500);
                    $("#btnClose").attr('title', "Show");
                    $("#btnClose").attr('src', "Images/bigright.png");
                    hidden = true;
                }
            });
        });
    </script>
    <script type="text/javascript">
        var map;
        function initialize() {
            var mapOptions = {
                zoom: 12,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
            var homeControlDiv = document.createElement('div');
            var homeControl = new HomeControl(homeControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(homeControlDiv);

            var ClearControlDiv = document.createElement('div');
            var clearcontrol = new ClearControl(ClearControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(ClearControlDiv);

            var StopControlDiv = document.createElement('div');
            var stopcontrol = new StopControl(StopControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(StopControlDiv);

            var DrawControlDiv = document.createElement('div');
            var drawcontrol = new DrawControl(DrawControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(DrawControlDiv);
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script type="text/javascript">
        function btn_generate_Click() {
            isfirstlocation = false;
            initialize();
            clearInterval(interval);
            var Username = '<%= Session["field1"] %>';
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.value;
            selected = document.getElementById('ddl_trips');
            var tripname = selected.value;
            var startdt = document.getElementById('txtFromDate').value;
            var checkinputs = $('#divtrips').find('.checkinput');
            var checkedroutes = "";
            checkinputs.each(function (list) {
                var checkbox = checkinputs[list];
                if (checkbox.checked == true) {
                    var spn = $(this).next('.livalue');
                    var spanvalue = spn[0].innerHTML;
                    if (spanvalue != "SelectAll") {
                        checkedroutes += spanvalue + "@";
                    }
                    else {
                        checkedroutes = "";
                        return false;
                    }
                }
            });
            if (checkedroutes.length > 0) {
                checkedroutes = checkedroutes.slice(0, checkedroutes.length - 1);
            }
            var data = { 'op': 'plantvehiclesdata', 'Username': Username, 'plantname': plantname, 'tripname': tripname, 'startdt': startdt, 'routes': checkedroutes };
            var s = function (msg) {
                if (msg) {
                    document.getElementById('btnShow').innerHTML = 'Pause';
                    polyroute(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function load() {
            var btnshow = document.getElementById('btnShow').innerHTML;
            if (btnshow == 'Draw') {
                btnclearclick();
                deleteOverlays();
                initialize();
                polyroute(triproutedata);
            }
            else if (btnshow == 'Pause') {
                clearInterval(interval);
                document.getElementById('btnShow').innerHTML = 'Resume';
            }
            else if (btnshow == 'Resume') {
                clearInterval(interval);
                var speed = $("#speedval").val();
                if (speed == "speed") {
                    speed = 3;
                }
                if (speed == 1) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 2000);
                }
                else if (speed == 2) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 1500);
                }
                else if (speed == 3) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 1000);
                }
                else if (speed == 4) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 500);
                }
                else if (speed == 5) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 300);
                }
                else if (speed == 6) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 100);
                }
                document.getElementById('btnShow').innerHTML = 'Pause';
            }
        }

        var triproutedata;
        var interval;
        var colors = new Array();
        var markersArray = [];
        var polilinepath = [];
        var firstlog = false;
        var stoppedmarkers = [];
        var flightPath = null;
        var maxcount = 0;
        var vehiclesarray = [];
        var allflightPlanCoordinates = [];
        var globellooper;
        var vehcolors = [];
        var isfirstlocation = false;
        function polyroute(msg) {
            logcount = 0;
            remcount = 0;
            $("#speedval").val("speed");
            polilinepath = [];
            allflightPlanCoordinates = [];
            if (flightPath) {
                flightPath.setMap(null);
            }
            colors = new Array("red", "blue", "black", "gray", "maroon", "Alabama Crimson", "Amber", "Bangladesh green", "Heart Gold", "Camouflage green", "Cadmium red", "Burgundy", "Bright green");

            triproutedata = msg;
            globellooper = triproutedata.GlobelLooper;
            maxcount = triproutedata.GlobelLooper.length;
            for (var cnt = 0; cnt < triproutedata.vehicleslist.length; cnt++) {
                var flightPlanCoordinates = [];
                vehiclesarray[triproutedata.vehicleslist[cnt].vehicleno] = flightPlanCoordinates;
                vehlogscount[triproutedata.vehicleslist[cnt].vehicleno] = 0;
                vehcolors[triproutedata.vehicleslist[cnt].vehicleno] = colors[cnt % colors.length];
                allflightPlanCoordinates.push(flightPlanCoordinates);
            }
            var selected = document.getElementById('ddl_plants');
            var plantname = selected.options[selected.selectedIndex].text;
            for (var cnt = 0; cnt < triproutedata.BranchDatalist.length; cnt++) {
                var Latitude = triproutedata.BranchDatalist[cnt].latitude;
                var Longitude = triproutedata.BranchDatalist[cnt].longitude;
                var point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                var pltname = triproutedata.BranchDatalist[cnt].BranchName;
                if (pltname == plantname) {
                    var marker = new google.maps.Marker({
                        position: point,
                        map: map,
                        center: location,
                        zoom: 12,
                        title: pltname
                    });
                    map.panTo(point);
                }
                else {
                    if (isfirstlocation) {
                        var lctnicon = "Images/ssmarker.png";
                        var marker = new google.maps.Marker({
                            position: point,
                            map: map,
                            center: location,
                            zoom: 12,
                            icon: lctnicon,
                            title: pltname
                        });
                    }
                    else {
                        var lctnicon = "Images/plantmarker.png";
                        var marker = new google.maps.Marker({
                            position: point,
                            map: map,
                            center: location,
                            zoom: 12,
                            icon: lctnicon,
                            title: pltname
                        });
                        isfirstlocation = true;
                    }
                }
            }
            clearInterval(interval);
            interval = setInterval(function () { timerlogs() }, 2000);
        }

        var logcount = 0;
        var remcount = 0;
        function timerlogs() {
            //            deleteOverlays();
            if (logcount < maxcount) {
                var searchlbl = globellooper[logcount];
                for (var cnt = 0; cnt < triproutedata.vehicleslist.length; cnt++) {
                    document.getElementById('lbl_logtime').innerHTML = searchlbl;
                    var getinfo = triproutedata.vehicleslist[cnt].logslist[searchlbl];

                    // if (triproutedata.vehicleslist[cnt].logslist.contans(.length > logcount) {
                    if (typeof getinfo !== "undefined") {
                        var flightPlanCoordinates1 = [];
                        flightPlanCoordinates1 = vehiclesarray[triproutedata.vehicleslist[cnt].vehicleno];
                        var Latitude = getinfo.latitude;
                        var Longitude = getinfo.longitude;
                        var point = new google.maps.LatLng(
              parseFloat(Latitude),
              parseFloat(Longitude));
                        var angle = getinfo.direction;
                        var iconsrc;
                        var VehicleType = "Car";
                        var imgangle;
                        VehicleType = "green" + VehicleType;
                        if (angle >= 0 && angle < 22.5) {
                            imgangle = 0;
                            iconsrc = "VehicleTypes/" + VehicleType + "4.png";
                        }
                        else if (angle >= 22.5 && angle < 45) {
                            imgangle = 22.5;
                            iconsrc = "VehicleTypes/" + VehicleType + "5.png";
                        }
                        else if (angle >= 45 && angle < 67.5) {
                            imgangle = 45.5;
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

                        for (var i = 0; i < markersArray.length; i++) {
                            var vehmarker = markersArray[i];
                            if (markersArray[i].id == triproutedata.vehicleslist[cnt].vehicleno) {
                                markersArray[i].setMap(null);
                                markersArray.splice(i, 1);
                            }
                        }
                        var image = new google.maps.MarkerImage(iconsrc, null, new google.maps.Point(0, 0), new google.maps.Point(20, 20));
                        var marker = new google.maps.Marker({
                            position: point,
                            map: map,
                            center: location,
                            zoom: 12,
                            icon: image,
                            id: triproutedata.vehicleslist[cnt].vehicleno
                        });

                        markersArray.push(marker);
                        attachInfowindow(marker, location, "VehicleID : " + triproutedata.vehicleslist[cnt].vehicleno + "<br/>" + "Route : " + triproutedata.vehicleslist[cnt].routename);


                        var point = new google.maps.LatLng(
                          parseFloat(Latitude),
                          parseFloat(Longitude));
                        remcount = vehlogscount[triproutedata.vehicleslist[cnt].vehicleno];
                        flightPlanCoordinates1[getinfo.sno - remcount] = new google.maps.LatLng(Latitude, Longitude);
                        var clr = vehcolors[triproutedata.vehicleslist[cnt].vehicleno];
                        var polyOptions = {
                            path: flightPlanCoordinates1,
                            strokeColor: clr,
                            strokeOpacity: 1.0,
                            strokeWeight: 2
                        }
                        flightPath = new google.maps.Polyline(polyOptions);
                        flightPath.setMap(map);
                        polilinepath.push(flightPath);
                    }
                }
                logcount++;
            }
            else {
                clearInterval(interval);
            }
        }
        function attachInfowindow(marker, latlng, country) {
            var location = latlng;
            var boxText = document.createElement("div");
            boxText.style.cssText = "border: 1px solid black; margin-top: 8px; background: white; padding: 5px;";
            boxText.innerHTML = '<b>' + country + '</b><br />';

            var myOptions = {
                content: boxText
				, disableAutoPan: false
				, maxWidth: 0
				, pixelOffset: new google.maps.Size(-140, 0)
				, zIndex: null
				, boxStyle: {
				    background: "url('Images/tipbox.gif') no-repeat"
				  , opacity: 0.9
				  , width: "350px"
				}
				, closeBoxMargin: "10px 5px 0px 2px"
                , closeBoxURL: ""
				, infoBoxClearance: new google.maps.Size(1, 1)
				, isHidden: false
				, pane: "floatPane"
				, enableEventPropagation: false
            };


            var ib = new InfoBox(myOptions);
            //var infowindow = new google.maps.InfoWindow({ content: '<b>' + description + '</b><br />' + location });
            google.maps.event.addListener(marker, 'mouseover', function () {
                //infowindow.open(map,marker);
                ib.open(map, marker);
            });
            google.maps.event.addListener(marker, 'mouseout', function () {
                //infowindow.close();
                ib.close();
            });
        }
        function deleteOverlays() {
            clearOverlays();
            markersArray = [];
        }

        // Sets the map on all markers in the array.
        function setAllMap(map) {
            for (var i = 0; i < markersArray.length; i++) {
                markersArray[i].setMap(map);
            }
        }

        // Removes the overlays from the map, but keeps them in the array.
        function clearOverlays() {
            setAllMap(null);
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
        function PrevValidating() {
            var speed = $("#speedval").val();
            if (speed == "speed") {
                speed = 3;
            }
            if (speed > 1) {
                speed--;
            }
            $("#speedval").val(speed);
            var btnshow = document.getElementById('btnShow').innerHTML;
            if (btnshow == 'Pause') {

                if (speed == 1) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 2000);
                }
                else if (speed == 2) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 1500);
                }
                else if (speed == 3) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 1000);
                }
                else if (speed == 4) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 500);
                }
                else if (speed == 5) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 300);
                }
                else if (speed == 6) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 100);
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
            var btnshow = document.getElementById('btnShow').innerHTML;
            if (btnshow == 'Pause') {
                if (speed == 1) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 2000);
                }
                else if (speed == 2) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 1500);
                }
                else if (speed == 3) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 1000);
                }
                else if (speed == 4) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 500);
                }
                else if (speed == 5) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 300);
                }
                else if (speed == 6) {
                    clearInterval(interval);
                    interval = setInterval(function () { timerlogs() }, 100);
                }
            }
            return false;
        }
        function btnstopclick() {
            clearInterval(interval);
            $("#speedval").val("speed");
            firstlog = false;
            document.getElementById('btnShow').innerHTML = 'Draw';
            logcount = 0;
            remcount = 0;
        }
        var vehlogscount = [];
        function btnclearclick() {
            deleteallOverlays();
            function deleteallOverlays() {
                clearallOverlays();
                polilinepath = [];
                //                if (flightPath) {
                //                    flightPath.setMap(null);
                //                }
                //                vehlogscount = [];
                //                flightPlanCoordinates = [];
                //                allflightPlanCoordinates = [];
                for (var cnt = 0; cnt < triproutedata.vehicleslist.length; cnt++) {
                    var flightPlanCoordinates = [];
                    flightPlanCoordinates = vehiclesarray[triproutedata.vehicleslist[cnt].vehicleno];
                    var prevcnt = vehlogscount[triproutedata.vehicleslist[cnt].vehicleno];
                    remcount = flightPlanCoordinates.length;
                    var tot = remcount + prevcnt;
                    vehlogscount[triproutedata.vehicleslist[cnt].vehicleno] = tot;
                    flightPlanCoordinates.length = 0;
                    vehiclesarray[triproutedata.vehicleslist[cnt].vehicleno] = flightPlanCoordinates;
                    //allflightPlanCoordinates.push(flightPlanCoordinates);
                }
            }

            // Sets the map on all markers in the array.
            function allsetAllMap(map) {
                for (i = 0; i < polilinepath.length; i++) {
                    polilinepath[i].setMap(map); //or line[i].setVisible(false);
                }
            }

            // Removes the overlays from the map, but keeps them in the array.
            function clearallOverlays() {
                allsetAllMap(null);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 100%; height: 100%;">
        <div class="togglediv" id="divtoggle">
            <div class="inner">
                <img id="btnClose" alt="" src="Images/bigleft.png" title="Hide" style="float: right;
                    border: 1px solid #d5d5d5; width: 17px; height: 20px; background-color: #ffffff;" />
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Plant Name</label>
                                    </td>
                                    <td>
                                        <%--  <asp:DropDownList ID="ddl_plant" CssClass="txtClass" runat="server" Width="150px">
                                    <asp:ListItem Value="0" Selected="True">Select Plant</asp:ListItem>
                                </asp:DropDownList>--%>
                                        <select id="ddl_plants" style="width: 200px;" onchange="plantsselectchange();">
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="divtrips" style="height: 200px; width: 200px; border: 1px solid #d5d5d5;
                                            border-radius: 3px 3px 3px 3px; overflow: auto;">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Trip Type</label>
                                    </td>
                                    <td>
                                        <%--    <asp:DropDownList ID="ddl_trip" CssClass="txtClass" runat="server" Width="150px">
                                    <asp:ListItem Value="0" Selected="True">Select Route</asp:ListItem>
                                </asp:DropDownList>--%>
                                        <select id="ddl_trips" style="width: 200px;">
                                            <option>AM</option>
                                            <option>PM</option>
                                        </select>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Date</label>
                                    </td>
                                    <td>
                                        <input id="txtFromDate" type="date" style="width: 200px; height: 22px; font-size: 13px;
                                            padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <%--      <tr>
                                <td>
                                    <label>
                                        End Time</label>
                                </td>
                                <td>
                                    <input id="txtToDate" type="datetime-local" style="width: 200px; height: 22px; font-size: 13px;
                                        padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>--%>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <input type="button" id="btn_generate" class="ContinueButton" value="Get Data" style="height: 25px;
                                            width: 100px;" onclick="btn_generate_Click();" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lbl_nofifier" runat="server" ForeColor="Red" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
                                    </td>
                                </tr>
                                <tr>
                                    <%--   <td>
                                <input type="button" class="btntogglecls" value="Pause" id="btnShow" style="width: 90px; background-color:#d5d5d5;color:#000000;"
                                    onclick="load();" />
                            </td>
                            <td>
                                <input type="button" class="btntogglecls" value="Stop" id="btnstop" style="width: 90px; background-color:#d5d5d5;color:#000000;"
                                    onclick="btnstopclick();" />
                                       <input type="button" class="btntogglecls" value="Clear" id="btnclear" style="width: 90px; background-color:#d5d5d5;color:#000000;"
                                    onclick="btnclearclick();" />
                            </td>--%>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <%--<input type="text" value="" readonly="readonly" id="lbl_logtime" style="color:Green;font-size:30px;font-weight:bolder;text-align:center;width:300px;" />--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="mapcontent">
            <div id="googleMap" style="width: 100%; height: 100%; position: relative; background-color: rgb(229, 227, 223);">
            </div>
        </div>
    </div>
</asp:Content>
