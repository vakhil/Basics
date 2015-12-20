<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Mylocation.aspx.cs" Inherits="Mylocation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .googleMapcls
        {
            width: 750px;
            height: 420px;
            position: relative;
            overflow: hidden;
        }
        .SaveButton
        {
            background: #2a81d7;
            background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
            background: -webkit-gradient(linear,0% 0,0% 100%,from(#3e9ee5),to(#206bcb));
            border-top: 1px solid #2a73a6;
            border-right: 1px solid #165899;
            border-bottom: 1px solid #07428f;
            border-left: 1px solid #165899;
            -moz-box-shadow: inset 0 1px 0 0 #62b1e9;
            -webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
            box-shadow: inset 0 1px 0 0 #62b1e9;
            color: #FFF;
            cursor: pointer;
            text-decoration: none;
            text-shadow: 0 -1px 1px #1d62ab;
            border-radius: 4px 4px 4px 4px;
            text-align: center;
        }
        .SaveButton:hover
        {
            background: #0066CC;
            background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
            background: -webkit-gradient(linear,0% 0,0% 100%,from(#0066CC),to(#0066CC));
            border-top: 1px solid #2a73a6;
            border-right: 1px solid #165899;
            border-bottom: 1px solid #07428f;
            border-left: 1px solid #165899;
            -moz-box-shadow: inset 0 1px 0 0 #62b1e9;
            -webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
            box-shadow: inset 0 1px 0 0 #0066CC;
            color: #FFF;
            cursor: pointer;
            text-decoration: none;
            text-shadow: 0 -1px 1px #1d62ab;
            border-radius: 4px 4px 4px 4px;
            text-align: center;
        }
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            getgridlocations();
        });

       
        var map;
        var Maptype;
        var zoomLevel = 15;
        function initialize() {

            var lat = 17.497535;
            var lng = 78.408622;

            var latstr = getParameterByName("lat");
            var lngstr = getParameterByName("long");

            if (latstr != "" && lngstr != "") {
                lat = latstr;
                lng = lngstr;
                document.getElementById('txt_Latitude').value = lat;
                document.getElementById('txt_Longitude').value = lng;

            }
            var myLatlng = new google.maps.LatLng(lat, lng);
            Maptype = google.maps.MapTypeId.ROADMAP;
            var myOptions = {
                zoom: 15,
                center: myLatlng,
                mapTypeId: Maptype
            }
            map = new google.maps.Map(document.getElementById("googleMap"), myOptions);

            var marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                title: "Your location"
            });
            markersArray.push(marker);

            google.maps.event.addListener(marker, "dragend", function (event) {

                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                document.getElementById('txt_Latitude').value = lat;
                document.getElementById('txt_Longitude').value = lng;

            });

            google.maps.event.addListener(map, 'maptypeid_changed', function () {
                Maptype = map.getMapTypeId();
            });

            google.maps.event.addListener(map, 'zoom_changed', function () {
                zoomLevel = map.getZoom();
            });
            //            google.maps.event.addListener(marker, 'click', function (event) {

            //            });
            //            var circle = new google.maps.Circle({
            //                map: map,
            //                radius: 16093    // 10 miles in metres fillColor: '#AA0000'
            //               
            //            });
            //            circle.bindTo('center', marker, 'position');

        }

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        function replaceQueryString(url, param, value) {
            var re = new RegExp("([?|&])" + param + "=.*?(&|$)", "i");
            if (url.match(re))
                return url.replace(re, '$1' + param + "=" + value + '$2');
            else
                return url + '&' + param + "=" + value;
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script type="text/javascript">
        function Clicking() {
            var txtlocacationname = document.getElementById('txt_LocationName').value;
            var txtDescription = document.getElementById('txt_Description').value;
            var txtlatitude = document.getElementById('txt_Latitude').value;
            var txtlongitude = document.getElementById('txt_Longitude').value;
            var txtphonenum = document.getElementById('txt_PhoneNumber').value;
            var txtMyLocationRadious = document.getElementById('txt_Mylocation_Radious').value;
            var txtplantname = document.getElementById('ddlPlantName').value;
            var ckplnt = document.getElementById("ckb_isplant").checked;
            if (ckplnt == true) {
                ckbisplant = "1";
            }
            else {
                ckbisplant = "0";
            }
            if (txtlocacationname == "") {
                alert("Please Enter locacation Name");
                return false;
            }
            if (txtDescription == "") {
                alert("Please Enter Description");
                return false;
            }
            if (txtlatitude == "") {
                alert("Please Enter latitude");
                return false;
            }
            if (txtlongitude == "") {
                alert("Please Enter longitude");
                return false;
            }
            if (txtMyLocationRadious == 0) {
                alert("Please Enter Radious");
                return false;
            }
           
            var btnval = document.getElementById('btn_Mylocation_add').value;
            var Data = { 'op': 'MylocationSaveClick', 'txtlocacationname': txtlocacationname, 'txtDescription': txtDescription, 'txtlatitude': txtlatitude, 'txtlongitude': txtlongitude, 'txtMyLocationRadious': txtMyLocationRadious, 'txtplantname': txtplantname, 'txtphonenum': txtphonenum,'ckbisplant':ckbisplant, 'btnval': btnval, 'refno': refno };
            var s = function (msg) {
                if (msg) {
                    alert(msg);
                    getgridlocations();
                    BtnMyLocatoinRefresh_Click();
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(Data, s, e);
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
        var marker;
        var markersArray = [];
        function PointsAdd() {
            var lat = document.getElementById('txt_Latitude').value;
            var log = document.getElementById('txt_Longitude').value;

            var myLatlng = new google.maps.LatLng(lat, log);

            var myOptions = {
                zoom: zoomLevel,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            deleteOverlays();

            //map = new google.maps.Map(document.getElementById("googleMap"), myOptions);
            marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                title: "Your location"
            });
            markersArray.push(marker);

            //            var circle = new google.maps.Circle({
            //                map: map,
            //                radius: rad    // 10 miles in metres fillColor: '#AA0000'

            //            });
            //            circle.bindTo('center', marker, 'position');
            rad = document.getElementById('txt_Mylocation_Radious').value;
            var rrr = parseInt(rad, rrr);
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
            markersArray.push(circle);
            map.panTo(myLatlng);

            google.maps.event.addListener(marker, "dragend", function (event) {

                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                document.getElementById('txt_Latitude').value = lat;
                document.getElementById('txt_Longitude').value = lng;
            });
        }
        var rad = 0;
        function Setradious() {
            deleteOverlays();
            var lat = document.getElementById('txt_Latitude').value;
            var log = document.getElementById('txt_Longitude').value;

            var myLatlng = new google.maps.LatLng(lat, log);
            marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                zoom: zoomLevel,
                title: "Your location"
            });
            map.panTo(myLatlng);
            markersArray.push(marker);
            google.maps.event.addListener(marker, "dragend", function (event) {

                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                document.getElementById('txt_Latitude').value = lat;
                document.getElementById('txt_Longitude').value = lng;
            });
            rad = document.getElementById('txt_Mylocation_Radious').value;
            var rrr = parseInt(rad, rrr);
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
            markersArray.push(circle);
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

        function BtnMyLocatoinRefresh() {
            deleteOverlays();
            var lat = 17.497535;
            var lng = 78.408622;
            var myLatlng = new google.maps.LatLng(lat, lng);
            var marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                title: "Your location"
            });

            google.maps.event.addListener(marker, "dragend", function (event) {

                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                document.getElementById('txt_Latitude').value = lat;
                document.getElementById('txt_Longitude').value = lng;

            });

            markersArray.push(marker);
            map.panTo(myLatlng);
        }
        function getgridlocations() {
            var data = { 'op': 'getgridlocations' };
            var s = function (msg) {
                if (msg) {
                    fillmylocations(msg);
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
        function fillmylocations(allroutesdata) {
            $("#grdMylocation").jqGrid("clearGridData");
            var newarray = [];
            var Headarray = [];
            var headdatacol = allroutesdata[1];
            var datacol = allroutesdata;
            var plants = [];
            var sel = document.getElementById('ddlPlantName');
            var opt = document.createElement('option');
            opt.innerHTML = "Select Plant";
            opt.value = "Select Plant";
            sel.appendChild(opt);
            for (var i = 0; i < allroutesdata.length; i++) {
                if (typeof allroutesdata[i] === "undefined" || allroutesdata[i].BranchID == "" || allroutesdata[i].BranchID == null) {
                }
                else {
                    if (plants.indexOf(allroutesdata[i].BranchID) == -1 && allroutesdata[i].IsPlant=="True") {
                        var plantname = allroutesdata[i].BranchID;
                        var PlantSno = allroutesdata[i].Sno;
                        plants.push(plantname);
                        var opt = document.createElement('option');
                        opt.innerHTML = plantname;
                        opt.value = PlantSno;
                        sel.appendChild(opt);
                    }
                }
                newarray.push({ 'BranchID': allroutesdata[i].BranchID, 'Description': allroutesdata[i].Description, 'Latitude': allroutesdata[i].Latitude, 'Longitude': allroutesdata[i].Longitude, 'PhoneNumber': allroutesdata[i].PhoneNumber, 'Radious': allroutesdata[i].Radious, 'Sno': allroutesdata[i].Sno, 'PlantName': allroutesdata[i].PlantName, 'IsPlant': allroutesdata[i].IsPlant });
            }
            $("#grdMylocation").jqGrid({
                datatype: "local",
                height: '200',
                width: '800',
                overflow: 'auto',
                shrinkToFit: true,
                colNames: Headarray,
                colModel: [{ name: 'BranchID', index: 'invdate', width: 150, sortable: false, align: 'center' },
        { name: 'Description', index: 'invdate', width: 270, sortable: false, align: 'center' },
         { name: 'Latitude', index: 'invdate', width: 150, sortable: false, align: 'center' },
         { name: 'Longitude', index: 'invdate', width: 170, sortable: false, align: 'center' },
         { name: 'PhoneNumber', index: 'invdate', width: 120, sortable: false, align: 'center' },
         { name: 'Radious', index: 'invdate', width: 120, sortable: false, align: 'center' },
         { name: 'Sno', index: 'invdate', width: 120, sortable: false, align: 'center' },
         { name: 'PlantName', index: 'invdate', width: 120, sortable: false, align: 'center' },
         { name: 'IsPlant', index: 'invdate', width: 120, sortable: false, align: 'center'}],
                rowNum: 10,
                rowList: [5, 10, 30],
                // rownumbers: true,
                gridview: true,
                loadonce: true,
                pager: "#page4",
                caption: "MyLocations"
            }).jqGrid('navGrid', '#page4', { edit: false, add: false, del: false, search: false, refresh: false });
            var mydata = newarray;
            for (var i = 0; i <= mydata.length; i++) {
                jQuery("#grdMylocation").jqGrid('addRowData', i + 1, mydata[i]);
            }
            $("#grdMylocation").jqGrid('setGridParam', { ondblClickRow: function (rowid, iRow, iCol, e) {
                document.getElementById('txt_LocationName').value = $('#grdMylocation').getCell(rowid, 'BranchID');
                document.getElementById('txt_Description').value = $('#grdMylocation').getCell(rowid, 'Description');
                document.getElementById('txt_Latitude').value = $('#grdMylocation').getCell(rowid, 'Latitude');
                document.getElementById('txt_Longitude').value = $('#grdMylocation').getCell(rowid, 'Longitude');
                document.getElementById('txt_PhoneNumber').value = $('#grdMylocation').getCell(rowid, 'PhoneNumber');
                document.getElementById('txt_Mylocation_Radious').value = $('#grdMylocation').getCell(rowid, 'Radious');
                document.getElementById('ddlPlantName').value = $('#grdMylocation').getCell(rowid, 'PlantName');
                refno = $('#grdMylocation').getCell(rowid, 'Sno');
                var IsPlant = $('#grdMylocation').getCell(rowid, 'IsPlant');
                if (IsPlant == "True")
                    document.getElementById("ckb_isplant").checked = true;
                else
                    document.getElementById("ckb_isplant").checked = false;
                document.getElementById('btn_Mylocation_add').value = "Edit";
                Setradious();
            }
            });
        }
        function BtnMyLocatoinRefresh_Click() {
            refno = "";
            document.getElementById('txt_LocationName').value = "";
            document.getElementById('txt_Description').value = "";
            document.getElementById('txt_Latitude').value = "";
            document.getElementById('txt_Longitude').value = "";
            document.getElementById('txt_PhoneNumber').value = "";
            document.getElementById('txt_Mylocation_Radious').value = "";
            document.getElementById('ddlPlantName').selectedIndex = 0;
            document.getElementById("ckb_isplant").checked = false;
            document.getElementById('btn_Mylocation_add').value = "Add";
        }
        function btn_MyLocation_Del_Click() {
            if (refno != "") {
                confirm("Do you want to delete this location?");
                {
                    var data = { 'op': 'deletelocation', 'refno': refno };
                    var s = function (msg) {
                        if (msg) {
                            alert(msg);
                            getgridlocations();
                        }
                        else {
                        }
                    };
                    var e = function (x, h, e) {
                    };
                    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

                    callHandler(data, s, e);
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div align="center" style="width: 100%;">
    <table>
        <tr>
            <td style="width: 10px;"></td>
            <td style="width: 20%;">
                <table>
                    <tr>
                        <td nowrap align="left" valign="top">
                            <span>Location Name</span>
                        </td>
                        <td>
                            <input id="txt_LocationName" placeholder="Enter Location Name" type="text" class="txtsize" />
                        </td>
                        <td>
                            <input type="checkbox" id="ckb_isplant" />
                        <td>
                            <span>Isplant</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span>Description</span>
                        </td>
                        <td>
                            <input id="txt_Description" placeholder="Enter Description" type="text" class="txtsize" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span>Latitude</span>
                        </td>
                        <td>
                            <input id="txt_Latitude" placeholder="Enter Latitude" type="text" class="txtsize" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span>Longitude</span>
                        </td>
                        <td>
                            <input id="txt_Longitude" placeholder="Enter Longitude" type="text" class="txtsize" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span>Radius</span>
                        </td>
                        <td>
                            <input id="txt_Mylocation_Radious" placeholder="Enter Radius" type="text" class="txtsize" onclick="Setradious();" />
                            <input id="btnSet" type="button" class="SaveButton" value="Preview" style="width:60px;height:20px;font-size:12px;" onclick="Setradious();" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span>Phone Number</span>
                        </td>
                        <td>
                            <input id="txt_PhoneNumber" placeholder="Enter Phone Number" type="text" class="txtsize" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <span>Plant Name</span>
                        </td>
                        <td>
                            <select id="ddlPlantName" class="txtsize">
                            </select>
                        </td>
                    </tr>
                </table>
                <table align="center">
                    <tr>
                        <td>
                            <input id="btn_Mylocation_add" type="button" class="SaveButton" value="Add" style="width:100px;height:24px;font-size:14px;" onclick="Clicking();" />
                            <input id="BtnMyLocatoinRefresh" type="button" class="SaveButton" value="Delete" style="width:100px;height:24px;font-size:14px;" onclick="btn_MyLocation_Del_Click();" />
                            <input id="btn_MyLocation_Del" type="button" class="SaveButton" value="Refresh" style="width:100px;height:24px;font-size:14px;" onclick="BtnMyLocatoinRefresh_Click();" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="width: 70%;">
                <div id="googleMap" class="googleMapcls">
                </div>
            </td>
        </tr>
    </table>
    </div>
    <div align="center" style="width: 100%;">
        <table id="grdMylocation">
        </table>
    </div>
</asp:Content>
