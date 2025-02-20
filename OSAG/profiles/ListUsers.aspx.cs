﻿/*code behind for list users*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OSAG.profiles
{
    public partial class ListUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Session["MustLogin"] = "You must log in to access that page.";
                Response.Redirect("/login/LoginPage.aspx");
            }
            else if (!IsPostBack)
            {
                if (Session["Username"].ToString() == "admin")
                    sqlsrcStudentQuery.SelectCommand = "SELECT Username, FirstName, LastName FROM Student";
            }

        }

        // event handler to search student
        protected void btnSearchStu_Click(object sender, EventArgs e)
        {
            String stuFilter = "SELECT Username, FirstName, LastName FROM Student " +
                   "WHERE FirstName LIKE '%[Search]%' OR LastName LIKE '%[Search]%' OR Username LIKE '%[Search]%'";
            if (Session["MemberType"] == null)
                stuFilter += " AND IsApproved = '1'";
            else if (Session["MemberType"].ToString() != "1")
                stuFilter += " AND IsApproved = '1'";

            sqlsrcStudentQuery.SelectCommand = stuFilter.Replace("[Search]", HttpUtility.HtmlEncode(searchBar.Value.Trim()));
            grdvStudent.DataBind(); // COMMAND FIXES BUG WHERE PREVIOUS SEARCH PERSISTS AFTER CLEARING SEARCHBAR
        }

        // event handler to search member
        protected void btnSearchMent_Click(object sender, EventArgs e)
        {
            String membFilter = "SELECT Username, FirstName, LastName FROM Member " +
                   "WHERE FirstName LIKE '%[Search]%' OR LastName LIKE '%[Search]%' OR Username LIKE '%[Search]%'";
            if(Session["MemberType"] == null)
                membFilter += " AND IsApproved = '1'";
            else if (Session["MemberType"].ToString() != "1")
                membFilter += " AND IsApproved = '1'";
            sqlsrcMemberQuery.SelectCommand = membFilter.Replace("[Search]", HttpUtility.HtmlEncode(searchBar2.Value.Trim()));
            grdvMember.DataBind(); // COMMAND FIXES BUG WHERE PREVIOUS SEARCH PERSISTS AFTER CLEARING SEARCHBAR
        }

        protected void grdvStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ViewProfileUsername"] = grdvStudent.SelectedRow.Cells[1].Text;
            Session["ViewProfileUserType"] = "student";
            Response.Redirect("ViewProfile.aspx");
        }

        protected void grdvMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ViewProfileUsername"] = grdvMember.SelectedRow.Cells[1].Text;
            Session["ViewProfileUserType"] = "member";
            Response.Redirect("ViewProfile.aspx");
        }
    }
}