﻿/*Code behind for internship details page */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Drawing;

namespace OSAG.internships
{
    public partial class InternshipDetails : System.Web.UI.Page
    {
        String payment = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack) // check if the webpage is loaded for the first time.
                    ViewState["PreviousPage"] = Request.UrlReferrer; // Saves the Previous page url in ViewState
                if (ViewState["PreviousPage"] == null) // easy way to filter non-logged in users
                    throw new NullReferenceException();

                if (Session["MemberType"] != null) // prevent null ref exception
                    if (Session["MemberType"].ToString() == "1") // prevent user from abusing querystring
                        if (Int32.TryParse(Request.QueryString["id"], out int i)) // retrieve querystring if it is being used
                            Session["View"] = i;

                // Query to populate page with data
                String sqlQuery = "Select InternshipName AS Name, " +
                    " + CompanyName AS Company, " +
                    "+  InternshipDescription AS Description, " +
                    "+  Payment AS UnformattedPayment, " +
                    " CAST(ApplicationDeadline AS VARCHAR) AS Deadline, " +
                    "CAST(StartDate AS VARCHAR) AS Start, " +
                    "CAST(WeeklyHours AS VARCHAR) AS Hours, " +
                    "FORMAT(Payment,'C') AS Payment, " +
                    "InternshipLink " +
                    "FROM Internship LEFT JOIN Company ON Company.CompanyID = Internship.CompanyID WHERE InternshipID = '" + Session["View"].ToString() + "'";
                SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlConnection.Open();
                SqlDataReader queryResults = sqlCommand.ExecuteReader();
                // read onto page
                while (queryResults.Read())
                {
                    lblName.Text = queryResults["Name"].ToString().ToUpper();
                    lblCompany.Text = queryResults["Company"].ToString().ToUpper();
                    lblDescription.Text = queryResults["Description"].ToString();
                    lblDeadline.Text = queryResults["Deadline"].ToString();
                    lblStart.Text = queryResults["Start"].ToString();
                    lblHours.Text = queryResults["Hours"].ToString();
                    lblPayment.Text = queryResults["Payment"].ToString();
                    payment = queryResults["UnformattedPayment"].ToString();
                    // give the linkbutton the stored URL
                    if (queryResults["InternshipLink"] != DBNull.Value)
                    {
                        divApplyButton.Visible = true;
                        lnkbtnApply.OnClientClick = "Navigate('" + queryResults["InternshipLink"].ToString() + "')";
                    }
                }
                sqlConnection.Close(); // marks end of above query run

                // populate radio buttons / bookmark button with current match status
                int stuID = UsernameToID(Session["Username"].ToString());
                int[] match = getMatch(stuID, (int)Session["View"]);
                if (match == null) // no record -> do nothing
                    return;
                else // record exists
                {
                    if (match[1] == 1)
                        btnBookmark.Text = "REMOVE BOOKMARK";
                    // switch statement for IsInterest record
                    switch (match[2])
                    {
                        case -1:    // NULL
                            break;
                        case 0:     // Low
                            rdoLow.Checked = true;
                            break;
                        case 1:     // Medium
                            rdoMed.Checked = true;
                            break;
                        case 2:     // High
                            rdoHi.Checked = true;
                            break;
                    }
                }
            }
            catch (NullReferenceException)
            {
                Session["MustLogin"] = "You must log in to access that page.";
                Response.Redirect("/login/LoginPage.aspx");
                throw;
            }
        }

        protected void btnBookmark_Click(object sender, EventArgs e)
        {
            // Define btn and retrieve InternshipID from Session Variable
            Button btn = (Button)sender;
            int InternshipID = Int32.Parse(Session["View"].ToString());

            // define database connection & retrieve StudentID of user
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            int StudentID = UsernameToID(Session["Username"].ToString());
            string sqlQuery;

            // Insert/Remove bookmark
            int[] matchRecord = getMatch(StudentID, InternshipID);
            if (matchRecord == null)        // add match record for bookmark
            {
                sqlQuery = "INSERT INTO InternshipMatch (IsBookmark, StudentID, InternshipID) VALUES (1, " + StudentID + ", " + InternshipID + ")";
                btn.Text = "REMOVE BOOKMARK";
            }
            else if (matchRecord[1] == 0)   // set bookmarked in existing match record to true
            {
                sqlQuery = "UPDATE InternshipMatch SET IsBookmark = 1 WHERE InternshipMatchID = " + matchRecord[0] + ";";
                btn.Text = "REMOVE BOOKMARK";
            }
            else                            // set bookmarked in existing match record to false
            {
                sqlQuery = "UPDATE InternshipMatch SET IsBookmark = 0 WHERE InternshipMatchID = " + matchRecord[0] + ";";
                btn.Text = "BOOKMARK";
            }
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteScalar();
            sqlConnection.Close();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (ViewState["PreviousPage"] != null)
            {
                Response.Redirect(ViewState["PreviousPage"].ToString());
            }
        }

        protected void lnkbtnApply_Click(object sender, EventArgs e)
        {
            divDidYouApply.Visible = true;
            if (lnkbtnApply.OnClientClick != null)
                divApplyButton.Visible = false;
            else
            {
                btnApplied.Visible = false;
                btnDidNotApply.Visible = false;
                lblStatus.Text = "The link to this form is unavailable. Please contact your administrator for further details.";
                lblStatus.ForeColor = Color.Red;
                lblStatus.Font.Bold = true;
            }
        }

        protected void btnApplied_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            int StudentID = UsernameToID(Session["Username"].ToString());
            string sqlQuery;

            // query based on whether a match exists
            if (MatchExists(StudentID, (int)Session["View"]))
            {
                sqlQuery = "UPDATE InternshipMatch SET AppStatus = 'Applied', ApplicationDate = @ApplicationDate " +
                    "WHERE StudentID = @StudentID AND InternshipID = @InternshipID;";
            }
            else
            {
                sqlQuery = "INSERT INTO InternshipMatch (AppStatus, ApplicationDate, StudentID, InternshipID) " +
                    "VALUES ('Applied', @ApplicationDate, @StudentID, @InternshipID)";
            }
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@ApplicationDate", DateTime.Now.ToString("yyyy-MM-dd"));
            sqlCommand.Parameters.AddWithValue("@StudentID", StudentID);
            sqlCommand.Parameters.AddWithValue("@InternshipID", Session["View"].ToString());
            sqlConnection.Open();
            sqlCommand.ExecuteScalar();
        }

        protected void btnDidNotApply_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Your response has been recorded.";
            lnkbtnApply.Visible = true;
            btnApplied.Visible = false;
            btnDidNotApply.Visible = false;
        }

        // event handler for low selection
        protected void rdoLow_CheckedChanged(object sender, EventArgs e)
        {
            // Retrieve InternshipID from Session Variable
            int InternshipID = Int32.Parse(Session["View"].ToString());

            // Retrieve StudentID of user
            int StudentID = UsernameToID(Session["Username"].ToString());

            // Insert/Update interestlevel
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            string sqlQuery;
            int[] matchRecord = getMatch(StudentID, InternshipID);
            if (matchRecord == null)    // student has not yet interacted with listing
                sqlQuery = "INSERT INTO InternshipMatch (IsInterest, StudentID, InternshipID) VALUES (0, " + StudentID + ", " + InternshipID + ")";
            else  // record of interaction exists
                sqlQuery = "UPDATE InternshipMatch SET IsInterest = 0 WHERE InternshipMatchID = " + matchRecord[0] + ";";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteScalar();
        }

        // event handler for medium selection
        protected void rdoMed_CheckedChanged(object sender, EventArgs e)
        {
            // Retrieve InternshipID from Session Variable
            int InternshipID = Int32.Parse(Session["View"].ToString());

            // Retrieve StudentID of user
            int StudentID = UsernameToID(Session["Username"].ToString());

            // Insert/Update interestlevel
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            string sqlQuery;
            int[] matchRecord = getMatch(StudentID, InternshipID);
            if (matchRecord == null)    // student has not yet interacted with listing
                sqlQuery = "INSERT INTO InternshipMatch (IsInterest, StudentID, InternshipID) VALUES (1, " + StudentID + ", " + InternshipID + ")";
            else  // record of interaction exists
                sqlQuery = "UPDATE InternshipMatch SET IsInterest = 1 WHERE InternshipMatchID = " + matchRecord[0] + ";";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteScalar();
        }

        // event handler for high selection
        protected void rdoHi_CheckedChanged(object sender, EventArgs e)
        {
            // Retrieve InternshipID from Session Variable
            int InternshipID = Int32.Parse(Session["View"].ToString());

            // Retrieve StudentID of user
            int StudentID = UsernameToID(Session["Username"].ToString());

            // Insert/Update interestlevel
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            string sqlQuery;
            int[] matchRecord = getMatch(StudentID, InternshipID);
            if (matchRecord == null)    // student has not yet interacted with listing
                sqlQuery = "INSERT INTO InternshipMatch (IsInterest, StudentID, InternshipID) VALUES (2, " + StudentID + ", " + InternshipID + ")";
            else  // record of interaction exists
                sqlQuery = "UPDATE InternshipMatch SET IsInterest = 2 WHERE InternshipMatchID = " + matchRecord[0] + ";";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteScalar();
        }

        // helper method to get matchID and bookmark status
        public bool MatchExists(int stuID, int itemID)
        {
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            String sqlQuery = "SELECT COUNT(*) FROM InternshipMatch WHERE StudentID = '" + stuID + "' AND InternshipID = '" + itemID + "';";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            sqlConnection.Open();
            if ((int)sqlCommand.ExecuteScalar() < 1)
                return false;
            return true;
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

        // helper method to execute stored procedure (username [GUID within program] -> StudentID/MemberID)
        protected int UsernameToID(string username)
        {
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("dbo.OSAG_UsernameToID", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@Username", username);
            sqlConnection.Open();
            return (int)sqlCommand.ExecuteScalar();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnect = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            String sqlQuery;
            sqlQuery = "Select CompanyID From Company WHERE CompanyName = '" + lblCompany.Text + "'";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnect);
            sqlConnect.Open();
            ddlCompany.SelectedValue = sqlCommand.ExecuteScalar().ToString();
            sqlConnect.Close();
            Edit.Style.Add("display", "normal");
            View.Style.Add("display", "none");
            txtName.Text = lblName.Text;
            txtDeadline.Text = lblDeadline.Text;
            txtStart.Text = lblStart.Text;
            txtHours.Text = lblHours.Text;
            txtPayment.Text = payment;
            txtDescription.Text = lblDescription.Text;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // define connection to DB and query String
            SqlConnection sqlConnect = new SqlConnection(WebConfigurationManager.ConnectionStrings["OSAG"].ConnectionString);
            String sqlQuery;
            sqlQuery = "UPDATE Internship SET " +
                "InternshipName = @InternshipName, " +
                "InternshipDescription = @InternshipDescription, " +
                "ApplicationDeadline = @ApplicationDeadline, " +
                "StartDate = @StartDate, " +
                "WeeklyHours = @WeeklyHours, " +
                "Payment = @Payment, " +
                "CompanyID = @CompanyID " +
                "WHERE InternshipID = '" + Session["View"].ToString() + "';";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnect);
            sqlCommand.Parameters.AddWithValue("@InternshipName", validate(txtName.Text));
            sqlCommand.Parameters.AddWithValue("@InternshipDescription", validate(txtDescription.Text));
            sqlCommand.Parameters.AddWithValue("@ApplicationDeadline", validate(txtDeadline.Text));
            sqlCommand.Parameters.AddWithValue("@StartDate", validate(txtStart.Text));
            sqlCommand.Parameters.AddWithValue("@WeeklyHours", validate(txtHours.Text));
            sqlCommand.Parameters.AddWithValue("@Payment", validate(txtPayment.Text));
            if (ddlCompany.SelectedValue != "0")
                sqlCommand.Parameters.AddWithValue("@CompanyID", ddlCompany.SelectedValue.ToString());
            else
                sqlCommand.Parameters.AddWithValue("@CompanyID", DBNull.Value);
            sqlConnect.Open();
            sqlCommand.ExecuteScalar();
            sqlConnect.Close();
            Response.Write("<script>alert('Changes successfully saved.');</script>");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            View.Style.Add("display", "normal");
            Edit.Style.Add("display", "none");
        }
        private object validate(String s)
        {
            s = s.Trim();
            if (s == "")
                return (object)DBNull.Value;
            return s;
        }
    }
}