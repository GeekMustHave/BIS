<%@ Page Title="BIS Reports" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="Reports.aspx.cs" Inherits="Admin_Reports" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="Server">
    <span style="font-weight: bold;">Reports</span> - <span>Proof Of Concept for Crystal
        Reports on BIS.</span>
    <br />
    <br />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
        HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" DisplayStatusbar="false"
        BestFitPage="True" ToolPanelView="None" HasToggleGroupTreeButton="false" HasToggleParameterPanelButton="true"
        HasZoomFactorList="True" />
</asp:Content>
