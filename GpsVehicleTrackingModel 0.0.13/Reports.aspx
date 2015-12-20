<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Reports.aspx.cs" Inherits="Reports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckList" Namespace="UNLV.IAP.WebControls" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="DropDownCheckList.js"></script>
    <script src="js/JTemplate.js" type="text/javascript"></script>

     <style>
    .modal-backdrop {
           background-color: rgba(0, 0, 0, 0.61);
           position: absolute;
           top: 0;
           bottom: 0;
           left: 0;
           right: 0;
           display: none;
       }
       .modal {
           width: 500px;
           position: absolute;
           top: 25%;
           z-index: 1020;
           background-color: #FFF;
           border-radius: 6px;
           display: none;
       }
       .modal-header {
           background-color: #333;
           color: #FFF;
           border-top-right-radius: 5px;
           border-top-left-radius: 5px;
       }
       .modal-header h3 {
           margin: 0;
           padding: 0 10px 0 10px;
           line-height: 40px;
       }
       .modal-header h3 .close-modal {
           float: right;
           text-decoration: none;
           color: #FFF;
       }
       .modal-footer {
           background-color: #F1F1F1;
           padding: 0 10px 0 10px;
           line-height: 40px;
           text-align: right;
           border-bottom-right-radius: 5px;
           border-bottom-left-radius: 5px;
           border-top: solid 1px #CCC;
       }
       .modal-body {
           padding: 10px 10px 10px 10px;
       }
    </style>
   <%--  <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript">
        $(function () {
            $("#dialog").dialog({
                autoOpen: false,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $(".opener").click(function () {
                $("#dialog").dialog("open");
            });
        });
       
    </script>



    <script type="text/javascript">
        $(function () {
            var hidden = false;
            $("#btnClose").click(function (e) {
                if (hidden) {
                    hidden = false;
                    $("#btnClose").attr('title', 'Hide');
                    $("#btnClose").attr('src', "LiveIcons/Left01.png");
                }
                else {
                    $("#btnClose").attr('title', 'Show');
                    $("#btnClose").attr('src', "LiveIcons/Right01.png");
                    hidden = true;
                }
                $('#cell').toggle();
            });
        });

        function onallcheck(id) {
            checkedvehicles = [];
            for (var vehicledata in livedata) {
                var vehicleno = livedata[vehicledata].vehicleno;
                var vehicle = $("#" + vehicleno + "");
                if (id.checked == true) {
                    if (typeof vehicle[0] === "undefined") {
                    }
                    else {
                        vehicle[0].checked = true;
                        checkedvehicles.push(vehicleno);
                    }
                }
                else {
                    if (typeof vehicle[0] === "undefined") {
                    }
                    else {
                        vehicle[0].checked = false;
                    }
                }
            }
            //              <%Session["ckdvehicles"] ="'+checkedvehicles+'";%>
            var session_value = '<%=Session["ckdvehicles"]%>';
            document.getElementById('<%=hdnResultValue.ClientID%>').value = session_value;
            var hdnValue = document.getElementById('<%=hdnResultValue.ClientID%>');
        }
        function deletelocationOverlays() {
        }
    </script>
    <script type="text/javascript">

        function NormalOpening() {
            window.open("RouteDrawing.aspx");
            return true;
        }
        function reportview(getid) {
            $("#dialog").dialog("open");
            //  alert("Hello") ;
            var row = $(getid).closest('tr');
            var framestring = "";
            var cells = 0;
            $(row).find('td').each(function () {
                //check if td having empty text
                //                if ($(this).text().trim() == "") {
                //                    //get parent of td and put css
                //                    framestring += $(this).text();
                //                }
                if (cells > 0) {
                    framestring += $(this).html() + "#";
                }
                cells++;
            });
            $("#ui-id-1").html(framestring);
//            for (var i = 0; i < row.cells.length; i++) {
//                framestring += row.cells[i].innerHTML + "#";
//            }
           // alert(framestring);
            return true;
        }
        function SplOpening() {
            window.open('RouteDrawing.aspx');
            return true;
        }
        function XXLOpening() {

            window.open("exporttoxl.aspx", "_blank");
            return true;
        }
    </script>
    <script type="text/javascript">
        $(function () {
            var data = { 'op': 'InitilizeVehiclesreports' };
            //            var data = { 'op': 'InitilizeGroups' };
            var s = function (msg) {
                if (msg) {
                    Groupsfilling(msg);
                }
                else {
                }
            };
            var e = function (x, h, e) {
            };
            callHandler(data, s, e);
        });
        var livedata = [];
        function Groupsfilling(data) {
            livedata = data;
            var vehiclenos = new Array();
            var vehicletypes = new Array();
            var vehkeys = Object.keys(data);
            vehkeys.forEach(function (veh) {
                vehiclenos.push({ vehicleno: data[veh].vehicleno, vehiclemodeltype: data[veh].vehicletype, Routename: data[veh].Routename });
            });
            //    }
            //            $('#divassainedvehs').css('display', 'block');
            //            $('#divassainedvehs').setTemplateURL('GpsLogs.htm');
            //            $('#divassainedvehs').processTemplate(vehiclenos);
        }

        function callHandler(d, s, e) {
            $.ajax({
                url: 'Bus.axd',
                data: d,
                type: 'GET',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                async: true,
                cache: true,
                success: s,
                error: e
            });
        }
    </script>
    <script type="text/javascript">
        var checkedvehicles = [];
        function onvehiclecheck(id, onckvehicleid) {
            for (var i = checkedvehicles.length - 1; i >= 0; i--) {
                if (checkedvehicles[i] === onckvehicleid) {
                    checkedvehicles.splice(i, 1);
                }
            }
            if (id.checked == true) {
                checkedvehicles.push(onckvehicleid);
            }
              <%Session["ckdvehicles"] ="'+checkedvehicles+'";%>
              var session_value='<%=Session["ckdvehicles"]%>'; 
              var hdnValue=document.getElementById('<%=hdnResultValue.ClientID%>');
            document.getElementById('<%=hdnResultValue.ClientID%>').value = session_value;
        }
        //for overcome liveupdate error in reports
        function liveupdate() {
        }   
    </script>
    <script type="text/javascript" language="javascript">
        $(function () {
            $('input[name="radio-choice"]').change(function () {
                var rblauthorizedtype = $(this).next('label').text();
                if (rblauthorizedtype == "Plants") {
                    document.getElementById('divchblvehicles').style.display = "block";
                    document.getElementById('divchblVendors').style.display = "none";
                }
                else if (rblauthorizedtype == "Vendors") {
                    document.getElementById('divchblvehicles').style.display = "none";
                    document.getElementById('divchblVendors').style.display = "block";
                }
                var data = { 'op': 'setauthorizedsession', 'Authorizedtype': rblauthorizedtype };
                var s = function (msg) {
                    if (msg) {
                    }
                    else {
                    }
                };
                var e = function (x, h, e) {
                };
                callHandler(data, s, e);

                function callHandler(d, s, e) {
                    $.ajax({
                        url: 'Bus.axd',
                        data: d,
                        type: 'GET',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        async: true,
                        cache: true,
                        success: s,
                        error: e
                    });
                }
            });
        });
        function bpmouseout() {
            $("#displaydiv").css("display", "none"); $("#displaydiv").html("");
        }
        function bphover(me, vehicleid) {
            var valu = $('#displaydiv');
            $('#displaydiv').css("display", "block");
            var pos = $(me).offset();
            var content = "";
            var timestamp;
            var myCenterZoom;
            for (var vehicledata in livedata) {
                var vehicleno = livedata[vehicledata].vehicleno;
                var Routename = livedata[vehicledata].Routename;
                var vehicle = $("#" + vehicleno + "");
                if (vehicleid == vehicleno) {
                    content = "Vehicle ID : " + vehicleno;
                }
            }
            $("#displaydiv").html(content);

            var top = $(document).scrollTop();
            var tothei = $(document).height();
            var xx = window.screen.availHeight;
            var aa = pos.top;
            var zz = aa + 20;
            if ((top + xx) >= (pos.top + $('#displaydiv').height() + 30 + ($('#displaydiv').height() * 0.5))) {
                $('#displaydiv').css("top", zz).css("left", pos.left + 30);
            }
            else {
                if ((pos.top - $('#displaydiv').height()) < 0) {
                    $('#displaydiv').css("top", "0").css("left", pos.left + 30);
                }
                else {
                    $('#displaydiv').css("top", pos.top - $('#displaydiv').height() - 30).css("left", pos.left + 30);
                }
            }
        }
    </script>
    <script type="text/javascript">
        function PopupClose() {
            $('#cell').toggle();
        }
    </script>
    <style type="text/css">
        #maindiv
        {
            height: 100%;
            width: 100%;
            margin: 0px;
            padding: 0px;
            overflow: hidden;
        }
        .gridcls
        {
            text-align: center;
        }
        .divvehiclescls
        {
            border: 1px solid #c0c0c0;
            height: 650px;
            overflow: auto; /*position:absolute;*/
            z-index: 99999;
            background-color: #f4f4f4;
            width: auto;
            opacity: 1.0;
        }
        .googleMapcls
        {
            width: 100%;
            height: 650px; /* position:relative;*/
            overflow: auto;
        }
        .bpmouseover
        {
            height: 20px;
            width: 200px;
            display: none;
            position: absolute;
            z-index: 99999;
            padding: 10px 5px 5px 15px;
            background-color: #fffffc;
            border: 1px solid #c0c0c0;
            border-radius: 3px 3px 3px 3px;
            box-shadow: 3px 3px 3px rgba(0,0,0,0.3);
            font-family: Verdana;
            font-size: 12px;
            opacity: 1.0;
        }
    </style>
    <div style="width: 100%; height: 100%;">
        <div id="displaydiv" class="bpmouseover">
        </div>
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
        <div style="width: 100%; height: 46px;">
            <div style="width: 150px; float: left; top: 33px; position: absolute; z-index: 99999;
                left: 15px;">
                <input type="radio" name="radio-choice" id="rblPlant" value="Plants" checked="checked" /><label
                    for="rblPlant" style="color: White;">Plants</label>
                <input type="radio" name="radio-choice" id="rblVendors" value="Vendors" /><label
                    for="rblVendors" style="color: White;">Vendors</label>
            </div>
            <div style="width: 50px; float: left; top: 32px; position: absolute; z-index: 99999;
                left: 175px;">
                <img id="btnClose" alt="" src="LiveIcons/Left01.png" title="Hide" style="height: 23px;
                    width: 25px" />
            </div>
        </div>
        <div style="width: 100%;">
            <table>
                <tr>
                    <td id="cell" valign="top" style="border: 1px solid #d5d5d5; background-color: #f4f4f4;
                        height: 650px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblreportname" runat="server" Font-Bold="true" Font-Size="14px" ForeColor="Black"
                                                Text=""></asp:Label>
                                            <table style="width: 98%;">
                                                <tr>
                                                    <td style="width: 80px;color:Black;font-weight:bold;">
                                                        <asp:Label ID="lblFromDate" runat="server" Text="From Date "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="startdate" runat="server" Width="205px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="startdate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="startdate" Format="dd-MM-yyyy HH:mm">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="color:Black;font-weight:bold;">
                                                        <asp:Label ID="lblToDate" runat="server" Text="   To Date   "></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="enddate" runat="server" Width="205px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="enddate_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="enddate" Format="dd-MM-yyyy HH:mm">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <td style="color:Black;font-weight:bold;">
                                                    <asp:Label ID="lbl_show" runat="server" Text="Speed" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txt_Reports_TimeGap" runat="server" Visible="False" Width="205px"></asp:TextBox>
                                                    <asp:DropDownList ID="DDL_locations" runat="server" AutoPostBack="True" Height="21px"
                                                        Width="205px">
                                                        <asp:ListItem>General Reports</asp:ListItem>
                                                        <asp:ListItem>Charts</asp:ListItem>
                                                        <asp:ListItem>Stopage Report</asp:ListItem>
                                                        <asp:ListItem>Geofence Report</asp:ListItem>
                                                        <asp:ListItem>OverSpeed Report</asp:ListItem>
                                                        <asp:ListItem>Location Wise Report</asp:ListItem>
                                                        <asp:ListItem>Daily Report</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                </tr>
												<tr>
                                                    <td>
                                                        <asp:Label ID="lblgroup" runat="server" Text="Select Group" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlgroup" runat="server" Height="21px"
                                                            Width="205px"  AutoPostBack="True" OnSelectedIndexChanged="ddlgroup_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="color:Black;font-weight:bold;">
                                                        <asp:Label ID="lblfromlocation" runat="server" Text="From Location" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlfromlocation" runat="server" AutoPostBack="True" Height="21px"
                                                            Width="205px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="color:Black;font-weight:bold;">
                                                        <asp:Label ID="lbltolocation" runat="server" Text="To Location" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddltolocation" runat="server" AutoPostBack="True" Height="21px"
                                                            Width="205px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td style="color:Black;font-weight:bold;">
                                                        <asp:Label ID="lblCost" runat="server" Text="Cost Per Km" Visible="False"></asp:Label>
                                                    </td>
                                                    <td>
                                                       <asp:TextBox ID="txtCost" runat="server" Width="205px" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <div style="display: block; width: 240px;" id="divchblvehicles">
                                                    <cc1:DropDownCheckList ID="chblVehicleTypes" runat="server" BorderColor="black" BorderStyle="Solid"
                                                        AutoPostBack="true" ForeColor="Black" Font-Size="14px" Font-Bold="true" CheckListCssStyle="position:absolute;z-index:99999;overflow: auto; border: 1px solid black; padding: 4px; max-height:300px; background-color: #ffffff;"
                                                        DisplayBoxCssStyle="border: 1px solid #000000; cursor: pointer; width:240px; height:30px;z-index:99999;color:black;font-size:14px;font-weight:bold;"
                                                        Width="160px" TextWhenNoneChecked="Vehicle Type">
                                                    </cc1:DropDownCheckList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <cc1:DropDownCheckList ID="chblZones" runat="server" BorderColor="black" BorderStyle="Solid"
                                                    AutoPostBack="true" ForeColor="Black" Font-Size="14px" Font-Bold="true" CheckListCssStyle="position:absolute;z-index:99999;overflow: auto; border: 1px solid black; padding: 4px; max-height:300px; background-color: #ffffff;"
                                                    DisplayBoxCssStyle="border: 1px solid #000000; cursor: pointer; width:240px; height:30px;z-index:99999;color:black;font-size:14px;font-weight:bold;"
                                                    Width="160px" TextWhenNoneChecked="Plant Name">
                                                </cc1:DropDownCheckList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hdnResultValue" Value="0" runat="server" />
                                            <asp:Button ID="btn_generate" CssClass="ContinueButton" runat="server" Text="Generate"
                                                Height="25px" Width="100px" Font-Size="16px" OnClick="btn_generate_Click" />
                                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/exporttoxl_utility.ashx">Export to XL</asp:HyperLink>
                                            <%--    <label id="lbl">Vehicles Count</label>
                                                    <label id="lblvehscount" style="font-size:14px;font-weight:bold;color:Red;float:right;">0</label>--%>
                                            <br />
                                            <asp:Label ID="lbl_nofifier" runat="server" ForeColor="Red"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <div id="divassainedvehs" style="border: 1px solid #c0c0c0; float: left; height: 500px;
                            padding-left: 10px; overflow: auto; width: 300px;">
                        </div>
                    </td>
                    <td valign="top" style="width: 100%;">
                        <table>
                            <tr>
                                <td style="width: 100%; vertical-align: top">
                                    <asp:Panel ID="pReports" runat="server">
                                        <%-- <div>--%>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <%--  <div  runat="server">--%>
                                                <table id="divExport" style="width: 100%;" valign="top">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_ReportStatus" runat="server" Text=" "></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 100%;" valign="top">
                                                            <div style="top: 100px; bottom: 0px; overflow: auto;">
                                                                <asp:Panel ID="Panel3" runat="server" Width="100%" Height="30%">
                                                                    <asp:GridView ID="grdReports" runat="server" CellPadding="5" CellSpacing="5" ForeColor="White"
                                                                        GridLines="Horizontal" Width="100%" CssClass="gridcls" OnSelectedIndexChanged="grdReports_SelectedIndexChanged">
                                                                        <EditRowStyle BackColor="#999999" />
                                                                        <FooterStyle BackColor="Gray" Font-Bold="False" ForeColor="White" />
                                                                        <HeaderStyle BackColor="#f4f4f4" Font-Bold="False" ForeColor="Black" Font-Italic="False"
                                                                            Font-Names="Raavi" Font-Size="Small" />
                                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                        <RowStyle BackColor="#ffffff" ForeColor="#333333" />
                                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Button ID="Button1" runat="server" Text="Draw" OnCommand="OnclickDrawRoute"
                                                                                        OnClientClick="NormalOpening();" CssClass="findme" CommandArgument='<%#Container.DataItemIndex%>' />
                                                                                         <asp:Button ID="btn_details" runat="server" Text="Details" OnCommand="OnclickViewDetails" CssClass="opener" OnClientClick="reportview(this);"  CommandArgument='<%#Container.DataItemIndex%>' />
                                                                                        
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                    <div runat="server" id="div1">
                                                                    </div>
                                                                    <div runat="server" id="divPieChart1">
                                                                    </div>
                                                                    <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none;">
                                                                    </asp:Label>
                                                                    <div runat="server" id="divPieChart">
                                                                    </div>
                                                                </asp:Panel>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%--</div>--%>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <%-- </div>--%>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>

      <div id="dialog" title="Locations Details" style="Width:100%;"   >
       <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
          <asp:GridView ID="GridView1" runat="server"  CellPadding="5" CellSpacing="5" 
                                                                        GridLines="Horizontal" Width="100%" CssClass="gridcls">
          </asp:GridView>
          </ContentTemplate>
          </asp:UpdatePanel>
</div>
 
</asp:Content>
