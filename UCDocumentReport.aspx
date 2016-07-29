<%@ Page Title="Use-Cases Document Report" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true" CodeFile="UCDocumentReport.aspx.cs" Inherits="UCDocumentReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlrunReprot" DefaultButton="btnRunReport" runat="server">
        <span style="font-weight: bold;">Use-Cases Document Report.</span>
        <br />
        <table width="100%">
            <tr>
                <td align="right" style="padding-bottom: 5px; padding-right: 10px; width: 30%;">
                    <span>Select Package </span>
                </td>
                <td align="left" style="padding-bottom: 5px;">
                    <asp:DropDownList ID="ddlPackage" runat="server" ToolTip="Select a package" Width="80%">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvPackage" runat="server" ControlToValidate="ddlPackage"
                        CssClass="failureNotification" ErrorMessage=" *Required. " ValidationGroup="valGroupRunReport"
                        InitialValue="null" Display="Dynamic" ToolTip="Package is required" Font-Bold="true">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="trfuturevisible" runat="server" visible="false">
                <td align="right" style="padding-bottom: 5px; padding-right: 10px;">
                    <span>Cover Page Sub-Title</span>
                </td>
                <td align="left" style="padding-bottom: 5px;">
                    <asp:TextBox ID="txtSubTitle" runat="server" ToolTip="Enter Sub Title" Width="78%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-bottom: 5px; padding-right: 10px;">
                    <span>Do NOT print Hidden Use-Cases</span>
                </td>
                <td align="left" style="padding-bottom: 5px;">
                    <asp:CheckBox ID="chkHiddenUC" runat="server" Checked="true" ToolTip="check to hide hidden usecases" />
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-bottom: 5px;"></td>
                <td align="left" style="padding-bottom: 5px;">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="customButton" OnClick="btnCancel_Click" />
                    &nbsp;
                    <asp:Button ID="btnRunReport" runat="server" Text="Run" CssClass="customButton" CausesValidation="true"
                        ValidationGroup="valGroupRunReport" OnClick="btnRunReport_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlReportViewer" runat="server">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" Height="400px"
            HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" DisplayStatusbar="false"
            BestFitPage="True" ToolPanelView="None" HasToggleGroupTreeButton="false" HasToggleParameterPanelButton="false"
            HasZoomFactorList="True" HasRefreshButton="False" />
    </asp:Panel>
</asp:Content>

