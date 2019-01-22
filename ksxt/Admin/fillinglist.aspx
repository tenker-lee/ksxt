<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fillinglist.aspx.cs" Inherits="ksxt.Admin.fillinglist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../Scripts/jquery-2.1.0.min.js"></script>
    <script src="../Scripts/jquery.easyui-1.4.5.min.js"></script>
    <script src="../Scripts/locale/easyui-lang-zh_CN.js"></script>
    <link href="../Content/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Content/themes/icon.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function () {
            $('#tt').datagrid({
                url: "HandlerFilling.ashx?opt=query",
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
             $('#dg_paper_list').datagrid({
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
                    $('#select_paper_id').textbox('setValue', rowData["v_id"]);
                    $('#select_paper_title').textbox('setValue', rowData["v_title"]); 
                    $('#select_ids').textbox('setValue', rowData["v_filling_id_arry"]);
                },
                onLoadSuccess: function (data) {
                }
            });            
            $('#win_paper_list').window({
                title: "选择试卷",
                collapsible: false,
                minimizable: false,
                maximizable: false,
                shadow: true,
                modal: true,
                closed: true,
                onClose: function () {
                    Search();
                }
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
                        url: 'HandlerFilling.ashx?opt=SearchById',
                        type: "POST",
                        data: { "s_id": selrow.v_id },
                        success: function (data) {
                            var result = JSON.parse(data);
                            $('#f_level').combobox("setValue", result.level);
                            $('#f_title').textbox("setValue",  result.title);
                            $('#f_answerA').textbox("setValue", result.a_a);
                            $('#f_answerB').textbox("setValue", result.a_b);
                            $('#f_answerC').textbox("setValue", result.a_c);
                            $('#f_answerD').textbox("setValue", result.a_d);
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
        function showPaperList() {
            $('#select_paper_id').textbox('setValue',"");
            $('#select_paper_title').textbox('setValue',"");
            $('#win_paper_list').window('open');
            $('#dg_paper_list').datagrid("reload");
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
                url: "HandlerFilling.ashx?opt=add",
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
                url: "HandlerFilling.ashx?opt=edit",
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
                url: 'HandlerFilling.ashx?opt=del',
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
        function formatOper(val, row, index) {
            //alert(row.v_id);
            var str = "<a href=\"#\" onclick=\"AddToPaper('add'," + row.v_id + ")\"><i>添加到试卷</i></a>";
            str = str + "&nbsp&nbsp&nbsp";
            str = str + "<a href=\"#\" onclick=\"AddToPaper('del'," + row.v_id + ")\"><i>从试卷移除</i></a>";
            return str;
        }
        function AddToPaper(type, title_id) {
            var paper_id = $('#select_paper_id').textbox("getValue");
            if (paper_id == "") {
                $.messager.alert("提示", "请选择要添加的试卷!!!!");
                return;
            }
            //alert(paper_id);
            $.ajax({
                url: 'HandlerPaper.ashx?opt=AddFillingToPaper',
                type: "POST",
                data: { "optType": type, "paper_id": paper_id, "title_id": title_id },
                success: function (data) {
                    //alert(data);
                    var result = JSON.parse(data);
                    if (result.stateCode == 0) {
                        $.messager.show({
                            title: '提示',
                            msg: result.msg,
                            timeout: 5000,
                            showType: 'slide'
                        });
                        $('#select_ids').textbox("setValue", result.title_list);
                    } else {
                        $.messager.alert("提示",result.msg);
                    }
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
        <table style="width: 100%;">
            <tr>
                <td style="text-align:left;width:20%">
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true"  onclick="showAddPannel('')">添加</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-edit',plain:true" onclick="showAddPannel('edit')">修改</a>
                    <a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-remove',plain:true"  onclick="DelData()">删除</a>
                </td>
                <td style="text-align:right;padding-right:5px;vertical-align:central">
                    <b>当前试卷信息:</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <small>编号</small>&nbsp;&nbsp;<input class="easyui-textbox" id="select_paper_id" data-options="readonly:true" style="width:50px">&nbsp;
                    <small>标题</small>&nbsp;&nbsp;<input class="easyui-textbox" id="select_paper_title" data-options="readonly:true" style="width:300px;border:0">&nbsp;&nbsp;
                    <small>已选择</small>&nbsp;&nbsp;<input class="easyui-textbox" id="select_ids" data-options="readonly:true" style="width:150px;border:0">&nbsp;&nbsp;
                    <a id="btn_select_paper" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-add',plain:true" onclick="showPaperList()"><b>选择试卷</b></a>
                </td>              
            </tr>           
        </table>        
    </div>
    <table id="tt" class="easyui-datagrid" style="width: auto;" data-options="">
        <thead>
            <tr>
                <th data-options="field:'ck',checkbox:true" ></th>
                <th data-options="field:'v_id'">编号</th>
                <th data-options="field:'v_level',width:80" >难度等级</th>
                <th data-options="field:'v_title',width:350">题目</th>
                <th data-options="field:'v_answer_arry', align:'center'">答案</th>
                <th data-options="field:'v_create_name', align:'center',width:80">创建人</th>
                <th data-options="field:'v_create_time', align:'center',width:120">创建时间</th>
                <th data-options="field:'v_oporate',formatter:formatOper, align:'center'">操作</th>
            </tr>
        </thead>
    </table>
    <div id="win" class="easyui-window" style="width: 600px; height: 500px" data-options="modal:true">
        <form id="ff" method="post">
            <div style="text-align: center; padding: 10px; margin-top: 10px; vertical-align: middle;">
                <input id="f_id" name="f_id" type="hidden" value="" />
                <label for="f_title">题 目:</label>
                <input id="f_title" name="f_title" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 50px;"/>
            </div>
            <div style="text-align: center; vertical-align:central; padding: 10px">
                <label for="f_level">难度等级</label>             
                <select id="f_level" name="f_level" class="easyui-combobox" data-options="select:0"  style="width:120px" >
                    <option value="0" >请选择难度等级</option>
                    <option value="1">初级</option>
                    <option value="2">中级</option>
                    <option value="3">高级</option>
                </select>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_answerA">空白1:</label>
                <input id="f_answerA" name="f_answerA" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_answerB">空白2:</label>
                <input id="f_answerB" name="f_answerB" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_answerC">空白3:</label>
                <input id="f_answerC" name="f_answerC" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>
            <div style="text-align: center; padding: 10px">
                <label for="f_answerD">空白4:</label>
                <input id="f_answerD" name="f_answerD" class="easyui-textbox" data-options="multiline:true" style="width: 371px; height: 45px"/>
            </div>            
            <div style="text-align: center; padding: 10px">
                <input id="btnType" type="hidden" value="" />
                <a id="btnClear" href="#" class="easyui-linkbutton" onclick="ClearForm()" data-options="iconCls:'icon-cancel'">重置</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                <a id="btnConfirm" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" onclick="Confirm()">确定</a>&nbsp;&nbsp;&nbsp;&nbsp; 
                <a id="btnCancel" href="#" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" onclick="hideAddPannel()">取消</a>&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </form>
    </div>
    <div id="win_paper_list" class="easyui-window" style="width: 600px; height: 500px" data-options="modal:true">
        <table id="dg_paper_list" class="easyui-datagrid" style="width: auto;" data-options="">
        <thead>
            <tr>
                <th data-options="field:'ck',checkbox:true" ></th>
                <th data-options="field:'v_id'">编号</th>
                <th data-options="field:'v_title',width:320">题目</th>
                <th data-options="field:'v_filling_id_arry'">已选列表</th>
            </tr>
        </thead>
    </table>
    </div>
</body>
</html>
