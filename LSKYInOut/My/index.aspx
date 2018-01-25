<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Unprotected.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYInOut.My.index1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Cache-Control" content="no-store" />
    <link href="../styles/my.css" rel="stylesheet" /> 
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page_container">
                    <asp:Literal ID="litStatus" runat="server"></asp:Literal>          
        
        <asp:Table ID="tblUserSelect" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <!--<div class="helpTextAbove">Update</div> -->
                    <asp:DropDownList ID="drpUsers" runat="server"></asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Button ID="btnSelectUser" runat="server" Text="Start updating" OnClick="btnSelectUser_Click" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>      



        <asp:Table ID="tblUpdateControls" runat="server" Visible="false" BorderWidth="2" Width="100%">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:HiddenField ID="txtUserID" runat="server" />
                    <div class="user_info_heading">
                        <div class="selectedUserName"><asp:Label ID="lblSelectedUser" runat="server" Text="USER"></asp:Label></div>
                    </div>

                    <div class="user_info_current_statuses">
                        <div class="section_title">Current statuses:</div>
                        <asp:CheckBoxList ID="chkSatusList" runat="server" CssClass="user_status"></asp:CheckBoxList>
                    </div>
                    

                    <div class="user_info_add_statuses">   
                        <div class="section_title">Remove statuses:</div>
                        <asp:Button ID="btnRemoveCheckedStatuses" runat="server" Text="Remove Checked Statuses" OnClick="btnRemoveCheckedStatuses_Click" />
                        <div class="section_title">Add a status:</div>                   
                        <asp:Table ID="tblAddStatuses" runat="server" Width="100%">
                            <asp:TableRow>                                
                                <asp:TableCell Width="25%"><asp:DropDownList ID="drpStatuses" runat="server" Width="100%"></asp:DropDownList></asp:TableCell>
                                <asp:TableCell Width="5%">until</asp:TableCell>
                                <asp:TableCell Width="25%"><asp:DropDownList ID="drpExpiry" Width="95%" runat="server"></asp:DropDownList></asp:TableCell>
                                <asp:TableCell Width="45%"><asp:Button ID="btnSetStatus" runat="server" Text="Add" OnClick="btnSetStatus_Click"/></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>     
                        <!--
                        <div class="section_title">Add a custom status:</div>                    
                        <asp:Table ID="tblAddCustomStatus" runat="server" Width="100%">                            
                            <asp:TableRow>                                
                                <asp:TableCell Width="25%"><asp:TextBox ID="txtCustomStatus" runat="server" Width="90%"></asp:TextBox></asp:TableCell>
                                <asp:TableCell Width="5%">until</asp:TableCell>
                                <asp:TableCell Width="25%">
                                    <asp:DropDownList ID="drpCustomExpiry" runat="server" Width="95%"></asp:DropDownList>
                                </asp:TableCell>
                                <asp:TableCell Width="45%"><asp:Button ID="btnAddCustomStatus" runat="server" Text="Add" OnClick="btnAddCustomStatus_Click"/></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>                        
                        -->
                    </div>     
                    </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        
    </div>
</asp:Content>
