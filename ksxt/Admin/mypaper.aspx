<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mypaper.aspx.cs" Inherits="ksxt.Admin.mypaper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-2.1.0.min.js"></script>
    <script src="../Scripts/jquery.easyui-1.4.5.min.js"></script>
    <script src="../Scripts/locale/easyui-lang-zh_CN.js"></script>
    <link href="../Content/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Content/themes/icon.css" rel="stylesheet" type="text/css" />
    <link href="main.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function () {
            $('#tt').datagrid({
                url: "HandlerMyPaper.ashx?opt=query",
                fit: false,
                autoRowHeight: false,
                striped: true,
                pageNumber: 1,
                pageSize: 10,
                singleSelect: true,
                pagination: true,
                rownumbers: true,
                nowrap: false,
                onSelect: function (rowIndex, rowData) {
                },
                onLoadSuccess: function (data) {
                    //alert("");
                }
            });
        });
        function Search() {
            $('#tt').datagrid('reload');
        }
        function formatOper(val, row, index) {
            var str = "<a target=\"_blank\" href=\"../ExamPaper.aspx?paperid=" + row.v_id + "\">开始考试</a>";
            str = str + "&nbsp&nbsp&nbsp";
            return str;
        }        
    </script>
</head>
<body>
    <div id="searchbar" style="border: thin solid #C0C0C0; text-align: right; padding: 5px">        
        <a id="btn" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="Search()">查询&刷新</a>
    </div>
    <table id="tt" class="easyui-datagrid" style="width: auto; margin-top: 5px" data-options="">
        <thead>
            <tr>
                <th data-options="field:'ck',checkbox:true"></th>
                <th data-options="field:'v_id'">编号</th>
                <th data-options="field:'v_title',width:230">试卷名称</th>        
                <th data-options="field:'v_start_time', align:'center',width:130">开始时间</th>
                <th data-options="field:'v_end_time', align:'center',width:130">结束时间</th>
                <th data-options="field:'v_oporate',formatter:formatOper, align:'center'">操作</th>
            </tr>
        </thead>
    </table>
</body>
</html>
