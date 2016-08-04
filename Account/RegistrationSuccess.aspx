<%@ Page Title="Registration Successfull" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="RegistrationSuccess.aspx.cs" Inherits="Account_RegistrationSuccess" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Registration Successfull
    </h2>
    <p>
        You have successfully registered as a new user on the system. An email confirmation has been sent to you regarding this.
        <br />
        However, your account is <i>NOT</i> active untill the Administrator approves it, you will recieve another email
        when that has occured.
        <br />        
        <br />
        -BIS Admin.
    </p>
</asp:Content>
