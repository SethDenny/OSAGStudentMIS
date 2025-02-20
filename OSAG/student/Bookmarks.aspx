﻿<%--bookmarks page--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/templates/Home.Master" AutoEventWireup="true" CodeBehind="Bookmarks.aspx.cs" Inherits="OSAG.student.Bookmarks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        body {
            width: 100%;
            margin: 5px;
        }

        .table-condensed tr th {
            border: 0px solid #6e7bd9;
            color: white;
            background-color: #6e7bd9;
        }

        .table-condensed, .table-condensed tr td {
            border: 0px solid #000;
        }

        tr:nth-child(even) {
            background: #f8f7ff
        }

        tr:nth-child(odd) {
            background: #fff;
        }
    </style>
    <div class="pt-5" style="margin-left: auto; margin-right: auto; text-align: center;">
        <h3>BOOKMARKED JOBS:</h3>
        <asp:GridView ID="grdvwJobs"
            runat="server"
            DataSourceID="sqlsrc"
            AllowSorting="true"
            AutoGenerateSelectButton="false"
            HorizontalAlign="Center"
            AutoGenerateColumns="false" DataKeyNames="JobID"
            CssClass="card-body border-0 shadow p-3 mb-2 bg-body rounded shadow--on-hover"
            PagerStyle-CssClass="pager"
            HeaderStyle-CssClass="header"
            RowStyle-CssClass="rows"
            CellPadding="25"
            CellSpacing="7"
            font="Roboto"
            ForeColor="black"
            AllowPaging="true"
            PageSize="10">
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="<<" LastPageText=">>" />
            <PagerStyle HorizontalAlign="Center" Font-Names="Roboto" Font-Size="Large" Font-Bold="true" ForeColor="#73637F" />
            <Columns>
                <asp:BoundField HeaderText="JobName" DataField="JobName" SortExpression="JobName" />
                <asp:BoundField HeaderText="CompanyName" DataField="CompanyName" SortExpression="CompanyName" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="sqlsrc" runat="server"
            ConnectionString="<%$ ConnectionStrings:OSAG %>"></asp:SqlDataSource>
        <br />
        <br />
        <h3>BOOKMARKED INTERNSHIPS:</h3>
        <asp:GridView ID="grdvwInternships"
            runat="server"
            DataSourceID="sqlsrc2"
            AllowSorting="true"
            AutoGenerateSelectButton="false"
            HorizontalAlign="Center"
            AutoGenerateColumns="false" DataKeyNames="InternshipID"
            CssClass="card-body border-0 shadow p-3 mb-2 bg-body rounded shadow--on-hover"
            PagerStyle-CssClass="pager"
            HeaderStyle-CssClass="header"
            RowStyle-CssClass="rows"
            CellPadding="25"
            CellSpacing="7"
            font="Roboto"
            ForeColor="black"
            AllowPaging="true"
            PageSize="10">
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="<<" LastPageText=">>" />
            <PagerStyle HorizontalAlign="Center" Font-Names="Roboto" Font-Size="Large" Font-Bold="true" ForeColor="#73637F" />
            <Columns>
                <asp:BoundField HeaderText="InternshipName" DataField="InternshipName" SortExpression="InternshipName" />
                <asp:BoundField HeaderText="CompanyName" DataField="CompanyName" SortExpression="CompanyName" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnView2" runat="server" Text="View" OnClick="btnView2_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="sqlsrc2" runat="server"
            ConnectionString="<%$ ConnectionStrings:OSAG %>"></asp:SqlDataSource>
        <br />
        <br />
        <h3>BOOKMARKED OPPORTUNITIES</h3>
        <asp:GridView ID="grdvwOpportunities"
            runat="server"
            DataSourceID="sqlsrc3"
            AllowSorting="true"
            AutoGenerateSelectButton="false"
            HorizontalAlign="Center"
            AutoGenerateColumns="false" DataKeyNames="OpportunityID"
            CssClass="card-body border-0 shadow p-3 mb-2 bg-body rounded shadow--on-hover"
            PagerStyle-CssClass="pager"
            HeaderStyle-CssClass="header"
            RowStyle-CssClass="rows"
            CellPadding="25"
            CellSpacing="7"
            font="Roboto"
            ForeColor="black"
            AllowPaging="true"
            PageSize="10">
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="<<" LastPageText=">>" />
            <PagerStyle HorizontalAlign="Center" Font-Names="Roboto" Font-Size="Large" Font-Bold="true" ForeColor="#73637F" />
            <Columns>
                <asp:BoundField HeaderText="Opportunity Name" DataField="OpportunityName" SortExpression="OpportunityName" />
                <asp:BoundField HeaderText="Description" DataField="OpportunityDescription" SortExpression="OpportunityDescription" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnView3" runat="server" Text="View" OnClick="btnView3_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="sqlsrc3" runat="server"
            ConnectionString="<%$ ConnectionStrings:OSAG %>"></asp:SqlDataSource>
    </div>
</asp:Content>
