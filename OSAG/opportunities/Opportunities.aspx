﻿<%--Displays available opportunities--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/templates/Home.Master" AutoEventWireup="true" CodeBehind="Opportunities.aspx.cs" Inherits="OSAG.opportunities.Opportunities" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%if (Session["UserType"].ToString() == "student")
        { %>
    <!--Banner starts-->
    <div style="text-align: center">
        <asp:Image ID="Image1" runat="server" ImageUrl="/_images/banners/opportunitybanner.png" Height="400px" />
        <br />
    </div>
    <!--Banner ends-->
    <%} %>

    <div class="container col-lg-4 text-center pt-5 ">
        <h2>UPCOMING OPPORTUNITIES</h2>
    </div>

    <div class="container col-lg-9 px-4">
        <!--Search bar starts-->
        <div class="pt-5 pb-5 px-4">
            <div class="input-group" style="margin-left: auto; margin-right: auto; text-align: center; padding-right: unset">
                <div class="input-group-text">
                    <div class="icon icon-lg">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-search">
                            <circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line></svg>
                    </div>
                </div>
                <input id="searchBar" runat="server" type="text" class="form-control form-control-lg ps-3" placeholder="Search opportunities" aria-label="Search opportunities">
                <asp:Button ID="btnSearch" runat="server" CssClass="input-group-text" Text="Search" OnClick="btnSearch_Click" />
            </div>
        </div>
        <!--Search bar ends-->
    </div>

    <asp:GridView ID="grdvwOpportunities"
        runat="server"
        DataSourceID="sqlsrc"
        AllowSorting="true"
        AutoGenerateSelectButton="false"
        AutoGenerateColumns="false"
        HorizontalAlign="Center"
        DataKeyNames="OpportunityID"
        CssClass="card-body border-0 shadow p-3 mb-2 bg-body rounded shadow--on-hover"
        HeaderStyle-CssClass="header"
        RowStyle-CssClass="rows"
        CellPadding="25"
        CellSpacing="7"
        font="Roboto"
        ForeColor="black"
        AllowPaging="true"
        PageSize="10" RowStyle-BackColor="#f8f7ff" AlternatingRowStyle-BackColor="#ffffff">
        <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="<<" LastPageText=">>" />
        <PagerStyle HorizontalAlign="Center" Font-Names="Roboto" Font-Size="Large" Font-Bold="true" ForeColor="#73637F" />
        <Columns>
            <asp:BoundField HeaderText="Opportunity Name" DataField="OpportunityName" SortExpression="OpportunityName" />
            <asp:BoundField HeaderText="Company" DataField="CompanyName" SortExpression="CompanyName" NullDisplayText="N/A" />
            <asp:BoundField HeaderText="Event Date" DataField="EventDate" DataFormatString="{0:MMMM dd, yyyy}" SortExpression="EventDate" NullDisplayText="N/A" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" />
                    <%if (Session["UserType"].ToString() == "student")
                        {%>
                    <asp:Button ID="btnBookmark" runat="server" Text="Bookmark" OnClick="btnBookmark_Click" />
                    <%} %>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:SqlDataSource ID="sqlsrc" runat="server"
        ConnectionString="<%$ ConnectionStrings:OSAG %>"
        SelectCommand="SELECT OpportunityID, OpportunityName, EventDate, CompanyName FROM Opportunity o LEFT JOIN Company c on o.CompanyID = c.CompanyID WHERE EventDate > GETDATE() ORDER BY EventDate ASC"></asp:SqlDataSource>
</asp:Content>
