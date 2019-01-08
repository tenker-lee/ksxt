<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="singlist.aspx.cs" Inherits="ksxt.Admin.singlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>单项选择题管理</title>
    <script src="../Scripts/jquery-2.1.0.min.js"></script>
    <script src="../Scripts/jquery.easyui-1.4.5.min.js"></script>
    <link href="../Content/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function () {
            $('#ee').click(function () {
                alert(this.value);
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding:10px">good</div>
        <input id="er" class="easyui-textbox" value="123"/>
        <input id="ee" type="button"  value="456"/>
    </form>
</body>
</html>
