<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logon.aspx.cs" Inherits="ksxt.logon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录系统</title>
    <style type="text/css">
        .auto-style2 {
            width: 500px;
        }
    </style>
</head>
<body style="text-align: center">
    <form id="form1" runat="server">
        <div style="background-color: whitesmoke">
            <table style="margin: 0 auto;" class="auto-style2">
                <tr>
                    <td style="text-align: right">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="帐号:"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtName" runat="server" Width="192px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="密码:"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="192px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="text-align: left">
                        <asp:Button ID="Button1" runat="server" Text="确定" Width="103px" />&nbsp;&nbsp;&nbsp;
                    <input id="Reset1" type="reset" value="重置" /></td>
                </tr>
            </table>
            <br />
        </div>
        <br />
    </form>
</body>
</html>
