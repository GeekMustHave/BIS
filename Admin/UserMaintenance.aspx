<%@ Page Title="User Maintenance" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true" CodeFile="UserMaintenance.aspx.cs"
    Inherits="Admin_UserMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link href="../Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function OpenSuccessNotification(inMsg) {
            notif({
                msg: inMsg,
                type: "success",
                position: "center",
                multiline: true
            });
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <span style="font-weight: bold">User Maintenance</span>
    <asp:MultiView ID="mvUsers" runat="server">
        <asp:View ID="vwUserList" runat="server">
            <br />
            <br />
            <asp:GridView ID="GridUsersList" runat="server" Width="100%" UseAccessibleHeader="True"
                GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                AllowPaging="true" PageSize="15" CssClass="table table-striped table-bordered table-condensed"
                DataKeyNames="User_GUID,UserName,Email,FirstName,LastName,Active,Department,Roles,LastLoginDate,FullName" OnRowCommand="GridUsersList_RowCommand" OnPageIndexChanging="GridUsersList_PageIndexChanging"
                OnSorting="GridUsersList_Sorting" AllowSorting="true">
                <Columns>
                    <asp:BoundField DataField="UserName" Visible="true" SortExpression="UserName" HeaderText="User Name"
                        HeaderStyle-Font-Underline="false" ItemStyle-Wrap="false"></asp:BoundField>
                    <asp:BoundField HeaderText="Full Name" DataField="FullName" Visible="true" SortExpression="FullName" ItemStyle-Wrap="false" />
                    <asp:BoundField HeaderText="Email" DataField="Email" Visible="true" SortExpression="Email" />
                    <asp:BoundField HeaderText="Roles" DataField="Roles" Visible="true" SortExpression="Roles" />
                    <asp:BoundField HeaderText="Last Login" DataField="LastLoginDate" Visible="true" DataFormatString="{0:d}"
                        SortExpression="LastLoginDate" />
                    <asp:BoundField HeaderText="Active" DataField="Active" Visible="true" SortExpression="Active" />
                    <asp:BoundField HeaderText="Department" DataField="Department" Visible="true" SortExpression="Department" />
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnUserEdit" runat="server" Text="Edit" ToolTip="view User" CommandName="UserEdit"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnUserResetPassword" runat="server" Text="Reset Password" ToolTip="Reset Password" CommandName="UserResetPassword"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Users found.
                </EmptyDataTemplate>
            </asp:GridView>
            <table>
                <tr>
                    <td style="text-align: left; padding-top: 15px;">&nbsp;&nbsp;
                        <asp:Label ID="lblErrorVwList" runat="server" ToolTip="Error Message" Text="" Font-Bold="true"
                            ForeColor="Red">
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwUserVIEWEDIT" runat="server">
            <fieldset>
                <legend>User Details -
                    <asp:Label ID="lblUserFullName" runat="server"></asp:Label>
                </legend>
                <table width="100%">
                    <tr>
                        <td>
                            <span class="EditITLabels">UserName:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" ToolTip="User Name" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorUSerName" runat="server" ControlToValidate="txtFirstName"
                                CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                ValidationGroup="UserUpdateValidationGroup">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <span class="EditITLabels">Last Login:</span>
                        </td>
                        <td>
                            <asp:Label ID="lblLastLogin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">First Name:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" ToolTip="First Name" Width="250px" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="FirstNameRequired" runat="server" ControlToValidate="txtFirstName"
                                CssClass="failureNotification" ErrorMessage="First Name is required." ToolTip="First Name is required."
                                ValidationGroup="UserUpdateValidationGroup">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <span class="EditITLabels">Last Name:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" ToolTip="Last Name" MaxLength="50" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="LastNameRequired" runat="server" ControlToValidate="txtLastName"
                                CssClass="failureNotification" ErrorMessage="Last Name is required." ToolTip="Last Name is required."
                                ValidationGroup="UserUpdateValidationGroup">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Email:</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" ToolTip="Email" MaxLength="50" Width="250px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtEmail"
                                CssClass="failureNotification" ErrorMessage="E-mail is required." ToolTip="E-mail is required."
                                ValidationGroup="UserUpdateValidationGroup">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                CssClass="failureNotification" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format."
                                ValidationGroup="UserUpdateValidationGroup">*</asp:RegularExpressionValidator>
                        </td>
                        <td>
                            <span class="EditITLabels">Active:</span>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkActive" runat="server" ToolTip="Active Inactivate user" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Department:</span>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlDepartment" runat="server" ToolTip="department" Width="200px">
                                <asp:ListItem Text="** Select a Department" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Optum" Value="Optum"></asp:ListItem>
                                <asp:ListItem Text="Customer" Value="Customer"></asp:ListItem>
                                <asp:ListItem Text="3rd party" Value="3rd party"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="DepartmentRequired" runat="server" ControlToValidate="ddlDepartment"
                                CssClass="failureNotification" ErrorMessage="Department Name is required."
                                ToolTip="Department is required." ValidationGroup="UserUpdateValidationGroup"
                                InitialValue="0">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Roles:</span>
                        </td>
                        <td colspan="3">
                            <asp:CheckBoxList ID="cblRoles" runat="server" ToolTip="User Roles" RepeatColumns="3" CellSpacing="1" CellPadding="4">
                                <asp:ListItem Text="Reviewer" Value="Reviewer"></asp:ListItem>
                                <asp:ListItem Text="BA" Value="BA"></asp:ListItem>
                                <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table style="width: 75%;">
                <tr>
                    <td style="text-align: left; padding-top: 15px; height: 15px;">
                        <asp:Button ID="btnUpdateUser" CssClass="customButton" runat="server" Text="Update User" ValidationGroup="UserUpdateValidationGroup"
                            ToolTip="update USer" OnClick="btnUpdateUser_Click" CausesValidation="true" Width="120px"></asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="btnReturnToList" CssClass="customButton" runat="server" Text="Return to List"
                            OnClick="btnReturnToList_Click"  Width="120px" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; padding-top: 15px;">&nbsp;&nbsp;
                        <asp:Label ID="lblErrorVwEdit" runat="server" ToolTip="Error Message" Text="" Font-Bold="true"
                            ForeColor="Red">
                        </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="UserUpdateValidationSummary" runat="server"
                            CssClass="failureNotification failureNotificationRegister" HeaderText="Please correct the following"
                            ValidationGroup="UserUpdateValidationGroup" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
