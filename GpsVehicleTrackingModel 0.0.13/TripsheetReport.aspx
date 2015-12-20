<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TripsheetReport.aspx.cs"
    MasterPageFile="~/MasterPage.master" Inherits="TripsheetReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/date.format.js" type="text/javascript"></script>
    <script type="text/javascript">
        function exportfn() {
            window.location = "exporttoxl.aspx";
        }

        //------------>Prevent Backspace<--------------------//
        $(document).unbind('keydown').bind('keydown', function (event) {
            var doPrevent = false;
            if (event.keyCode === 8) {
                var d = event.srcElement || event.target;
                if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD'))
            || d.tagName.toUpperCase() === 'TEXTAREA') {
                    doPrevent = d.readOnly || d.disabled;
                } else {
                    doPrevent = true;
                }
            }

            if (doPrevent) {
                event.preventDefault();
            }
        });
    </script>
    <style type="text/css">
        .txtClass
        {
            width: 186px;
            height: 28px;
            padding: 6px 12px;
            font-size: 14px;
            line-height: 1.428571429;
            color: #555;
            vertical-align: middle;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,0.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,0.075);
            -webkit-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        }
        .SaveButton
        {
            background: #2a81d7;
            background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
            background: -webkit-gradient(linear,0% 0,0% 100%,from(#3e9ee5),to(#206bcb));
            border-top: 1px solid #2a73a6;
            border-right: 1px solid #165899;
            border-bottom: 1px solid #07428f;
            border-left: 1px solid #165899;
            -moz-box-shadow: inset 0 1px 0 0 #62b1e9;
            -webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
            box-shadow: inset 0 1px 0 0 #62b1e9;
            color: #FFF;
            cursor: pointer;
            text-decoration: none;
            text-shadow: 0 -1px 1px #1d62ab;
            border-radius: 4px 4px 4px 4px;
            width: 100px;
            height: 30px;
        }
        .SaveButton:hover
        {
            background: #0066CC;
            background: -moz-linear-gradient(0% 100% 90deg,#206bcb,#3e9ee5);
            background: -webkit-gradient(linear,0% 0,0% 100%,from(#0066CC),to(#0066CC));
            border-top: 1px solid #2a73a6;
            border-right: 1px solid #165899;
            border-bottom: 1px solid #07428f;
            border-left: 1px solid #165899;
            -moz-box-shadow: inset 0 1px 0 0 #62b1e9;
            -webkit-box-shadow: inset 0 1px 0 0 #62b1e9;
            box-shadow: inset 0 1px 0 0 #0066CC;
            color: #FFF;
            cursor: pointer;
            text-decoration: none;
            text-shadow: 0 -1px 1px #1d62ab;
            border-radius: 4px 4px 4px 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="updPanel" runat="server">
        <ContentTemplate>
            <div style="height: 100%; width: 100%; border-spacing: 10px;" align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Label">From Date</asp:Label>&nbsp;
                            <asp:TextBox ID="dtp_FromDate" runat="server" CssClass="txtClass"></asp:TextBox>
                            <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="dtp_FromDate"  Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Label">To Date</asp:Label>&nbsp;
                            <asp:TextBox ID="dtp_Todate" runat="server" CssClass="txtClass">
                            </asp:TextBox>
                            <asp:CalendarExtender ID="enddate_CalendarExtender2" runat="server" Enabled="True"
                                TargetControlID="dtp_Todate"  Format="dd-MM-yyyy HH:mm">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="GENERATE" CssClass="SaveButton"
                                OnClick="btn_Generate_Click" />
                         <%--   <asp:Button ID="print_btn" runat="server" Text="PRINT" CssClass="SaveButton"
                                OnClick="btn_Print_Click" />--%>
                              <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <div style="width: 98%; height: 550px; overflow: auto;">
                                <asp:GridView ID="dataGridView1" runat="server" GridLines="Both" CellPadding="3"
                                    RowStyle-Wrap="true" RowStyle-BorderStyle="Solid" RowStyle-BorderColor="#cccccc"
                                    RowStyle-BorderWidth="1px" RowStyle-ForeColor="#454545" RowStyle-HorizontalAlign="Center"
                                    RowStyle-VerticalAlign="Middle" RowStyle-Height="30px" BackColor="#d6dadf" AlternatingRowStyle-BackColor="#f3f4f5"
                                    HeaderStyle-BackColor="#E6CACA" Font-Names="Arial, Helvetica, sans-serif" Font-Size="13px"
                                    AllowSorting="false" AutoGenerateColumns="true">
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:UpdateProgress ID="updateProgress1" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                        right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                        <br />
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="images/Loading.gif" AlternateText="Loading ..."
                            ToolTip="Loading ..." Style="padding: 10px; position: absolute; top: 35%; left: 40%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
