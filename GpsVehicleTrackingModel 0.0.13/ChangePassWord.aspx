<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassWord.aspx.cs" Inherits="ChangePassWord" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
	track my asset
</title>
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
    <asp:Label runat="server" Text="Change Password" ID="lblASnTech" ForeColor="White" Font-Size="Large"></asp:Label>
  </td>
  </tr>
  </table>
  </div>
  <br />
  <br />
    <table align="center"  style="border:1px solid gray;">
    
    <tr>
            <td>
                <asp:Label ID="lblOldPassWord" runat="server" Text="Old Password"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtOldPassWord" TextMode="Password" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtOldPassWord" ForeColor="Red" ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
    </tr>
      <tr>
            <td>
                <asp:Label ID="lblNewPassWord" runat="server" Text="New Password"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtNewPassWord" TextMode="Password" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassWord" ForeColor="Red" ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
    </tr>
      <tr>
            <td>
                <asp:Label ID="lblConformPassWord" runat="server" Text="Conform Password"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtConformPassWord" TextMode="Password" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConformPassWord" ForeColor="Red" ErrorMessage="*">
                </asp:RequiredFieldValidator>
            </td>
    </tr>
    <tr>
    <td>
     <a href="LogOut.aspx">Back To Login Page</a>
    </td>
    <td>
        <asp:Button ID="btnSubmitt" runat="server"  Text="Submitt" 
            onclick="btnSubmitt_Click" />
    </td>
    </tr>
    <tr>
    <td>
       
    </td>
    <td>
    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblMessage" ForeColor="Green" runat="server" Text=""></asp:Label>
    </td>
        
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
