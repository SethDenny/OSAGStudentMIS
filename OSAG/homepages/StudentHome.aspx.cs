﻿/*code behind for student hompage */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSAG.homepages
{
    public partial class LoggedInHomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnInternships_Click(object sender, EventArgs e)
        {
            Response.Redirect("/internships/Internships.aspx");
        }

        protected void btnJobs_Click(object sender, EventArgs e)
        {
            Response.Redirect("/jobs/Jobs.aspx");
        }
    }
}