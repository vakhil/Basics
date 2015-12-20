<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="VehicleMaster.aspx.cs" Inherits="VehicleMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckList" Namespace="UNLV.IAP.WebControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
<style type="text/css">
.txtClass
        {
           width:250px;
           height:30px; 
            font-size:14px;
           border:1px solid gray;
           border-radius: 7px 7px 7px 7px;
        }
        .txtClassforDate
        {
           width:165px;
           height:30px; 
            font-size:14px;
           border:1px solid gray;
           border-radius: 7px 7px 7px 7px;
        }
</style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
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
    <br />
    <br />
    <div class="shell">
        <br />
        <br />
        <asp:UpdatePanel ID="updVehicleMaster" runat="server">
            <ContentTemplate>
                <div style="width: 90%; padding-left: 20px">
                    <table style="width: 89%; height: 250px;">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label3" Text="Vehicle No"></asp:Label>
                            </td>
                            <td>
                             <asp:DropDownList ID="ddlVehicleNo" CssClass="txtClass" runat="server">
                        <asp:ListItem Value="0"  Selected="True">Select Vehicle No</asp:ListItem>
                    </asp:DropDownList>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*"
                                                                ControlToValidate="ddlVehicleNo" InitialValue="Select Vehicle No">
                                                            </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label4" Text="Vehicle Type"></asp:Label>
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddlVehicleType" CssClass="txtClass" runat="server">
                        <asp:ListItem Value="0"  Selected="True">Select Vehicle Type</asp:ListItem>
                    </asp:DropDownList>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*"
                                                                ControlToValidate="ddlVehicleType" InitialValue="Select Vehicle Type">
                                                            </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label7" Text="Driver Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDriverName"  runat="server" placeholder="Enter Driver Name" CssClass="txtClass"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtDriverName" >
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label8" Text="Plant Name"></asp:Label>
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddlPlantName" CssClass="txtClass" runat="server">
                        <asp:ListItem Value="0"  Selected="True">Select Plant Name</asp:ListItem>
                    </asp:DropDownList>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                                                                ControlToValidate="ddlPlantName" InitialValue="Select Plant Name">
                                                            </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label9" Text="Phone Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNumber" placeholder="Enter Phone Number" runat="server" CssClass="txtClass"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtPhoneNumber" >
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label10" Text="Vehicle Make"></asp:Label>
                            </td>
                            <td>
                            <asp:DropDownList ID="cmb_vehmake" CssClass="txtClass" runat="server">
                        <asp:ListItem Value="0"  Selected="True">Select Vehicle Make</asp:ListItem>
                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                             <td>
                                <asp:Label runat="server" ID="Label6" Text="Company ID "></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCompanyID" placeholder="Enter Company ID" runat="server" CssClass="txtClass"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtCompanyID" >
                                </asp:RequiredFieldValidator>
                            </td>
                               <td>
                                <asp:Label runat="server" ID="Label11" Text="Vehicle Model"></asp:Label>
                            </td>
                            <td>
                             <asp:DropDownList ID="cmb_vehmodel" CssClass="txtClass" runat="server">
                        <asp:ListItem Value="0"  Selected="True">Select Vehicle Model</asp:ListItem>
                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label2" Text="Address"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" placeholder="Enter Address" runat="server" CssClass="txtClass"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*"
                                    ControlToValidate="txtAddress" >
                                </asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label12" Text="Capacity"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_capacity" placeholder="Enter Capacity" runat="server" CssClass="txtClass"></asp:TextBox>
                            </td>
                        </tr>
                           <tr>
                            <td>
                                <asp:Label runat="server" ID="Label1" Text="Scheduled Route"></asp:Label>
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddlScheduledRoute" CssClass="txtClass" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlScheduledRoute_OnSelectedIndexChanged">
                        <asp:ListItem Value="0"  Selected="True">Select Route</asp:ListItem>
                    </asp:DropDownList>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*"
                                                                ControlToValidate="ddlScheduledRoute" InitialValue="Select Route">
                                                            </asp:RequiredFieldValidator>
                            </td>
                              <td>
                                <asp:Label runat="server" ID="Label5" Text="Route Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRouteName"  runat="server" CssClass="txtClass" ReadOnly></asp:TextBox>
                            </td>
                        </tr>




                          <tr>
                            <td>
                                <asp:Label runat="server" ID="Label13" Text="RC Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_rcnumber" placeholder="Enter RC Number" runat="server" CssClass="txtClass"></asp:TextBox>
                            </td>
                        </tr>
                          <tr>
                            <td>
                                <asp:Label runat="server" ID="Label15" Text="Insurance Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_insurance" placeholder="Enter Insurance Number" runat="server" CssClass="txtClass"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label16" Text="Insurance Expiration Date "></asp:Label>
                            </td>
                            <td>
                                 <asp:TextBox ID="txt_insuranceexpdate" runat="server" CssClass="txtClass" placeholder="Select Expiration Date"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txt_insuranceexpdate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txt_insuranceexpdate" Format="dd-MM-yyyy HH:mm">
                                                        </asp:CalendarExtender>
                            </td>
                        </tr>
                    <tr>
                            <td>
                                <asp:Label runat="server" ID="Label14" Text="RoadTax Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_RoadTax" placeholder="Enter RoadTax Number" runat="server" CssClass="txtClass"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label17" Text="RoadTax Expiration Date "></asp:Label>
                            </td>
                            <td>
                                 <asp:TextBox ID="txt_RoadTaxempdate" runat="server" CssClass="txtClass" placeholder="Select Expiration Date"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txt_RoadTaxempdate__CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txt_RoadTaxempdate" Format="dd-MM-yyyy HH:mm">
                                                        </asp:CalendarExtender>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label runat="server" ID="Label18" Text="Fitness Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Fitness" placeholder="Enter Fitness Number" runat="server" CssClass="txtClass"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label19" Text="Fitness Expiration Date "></asp:Label>
                            </td>
                            <td>
                                 <asp:TextBox ID="txt_Fitnessexpdate" runat="server" CssClass="txtClass" placeholder="Select Expiration Date"></asp:TextBox>
                                                        <asp:CalendarExtender ID="Fitnessexpdate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txt_Fitnessexpdate" Format="dd-MM-yyyy HH:mm">
                                                        </asp:CalendarExtender>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:Label runat="server" ID="Label20" Text="Pollution Number"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Pollutionnumber" placeholder="Enter Pollution Number" runat="server" CssClass="txtClass"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label21" Text="Pollution Expiration Date "></asp:Label>
                            </td>
                            <td>
                                 <asp:TextBox ID="txt_Pollutionexpdate" runat="server" CssClass="txtClass" placeholder="Select Expiration Date"></asp:TextBox>
                                                        <asp:CalendarExtender ID="txt_Pollutionexpdate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txt_Pollutionexpdate" Format="dd-MM-yyyy HH:mm">
                                                        </asp:CalendarExtender>
                            </td>
                        </tr>




                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" CssClass="ContinueButton" runat="server" Text="Add" OnClick="btnAdd_Click"
                                    Width="71px" Height="25px" />
                                <asp:Button ID="btnRefresh" CssClass="ContinueButton" CausesValidation="false" runat="server"
                                    Text="Reset" OnClick="btnRefresh_Click" Width="71px" Height="25px" />
                                <asp:Button ID="btnDelete" CssClass="ContinueButton" runat="server" Text="Delete"
                                    OnClick="btnDelete_Click" Width="71px" Height="25px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSuccess" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="width: 100%;">
                                <div style="height: 250px; width: 900px; overflow: auto;">
                                    <asp:GridView ID="grdVehicleMaster" runat="server" AutoGenerateSelectButton="True"
                                        CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="grdVehicleMaster_SelectedIndexChanged">
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
    </div>
</asp:Content>
