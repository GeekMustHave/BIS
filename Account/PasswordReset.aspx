<%@ Page Title="Password Reset" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true" CodeFile="PasswordReset.aspx.cs" Inherits="Account_PasswordReset" %>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript" src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit" async defer></script>
    <script type="text/javascript">
        var onloadCallback = function () {
            grecaptcha.render('recaptcha', {
                'sitekey': '6LfpNyYTAAAAAL2xoyDHp6u7r-3kP2PaUARua-r8'
            });
        };
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <h2>Password Reset
    </h2>
    <asp:UpdatePanel ID="updatePanelReq" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;">
                <tr>
                    <td align="center">
                        <div style="margin: 0 auto !important;">
                            <span class="failureNotification">
                                <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                            </span>
                            <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
                                <br />
                                <label style="color: green;">Your Password has been reset successfully. Please check your email for the Temporary password.!</label>
                            </asp:Panel>
                            <div style="width: 60%;">
                                <fieldset class="login">
                                    <legend>Password Reset</legend>
                                    <span>Please enter your UserName or Email associated with your account.</span>
                                    <br />
                                    <br />
                                    <table style="width: 100%;">
                                        <tr>
                                            <td align="right" style="padding-right: 10px;">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                                &nbsp; (OR)
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="padding-right: 10px;">
                                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="EmailTextBox">E-mail:</asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="EmailTextBox" runat="server" CssClass="textEntry"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                    CssClass="failureNotification" ControlToValidate="EmailTextBox" ErrorMessage="Invalid Email Format."
                                                    ValidationGroup="ResetUserPasswordValidationGroup">*</asp:RegularExpressionValidator>
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
                                                <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password"
                                                    ValidationGroup="ResetUserPasswordValidationGroup" CausesValidation="true" Width="120px"
                                                    CssClass="customButton" OnClick="btnResetPassword_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td style="padding-top: 10px; font-size: 1em;" align="left">
                                                <asp:ValidationSummary ID="ResetUserPasswordValidationSummary" runat="server"
                                                    CssClass="failureNotification failureNotificationRegister" HeaderText="Please correct the following"
                                                    ValidationGroup="ResetUserPasswordValidationGroup" />
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

