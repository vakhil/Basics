<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AssignGeofense.aspx.cs" Inherits="AssignGeofense" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" runat="server" 
    contentplaceholderid="ContentPlaceHolder1">
     <br />
    <br />
    <br />
 <div class="shell">
 <div style="padding-left:150px;">
     <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td>
                                    <asp:Label ID="lblSelectVehicles" runat="server" Text="Select Vehicle"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectVehicle" AutoPostBack="true" runat="server" CssClass="ddldrop">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSelectGeofence" runat="server" Text="Select Geofence"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectGeofence" AutoPostBack="true" runat="server" CssClass="ddldrop">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblGeofenceType" runat="server" Text="Geofence Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGeofenceType" AutoPostBack="true" runat="server" CssClass="ddldrop">
                                        <asp:ListItem>In Side</asp:ListItem>
                                        <asp:ListItem>Out Side</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDateofAssign" runat="server" Text="Date Of Assign"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateofAssign" CssClass="txtsize" runat="server"></asp:TextBox><asp:CalendarExtender
                                        ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtDateofAssign" Format="d/M/yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDateofUnAssign" runat="server" Text="Date Of UnAssign"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateofUnAssign" CssClass="txtsize" runat="server"></asp:TextBox><asp:CalendarExtender
                                        ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtDateofUnAssign" Format="d/M/yyyy HH:mm">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                        </table>
                        <table align="center">
                            <tr>
                                <td>
                                    <asp:Button ID="BtnAddAssignGeofence" CssClass="ContinueButton" runat="server" CausesValidation="true"
                                        Text="Save" OnClick="BtnAddAssignGeofence_Click" Width="71px" Height="25px" />
                                    <asp:Button ID="btnDeleteAssignGeofence" runat="server" CssClass="ContinueButton" Text="Delete"
                                        OnClientClick="return confirm('Are you sure you want to delete?');" OnClick="btnDeleteAssignGeofence_Click"  Width="71px" Height="25px"/>
                                    <asp:Button ID="btnRefreshAssignGeofence" runat="server" CssClass="ContinueButton"
                                        CausesValidation="false" Text="Refresh" OnClick="btnRefreshAssignGeofence_Click" Width="71px" Height="25px" />
                                </td>
                            </tr>
                            <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lbl_nofifier" runat="server"  ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                        </table>
                        <div>
                            <table align="center">
                                <tr>
                                    <td style="overflow: auto; width: 100%; height: 160px;">
                                        <asp:GridView ID="GrdAssignGeofence" runat="server" AutoGenerateSelectButton="True"
                                            CellPadding="3" ForeColor="#333333" GridLines="Horizontal" Width="1200px" ShowHeaderWhenEmpty="True"
                                            OnPageIndexChanging="GrdAssignGeofence_PageIndexChanging" OnSelectedIndexChanged="GrdAssignGeofence_SelectedIndexChanged">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>
                </div>
</asp:Content>

