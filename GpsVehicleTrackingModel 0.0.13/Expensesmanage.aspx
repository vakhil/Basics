<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Expensesmanage.aspx.cs" Inherits="Expensesmanage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
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
      <div style="width:50%;">
        <ul id="submenulist" style="color: Red; background-color: transparent;">
            <li><a id="A1" href="VehicleExpences.aspx" style="color: Gray; text-decoration: none;
                font-size: 12px; font-weight: bold;" runat="server">Vehicle Expenses</a></li>
            <li><a style="font-size: 18px; font-weight: normal; color: Gray;">|</a></li>
            <li><a id="A2" href="Expensesmanage.aspx" style="color: Red; font-size: 12px;
                text-decoration: none; font-weight: bold;" runat="server">Expenses Manage</a></li>
        </ul>
    </div>
    <div>
    <br />
    <br />
    </div>
    <div style="padding-left: 150px;">
        <table style="height:150px;">
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label1" Text="Part No"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartNo" CssClass="txtsize" placeholder="Enter Part No" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtPartNo"
                        ForeColor="Red" runat="server" ErrorMessage="Enter Part No"></asp:RequiredFieldValidator>
                </td>
              </tr>  <tr>
                <td>
                    <asp:Label runat="server" ID="lblItemName" Text="Part Name"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPartName" CssClass="txtsize" placeholder="Enter Part Name" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtPartName"
                        ForeColor="Red" runat="server" ErrorMessage="Enter Part Name"></asp:RequiredFieldValidator>
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
                        Width="71px" Height="25px" />
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
        </div>
    </div>
</asp:Content>
