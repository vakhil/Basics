<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RouteManagement.aspx.cs" Inherits="RouteManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckList" Namespace="UNLV.IAP.WebControls" TagPrefix="cc1" %>
<%--<%@ Register Src="GoogleMapForASPNet.ascx" TagName="GoogleMapForASPNet" TagPrefix="uc1" %>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<td>  <asp:Label ID="lblMessage" Font-Size="X-Large" ForeColor="Orange" runat="server" Text=""></asp:Label></td>--%>
    <script type="text/javascript" src="DropDownCheckList.js"></script>
    <script src="js/JTemplate.js" type="text/javascript"></script>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script type="text/javascript">
        $(function () {
            var hidden = false;
            $("#btnClose").click(function (e) {
                if (hidden) {
                    // $(".divvehiclescls").stop().animate({ left: 0 }, 1000);
                    hidden = false;
                    $("#btnClose").attr('title', 'Hide');
                    $("#btnClose").attr('src', "LiveIcons/Left01.png");
                }
                else {
                    //$(".divvehiclescls").css('margin-left', 0);
                    // $(".divvehiclescls").css('margin-right', 0);
                    //$(".divvehiclescls").animate({ left: '-500px' }, 1000);
                    $("#btnClose").attr('title', 'Show');
                    $("#btnClose").attr('src', "LiveIcons/Right01.png");
                    hidden = true;
                }
                $('#cell').toggle();
            });
        });
        
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
              <%Session["ckdvehicles"] ="'+checkedvehicles+'";%>
              var session_value='<%=Session["ckdvehicles"]%>'; 
        }
    </script>
    <script type="text/javascript">
        function Addtomap() {
            var data = { 'op': 'GetRouteValues' };
            var s = function (msg) {
                if (msg) {
                    BindLocationtoMap(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function BindLocationtoMap(data) {
            var map = new google.maps.Map(document.getElementById('googleMap'), {
                zoom: 12,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
            var ArrayPath = [];
            for (var i = 0; i < data.length; i++) {
                ArrayPath.push(new google.maps.LatLng(data[i].latitude, data[i].longitude));
            }
            var path = ArrayPath;
            var line = new google.maps.Polyline({
                path: path,
                strokeColor: '#ff0000',
                strokeOpacity: 2.0,
                zoom: 14,
                strokeWeight: 3
            });
            line.setMap(map);
            var j = 1;
            BrachNames = "";
            Ranks = "";
            Sno = "";
            for (var i = 0; i < data.length; i++) {
                BrachNames += data[i].BranchName + "-";
                Ranks += j + "-";
                Sno += data[i].Sno + "-";
                var content = "Branch Name : " + data[i].BranchName + "\n" + "Rank : " + j;
                marker = new google.maps.Marker({
                    position: new google.maps.LatLng(data[i].latitude, data[i].longitude),
                    map: map,
                    title: content
                });
                j++;
            }
        }
        var BrachNames = "";
        var Ranks = "";
        var Sno = "";
        function ChangeButtonText() {
            $('#btnRouteSave').val("Edit");
             Addtomap();
        }
       
        function ClearPolylines() {
            var map = new google.maps.Map(document.getElementById('googleMap'), {
                zoom: 12,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });
            var ArrayPath = [];
            var path = ArrayPath;
            var line = new google.maps.Polyline({
                path: path,
                strokeColor: '#ff0000',
                strokeOpacity: 2.0,
                zoom: 14,
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
   
    <script type="text/javascript">
        var map;
        function initialize() {
            var mapOptions = {
                zoom: 13,
                center: new google.maps.LatLng(17.445974, 80.150965),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById('googleMap'),
      mapOptions);
        }

        google.maps.event.addDomListener(window, 'load', initialize);
    </script>
 
    <script type="text/javascript">
       
    </script>
    <style type="text/css">
        #maindiv
        {
            height: 100%;
            width: 100%;
            margin: 0px;
            padding: 0px;
            overflow: hidden;
        }
        .gridcls
        {
            text-align: center;
        }
        .divvehiclescls
        {
            border: 1px solid #c0c0c0;
            height: 650px;
            overflow: auto; /*position:absolute;*/
            z-index: 99999;
            background-color: #f4f4f4;
            width: auto;
            opacity: 1.0;
        }
        .googleMapcls
        {
            width: 100%;
            height: 650px; /* position:relative;*/
            overflow: auto;
        }
    </style>
    <div style="width: 100%; height: 100%;">
        <div>
            <asp:UpdateProgress ID="updateProgress1" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                        right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                            Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <div style="width: 100%;">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="updRoute" runat="server">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <div style="display: block; width: 240px; height: 300px; overflow: auto; border: 1px solid gray;">
                                                <asp:CheckBoxList ID="chbllocations" runat="server" Style="display: block;
                                                    width: 240px; height: 500px;"></asp:CheckBoxList>
                                                <%-- <cc1:DropDownCheckList ID="chbllocations" runat="server" BorderColor="black" BorderStyle="Solid"
                                        AutoPostBack="true" ForeColor="#979797" Style="color: #979797" CheckListCssStyle="position:absolute;z-index:99999;overflow: auto; border: 1px solid black; padding: 4px; max-height:300px; background-color: #ffffff;"
                                        DisplayBoxCssStyle="border: 1px solid #000000; cursor: pointer; width:240px; height:30px;z-index:99999;"
                                        Width="160px" TextWhenNoneChecked="Select locations">
                                    </cc1:DropDownCheckList>--%>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnRight" runat="server" Text="" CausesValidation="false" CssClass="btnRight" OnClick="btnRight_OnClick" /><br />
                                            <asp:Button ID="btmLeft" runat="server" Text="" CausesValidation="false" CssClass="btnleft" OnClick="btmLeft_OnClick" />
                                        </td>
                                        <td>
                                            <div style="display: block; width: 240px; height: 300px; overflow: auto; border: 1px solid gray;">
                                                <asp:ListBox ID="chblselected" runat="server" Style="display: block;
                                                    width: 240px; height: 500px;"></asp:ListBox>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnUp" runat="server" Text="" CssClass="btnUp" CausesValidation="false" OnClick="btnUp_OnClick" /><br />
                                            <asp:Button ID="btnDown" runat="server" Text=" " CssClass="btnDown" CausesValidation="false" OnClick="btnDown_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="vertical-align: top; width: 400px; height: 300px;">
                        <div>
                            <div id="googleMap" style="width: 400px; height: 300px; position: relative; background-color: rgb(229, 227, 223);">
                            </div>
                        </div>
                    </td>
                    <br />
                    <td>
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                        <asp:TextBox  ID="txtRouteName" runat="server" class="txtsize" placeholder="Enter Route Name" ></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="***"
                                    ControlToValidate="txtRouteName" >
                                </asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                             <asp:Button ID="btnRouteSave" runat="server" Text="Save" OnClick="btnRouteSave_OnClick" CssClass="ContinueButton"/>
                             <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_OnClick" CssClass="ContinueButton"/>
                             <br />
                             <br />
                             <asp:Label ID="lblmsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                             <br />
                             <br />
                              <span id="Span1" style="color: Red;"></span>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </td>
                </tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
                        <div style="height: 180px; overflow: auto;width:100%;">
                            
                            <asp:GridView ID="grdRouteData" runat="server" AutoGenerateSelectButton="True"
                                CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="grdRouteData_SelectedIndexChanged">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            </asp:GridView>
                        </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </table>
            <%--  </td>
                </tr>
            </table>--%>
        </div>
    </div>
</asp:Content>
