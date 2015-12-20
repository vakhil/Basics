<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="EmailUser.aspx.cs" Inherits="EmailUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <br />
    <div class="shell" style="padding-left: 10%;">
        <br />
        <asp:UpdatePanel ID="updVehicleMaster" runat="server">
            <ContentTemplate>
                <div>
                    <span style="color: Red; font-size: 18px;">User Email:</span>
                </div>
                <table>
                    <tr>
                        <td>
                            Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Text="" placeholder="Enter Name" CssClass="txtsize"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="***"
                                ControlToValidate="txtName">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Email Id</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" Text="" placeholder="Enter Email Address"
                                CssClass="txtsize"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="***"
                                ControlToValidate="txtEmail">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="Invalid EmailID"
                                ControlToValidate="txtEmail" SetFocusOnError="true" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ForeColor="Red" CssClass="Validation-Msg" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Level</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLevel" runat="server" CssClass="ddldrop">
                                <asp:ListItem Value="1">Level1</asp:ListItem>
                                <asp:ListItem Value="2">Level2</asp:ListItem>
                                <asp:ListItem Value="3">Level3</asp:ListItem>
                                <asp:ListItem Value="4">Level4</asp:ListItem>
                                <asp:ListItem Value="5">Level5</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_OnClick" CssClass="ContinueButton" />
                        </td>
                        <td>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_OnClick"
                                CssClass="ContinueButton" />
                      <%--  </td>
                        <td>--%>
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_OnClick"
                                CssClass="ContinueButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                       <%-- </td>
                        <td>--%>
                            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div style="height: 280px; overflow: auto;">
                    <asp:GridView ID="grdEmail" runat="server" AutoGenerateSelectButton="True" CellPadding="4"
                        ForeColor="#333333" GridLines="None" Width="520px" OnSelectedIndexChanged="grdEmail_SelectedIndexChanged"
                        OnRowDataBound="grdEmail_RowDataBound">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
