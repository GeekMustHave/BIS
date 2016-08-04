<%@ Page Title="Register" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="Register.aspx.cs" Inherits="Account_Register" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit" async defer></script>
    <script type="text/javascript">
        var onloadCallback = function () {
            grecaptcha.render('recaptcha', {
                'sitekey': '6LfpNyYTAAAAAL2xoyDHp6u7r-3kP2PaUARua-r8'
            });
        };
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="divLoginPageText">
        <span id="rtr-s-Rich_text_1_0">Welcome to the Build it Simple. Optum Business Requirement,
            Use-Case, Business Rule and Test-Case manager. &nbsp;
            <br />
            <br />
            Use the form below to create a new account.
            <br />
        </span>
    </div>
    <asp:UpdatePanel ID="updatePanelReq" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td align="center">
                        <div style="margin: 0 auto !important;">
                            <span class="failureNotification">
                                <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                            </span>
                            <div style="width: 80%;">
                                <fieldset class="login">
                                    <legend>Create Account Information</legend>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 65%;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                                                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstName">First Name:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="FirstName" runat="server" CssClass="textEntry"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="FirstName"
                                                                CssClass="failureNotification" ErrorMessage="First Name is required." ToolTip="First Name is required."
                                                                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastName">Last Name:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="LastName" runat="server" CssClass="textEntry"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="LastName"
                                                                CssClass="failureNotification" ErrorMessage="Last Name is required." ToolTip="Last Name is required."
                                                                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="EmailTextBox">E-mail:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="EmailTextBox" runat="server" CssClass="textEntry"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="EmailTextBox"
                                                                CssClass="failureNotification" ErrorMessage="E-mail is required." ToolTip="E-mail is required."
                                                                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                CssClass="failureNotification" ControlToValidate="EmailTextBox" ErrorMessage="Invalid Email Format." ValidationGroup="RegisterUserValidationGroup">*
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required."
                                                                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regexPasswordValid" runat="server"
                                                                ValidationExpression="^(?=.*[A-Z])(?=.*[!@#$%^&*()_+<>?:])(?=.*[0-9])(?=.*[a-z]).{8,20}$"
                                                                CssClass="failureNotification" ControlToValidate="Password" ErrorMessage="Invalid Password Format." ValidationGroup="RegisterUserValidationGroup">*
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="failureNotification"
                                                                Display="Dynamic" ErrorMessage="Confirm Password is required." ID="ConfirmPasswordRequired"
                                                                runat="server" ToolTip="Confirm Password is required."
                                                                ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                                                                ControlToValidate="ConfirmPassword" CssClass="failureNotification" Display="Dynamic"
                                                                ErrorMessage="Password & Confirm Password must match."
                                                                ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="DepartmentLabel" runat="server" AssociatedControlID="Department">Department:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="Department" runat="server" ToolTip="select a department" CausesValidation="true"
                                                                Width="172px">
                                                                <asp:ListItem Text="** Select a Department" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Optum" Value="Optum"></asp:ListItem>
                                                                <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                                                                <asp:ListItem Text="3rd party" Value="3rd party"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="DepartmentRequired" runat="server" ControlToValidate="Department"
                                                                CssClass="failureNotification" ErrorMessage="Department Name is required."
                                                                ToolTip="Department is required." ValidationGroup="RegisterUserValidationGroup"
                                                                InitialValue="0">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-top: 10px;"></td>
                                                        <td style="padding-top: 10px;" align="left">
                                                            <div id="recaptcha" class="recaptcha"></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-top: 10px;"></td>
                                                        <td style="padding-top: 10px;" align="left">
                                                            <asp:Button ID="CreateUserButton" runat="server" Text="Create Account"
                                                                Width="120px" CssClass="customButton" ValidationGroup="RegisterUserValidationGroup"
                                                                OnClick="CreateUserButton_Click" CausesValidation="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td style="padding-top: 10px; font-size: 1em;" align="left">
                                                            <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server"
                                                                CssClass="failureNotification failureNotificationRegister" HeaderText="Please correct the following"
                                                                ValidationGroup="RegisterUserValidationGroup" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align: top">
                                                <span style="text-decoration: underline;">Password Criteria:</span>
                                                <ul>
                                                    <li>Between 8 to 20 characters long</li>
                                                    <li>At least 1 uppercase character</li>
                                                    <li>At least 1 lowercase character</li>
                                                    <li>At least 1 special character</li>
                                                    <li>At least 1 number</li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
