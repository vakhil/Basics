<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="VehicleExpences.aspx.cs" Inherits="Deisel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function DeiselClicking() {
            var ddlVehicleNo = document.getElementById('<%= ddlVehicleNo.ClientID %>').value;
            var txtPartNo = document.getElementById('<%= txtPartNo.ClientID %>').value;
            var ddlPartName = document.getElementById('<%= ddlPartName.ClientID %>').value;
            var txtQty = document.getElementById('<%= txtQty.ClientID %>').value;
            var txtUnitPrice = document.getElementById('<%= txtUnitPrice.ClientID %>').value;
            var txtTotalAmount = document.getElementById('<%= txtTotalAmount.ClientID %>').value;
            var txtDate = document.getElementById('<%= txtDate.ClientID %>').value;
            if (ddlVehicleNo == "Select Vehicle No" ) {
                alert("Please Fill VehicleNo");
                return false;
            }
            if (ddlPartName == "Select Part Name" ) {
                alert("Please Fill PartName");
                return false;
            }
            if (txtDate == "" ) {
                alert("Please Enter Date");
                return false;
            }
            if (txtTotalAmount == "") {
                alert("Please Enter TotalAmount");
                return false;
            }
            if (txtQty == "") {
                alert("Please Enter Qty");
                return false;
            }
            if (txtUnitPrice == "") {
                alert("Please Enter feilUnitPriceds");
                return false;
            }
             else {
                return true;
            }
            return true;
        }
    </script>
    <style type="text/css">
        .ddldropdown
        {
            width: 210px;
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <br />
    <br />
    <br />
    <br />
    <div class="shell">
        <div style="width:60%;">
            <ul id="submenulist" style="color: Red; background-color: transparent;">
                <li><a id="A1" href="VehicleExpences.aspx" style="color: Red; text-decoration: none;
                    font-size: 12px; font-weight: bold;" runat="server">Vehicle Expenses</a></li>
                <li><a style="font-size: 18px; font-weight: normal; color: Gray;">|</a></li>
                <li><a id="A2" href="Expensesmanage.aspx" style="color: Gray; font-size: 12px; text-decoration: none;
                    font-weight: bold;" runat="server">Expenses Manage</a></li>
            </ul>
        </div>
        <div>
        <br />
        <br />
        </div>
        <div style="padding-left: 50px;">
            <table align="center" style="height: 105px;">
                <tr>
                    <td>
                        Vehicle No
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVehicleNo" CssClass="ddldropdown" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" placeholder="Select Date" CssClass="txtsize" runat="server"></asp:TextBox>
                        <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                            TargetControlID="txtDate" Format="d/M/yyyy HH:mm">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Part Name
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPartName" CssClass="ddldropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPartName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Part No
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartNo" placeholder="Enter Part No" CssClass="txtsize" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty
                    </td>
                    <td>
                        <asp:TextBox ID="txtQty" placeholder="Enter Qty" CssClass="txtsize" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Unit Price
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnitPrice" placeholder="Enter Unit Price" CssClass="txtsize"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Total Amount
                    </td>
                    <td>
                        <asp:TextBox ID="txtTotalAmount" placeholder="Enter Total Amount" CssClass="txtsize"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <asp:Button ID="btn_DeiselVal_Add" CssClass="ContinueButton" runat="server" OnClientClick="return DeiselClicking();"
                            Text="Add" Width="71px" Height="25px" OnClick="btn_DeiselVal_Add_Click" />
                        <asp:Button ID="btn_DeiselVal_Del" runat="server" CssClass="ContinueButton" Text="Del"
                            OnClientClick="return confirm('Are you sure you want to delete?');" Width="71px"
                            Height="25px" OnClick="btn_DeiselVal_Del_Click" />
                        <asp:Button ID="btn_DeiselVal_Refresh" runat="server" CssClass="ContinueButton" Text="Refresh"
                            Width="71px" Height="25px" OnClick="btn_DeiselVal_Refresh_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStatus" ForeColor="Red" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <table align="center">
                <tr>
                    <td>
                        <div style="height: 305px; overflow: auto;">
                            <asp:GridView ID="grdVehicleExpences" runat="server" AutoGenerateSelectButton="True"
                                CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="grdDeiselValue_SelectedIndexChanged">
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
</asp:Content>
