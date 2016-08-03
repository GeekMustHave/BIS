<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="ChangePassword.aspx.cs" Inherits="Account_ChangePassword" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>Change Password
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
                            <div style="width: 80%;">
                                <fieldset class="login">
                                    <legend>Update Account Information</legend>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 65%;">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Old Password:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="CurrentPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                                                CssClass="failureNotification" ErrorMessage="Old Password is required." ToolTip="Old Password is required."
                                                                ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="NewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                                                CssClass="failureNotification" ErrorMessage="New Password is required." ToolTip="New Password is required."
                                                                ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regexPasswordValid" runat="server" Display="Dynamic"
                                                                ValidationExpression="^(?=.*[A-Z])(?=.*[!@#$%^&*()_+<>?:])(?=.*[0-9])(?=.*[a-z]).{8,20}$"
                                                                CssClass="failureNotification" ControlToValidate="NewPassword" ErrorMessage="Invalid Password Format."
                                                                ValidationGroup="ChangeUserPasswordValidationGroup">*
                                                            </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="padding-right: 10px;">
                                                            <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="ConfirmNewPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="Confirm New Password is required."
                                                                ToolTip="Confirm New Password is required." ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                                                CssClass="failureNotification" Display="Dynamic" ErrorMessage="New Password & Confirm New Password must match."
                                                                ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="padding-top: 10px;"></td>
                                                        <td style="padding-top: 10px;" align="left">
                                                            <%--<asp:Button ID="btnCancelUpdate" runat="server" CausesValidation="False" CommandName="Cancel"
                                                         CssClass="customButton" Text="Cancel" />--%>

                                                            <asp:Button ID="btnUpdatePassword" runat="server" CommandName="ChangePassword" Text="Change Password"
                                                                ValidationGroup="ChangeUserPasswordValidationGroup" CausesValidation="true" Width="120px"
                                                                CssClass="customButton" OnClick="btnUpdatePassword_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td style="padding-top: 10px; font-size: 1em;" align="left">
                                                            <asp:ValidationSummary ID="ChangeUserPasswordValidationSummary" runat="server"
                                                                CssClass="failureNotification failureNotificationRegister" HeaderText="Please correct the following"
                                                                ValidationGroup="ChangeUserPasswordValidationGroup" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align: top">
                                                <span style="text-decoration: underline;">New Password Criteria:</span>
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
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
                <br />
                <label style="color: green;">Your Password has been updated successfully.</label>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
