<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ForgetPassWord.aspx.cs" Inherits="ForgetPassWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    

<%--<link rel="shortcut icon" href="Images/favicon.ico" type="image/x-icon" />--%>
<link href="css/style.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="css/default.css" type="text/css" media="screen" />
<link rel="stylesheet" href="css/nivo-slider.css" type="text/css" media="screen" />
<script type="text/javascript" src="js/jquery-1.7.1.min.js"></script>
<script type="text/javascript" src="js/jquery.nivo.slider.pack.js"></script>
<script type="text/javascript">
    $(window).load(function () {
        $('#slider').nivoSlider();
    });
</script>
</head>
<body>
    <form id="form1" class="titlePane" runat="server">
    <div id="container">
    <div id="head">	
  <table align="center">
  <tr>
  <td>
    <asp:Label runat="server" Text="Forgot PassWord" ID="lblASnTech" ForeColor="White" Font-Size="Large"></asp:Label>
  </td>
  </tr>
     
  </table>
   </div>
    <div  align="center">
<%--  <fieldset style="width:350px;height:150px;">
    <legend>Forgot Password</legend> --%>
    <table align="center">
      <tr>
           <td>
            <asp:Label ID="lblEmail" runat="server" Text="Email Address: "/>
           </td>
           <td>
            <asp:TextBox ID="txtEmail" runat="server"/>
   <asp:RequiredFieldValidator ID="RV1" runat="server"  ControlToValidate="txtEmail"  ErrorMessage="*" SetFocusOnError="True">
   </asp:RequiredFieldValidator>
               <asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                   ForeColor="Red" ControlToValidate="txtEmail" runat="server" 
                   ErrorMessage="InValid EmailID" 
                   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
           </td>
      </tr>
      <tr></tr>
     <tr>
     <td>
         <a href="Login.aspx">Back to Login Page</a>
     </td>
     <td>
      <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
          onclick="btnSubmit_Click" />
    
  <asp:Label ID="lblMessage" ForeColor="Red" runat="server" Text=""/>
     </td>
     </tr>
    
    </table>
    
    
  
   
  <%-- </fieldset>--%>
    </div>
  </div>
    </form>
</body>
</html>
