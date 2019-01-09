<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="singlist.aspx.cs" Inherits="ksxt.Admin.singlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-2.1.0.min.js"></script>
    <script src="../Scripts/jquery.easyui-1.4.5.min.js"></script>
    <script type="text/javascript" src="../Scripts/locale/easyui-lang-zh_CN.js"></script>
    <link href="../Content/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Content/themes/icon.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function () {
            $('#tt').datagrid({
                url: "HandlerSingle.ashx?opt=query",
                fit: false,
                autoRowHeight: true,
                striped: true,
                pageNumber: 1,
                pageSize: 10,
                singleSelect: true,
                pagination: true,
                rownumbers: true,
                onSelect: function (rowIndex, rowData) {
                    //alert(rowIndex);
                    //alert(rowData["id"]);
                },
                onLoadSuccess: function (data) {
                    alert(JSON.stringify(data));                    
	            }
            });
            var pager = $('#tt').datagrid('getPager');
            pager.pagination({
                onSelectPage: function (pageNum, pageSize) {
                    //alert(pageNum);
                    //alert(pageSize);
                }
            });
            $('#win').window({
                title: "添加&编辑(单选题)",
                collapsible: false,
                minimizable: false,
                maximizable: false,
                shadow: true,
                modal: true,
                closed: true
            });
            $('#ff').form({
                url: "HandlerSingle.ashx?opt=edit",
                onSubmit: function (param) {
                    alert(JSON.stringify(param));
                    // do some check
                    // return false to prevent submit;
                },
                success: function (data) {
                    alert(data)
                }
            });
        });
        function showAddPannel() {
            $('#win').window('open');
            //$('#tt').datagrid('load',{code: '01',name: 'name01'});
        }
        function hideAddPannel() {
            $('#win').window('close');
        }
        function ClearForm() {
            $('#singleTitle').textbox('setText', "");
            $('#singleA').textbox('setText', "");
            $('#singleB').textbox('setText', "");
            $('#singleC').textbox('setText', "");
            $('#singleD').textbox('setText', "");
        }
        function Search() {
            alert("search");
        }
        function SubmitToSvr(url) {
            // submit the form
            //alert("");
            $('#ff').submit();
        }
    </script>
</head>
<body>
    <div id="" style="text-align: center; padding: 5px">
        <span><b>单项选择题库</b></span>
    </div>
    <div id="searchbar" style="border: thin solid #C0C0C0; text-align: right; padding: 20px">
        <a id="btn" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'">查询&刷新</a>
    </div>
    <br />
    <br />
    <div id="toolbar" style="text-align: right; display: inline">
        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="showAddPannel()">添加</a>
        <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="javascript:void(0)">修改</a>
        <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="javascript:void(0)">删除</a>
    </div>
    <table id="tt" class="easyui-datagrid" style="width: auto;" multiple="false">
        <thead>
            <tr>
                <th field="ck" checkbox="true"></th>
                <th field="id">编号</th>
                <th field="level">难度等级</th>
                <th field="titile" align="center">题目</th>
                <th field="selects" align="left">选项</th>
                <th field="answers" align="center">答案</th>
                <th field="create_name" align="center">创建人</th>
                <th field="create_time" align="center">创建时间</th>
            </tr>
        </thead>
    </table>
    <div id="win" class="easyui-window" hidden="true" style="width: 600px; height: 550px" data-options="modal:true">
        <form id="ff" method="post">
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <label for="name">题 目:</label>
                <input id="singleTitle" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 95px;">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="email">选项A:</label>
                <input id="singleA" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="email">选项B:</label>
                <input id="singleB" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="email">选项C:</label>
                <input id="singleC" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="email">选项D:</label>
                <input id="singleD" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            <div style="text-align: center; padding: 10px">
                <label>答案:</label>
                <input type="radio" name="singleAnswer" value="0">A</input>
                <input type="radio" name="singleAnswer" value="1">B</input>
                <input type="radio" name="singleAnswer" value="2">C</input>
                <input type="radio" name="singleAnswer" value="3">D</input>
            </div>
            <div style="text-align: center; padding: 10px">
                <a id="btnClear" href="#" class="easyui-linkbutton" onclick="ClearForm()" data-options="iconCls:'icon-cancel'">重置</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                    <a id="btnConfirm" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" onclick="SubmitToSvr()">确定</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                    <a id="btnCancel" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="hideAddPannel()">取消</a>&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </form>
    </div>
</body>
</html>
