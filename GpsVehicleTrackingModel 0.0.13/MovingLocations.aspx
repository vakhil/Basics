<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MovingLocations.aspx.cs" Inherits="MovingLocations" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" runat="server" 
    contentplaceholderid="ContentPlaceHolder1">
    <br />
    <br />
    <br />
 <div class="shell">
 <div style="padding-left:10px;">
  <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <table align="center" style="width:500px;" >
                            <tr>
                            
                                <td >
                                    <asp:RadioButtonList ID="rb_ML_vehicletype" Visible="false" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblUnAuthorisedManagement_SelectedIndexChanged"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem>Groups</asp:ListItem>
                                        <asp:ListItem Selected="True">Vehicles</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <table align="center" style="height:150px;">
                                        <tr>
                                            <td style="width:150px;">
                                                select
                                            </td>
                                            <td  >
                                                <asp:DropDownList ID="ddl_ML_VehicleNo" runat="server" CssClass="ddldrop">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td >
                                                Type
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_ml_type" runat="server" AutoPostBack="True" 
                                                    OnSelectedIndexChanged="ddl_ml_type_SelectedIndexChanged" CssClass="ddldrop">
                                                    <asp:ListItem>Moving Loc</asp:ListItem>
                                                    <asp:ListItem>Economic State</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Min Speed
                                            </td>
                                            <td >
                                                <asp:TextBox ID="txt_ml_MinSpeed" CssClass="txtsize" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Max Speed
                                            </td>
                                            <td >
                                                <asp:TextBox ID="txt_ml_MaxSpeed" CssClass="txtsize" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                      
                                    </table>
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_ML_Add" runat="server" CssClass="ContinueButton" OnClick="btn_ML_Add_Click"
                                                    Text="Add" Width="71px" Height="25px" />
                                                <asp:Button ID="btn_ML_Delete" runat="server" CssClass="ContinueButton" OnClick="btn_ML_Delete_Click"
                                                    OnClientClick="return confirm('Are you sure you want to delete?');" Text="Delete" Width="71px" Height="25px" />
                                                <asp:Button ID="btn_ML_Refresh" runat="server" CssClass="ContinueButton" OnClick="btn_ML_Refresh_Click"
                                                    Text="Refresh" Width="71px" Height="25px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                         
                        </table>
                        <table align="center">
                            <tr>
                                <td align="center">
                                <div style="overflow: auto;width:600px;">
                                    <asp:GridView ID="gv_ML_Data" runat="server" CellPadding="4" ForeColor="#333333"
                                        GridLines="None" OnClientClick="return confirm('Are you sure you want to delete?');"
                                        OnSelectedIndexChanged="gv_ML_Data_SelectedIndexChanged" AutoGenerateSelectButton="True">
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>
                </div>
</asp:Content>

