<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="asp_net_session_app.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <strong>Sessions:<br />
            </strong>
            <br />
            <asp:Button ID="btncalc" runat="server" Text="Button" OnClick="btncalc_Click" />
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Text="Sum:"></asp:Label>
            :
            <asp:Label ID="lblsum" runat="server" Text=""></asp:Label>
        </div>

        <div>
            <p>
                Cache  
                <br /> Key: <asp:TextBox ID="txtKey" runat="server">mykey</asp:TextBox>
                <br /> Duration: <asp:TextBox ID="txtDuration" runat="server">1</asp:TextBox>
                <br /> Value: <asp:TextBox ID="txtValue" runat="server"></asp:TextBox>
                <br /> Last read: <asp:Label ID="lblLastread" runat="server"></asp:Label>
                <br /> Time to process: <asp:Label ID="lblTime" runat="server"></asp:Label>
                <br /> 
                <asp:Button ID="btnGetCache" runat="server" Text="Get cache" OnClick="btnGetCache_Click" />
                <asp:Button ID="btnSetCache" runat="server" Text="Set cache" OnClick="btnSetCache_Click" />
            </p>
        </div>
    </form>

</body>
</html>
