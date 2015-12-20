<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Tools.aspx.cs" Inherits="Tools" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .googleMapcls
        {
            width: 940px;
            height: 420px;
            position: relative;
            overflow: hidden;</style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script src="js/JTemplate.js?v=1001" type="text/javascript"></script>
    <script src="DropDownCheckList.js?v=1001" type="text/javascript"></script>
    <script type="text/javascript">
        var map;
        var geocoder;
        var marker;
        function initialize() {
            var myLatlng = new google.maps.LatLng(17.497535, 78.408622);
            geocoder = new google.maps.Geocoder();
            var myOptions = {
                zoom: 8,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("googleMap"), myOptions);

            marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                title: "Your location"
            });
            google.maps.event.addListener(marker, "dragend", function (event) {

                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                document.getElementById('<%= txtLatitude.ClientID %>').value = lat;
                document.getElementById('<%= txtLongitude.ClientID %>').value = lng;
                var location = new google.maps.LatLng(lat, lng);
                getAddress(location);
            });
            function getAddress(latLng) {
                geocoder.geocode({ 'latLng': latLng },
      function (results, status) {
          if (status == google.maps.GeocoderStatus.OK) {
              if (results[0]) {
                  document.getElementById('<%= txtLocation.ClientID %>').value = results[0].formatted_address;
              }
              else {
                  document.getElementById('<%= txtLocation.ClientID %>').value = "No results";
              }
          }
          else {
              document.getElementById('<%= txtLocation.ClientID %>').value = status;
          }
      });
            }
        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script type="text/javascript">
        $(function () {
            $('#btnFix').click(function (e) {
                deleteOverlays();
                var myLatlng = new google.maps.LatLng(17.497535, 78.408622);
                var myOptions = {
                    zoom: 8,
                    center: myLatlng,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                }
                map = new google.maps.Map(document.getElementById("googleMap"), myOptions);
                var txtLocation = document.getElementById('<%= txtLocation.ClientID %>').value;
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({ 'address': txtLocation }, function (results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        var latitude = results[0].geometry.location.lat();
                        var longitude = results[0].geometry.location.lng();
                        document.getElementById('<%= txtLatitude.ClientID %>').value = latitude;
                        document.getElementById('<%= txtLongitude.ClientID %>').value = longitude;
                        var locatipon = new google.maps.LatLng(latitude, longitude);
                        marker = new google.maps.Marker({
                            draggable: true,
                            position: locatipon,
                            map: map,
                            title: "Your location"
                        });
                        map.panTo(locatipon);
                        google.maps.event.addListener(marker, "dragend", function (event) {
                            var lat = event.latLng.lat();
                            var lng = event.latLng.lng();
                            document.getElementById('<%= txtLatitude.ClientID %>').value = lat;
                            document.getElementById('<%= txtLongitude.ClientID %>').value = lng;
                            var location = new google.maps.LatLng(lat, lng);
                            getAddress(location);
                        });
                        function getAddress(latLng) {
                            geocoder.geocode({ 'latLng': latLng },
      function (results, status) {
          if (status == google.maps.GeocoderStatus.OK) {
              if (results[0]) {
                  document.getElementById('<%= txtLocation.ClientID %>').value = results[0].formatted_address;
              }
              else {
                  document.getElementById('<%= txtLocation.ClientID %>').value = "No results";
              }
          }
          else {
              document.getElementById('<%= txtLocation.ClientID %>').value = status;
          }
      });
                        }
                    } else {
                    }
                });
            });
            $('#btnClear').click(function (e) {
                document.getElementById('<%= txtLocation.ClientID %>').value = "";
                document.getElementById('<%= txtLatitude.ClientID %>').value = "";
                document.getElementById('<%= txtLongitude.ClientID %>').value = "";
                document.getElementById('<%= txtKMs.ClientID %>').value = "";
                deleteOverlays();
            });

            $('#Button1').click(function (e) {
                deleteOverlays();
                var txtLocation = document.getElementById('<%= txtLocation.ClientID %>').value;
                var txtKMs = document.getElementById('<%= txtKMs.ClientID %>').value;
                if (txtLocation == "") {
                    alert("Please Enter Location");
                    return false;
                }
                if (txtKMs == "") {
                    alert("Please Enter KMs");
                    return false;
                }

                var latt = document.getElementById('<%= txtLatitude.ClientID %>').value;
                var long = document.getElementById('<%= txtLongitude.ClientID %>').value;
                var Nokm = document.getElementById('<%= txtKMs.ClientID %>').value;
                var data = { 'op': 'getNearestVehicle', 'latt': latt, 'long': long, 'Nokm': Nokm };
                var s = function (msg) {
                    if (msg) {
                        BindVehicles(msg);
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                    // $('#BookingDetails').html(x);
                };
                callHandler(data, s, e);
            });
            function deleteOverlays() {
                clearOverlays();
                LocationsArray = [];
            }
            function clearOverlays() {
                setAllMap(null);
            }
            function setAllMap(map) {
                for (var i = 0; i < LocationsArray.length; i++) {
                    LocationsArray[i].setMap(map);
                }
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
            var vehlat;
            var vehlong;
            var vehicleno;
            function calcRoute(Lat1, Long1, VehicleID) {
                vehlat = Lat1;
                vehlong = Long1;
                vehicleno = VehicleID;
                var origin1 = new google.maps.LatLng(Lat1, Long1);
                var latitude = document.getElementById('<%= txtLatitude.ClientID %>').value;
                var longitude = document.getElementById('<%= txtLongitude.ClientID %>').value;
                var destinationA = new google.maps.LatLng(latitude, longitude);
                var service = new google.maps.DistanceMatrixService();
                service.getDistanceMatrix({
                    origins: [origin1],
                    destinations: [destinationA],
                    travelMode: google.maps.TravelMode.DRIVING,
                    unitSystem: google.maps.UnitSystem.METRIC,
                    avoidHighways: false,
                    avoidTolls: false
                }, callback);
            }

            function callback(response, status) {
                if (status != google.maps.DistanceMatrixStatus.OK) {
                    alert('Error was: ' + status);
                } else {
                    var origins = response.originAddresses;
                    var destinations = response.destinationAddresses;
                    vehdistance = response.rows[0].elements[0].distance.text;
                    duration = response.rows[0].elements[0].duration.text;
                    var Nokm = document.getElementById('<%= txtKMs.ClientID %>').value;
                    var distance = vehdistance.split(' ')[0];
                    if (Math.ceil(distance) < Math.ceil(Nokm)) {
                        var location = new google.maps.LatLng(vehlat, vehlong);
                        placeMarker(location, vehicleno, vehdistance, duration);
                    }
                }
            }
            var interval;
            function BindVehicles(data) {
                interval = setInterval(function () { vehupdate(data) }, 200);
            }
            var vehicledata = 0;
            function vehupdate(data) {
                if (vehicledata < data.length) {
                    //                for (var vehicledata in data) {
                    var vehicleno = data[vehicledata].Vehicleno;
                    var latitude = data[vehicledata].latitude;
                    var longitude = data[vehicledata].longitude;
                    var Distance = data[vehicledata].Distance;
                    var ExpectedTime = data[vehicledata].ExpectedTime;
                    calcRoute(latitude, longitude, vehicleno);
                    //                    break;
                    //                    placeMarker(location, vehicleno, Distance, ExpectedTime);
                    //                }
                    vehicledata++;
                }
                else {
                    vehicledata = 0;
                    clearInterval(interval);
                }
            }
            var LocationsArray = new Array();
            function placeMarker(location, vehicleno, Distance, ExpectedTime) {
                var marker = new google.maps.Marker({
                    position: location,
                    map: map,
                    center: location,
                    zoom: 10,
                    icon: 'Vehicletypes/greenCar7.png',
                    title: 'vehicleno:' + vehicleno + "\n" + 'Distance:' + Distance + "\n" + 'ExpectedTime:' + ExpectedTime
                });
                LocationsArray.push(marker);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <br />
    <br />
    <br />
    <div class="shell">
        <div class="LoadingPanel">
            <asp:UpdateProgress ID="apLoadingPanel" runat="server">
                <ProgressTemplate>
                    <div style="text-align: center; color: red;">
                        <img src="Images/25-1.gif" style="height: 31px; width: 32px" />
                        <strong><span>Please Wait ......</span></strong>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <asp:UpdatePanel ID="uppanel" runat="server">
            <ContentTemplate>
                <table align="center" style="height: 80px;">
                    <tr>
                        <td>
                            <span style="color: Orange; font-size: 24px;">Finding Nearest Vehicle</span>
                        </td>
                    </tr>
                </table>
                <table align="center" style="height: 80px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocation" CssClass="txtsize" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLatitude" CssClass="txtsize" Enabled="false" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <input type="button" id="btnFix" value="Fix" class="ContinueButton" style="height: 24px;
                                font-size: 16px;" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtLongitude" Enabled="false" CssClass="txtsize" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblKMs" runat="server" Text="No Of KMs"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKMs" CssClass="txtsize" runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <input type="button" id="Button1" value="Find" style="width: 100px; height: 24px;
                                font-size: 16px;" class="ContinueButton" />
                            <%--  </td>
              <td>--%>
                            <input type="button" id="btnClear" value="Clear" style="width: 100px; height: 24px;
                                font-size: 16px;" class="ContinueButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table align="center">
            <tr>
                <td style="width: 199px">
                    <div id="googleMap" class="googleMapcls">
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
