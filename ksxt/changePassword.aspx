<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changePassword.aspx.cs" Inherits="ksxt.changePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>修改密码</title>
    <style type="text/css">
        .auto-style1 {
            width: 111px;
        }
    </style>
</head>
<body style="text-align:center">
   <form id="form1" runat="server">
        <div style="background-color: whitesmoke;text-align:center">
            <table style="margin: 0 auto;width:500px" class="auto-style2">
                <tr>
                    <td style="text-align: right" class="auto-style1">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: right" class="auto-style1">
                        <asp:Label ID="Label1" runat="server" Text="旧密码:"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtOldPass" runat="server" Width="192px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="auto-style1">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1" style="text-align:right">
                        <asp:Label ID="Label2" runat="server" Text="新密码:"></asp:Label>
                    </td>
                    <td style="text-align:left">
                        <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" Width="192px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="auto-style1">
                        &nbsp;</td>
                    <td style="text-align: left">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td style="text-align: right" class="auto-style1">
                        <asp:Label ID="Label3" runat="server" Text="确认新密码:"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtNewPassConfim" runat="server" TextMode="Password" Width="192px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">&nbsp;</td>
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
