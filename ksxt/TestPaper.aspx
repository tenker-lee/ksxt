<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPaper.aspx.cs" Inherits="ksxt.TestPaper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="Scripts/jquery-2.1.0.js"></script>
    <script src="Scripts/jquery.easyui-1.4.5.js"></script>
    <script src="Scripts/locale/easyui-lang-zh_CN.js"></script>
    <link href="Content/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="Content/themes/icon.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .table-c table {
            border-right: 1px solid #95B8E7;
            border-bottom: 1px solid #95B8E7
        }
        .table-c table td {
            border-left: 1px solid #95B8E7;
            border-top: 1px solid #95B8E7
        }
        .auto-style9 {
            width: 34px;
        }
    </style>
    <script type="text/javascript">
        function closeWin() {
            window.opener = null;
            window.close();
        }
        $(document).ready(function () {
            //选择框
            $("input[type=radio]").click(function () {
                //选项题
                //alert($(this).attr('id'));
                //$(this).attr('checked', false);
                //updateAnswerList($(this).attr('id'), $(this).val());
            });
            //填空 评分
            $(".easyui-textbox").textbox({
                onChange: function () {
                    //alert($(this).attr('id'));
                    //updateAnswerList($(this).attr('id'),$(this).val());
                }
            });
            //选择框
           $('textarea').bind('input propertychange', function(){  
               var length = $(this).val();
               //alert(length);
               //updateAnswerList($(this).attr('id'),length);
            });  
            //简答框
            //$('textarea').onblur(function(){  
            //    var length = $(this).val();
            //    //alert(length);
            //    //updateAnswerList($(this).attr('id'),length);
            //});  
            
        });              
        //???
        function onqaChange(obj) {
            var dom = $(obj)
            //updateAnswerList(dom.attr('id'),dom.val() );
            //alert(dom.attr('id') + dom.val());
        }       
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 800px; margin: 0 auto; padding: 2px" class="table-c">
            <table style="margin: 0 auto; background-color: whitesmoke; width: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td colspan="9" style="text-align: center">
                        <asp:Label ID="lab_title" runat="server" Text="标题"></asp:Label>
                    &nbsp;</td>
                </tr>                
            </table>
            <div class="easyui-accordion" data-options="animate:true" style="width: 100%; height: auto;">
                <div title="单选" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <asp:Repeater ID="repChoice" runat="server">
                        <HeaderTemplate>
                            <table style="width: 97%; margin: 0 auto" border="0" cellspacing="0" cellpadding="0">
                                <thead>
                                    <td class="auto-style9" style="text-align: center">编号</td>
                                    <td style="width: 40%; text-align: center">题目</td>
                                    <td style="text-align: center">选项</td>
                                    <td style="text-align: center; width: 30px">得分</td>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td rowspan="2" style="text-align: center"><%# Eval("id") %></td>
                                <td><%# Eval("title") %></td>
                                <td>
                                    <input id="choice_<%# Eval("id") %>_answer_0"  name="choice_<%# Eval("id") %>_answer" type="radio" value="0"  /><label for="choice_<%# Eval("id") %>_answer_0">A:<%# ReadArryString(Eval("select_arry").ToString(),0) %></label><br /><input id="choice_<%# Eval("id") %>_answer_1" name="choice_<%# Eval("id") %>_answer" type="radio" value="1"   /><label for="choice_<%# Eval("id") %>_answer_1">B:<%# ReadArryString(Eval("select_arry").ToString(),1) %></label><br /><input id="choice_<%# Eval("id") %>_answer_2"   name="choice_<%# Eval("id") %>_answer" type="radio" value="2"   /><label for="choice_<%# Eval("id") %>_answer_2">C:<%# ReadArryString(Eval("select_arry").ToString(),2) %></label><br /><input id="choice_<%# Eval("id") %>_answer_3"  name="choice_<%# Eval("id") %>_answer" type="radio" value="3"  /><label for="choice_<%# Eval("id") %>_answer_3">D:<%# ReadArryString(Eval("select_arry").ToString(),3) %></label></td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="choice_<%#Eval("id") %>_score"   />

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:&nbsp;<a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-tip'" onclick="$('#span_choice_<%# Eval("id").ToString() %>').show();" >查看</a>&nbsp;&nbsp;&nbsp;<span id="span_choice_<%# Eval("id").ToString() %>" style="display:none"><%#  choiceAnswerTochar(Eval("answer_arry").ToString()) %></span></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div title="判断" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <asp:Repeater ID="repJudge" runat="server">
                        <HeaderTemplate>
                            <table style="width: 97%; margin: 0 auto" border="0" cellspacing="0" cellpadding="0">
                                <thead>
                                    <td class="auto-style9" style="text-align: center">编号</td>
                                    <td style="width: 40%; text-align: center">题目</td>
                                    <td style="text-align: center">选项</td>
                                    <td style="text-align: center; width: 30px">得分</td>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td rowspan="2" style="text-align: center"><%# Eval("id") %></td>
                                <td><%# Eval("title") %></td>
                                <td>
                                    <input id="judge_<%# Eval("id") %>_answer_0"   name="judge_<%# Eval("id") %>_answer" type="radio" value="0"  /><label for="judge_<%# Eval("id") %>_answer_0">错误</label><br />
                                    <input id="judge_<%# Eval("id") %>_answer_1"   name="judge_<%# Eval("id") %>_answer" type="radio" value="1" /><label for="judge_<%# Eval("id") %>_answer_1">正确</label><br />
                                </td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="judge_<%# Eval("id") %>_score"  />

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:&nbsp;<a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-tip'" onclick="$('#span_judge_<%# Eval("id").ToString() %>').show();" >查看</a>&nbsp;&nbsp;&nbsp;<span id="span_judge_<%# Eval("id").ToString() %>" style="display:none"> <%# Eval("answer_arry").ToString() == "1" ? "正确" : "错误"  %> </span></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div title="填空" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <asp:Repeater ID="repFilling" runat="server">
                        <HeaderTemplate>
                            <table style="width: 99%; margin: 0 auto" border="0" cellspacing="0" cellpadding="0">
                                <thead>
                                    <td class="auto-style9" style="text-align: center">编号</td>
                                    <td style="width: 40%; text-align: center">题目</td>
                                    <td style="text-align: center">选项</td>
                                    <td style="text-align: center; width: 30px">得分</td>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td rowspan="2" style="text-align: center"><%# Eval("id") %></td>
                                <td><%# Eval("title") %></td>
                                <td>&nbsp<%#fillingHtml(int.Parse(Eval("id").ToString()),Eval("answer_arry").ToString(),"")%></td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="filling_<%# Eval("id") %>_score"   />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:&nbsp;<a href="#" class="easyui-linkbutton" data-options="iconCls:'icon-tip'" onclick="$('#span_filling_<%# Eval("id").ToString() %>').show();" >查看</a>&nbsp;&nbsp;&nbsp;<span id="span_filling_<%# Eval("id").ToString() %>" style="display:none"><%#  Eval("answer_arry")  %></span></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
                <div title="简答" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <asp:Repeater ID="repQa" runat="server">
                        <HeaderTemplate>
                            <table style="width: 97%; margin: 0 auto" border="0" cellspacing="0" cellpadding="0">
                                <thead>
                                    <td class="auto-style9" style="text-align: center">编号</td>
                                    <td style="width: 50%; text-align: center">题目</td>
                                    <td style="text-align: center">选项</td>
                                    <td style="text-align: center; width: 30px">得分</td>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td rowspan="2" style="text-align: center"><%# Eval("id") %></td>
                                <td><%# Eval("title") %></td>
                                <td>
                                    <textarea  rows="10" style="width: 98%; height: 100%" id="qa_<%# Eval("id") %>_answer_0"></textarea>
                                </td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="qa_<%# Eval("id") %>_score"   />                                   

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:&nbsp;<a href="#" class="easyui-linkbutton"  data-options="iconCls:'icon-tip'" onclick="$('#span_qa_<%# Eval("id").ToString() %>').show();" >查看</a>&nbsp;&nbsp;&nbsp;<span id="span_qa_<%# Eval("id").ToString() %>" style="display:none"><%#  Eval("answer") %></span></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>                             
            <div style="margin: 0 auto; width: 100%; text-align: center; padding: 5px; margin-top: 5px">
                <a id="btnConfirm" class="easyui-linkbutton" data-options="iconCls:'icon-ok',disabled:true" href="#" >提交</a>&nbsp;&nbsp;&nbsp;&nbsp;
                <a id="btnClose" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="closeWin()">关闭</a>
            </div>
        </div>
    </form>
</body>
</html>
