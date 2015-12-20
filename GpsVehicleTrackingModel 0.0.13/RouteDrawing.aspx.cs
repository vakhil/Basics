using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class RouteDrawing : System.Web.UI.Page
{
    public static DataTable selecteddata = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        selecteddata = (DataTable)Session["Data"];
    }
}