using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using MySql.Data.MySqlClient; 

public partial class ForgetPassWord : System.Web.UI.Page
{
    MySqlCommand command;
    MailMessage mail = new MailMessage();
    static VehicleDBMgr vdm;
    static DataDownloader ddwnldr;
    protected void Page_Load(object sender, EventArgs e)
    {
        vdm = new VehicleDBMgr();
        ddwnldr = new DataDownloader();
        vdm.InitializeDB();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        command = new MySqlCommand("SELECT UserName,Password FROM UserAccounts WHERE EmailID = @Email");
        command.CommandType = CommandType.Text;
        MySqlParameter email = new MySqlParameter("@Email", MySqlDbType.VarChar, 50);
        email.Value = txtEmail.Text.Trim().ToString();
        command.Parameters.Add(email);
        DataTable dt = vdm.SelectQuery(command).Tables[0];
        if (dt.Rows.Count > 0)
        {
            MailMessage loginInfo = new MailMessage();
            loginInfo.To.Add(txtEmail.Text.ToString());
            loginInfo.From = new MailAddress("ravindra1507@gmail.com");
            loginInfo.Subject = "Forgot Password Information";
            loginInfo.Body = "Username: " + dt.Rows[0]["UserName"] + "<br><br>Password: " + dt.Rows[0]["Password"] + "<br><br>";
            loginInfo.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = true;
            //smtp.Credentials = new NetworkCredential("ravindra1507@gmail.com", "ASnTech", "smtp.gmail.com");
            smtp.Credentials = new System.Net.NetworkCredential("ravindra1507@gmail.com", "ASnTech", "ravindra142");
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            // smtp.Credentials = new System.Net.NetworkCredential("ravindra1507@gmail.com", "ravindra142");
            smtp.Send(loginInfo);
            lblMessage.Text = "Password is sent to you email id";
        }
        else
        {
            lblMessage.Text = "Email Address Not Registered";
        }
        Response.Redirect("Login.aspx");
    }
}