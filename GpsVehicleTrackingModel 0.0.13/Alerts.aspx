<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Alerts.aspx.cs" Inherits="Alerts" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="Js/jquery-1.4.4.js?v=3000" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
          
        });
        
        function ddlRouteNameChange() {
            var RoteName = document.getElementById('ddlRouteName').value;
            //            var Locations = document.getElementById('ddlLocations').value;
            var data = { 'op': 'ddlRouteNameChange', 'RoteName': RoteName };
            var s = function (msg) {
                if (msg) {
                    BindtoGrid(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        var DataTable;
        function BindtoGrid(data) {
            DataTable = data;
            $('#divTrip').setTemplateURL('Trip.htm');
            $('#divTrip').processTemplate(data);
        }
        function FillRoutes() {
            var data = { 'op': 'GetRouteNames' };
            var s = function (msg) {
                if (msg) {
                    BindRouteName(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        }
        function SelectionTrip() {
            $('#tripsave').css('display', 'block');
            FillRoutes();
            document.getElementById('ddlRouteName').value = '<%=Session["status"] %>';
        }
        function btnTripSaveClick() {

            $('#tripsave').css('display', 'block');
            $('#divRouteAssign').css('display', 'none');
        }
        function BindRouteName(msg) {
            document.getElementById('ddlRouteName').options.length = "";
            var veh = document.getElementById('ddlRouteName');
            var length = veh.options.length;
            for (i = 0; i < length; i++) {
                veh.options[i] = null;
            }
            var opt = document.createElement('option');
            opt.innerHTML = "Select Route";
            opt.value = "";
            veh.appendChild(opt);
            for (var i = 0; i < msg[0].RouteNames.length; i++) {
                if (msg[0].RouteNames[i] != null) {
                    var opt = document.createElement('option');
                    opt.innerHTML = msg[0].RouteNames[i];
                    opt.value = msg[0].RouteNames[i];
                    veh.appendChild(opt);
                }
            }
           
        }
        function saveclick() {
            var ddlemail = document.getElementById('<%=ddlSelectEmail.ClientID%>').value;
            if (ddlemail == "" || ddlemail == "Select Email" || ddlemail == "-1") {
                alert("Select Email ID");
                return false;
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
    </script>
    <style type="text/css">
        ul#menulist
        {
            background: none repeat scroll 0 0 #ECECEC;
            margin: 0;
            padding: 7px 20px;
        }
        ul#menulist li
        {
            font-size: 12px;
            padding-bottom: 2px;
            margin: 1px 0px 1px 2px;
            color: #000000;
            line-height: 17px;
            padding: 2px 5px 2px 5px;
            border-bottom: #efefef 1px dotted;
            list-style-type: none;
            text-align: right;
            display: inline;
        }
    </style>
    
    <asp:UpdatePanel ID="updRoute" runat="server">
                            <ContentTemplate>
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
    <div>
        <table align="center">
          
               <tr>
                
                <td style="width:160px;vertical-align: top;">
                
                 <div style="height: 205px; width: 160px; overflow: auto;" class="borderradius">
                                                <asp:CheckBox runat="server" ID="ckbZones" Text="All Plants" AutoPostBack="true"
                                                    OnCheckedChanged="ckbZones_OnCheckedChanged" />
                                                <asp:CheckBoxList ID="chblZones" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chblZones_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </div>
                   </td>
                    <td style="width:160px;vertical-align: top;">
                                          <div style="min-height: 205px;max-height: 405px; overflow: auto;border:1px solid #d5d5d5;border-radius:4px 4px 4px 4px;" >
                                          <asp:CheckBox runat="server" ID="ckballvehicles" Text="All Routes" AutoPostBack="true"
                                                    OnCheckedChanged="ckballvehicles_SelectedIndexChanged" />
                                    <asp:CheckBoxList ID="ckbVehicles" Width="160px" runat="server">
                                    </asp:CheckBoxList>
                                    </div>
                                    </td>
            </tr>
            </table>
           <table align="center">
            <tr>
            <td>
            <label>
                        Email Id</label>
            </td>
            <td>
            <asp:DropDownList ID="ddlSelectEmail" runat="server" class="ddldrop" AutoPostBack="True"  onselectedindexchanged="emailSelected" >
                <asp:ListItem Value="0" Selected="True">Select Email</asp:ListItem>
                </asp:DropDownList>

            </td>
            
            <td>
            <asp:Label ID="lblPersonname" runat="server" Text="Person Name :" Font-Size="12px" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblvehcount" runat="server" Text="___________" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
            </td>
            </tr>
          
            <tr>
            <td></td>
                <td>
                    <asp:Button ID="btnalertSave" runat="server" CssClass="ContinueButton" Text="Save" OnClick="btnalertSave_Click" OnClientClick="saveclick();"/>
                    <asp:Button ID="btnalertDelete" runat="server" CssClass="ContinueButton" Text="Delete" OnClick="btnalertDelete_Click" />
                    <asp:Button ID="btnalertclear" runat="server" CssClass="ContinueButton" Text="Clear" OnClick="btnalertclear_Click" />
                </td>
            </tr>
            
            <tr>
            <td>
            </td>
                <td>
                    <asp:Label ID="lblmsg" Text="" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <table align="center">
            <tr>
                <td>
                    <div style="height: 280px; overflow: auto;">
                        <asp:GridView ID="grdAlerts" runat="server" AutoGenerateSelectButton="True" CellPadding="4" onrowdatabound="grdAlerts_RowDataBound" 
                            ForeColor="#333333" GridLines="None"  OnSelectedIndexChanged="grdAlerts_SelectedIndexChanged" >
                          
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
  
</asp:Content>
