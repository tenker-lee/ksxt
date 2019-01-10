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
                    //alert(JSON.stringify(data));                    
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
        });
        function showAddPannel() {
            $('#win').window('open');
            //$('#tt').datagrid('load',{code: '01',name: 'name01'});
        }
        function hideAddPannel() {
            $('#win').window('close');
        }
        function ClearForm() {
            $('#ff').form('clear');
        }
        function Search() {
            //alert("search");
            $('#tt').datagrid('reload');
        }
        function SubmitToSvr(url) {
            // submit the form            
            $('#ff').form('submit', {
	            url: "HandlerSingle.ashx?opt=add",
	            onSubmit: function(){
		            //var isValid = $(this).form('validate');
		            //if (!isValid){
			           // $.messager.progress('close');	// 如果表单是无效的则隐藏进度条
		            //}
		            //return isValid;	// 返回false终止表单提交
	            },
                success: function (data) {
                    alert(JSON.parse(data));
		            //$.messager.progress('close');	// 如果提交成功则隐藏进度条
	            }
            });
        }
        function ModifyData() {

        }
        function DelData() {

        }
    </script>
</head>
<body>
    <div id="" style="text-align: center; padding: 5px">
        <span><b>单项选择题库</b></span>
    </div>
    <div id="searchbar" style="border: thin solid #C0C0C0; text-align: right; padding: 20px">
        <a id="btn" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="Search()">查询&刷新</a>
    </div>
    <br />
    <br />
    <div id="toolbar" style="text-align: right; display: inline">
        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="showAddPannel()">添加</a>
        <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ModifyData()">修改</a>
        <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="DelData()">删除</a>
    </div>
    <table id="tt" class="easyui-datagrid" style="width: auto;" multiple="false">
        <thead>
            <tr>
                <th field="ck" checkbox="true"></th>
                <th field="v_id">编号</th>
                <th field="v_level" width="80">难度等级</th>
                <th field="v_title" align="center" width="50">题目</th>
                <th field="v_select_arry" align="left" width="50">选项</th>
                <th field="v_answer_arry" align="center" width="50">答案</th>
                <th field="v_create_name" align="center" width="50">创建人</th>
                <th field="v_create_time" align="center" width="80">创建时间</th>
            </tr>
        </thead>
    </table>
    <div id="win" class="easyui-window" hidden="true" style="width: 600px; height: 550px" data-options="modal:true">
        <form id="ff" method="post">
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <input id="f_id" name="f_id" type="hidden" value="" />
                <label for="f_title">题 目:</label>
                <input id="f_title" name="f_title" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 95px;">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_level">困难等级</label>
                <select id="f_level" name="f_level">
                    <option value="0" selected="true">无</option>
                    <option value="1">初级</option>
                    <option value="2">中级</option>
                    <option value="3">高级</option>
                </select>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="selectA">选项A:</label>
                <input id="selectA" name="selectA" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            
            <div style="text-align: center; padding: 10px">
                <label for="f_selectB">选项B:</label>
                <input id="f_selectB" name="f_selectB" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_selectC">选项C:</label>
                <input id="f_selectC" name="f_selectC" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_selectD">选项D:</label>
                <input id="f_selectD" name="f_selectD"  class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px">
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_singleAnswer">答案:</label>
                <input type="radio" name="f_singleAnswer" value="0">A</input>
                <input type="radio" name="f_singleAnswer" value="1">B</input>
                <input type="radio" name="f_singleAnswer" value="2">C</input>
                <input type="radio" name="f_singleAnswer" value="3">D</input>
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
