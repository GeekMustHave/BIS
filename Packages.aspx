<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="Packages.aspx.cs" Inherits="_Packages" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://tinymce.cachefly.net/4.1/tinymce.min.js"></script>
    <script type="text/javascript">
        tinymce.init({
            selector: ".tinymce",
            theme: "modern",
            theme_advanced_toolbar_location: "top",
            removed_menuitems: 'newdocument',
            plugins: [
                "advlist autolink lists link charmap print preview anchor",
                "searchreplace visualblocks code fullscreen",
                "insertdatetime paste"
            ],
            toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image",
            encoding: "xml",
            invalid_elements: 'meta,script,object,applet,iframe',
            height: "250",
            browser_spellcheck: true,
            contextmenu: false
        });
    </script>
    <style type="text/css">
        .mce-menu
        {
            z-index: 1000003 !important;
        }
        .mce-container
        {
            z-index: 1000004 !important;
        }
        .mce-path
        {
            /* CSS */
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table width="100%">
        <tr>
            <td align="left" style="padding-bottom: 5px;">
                <span style="font-weight: bold">Search </span>
            </td>
            <td>
                <asp:TextBox ID="txtSearchProjPackage" runat="server" Width="350px" ToolTip="Enter partial Package name to search"></asp:TextBox>
                &nbsp;&nbsp;
                <asp:Button ID="btnSearchProjPackage" runat="server" Text="Refresh" CssClass="customButton" OnClick="btnSearchProjPackage_Click" />                                    
            </td>
        </tr>
        <tr>
            <td align="left" style="padding-bottom: 5px;">
                <span style="font-weight: bold">Packages </span>
            </td>
            <td align="right" style="padding-bottom: 5px;">
                <asp:Button ID="btnAddProject" runat="server" Text="Add Project" CssClass="customButton"
                    OnClick="btnAddProject_Click" />
                <asp:Button ID="btnAddPackage" runat="server" Text="Add Package" CssClass="customButton"
                    OnClick="btnAddPackage_Click" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridPackages" runat="server" Width="100%" UseAccessibleHeader="True"
        GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
        AllowPaging="false" CssClass="table table-striped table-bordered table-condensed"
        DataKeyNames="DisplayName,StatusName,Package_GUID,Project_GUID,Act1,Act2,Act3,Status_Guid"
        OnRowCommand="GridPackages_RowCommand" OnRowDataBound="GridPackages_RowDataBound">
        <Columns>
            <asp:TemplateField AccessibleHeaderText="Project/Package" HeaderText="Project/Package"
                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPackage" runat="server" Text='<%# Eval("DisplayName") %>'
                        ToolTip="Click to see more information" CommandName="ViewPackageInfo" CommandArgument='<%# Container.DataItemIndex %>'> 
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField AccessibleHeaderText="Version" HeaderText="Version" DataField="Version"
                SortExpression="Version" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField AccessibleHeaderText="Created Date" HeaderText="Created Date" DataField="CreatedDate"
                SortExpression="CreatedDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField AccessibleHeaderText="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Left"
                ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPrjPkgStatusName" runat="server" Text='<%# Eval("StatusName") %>'
                        ToolTip="Click to Update Status" CommandName="PrepareToUpdateStatus" CommandArgument='<%# Container.DataItemIndex %>'> 
                    </asp:LinkButton>
                    <asp:DropDownList ID="ddlPrjPkgStatusName" runat="server" AutoPostBack="true" Visible="false"
                        ToolTip="Click to see more information" OnSelectedIndexChanged="UpdatePrjPkgStatus">
                    </asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <%--<asp:BoundField AccessibleHeaderText="Status" HeaderText="Status" DataField="StatusName"
                SortExpression="StatusName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />--%>
            <asp:BoundField AccessibleHeaderText="Created By" HeaderText="Created By" DataField="CreatedByName"
                SortExpression="CreatedByName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" ToolTip="Edit Package" Visible='<%# Eval("Act1").ToString().Trim() != "" %>'
                        CommandName="EDITPROJPKG" CommandArgument='<%# Container.DataItemIndex %>'> 
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkSelect" runat="server" Text="Select" ToolTip="Select Package"
                        CommandName="SELECT" Visible='<%# Eval("Act2").ToString().Trim() != "" %>' CommandArgument='<%# Container.DataItemIndex %>'> 
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkHistory" runat="server" Text="History" ToolTip="Package History"
                        CommandName="HISTORY" Visible='<%# Eval("Act3").ToString().Trim() != "" %>' CommandArgument='<%# Container.DataItemIndex %>'> 
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            Sorry.! nothing to Display
        </EmptyDataTemplate>
    </asp:GridView>
    <br />
    <asp:Label ID="lblRoles" runat="server" Visible="false"> </asp:Label>
    <br />
    <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
    <br />
    <!-- ModalPopupExtender View/Edit Package -->
    <asp:HiddenField runat="server" ID="hrfMp1" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpViewPackageNote" runat="server" PopupControlID="Panel1"
        TargetControlID="hrfMp1" CancelControlID="btnClose" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="left" Style="display: none;
        width: 60%;">
        <asp:HiddenField ID="hfCurProjPackGuid" runat="server" />
        <table width="100%">
            <tr>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Project/Package:
                </td>
                <td align="left">
                    <asp:Label ID="lblviewPrjName" runat="server"></asp:Label>
                    <asp:TextBox ID="txtviewUpdatePrjName" Visible="false" runat="server"></asp:TextBox>
                </td>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Version:
                </td>
                <td align="left">
                    <asp:Label ID="lblviewPrjVersion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Created By:
                </td>
                <td align="left">
                    <asp:Label ID="lblviewPrjCreatedBy" runat="server"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Created Date:
                </td>
                <td align="left">
                    <asp:Label ID="lblviewPrjCreatedDate" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <hr />
        <div id="divNotes" runat="server" style="overflow: auto; height: 300px;">
            <asp:Literal ID="litviewPrjNotes" runat="server"></asp:Literal>
        </div>
        <asp:TextBox ID="htmlEditorPrjNotes" runat="server" ClientIDMode="Static" TextMode="MultiLine"
            Visible="false" Rows="30" Style="overflow: auto;" CssClass="tinymce" />
        <br />
        &nbsp;&nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" CssClass="customButton" />
        &nbsp;&nbsp;
        <asp:Button ID="btnEditPackage" runat="server" Text="Edit" CssClass="customButton"
            OnClick="btnEditPackage_Click" />
    </asp:Panel>
    <!-- End ModalPopupExtender View/Edit Package -->
    <!-- ModalPopupExtender View Package History -->
    <asp:HiddenField runat="server" ID="hrfMp2" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpViewPackageHist" runat="server" PopupControlID="Panel2"
        TargetControlID="hrfMp2" CancelControlID="btnClosePkgHist" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" align="left" Style="display: none;
        width: 60%;">
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <span style="font-weight: bold; text-align: left; padding-right: 10px !important;">Project/Package:</span>
        <asp:Label ID="lblPrjPkgNameHist" runat="server" Width="350px"></asp:Label>
        <span style="font-weight: bold;">History</span>
        <hr />
        <asp:UpdatePanel runat="server" ID="upModal" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="GridProjPkgHist" runat="server" Width="100%" UseAccessibleHeader="True"
                    GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                    AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered table-condensed"
                    DataKeyNames="Version,ea_guid,CreatedDate,StatusName" OnRowCommand="GridProjPkgHist_RowCommand"
                    OnPageIndexChanging="GridProjPkgHist_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField AccessibleHeaderText="Version" HeaderText="Version" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPrjPkgViewHist" runat="server" Text='<%# Eval("Version") %>'
                                    ToolTip="Click to see more information" CommandName="ViewPrjPkgHistInfo" CommandArgument='<%# Container.DataItemIndex %>'> 
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField AccessibleHeaderText="Created Date" HeaderText="Created Date" DataField="CreatedDate"
                            SortExpression="CreatedDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField AccessibleHeaderText="Status" HeaderText="Status" DataField="StatusName"
                            SortExpression="StatusName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField AccessibleHeaderText="Created By" HeaderText="Created By" DataField="CreatedByName"
                            SortExpression="CreatedByName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    </Columns>
                    <EmptyDataTemplate>
                        Sorry.! nothing to Display
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;&nbsp;<asp:Button ID="btnClosePkgHist" runat="server" Text="Close" CssClass="customButton" />
    </asp:Panel>
    <!-- End ModalPopupExtender View Package History -->
    <!-- ModalPopupExtender Create Project Package  -->
    <asp:HiddenField runat="server" ID="hfMpCreatePrj" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpCreatePrjPkg" runat="server" PopupControlID="PanelCreatePrjPkg"
        TargetControlID="hfMpCreatePrj" CancelControlID="btnCreatePrjPkgClose" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelCreatePrjPkg" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <table width="100%">
            <tr id="trPckgCreate" runat="server">
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Project Owner:
                </td>
                <td align="left" colspan="3">
                    <asp:DropDownList ID="ddlCreatePkgProjectList" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    <asp:Label ID="lblCreatePrjPkgTitle" runat="server" Text="Project Title"></asp:Label>
                </td>
                <td align="left" colspan="3">
                    <asp:TextBox ID="txtCreatePrjPkgName" runat="server" Width="400px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                </td>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Version:
                </td>
                <td align="left">
                    <asp:Label ID="lblCreatePkgPrjVersion" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Created By:
                </td>
                <td align="left">
                    <asp:Label ID="lblCreatePrjPkgCreatedBy" runat="server"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Created Date:
                </td>
                <td align="left">
                    <asp:Label ID="lblCreatePrjPkgCreatedDate" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <hr />
        <asp:TextBox ID="htmlEditorCreatePrjPkgNotes" runat="server" ClientIDMode="Static"
            TextMode="MultiLine" Visible="true" Rows="20" Style="overflow: auto;" CssClass="tinymce" />
        <br />
        &nbsp;&nbsp;<asp:Button ID="btnCreatePrjPkgClose" runat="server" Text="Close" CssClass="customButton" />
        &nbsp;&nbsp;
        <asp:Button ID="btnCreatePrjPkg" runat="server" Text="Update" CssClass="customButton"
            OnClick="btnCreatePrjPkg_Click" />
    </asp:Panel>
    <!-- End ModalPopupExtender Create Project Package  -->
</asp:Content>
