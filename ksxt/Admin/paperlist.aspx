<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paperlist.aspx.cs" Inherits="ksxt.Admin.paperlist" %>

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
    <script>
        $(document).ready(function () {
            $('#tt').datagrid({
                url: "HandlerPaper.ashx?opt=query",
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
            $('#win').window({
                title: "添加&编辑",
                collapsible: false,
                minimizable: false,
                maximizable: false,
                shadow: true,
                modal: true,
                closed: true
            });
        });
        function showAddPannel(opt) {
            if (opt == "edit") {
                $('#btnType').val("edit");
                var selrow = $('#tt').datagrid('getSelected');
                if (null == selrow) {
                    $.messager.alert('警告', "未选择要编辑项!!!!");
                    return;
                }
                else {
                    $.ajax({
                        url: 'HandlerPaper.ashx?opt=SearchById',
                        type: "POST",
                        data: { "s_id": selrow.v_id },
                        success: function (data) {
                            var result = JSON.parse(data);
                            $('#f_title').textbox("setValue", result.title);
                            $('#f_choice_score').textbox("setValue", result.choice_score);
                            $('#f_filling_score').textbox("setValue", result.filling_score);
                            $('#f_judge_score').textbox("setValue", result.judge_score);
                            $('#f_qa_score').textbox("setValue", result.qa_score);
                            $('#f_start_time').textbox("setValue", result.start_time);
                            $('#f_end_time').textbox("setValue", result.end_time);
                        }
                    });
                }
                $('#win').window({ title: "编辑" });
            }
            else {
                $('#btnType').val("");
                $('#win').window({ title: "添加" });
            }
            $('#win').window('open');
        }
        function hideAddPannel() {
            $('#win').window('close'); Search();
        }
        function ClearForm() {
            $('#ff').form('clear');
        }
        function CheckForm() {
            return true;
        }
        function Search() {
            $('#tt').datagrid('reload');
        }
        function Confirm() {
            if ($('#btnType').val() == "edit") {
                ModifyData();
            }
            else {
                SubmitToSvr();
            }
        }
        function SubmitToSvr() {
            $('#ff').form('submit', {
                url: "HandlerPaper.ashx?opt=add",
                onSubmit: function () {
                    return CheckForm();
                },
                success: function (data) {
                    var result = JSON.parse(data);
                    $.messager.alert('警告', result.msg);
                    if (result.stateCode == 0) {
                        //ClearForm();
                    }
                }
            });
        }
        function ModifyData() {
            $('#ff').form('submit', {
                type: 'post',
                url: "HandlerPaper.ashx?opt=edit",
                onSubmit: function (param) {
                    var selrow = $('#tt').datagrid('getSelected');
                    if (null != selrow)
                        param.edit_id = selrow.v_id;
                    else
                        return false;
                },
                success: function (data) {
                    var result = JSON.parse(data);
                    $.messager.show({
                        title: '提示',
                        msg: result.msg,
                        timeout: 5000,
                        showType: 'slide'
                    });
                    if (result.stateCode == 0) {
                        hideAddPannel();
                    }
                }
            });
        }
        function DelData() {
            var selrow = $('#tt').datagrid('getSelected');
            if (null == selrow) {
                $.messager.alert('警告', "未选择要删除项!!!!");
                return;
            }
            $.messager.confirm('警告', '是否删除 id=' + selrow.v_id + ' 项', function (b) {
                if (b) {
                    $.ajax({
                        url: 'HandlerPaper.ashx?opt=del',
                        type: "POST",
                        data: { "delid": selrow.v_id },
                        success: function (data) {
                            var result = JSON.parse(data);
                            if (result.stateCode == 0) {
                                $.messager.show({
                                    title: '提示',
                                    msg: result.msg,
                                    timeout: 5000,
                                    showType: 'slide'
                                });
                            }
                            Search();
                        }
                    });
                }
            });
        }
    </script>
</head>
<body>
    <div id="searchbar" style="border: thin solid #C0C0C0; text-align: right; padding: 5px">
        <a id="btn" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-search'" onclick="Search()">查询&刷新</a>
    </div>
    <div id="toolbar" style="text-align: left;">
        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="showAddPannel('')">添加</a>
        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="showAddPannel('edit')">修改</a>
        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true" onclick="DelData()">删除</a>
    </div>
    <table id="tt" class="easyui-datagrid" style="width: auto;" data-options="">
        <thead>
            <tr>
                <th data-options="field:'ck',checkbox:true"></th>
                <th data-options="field:'v_id'">编号</th>
                <th data-options="field:'v_title',width:300">题目</th>
                <th data-options="field:'v_choice_score',width:75">单选分值</th>
                <th data-options="field:'v_filling_score',width:75">填空分值</th>
                <th data-options="field:'v_judge_score',width:75">判断分值</th>
                <th data-options="field:'v_qa_score',width:75">简答分值</th>
                <th data-options="field:'v_start_time', align:'center',width:120">开始时间</th>
                <th data-options="field:'v_end_time', align:'center',width:120">结束时间</th>
                <th data-options="field:'v_create_name', align:'center',width:80">创建人</th>
                <th data-options="field:'v_create_time', align:'center',width:120">创建时间</th>
            </tr>
        </thead>
    </table>
    <div id="win" class="easyui-window" style="width: 600px; height: 480px" data-options="modal:true">
        <form id="ff" method="post">
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <input id="f_id" name="f_id" type="hidden" value="" />
                <label for="f_title">试卷名称:</label>
                <input id="f_title" name="f_title" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 50px;" />
            </div>
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <label for="f_choice_score">单选分值:</label>
                <input type="text" id="f_choice_score" name="f_choice_score" class="easyui-numberbox" value="5" data-options="min:0,precision:0">
            </div>
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <label for="f_filling_score">填空分值:</label>
                <input type="text" id="f_filling_score" name="f_filling_score" class="easyui-numberbox" value="5" data-options="min:0,precision:0">
            </div>
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <label for="f_judge_score">判断分值:</label>
                <input type="text" id="f_judge_score" name="f_judge_score" class="easyui-numberbox" value="2" data-options="min:0,precision:0">
            </div>
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <label for="f_qa_score">问答分值:</label>
                <input type="text" id="f_qa_score" name="f_qa_score" class="easyui-numberbox" value="20" data-options="min:0,precision:0">
            </div>
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <label for="">开始时间:</label>
                <input class="easyui-datetimebox" name="f_start_time"
                    data-options="required:true,showSeconds:false" style="width: 150px" />
            </div>
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <label for="">结束时间:</label>
                <input class="easyui-datetimebox" name="f_end_time"
                    data-options="required:true,showSeconds:false" style="width: 150px" />
            </div>
            <div style="text-align: center; padding: 10px">
                <div>
                    <input id="btnType" type="hidden" value="" />
                </div>
                <a id="btnClear" href="#" class="easyui-linkbutton" onclick="ClearForm()" data-options="iconCls:'icon-cancel'">重置</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                <a id="btnConfirm" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" onclick="Confirm()">确定</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                <a id="btnCancel" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="hideAddPannel()">取消</a>&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </form>
    </div>
</body>
</html>
