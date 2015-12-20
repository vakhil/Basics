<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/style.css?v=1001" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="css/default.css?v=1001" type="text/css" media="screen" />
<script type="text/javascript">
    $(window).load(function () {
        $('#slider').nivoSlider();
    });
</script>
<style type="text/css">
#container
{
margin: 0 auto;
text-align: left;
height:600px;
}
.shells
 {
      
     
       position: relative;
       border-radius:4px 4px 4px 4px;
}
</style>
<script language="javascript" type="text/javascript">
    function mouseOverImage() {
        document.getElementById("btnLogIn").src = "Images/login_bt1.gif";
    }

    function mouseOutImage() {
        document.getElementById("btnLogIn").src = "Images/login_bt.gif";
    }
    function mouseOverResetImage() {
        document.getElementById("btnReset").src = "Images/reset_bt1.gif";
    }

    function mouseOutresetImage() {
        document.getElementById("btnReset").src = "Images/reset_bt.gif";
    }
</script>   
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%-- <div id="container" > 
    <div class="shells" >--%>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <div  style="background-color:#f4f4f4;border-radius:7px 7px 7px 7px; padding-left:30px;padding-right:40px;">
    <div style="width:700px;float:left;padding-left:50px;top:0px;">
   <span style="color:Gray; font-size:30px;float:left;padding-top:40px;"><b>GPS VEHICLE TRACKING</b></span>
   <img style="float:right; padding-top:5px;width: 140px;height: 150px;" src="Images/Modellogo.gif" />
   </div>
   <div id="main">
     <div id="content_left">        
        <div class="slider-wrapper theme-default">
            <div id="slider" class="nivoSlider">
                <img src="VehicleImages/$RCEH161.png" />
                <img src="VehicleImages/$RX4K025.png" />
                <img src="VehicleImages/Track5.png" />
                <img src="VehicleImages/Track6jpg.png" />
            </div>
        </div>
     </div>
        
     <div id="content_right">
      <div class="members_login" >
         <h4>User Login</h4>
         <div class="login_row" style="text-align:left;">                
          <strong><span id="lblUserName" class="labelClass">User Name</span></strong> 
             <asp:TextBox name="txtUserName" type="text" id="txtUserName" runat="server" class="txtBoxClass" style="width:120px;" />
           
         </div>
         <div class="login_row"  style="text-align:left;">
         &nbsp;<strong><span id="lblPassword" class="labelClass">Pass word</span></strong>&nbsp; 
             <asp:TextBox name="txtPassword" TextMode="password" runat="server" id="txtPassword" class="txtBoxClass" style="width:120px;" />
           
         </div>
         <div class="login_row"></div>
         <div class="login_row"> 
              &nbsp; 
             <asp:ImageButton type="image" name="btnLogIn" id="btnLogIn" onmouseover="mouseOverImage()"   onmouseout="mouseOutImage()" runat="server" src="Images/login_bt.gif" 
             
                  style="border-width:0px;position: relative; left: 11px; top: 0px; height: 22px;" 
                  onclick="btnLogIn_Click" />
          	<asp:ImageButton type="image" name="btnReset" id="btnReset" onmouseover="mouseOverResetImage()"  onmouseout="mouseOutresetImage()"  src="Images/reset_bt.gif" runat="server" style="border-width:0px;position: relative; left: 18px; top: 0px;"    onclick="btnReset_Click" />
                  <br />
                  <asp:Label Text="" runat="server" id="lbl_validation"></asp:Label>
                  <br />
                  <ul>
                  <li>
                    <a href="ForgetPassWord.aspx">Forgot PassWord</a>
                  </li>
                  <br />
              <%--  <li>
                <a href="DealersLogin.aspx"><strong ><span>Dealers Login</span></strong></a>
                </li>--%>
                  </ul>
         </div>
        </div>
       </div>
       <div class="spacer"></div>
      </div>
       <%--  <br />
    <br />
    <br />--%>
    <br />
    <br />
          </div>
     <%--    
        </div>
        </div>--%>
</asp:Content>

