<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checkpaper.aspx.cs" Inherits="ksxt.Admin.checkpaper" %>

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
                url: "HandlerCheckPaper.ashx?opt=query",
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
                }
            });
        });
        function Search() {
            $('#tt').datagrid('reload');
        }
        function formatOper(val, row, index) {
            //alert(val);
            if (val != "1")
                return;
            //alert(row.v_id);
            var str = "<a target=\"_blank\" href=\"../GradePaper.aspx?paperid=" + row.v_paper_id + "&userid="+row.v_uid+"\">评分</a>";
            str = str + "&nbsp&nbsp&nbsp";
            return str;
        }        
    </script>
</head>
<body>
    <div id="searchbar" style="border: thin solid #C0C0C0; text-align: right; padding: 5px">
        <div style="width: auto; text-align: center; display: inline-block; float: left; font-size: 12px">
            姓名: &nbsp;&nbsp;
            <input class="easyui-textbox" id="search_name" data-options="readonly:false" style="width: 100px" />
            &nbsp;&nbsp;&nbsp;&nbsp;试卷: &nbsp;&nbsp;<input class="easyui-textbox" id="search_paper_name" data-options="readonly:false" style="width: 100px" />
        </div>
        <a id="btn" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="Search()">查询&刷新</a>
    </div>
    <table id="tt" class="easyui-datagrid" style="width: auto; margin-top: 5px" data-options="">
        <thead>
            <tr>
                <th data-options="field:'ck',checkbox:true"></th>
                <th data-options="field:'v_id'">编号</th>
                <th data-options="field:'v_paper_id'">试卷编号</th>
                <th data-options="field:'v_title'">试卷名称</th>
                <th data-options="field:'v_uid'">学生编号</th>
                <th data-options="field:'v_user_name', align:'center',width:80">考生</th>
                <th data-options="field:'v_total_score', align:'center',width:80">得分</th>
                <th data-options="field:'v_check_state', align:'center',width:100">试卷状态</th>
                <th data-options="field:'v_check_name', align:'center',width:100">评分人</th>
                <th data-options="field:'v_check_time', align:'center',width:130">评分时间</th>
                <th data-options="field:'v_utype',formatter:formatOper, align:'center'">操作</th>
            </tr>
        </thead>
    </table>
</body>
</html>
