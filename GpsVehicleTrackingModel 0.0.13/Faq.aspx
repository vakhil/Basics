<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.master" AutoEventWireup="true"
    CodeFile="Faq.aspx.cs" Inherits="Faq" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="ContentPlaceHolder1">

    <br />
<br />
<br />
    <div class="shell">
      <br />
    <br />
    <br />
    <div  style="width:100%;height:90%;background-color:#FFFFFF;border-radius:7px 7px 7px 7px;">
        <div id="innertitle">
            <table align="center">
                <tr>
                    <td style="width: 116px">
                        <strong>
                            <asp:Label ID="lblheat" runat="server" Text="FAQ" Font-Size="X-Large" ForeColor="#F7990D"></asp:Label></strong>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
        <div id="staticcontent" style="margin-top: 10px; margin-bottom: 20px">
            <table cellspacing="0" cellpadding="0" width="90%" align="center" border="0">
                <tbody>
                    <tr>
                        <td class="innerrightbg">
                        </td>
                        <td class="innerboxbg" align="center">
                            <table cellspacing="0" cellpadding="0" width="90%" border="0">
                                <tbody>
                                    <tr>
                                        <td align="left">
                                            <p>
                                                <b class="orange11" style="font-size: 14px; font-weight: bold">Q: What is GPS?</b>
                                                <br />
                                                A. GPS stands for Global Positioning System. Created by the US military and subsequently
                                                made available for commercial use, it employs a series of satellites that enable
                                                a receiver on the earth to calculate its position very accurately. A typical GPS
                                                receiver calculates its position using the signals from four or more GPS satellites.
                                                These values are then turned into more user-friendly forms, such as latitude/longitude
                                                or location on a map, then displayed to the user.
                                            <br />
                                            <br />
                                            </p>
                                            <p>
                                                <b class="orange11" style="font-size: 14px; font-weight: bold">Q. Can I see multiple
                                                    car locations on one map?</b>
                                                <br />
                                                A. Yes, the tracking site allows you to view multiple car locations on one map.
                                            <br />
                                            <br />
                                            </p>
                                            <p>
                                                <b class="orange11" style="font-size: 14px; font-weight: bold">Q: Can anyone log in
                                                    and see my vehicle?</b>
                                                <br />
                                                A. No. The ASnTech tracking device's monitoring data is password protected on the
                                                ASnTech Web-portal.
                                            <br />
                                            <br />
                                            </p>
                                            <p>
                                                <b class="orange11" style="font-size: 14px; font-weight: bold">Q: How are notifications
                                                    received?</b>
                                                <br />
                                                A. Alert notification can be received via email and/or text message alerts.
                                            <br />
                                            <br />
                                            </p>
                                            <p>
                                                <b class="orange11" style="font-size: 16px; font-weight: bold">Q: What is the range
                                                    of coverage?</b>
                                                <br />
                                                A. ASnTech is able to track in any area where there is cellular coverage. If coverage
                                                is lost the unit stores the GPS data until the coverage is restored and then transmits
                                                it.
                                            <br />
                                            <br />
                                            </p>
                                            <p>
                                                <b class="orange11" style="font-size: 14px; font-weight: bold">Q: Are teens receptive
                                                    to the ASnTech tracking device?</b>
                                                <br />
                                                A. Initally some aren't. The ASnTech monitoring device can help make teens more
                                                aware of their driving habits and, because parents feel more at ease when the device
                                                is being used, teen drivers are often given greater driving privileges.
                                            <br />
                                            <br />
                                            </p>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    </div>
    <br />
<br />
<br />
</asp:Content>
