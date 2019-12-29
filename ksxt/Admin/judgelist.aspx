<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="judgelist.aspx.cs" Inherits="ksxt.Admin.judgelist" %>

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
    <link href="main.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function () {
            $('#tt').datagrid({
                url: "HandlerJudge.ashx?opt=query",
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
                                        $(".add").linkbutton({plain:true, iconCls:'icon-add' });
                                                            $(".del").linkbutton({plain:true, iconCls:'icon-remove' });

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
                    $('#select_ids').textbox('setValue', rowData["v_judge_id_arry"]);
                },
                onLoadSuccess: function (data) {
                    //$.parser.parse();
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
                        url: 'HandlerJudge.ashx?opt=SearchById',
                        type: "POST",
                        data: { "s_id": selrow.v_id },
                        success: function (data) {
                            var v = JSON.parse(data);
                            $('#f_level').combobox("setValue", v.level);
                            $('#f_title').textbox("setValue", v.title);
                            $("input:radio[name='f_answer'][value='" + v.answer + "']").prop("checked", "checked");
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
                url: "HandlerJudge.ashx?opt=add",
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
                url: "HandlerJudge.ashx?opt=edit",
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
                url: 'HandlerJudge.ashx?opt=del',
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
             var str = "<a href=\"#\" class=\"add\" onclick=\"AddToPaper('add'," + row.v_id + ")\"><i>添加</i></a>";
            //str = str + "&nbsp&nbsp&nbsp";
            str = str + "<a href=\"#\" class=\"del\" onclick=\"AddToPaper('del'," + row.v_id + ")\"><i>移除</i></a>";
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
                url: 'HandlerPaper.ashx?opt=AddJudgeToPaper',
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
                    <small>编号</small>&nbsp;&nbsp;<input class="easyui-textbox" id="select_paper_id" data-options="readonly:true" style="width:50px"/>&nbsp;
                    <small>标题</small>&nbsp;&nbsp;<input class="easyui-textbox" id="select_paper_title" data-options="readonly:true" style="width:300px;border:0"/>&nbsp;&nbsp;
                    <small>已选择</small>&nbsp;&nbsp;<input class="easyui-textbox" id="select_ids" data-options="readonly:true" style="width:150px;border:0"/>&nbsp;&nbsp;
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
                <th data-options="field:'v_title',width:300">题目</th>
                <th data-options="field:'v_answer', align:'center',width:50">答案</th>
                <th data-options="field:'v_create_name', align:'center',width:80">创建人</th>
                <th data-options="field:'v_create_time', align:'center',width:120">创建时间</th>
                <th data-options="field:'v_oporate',formatter:formatOper, align:'center',width:120">操作 </th>
              
            </tr>
        </thead>
    </table>
    <div id="win" class="easyui-window" style="width: 600px; height: 400px" data-options="modal:true">
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
                <label for="f_answer">答案:</label>
                <input  type="radio"  name="f_answer" value="0"/>错误
                <input  type="radio"  name="f_answer" value="1"/>正确
            </div>
            <div style="text-align: center; padding: 10px">
                <div><input id="btnType" type="hidden" value="" /></div>
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
                <th data-options="field:'v_judge_id_arry'">已选列表</th>
            </tr>
        </thead>
    </table>
    </div>
</body>
</html>
