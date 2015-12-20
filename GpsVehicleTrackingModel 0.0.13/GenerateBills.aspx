<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master"
    CodeFile="GenerateBills.aspx.cs" Inherits="GenerateBills" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
        <div style="width: 100%; height: 46px;">
            <div style="width: 150px; float: left; top: 33px; position: absolute; z-index: 99999;
                left: 15px;">
                <input type="radio" name="radio-choice" id="rblPlant" value="Plants" checked="checked" /><label
                    for="rblPlant" style="color: White;">Plants</label>
                <input type="radio" name="radio-choice" id="rblVendors" value="Vendors" /><label
                    for="rblVendors" style="color: White;">Vendors</label>
            </div>
            <div style="width: 50px; float: left; top: 32px; position: absolute; z-index: 99999;
                left: 175px;">
                <img id="btnClose" alt="" src="LiveIcons/Left01.png" title="Hide" style="height: 23px;
                    width: 25px" />
            </div>
        </div>
        <div style="width: 100%;">
            <table>
                <tr>
                    <td id="cell" valign="top" style="border: 1px solid #d5d5d5; background-color: #f4f4f4;
                        height: 650px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table style="width: 300px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblreportname" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="#a9a9a9"
                                                Text=""></asp:Label>
                                            <table style="width: 98%;">
                                                <tr>
                                                    <td style="width: 80px;">
                                                        <asp:Label ID="lblFromDate" runat="server" Text="Date "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="startdate" runat="server" Width="205px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="startdate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="startdate" Format="dd-MM-yyyy HH:mm">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_show" runat="server" Text="Route" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DDL_locations" runat="server" AutoPostBack="True" Height="21px"
                                                            Visible="false" Width="205px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Plant Name</label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_plant" CssClass="txtClass" runat="server" Width="205px"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_plant_selectionchanged">
                                                            <asp:ListItem Value="0" Selected="True">Select Plant</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_route" runat="server" Text="Route"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_routes" runat="server" AutoPostBack="True" Height="21px"
                                                            Width="205px" OnSelectedIndexChanged="ddl_routes_selectindexchanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lbl_trip" runat="server" Text="Trip"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddl_trip" runat="server" AutoPostBack="True" Height="21px"
                                                            Width="205px"  OnSelectedIndexChanged="ddl_trip_selectindexchanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:HiddenField ID="hdnResultValue" Value="0" runat="server" />
                                                    <asp:Button ID="btn_generate" CssClass="ContinueButton" runat="server" Text="Generate"
                                                        Height="25px" Width="100px" OnClick="btn_generate_Click" />
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                                                    <br />
                                                    <asp:Label ID="lbl_nofifier" runat="server" ForeColor="Red"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px">
                                            <asp:Label ID="Label2" runat="server" Text="GPS KMS " Width="100px"></asp:Label>
                                            <asp:Label ID="lbl_gpskms" runat="server" Text="_________" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td style="height: 40px">
                                            <asp:Label ID="Label8" runat="server" Text="EMPTY KMS " Width="100px"></asp:Label>
                                            <asp:Label ID="lbl_emptykms" runat="server" Text="_________" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td style="height: 40px">
                                            <asp:Label ID="Label10" runat="server" Text="GPS+EMPTY KMS " Width="100px"></asp:Label>
                                            <asp:Label ID="lbl_gpsandemptykms" runat="server" Text="_________" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px">
                                            <asp:Label ID="Label3" runat="server" Text="ACTUAL KMS " Width="100px"></asp:Label>
                                            <asp:Label ID="lbl_actualkms" runat="server" Text="_________" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px">
                                            <asp:Label ID="Label6" runat="server" Text="Charge Per KM" Width="100px"></asp:Label>
                                            <asp:Label ID="lbl_charge" runat="server" Text="_________" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px">
                                            <asp:Label ID="Label4" runat="server" Text="Billing KMS  " Width="100px"></asp:Label>
                                            <asp:TextBox ID="txt_billingkms" runat="server" Width="50px" Text="" AutoPostBack="true"
                                                OnTextChanged="txt_billingkmstxtchngd"></asp:TextBox>
                                            <asp:Label ID="Label5" runat="server" Text="KMS"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 40px">
                                            <asp:Label ID="Label7" runat="server" Text="Total Charge" Width="100px"></asp:Label>
                                            <asp:Label ID="lbl_totalcharge" runat="server" Text="_________" ForeColor="Red" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btn_save" CssClass="ContinueButton" runat="server" Text="Save" Height="25px"
                                                Width="100px" OnClick="btn_save_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_savemsg" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" style="width: 100%;">
                        <table>
                            <tr>
                                <td style="width: 100%; vertical-align: top">
                                    <asp:Panel ID="pReports" runat="server">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <table id="divExport" style="width: 100%;" valign="top">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_ReportStatus" runat="server" Text=" "></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 100%;" valign="top">
                                                            <div style="top: 100px; bottom: 0px; overflow: auto;">
                                                                <asp:Panel ID="Panel3" runat="server" Width="100%" Height="30%">
                                                                    <asp:GridView ID="grdReports" runat="server" CellPadding="5" CellSpacing="5" ForeColor="White"
                                                                        GridLines="Horizontal" Width="100%" CssClass="gridcls">
                                                                        <EditRowStyle BackColor="#999999" />
                                                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                                                            Font-Names="Raavi" Font-Size="Small" />
                                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    </asp:GridView>
                                                                    <div runat="server" id="div1">
                                                                    </div>
                                                                    <div runat="server" id="divPieChart1">
                                                                    </div>
                                                                    <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;">
                                                                    </asp:Label>
                                                                    <div runat="server" id="divPieChart">
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
