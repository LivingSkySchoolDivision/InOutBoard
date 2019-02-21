<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Unprotected.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYInOut.My.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    <!-- Tell Safari that this is an App, not a website -->
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="viewport" content="user-scalable=1.0,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0">
    <meta name="format-detection" content="telephone=no">
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <link href="../styles/ipad.css" rel="stylesheet" />  

    <meta http-equiv="Cache-Control" content="no-store" />

    <!-- Apple will open links in a new window, unless we do this -->
    <script src="../jquery3.2.1.js"></script>
    <script src="../AppleLinks.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <div class="ipad_sized_page_container">
        <asp:Literal ID="litStaffList" runat="server"></asp:Literal>
        <asp:Literal ID="litStatusList" runat="server"></asp:Literal>
        <asp:Literal ID="litExpiration" runat="server"></asp:Literal>
        <asp:Literal ID="litReceipt" runat="server"></asp:Literal>
    </div>
</asp:Content>
