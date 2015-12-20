<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="POI_Management.aspx.cs" Inherits="POI_Management" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .googleMapcls
        {
            width: 750px;
            height: 420px;
            position: relative;
            overflow: hidden;
        }
        </style>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false"></script>
    <script type="text/javascript">
      function confirmation(){
          
      if(confirm('Do you want to delete the selected locations ?'))
      { 
      window.location.href="exporttoxl_utility.ashx";  
      return true;
      }
      else{ 
       return false;
       }
      }

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
        function replaceQueryString(url, param, value) {
            var re = new RegExp("([?|&])" + param + "=.*?(&|$)", "i");
            if (url.match(re))
                return url.replace(re, '$1' + param + "=" + value + '$2');
            else
                return url + '&' + param + "=" + value;
        }
    </script>
    <script type="text/javascript">
       
    </script>
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
    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
        <ContentTemplate>
            <table style="height: 120px; width: 100%;">
            <tr> <td colspan="2"><table><tr>
                                <td nowrap align="left" valign="top">
                                    <asp:Label ID="Label3" runat="server" Text="Add Group Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_groupname" runat="server"></asp:TextBox>
                                   
                                </td>
                                  <td>
                                  <asp:Button ID="btn_save" Text="Save" runat="server" CssClass="ContinueButton"
                                        OnClick="BtnCategory_Save_Click" Height="25px" />
                                </td>
                                  <td>
                                  <asp:Button ID="Button4" Text="Clear" runat="server" CssClass="ContinueButton"
                                        OnClick="BtnMyLocatoinRefresh_Click" Height="25px" />
                                </td>
                                  <td>
                                  <asp:Button ID="Button2" Text="Apply Delete to Selected" runat="server" CssClass="ContinueButton"
                                        OnClick="Btndelete_all_Click" OnClientClick="return confirmation();" Height="25px" />
                                </td>
                                  <td>
                                 <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                                 <a id="a_export_excel" href="exporttoxl_utility.ashx">Export to XL</a>
                                </td>
                            </tr>
                            <tr><td colspan="5"><asp:Label ID="lblMylocmsg" runat="server" ForeColor="Red" Text=""></asp:Label></td></tr></table></td></tr>
                 
        
                <tr>
                <td style="width: 25%;vertical-align:top;" >
                        
                            <asp:GridView ID="list_groups" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="True"
                                OnRowDataBound="list_Groups_OnRowDataBound" OnSelectedIndexChanged="GridView3_SelectedIndexChanged">
                                 <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />

                            </asp:GridView>

                </td>
                    <td style="width: 75%;">
                        <div style=" overflow: auto;">
                            <asp:GridView ID="grdMylocation" runat="server"  CellPadding="4" AllowSorting="true"
                                ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="grdMylocation_SelectedIndexChanged"
                               >
                                <Columns>
                                
                                <asp:TemplateField HeaderText="Select">
    <ItemTemplate>
       <asp:CheckBox ID="chkSelect" runat="server" />
    </ItemTemplate>
    <HeaderTemplate>
    </HeaderTemplate>
   </asp:TemplateField>
                                </Columns>
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
</asp:Content>
