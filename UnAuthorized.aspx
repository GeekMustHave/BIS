<%@ Page Title="Sneaky" Language="C#" MasterPageFile="~/BISMaster.master" AutoEventWireup="true"
    CodeFile="UnAuthorized.aspx.cs" Inherits="UnAuthorized" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <table style="width: 100%;">
        <tr>
            <td align="center">
                <div style="margin: 0 auto !important;">
                    <br />
                    <asp:Label ID="lblUSerDetails" runat="server"></asp:Label>
                    <br />
                    <asp:Image ID="imgNOAccess" runat="server" AlternateText="Not Authrized to access"
                        ImageUrl="~/Images/UnAuthorized.jpg" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
