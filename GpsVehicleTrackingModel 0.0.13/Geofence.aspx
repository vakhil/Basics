<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Geofence.aspx.cs" Inherits="Geofence" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .googleMapcls
        {
            width: 850px;
            height: 420px;
            position: relative;
            overflow: hidden;}
        .style3
        {
            width: 280px;
        }
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script type="text/javascript">
        var map;
        function initialize() {
            var myLatlng = new google.maps.LatLng(17.497535, 78.408622);

            var myOptions = {
                zoom: 8,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("googleMap"), myOptions);

            var marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                title: "Your location"
            });
            google.maps.event.addListener(marker, "dragend", function (event) {

                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                document.getElementById('<%= txt_GeoFence_latitude.ClientID %>').value = lat;
                document.getElementById('<%= txt_Geofence_longitude.ClientID %>').value = lng;
            });
            //            google.maps.event.addListener(marker, 'click', function (event) {

            //            });

        }
        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
    <script type="text/javascript">
        function GeofenceClicking() {
            var txtGeofencename = document.getElementById('<%= txt_Geofencename.ClientID %>').value;
            var txtGeofenceLatitude = document.getElementById('<%= txt_GeoFence_latitude.ClientID %>').value;
            var txtGeofenceLongitude = document.getElementById('<%= txt_Geofence_longitude.ClientID %>').value;
            var txtgeofenceRadious = document.getElementById('<%= txt_Radious.ClientID %>').value;
            if (txtGeofencename == "" ) {
                alert("Please Enter Geofence Name");
                return false;
            }
            if (txtGeofenceLatitude == "" ) {
                alert("Please Enter Latitude");
                return false;
            }
            if (txtGeofenceLongitude == "" || ) {
                alert("Please Enter Longitude");
                return false;
            }
            if (txtgeofenceRadious == "") {
                alert("Please Enter Radious");
                return false;
            } 
            else {
                return true;
            }
            return true;
        }
        function PointsAdd() {
            var lat = document.getElementById('<%= txt_GeoFence_latitude.ClientID %>').value;
            var log = document.getElementById('<%= txt_Geofence_longitude.ClientID %>').value
            var myLatlng = new google.maps.LatLng(lat, log);

            var myOptions = {
                zoom: 8,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("googleMap"), myOptions);

            var marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                title: "Your location"
            });
            google.maps.event.addListener(marker, "dragend", function (event) {

                var lat = event.latLng.lat();
                var lng = event.latLng.lng();
                document.getElementById('<%= txt_GeoFence_latitude.ClientID %>').value = lat;
                document.getElementById('<%= txt_Geofence_longitude.ClientID %>').value = lng;
            });
        }
        var rad = 0;
        function Setradious() {
            var lat = document.getElementById('<%= txt_GeoFence_latitude.ClientID %>').value;
            var log = document.getElementById('<%= txt_Geofence_longitude.ClientID %>').value;

            var myLatlng = new google.maps.LatLng(lat, log);

            var myOptions = {
                zoom: 16,
                center: myLatlng,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(document.getElementById("googleMap"), myOptions);

            marker = new google.maps.Marker({
                draggable: true,
                position: myLatlng,
                map: map,
                zoom: 16,
                title: "Your location"
            });
            rad = document.getElementById('<%= txt_Radious.ClientID %>').value;
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
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table align="left">
        <tr>
            <td align="left" valign="top">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <%--<fieldset style="width: 94%;">
                                            <legend>Geofence</legend>--%>
                            <table>
                                <tr>
                                    <td valign="top" style="width: 290px; height: 150px;">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <%-- <fieldset>--%>
                                                <table style="width: 250px; height: 150px;">
                                                    <tr>
                                                        <td nowrap="nowrap">
                                                            <asp:Label ID="lblGeofencename" runat="server" Text="Geofence Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Geofencename" CssClass="txtsize" placeholder="Enter Geofence Name" MaxLength="35" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style3">
                                                            <asp:Label ID="lblGeofenceLatitude" runat="server" Text="Latitude"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_GeoFence_latitude" CssClass="txtsize" placeholder="Enter Latitude" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style3">
                                                            <asp:Label ID="lblGeofencveLongitude" runat="server" Text="Longitude"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Geofence_longitude" CssClass="txtsize" placeholder="Enter Longitude" runat="server"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style3">
                                                            <asp:Label ID="lblGeofenceRadious" runat="server" Text="Radius"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Radious" MaxLength="8" CssClass="txtsize" placeholder="Enter Radious" runat="server"></asp:TextBox>
                                                       <asp:Button ID="btnSet" Text="Preview" runat="server" OnClientClick="return Setradious();"/>
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                                <table>
                                                    <tr align="center">
                                                        <td>
                                                            <asp:Button ID="BtnSave" runat="server" CssClass="ContinueButton" Text="Save" OnClick="BtnSave_Click"
                                                                OnClientClick="return GeofenceClicking();" Width="71px" Height="25px" />
                                                            <asp:Button ID="btnGeofenceDelete" runat="server" CssClass="ContinueButton" Text="Delete"
                                                                OnClientClick="return confirm('Are you sure you want to delete?');" OnClick="btnGeofenceDelete_Click"
                                                                Width="71px" Height="25px" />
                                                            <asp:Button ID="btnGeofenceRefresh" runat="server" CssClass="ContinueButton" Text="Refresh"
                                                                OnClick="btnGeofenceRefresh_Click" Width="71px" Height="25px" />
                                                        </td>
                                                    </tr>
                                                    <tr><td>
                                                    <asp:Label ID="lblstatus" ForeColor="Red" runat="server" Text=""></asp:Label>
                                                    </td></tr>
                                                </table>
                                                <%-- </fieldset>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td >
                                        <%--    <uc1:GoogleMapForASPNet ID="GoogleMapForASPNet2" runat="server"></uc1:GoogleMapForASPNet>--%>
                                        <div id="googleMap" class="googleMapcls">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <%--</fieldset>--%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 160px; width: 100%">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td style="overflow: auto; width: 100%;" align="center">
                                  <div style="height: 280px; overflow: auto;">
                                    <asp:GridView ID="grdGeofence" runat="server" AutoGenerateSelectButton="True" CellPadding="4"
                                        ForeColor="White" GridLines="None" Width="900px" OnSelectedIndexChanged="grdGeofence_SelectedIndexChanged">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="Black" />
                                        <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
