<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="ReplayRoutes_general.aspx.cs" Inherits="ReplayRoutes" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckList" Namespace="UNLV.IAP.WebControls" TagPrefix="cc1" %>
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
        
          #slider_preview .ui-slider-range { background: #ef2929; }
  #slider_preview .ui-slider-handle { border-color: #ef2929; }
    </style>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script src="Newjs/infobox.js" type="text/javascript"></script>
    <script type="text/javascript" src="DropDownCheckList.js"></script>
     <%-- <script src="DropDownCheckList.js" type="text/javascript"></script>
  <script src="DropDownCheckList1.js" type="text/javascript"></script>--%>
    <script src="js/JTemplate.js" type="text/javascript"></script>
       <script src="js/jquery-ui.js" type="text/javascript"></script>
    <link href="js/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="font/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Draw_lines_check(controlDiv, map) {
            controlDiv.style.padding = '5px 1px 0px 0px';
            var controlUI1 = document.createElement('div');
            controlUI1.style.backgroundColor = 'rgb(255, 255, 255)';
            controlUI1.style.border = '1px solid rgb(113, 123, 135)';
            controlUI1.style.cursor = 'pointer';
            // controlUI1.style.float = 'left';
            //  controlUI1.style.textAlign = 'center';
            controlUI1.style.width = '250px';
            controlUI1.title = 'With Lines';
            controlUI1.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            controlDiv.appendChild(controlUI1);

            var checkbox = document.createElement('input');
            checkbox.type = "checkbox";
            checkbox.name = "name";
            checkbox.value = "value";
            //checkbox.style.paddingLeft = '20px';
            //checkbox.style.paddingRight = '20px';
            //checkbox.style.paddingTop = '4px';
            checkbox.style.height = '20px';
            //checkbox.style.paddingTop = '15px';
            checkbox.style.float = "left";
            checkbox.id = "with_lines";
            controlUI1.appendChild(checkbox);
            var controlText1 = document.createElement('div');
            controlText1.style.fontFamily = 'Arial,sans-serif';
            controlText1.style.fontSize = '20px';
            controlText1.style.color = 'Green';
            controlText1.style.paddingLeft = '20px';
            controlText1.style.paddingRight = '20px';
            controlText1.style.paddingTop = '4px';
            controlText1.style.height = '20px';
            controlText1.style.paddingTop = '5px';

            controlText1.id = 'with_lines_lbl';
            // controlText1.type = "checkbox";
            controlText1.innerHTML = "With Lines";
            //  controlText1.text = "With Lines";
            controlUI1.appendChild(controlText1);

            google.maps.event.addDomListener(controlUI1, 'click', function () {
                btnclearclick();
            });
        }

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
            controlText.style.height = '25px';
           // controlText.style.paddingTop = '5px';
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
            controlUI.style.width = '40px';
            controlUI.style.height = '30px';
            controlUI.title = 'Pause';
            controlUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            controlDiv.appendChild(controlUI);
            var controlText = document.createElement('div');
            controlText.className = "fa fa-play";
            controlText.style.fontSize = '20px';
            controlText.style.color = 'Green';
            controlText.style.paddingLeft = '4px';
            controlText.style.paddingRight = '4px';
            controlText.style.paddingTop = '4px';
            controlText.style.height = '20px';
            controlText.style.width = '20px';
            controlText.style.paddingTop = '4px';
            //controlText.innerHTML = 'Pause';
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
            controlUI.style.width = '40px';
            controlUI.style.height = '30px';
            controlUI.title = 'Stop';
            controlUI.style.boxShadow = "rgba(0, 0, 0, 0.4) 0px 2px 4px";
            controlDiv.appendChild(controlUI);
            var controlText = document.createElement('div');
            controlText.style.fontSize = '20px';
            controlText.style.color = 'red';
            controlText.style.paddingLeft = '4px';
            controlText.style.paddingRight = '4px';
            controlText.style.paddingTop = '4px';
            controlText.style.height = '20px';
            controlText.style.width = '20px';
            controlText.style.paddingTop = '4px';
           // controlText.innerHTML = 'Stop';
            controlText.className = "fa fa-stop";
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
            controlUI.style.width = '60px';
            controlUI.style.height = '30px';
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
            controlText.style.width = '20px';
            controlText.style.paddingTop = '4px';
            controlText.innerHTML = 'Clear';
            controlText.id = 'btnclear';
            controlUI.appendChild(controlText);

            // Setup click-event listener: simply set the map to London
            google.maps.event.addDomListener(controlUI, 'click', function () {
                btnclearclick();
            });
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
        var map;
        function initialize() {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            var mapOptions = {
                zoom: 12,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };


            map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
            var Draw_lineControlDiv = document.createElement('div');
            var Draw_lineControl = new Draw_lines_check(Draw_lineControlDiv, map);
            map.controls[google.maps.ControlPosition.TOP_RIGHT].push(Draw_lineControlDiv);

        //    map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);
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
            var startdt = document.getElementById('txtFromDate').value;
            var enddt = document.getElementById('txtToDate').value;
            var checkedvehicle = "";
            for (var i = 0; i < checkedvehicles.length; i++) {
                if (checkedvehicles[i] != "SelectAll") {
                    checkedvehicle += checkedvehicles[i] + "@";
                }
                else {
                    checkedvehicle = "";
                    return false;
                }
            }
            if (checkedvehicle.length > 0) {
                checkedvehicle = checkedvehicle.slice(0, checkedvehicle.length - 1);
            }
            var data = { 'op': 'plantvehiclesdata_gen', 'Username': Username, 'startdt': startdt, 'enddt': enddt, 'checkedvehicle': checkedvehicle };
            var s = function (msg) {
                if (msg) {
                    //document.getElementById('btnShow').innerHTML = 'Pause';
                    document.getElementById('btnShow').className = 'fa fa-pause';

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
            // var btnshow = document.getElementById('btnShow').innerHTML;
            var btn_Showobj = document.getElementById('btnShow');
            var btnshow = btn_Showobj.className;
           // if (btnshow == 'Draw') {

            if (btnshow == 'fa fa-play') {
//                btn_Showobj.className = 'fa fa-pause';
//                btn_Showobj.title = 'Paused'
                btnclearclick();
                deleteOverlays();
                //initialize();
                polyroute(triproutedata);
                document.getElementById('btnShow').className = 'fa fa-pause';
                document.getElementById('btnShow').title = 'Paused'
       
                
            }
            else if (btnshow == 'fa fa-pause')//'Pause') {
            {
                clearInterval(interval);
               // document.getElementById('btnShow').innerHTML = 'Resume';
                document.getElementById('btnShow').className = 'fa  fa-play-circle-o';
                document.getElementById('btnShow').title = 'Resume';

            }
            else if (btnshow == 'fa  fa-play-circle-o'){//'Resume') {
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
                //   document.getElementById('btnShow').innerHTML = 'Pause';
                document.getElementById('btnShow').className = 'fa fa-pause';
                document.getElementById('btnShow').title = 'Pause'
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

            $('#slider_preview').slider({
                min: 1,
                max: maxcount,
                range: "min",
                value: 0,
                slide: function (event, ui) {

                   // var btnshow = document.getElementById('btnShow').innerHTML;
                    var btnshow = document.getElementById('btnShow').className;
                    if (btnshow == 'fa fa-pause') {
                        load();
                        btnclearclick();
                    }
                    $('#with_lines').removeAttr('checked'); //.attr('checked', 'unchecked');

                    logcount = ui.value - 1;
                    slider_timerlogs();


                    //  select[ 0 ].selectedIndex = ui.value - 1;
                }
            });

            for (var cnt = 0; cnt < triproutedata.vehicleslist.length; cnt++) {
                var flightPlanCoordinates = [];
                vehiclesarray[triproutedata.vehicleslist[cnt].vehicleno] = flightPlanCoordinates;
                vehlogscount[triproutedata.vehicleslist[cnt].vehicleno] = 0;
                vehcolors[triproutedata.vehicleslist[cnt].vehicleno] = colors[cnt % colors.length];
                allflightPlanCoordinates.push(flightPlanCoordinates);
            }
            clearInterval(interval);
            interval = setInterval(function () { timerlogs() }, 2000);
        }

        var logcount = 0;
        var remcount = 0;
        var grpvehicles = [];
        function timerlogs() {
            //            deleteOverlays();
            if (logcount < maxcount) {
                var searchlbl = globellooper[logcount];
                for (var cnt = 0; cnt < triproutedata.vehicleslist.length; cnt++) {
                    document.getElementById('lbl_logtime').innerHTML = searchlbl;
                    var getinfo = triproutedata.vehicleslist[cnt].logslist[searchlbl];
                    $('#slider_preview').slider("value", logcount);
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

                        var infowindow = new google.maps.InfoWindow();
                        var Routename = "";
                        var vehiclemodeltype = "";
                        var vehicleno = "";
                        for (var v = 0; v < grpvehicles.length; v++) {
                            if (triproutedata.vehicleslist[cnt].vehicleno == grpvehicles[v].vehicleno) {
                                Routename = grpvehicles[v].Routename;
                                vehiclemodeltype = grpvehicles[v].vehiclemodeltype;
                                vehicleno = grpvehicles[v].vehicleno;
                            }
                        }
                   
                        infowindow.setContent("VehicleID : <label style='font-size:15px;font-weight:bold;'>" + vehicleno + "</label></br>Route : <label style='font-size:15px;font-weight:bold;'>" + Routename + "</label>");
                        infowindow.open(map, marker);


                        var point = new google.maps.LatLng(
                          parseFloat(Latitude),
                          parseFloat(Longitude));
                        remcount = vehlogscount[triproutedata.vehicleslist[cnt].vehicleno];
                        flightPlanCoordinates1[getinfo.sno - remcount] = new google.maps.LatLng(Latitude, Longitude);
                        var clr = vehcolors[triproutedata.vehicleslist[cnt].vehicleno];

                        if ($('#with_lines').is(":checked")) {
                            // it is checked
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
                        if (!isfirstlocation) {
                            var latlngbounds = new google.maps.LatLngBounds();
                            latlngbounds.extend(point);
                            map.fitBounds(latlngbounds);
                            map.setZoom(13);
                            isfirstlocation = true;
                        }
                        //                        var polyOptions = {
                        //                            path: flightPlanCoordinates1,
                        //                            strokeColor: clr,
                        //                            strokeOpacity: 1.0,
                        //                            strokeWeight: 2
                        //                        }
                        //                        flightPath = new google.maps.Polyline(polyOptions);
                        //                        flightPath.setMap(map);
                        //                        polilinepath.push(flightPath);
                    }
                }
                logcount++;
            }
            else {
                clearInterval(interval);
            }
        }


        function slider_timerlogs() {
            //            deleteOverlays();
            if (logcount < maxcount) {
                var searchlbl = globellooper[logcount];
                for (var cnt = 0; cnt < triproutedata.vehicleslist.length; cnt++) {
                    document.getElementById('lbl_logtime').innerHTML = searchlbl;
                    var getinfo = triproutedata.vehicleslist[cnt].logslist[searchlbl];
                    $('#slider_preview').slider("value", logcount);
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
                        //                        var polyOptions = {
                        //                            path: flightPlanCoordinates1,
                        //                            strokeColor: clr,
                        //                            strokeOpacity: 1.0,
                        //                            strokeWeight: 2
                        //                        }
                        //                        flightPath = new google.maps.Polyline(polyOptions);
                        //                        flightPath.setMap(map);
                        //                        polilinepath.push(flightPath);
                    }
                }
                // logcount++;
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
           // var btnshow = document.getElementById('btnShow').innerHTML;
            var btnshow = document.getElementById('btnShow').className;
            if (btnshow == 'fa fa-pause') {

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
           // var btnshow = document.getElementById('btnShow').innerHTML;
            var btnshow = document.getElementById('btnShow').className;
            if (btnshow == 'fa fa-pause') {
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
         //   var btnshow = document.getElementById('btnShow').className;
            document.getElementById('btnShow').className = 'fa fa-play';
            document.getElementById('btnShow').title = 'Play';
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
    <script type="text/javascript">

        function onallcheck(id) {
             checkedvehicles = [];
            for (var vehicledata in livedata) {
                var vehicleno = livedata[vehicledata].vehicleno;
                var vehicle = $("#" + vehicleno + "");
                if (id.checked == true) {
                    if (typeof vehicle[0] === "undefined") {
                    }
                    else {
                        vehicle[0].checked = true;
                        checkedvehicles.push(vehicleno);
                    }
                }
                else {
                    if (typeof vehicle[0] === "undefined") {
                    }
                    else {
                        vehicle[0].checked = false;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        function NormalOpening() {
            window.open("RouteDrawing.aspx");
            return true;
        }
        function SplOpening() {
            window.open('RouteDrawing.aspx');
            return true;
        }
        function XXLOpening() {

            window.open("exporttoxl.aspx", "_blank");
            return true;
        }
       
    </script>
    <script type="text/javascript">
        $(function () {
            var data = { 'op': 'InitilizeVehiclesreports_gen' };
            //            var data = { 'op': 'InitilizeGroups' };
            var s = function (msg) {
                if (msg) {
                    Groupsfilling(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        });
        var livedata = [];
        function Groupsfilling(data) {
            livedata = data;
            var vehiclesdata = data;
            var vehiclenos = new Array();
            for (var vehicleid in vehiclesdata) {
                vehiclenos.push({ vehicleno: vehiclesdata[vehicleid].vehicleno, vehiclemodeltype: vehiclesdata[vehicleid].vehicletype });
            }
            $('#divassainedvehs').css('display', 'block');
            $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
                $('#divassainedvehs').setTemplateURL('Reports1.htm');
                $('#divassainedvehs').processTemplate(vehiclenos);
                document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
            });
//            $('#divAllvehicles').css('display', 'block');
//            $.getScript("js/JTemplate.js", function (data, textStatus, jqxhr) {
//                $('#divAllvehicles').setTemplateURL('LiveView1.htm');
//                $('#divAllvehicles').processTemplate(vehiclenos);
//                liveupdate();
//                document.getElementById('lblvehscount').innerHTML = vehiclenos.length;
//            });
        }

      
    </script>
    <script type="text/javascript">
        var checkedvehicles = [];
        function onvehiclecheck(id, onckvehicleid) {
            for (var i = checkedvehicles.length - 1; i >= 0; i--) {
                if (checkedvehicles[i] === onckvehicleid) {
                    checkedvehicles.splice(i, 1);
                }
            }
            if (id.checked == true) {
                checkedvehicles.push(onckvehicleid);
            }
        }
        //for overcome liveupdate error in reports
        function liveupdate() {
        }   
    </script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $('input[name="radio-choice"]').change(function () {
                var rblauthorizedtype = $(this).next('label').text();
                if (rblauthorizedtype == "Plants") {
                    document.getElementById('divchblvehicles').style.display = "block";
                    document.getElementById('divchblVendors').style.display = "none";
                }
                else if (rblauthorizedtype == "Vendors") {
                    document.getElementById('divchblvehicles').style.display = "none";
                    document.getElementById('divchblVendors').style.display = "block";
                }
                var data = { 'op': 'setauthorizedsession', 'Authorizedtype': rblauthorizedtype };
                var s = function (msg) {
                    if (msg) {
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                callHandler(data, s, e);

               
            });
        });
    </script>
    <script type="text/javascript">
        function PopupClose() {
            $('#cell').toggle();
        }
        function deletelocationOverlays() {
        }
        function bphover() {
        }
        function bpmouseout() {
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
            <table style="position:absolute;">
                <tr>
                    <td id="cell" valign="top" style="border: 1px solid #d5d5d5; background-color: #f4f4f4;
                        height: 650px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                          <asp:Label ID="lblreportname" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#a9a9a9" Text=""></asp:Label>
                                            <table style="width: 98%;">
                                                <tr>
                                                    <td style="width: 80px;color:Black;font-weight:bold;">
                                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                                                    </td>
                                                    <td>
                                                       <input id="txtFromDate" type="datetime-local" style="width: 200px; height: 22px; font-size: 13px;
                                            padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="color:Black;font-weight:bold;">
                                                        <asp:Label ID="lblToDate" runat="server" Text="   To Date   "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <input id="txtToDate" type="datetime-local" style="width: 200px; height: 22px; font-size: 13px;
                                            padding: .2em .4em; border: 1px solid gray; border-radius: 4px 4px 4px 4px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                 <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <div style="display: block; width: 240px;" id="divchblvehicles">
                                                    <cc1:DropDownCheckList ID="chblVehicleTypes" runat="server" BorderColor="black" BorderStyle="Solid"
                                                        AutoPostBack="true" ForeColor="#979797" Style="color: #979797" CheckListCssStyle="position:absolute;z-index:99999;overflow: auto; border: 1px solid black; padding: 4px; max-height:300px; background-color: #ffffff;"
                                                        DisplayBoxCssStyle="border: 1px solid #000000; cursor: pointer; width:240px; height:30px;z-index:99999;color:black;font-size:14px;font-weight:bold;"
                                                        Width="160px" TextWhenNoneChecked="Vehicle Type">
                                                    </cc1:DropDownCheckList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:DropDownCheckList ID="chblZones" runat="server" BorderColor="black" BorderStyle="Solid"
                                                    AutoPostBack="true" ForeColor="#979797" Style="color: #979797" CheckListCssStyle="position:absolute;z-index:99999;overflow: auto; border: 1px solid black; padding: 4px; max-height:300px; background-color: #ffffff;"
                                                    DisplayBoxCssStyle="border: 1px solid #000000; cursor: pointer; width:240px; height:30px;z-index:99999;color:black;font-size:14px;font-weight:bold;"
                                                    Width="160px" TextWhenNoneChecked="Plant Name">
                                                </cc1:DropDownCheckList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                              <tr>
                                         <td>
                                            <table cellpadding="0" cellspacing="0" style="border: 1px solid #d5d5d5; width: 55px;
                                            height: 22px; border-radius: 3px 3px 3px 3px;">
                                            <tr>
                                            <td>
                                               <input type="button" id="btn_generate" class="ContinueButton" value="Get Data" style="height: 25px;
                                            width: 100px;" onclick="btn_generate_Click();" />
                                            </td>
                                            <td></td>
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
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div id="divassainedvehs" style="border: 1px solid rgb(192, 192, 192);float: left;padding-left: 10px;overflow: auto;width: 300px;display: block;position: absolute;bottom: 0px;top: 215px;">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        </div>
    </div>
      <div id="mapcontent">
        <div style="width: 98%; margin: 14px 3px 7px 20px; position: relative;" id="slider_preview"></div>
            <div id="googleMap" style="width: 100%; height: 95%; position: relative; background-color: rgb(229, 227, 223);">
            </div>
        </div>
        </div>
</asp:Content>
