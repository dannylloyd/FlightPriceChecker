<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="index.aspx.vb" Inherits="SouthwestAirlines.index" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Southwest Fare Check</title>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.15/themes/vader/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="Content/southwest.css" rel="stylesheet" type="text/css" media="screen" />
    <%--<link href="content/mobile.css" rel="stylesheet" type="text/css" media="screen" />--%>
    <link href="Content/mobile.css" rel="stylesheet" type="text/css" media="screen and (max-device-width: 1024px)" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.15/jquery-ui.min.js"></script>
    <script src="Scripts/jquery.sorting.js" type="text/javascript"></script>
    <script src="Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="Scripts/southwest.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Southwest flight fare checker thing</h1>
        <div id="search">
            <asp:DropDownList ID="ddlOriginAirport" runat="server"></asp:DropDownList>
            <asp:DropDownList ID="ddlDestinationAirport" runat="server"></asp:DropDownList>        
            Departure Date<asp:TextBox ID="txtDepartureDate" CssClass="datepicker" runat="server"></asp:TextBox>
            Return Date<asp:TextBox ID="txtReturnDate" CssClass="datepicker" runat="server"></asp:TextBox>
            <asp:Button ID="btnSubmit" runat="server" Text="Search" />
            <asp:Button ID="btnGoToFare" runat="server" Text="Show Southwest UI" />
        </div>
        <div id="continue"></div>
        <div id="results" style="display: none;">
            <asp:Literal ID="litResult" runat="server"></asp:Literal>            
        </div>
    </form>
</body>
</html>
