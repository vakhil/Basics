<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageLogins.aspx.cs" Inherits="Manage_Logins" %>

<%@ Register Assembly="DropDownCheckList" Namespace="UNLV.IAP.WebControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="js/JTemplate.js?v=1001" type="text/javascript"></script>
    <script src="DropDownCheckList.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="Newjs/DropDownCheckList.js"></script>--%>
    <script type="text/javascript">
        $(function () {
            var data = { 'op': 'InitilizeVehiclesreports' };
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
            var vehiclenos = new Array();
            //    for (var grparray in groupsdata) {
            //        var vehicleids = groupsdata[grparray].vehicleno;
            var vehkeys = Object.keys(data);
            vehkeys.forEach(function (veh) {
                vehiclenos.push({ vehicleno: data[veh].vehicleno });
            });
            //    }
            $('#divassainedvehs').css('display', 'block');
//            $('#divassainedvehs').setTemplateURL('GpsLogs.htm');
          //  $('#divassainedvehs').processTemplate(vehiclenos);
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
              <%Session["ckdvehicles"] ="'+checkedvehicles+'";%>
              var session_value='<%=Session["ckdvehicles"]%>'; 
        }
    </script>
    <script type="text/javascript">
        function onallcheck(id) {
            checkedvehicles = [];
            for (var vehicledata in livedata) {
                var vehicleno = livedata[vehicledata].vehiclenum;
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
                        //                    for (var i = checkedvehicles.length - 1; i >= 0; i--) {
                        //                        if (checkedvehicles[i] === vehicle) {
                        //                            checkedvehicles.splice(i, 1);
                        //                        }
                        //                    }
                    }
                }
            }
        }
    </script>
    <style type="text/css">
        .ddldropd
        {
            width: 213px;
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
        <br />
        <br />
        <br />
        <br />
        <div style="height: 500px;" class="borderradius">
                <br />
            <div align="center" style="width: 100%;">
                <span style="color: Orange; font-size: 22px;">Manage Logins</span>
                <br />
            </div>
                <br />
            <div>
                <asp:UpdateProgress ID="updateProgress1" runat="server">
                    <ProgressTemplate>
                        <div style="position: fixed; text-align: center; width: 100%; top: 35%;
                            right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 1.0;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                                Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="width: 100%;">
                            <div style="float: left;width:33%;padding-left:2%;">
                                <table>
                                    <tr>
                                        <td>
                                            <div>
                                                <%--<input type="button" class="btntogglecls" value="" id="btnshowhide" onclick="clicked();" />--%>
                                                <div id="divvehicles"">
                                                    <table border="0" align="center" height="220px">
                                                        <tr>
                                                            <td width="100px">
                                                                <asp:Label ID="lblLoginID" runat="server"> 
                               Login ID:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLoginID" placeholder="Enter Login ID" CssClass="txtsize" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                              <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtLoginID"
                                                               ErrorMessage="Spaces are not allowed!" ValidationExpression="[^\s]+" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                                                    ErrorMessage="*" ControlToValidate="txtLoginID" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblPassword" runat="server"> 
                               Password:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPassword" placeholder="Enter Password" CssClass="txtsize" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                                                    ErrorMessage="*" ControlToValidate="txtPassword" Display="Dynamic"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                                <tr>
                                                            <td>
                                                                <asp:Label ID="Label2" runat="server"> 
                              Mobile Number :</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_mobno" placeholder="Enter Mobile Number" CssClass="txtsize" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                                                    ErrorMessage="*" ControlToValidate="txt_mobno" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator id="RegularExpressionValidator1" ControlToValidate="txt_mobno" ValidationExpression="\d+"
                   Display="Static" EnableClientScript="true" ErrorMessage="Please enter numbers only" runat="server"/>
                                                                               </td>
                                                        </tr>
                                                                <tr>
                                                            <td>
                                                                <asp:Label ID="Label3" runat="server"> 
                               Email ID :</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txt_emailid" placeholder="Enter Email ID" CssClass="txtsize" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                                                                    ErrorMessage="*" ControlToValidate="txt_mobno" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                     ControlToValidate="txt_emailid" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblUserType" runat="server">
                               User Type:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlUserType" CssClass="ddldropd" runat="server">
                                                                    <asp:ListItem Selected="True">Select User Type</asp:ListItem>
                                                                    <asp:ListItem>Admin</asp:ListItem>
                                                                    <asp:ListItem>User</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*"
                                                                    ControlToValidate="ddlUserType" InitialValue="Select User Type">
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSave" Text="Save" CssClass="ContinueButton" runat="server" OnClick="btnSave_Click" />
                                                                <asp:Button ID="btnCancel" CausesValidation="false" CssClass="ContinueButton" Text="Cancel"
                                                                    runat="server" OnClick="btnCancel_Click" />
                                                                <asp:Button ID="btnDelete" Text="Delete" CssClass="ContinueButton" runat="server"
                                                                    OnClick="btnDelete_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table border="0" align="center">
                                                        <tr>
                                                            <td>
                                                                <div style="height: 180px;overflow:auto">
                                                                    <asp:GridView ID="grdManageLogins" runat="server" AutoGenerateSelectButton="True"
                                                                        CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="grdManageLogins_SelectedIndexChanged">
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
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_nofifier" ForeColor="Red" runat="server"> </asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="float: right;width:60%;">
                                <table cellpadding="0" cellspacing="0" >
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
                                            <div style="height: 205px; width: 160px; overflow: auto;" class="borderradius">
                                                <asp:CheckBox runat="server" ID="ckbVehicleTypes" Text="All Vehicle Types" AutoPostBack="true"
                                                 OnCheckedChanged="ckbVehicleTypes_OnCheckedChanged"   />
                                                <asp:CheckBoxList ID="chblVehicleTypes" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chblVehicleTypes_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </div>
                                        </td>
                                        <td style="width:160px;vertical-align: top;">
                                          <div style="min-height: 205px;max-height: 405px; overflow: auto;border:1px solid #d5d5d5;border-radius:4px 4px 4px 4px;" >
                                          <asp:CheckBox runat="server" ID="ckballvehicles" Text="All Vehicles" AutoPostBack="true"
                                                    OnCheckedChanged="ckballvehicles_SelectedIndexChanged" />
                                    <asp:CheckBoxList ID="ckbVehicles" Width="160px" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                                  <asp:Label ID="Label1" runat="server" Text="Vehicles Count :" Font-Size="12px" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lblvehcount" runat="server" Text="0" Font-Bold="true" Font-Size="14px" ForeColor="Red"></asp:Label>
                                      </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
</asp:Content>
