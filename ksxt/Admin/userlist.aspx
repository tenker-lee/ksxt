<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userlist.aspx.cs" Inherits="ksxt.Admin.userlist" %>

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
                url: "HandlerUser.ashx?opt=query",
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
                        url: 'HandlerUser.ashx?opt=SearchById',
                        type: "POST",
                        data: { "s_id": selrow.v_id },
                        success: function (data) {
                            var v = JSON.parse(data);
                            $('#f_type').combobox("setValue", v.type);
                            $('#f_name').textbox("setValue", v.name);
                            $('#f_department').textbox("setValue", v.department);
                            $('#f_job').textbox("setValue", v.job);
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
                url: "HandlerUser.ashx?opt=add",
                onSubmit: function () {
                    return CheckForm();
                },
                success: function (data) {
                    var result = JSON.parse(data);
                    $.messager.alert('警告', result.msg);
                    if (result.stateCode == 0) {
                       // ClearForm();
                    }
                }
            });
        }
        function ModifyData() {
            $('#ff').form('submit', {
                type: 'post',
                url: "HandlerUser.ashx?opt=edit",
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
                        url: 'HandlerUser.ashx?opt=del',
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
            return "<a href=\"#\" class=\"easyui-linkbutton\" data-options=\"iconCls:'icon-add',plain:false\" onclick=\"\">添加</a>";
        }
        function AddToPaper(id) {
            alert("add to paper");
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 273px;
            text-align:right;
            padding:10px;
        }
        .auto-style2 {
            width: 657px;
            height: 350px;
        }
    </style>
</head>
<body>
    <div id="" style="text-align: center; padding: 5px">
    </div>
    <div id="searchbar" style="border: thin solid #C0C0C0; text-align: right; padding: 20px">
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
                <th data-options="field:'v_type',width:80">类型</th>
                <th data-options="field:'v_name',width:80">帐号</th>
                <th data-options="field:'v_department',align:'left'">部门</th>
                <th data-options="field:'v_job', align:'center'">职位</th>
                <th data-options="field:'v_create_name', align:'center',width:80">创建人</th>
                <th data-options="field:'v_create_time', align:'center',width:120">创建时间</th>
            </tr>
        </thead>
    </table>
    <div id="win" class="auto-style2" data-options="modal:true">
        <form id="ff" method="post">
             <div style="text-align: left; padding: 10px; padding-left: 100px;">                
            </div>
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1" >
                        <label for="f_type">难度等级:</label>
                    </td>
                    <td>
                        <select id="f_type" name="f_type" class="easyui-combobox" data-options="select:0" style="width: 80px">
                            <option value="0">普通</option>
                            <option value="1">管理员</option>
                        </select></td>
                </tr>
                <tr>
                    <td class="auto-style1" >
                        <label for="f_name">帐号:</label>
                    </td>
                    <td>
                        <input id="f_name" name="f_name" class="easyui-textbox" data-options="multiline:false" style="width: 100px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1" >
                        <label for="f_password" >密码:</label></td>
                    <td>
                        <input id="f_password" name="f_password" class="easyui-textbox" data-options="multiline:false,type:'password'" style="width: 100px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1" >
                        <label for="f_password_confirm" style="text-align: right">确认密码:</label></td>
                    <td>
                        <input id="f_password_confirm" name="f_password_confirm" class="easyui-textbox" data-options="multiline:false,type:'password'" style="width: 100px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1" >
                        <label for="f_department">部门:</label></td>
                    <td>
                        <input id="f_department" name="f_department" class="easyui-textbox" data-options="multiline:false" style="width: 250px;" /></td>
                </tr>
                <tr>
                    <td class="auto-style1" >
                        <label for="f_job">职位:</label></td>
                    <td>
                        <input id="f_job" name="f_job" class="easyui-textbox" data-options="multiline:false" style="width: 100px;" /></td>
                </tr>
            </table>
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
