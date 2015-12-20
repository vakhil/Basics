<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TripView.aspx.cs" Inherits="TripView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div id="divTabs" style="width: 100%;padding-left:80%;height:20px;">
</div>
    <div style="width: 100%; overflow: auto; position: relative;">
        <div style="width: 50%; float: left; border: 1px solid black; overflow: auto; position: relative;">
            <div id="divtripview1" style="width: 100%; height: 300px; position: relative; background-color: rgb(229, 227, 223);">
                <asp:GridView ID="GridView0" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                    Width="420px">
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
        <div style="width: 49%; float: right; border: 1px solid black; overflow: auto;">
            <div id="divtripview2" style="width: 100%; height: 300px; position: relative; background-color: rgb(229, 227, 223);">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" CellPadding="4"
                    ForeColor="#333333" GridLines="None" Width="1220px">
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
    </div>
<div style="width: 100%;">
    <div style="width: 50%; float: left;border:1px solid black;overflow:auto;">
        <div id="divtripview3" style="width: 100%; height: 300px; position: relative; background-color: rgb(229, 227, 223);">
        <asp:GridView ID="GridView2" runat="server" AutoGenerateSelectButton="True" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" Width="1220px" >
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
    <div style="width:49%; float: right;border:1px solid black;overflow:auto;">
     <div id="divtripview4" style="width: 100%; height: 300px; position: relative; background-color: rgb(229, 227, 223);">
     <asp:GridView ID="GridView3" runat="server" AutoGenerateSelectButton="True" CellPadding="4"
                                        ForeColor="#333333" GridLines="None" Width="1220px" >
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
</div>

</asp:Content>

