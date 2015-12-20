<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TripAssignMent.aspx.cs" Inherits="TripAssignMent" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="Js/jquery-1.4.4.js?v=3000" type="text/javascript"></script>
    <script src="Js/JTemplate.js?v=3000" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#divclose').click(function () {
                $('#divRouteAssign').css('display', 'none');
            });
        });
        function btnSelectRouteclick() {
            //            document.getElementById('lblmsg').innerHTML = "";
            var txtTripName = document.getElementById('<%=txtTripName.ClientID %>').value;
            var ddlVehicleno = document.getElementById('<%=ddlVehicleno.ClientID %>').value;
            //            var txtAdate = document.getElementById('txtAdate').value;
            document.getElementById('txtTripNo1').innerHTML = txtTripName;
            document.getElementById('txtVehicleNo1').innerHTML = ddlVehicleno;
            //            document.getElementById('txtAssigndate1').innerHTML = txtAdate;
            if (txtTripName == "") {
                alert("Please Enter Trip Name");
                return false;
            }
            if (ddlVehicleno == "") {
                alert("Please Select Vehicleno");
                return false;
            }
            FillRoutes();
            $('#divTrip').css('display', 'block');
            $('#divTrip').setTemplateURL('Trip.htm');
            $('#divTrip').processTemplate();
            $('#divRouteAssign').css("display", "block");
        }
        function ddlRouteNameChange() {
            var RoteName = document.getElementById('ddlRouteName').value;
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
//            $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
            callHandler(data, s, e);
        }
        function SelectionTrip() {
            $('#tripsave').css('display', 'block');
//            fillvehicleno();
            FillRoutes();
            document.getElementById('ddlRouteName').value = '<%=Session["status"] %>';
        }
        function btnTripSaveClick() {

            $('#tripsave').css('display', 'block');
            document.getElementById('<%=txtRouteName.ClientID %>').value = document.getElementById('ddlRouteName').value;
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
//            var result = [];
//            for (var key in msg[0].Locations) {
//                if (msg[0].Locations.hasOwnProperty(key)) {
//                    result.push({ key: key, value: msg[0].Locations[key] });
//                }
//            }
//            var veh1 = document.getElementById('ddlLocations');
//            for (var i = 0; i < result.length; i++) {
//                var opt = result[i];
//                var el = document.createElement("option");
//                el.textContent = opt.key;
//                el.value = opt.value;
//                veh1.appendChild(el);
//            }
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
    <br />
    <br />
    <br />
    <div style="padding-left: 36%;">
        <div id="submenu" style="background-color: #FFFFFF; color: White;" runat="server">
            <ul id="menulist" style="background-color: transparent; color: White;">
                <li><a id="A1" href="TripAssignMent.aspx" style="color: Red; text-decoration: none;
                    font-size: 16px; font-weight: bold;" runat="server">Trip Configuration</a></li>
                <li><a style="font-size: 18px; font-weight: normal;">|</a></li>
                <li><a id="A3" href="Tripend.aspx" style="color: Gray; font-size: 16px; text-decoration: none;
                    font-weight: bold;" runat="server">TripEnd</a></li>
            </ul>
        </div>
    </div>
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
    <br />
    <asp:UpdatePanel ID="updRoute" runat="server">
                            <ContentTemplate>
    <div>
        <table align="center">
            <tr>
                <td>
                    <label>
                        Trip Name</label>
                </td>
                <td>
                    <input type="text" id="txtTripName" placeholder="Enter Trip Name" class="txtsize" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        Vehicle No</label>
                </td>
                <td>
                <asp:DropDownList ID="ddlVehicleno" runat="server" class="ddldrop" >
                </asp:DropDownList>
                   <%-- <select id="ddlVehicleno" class="ddldrop" >
                    </select>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        Start Time</label>
                </td>
                <td>
                <asp:TextBox ID="txtstartTime" runat="server" Width="200px" Text="07:30"  CssClass="txtsize" ></asp:TextBox>
                <asp:MaskedEditExtender ID="meeStartTime" AcceptAMPM="false" runat="server"  MaskType="Time"
                    Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtstartTime"
                    InputDirection="LeftToRight" AcceptNegative="Left" AutoComplete="true" ClearMaskOnLostFocus="true">
                </asp:MaskedEditExtender>
               <%--     <input type="time" id="txtstartTime"  class="txtsize" value="" runat="server" />--%>
                    
                </td>
            </tr>
            <tr>
                <td>
                    <label>
                        End Time</label>
                </td>
                <td>
                <asp:TextBox ID="txtendtime" runat="server" Width="200px" Text="10:30"  CssClass="txtsize" ></asp:TextBox>
                <asp:MaskedEditExtender ID="meeendtime" AcceptAMPM="false" runat="server"  MaskType="Time"
                    Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                    ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txtendtime"
                    InputDirection="LeftToRight" AcceptNegative="Left" AutoComplete="true" ClearMaskOnLostFocus="true">
                </asp:MaskedEditExtender>
                    <%--<input type="time" id="txtendtime" class="txtsize" value="10:30" runat="server" />--%>
                </td>
                <td>
                <asp:CheckBox ID="ckbRepeat" Text="IsRepeat" runat="server" />
                </td>
            </tr>
            <tr>
             <td>
                    <label>
                      Actual Kms</label>
                </td>
                <td>
                <asp:TextBox ID="txtKms" runat="server" Width="200px" placeholder="Enter Actual Kms"  CssClass="txtsize" ></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtKms">
                                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtKms" 
                                          runat="server" ErrorMessage="Numeric values only." Display="Dynamic" ValidationExpression="^\d+(\.\d\d)?$"></asp:RegularExpressionValidator>
               </td>
            </tr>
              <tr>
             <td>
                    <label>
                      Empty Kms</label>
                </td>
                <td>
                <asp:TextBox ID="txt_extrakms" runat="server" Width="200px" placeholder="Enter Empty Kms"  CssClass="txtsize" ></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtKms">
                                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txt_extrakms" 
                                          runat="server" ErrorMessage="Numeric values only." Display="Dynamic" ValidationExpression="^\d+(\.\d\d)?$"></asp:RegularExpressionValidator>
               </td>
            </tr>
              <tr>
                                        <td style="height:40px">
                                              <label>
                      Charge Per Km</label>
                                        </td>
                      <td>
                <asp:TextBox ID="txt_charge" runat="server" Width="200px" placeholder="Enter Charge Per Km"  CssClass="txtsize" ></asp:TextBox>
                                             <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="txt_charge" 
                                          runat="server" ErrorMessage="Numeric values only." Display="Dynamic" ValidationExpression="^\d+(\.\d\d)?$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
            <tr>
                <td>
                    <label>
                        Status</label>
                </td>
                <td>
                    <select id="ddlStatus" class="ddldrop" runat="server">
                    <option >Active</option>
                    <option>Inactive</option>
                    </select>
                </td>
            </tr>
               <tr>
                <td>
                    <label>
                        Palnt Name</label>
                </td>
                <td>
                <asp:DropDownList ID="ddlPlantName" runat="server" class="ddldrop" >
                
                </asp:DropDownList>
                   </td>
            </tr>
            <tr>
                <td>
                    <label>
                        Route Name</label>
                </td>
                <td>
                    <input type="button" value="Select Route" id="btnSelectRoute" onclick="btnSelectRouteclick();"
                        class="ContinueButton" />
                </td>
                <td>
                    <input type="text" id="txtRouteName" class="txtsize" value="" readonly="readonly"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td id="tripsave" style="display: none;" >
                    <asp:Button ID="btnTripSave" runat="server" CssClass="ContinueButton" Text="Save"
                        OnClick="btnTripSave_OnClick" />
                      
                </td>
            </tr>
            <tr>
            <td></td>
            <td></td>
            <td>
                <asp:Button ID="btnRefresh" runat="server" CssClass="ContinueButton" Text="Refresh"
                        OnClick="btnRefresh_OnClick" />
                        </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblmsg" Text="" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <table align="center">
            <tr>
                <td>
                    <div style="height: 280px; overflow: auto;">
                        <asp:GridView ID="grdTrip" runat="server" AutoGenerateSelectButton="True" CellPadding="4"
                            ForeColor="#333333" GridLines="None"  
                            OnSelectedIndexChanged="grdTrip_SelectedIndexChanged" 
                            onrowdatabound="grdTrip_RowDataBound">
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
    <div id="divRouteAssign" class="pickupclass" style="text-align: center; height: 100%;
        width: 100%; position: absolute; display: none; left: 0%; top: 0%; z-index: 99999;
        background: rgba(192, 192, 192, 0.7);">
        <div id="divRoute" style="border: 5px solid #A0A0A0; position: absolute; top: 10%;
            background-color: White; left: 20%; right: 20%; width: 50%; height: 70%; -webkit-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            -moz-box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65); box-shadow: 1px 1px 10px rgba(50, 50, 50, 0.65);
            border-radius: 10px 10px 10px 10px;">
            <br />
            <br />
            <table>
                <tr>
                    <td>
                        <label>
                            Trip No</label>
                    </td>
                    <td>
                        <span id="txtTripNo1" style="font-size: 12px; color: Red;"></span>
                    </td>
                    <td>
                        <label>
                            Vehicle No</label>
                    </td>
                    <td>
                        <span id="txtVehicleNo1" style="font-size: 12px; color: Red;"></span>
                    </td>
                    <%--<td>
                        <label>
                            Assign Date</label>
                    </td>
                    <td>
                        <span id="txtAssigndate1" style="font-size: 12px; color: Red;"></span>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        <label>
                            Route Name</label>
                    </td>
                    <td>
                        <select id="ddlRouteName" class="ddldrop" onchange="ddlRouteNameChange();">
                        </select>
                    </td>
                </tr>
                <tr>
                  <%--  <td>
                        <label>
                            Locations</label>
                    </td>
                    <td>
                        <select id="ddlLocations" class="ddldrop">
                        </select>
                    </td>--%>
                    <td>
                        <input type="button" value="Add" id="btnAdd" onclick="btnTripAddClick();" style="display:none;" class="ContinueButton" />
                    </td>
                </tr>
            </table>
            <div id="divTrip" style="height: 70%; overflow: auto;">
            </div>
            <table>
                <tr>
                    <td>
                    </td>
                    <td>
                        <input type="button" value="OK" id="btnSave" onclick="btnTripSaveClick();" class="ContinueButton"
                            style="width: 100px;" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divclose" style="width: 25px; top: 8.5%; right: 29%; position: absolute;
            z-index: 99999; cursor: pointer;">
            <img src="Images/Close.png" alt="close" />
        </div>
    </div>
</asp:Content>
