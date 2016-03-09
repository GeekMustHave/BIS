<%@ Page Title="Use-Cases" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="UseCases.aspx.cs" Inherits="UseCases" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<%@ Register TagPrefix="CustomButtonDropDown" TagName="ButtonDropDown" Src="~/Plugins/UserControls/buttonDropDown.ascx" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://tinymce.cachefly.net/4.1/tinymce.min.js"></script>
    <%--<script type="text/javascript" src="http://code.jquery.com/jquery-1.10.2.js"></script>--%>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link href="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/css/select2.min.css?vv=11"
        rel="stylesheet" />
    <script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/select2/4.0.0/js/select2.min.js"></script>
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
        /* change z index to show the select2 dropdown on modal popup. Labels Dropdown*/
        .select2-dropdown
        {
            z-index: 1000005 !important;
            border: 1px solid #d3d3d3 !important;
        }
        /* For not to highlight the labels textbox - removing border*/
        .select2-container--default .select2-selection--multiple
        {
            border: 1px solid #d3d3d3 !important;
        }
    </style>
    <script type="text/javascript">
        // for tiny MCE Editor
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
            height: "220",
            browser_spellcheck: true,
            contextmenu: false
        });
        /* Script for LABELs - USEd SELECT2 jquery pligin .. check the website for documentation */
        /* fucntion to pre load the labels for ths requirement when the edit link is clicked with  show details is checked */
        /* this javascripot is called from code behind register startup script.. check the code behind for details*/
        /*  this will call the database as an ajax call and get the labels for the req and binds them when the popup is shwow */
        function initBindLabels(eguid) {
            $.ajax({
                type: "POST",
                url: 'UseCases.aspx/GetUseCaseLabels',
                contentType: "application/json; charset=utf-8",
                //data: "{'currentUCID' :'{d17a1331-2274-4f68-ad37-e46c18e20318}'}",
                data: "{'currentUCID' :'" + eguid + "'}",
                success: function (msg) {
                    var data = JSON.parse(msg.d);
                    for (i = 0; i < data.length; i++) {
                        $('#select2Labelddl').append($("<option/>", {
                            value: data[i].text,
                            text: data[i].text,
                            selected: true
                        }));
                        configSelect2Labelddl(eguid);
                    }
                    if (data.length == 0) {
                        //To configure when no results were found. 
                        configSelect2Labelddl(eguid);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    if (errorThrown != 'abort') {
                        // Do something here with the error
                        $('#<%=lblError.ClientID%>').html(errorMessage);
                        alert(textStatus);
                        alert(errorThrown);
                    }
                }
            });
            /* bind delete tag and create tag events for the Label ddl (select2) so that we can do ajax post for updates*/
            var $eventSelect = $("#select2Labelddl");
            $eventSelect.on("select2:select", function (e) { UpdateReqLabel("select", eguid, e); });
            $eventSelect.on("select2:unselect", function (e) { UpdateReqLabel("unselect", eguid, e); });
        }
        /* function to configure the select2 dropwdown for labels after we load the intial values. */
        function configSelect2Labelddl(eaguid) {
            // the projGuid is passed jsut to save time in retriveing the eaguid again when select/unselect happens in the label ddl.
            //the configuration required to make mutiple selects and add/remove tags for the label dropdown.
            // it has a ajax call to get the list for dropdown when user starts to enter something in the lables ddl.
            $("#select2Labelddl").select2({
                tags: "true",
                multiple: "true",
                placeholder: "Enter Labels",
                minimumInputLength: 1,
                width: "resolve",
                tokenSeparators: [','],
                ajax: {
                    //How long the user has to pause their typing before sending the next request
                    // for some reason this stupid delay isn't working as expected.. but who cares.
                    quietMillis: 35000,
                    type: "POST",
                    url: 'UseCases.aspx/GetAllUniqueLabels',
                    contentType: "application/json; charset=utf-8",
                    data: function (params) {
                        return "{'searchText' :'" + params.term + "'}";
                    },
                    processResults: function (msg) {
                        //alert(msg.d);
                        return { results: JSON.parse(msg.d) };
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                        if (errorThrown != 'abort') {
                            // Do something here with the error
                            $('#<%=lblError.ClientID%>').html(errorMessage);
                            alert(textStatus);
                            alert(errorThrown);
                        }
                    }
                }
            });
        }
        /* event that will be fired when a tag is created/selected or removed./unselected */
        function UpdateReqLabel(name, eaguid, evt) {
            // the eaguid is passed jsut to save time in retriveing the eaguid again when select/unselect happens in the label ddl.
            //alert(name);
            var curLabel = '';
            if (!evt) {
                var args = "{}";
            }
            else {
                var args = JSON.stringify(evt.params, function (key, value) {
                    if (value && value.nodeName) return "[DOM node]";
                    if (value instanceof $.Event) {
                        return "[$.Event]";
                    }
                    if (key == 'data') {
                        curLabel = value.text;
                        //alert(curLabel);
                        //alert(value.id);
                        // var str = JSON.stringify(value);
                        // var obj = JSON.parse(str);
                        // curLabel = obj.text;
                    }
                    return value;
                });
            }
            //Ajax post data to remove or add the label to the reqguid.
            $.ajax({
                type: "POST",
                url: 'UseCases.aspx/UpdateUCLabels',
                contentType: "application/json; charset=utf-8",
                //data: "{'currentReqID' :'{d17a1331-2274-4f68-ad37-e46c18e20318}'}",
                data: "{'currentUCID' :'" + eaguid + "', 'actionEvent' :'" + name + "', 'tagName' :'" + curLabel + "'}",
                success: function (response) {
                    // .. do something .. nothin to do for now
                    //alert(response.d);
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    //console.log(errorMessage); // Optional
                    $('#<%=lblError.ClientID%>').html(errorMessage);
                    alert(errorMessage);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="welcomeMessage">
        <asp:Label ID="lblPackageName" runat="server">
        </asp:Label>
    </div>
    <asp:MultiView ID="mvUseCases" runat="server">
        <asp:View ID="vwNoSelect" runat="server">
            <br />
            <br />
            <span>No Package is selected. Please navigate to
                <asp:HyperLink ID="lnkPackages" runat="server" NavigateUrl="~/Packages.aspx" Text="Packages"></asp:HyperLink>
                Page and select a package. </span>
        </asp:View>
        <asp:View ID="vwDefault" runat="server">
            <table width="100%">
                <tr>
                    <td colspan="3" align="left" style="padding-bottom: 5px; padding-top: 5px;">
                        <span style="font-weight: bold">Use-Cases </span>
                    </td>
                     <td align="right" style="padding-bottom: 5px; padding-top: 5px;">
                         <CustomButtonDropDown:ButtonDropDown ID="btnDropDownReports" runat="server" />
                         &nbsp;
                        <asp:Button ID="btnAddUsecase" runat="server" Text="Add Use-Case" CssClass="customButton"
                            OnClick="btnAddUsecase_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <span>Requirement</span>&nbsp;
                        <asp:DropDownList ID="ddlRequirements" runat="server" Width="250px" ToolTip="Select a requirement"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlRequirements_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td align="left">
                        <span>Search</span>&nbsp;
                        <asp:TextBox ID="txtSearchUseCase" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td align="right">
                        <asp:CheckBox ID="chkshowHidden" runat="server" Text="Show Hidden" TextAlign="Left"
                            Checked="true" />
                    </td>
                    <td align="left" style="padding-bottom: 5px; padding-top: 5px;">
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnReferesh" runat="server" Text="Refresh" CssClass="customButton"
                            OnClick="btnReferesh_Click" />                        
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GridUseCaseList" runat="server" Width="100%" UseAccessibleHeader="True"
                GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                AllowPaging="false" CssClass="table table-striped table-bordered table-condensed"
                DataKeyNames="Version,ea_guid,CreatedDate,Status" OnRowCommand="GridUseCaseList_RowCommand">
                <Columns>
                    <asp:BoundField AccessibleHeaderText="Use Case" HeaderText="Use Case" DataField="Use_Case"
                        SortExpression="Use_Case" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField AccessibleHeaderText="Version" HeaderText="Version" DataField="Version"
                        SortExpression="Version" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField AccessibleHeaderText="Created Date" HeaderText="Created Date" DataField="CreatedDate"
                        SortExpression="CreatedDate" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField AccessibleHeaderText="Status" HeaderText="Status" DataField="Status"
                        SortExpression="StatusName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField AccessibleHeaderText="Hidden" HeaderText="Hidden" DataField="Hidden"
                        SortExpression="Hidden" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                        ItemStyle-Width="25%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkUseCaseListEdit" runat="server" Text="Edit" ToolTip="Click to edit usecase"
                                CommandName="UseCaseListEdit" CommandArgument='<%# Container.DataItemIndex %>'> 
                            </asp:LinkButton>
                            |
                            <asp:LinkButton ID="lnkUseCaseListFlows" runat="server" Text="Flows" ToolTip="Click to view Flows"
                                CommandName="UseCaseListFlows" CommandArgument='<%# Container.DataItemIndex %>'> 
                            </asp:LinkButton>
                            |
                            <asp:LinkButton ID="lnkUseCaseListReq" runat="server" Text="Requirements" ToolTip="Click to view requirements"
                                CommandName="UseCaseListReq" CommandArgument='<%# Container.DataItemIndex %>'> 
                            </asp:LinkButton>
                            |
                            <asp:LinkButton ID="lnkUseCaseListHist" runat="server" Text="History" ToolTip="Click to view History"
                                CommandName="UseCaseListHist" CommandArgument='<%# Container.DataItemIndex %>'> 
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Results
                </EmptyDataTemplate>
            </asp:GridView>
        </asp:View>
    </asp:MultiView>
    <br />
    <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
    <!-- ModalPopupExtender Create Use case -->
    <asp:HiddenField runat="server" ID="hfMpCreateUpdateUC" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpCreateUpdateUC" runat="server" PopupControlID="PanelCreateUpdateUC"
        TargetControlID="hfMpCreateUpdateUC" CancelControlID="btnCreateUpdateUCClose"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelCreateUpdateUC" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:HiddenField ID="hfCurrentUseCaseID" runat="server" />
        <table width="100%">
            <tr id="trPckgCreate" runat="server">
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Use Case:
                </td>
                <td align="left" colspan="5">
                    <asp:TextBox ID="txtCreateUpdateUCName" runat="server" Width="500px" ToolTip="enter use case name">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Version:
                </td>
                <td align="left">
                    <asp:Label ID="lblCreateUpdateUCVersion" runat="server"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Created By:
                </td>
                <td align="left">
                    <asp:Label ID="lblCreateUpdateUCCreatedBy" runat="server"></asp:Label>
                </td>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Created Date:
                </td>
                <td align="left">
                    <asp:Label ID="lblCreateUpdateUCCreatedDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr id="trCreateUpdateUC" runat="server" visible="false">
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Status:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlCreateUpdateUCStatus" runat="server">
                    </asp:DropDownList>
                </td>
                <td align="right" style="padding-right: 10px; font-weight: bold;">
                    Hidden:
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkCreateUpdateUCHidden" runat="server" />
                </td>
                <td colspan="2">
                </td>
            </tr>
            <tr id="trCreateUpdateUCLabels" runat="server" visible="false">
                <td align="right" style="font-weight: bold;">
                    Labels:
                </td>
                <td colspan="5" align="left">
                    <select id="select2Labelddl" style="width: 500px;">
                    </select>
                </td>
            </tr>
        </table>
        <hr />
        <asp:TextBox ID="htmlEditorCreateUpdateUCNotes" runat="server" ClientIDMode="Static"
            TextMode="MultiLine" Visible="true" Rows="20" Style="overflow: auto;" CssClass="tinymce" />
        <br />
        &nbsp;&nbsp;<asp:Button ID="btnCreateUpdateUCClose" runat="server" Text="Close" CssClass="customButton" />
        &nbsp;&nbsp;
        <asp:Button ID="btnCreateUpdateUCNew" runat="server" Text="Update" CssClass="customButton"
            OnClick="btnCreateUpdateUCNew_Click" />
    </asp:Panel>
    <!-- End ModalPopupExtender Create Use case  -->
    <!-- ModalPopupExtender View Use History -->
    <asp:HiddenField runat="server" ID="hfUCHist" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpViewUCHist" runat="server" PopupControlID="pnlViewUCHist"
        TargetControlID="hfUCHist" CancelControlID="btnCloseUCHist" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlViewUCHist" runat="server" CssClass="modalPopup" align="left" Style="display: none;
        width: 60%;">
        <asp:HiddenField ID="hfCurrentUCIdHist" runat="server" />
        <span style="font-weight: bold; text-align: left; padding-right: 10px !important;">Use
            Case Id:</span>
        <asp:Label ID="lblCurrentUCIDHist" runat="server"></asp:Label>
        &nbsp;&nbsp;-
        <asp:Label ID="lblUCNameHist" runat="server" Width="350px"></asp:Label>
        <hr />
        <asp:UpdatePanel runat="server" ID="upModalUCHist" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="GridUCHist" runat="server" Width="100%" UseAccessibleHeader="True"
                    GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                    AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered table-condensed"
                    DataKeyNames="Version,ea_guid,CreatedDate,StatusName" OnRowCommand="GridUCHist_RowCommand"
                    OnPageIndexChanging="GridUCHist_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField AccessibleHeaderText="Version" HeaderText="Version" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkUCViewHist" runat="server" Text='<%# Eval("Version") %>' ToolTip="Click to see more information"
                                    CommandName="ViewUCHistInfo" CommandArgument='<%# Container.DataItemIndex %>'> 
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
                        No Results
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;&nbsp;<asp:Button ID="btnCloseUCHist" runat="server" Text="Close" CssClass="customButton" />
    </asp:Panel>
    <!-- End ModalPopupExtender View Use Case History -->
    <!-- ModalPopupExtender View Use Case History Detail  -->
    <asp:HiddenField runat="server" ID="hrfViewUCHistDetail" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpViewUCHistDetail" runat="server" PopupControlID="pnlViewUCHistDetail"
        TargetControlID="hrfViewUCHistDetail" CancelControlID="btnViewUCHistDetailClose"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlViewUCHistDetail" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <table style="border: 1; width: 100% !important;">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td colspan="3" align="left" style="padding-bottom: 5px;">
                                <span style="font-weight: bold">
                                    <asp:Label ID="lblViewUCHistDetailName" runat="server"></asp:Label>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span style="font-weight: bold">Version: </span>
                                <asp:Label ID="lblViewUCHistDetailsVersion" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Created Date: </span>
                                <asp:Label ID="lblViewUCHistDetailsCreatedDate" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Created By: </span>
                                <asp:Label ID="lblViewUCHistDetailsCreatedBy" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span style="font-weight: bold">Type: </span>
                                <asp:Label ID="lblViewUCHistDetailsType" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Complexity: </span>
                                <asp:Label ID="lblViewUCHistDetailsComplexity" runat="server"></asp:Label>
                            </td>
                            <td>
                                <span style="font-weight: bold">Status: </span>
                                <asp:Label ID="lblViewUCHistDetailsStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span style="font-weight: bold">Labels: </span>
                                <asp:Label ID="lblViewUCHistDetailsLabel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divText" style="overflow: auto; height: 300px !important;">
                        <asp:Literal ID="litViewUCHistDetailTextNotes" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnViewUCHistDetailClose" runat="server" Text="Close" CssClass="customButton" />
        &nbsp;&nbsp;
        <asp:Button ID="btnViewEditUCHistDetail" runat="server" Text="Edit" CssClass="customButton"
            OnClick="btnViewEditUCHistDetail_Click" />
    </asp:Panel>
    <!-- End ModalPopupExtender View UseCase History Detail -->
    <!-- ModalPopupExtender View Case Requirements -->
    <asp:HiddenField runat="server" ID="hrfEditUCReq" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpEditUCReq" runat="server" PopupControlID="pnlEditUCReq"
        TargetControlID="hrfEditUCReq" CancelControlID="hrfEditUCReq" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlEditUCReq" runat="server" CssClass="modalPopup" align="left" Style="display: none;
        width: 60%;">
        <span style="font-weight: bold; text-align: left; padding-right: 10px !important;">Use
            Case Id:</span>
        <asp:Label ID="lblEditUCReqID" runat="server"></asp:Label>
        &nbsp;&nbsp;-
        <asp:Label ID="lblEditUCReqName" runat="server" Width="350px"></asp:Label>
        <hr />
        <span>Requirements using this Use-Case</span>
        <br />
        <br />
        <asp:UpdatePanel runat="server" ID="upEditUCReq" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="GridEditUCReq" runat="server" Width="100%" UseAccessibleHeader="True"
                    GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                    AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered table-condensed"
                    DataKeyNames="REQ_GUID,UC_GUID" OnRowCommand="GridEditUCReq_RowCommand" OnPageIndexChanging="GridEditUCReq_PageIndexChanging">
                    <Columns>
                        <asp:BoundField AccessibleHeaderText="Requirement" HeaderText="Requirement" DataField="Req_Name"
                            SortExpression="Req_Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField AccessibleHeaderText="Status" HeaderText="Status" DataField="Status"
                            SortExpression="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                        <asp:TemplateField AccessibleHeaderText="Version" HeaderText="Version" HeaderStyle-HorizontalAlign="Center"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDeleteEditUCReq" runat="server" Text="Delete" ToolTip="Click to see delete Requirement"
                                    CommandName="DeleteEditUCReq" CommandArgument='<%# Container.DataItemIndex %>'> 
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        No Requirements Found
                    </EmptyDataTemplate>
                </asp:GridView>
                <br />
                <span>Add Requirement to this use-case</span>
                <br />
                <br />
                &nbsp;&nbsp;<asp:DropDownList ID="ddlRequirementEditUCReq" runat="server" ToolTip="Select a req from list"
                    Width="350px">
                </asp:DropDownList>
                <br />
                <br />
                &nbsp;&nbsp;<asp:Button ID="btnCloseEditUCReq" runat="server" Text="Close" CssClass="customButton"
                    OnClick="btnCloseEditUCReq_Click" />
                &nbsp;&nbsp;<asp:Button ID="btnUpdateEditUCReq" runat="server" Text="Update" CssClass="customButton"
                    OnClick="btnUpdateEditUCReq_Click" />
                &nbsp;&nbsp;<asp:Label ID="lblErrorEditUCReq" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- End ModalPopupExtender View Use Case History -->
</asp:Content>
