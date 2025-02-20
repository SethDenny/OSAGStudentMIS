﻿/* Code behind for displaying available internships */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//Sql Imports
using System.Data;
using System.Data.SqlClient;
//Connection Strings in web.config
using System.Web.Configuration;


namespace OSAG.internships
{
    public partial class Internships : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Session["MustLogin"] = "You must log in to access that page.";
                Response.Redirect("/login/LoginPage.aspx");
            }

            // check each bookmark button and change text based on whether a bookmark exists
            if (!IsPostBack && Session["UserType"].ToString() == "student") // only when first loading page
            {
                // Retrieve StudentID of user
                int StudentID = UsernameToID(Session["Username"].ToString());

                // for each grid row
                for (int i = 0; i < grdvwInternships.Rows.Count; i++)
                {
                    // define button to be changed and gridview row being used
                    Button btn = (Button)grdvwInternships.Rows[i].FindControl("btnBookmark");

                    // Retrieve InternshipID from row
                    int ItemID = (int)grdvwInternships.DataKeys[((GridViewRow)btn.NamingContainer).RowIndex]["InternshipID"];

                    // check bookmark. get match record (if any)
                    int[] match = getMatch(StudentID, ItemID);
                    // go to next row if no record exists
                    if (match == null)
                        continue;
                    // if bookmarked, change button text
                    if (match[1] == 1)
                        btn.Text = "Remove Bookmark";
                }
            }
        }

        protected void btnBookmark_Click(object sender, EventArgs e)
        {
            // Define btn and retrieve InternshipID from gridview
            Button btn = (Button)sender;
            int InternshipID = (int)grdvwInternships.DataKeys[((GridViewRow)btn.NamingContainer).RowIndex].Value;

            // define database connection & retrieve StudentID of user
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            int StudentID = UsernameToID(Session["Username"].ToString());
            string sqlQuery;

            // Insert/Remove bookmark
            int[] matchRecord = getMatch(StudentID, InternshipID);
            if (matchRecord == null)        // add match record for bookmark
            {
                sqlQuery = "INSERT INTO InternshipMatch (IsBookmark, StudentID, InternshipID) VALUES (1, " + StudentID + ", " + InternshipID + ")";
                btn.Text = "Remove Bookmark";
            }
            else if (matchRecord[1] == 0)   // set bookmarked in existing match record to true
            {
                sqlQuery = "UPDATE InternshipMatch SET IsBookmark = 1 WHERE InternshipMatchID = " + matchRecord[0] + ";";
                btn.Text = "Remove Bookmark";
            }
            else                            // set bookmarked in existing match record to false
            {
                sqlQuery = "UPDATE InternshipMatch SET IsBookmark = 0 WHERE InternshipMatchID = " + matchRecord[0] + ";";
                btn.Text = "Bookmark";
            }
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteScalar();
            sqlConnection.Close();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Session["View"] = grdvwInternships.DataKeys[((GridViewRow)((Button)sender).NamingContainer).RowIndex].Value;
            Response.Redirect("InternshipDetails.aspx");
        }

        // helper method to execute stored procedure (username [GUID within program] -> StudentID/MemberID)
        protected int UsernameToID(string username)
        {
            SqlConnection sqlConnect = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("dbo.OSAG_UsernameToID", sqlConnect);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Username", username);
            sqlConnect.Open();
            return (int)sqlCommand.ExecuteScalar();
        }

        // helper method to get matchID and bookmark status
        public int[] getMatch(int stuID, int itemID)
        {
            try
            {
                // query for match ID and bookmark status
                SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
                String sqlQuery = "SELECT InternshipMatchID, IsBookmark, IsInterest FROM InternshipMatch WHERE StudentID = " + stuID + " AND InternshipID = " + itemID + ";";
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlConnection.Open();
                SqlDataReader queryResults = sqlCommand.ExecuteReader();
                queryResults.Read();
                // returns integer array with match ID index 0, bookmark status at 1, and interest level at 2
                int[] intArr = new int[3];
                intArr[0] = (int)queryResults["InternshipMatchID"];
                // handles null values (null = false/0)
                if (bitToBoolean(queryResults["IsBookmark"]))
                    intArr[1] = 1;
                else
                    intArr[1] = 0;
                // handles null values (null = -1)
                if (queryResults["IsInterest"] == DBNull.Value)
                    intArr[2] = -1; // for main method handling, prevents null -> int conversion error
                else
                    intArr[2] = Convert.ToInt32(queryResults["IsInterest"]);
                return intArr;
            }
            catch (InvalidOperationException)
            {   // if query results is empty, return null for main method handling
                return null;
                throw;
            }
        }

        // SQL Server BIT -> Boolean. read above comments for more details
        private bool bitToBoolean(object o)
        {
            if (o == DBNull.Value)
                return false;
            return (bool)o;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String search = "SELECT InternshipID, j.InternshipName, c.CompanyName, ApplicationDeadline FROM Internship j LEFT JOIN Company c on j.CompanyID = c.CompanyID WHERE (j.InternshipName like '%[Search]%' OR c.CompanyName like '%[Search]%') AND ApplicationDeadline > GETDATE() ORDER BY ApplicationDeadline ASC";
            sqlsrc.SelectCommand = search.Replace("[Search]", HttpUtility.HtmlEncode(searchBar.Value.Trim()));
        }
    }
}