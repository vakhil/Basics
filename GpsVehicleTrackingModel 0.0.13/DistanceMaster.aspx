<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="DistanceMaster.aspx.cs" Inherits="DistanceMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
<style type="text/css">
.dropbox
{
    width:202px;
    height:25px;
    border:1px solid gray;
    border-radius:4px 4px 4px 4px;
}
</style>
    <script type="text/javascript">
        function isNumericKey(e) {
            var charInp = window.event.keyCode;
            if (charInp > 31 && (charInp < 48 || charInp > 57) && charInp != 46) {
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:UpdateProgress ID="updateProgress1" runat="server" >
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0;
                    right: 0; left: 0; z-index: 9999; background-color: #FFFFFF; opacity: 0.7;">
                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="thumbnails/loading.gif"
                        Style="padding: 10px; position: absolute; top: 40%; left: 40%; z-index: 99999;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="updatepanel1" runat="server" >
        <ContentTemplate>
            <div style="padding-left: 150px;">
                <div>
                    <table align="center">
                        <tr>
                         <td>
                                <label>
                                    Group</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_group" runat="server" AutoPostBack="True" CssClass="dropbox"
                                    OnSelectedIndexChanged="ddlgroup_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                            </td>
                            <td>
                                <label>
                                    From Branch</label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlfromlocation" runat="server" AutoPostBack="True" CssClass="dropbox"
                                    OnSelectedIndexChanged="ddlfromlocation_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    From Location
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtFrombranch" runat="server" Text='<%# Eval("frombranch")%>' ReadOnly="true" Enabled="false" BackColor="White"
                                        BorderStyle="none" BorderWidth="0px" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    To Location
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTobranch" runat="server" Text='<%# Eval("tobranch")%>' ReadOnly="true" Enabled="false" BackColor="White"
                                        BorderStyle="none" BorderWidth="0px" Width="200px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Distance
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtkms" runat="server" Width="200px" Text='<%# Eval("kms")%>' onkeypress="return isNumericKey(event);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    Expected Time
                                </HeaderTemplate>
                                <ItemTemplate>
                                <asp:TextBox ID="txttime" runat="server" Width="200px" Text='<%# Bind("time", "{0:t}") %>'></asp:TextBox>
                                    <%--<asp:TextBox ID="txttime" runat="server" Width="200px" Text='<%# Eval("time")%>'></asp:TextBox>--%>
                                    <%--Text='<%# Bind("StartTime", "{0:t}") %>'--%>
                                 <asp:MaskedEditExtender ID="meeStartTime" runat="server" AcceptAMPM="false" MaskType="Time"
                                        Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError"
                                        ErrorTooltipEnabled="true" UserTimeFormat="None" TargetControlID="txttime"
                                        InputDirection="LeftToRight" AcceptNegative="Left" AutoComplete="true" ClearMaskOnLostFocus="true">
                                    </asp:MaskedEditExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <asp:Button ID="btnsave" runat="server" OnClick="btnsave_click" Text="Save" Visible="false"
                    Width="200px" Height="30px" BackColor="#444" ForeColor="#ffffff" Font-Bold="false"
                    Font-Size="13px" />
                <br />
            </div>
            <div style="padding-left:200px;">
                <br />
                <asp:Label ID="lbl_Status" runat="server" ForeColor="Red" Text=""></asp:Label>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
