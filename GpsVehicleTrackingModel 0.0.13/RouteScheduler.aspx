<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RouteScheduler.aspx.cs" Inherits="RouteScheduler" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <script type="text/javascript">
        function ValidateRoute() {
            var RouteName = document.getElementById('<%= ddlRouteName.ClientID %>').value
            if (RouteName == "0") {
                alert("Select Route Name");
                return false;
            }
        }
    </script>
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
    <table align="center">
        <tr>
            <td>
                <span style="font-size: 30px; color: Orange;">Route Scheduler</span>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="updRoute" runat="server">
        <ContentTemplate>
            <div>
                <table align="center">
                    <tr>
                        <td>
                            <label>
                                Route Name</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRouteName" CssClass="ddldrop" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlRouteName_OnSelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Select Route Name</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                   <%-- <tr>
                    <td>
                    <asp:TextBox ID="TextBox2" runat="server" Width="180"></asp:TextBox>
                    <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="TextBox2"
                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="RightToLeft"
                        ErrorTooltipEnabled="True" ClearTextOnInvalid="true" ClearMaskOnLostFocus="true" />
                    </td>
                    </tr>--%>
                </table>
                <table align="center">
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            From Branch Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFrombranch" runat="server" Text='<%# Eval("FromBranchName")%>'
                                                ReadOnly="true" BorderStyle="none" Enabled="false" BackColor="White" BorderWidth="0px" Width="120"></asp:TextBox>
                                            <asp:TextBox ID="hbnFrombranch" runat="server" Visible="false" Text='<%# Eval("FromBranchID")%>'
                                                ReadOnly="true" BorderStyle="none" BorderWidth="0px" Width="80"></asp:TextBox>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <headertemplate>
                                            To Branch Name
                                        </headertemplate>
                                            <itemtemplate>
                                            <asp:TextBox ID="txtTobranch" runat="server" Text='<%# Eval("ToBranchName")%>' ReadOnly="true"
                                                BorderStyle="none" BorderWidth="0px" BackColor="White" Enabled="false" Width="120"></asp:TextBox>
                                                  <asp:TextBox ID="hbnTobranch" runat="server" Visible="false" Text='<%# Eval("ToBranchID")%>'
                                                ReadOnly="true" BorderStyle="none" BorderWidth="0px" Width="80"></asp:TextBox>
                                                                                     </itemtemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Time
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txttime" runat="server" Text='<%# Eval("Time")%>' Width="200px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <table align="center">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ContinueButton" OnClick="btnSave_OnClick"
                                OnClientClick="return ValidateRoute();" />
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <asp:Label ID="lblmsg" runat='server' Text="" ForeColor="Red"></asp:Label>
                    </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
