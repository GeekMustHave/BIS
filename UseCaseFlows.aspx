<%@ Page Title="Use-Case Flows" Language="C#" MasterPageFile="~/BISMaster.master"
    AutoEventWireup="true" CodeFile="UseCaseFlows.aspx.cs" Inherits="UseCaseFlows"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/bootstrap.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        /* increase z-index to higher to show all the menus while loading on a popup.*/
        .cke_dialog
        {
            z-index: 1000001 !important;
        }
        .cke_bottom
        {
            display: none !important;
        }
        .cke_panel
        {
            z-index: 1000001 !important;
        }
        .customCKEdit
        {
            height: 50px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="welcomeMessage">
        <asp:Label ID="lblPackageName" runat="server">
        </asp:Label>
    </div>
    <div class="welcomeMessage">
        <asp:Label ID="lblUseCaseName" runat="server">
        </asp:Label>
    </div>
    <asp:MultiView ID="mvUseCases" runat="server">
        <asp:View ID="vwNoSelect" runat="server">
            <br />
            <br />
            <span>Package/UseCase is not valid. Please navigate to
                <asp:HyperLink ID="lnkPackages" runat="server" NavigateUrl="~/Packages.aspx" Text="Packages"></asp:HyperLink>
                Page and select a package. </span>
        </asp:View>
        <asp:View ID="vwDefault" runat="server">
            <asp:UpdatePanel runat="server" ID="UpdatePanelUCFlows" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: auto; padding-bottom: 5px; padding-top: 5px;">
                                <span style="font-weight: bold">Steps for Flow </span>&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlFlows" runat="server" Width="350px" ToolTip="Select a flow"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFlows_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hfTotalStepsCount" runat="server" Value="" />
                    <asp:GridView ID="GridStepsForFlow" runat="server" Width="100%" UseAccessibleHeader="True"
                        GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                        AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered table-condensed"
                        DataKeyNames="Step_GUID,ActorSystemDoes,StepNumber,BR_Flag,EM_Flag" OnPageIndexChanging="GridStepsForFlow_PageIndexChanging"
                        OnRowCommand="GridStepsForFlow_RowCommand" AllowSorting="false" OnRowDataBound="GridStepsForFlow_RowDataBound">
                        <Columns>
                            <asp:BoundField AccessibleHeaderText="Step" HeaderText="Step" DataField="StepNumber"
                                SortExpression="StepNumber" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField AccessibleHeaderText="Actor Does" HeaderText="Actor Does" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblGridStepsActorDoes" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="System Does" HeaderText="System Does" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lblGridStepsSystemDoes" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField AccessibleHeaderText="Screen" HeaderText="Screen" HeaderStyle-HorizontalAlign="Left"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkGridStepsScreen" runat="server" Text='<%# Eval("ImageID") != DBNull.Value ? "View Screen" : "Add Screen" %>'
                                        ToolTip="Click to pdate Screen" CommandName="ViewAddFlowScreen" CommandArgument='<%# Container.DataItemIndex %>'> 
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center"
                                ItemStyle-Width="35%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkGridStepsEditStep" runat="server" Text="Edit" ToolTip="Click to edit Step"
                                        CommandName="EditStep" CommandArgument='<%# Container.DataItemIndex %>'> 
                                    </asp:LinkButton>
                                    |
                                    <asp:LinkButton ID="lnkGridStepsAddStep" runat="server" Text="Add" ToolTip="Click to Add New step"
                                        CommandName="AddStep" CommandArgument='<%# Container.DataItemIndex %>'> 
                                    </asp:LinkButton>
                                    |
                                    <asp:LinkButton ID="lnkGridStepsDeleteStep" runat="server" Text="Delete" ToolTip="Click to delete step"
                                        CommandName="DeleteStep" CommandArgument='<%# Container.DataItemIndex %>'> 
                                    </asp:LinkButton>
                                    |
                                    <asp:LinkButton ID="lnkGridStepsBusinessRule" runat="server" Text='<%# ((int)Eval("BR_Flag")) == 1 ? "Business Rule(s)" : "Add Rule" %>'
                                        ToolTip="Click to update business rule" CommandName="ViewUpdateBusinessRule"
                                        CommandArgument='<%# Container.DataItemIndex %>'> 
                                    </asp:LinkButton>
                                    |
                                    <asp:LinkButton ID="lnkGridStepsErrorMessage" runat="server" Text='<%# ((int)Eval("EM_Flag")) == 1 ? "Error Message(s)" : "Add Message"  %>'
                                        ToolTip="Click to update Error Message" CommandName="ViewUpdateErrorMessage"
                                        CommandArgument='<%# Container.DataItemIndex %>'> 
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Results
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: auto; padding-bottom: 5px; padding-top: 5px;">
                                <span style="font-weight: bold">Flows </span>&nbsp;&nbsp;
                            </td>
                            <td align="right" style="width: auto;">
                                <asp:Button ID="btnAddNewFlow" runat="server" ToolTip="click to add new Step" Text="Add Flow"
                                    CssClass="customButton" OnClick="btnAddNewFlow_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="GridFlows" runat="server" Width="100%" UseAccessibleHeader="True"
                        GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                        AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered table-condensed"
                        DataKeyNames="Flow_GUID,EntryPoint_GUID,FlowName,HiddenFlagText,HiddenFlag,FlowType,JoinPoint_GUID"
                        AllowSorting="false" OnPageIndexChanging="GridFlows_PageIndexChanging" OnRowCommand="GridFlows_RowCommand"
                        OnRowDataBound="GridFlows_RowDataBound">
                        <Columns>
                            <asp:BoundField AccessibleHeaderText="Entry Point" HeaderText="Entry Point" DataField="EntryPoint"
                                SortExpression="EntryPoint" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField AccessibleHeaderText="Flow Name" HeaderText="Flow Name" DataField="FlowName"
                                SortExpression="FlowName" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField AccessibleHeaderText="Hidden" HeaderText="Hidden" DataField="HiddenFlagText"
                                SortExpression="HiddenFlagText" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField AccessibleHeaderText="Flow Type" HeaderText="Flow Type" DataField="FlowType"
                                SortExpression="FlowType" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField AccessibleHeaderText="Join" HeaderText="Join" DataField="JoinPoint"
                                SortExpression="JoinPoint" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkGridFlowsEdit" runat="server" Text="Edit |" ToolTip="Click to edit flow"
                                        CommandName="EditFlow" CommandArgument='<%# Container.DataItemIndex %>' Visible='<%# Eval("FlowType").ToString().Trim() == "Basic" ? false : true  %>'> 
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkGridFlowsHideUnHideFlow" runat="server" Text='<%# Eval("HiddenFlag") %>'
                                        ToolTip="Click to Hide/Unhide Flow" CommandName="HideUnhideFlow" CommandArgument='<%# Container.DataItemIndex %>'> 
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Results
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAddNewFlow" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
    <br />
    <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
    <!-- ModalPopupExtender Create Update Step   -->
    <asp:HiddenField runat="server" ID="hfMpCreateUpdateStep" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpCreateUpdateStep" runat="server" PopupControlID="PanelCreateUpdateStep"
        TargetControlID="hfMpCreateUpdateStep" CancelControlID="hfMpCreateUpdateStep"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelCreateUpdateStep" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 65%;">
        <asp:UpdatePanel runat="server" ID="upCreateUpdateStep" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfCurentStepId" runat="server" />
                <span style="font-weight: bold;">Step Add/Edit</span>
                <table width="100%" id="tblStepAddEdit" runat="server">
                    <tr>
                        <td>
                            <span style="font-weight: bold;">Step: </span>
                            <asp:Label ID="lblCreateUpdateStepId" runat="server"></asp:Label>
                            &nbsp;&nbsp;<span style="font-weight: bold;">Flow</span>
                            <asp:Label ID="lblCreateUpdateStepFlowName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-weight: bold;">Actor Does</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 200px !important;">
                            <CKEditor:CKEditorControl ID="htmlEditorCreateUpdateStepActorDoes" BasePath="~/Plugins/ckeditor/"
                                runat="server" Height="100px" EnterMode="DIV"></CKEditor:CKEditorControl>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="font-weight: bold;">System Does</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 200px !important;">
                            <CKEditor:CKEditorControl ID="htmlEditorCreateUpdateStepSystemDoes" BasePath="~/Plugins/ckeditor/"
                                runat="server" Height="100px" EnterMode="DIV"></CKEditor:CKEditorControl>
                        </td>
                    </tr>
                </table>
                <br />
                &nbsp;&nbsp;
                <asp:Button ID="btnCreateUpdateStep" runat="server" Text="Update" OnClick="btnCreateUpdateStep_Click"
                    CssClass="customButton" />
                &nbsp;&nbsp;<asp:Button ID="btnCreateUpdateStepClose" runat="server" Text="Close"
                    CssClass="customButton" OnClick="btnCreateUpdateStepClose_Click" />
                &nbsp;&nbsp;<asp:Label ID="lblErrorCreateUpdateStep" runat="server" Font-Bold="true"
                    ForeColor="Red"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCreateUpdateStep" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCreateUpdateStepClose" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- End ModalPopupExtender Create update step  -->
    <!-- ModalPopupExtender Cannot Delete Step -->
    <asp:HiddenField runat="server" ID="hfDeleteStep" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpDeleteStep" runat="server" PopupControlID="PanelDeleteStep"
        TargetControlID="hfDeleteStep" CancelControlID="btnDeleteStepClose" BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelDeleteStep" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 65%;">
        <span>This step cannot be deleted as it is used in an entry point or join point.</span>
        <br />
        <asp:Button ID="btnDeleteStepClose" runat="server" Text="Close" CssClass="customButton" />
    </asp:Panel>
    <!-- End ModalPopupExtender Cannot Delete Step -->
    <!-- ModalPopupExtender Create Update Flow -->
    <asp:HiddenField runat="server" ID="hfMpCreateUpdateFlow" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpCreateUpdateFlow" runat="server" PopupControlID="PanelCreateUpdateFlow"
        TargetControlID="hfMpCreateUpdateFlow" CancelControlID="hfMpCreateUpdateFlow"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelCreateUpdateFlow" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:UpdatePanel runat="server" ID="upCreateUpdateFlow" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="hfCurrentFlowID" />
                <span style="font-weight: bold;">Flow Add/Edit</span>
                <hr />
                <table width="100%">
                    <tr>
                        <td align="right" style="padding-top: 10px; padding-right: 10px;">
                            <span style="font-weight: bold;">Entry Point: </span>
                        </td>
                        <td style="padding-top: 10px;">
                            <asp:DropDownList ID="ddlCreateUpdateFlowEntryPoint" runat="server" ToolTip="Select entry point">
                            </asp:DropDownList>
                            <span style="color: Gray">&nbsp;&nbsp;What step does this flow originate from</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-top: 10px; padding-right: 10px;">
                            <span style="font-weight: bold;">Flow Name: </span>
                        </td>
                        <td style="padding-top: 10px;">
                            <asp:TextBox ID="txtCreateUpdateFlowName" Width="90%" runat="server" ToolTip="Enter flow name">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trCreateUpdateFlowHidden" runat="server" visible="false">
                        <td align="right" style="padding-top: 10px; padding-right: 10px;">
                            <span style="font-weight: bold;">Hidden: </span>
                        </td>
                        <td style="padding-top: 10px;">
                            <asp:CheckBox ID="chkCreateUpdateFlowHidden" runat="server" ToolTip="Check to Hide">
                            </asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-top: 10px; padding-right: 10px;">
                            <span style="font-weight: bold;">Flow Type: </span>
                        </td>
                        <td style="padding-top: 10px;">
                            <asp:DropDownList ID="ddlCreateUpdateFlowType" runat="server" ToolTip="Select flow type"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlCreateUpdateFlowType_SelectedIndexChanged">
                                <asp:ListItem Text="Alternative" Value="Alternative" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Exception" Value="Exception"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-top: 10px; padding-right: 10px;">
                            <span style="font-weight: bold;">Join: </span>
                        </td>
                        <td style="padding-top: 10px;">
                            <asp:DropDownList ID="ddlCreateUpdateFlowJoin" runat="server" ToolTip="Select join">
                            </asp:DropDownList>
                            <span style="color: Gray;">&nbsp;&nbsp;When this flow is done, what step in basic flow
                                to resume with</span>
                        </td>
                    </tr>
                </table>
                <br />
                &nbsp;&nbsp;
                <asp:Button ID="btnCreateUpdateFlow" runat="server" Text="Update" OnClick="btnCreateUpdateFlow_Click"
                    CssClass="customButton" />
                &nbsp;&nbsp;<asp:Button ID="btnCreateUpdateFlowClose" runat="server" Text="Close"
                    CssClass="customButton" OnClick="btnCreateUpdateFlowClose_Click" />
                &nbsp;&nbsp;<asp:Label ID="lblErrorCreateUpdateFlow" runat="server" Font-Bold="true"
                    ForeColor="Red"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnCreateUpdateFlow" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCreateUpdateFlowClose" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- End ModalPopupExtender Create Update Flow -->
    <!-- ModalPopupExtender Show Business Rules List -->
    <asp:HiddenField runat="server" ID="hfMpBusinessRuleList" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpBusinessRuleList" runat="server" PopupControlID="PanelBusinessRuleList"
        TargetControlID="hfMpBusinessRuleList" CancelControlID="hfMpBusinessRuleList"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelBusinessRuleList" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:UpdatePanel runat="server" ID="upBusinessRuleList" UpdateMode="Conditional">
            <ContentTemplate>               
                <asp:HiddenField ID="hfRuleListCurStepId" runat="server" />
                <div style="width: 100%; height:auto !important; max-height: 400px; overflow:auto; padding-bottom:5px;">
                    <span style="font-weight: bold;">Business Rules List</span>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Step: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblBusinessRuleListStep" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Flow: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblBusinessRuleListFlow" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Actor: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblBusinessRuleListActorDoes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"></td>
                            <td align="right" style="padding-top: 5px; padding-right: 10px;" >
                                <span style="font-weight: bold;">System: </span>
                            </td>
                            <td style="padding-top: 5px;">
                                <asp:Label ID="lblBusinessRuleListSystemDoes" runat="server"></asp:Label>
                            </td>
                        </tr>                    
                    </table>
                    <br /><hr />
                    <table width="100%">
                        <tr>
                            <td align="right">
                                 <asp:Button ID="btnBusinessRuleListAddRule" runat="server" Text="Add Business Rule" OnClick="btnBusinessRuleListAddRule_Click" 
                                     CssClass="customButton" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:GridView ID="GridBusinessRules" runat="server" Width="100%" UseAccessibleHeader="True"
                                    GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                                    AllowPaging="true" PageSize="5" CssClass="table table-striped table-bordered table-condensed"
                                    DataKeyNames="BR_ObjectPK,UV_ID,BR_ShortName,BR_Name,BR_Note,BR_GUID" AllowSorting="false" OnRowCommand="GridBusinessRules_RowCommand" OnRowDataBound="GridBusinessRules_RowDataBound" OnPageIndexChanging="GridBusinessRules_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="Business Rule" HeaderText="Business Rule" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGridBusinessRuleName" runat="server" Text='<%# Eval("BR_Name") %>' Font-Bold="true"></asp:Label>
                                                <br />
                                                <asp:Label ID="lblGridBusinessRuleNote" runat="server"  Text='<%# Eval("BR_Note") %>' ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="15%" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkGridBusinessRuleEditRule" runat="server" Text="Edit" ToolTip="Click to edit business rule"
                                                    CommandName="EditBusinessRule" CommandArgument='<%# Container.DataItemIndex %>'> 
                                                </asp:LinkButton>
                                                |
                                                <asp:LinkButton ID="lnkGridBusinessRuleDeleteRule" runat="server" Text="Delete" ToolTip="Click to business rule"
                                                    CommandName="DeleteBusinessRule" CommandArgument='<%# Container.DataItemIndex %>'> 
                                                </asp:LinkButton>                                            
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Results
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <asp:Button ID="btnBusinessRuleListClose" runat="server" Text="Close" OnClick="btnBusinessRuleListClose_Click"
                    CssClass="customButton"  ToolTip="click to close Business rules popup"/>
                &nbsp;&nbsp;<asp:Label ID="lblBusinessRuleListCloseError" runat="server" Font-Bold="true"
                    ForeColor="Red"></asp:Label>
            </ContentTemplate>
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="btnBusinessRuleListClose" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnBusinessRuleListAddRule" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- End ModalPopupExtender Show Business Rules List -->    
    <!-- ModalPopupExtender Add Edit Business Rule -->
    <asp:HiddenField runat="server" ID="hfMpBusinessRuleAddEdit" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpBusinessRuleAddEdit" runat="server" PopupControlID="PanelBusinessRuleAddEdit"
        TargetControlID="hfMpBusinessRuleAddEdit" CancelControlID="hfMpBusinessRuleAddEdit"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelBusinessRuleAddEdit" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:UpdatePanel runat="server" ID="upBusinessRuleAddEdit" UpdateMode="Conditional">
            <ContentTemplate>                
                <asp:HiddenField ID="hfBusinessRuleAddEditBrGUID" runat="server" />
                <div style="width: 100%; height:auto !important; max-height: 400px; overflow:auto;">
                    <span style="font-weight: bold;">Business Rules Add/Edit</span>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Step: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblBusinessRuleAddEditStep" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Flow: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblBusinessRuleAddEditFlow" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Actor: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblBusinessRuleAddEditActorDoes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"></td>
                            <td align="right" style="padding-top: 5px; padding-right: 10px;" >
                                <span style="font-weight: bold;">System: </span>
                            </td>
                            <td style="padding-top: 5px;">
                                <asp:Label ID="lblBusinessRuleAddEditSystemDoes" runat="server"></asp:Label>
                            </td>
                        </tr>                    
                    </table>
                    <hr />
                    <table width="100%">
                        <tr id="trBusinessruleAdd" runat="server">                        
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Exsisting Business Rules: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:DropDownList ID="ddlBusinessRuleAddEditRules" runat="server" ToolTip="exsiting business rules in the package" 
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlBusinessRuleAddEditRules_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-top: 10px;padding-right:5px;" runat="server" id="tdBusinessruleEdit">
                                <span style="font-weight: bold;">Business Rule ID: </span>
                                &nbsp;&nbsp;
                                <asp:Label ID="lblBusinessRulesAddEditRuleId" runat="server" ></asp:Label>
                            </td>
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Business Rule Name: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:TextBox ID="txtBusinessRuleAddEditRuleName" runat="server" Width="300px" ToolTip="Business rule name"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>                        
                            <td style="height: 200px !important;" colspan="3">
                                <CKEditor:CKEditorControl ID="htmlEditorBusinessRuleAddEditNote" BasePath="~/Plugins/ckeditor/"
                                    runat="server" Height="100px" EnterMode="DIV"></CKEditor:CKEditorControl>
                            </td>
                        </tr>
                    </table>               
                    &nbsp;&nbsp;
                    <asp:Button ID="btnBusinessRuleAddEditClose" runat="server" Text="Close" OnClick="btnBusinessRuleAddEditClose_Click"
                        CssClass="customButton"  ToolTip="click to close Business rules popup"/>
                    &nbsp;&nbsp;<asp:Button ID="btnBusinessRuleAddEditAddRule" runat="server" Text="Update" OnClick="btnBusinessRuleAddEditAddRule_Click" 
                                     CssClass="customButton" />
                    &nbsp;&nbsp;<asp:Label ID="lblBusinessRuleAddEditError" runat="server" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                </div>
            </ContentTemplate>
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="btnBusinessRuleAddEditClose" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnBusinessRuleAddEditAddRule" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- End ModalPopupExtender Show Business Rules AddEdit -->

    <!-- ModalPopupExtender Show  Error Message List -->
    <asp:HiddenField runat="server" ID="hfMpErrorMessageList" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpErrorMessageList" runat="server" PopupControlID="PanelErrorMessageList"
        TargetControlID="hfMpErrorMessageList" CancelControlID="hfMpErrorMessageList"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelErrorMessageList" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:UpdatePanel runat="server" ID="upErrorMessageList" UpdateMode="Conditional">
            <ContentTemplate>                               
                <div style="width: 100%; height:auto !important; max-height: 450px; overflow:auto; padding-bottom:5px;">
                    <span style="font-weight: bold;">Error Messages List</span>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Step: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblErrorMessageListStep" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Flow: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblErrorMessageListFlow" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Actor: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblErrorMessageListActorDoes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"></td>
                            <td align="right" style="padding-top: 5px; padding-right: 10px;" >
                                <span style="font-weight: bold;">System: </span>
                            </td>
                            <td style="padding-top: 5px;">
                                <asp:Label ID="lblErrorMessageListSystemDoes" runat="server"></asp:Label>
                            </td>
                        </tr>                    
                    </table>
                    <br /><hr />
                    <table width="100%">
                        <tr>
                            <td align="right">
                                 <asp:Button ID="btnErrorMessageListAddMessage" runat="server" Text="Add Error Message" OnClick="btnErrorMessageListAddMessage_Click" 
                                     CssClass="customButton" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:GridView ID="GridErrorMessages" runat="server" Width="100%" UseAccessibleHeader="True"
                                    GridLines="None" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="false"
                                    AllowPaging="true" PageSize="5" CssClass="table table-striped table-bordered table-condensed"
                                    DataKeyNames="EM_ObjectPK,UV_ID,EM_ShortName,EM_Name,EM_Note,EM_GUID" AllowSorting="false" OnRowCommand="GridErrorMessages_RowCommand" OnRowDataBound="GridErrorMessages_RowDataBound" OnPageIndexChanging="GridErrorMessages_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="Error Message" HeaderText="Error Message" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGridErrorMessageName" runat="server" Text='<%# Eval("EM_Name") %>' Font-Bold="true"></asp:Label>
                                                <br />
                                                <asp:Label ID="lblGridErrorMessageNote" runat="server"  Text='<%# Eval("EM_Note") %>' ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            ItemStyle-Width="15%" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkGridErrorMessageEditMessage" runat="server" Text="Edit" ToolTip="Click to edit Error Message"
                                                    CommandName="EditErrorMessage" CommandArgument='<%# Container.DataItemIndex %>'> 
                                                </asp:LinkButton>
                                                |
                                                <asp:LinkButton ID="lnkGridErrorMessageDeleteMessage" runat="server" Text="Delete" ToolTip="Click to Error Message"
                                                    CommandName="DeleteErrorMessage" CommandArgument='<%# Container.DataItemIndex %>'> 
                                                </asp:LinkButton>                                            
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Results
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <asp:Button ID="btnErrorMessageListClose" runat="server" Text="Close" OnClick="btnErrorMessageListClose_Click"
                    CssClass="customButton"  ToolTip="click to close Error Messages popup"/>
                &nbsp;&nbsp;<asp:Label ID="lblErrorMessageListCloseError" runat="server" Font-Bold="true"
                    ForeColor="Red"></asp:Label>
            </ContentTemplate>
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="btnErrorMessageListClose" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnErrorMessageListAddMessage" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- End ModalPopupExtender Show Error Messages List -->    
    <!-- ModalPopupExtender Add Edit Error Message -->
    <asp:HiddenField runat="server" ID="hfMpErrorMessageAddEdit" />
    <AjaxControlToolkit:ModalPopupExtender ID="mpErrorMessageAddEdit" runat="server" PopupControlID="PanelErrorMessageAddEdit"
        TargetControlID="hfMpErrorMessageAddEdit" CancelControlID="hfMpErrorMessageAddEdit"
        BackgroundCssClass="modalBackground">
    </AjaxControlToolkit:ModalPopupExtender>
    <asp:Panel ID="PanelErrorMessageAddEdit" runat="server" CssClass="modalPopup" align="left"
        Style="display: none; width: 60%;">
        <asp:UpdatePanel runat="server" ID="upErrorMessageAddEdit" UpdateMode="Conditional">
            <ContentTemplate>                
                <asp:HiddenField ID="hfErrorMessageAddEditEMGUID" runat="server" />
                <div style="width: 100%; height:auto !important; max-height: 400px; overflow:auto;">
                    <span style="font-weight: bold;">Error Messages Add/Edit</span>
                    <br />
                    <table width="100%">
                        <tr>
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Step: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblErrorMessageAddEditStep" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Flow: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblErrorMessageAddEditFlow" runat="server"></asp:Label>
                            </td>
                    
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Actor: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:Label ID="lblErrorMessageAddEditActorDoes" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4"></td>
                            <td align="right" style="padding-top: 5px; padding-right: 10px;" >
                                <span style="font-weight: bold;">System: </span>
                            </td>
                            <td style="padding-top: 5px;">
                                <asp:Label ID="lblErrorMessageAddEditSystemDoes" runat="server"></asp:Label>
                            </td>
                        </tr>                    
                    </table>
                    <hr />
                    <table width="100%">
                        <tr id="trErrorMessageAdd" runat="server">                        
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Exsisting Error Messages: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:DropDownList ID="ddlErrorMessageAddEditMessages" runat="server" ToolTip="exsiting Error Messages in the package" 
                                    AutoPostBack="True" OnSelectedIndexChanged="ddlErrorMessageAddEditMessages_SelectedIndexChanged" ></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="padding-top: 10px;padding-right:5px;" runat="server" id="tdErrorMessageEdit">
                                <span style="font-weight: bold;">Error Message ID: </span>
                                &nbsp;&nbsp;
                                <asp:Label ID="lblErrorMessagesAddEditMessageId" runat="server" ></asp:Label>
                            </td>
                            <td align="right" style="padding-top: 10px; padding-right: 10px;">
                                <span style="font-weight: bold;">Error Message Name: </span>
                            </td>
                            <td style="padding-top: 10px;">
                                <asp:TextBox ID="txtErrorMessageAddEditMessageName" runat="server" Width="300px" ToolTip="Error Message name"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>                        
                            <td style="height: 200px !important;" colspan="3">
                                <CKEditor:CKEditorControl ID="htmlEditorErrorMessageAddEditNote" BasePath="~/Plugins/ckeditor/"
                                    runat="server" Height="100px" EnterMode="DIV"></CKEditor:CKEditorControl>
                            </td>
                        </tr>
                    </table>               
                    &nbsp;&nbsp;
                    <asp:Button ID="btnErrorMessageAddEditClose" runat="server" Text="Close" OnClick="btnErrorMessageAddEditClose_Click"
                        CssClass="customButton"  ToolTip="click to close Error Messages popup"/>
                    &nbsp;&nbsp;<asp:Button ID="btnErrorMessageAddEditAddMessage" runat="server" Text="Update" OnClick="btnErrorMessageAddEditAddMessage_Click" 
                                     CssClass="customButton" />
                    &nbsp;&nbsp;<asp:Label ID="lblErrorMessageAddEditError" runat="server" Font-Bold="true"
                        ForeColor="Red"></asp:Label>
                </div>
            </ContentTemplate>
            <Triggers>                
                <asp:AsyncPostBackTrigger ControlID="btnErrorMessageAddEditClose" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnErrorMessageAddEditAddMessage" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
    <!-- End ModalPopupExtender Show Error Messages AddEdit -->
</asp:Content>
