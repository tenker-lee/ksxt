<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qalist.aspx.cs" Inherits="ksxt.Admin.qalist" %>

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
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
