﻿<%@ Page Title="" Language="C#" MasterPageFile="~/templates/Home.Master" AutoEventWireup="true" CodeBehind="StartChat.aspx.cs" Inherits="OSAG.profiles.StartChat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="margin-left: auto; margin-right: auto; text-align: center;">
        <h3>Select a User to Chat With</h3>
        <br />
        <h3>Students:</h3>
        <asp:GridView ID="grdvStudent"
            runat="server"
            DataSourceID="sqlsrcStudent"
            AllowSorting="true"
            AutoGenerateSelectButton="false" 
            HorizontalAlign="Center"
            AutoGenerateColumns="false" DataKeyNames="StudentID">
            <Columns>
                <asp:BoundField HeaderText="FirstName" DataField="FirstName" SortExpression="FirstName" />
                <asp:BoundField HeaderText="LastName" DataField="LastName" SortExpression="LastName" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btn_studentChat" runat="server" Text="Chat" OnClick="btn_studentChat_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="sqlsrcStudent" runat="server"
            ConnectionString="<%$ ConnectionStrings:OSAG %>"></asp:SqlDataSource>
        
        <br />
        <h3>Members:</h3>
        <asp:GridView ID="grdvMember"
            runat="server"
            DataSourceID="sqlsrcMember"
            AllowSorting="true"
            AutoGenerateSelectButton="false" 
            HorizontalAlign="Center"
            AutoGenerateColumns="false" DataKeyNames="MemberID">
            <Columns>
                <asp:BoundField HeaderText="FirstName" DataField="FirstName" SortExpression="FirstName" />
                <asp:BoundField HeaderText="LastName" DataField="LastName" SortExpression="LastName" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btn_memberChat" runat="server" Text="Chat" OnClick="btn_memberChat_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="sqlsrcMember" runat="server"
            ConnectionString="<%$ ConnectionStrings:OSAG %>"></asp:SqlDataSource>
    </div>
</asp:Content>
