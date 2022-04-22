﻿<%@ Page Title="" Language="C#" MasterPageFile="~/templates/Home.Master" AutoEventWireup="true" CodeBehind="DeclareMajor.aspx.cs" Inherits="OSAG.profiles.DeclareMajor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <div style="margin-right: auto; margin-left: auto; text-align: center">
        <h4>
            <asp:Label ID="lblHead" runat="server" Text="Add Major(s)"></asp:Label></h4>
        <div class="input-group mb-3 px-0" style="width: 30vh; margin-left: auto; margin-right: auto">
            <asp:DropDownList ID="ddlMajors" runat="server"
                AutoPostBack="true"
                DataSourceID="sqlsrcMajors"
                DataTextField="MajorName"
                DataValueField="MajorID"
                CssClass="form-control">
            </asp:DropDownList>
            <asp:Button ID="btnAddMajor" runat="server" Text="ADD" CssClass="btn btn-primary" OnClick="btnAddMajor_Click" />
        </div>

        <div class="pt-4 pb-5">
            <asp:Button ID="btnDoneMajors" runat="server" Text="DONE" CssClass="btn btn-secondary" OnClick="btnDoneMajors_Click" />
        </div>

        <h4>Current Majors/Minors</h4>
        <asp:GridView ID="grdvHasMajor" runat="server" DataSourceID="sqlsrcHasMajor"
            AutoGenerateColumns="false"
            AllowPaging="true"
            CellPadding="10"
            DataKeyNames="MajorID, StudentID, MemberID"
            HorizontalAlign="Center"
            ShowHeaderWhenEmpty="true"
            AutoGenerateDeleteButton="true"
            OnRowDataBound="grdvHasMajor_RowDataBound"
            CssClass="card-body border-0 shadow p-3 mb-2 bg-body rounded shadow--on-hover"
            HeaderStyle-CssClass="header"
            RowStyle-CssClass="rows"
            CellSpacing="7"
            font="Roboto" OnRowDeleted="grdvHasMajor_RowDeleted">
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="<<" LastPageText=">>" />
            <PagerStyle HorizontalAlign="Center" Font-Names="Roboto" Font-Size="Large" Font-Bold="true" ForeColor="#73637F" />
            <Columns>
                <asp:BoundField ReadOnly="true" HeaderText="Major/Minor Name" DataField="MajorName" />
                <asp:BoundField ReadOnly="true" HeaderText="Type" DataField="IsMinor" NullDisplayText="Major" />
            </Columns>
        </asp:GridView>

        <asp:SqlDataSource ID="sqlsrcMajors" runat="server"
            ConnectionString="<%$ ConnectionStrings:OSAG %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlsrcHasMajor" runat="server"
            ConnectionString="<%$ ConnectionStrings:OSAG %>"
            DeleteCommand="DELETE FROM HasMajor WHERE MajorID = @MajorID 
            AND ((StudentID = @StudentID AND MemberID IS NULL) 
            OR (MemberID = @MemberID AND StudentID IS NULL))"></asp:SqlDataSource>
    </div>
</asp:Content>
