using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderFood
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Image1.ImageUrl = Request.QueryString["Type"] + ".jpg";
        }

        protected void btTry_Click(object sender, EventArgs e)
        {
            Response.Redirect("Selector.aspx");
        }
    }
}