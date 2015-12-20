<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RouteAssignSales.aspx.cs" Inherits="RouteAssignSales" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <style>
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
            height: 30px;
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
        
    </style>
    <script type="text/javascript">
        $(function () {
            FillBranches();
            bindmanageroutes();
        });

        

        function divonclick(selected) {
            selectedindex = selected;
            var elem = document.getElementById(selectedindex);
            if (elem.style.backgroundColor == "" || elem.style.backgroundColor == 'rgb(255, 255, 255)' || elem.style.backgroundColor == 'rgba(0, 0, 0, 0)') {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
                elem.style.backgroundColor = '#ffffcc';
            }
            else {
                $('.divselectedclass').each(function () {
                    $(this).css('background-color', '#ffffff');
                });
            }
        }
        function btnUpClick() {
            var elem = document.getElementById(selectedindex);
            $(elem).insertBefore($(elem).prev());
            GetPoly();
        }
        function btnDownClick() {
            var elem = document.getElementById(selectedindex);
            $(elem).insertAfter($(elem).next());
            GetPoly();
        }
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
        var zoomLevel = 10;
        function GetPoly() {
            var ArrayPath = [];
            var j = 1;
            BrachNames = "";
            Ranks = "";
            map = new google.maps.Map(document.getElementById('googleMap'), {
                zoom: zoomLevel,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });

            google.maps.event.addListener(map, 'zoom_changed', function () {
                zoomLevel = map.getZoom();
            });
            var maxcount = 0;
            $('.divselectedclass').each(function () {
                var Selected = $(this);
                var Selectedid = Selected[0].id;
                for (var cnt = 0; cnt < allbranchesdata.length; cnt++) {
                    if (Selectedid == allbranchesdata[cnt].id) {
                        maxcount++;
                    }
                }
            });

            $('.divselectedclass').each(function () {
                var Selected = $(this);
                var Selectedid = Selected[0].id;
                for (var cnt = 0; cnt < allbranchesdata.length; cnt++) {
                    if (Selectedid == allbranchesdata[cnt].id) {
                        ArrayPath.push(new google.maps.LatLng(allbranchesdata[cnt].latitude, allbranchesdata[cnt].longitude));

                        BrachNames += allbranchesdata[cnt].Name + "-";
                        Ranks += j + "-";
                        var content = "Branch Name : " + allbranchesdata[cnt].Name + "\n" + "Rank : " + j;
                        var iconpath = ""
                        if (j == 1|| j==maxcount) {
                            iconpath = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + (j) + "|5ADFB0|000000";
                        } else {

                            iconpath = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + (j) + "|FF0000|000000";
                        }
                        marker = new google.maps.Marker({
                            position: new google.maps.LatLng(allbranchesdata[cnt].latitude, allbranchesdata[cnt].longitude),
                            map: map,
                            title: content,
                            icon: iconpath
                        });
                        j++;
                        var myCenter = new google.maps.LatLng(allbranchesdata[cnt].latitude, allbranchesdata[cnt].longitude);
                        map.panTo(myCenter);
                    }
                }
            });
            var line = new google.maps.Polyline({
                path: ArrayPath,
                strokeColor: '#ff0000',
                strokeOpacity: 2.0,
                strokeWeight: 3
            });
            line.setMap(map);
        }
        function TabclassClick() {
            $("input[type='checkbox']").click(function () {
                if ($(this).is(":checked")) {
                    var Selected = $(this).next().text();
                    var Selectedid = $(this).next().next().val();

                    var label = document.createElement("div");
                    var Crosslabel = document.createElement("img");
                    Crosslabel.style.float = "right";
                    Crosslabel.src = "Images/Cross.png";
                    Crosslabel.onclick = function () { RemoveClick(Selectedid); };
                    label.id = Selectedid;
                    label.innerHTML = Selected;
                    label.className = 'divselectedclass';
                    label.onclick = function () { divonclick(Selectedid); };
                    document.getElementById('divselected').appendChild(label);
                    label.appendChild(Crosslabel);
                    GetPoly();
                }
                else {
                    var Selected = $(this).next().next().val();
                    var elem = document.getElementById(Selected);
                    var p = elem.parentNode;
                    p.removeChild(elem);
                    GetPoly();
                }
            });
        }

        function RemoveClick(Selected) {
            var elem = document.getElementById(Selected);
            var p = elem.parentNode;
            p.removeChild(elem);
            $('.chkclass').each(function () {
                if ($(this).next().next().val() == Selected) {
                    $(this).attr("checked", false);
                }
            });
            GetPoly();
        }

        function gridselctionchanged(msg) {
            var j = 1;
            BrachNames = "";
            Ranks = "";
            var ArrayPath = [];
            map = new google.maps.Map(document.getElementById('googleMap'), {
                zoom: zoomLevel,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
            google.maps.event.addListener(map, 'zoom_changed', function () {
                zoomLevel = map.getZoom();
            });
            if (msg.length > 0) {
                document.getElementById('ddlStartLoaction').value = msg[0].startpoint;
                document.getElementById('ddlEndLoaction').value = msg[0].endpoint;
            }

            for (var cnt = 0; cnt < msg.length; cnt++) {
                $('.chkclass').each(function () {
                    if ($(this).next().next().val() == msg[cnt].id) {
                        $(this).attr("checked", true);
                    }
                });
                var Selected = msg[cnt].Name;
                var Selectedid = msg[cnt].id;
                var label = document.createElement("div");
                var Crosslabel = document.createElement("img");
                Crosslabel.style.float = "right";
                Crosslabel.id = Selectedid;
                Crosslabel.src = "Images/Cross.png";
                Crosslabel.onclick = function () { RemoveClick(this.id); };
                label.id = Selectedid;
                label.innerHTML = Selected;
                label.className = 'divselectedclass';
                Locations = Selected + ",";
                label.onclick = function () { divonclick(this.id); };
                document.getElementById('divselected').appendChild(label);
                label.appendChild(Crosslabel);

                for (var i = 0; i < msg.length; i++) {
                    ArrayPath.push(new google.maps.LatLng(msg[cnt].latitude, msg[cnt].longitude));
                }

                BrachNames += msg[cnt].Name + "-";
                Ranks += j + "-";
                var content = "Branch Name : " + msg[cnt].Name + "\n" + "Rank : " + j;
                var iconpath=""
                if (j == 1 || msg.length==j) {
                    iconpath = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + (j) + "|5ADFB0|000000";
                } else {

                    iconpath = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + (j) + "|FF0000|000000";
                }
                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(msg[cnt].latitude, msg[cnt].longitude),
                    map: map,
                    title: content,
                    icon: iconpath
                });
                var myCenter = new google.maps.LatLng(msg[cnt].latitude, msg[cnt].longitude);
                map.panTo(myCenter);
                j++;
            }
            var line = new google.maps.Polyline({
                path: ArrayPath,
                strokeColor: '#ff0000',
                strokeOpacity: 2.0,
                strokeWeight: 3
            });
            line.setMap(map);
        }
        function RefreshClick() {
            $('.chkclass').each(function () {
                $(this).attr("checked", false);
            });
            document.getElementById('divselected').innerHTML = "";
            document.getElementById('txtRouteName').value = "";
            document.getElementById('btnSave').value = "Save";
            ClearPolylines();
        }

        function btnRouteAssignDeleteclick() {
            var routename = document.getElementById('txtRouteName').value;
            if (routename == "") {
                alert("Enter Route Name");
                return false;
            }
            var Data = { 'op': 'btnRoutesDeleteClick', 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    bindmanageroutes();
                    RefreshClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(Data, s, e);
        }
        function btnRouteAssignSaveclick() {
            var routename = document.getElementById('txtRouteName').value;
            if (routename == "") {
                alert("Enter Route Name");
                return false;
            }
            var selectedplant = document.getElementById('ddlselectplant');
            var plantsno = selectedplant.value;
            if (plantsno == "Select Plant") {
                alert("Please select Plant Name");
                return false;
            }
            var div = document.getElementById('divselected');
            var divs = div.getElementsByTagName('div');
            var divArray = [];
            var ddlStartLoaction = document.getElementById('ddlStartLoaction').value;
            if (ddlStartLoaction == "Select Start Location") {
                alert("Please Select Start Location");
                return false;
            }
//            divArray.push(ddlStartLoaction);
            for (var i = 0; i < divs.length; i += 1) {
                divArray.push(divs[i].id);
            }
            var ddlEndLoaction = document.getElementById('ddlEndLoaction').value;
            if (ddlEndLoaction == "Select End Location") {
                alert("Please Select End Location");
                return false;
            }
//            divArray.push(ddlEndLoaction);
            if (divArray.length == 0) {
                alert("Please select Branches");
                return false;
            }
            var btnsave = document.getElementById('btnSave').value;

            var Data = { 'op': 'btnRoutesSalesSaveClick', 'routename': routename, 'plantsno': plantsno, 'data': divArray, 'btnsave': btnsave, 'refno': refno, 'StartLoaction': ddlStartLoaction, 'EndLoaction': ddlEndLoaction };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    bindmanageroutes();
                    RefreshClick();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            CallHandlerUsingJson(Data, s, e);

        }
        function btnRouteAssignRefreshclick() {
            RefreshClick();
        }
//        function LocationChange(location) {
//            alert(location);
//        }
        function bindmanageroutes() {
            var data = { 'op': 'updateroutesAssigntogrid' };
            var s = function (msg) {
                if (msg) {
                    bindingmanageroutes(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

            callHandler(data, s, e);
        }
        var refno = "";
        var allroutesdata;
        function bindingmanageroutes(databind) {
            RefreshClick();
            allroutesdata = databind;
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
        function fillLocation(msg) {
            document.getElementById("ddlStartLoaction").options.length = null;
            var ddlStartLoaction = document.getElementById('ddlStartLoaction');
            var length = ddlStartLoaction.options.length;
            ddlStartLoaction.options.length = null;
            var opt1 = document.createElement('option');
            opt1.innerHTML = "Select Start Location";
            ddlStartLoaction.appendChild(opt1);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].Name != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].Name;
                    opt.value = msg[i].id;
                    ddlStartLoaction.appendChild(opt);
                }
            }

            document.getElementById("ddlEndLoaction").options.length = null;
            var ddlEndLoaction = document.getElementById('ddlEndLoaction');
            var length = ddlEndLoaction.options.length;
            ddlEndLoaction.options.length = null;
            var opt1 = document.createElement('option');
            opt1.innerHTML = "Select End Location";
            ddlEndLoaction.appendChild(opt1);
            for (var i = 0; i < msg.length; i++) {
                if (msg[i].Name != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[i].Name;
                    opt.value = msg[i].id;
                    ddlEndLoaction.appendChild(opt);
                }
            }
        }
        function ddlselectplant_selectionchanged() {
            var selectedplant = document.getElementById('ddlselectplant').value
            if (selectedplant == "") {
                selectedplant = "Select All";
            }
            document.getElementById('divchblroutes').innerHTML = "";
            var plants = [];
            for (var i = 0; i < allbranchesdata.length; i++) {
                if (typeof allbranchesdata[i] === "undefined" || allbranchesdata[i].PlantName == "" || allbranchesdata[i].PlantName == null) {
                }
                else {
                    var tplantname = allbranchesdata[i].PlantName;
                    var tplantsno = allbranchesdata[i].PlantSno;

                    if (selectedplant != "Select All") {
                        if (tplantsno == selectedplant) {
                            tplantname = tplantname.replace(/[^a-zA-Z0-9]/g, '');
                            var exists = plants.indexOf(tplantname);
                            if (exists == -1) {
                                var plantname = allbranchesdata[i].PlantName;
                                plantname = plantname.replace(/[^a-zA-Z0-9]/g, '');
                                plants.push(plantname);
                                $("#divchblroutes").append("<div id='div" + plantname + "' class='divcategory'>");
                            }
                        }
                    }
                    else {
                        tplantname = tplantname.replace(/[^a-zA-Z0-9]/g, '');
                        var exists = plants.indexOf(tplantname);
                        if (exists == -1) {
                            var plantname = allbranchesdata[i].PlantName;
                            plantname = plantname.replace(/[^a-zA-Z0-9]/g, '');
                            plants.push(plantname);
                            $("#divchblroutes").append("<div id='div" + plantname + "' class='divcategory'>");
                        }
                    }
                }
            }
            fillLocation(allbranchesdata);
            for (var p = 0; p < plants.length; p++) {
                $("#div" + plants[p] + "").append("<div class='titledivcls'><table style='width:100%;'><tr><td style='width: 120px;'><h2 class='unitline'>" + plants[p] + "</h2></td><td></td><td style='padding-right: 20px;vertical-align: middle;'><span class='iconminus' title='Hide' onclick='minusclick(this);'></span></td></tr></table></div>");
                $("#div" + plants[p] + "").append("<ul id='ul" + plants[p] + "' class='ulclass'>");
                for (var i = 0; i < allbranchesdata.length; i++) {
                    var tplantname = allbranchesdata[i].PlantName;
                    tplantname = tplantname.replace(/[^a-zA-Z0-9]/g, '');
                    if (typeof allbranchesdata[i] === "undefined" || allbranchesdata[i].Name == "" || allbranchesdata[i].Name == null) {
                    }
                    else {
                        if (plants[p] == tplantname) {
                            var label = document.createElement("span");
                            var hidden = document.createElement("input");
                            hidden.type = "hidden";
                            hidden.name = "hidden";
                            hidden.value = allbranchesdata[i].id;
                            var checkbox = document.createElement("input");
                            checkbox.type = "checkbox";
                            checkbox.name = "checkbox";
                            checkbox.value = "checkbox";
                            checkbox.id = "checkbox";
                            checkbox.className = 'chkclass';
                            checkbox.onclick = 'checkclick();';
                            document.getElementById('ul' + plants[p]).appendChild(checkbox);
                            label.innerHTML = allbranchesdata[i].Name;
                            document.getElementById('ul' + plants[p]).appendChild(label);
                            document.getElementById('ul' + plants[p]).appendChild(hidden);
                            document.getElementById('ul' + plants[p]).appendChild(document.createElement("br"));
                        }
                    }
                }
            }
            TabclassClick();

            $("#grd_routesmangelist").jqGrid("clearGridData");
            var newarray = [];
            var Headarray = [];
            var headdatacol = allroutesdata[1];
            var datacol = allroutesdata;
            var selectedplant = document.getElementById('ddlselectplant').value
            for (var Booking in allroutesdata) {
                if (selectedplant != "Select All") {
                    if (selectedplant == allroutesdata[Booking].PlantSno) {
                        newarray.push({ 'Sno': newarray.length + 1, 'Route Name': allroutesdata[Booking].RouteName, 'RefNo': allroutesdata[Booking].SNo, 'Plant Name': allroutesdata[Booking].PlantName, 'Plant RefNo': allroutesdata[Booking].PlantSno });
                    }
                }
                else {
                    newarray.push({ 'Sno': newarray.length + 1, 'Route Name': allroutesdata[Booking].RouteName, 'RefNo': allroutesdata[Booking].SNo, 'Plant Name': allroutesdata[Booking].PlantName, 'Plant RefNo': allroutesdata[Booking].PlantSno });
                }
            }
            $("#grd_routesmangelist").jqGrid({
                datatype: "local",
                height: '130',
                width: '350',
                //overflow-x:'auto',
                overflow: 'auto',
                shrinkToFit: true,
                colNames: Headarray,
                colModel: [{ name: 'Sno', index: 'invdate', width: 50, sortable: false, align: 'center' },
        { name: 'Route Name', index: 'invdate', width: 270, sortable: false, align: 'center' },
         { name: 'RefNo', index: 'invdate', width: 70, sortable: false, align: 'center',hidden:true },
         { name: 'Plant Name', index: 'invdate', width: 170, sortable: false, align: 'center' },
         { name: 'Plant RefNo', index: 'invdate', width: 120, sortable: false, align: 'center', hidden: true}],
                rowNum: 10,
                rowList: [5, 10, 30],
                // rownumbers: true,
                gridview: true,
                loadonce: true,
                pager: "#page4",
                caption: "Routes Manage"
            }).jqGrid('navGrid', '#page4', { edit: false, add: false, del: false, search: false, refresh: false });
            var mydata = newarray;
            for (var i = 0; i <= mydata.length; i++) {

                jQuery("#grd_routesmangelist").jqGrid('addRowData', i + 1, mydata[i]);
            }
            $("#grd_routesmangelist").jqGrid('setGridParam', { ondblClickRow: function (rowid, iRow, iCol, e) {

                var routename = $('#grd_routesmangelist').getCell(rowid, 'Route Name');
                var PlantName = $('#grd_routesmangelist').getCell(rowid, 'Plant Name');
                var PlantRefNo = $('#grd_routesmangelist').getCell(rowid, 'Plant RefNo');
                document.getElementById('divselected').innerHTML = "";
                document.getElementById('txtRouteName').value = routename;
                document.getElementById('ddlselectplant').value = PlantRefNo;
                ddlselectplant_selectionchanged(PlantRefNo);
                document.getElementById('btnSave').value = "MODIFY";

                refno = $('#grd_routesmangelist').getCell(rowid, 'RefNo');
                var data = { 'op': 'updatedivselectedsales', 'refno': refno };
                var s = function (msg) {
                    if (msg) {
                        if (msg.length > 0) {
                            gridselctionchanged(msg);
                        }
                        else {
                            alert("There are no locations added for this route");
                        }
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                callHandler(data, s, e);
            }
            });
        }

   </script>
    <script type="text/javascript">
        var map;
        function initialize() {
            var mapOptions = {
                zoom: zoomLevel,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById('googleMap'),
      mapOptions);
            google.maps.event.addListener(map, 'zoom_changed', function () {
                zoomLevel = map.getZoom();
            });
        }
      
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script type="text/javascript">
//        function Addtomap() {
//            var data = { 'op': 'GetRouteValues', 'Locations': Locations };
//            var s = function (msg) {
//                if (msg) {
//                    BindLocationtoMap(msg);
//                }
//                else {
//                }
//            };
//            var e = function (x, h, e) {
//            };
//            callHandler(data, s, e);
//        }
//        function BindLocationtoMap(data) {
//            var map = new google.maps.Map(document.getElementById('googleMap'), {
//                zoom: 3,
//                center: new google.maps.LatLng(17.445974, 80.150965),
//                mapTypeId: google.maps.MapTypeId.ROADMAP
//            });
//            var ArrayPath = [];
//            for (var i = 0; i < data.length; i++) {
//                ArrayPath.push(new google.maps.LatLng(data[i].latitude, data[i].longitude));
//            }
//            var path = ArrayPath;
//            var line = new google.maps.Polyline({
//                path: path,
//                strokeColor: '#ff0000',
//                strokeOpacity: 2.0,
//                zoom:7,
//                strokeWeight: 3
//            });
//            line.setMap(map);
//            var j = 1;
//            BrachNames = "";
//            Ranks = "";
//            Sno = "";
//            for (var i = 0; i < data.length; i++) {
//                BrachNames += data[i].BranchName + "-";
//                Ranks += j + "-";
//                Sno += data[i].Sno + "-";
//                var content = "Branch Name : " + data[i].BranchName + "\n" + "Rank : " + j;
//                marker = new google.maps.Marker({
//                    position: new google.maps.LatLng(data[i].latitude, data[i].longitude),
//                    map: map,
//                    title: content
//                });
//                j++;
//            }
//        }
        var BrachNames = "";
        var Ranks = "";
        var Sno = "";
        function ChangeButtonText() {
            $('#btnRouteSave').val("Edit");
            Addtomap();
        }

        function ClearPolylines() {
            var map = new google.maps.Map(document.getElementById('googleMap'), {
                zoom: zoomLevel,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
            google.maps.event.addListener(map, 'zoom_changed', function () {
                zoomLevel = map.getZoom();
            });
            var ArrayPath = [];
            var path = ArrayPath;
            var line = new google.maps.Polyline({
                path: path,
                strokeColor: '#ff0000',
                strokeOpacity: 2.0,
                zoom: 3,
                strokeWeight: 3
            });
            line.setMap(map);
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
    <div style="width: 100%; height: 550px; background-color: #fff">
        <div style="width: 100%; height: 100%;">
            <div style="width: 22%; float: left; height: 100%;margin-left:2%;margin-top:1%;">
                <div id="divchblroutes" style="float: left; width: 250px; height: 100%; border: 1px solid gray;
                    overflow: auto;">
                </div>
            </div>
            <div style="width: 30%; float: left;">
                <div style="margin: 10px 0px 10px 0px;">
                    <span>Select Plant</span>
                    <select id="ddlselectplant" style="min-width: 170px;" class="txtsize" onchange="ddlselectplant_selectionchanged();">
                    </select>
                </div>
                <div id="div1" style="width: 400px;">
                    <span>Route Name</span>
                    <input type="text" id="txtRouteName" class="txtsize" placeholder="Enter Route Name" />
                </div>
                <%--<div style="width: 100%;">--%>
                    <div style="margin: 10px 0px 10px 0px;">
                        <select id="ddlStartLoaction" class="txtsize" style="min-width: 170px;">
                        </select>
                    </div>
                    <div>
                        <div id="divselected" style="float: left; width: 340px; height: 200px; border: 1px solid gray;
                            overflow: auto;">
                        </div>
                        <br />
                        <br />
                        <br />
                        <div style="float: left;">
                            <input type="button" class="btnUp" onclick="btnUpClick();" /><br />
                            <input type="button" class="btnDown" onclick="btnDownClick();" />
                        </div>
                    </div>
                    <div >
                        <select id="ddlEndLoaction" class="txtsize" style="min-width: 170px;margin: 10px 0px 10px 0px;">
                        </select>
                    </div>
                    <div>
                        <input type="button" id="btnSave" value="Save" class="SaveButton" onclick="btnRouteAssignSaveclick();" />
                        <input type="button" id="btnDelete" value="Delete" class="SaveButton" onclick="btnRouteAssignDeleteclick();" />
                        <input type="button" id="btnRefresh" value="Refresh" class="SaveButton" onclick="btnRouteAssignRefreshclick();" />
                    </div>
                    <br />
               <%-- </div>--%>
                <div style="padding-left: 0%;">
                    <div id="div_routesmgnt" style="width: 90%;">
                        <table id="grd_routesmangelist">
                        </table>
                       <%-- <div id="page4">
                        </div>--%>
                    </div>
                    <%--<table id="routemangt_table">
                        <tr>
                            <td>
                                
                            </td>
                            <td>
                               
                            </td>
                        </tr>
                    </table>--%>
                </div>
            </div>
            <div style="width: 44%; float: left;margin-top:1%;">
                    <div id="googleMap" style="height: 550px; position: relative; background-color: rgb(229, 227, 223);">
                    </div>
            </div>
        </div>
    </div>
    
</asp:Content>
