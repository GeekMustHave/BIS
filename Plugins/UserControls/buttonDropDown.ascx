<%@ Control Language="C#" AutoEventWireup="true" CodeFile="buttonDropDown.ascx.cs" Inherits="Plugins_UserControls_buttonDropDown" %>
<link href="Styles/ButtonDropDown.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    //For the Custom DropDown Button Click 
    function GoToReportsPage(inPageUrl) {
        window.location = ResolveUrl(inPageUrl);
    }
    function ResolveUrl(url) {
        var baseUrl = '<%= Page.ResolveUrl("~/") %>';

        if (url.indexOf("~/") == 0) {
            url = baseUrl + url.substring(2);
        }
        return url;
    }
</script>

<div style="display: inline-block; vertical-align: top;" class="customDivDropDownBtn">
    <asp:Button ID="btnRequiremtnReports" runat="server" Text="Reports" CssClass="customButton" OnClientClick="javascript:return false;" />
    <!-- dropdown menu -->    
    <ul class="customBtnDropDown ui-corner-bottom" id="ddlCustomDropDown" runat="server">
        <%--<li onclick="GoToReportsPage('~/RVDReport.aspx');">RVD-Requirement Validation Document</li>
        <li onclick="GoToReportsPage('~/RTMReport.aspx');">RTM-Requirement Traceability Matrix</li>
        <li onclick="GoToReportsPage('~/UCDReport.aspx');">Use-Case Details (UCD) Report</li>--%>
    </ul>
</div>
