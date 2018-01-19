<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="asp_net_session_app.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btncalc" runat="server" Text="Button" OnClick="btncalc_Click" />
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Sum:"></asp:Label>
            :
            <asp:Label ID="lblsum" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
