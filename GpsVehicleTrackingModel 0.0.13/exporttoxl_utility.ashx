<%@ WebHandler Language="C#" Class="exporttoxl_utility" %>

using System;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public class exporttoxl_utility : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");

        context.Response.AddHeader("content-disposition", "attachment; filename=excelData.xls");
        context.Response.ContentType = "application/ms-excel";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        System.Data.DataTable dtt = (System.Data.DataTable)context.Session["xportdata"];
        string maintitle = "";
        if (context.Session["field1"] != null)
        {
            if (context.Session["field1"].ToString() == "heritagenew")
            {
                maintitle = "HERITAGE FOODS (INDIA) LTD";
            }
        }
        string title = "";
        if (context.Session["title"] != null)
        {
            title = context.Session["title"].ToString();
        }
        // ExportToExcel(dtt);
        string exportContent = ExportToExcel(title, dtt, maintitle);
        response.Write(exportContent);
    }
    private string ExportToExcel(string title, System.Data.DataTable dtt, string maintitle)
    {
        string _exportContent = "";
        Table table = new Table();
        using (StringWriter sb = new StringWriter())
        {
            using (HtmlTextWriter htmlWriter = new HtmlTextWriter(sb))
            {
                table.GridLines = GridLines.Horizontal;
                table.BorderWidth = new Unit(1);

                TableRow xltitlerow = new TableRow();
                TableCell xltitlerowcell = new TableCell();
                xltitlerowcell.Text = maintitle;
                xltitlerowcell.Font.Bold = true;
                xltitlerowcell.Font.Size = 14;
                xltitlerow.Cells.Add(xltitlerowcell);
                table.Rows.Add(xltitlerow);

                TableRow trow1 = new TableRow();
                TableCell tcell = new TableCell();
                tcell.Text = title;
                tcell.Font.Bold = true;
                tcell.Font.Size = 12;
                trow1.Cells.Add(tcell);
                table.Rows.Add(trow1);


                TableRow trow = new TableRow();
                for (int j = 0; j < dtt.Columns.Count; j++)
                {
                    TableCell cell = new TableCell();
                    cell.Text = dtt.Columns[j].ColumnName;
                    trow.Cells.Add(cell);
                }
                table.Rows.Add(trow);

                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    TableRow row = new TableRow();
                    //row.BackColor = (i % 2 == 0) ? System.Drawing.Color.BlanchedAlmond :
                    //                           System.Drawing.Color.BurlyWood;
                    for (int j = 0; j < dtt.Columns.Count; j++)
                    {
                        TableCell cell = new TableCell();
                        cell.Text = dtt.Rows[i][j].ToString();
                        row.Cells.Add(cell);

                    }
                    table.Rows.Add(row);
                }
                table.Rows[0].Cells[0].ColumnSpan = dtt.Columns.Count;
                table.Rows[1].Cells[0].ColumnSpan = dtt.Columns.Count;
                //table.Rows[0].Cells.RemoveAt(1);
                table.RenderControl(htmlWriter);
                _exportContent = sb.ToString();
            }
        }

        //System.IO.StringWriter tw = new System.IO.StringWriter();
        //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
        //DataGrid dgGrid = new DataGrid();
        //dgGrid.DataSource = table;
        //dgGrid.DataBind();

        ////Get the HTML for the control.
        //dgGrid.RenderControl(hw);

        //_exportContent = tw.ToString();
        return _exportContent;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}