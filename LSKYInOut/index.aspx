<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Unprotected.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYInOut.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <asp:Literal ID="litLinks" runat="server"></asp:Literal>
    <asp:Table ID="tblAllUsersStatus" runat="server"></asp:Table>
</asp:Content>
