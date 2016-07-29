<%@ Page Title="Log In" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Account_Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="divLoginPageText">
        <span id="rtr-s-Rich_text_1_0">Welcome to the Build it Simple. Optum Business Requirement,
            Use-Case, Business Rule and Test-Case manager. &nbsp;
            <br />
            <br />
            You must login with the User ID and Password assigned to you by the BIS administrator.<br>
            <br />
            Navigate through the system by using the menu selections at the top of the page.</span>
    </div>
    <table style="width: 100%;">
        <tr>
            <td align="center">
                <div style="margin: 0 auto !important;">
                    <span class="failureNotification">
                        <br />
                        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                    </span>
                    <div style="width: 50%;">
                        <fieldset class="login">
                            <legend>Account Information</legend>
                            <table style="width: 100%;">
                                <tr>
                                    <td align="right" style="padding-right: 10px;">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User ID</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                            CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                            ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" style="padding-right: 10px;">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password</asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"
                                            Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                            CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required."
                                            ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="RememberMe" runat="server" AutoPostBack="false" />
                                        <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Stay signed in</asp:Label>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="LoginButton" runat="server" Text="Log In" ValidationGroup="LoginUserValidationGroup"
                                            OnClick="LoginButton_Click" CssClass="customButton" Width="80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:HyperLink ID="btnRegisterUser" EnableViewState="false" CssClass="loginPageLinkButton" runat="server" 
                                            Text="Register" ToolTip="click to create an account" NavigateUrl="~/Account/Register.aspx">
                                        </asp:HyperLink>
                                         &nbsp;|&nbsp;
                                        <asp:HyperLink ID="btnForgotPassword" EnableViewState="false" CssClass="loginPageLinkButton"  runat="server" 
                                            Text="Lost your password?" ToolTip="click to reset your password" NavigateUrl="~/Account/PasswordReset.aspx">
                                        </asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
