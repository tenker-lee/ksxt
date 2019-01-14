<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="singlist.aspx.cs" Inherits="ksxt.Admin.singlist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>单选题管理</title>
    <script src="../Scripts/jquery-2.1.0.min.js"></script>
    <script src="../Scripts/jquery.easyui-1.4.5.min.js"></script>
    <script src="../Scripts/locale/easyui-lang-zh_CN.js"></script>
    <link href="../Content/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Content/themes/icon.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function () {
            $('#tt').datagrid({
                url: "HandlerSingle.ashx?opt=query",
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
                        url: 'HandlerSingle.ashx?opt=SearchById',
                        type: "POST",
                        data: { "s_id": selrow.v_id },
                        success: function (data) {
                            var v = JSON.parse(data);
                            //alert(v);
                            $('#f_level').combobox("setValue", v.level);
                            $('#f_title').textbox("setValue", v.title);
                            $('#f_selectA').textbox("setValue", v.s_a);
                            $('#f_selectB').textbox("setValue", v.s_b);
                            $('#f_selectC').textbox("setValue", v.s_c);
                            $('#f_selectD').textbox("setValue", v.s_d);
                            $("input:radio[name='f_singleAnswer'][value='" + v.s_answer + "']").prop("checked", "checked");
                        }
                    });
                }
                $('#win').window({ title: "编辑(单选题)" });
            }
            else {
                $('#btnType').val("");
                $('#win').window({ title: "添加(单选题)" });
            }
            $('#win').window('open');
        }
        function hideAddPannel() {
            $('#win').window('close'); Search();
        }
        function ClearForm() {
            //$('#ff').form('clear');
            //$("input[value='1']").attr('checked', 'checked');            
            //$('#f_level').combobox('select', 0);
        }
        function CheckForm() {
            return true;
        }
        function Search() {
            $('#tt').datagrid('reload');
        }
        function Confirm() {
            //alert($('#btnType').val());
            if ($('#btnType').val() == "edit") {
                ModifyData();
            }
            else {
                SubmitToSvr();
            }
        }
        function SubmitToSvr() {
            $('#ff').form('submit', {
                url: "HandlerSingle.ashx?opt=add",
                onSubmit: function () {
                    return CheckForm();
                },
                success: function (data) {
                    var result = JSON.parse(data);
                    $.messager.alert('警告', result.msg);
                    if (result.stateCode == 0) {                        
                        ClearForm();
                    } 
                }
            });
        }
        function ModifyData() {
            $('#ff').form('submit', {
                type: 'post',
                url: "HandlerSingle.ashx?opt=edit",
                onSubmit: function (param) {
                    var selrow = $('#tt').datagrid('getSelected');
                    if (null != selrow)
                        param.edit_id = selrow.v_id;
                    else
                        return false;
                },
                success: function (data) {
                    var result = JSON.parse(data);
                    //$.messager.alert('警告', result.msg);
                    $.messager.show({
	                    title:'提示',
	                    msg:result.msg,
	                    timeout:5000,
	                    showType:'slide'
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
	        if(b){
                $.ajax({
                url: 'HandlerSingle.ashx?opt=del',
                type: "POST",
                data: { "delid": selrow.v_id },
                success: function (data) {
                            var v = JSON.parse(data);
                            //alert(v.msg);
                            Search();
                    }
                });
	          }
            });            
        }
        function formatOper(val,row,index){  
                return "<a href=\"#\" class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-add',plain:false\" onclick=\"\">添加</a>";  
        }
        function AddToPaper(id) {
            alert("add to paper");
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
    <div id="toolbar" style="text-align: left;">
        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true"  onclick="showAddPannel('')">添加</a>
        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="showAddPannel('edit')">修改</a>
        <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true"  onclick="DelData()">删除</a>
    </div>
    <table id="tt" class="easyui-datagrid" style="width: auto;" data-options="">
        <thead>
            <tr>
                <th data-options="field:'ck',checkbox:true" ></th>
                <th data-options="field:'v_id'">编号</th>
                <th data-options="field:'v_level',width:80" >难度等级</th>
                <th data-options="field:'v_title',width:300">题目</th>
                <th data-options="field:'v_select_arry',align:'left'">选项</th>
                <th data-options="field:'v_answer_arry', align:'center',width:50">答案</th>
                <th data-options="field:'v_create_name', align:'center',width:80">创建人</th>
                <th data-options="field:'v_create_time', align:'center',width:120">创建时间</th>
                <th data-options="field:'v_oporate',formatter:formatOper, align:'center'">操作</th>
            </tr>
        </thead>
    </table>
    <div id="win" class="easyui-window" style="width: 600px; height: 550px" data-options="modal:true">
        <form id="ff" method="post">
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <input id="f_id" name="f_id" type="hidden" value="" />
                <label for="f_title">题 目:</label>
                <input id="f_title" name="f_title" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 50px;"/>
            </div>
            <div style="text-align: center; vertical-align:central; padding: 10px">
                <label for="f_level">难度等级</label>             
                <select id="f_level" name="f_level" class="easyui-combobox" data-options="select:0"  style="width:80px" >
                    <option value="0" >请选择难度等级</option>
                    <option value="1">初级</option>
                    <option value="2">中级</option>
                    <option value="3">高级</option>
                </select>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="selectA">选项A:</label>
                <input id="f_selectA" name="f_selectA" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_selectB">选项B:</label>
                <input id="f_selectB" name="f_selectB" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_selectC">选项C:</label>
                <input id="f_selectC" name="f_selectC" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_selectD">选项D:</label>
                <input id="f_selectD" name="f_selectD" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_singleAnswer">答案:</label>
                <input  type="radio"  name="f_singleAnswer" value="0"/>A
                <input  type="radio"  name="f_singleAnswer" value="1"/>B
                <input  type="radio"  name="f_singleAnswer" value="2"/>C
                <input  type="radio"  name="f_singleAnswer" value="3"/>D
            </div>
            <div style="text-align: center; padding: 10px">
                <input id="btnType" type="hidden" value="" />
                <a id="btnClear" href="#" class="easyui-linkbutton" onclick="ClearForm()" data-options="iconCls:'icon-cancel'">重置</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                <a id="btnConfirm" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" onclick="Confirm()">确定</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                <a id="btnCancel" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="hideAddPannel()">取消</a>&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </form>
    </div>
</body>
</html>
