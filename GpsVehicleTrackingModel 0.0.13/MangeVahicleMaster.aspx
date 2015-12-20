<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MangeVahicleMaster.aspx.cs" Inherits="MangeVahicleMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script language="javascript" type="text/javascript">
    function RemoveSpecialChar(txtName) {
        if (txtName.value != '' && txtName.value.match(/^[\w ]+$/) == null) {
            txtName.value = txtName.value.replace(/[\W]/g, '');
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="ContentPlaceHolder1">
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
     <asp:UpdatePanel ID="updVehicleMaster" runat="server">
    <ContentTemplate>
    <div class="shell">
        <table style="padding-top:20px;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblItemType" Text="Item Type"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlItemType" CssClass="ddldrop" runat="server">
                    <asp:ListItem>Vehicle Type</asp:ListItem>
                    <asp:ListItem>Vehicle Make</asp:ListItem>
                    <asp:ListItem>Vehicle Model</asp:ListItem>
                    <asp:ListItem>Scheduled Route</asp:ListItem>
                    <asp:ListItem>Expenses</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
             <td>
                    <asp:Label runat="server" ID="Label1" Text="Item Code"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtItemCode" CssClass="txtsize" placeholder="Enter Item Code" runat="server"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtItemCode"
                                ForeColor="Red" runat="server" ErrorMessage="Enter Item Code"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblItemName" Text="Item Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtItemName" CssClass="txtsize" placeholder="Enter Item Name" runat="server" ></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtItemName"
                                ForeColor="Red" runat="server" ErrorMessage="Enter Item Name"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnAdd" CssClass="ContinueButton" runat="server" Text="Add" 
                        onclick="btnAdd_Click" Width="71px" Height="25px"/>
                    <asp:Button ID="btnRefresh" CssClass="ContinueButton" CausesValidation="false" runat="server" 
                        Text="Reset" onclick="btnRefresh_Click" Width="71px" Height="25px"/>
                    <asp:Button ID="btnDelete" CssClass="ContinueButton" runat="server" 
                        Text="Delete" Width="71px" Height="25px" onclick="btnDelete_Click"/>
                     <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                </td>
            </tr>
            <tr>
            <td></td>
            <td>
                    <asp:Label runat="server" ID="lblSuccess" ForeColor="Red" Text=""></asp:Label>
            
            </td>
            </tr>
        </table>
           <table>
                            <tr>
                                <td style=" width: 100%;" >
                                  <div style="height: 250px;width: 900px; overflow: auto;">
                                    <asp:GridView ID="grdVehicleManage" runat="server" 
                                        AutoGenerateSelectButton="True" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" 
                                        onselectedindexchanged="grdVehicleManage_SelectedIndexChanged">
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
    <br />
    <br />
</asp:Content>

