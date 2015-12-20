using System.Web;
public class HelloWorldHandler : IHttpHandler
{
    public HelloWorldHandler()
    {
    }
    public void ProcessRequest(HttpContext context)
    {
       string method = context.Request.QueryString["MethodName"].ToString();
    context.Response.ContentType = “text/json”;
    switch (method)
    {
        case “GetData” :
            context.Response.Write(GetData());
            break;
    }


    }

    protected string GetData()
{
    return (@”{“”FirstName”":”"Ravi”", “”LastName”":”"Baghel”", 
               “”Blog”":”"ravibaghel.wordpress.com”"}”);
}



    public bool IsReusable
    {
        // To enable pooling, return true here.
        // This keeps the handler in memory.
        get { return false; }
    }
}