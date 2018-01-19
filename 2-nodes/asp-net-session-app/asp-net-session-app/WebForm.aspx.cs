using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asp_net_session_app
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            show();
        }

        protected void btncalc_Click(object sender, EventArgs e)
        {
            int v = 0;

            if (Session["value"] != null)
            {
                v = (int)Session["value"];
            }
            v++;
            Session["value"] = v;
            show();
        }
        private void show()
        {
            if (Session["value"] != null)
                lblsum.Text = Session["value"].ToString();
            else
                lblsum.Text = "-";
        }
    }
}