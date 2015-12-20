<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Tripend.aspx.cs" Inherits="Tripend" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
    function ValidateDates() {
        var txtFromDate = document.getElementById('<%=txtFromDate.ClientID %>').value;
        var txtEnddate = document.getElementById('<%=txtEnddate.ClientID %>').value;
        if (txtFromDate = "") {
            alert("Please Select FromDate");
            return false;
        }
        if (txtEnddate == "") {
            alert("Please Select Enddate");
            return false;
        }
    }
</script>
    <style type="text/css">
        ul#menulist
        {
            background: none repeat scroll 0 0 #ECECEC;
            margin: 0;
            padding: 7px 20px;
        }
        ul#menulist li
        {
            font-size: 12px;
            padding-bottom: 2px;
            margin: 1px 0px 1px 2px;
            color: #000000;
            line-height: 17px;
            padding: 2px 5px 2px 5px;
            border-bottom: #efefef 1px dotted;
            list-style-type: none;
            text-align: right;
            display: inline;
        }
    </style>
    <br />
    <br />
    <br />
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
    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
        <ContentTemplate>
            <div style="width: 100%; padding-left: 36%;">
                <div id="submenu" style="background-color: #FFFFFF; color: White;" runat="server">
                    <ul id="menulist" style="background-color: transparent; color: White;">
                        <li><a id="A1" href="TripAssignMent.aspx" style="color: Gray; text-decoration: none;
                            font-size: 16px; font-weight: bold;" runat="server">Trip Configuration</a></li>
                        <li><a style="font-size: 18px; font-weight: normal;">|</a></li>
                        <li><a id="A3" href="Tripend.aspx" style="color: Red; font-size: 16px; text-decoration: none;
                            font-weight: bold;" runat="server">TripEnd</a></li>
                    </ul>
                </div>
            </div>
            <br />
            <div>
                <table align="center">
                    <tr>
                        <td>
                            <label>
                                From Date</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" class="txtsize" runat="server"  placeholder="Enter From Date"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtFromDate"
                                Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*****"
                                ControlToValidate="txtFromDate">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <label>
                                End Date</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEnddate" class="txtsize" runat="server" placeholder="Enter End Date"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtEnddate"
                                Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*****"
                                ControlToValidate="txtEnddate">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                          <asp:Button ID="btnGenerate" runat="server" Text="Generate" CausesValidation="false" CssClass="ContinueButton"
                                OnClick="btnGenerate_OnClick" OnClientClick="ValidateDates();"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Trip Name</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTripName" class="txtsize" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*****"
                                ControlToValidate="txtTripName">
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <label>
                                Assign Date</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssigndate" class="txtsize" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtAssigndate"
                                Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*****"
                                ControlToValidate="txtAssigndate">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Complete Date</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompleteDate" class="txtsize" runat="server"></asp:TextBox>
                            <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtCompleteDate" Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*****"
                                ControlToValidate="txtCompleteDate">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Status</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" class="txtsize" runat="server">
                                <asp:ListItem Value="A">Assigned</asp:ListItem>
                                <asp:ListItem Value="B">Cancelled</asp:ListItem>
                                <asp:ListItem Value="C">Completed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnTripend" runat="server" Text="Save" CssClass="ContinueButton"
                                OnClick="btnTripend_OnClick" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblmsg" runat="server" Text="" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 100%; padding-left: 36%;">
                <br />
                <div style="height: 280px; overflow: auto;">
                    <asp:GridView ID="grdTripend" runat="server" AutoGenerateSelectButton="True" CellPadding="4"
                        ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="grdTripend_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
