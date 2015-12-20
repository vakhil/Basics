<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="VehicleGroups.aspx.cs" Inherits="VehicleGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" runat="server" 
    contentplaceholderid="ContentPlaceHolder1">
     <br />
    <br />
    <br />
 <div class="shell">
 <div style="padding-left:150px;">

    <table align="center" style="height: 105px;">
                    <tr>
                        <td >
                        <div style="height: 205px;width:200px;border:2px solid gray;padding-left:30px; overflow: auto;">
                                <b class="orange11" style="font-size: 14px; font-weight: bold;">
                                    <asp:Label ID="lblSelectVehicle" runat="server" Text="Select Vehicle"></asp:Label></b>
                                    <asp:CheckBoxList
                                        ID="cblSelectVehicle" runat="server">
                                    </asp:CheckBoxList>
                                    </div>
                        </td>
                        <td>
                            <table align="center">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGroupName" runat="server" Text="Group Name"></asp:Label><asp:TextBox
                                            ID="txtGroupName" MaxLength="35" placeholder="Enter Group Name" CssClass="txtsize" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table align="center">
                                <tr>
                                    <td>
                                        <asp:Button ID="btn_VehicleGroup_Add" CssClass="ContinueButton" runat="server" OnClientClick="return VehicleClicking();"
                                            Text="Add" OnClick="btn_VehicleGroup_Add_Click" Width="71px" Height="25px" />
                                        <asp:Button ID="btn_VehicleGroup_Del" runat="server" CssClass="ContinueButton" Text="Del"
                                            OnClick="btn_VehicleGroup_Del_Click" OnClientClick="return confirm('Are you sure you want to delete?');" Width="71px" Height="25px"/>
                                        <asp:Button ID="btn_VehicleGroup_Refresh" runat="server" CssClass="ContinueButton"
                                            Text="Refresh" OnClick="btn_VehicleGroup_Refresh_Click" Width="71px" Height="25px"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                         <asp:Label ID="lblGroupStatus" ForeColor="Red" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <table align="center">
                    <tr>
                        <td style="overflow: auto">
                         <div style="height: 305px; overflow: auto;">
                            <asp:GridView ID="grdVehicleGroup" runat="server" AutoGenerateSelectButton="True"
                                CellPadding="4" ForeColor="#333333" GridLines="None" Width="424px" OnSelectedIndexChanged="grdVehicleGroup_SelectedIndexChanged">
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

