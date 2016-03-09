<%@ Page Title="Issue Track" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="IssueTrack.aspx.cs" Inherits="Admin_IssueTrack" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link href="../Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function DownloadFile(filename) {
            javascript: window.open("../Handlers/JIRADocHandler.ashx?ResourceURL=" + filename);
            return false;
        }
    </script>
    <style type="text/css">
        /* Some Styles to override for Issue Track Page.*/
        .CustText
        {
            max-width: 100% !important;
            width: 100% !important;
        }
        .EditITLabels
        {
            color: #707070;
            padding: 2px 5px 2px 0;
            word-wrap: break-word;
        }
        fieldset
        {
            /*margin: 0 !important;*/
            padding: 0em 0.5em 0.2em 0.5em !important;
        }
        legend
        {
            font-weight: normal !important;
        }
    </style>
    <script type="text/javascript">
        function OpenITCreateNotification(inMsg) {
            notif({
                msg: "Succesfully Created Issue Track: <b>" + inMsg + "</b> <br />  This message will disappear in 5 seconds.",
                type: "success",
                position: "center",
                multiline: true
            });
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <span style="font-weight: bold">Issue Track </span>
    <asp:MultiView ID="mvIssueTrack" runat="server">
        <asp:View ID="vwITList" runat="server">
            <br />
            <table>
                <tr>
                    <td align="left" style="padding-bottom: 5px; padding-top: 5px; padding-right: 15px;">
                        <span style="font-weight: bold">Status: </span>
                    </td>
                    <td align="left" style="padding-bottom: 5px; padding-top: 5px;">
                        <asp:DropDownList ID="ddlSearchByStatus" runat="server" ToolTip="Filter by IT Status"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSearchByStatusNType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td align="left" style="padding-bottom: 5px; padding-top: 5px; padding-right: 15px;
                        padding-left: 15px;">
                        <span style="font-weight: bold">Type: </span>
                    </td>
                    <td align="left" style="padding-bottom: 5px; padding-top: 5px;">
                        <asp:DropDownList ID="ddlSearchByType" runat="server" ToolTip="Filter by IT Type"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlSearchByStatusNType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="GridIssueTrackList" runat="server" Width="100%" UseAccessibleHeader="True"
                GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                AllowPaging="true" PageSize="15" CssClass="table table-striped table-bordered table-condensed"
                DataKeyNames="summary,key,id" OnRowCommand="GridIssueTrackList_RowCommand" OnPageIndexChanging="GridIssueTrackList_PageIndexChanging"
                OnSorting="GridIssueTrackList_Sorting" AllowSorting="true">
                <Columns>
                    <asp:BoundField DataField="key" Visible="true" SortExpression="id" HeaderText="Issue ID"
                        HeaderStyle-Font-Underline="false" ItemStyle-Wrap="false"></asp:BoundField>
                    <asp:TemplateField HeaderText="Summary" SortExpression="summary" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSummary" runat="server" Text='<%#Eval("summary") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Status" DataField="status" Visible="true" SortExpression="status" ItemStyle-Wrap="false"/>
                    <asp:BoundField HeaderText="Type" DataField="issuetype" Visible="true" SortExpression="issuetype" ItemStyle-Wrap="false" />
                    <%--<asp:BoundField HeaderText="Assigned To" DataField="assignee" Visible="true" SortExpression="assignee" />--%>
                    <asp:BoundField HeaderText="Priority" DataField="priority" Visible="true" SortExpression="priority" />
                    <asp:BoundField HeaderText="Created Date" DataField="created" Visible="true" DataFormatString="{0:d}"
                        SortExpression="created" />
                    <asp:BoundField HeaderText="UserId" DataField="ssouserid" Visible="true" SortExpression="ssouserid" />
                    <asp:BoundField HeaderText="Sprint" DataField="sprints" Visible="true" ItemStyle-Wrap="false" SortExpression="sprints" />
                    <asp:TemplateField SortExpression="ObjectShortDescription">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnITEdit" runat="server" Text="Edit" ToolTip="view IT" CommandName="editIT"
                                CommandArgument='<%#Eval("key") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Issue Tracks are found.
                </EmptyDataTemplate>
            </asp:GridView>
            <table>
                <tr>
                    <td style="text-align: right; padding-top: 5px; height: 15px;">
                        <asp:Button ID="btnCreateIT" runat="server" Text="Create Issue Track" CssClass="customButton"
                            ToolTip="Create Issue Track" OnClick="btnCreateIT_Click"></asp:Button>
                    </td>
                    <td style="text-align: left; padding-top: 15px;">
                        &nbsp;&nbsp;
                        <asp:Label ID="lblErrorVwList" runat="server" ToolTip="Error Message" Text="" Font-Bold="true"
                            ForeColor="Red">
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwITVIEWEDIT" runat="server">
            <fieldset>
                <legend>Details -
                    <asp:Label ID="lblITID" runat="server"></asp:Label></legend>
                <table width="100%">                    
                    <tr>
                        <td>
                            <span class="EditITLabels">Sprint:</span>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lblSprint" runat="server" Width="100%" ToolTip="Sprint the IT assigned To"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" id="trCreatedBy" visible="true">
                        <td>
                            <span class="EditITLabels">Created By:</span>
                        </td>
                        <td>
                            <asp:Label ID="lblCreatebySSO" runat="server"></asp:Label>
                        </td>
                        <td>
                            <span class="EditITLabels">Created On:</span>
                        </td>
                        <td>                            
                            <asp:Label ID="lblCreatedOn" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Summary:</span>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="lblSummaryEdit" runat="server" TextMode="SingleLine" Width="100%"
                                Height="20px" ToolTip="Description" CssClass="CustText"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Type:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlITTypeEdit" runat="server" Visible="false">
                            </asp:DropDownList>
                            <asp:Label ID="lblITtypeEdit" runat="server"></asp:Label>
                        </td>
                        <td>
                            <span class="EditITLabels">IT Status:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlITStatusEdit" runat="server" Enabled="false" Visible="false">
                            </asp:DropDownList>
                            <asp:Label ID="lblITStatusEdit" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Priority:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlITPriorityEdit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span class="EditITLabels">Resolution Status:</span>
                        </td>
                        <td>
                            <asp:Label ID="lblResolutionEdit" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Action:</span>
                        </td>
                        <td style="padding-top: 3px;">
                            <asp:PlaceHolder ID="phTransitions" runat="server">
                                <asp:LinkButton ID="btnITAction_1" runat="server" OnClick="ITActionbtn_click" Visible="false"></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnITAction_2" runat="server" OnClick="ITActionbtn_click"
                                    Visible="false"></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnITAction_3" runat="server" OnClick="ITActionbtn_click"
                                    Visible="false"></asp:LinkButton>
                            </asp:PlaceHolder>
                        </td>
                        <td>
                            <span class="EditITLabels">Resolution:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlITResolutionTypeEdit" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Description:</span>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDescriptionEdit" runat="server" TextMode="MultiLine" CssClass="CustText"
                                Height="50px" ToolTip="Description"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Comments:</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <span class="EditITLabels">Comments:</span>
                        </td>
                        <td>
                            <div id="divCommentsEdit" runat="server" style="">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Add New Comment:</span>
                        </td>
                        <td style="width: 85%;">
                            <asp:TextBox ID="txtAddCommentEdit" runat="server" TextMode="MultiLine" CssClass="CustText"
                                Height="50px" ToolTip="Description"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td valign="middle" style="width: auto;">
                            <asp:Button ID="btnAddComment" runat="server" CssClass="customButton" Text="Add Comment"
                                ToolTip="click to Add" OnClick="btnAddComment_Click"></asp:Button>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <fieldset>
                <legend>Attachments:</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <span class="EditITLabels">Attachments:</span>
                        </td>
                        <td>
                            <div id="divAttachments" runat="server">
                                <asp:PlaceHolder ID="phAttachmentsEdit" runat="server"></asp:PlaceHolder>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Add New Attachment:</span>
                        </td>
                        <td style="width: 85%;">
                            <asp:FileUpload ID="fileUpload" runat="server" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnUploadNewFile" runat="server" Text="Upload" CssClass="customButton"
                                OnClick="btnUploadNewFile_Click" />
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table>
                <tr>
                    <td style="text-align: right; padding-top: 15px; height: 15px;">
                        <asp:Button ID="btnUpdateIssueTRackEdit" CssClass="customButton" runat="server" Text="Update IssueTrack"
                            ToolTip="update Issue Track" OnClick="btnUpdateIssueTRackEdit_Click"></asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="btnReturnToList" CssClass="customButton" runat="server" Text="Return to List"
                            OnClick="btnReturnFromViewEdit_Click" />&nbsp;
                    </td>
                    <td style="text-align: left; padding-top: 15px;">
                        &nbsp;&nbsp;
                        <asp:Label ID="lblErrorVwEdit" runat="server" ToolTip="Error Message" Text="" Font-Bold="true"
                            ForeColor="Red">
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwITCREATE" runat="server">
            <fieldset>
                <legend>Create Issue Track </legend>
                <table width="95%">
                    <tr>
                        <td style="padding-top: 3px;">
                            <span class="EditITLabels">Summary: </span>
                        </td>
                        <td style="width: 80%">
                            <asp:TextBox ID="txtSummaryNew" runat="server" TextMode="SingleLine" CssClass="CustText"
                                Height="20px" ToolTip="Description"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Priority:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPriorityNew" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trAssignToNew" runat="server" visible="false">
                        <td>
                            <span class="EditITLabels">Assign To:</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAssigneeNew" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span class="EditITLabels">Description: </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptionNew" runat="server" TextMode="MultiLine" CssClass="CustText"
                                Height="130px" ToolTip="Description"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <table>
                <tr>
                    <td style="text-align: right; padding-top: 15px; height: 15px;">
                        <asp:Button ID="btnAddIssueTrack" runat="server" Text="Add Issue Track" CausesValidation="true"
                            ValidationGroup="valGroupNew" ToolTip="Create Issue Track" CssClass="customButton"
                            OnClick="btnAddIssueTrack_Click"></asp:Button>&nbsp;&nbsp;
                        <asp:Button ID="btnReturntoList2" runat="server" CssClass="customButton" Text="Return to List"
                            OnClick="btnReturnFromViewEdit_Click" />&nbsp;&nbsp;
                    </td>
                    <td style="text-align: left; padding-top: 15px;">
                        &nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="rfvNewsummary" runat="server" ValidationGroup="valGroupNew"
                            Font-Bold="true" ControlToValidate="txtSummaryNew" ErrorMessage=" *Summary Required "
                            Display="Dynamic" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                        &nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="rfvNewDescription" runat="server" ValidationGroup="valGroupNew"
                            Font-Bold="true" ControlToValidate="txtDescriptionNew" ErrorMessage=" *Description Required "
                            Display="Dynamic" ForeColor="Red">
                        </asp:RequiredFieldValidator>
                        &nbsp;&nbsp;
                        <asp:Label ID="lblErrorVwNew" runat="server" ToolTip="Error Message" Text="" Font-Bold="true"
                            ForeColor="Red">
                        </asp:Label>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
