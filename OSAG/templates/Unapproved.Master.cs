﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSAG.templates
{
    public partial class Unapproved : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TempUsername"] == null)
            {
                Session["MustLogin"] = "You must log in to access that page.";
                Response.Redirect("/login/LoginPage.aspx");
            }
            else
                lblUsername.Text = Session["TempUsername"].ToString();
        }

        protected void lnkbtnSignOut_Click(object sender, EventArgs e)
        {
            /********************************************
            *  Any user data that you need to work with *
            *  do here before you abandon session       *
            *********************************************/
            Session.Abandon();
            Response.Redirect("/login/LoginPage.aspx");
        }
    }
}