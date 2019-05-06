<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GradePaper.aspx.cs" Inherits="ksxt.GradePaper" %>

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
        .auto-style5 {
            width: 87px;
        }
        .auto-style7 {
            height: 20px;
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
                updateAnswerList($(this).attr('id'), $(this).val());
            });
            //填空 评分
            $(".easyui-textbox").textbox({
                onChange: function () {
                    //alert($(this).attr('id'));
                    updateAnswerList($(this).attr('id'),$(this).val());
                }
            });
            //选择框
           $('textarea').bind('input propertychange', function(){  
               var length = $(this).val();
               //alert(length);
               //updateAnswerList($(this).attr('id'),length);
            });  
            //简答框
            $('textarea').onblur(function(){  
                var length = $(this).val();
                //alert(length);
                updateAnswerList($(this).attr('id'),length);
            });  
            
        });       
        //上传结果
        function updateAnswerList(answerStr,value) {
             $.ajax({
                 url: 'HandlerPublicFun.ashx?opt=UpdateAnswerList',
                 type: "POST",
                 data: { "answerStr": answerStr, "value": value },
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
                            else {
                                $.messager.alert('警告', result.msg);
                            }
                            
                        }
            });
            
        }
        //???
        function onqaChange(obj) {
            var dom = $(obj)
            updateAnswerList(dom.attr('id'),dom.val() );
            //alert(dom.attr('id') + dom.val());
        }
        function saveCheckPaper(paper_id,user_id) {
            $.ajax({
                 url: 'HandlerPublicFun.ashx?opt=SaveCheckPaper',
                 type: "POST",
                 data: { "paperId": paper_id, "userId": user_id },
                        success: function (data) {
                            var result = JSON.parse(data);
                            if (result.stateCode == 0) {
                                //$.messager.show({
                                //    title: '提示',
                                //    msg: result.msg,
                                //    timeout: 5000,
                                //    showType: 'slide'
                                //});
                                $.messager.alert('警告', result.msg);
                            }
                            else {
                                $.messager.alert('警告', result.msg);
                            }
                            
                        }
            });
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
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" class="auto-style7">&nbsp;</td>
                    <td style="text-align: center" class="auto-style7">分值</td>
                    <td style="text-align: center" class="auto-style7">数量</td>
                    <td style="text-align: center" class="auto-style7">总分</td>
                    <td style="text-align: center" rowspan="5">&nbsp;</td>
                    <td style="text-align: center" colspan="3">考试时间</td>
                    <td style="text-align: center">提交时间</td>
                </tr>
                <tr>
                    <td class="auto-style5" style="text-align: right">单项选择</td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_choice_score" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_choice_count" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_choice_total" runat="server" Text="无"></asp:Label>
                    </td>
                    <td style="text-align: center" colspan="3">
                        <asp:Label ID="labTime" runat="server" Text="时间"></asp:Label>
                    </td>
                    <td style="text-align: center">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style5" style="text-align: right">判断</td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_judge_score" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_judge_count" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_judge_total" runat="server" Text="无"></asp:Label>
                    </td>
                    <td style="text-align: center" colspan="2">考生帐号</td>
                    <td style="text-align: center" colspan="2">
                        <asp:Label ID="lab_user" runat="server" Text="无"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5" style="text-align: right">填空</td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_filling_score" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_filling_count" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_filling_total" runat="server" Text="无"></asp:Label>
                    </td>
                    <td style="text-align: center">试卷总分</td>
                    <td style="text-align: center" colspan="3">
                        <asp:Label ID="lab_total_score_paper" runat="server" Text="无"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5" style="text-align: right">简答</td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_qa_score" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_qa_count" runat="server" Text="无"></asp:Label>
                    </td>
                    <td class="auto-style5" style="text-align: center">
                        <asp:Label ID="lab_qa_total" runat="server" Text="无"></asp:Label>
                    </td>
                    <td style="text-align: center" colspan="3">得分</td>
                    <td style="text-align: center">
                        <asp:Label ID="lab_total_score" runat="server" Text="无"></asp:Label>
                    </td>
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
                                    <input id="choice_<%# Eval("tid") %>_<%=user_id %>_answer_0" <% =enableEdit?"": "disabled=\"disabled\""%>  name="choice_<%# Eval("tid") %>_<%=user_id %>_answer" type="radio" value="0" <%# Eval("value").ToString()=="0"?"checked=\"checked\"":"" %>  /><label for="choice_<%# Eval("tid") %>_<%=user_id %>_answer_0">A:<%# ReadArryString(Eval("select_arry").ToString(),0) %></label>
                                    <br />
                                    <input id="choice_<%# Eval("tid") %>_<%=user_id %>_answer_1" <% =enableEdit?"": "disabled=\"disabled\""%>  name="choice_<%# Eval("tid") %>_<%=user_id %>_answer" type="radio" value="1" <%# Eval("value").ToString()=="1"?"checked=\"checked\"":"" %>  /><label for="choice_<%# Eval("tid") %>_<%=user_id %>_answer_1">B:<%# ReadArryString(Eval("select_arry").ToString(),1) %></label>
                                    <br />
                                    <input id="choice_<%# Eval("tid") %>_<%=user_id %>_answer_2" <% =enableEdit?"": "disabled=\"disabled\""%>  name="choice_<%# Eval("tid") %>_<%=user_id %>_answer" type="radio" value="2" <%# Eval("value").ToString()=="2"?"checked=\"checked\"":"" %>  /><label for="choice_<%# Eval("tid") %>_<%=user_id %>_answer_2">C:<%# ReadArryString(Eval("select_arry").ToString(),2) %></label>
                                    <br />
                                    <input id="choice_<%# Eval("tid") %>_<%=user_id %>_answer_3" <% =enableEdit?"": "disabled=\"disabled\""%>  name="choice_<%# Eval("tid") %>_<%=user_id %>_answer" type="radio" value="3" <%# Eval("value").ToString()=="3"?"checked=\"checked\"":"" %>  /><label for="choice_<%# Eval("tid") %>_<%=user_id %>_answer_3">D:<%# ReadArryString(Eval("select_arry").ToString(),3) %></label>
                                </td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="choice_<%#Eval("tid") %>_<%=user_id %>_score"  <%=grade?"type=\"text\"":"type=\"hidden\"" %>   value="<%# Eval("score") %>"/>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:<%# showAnswer ? choiceAnswerTochar(Eval("answer_arry").ToString()):"" %></td>
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
                                    <input id="judge_<%# Eval("tid") %>_<%=user_id %>_answer_0" <% =enableEdit?"": "disabled=\"disabled\""%>  name="judge_<%# Eval("tid") %>_<%=user_id %>_answer" type="radio" value="0" <%# Eval("value").ToString()=="0"?"checked=\"checked\"":"" %> /><label for="judge_<%# Eval("tid") %>_<%=user_id %>_answer_0">错误</label><br />
                                    <input id="judge_<%# Eval("tid") %>_<%=user_id %>_answer_1" <% =enableEdit?"": "disabled=\"disabled\""%> name="judge_<%# Eval("tid") %>_<%=user_id %>_answer" type="radio" value="1" <%# Eval("value").ToString()=="1"?"checked=\"checked\"":"" %> /><label for="judge_<%# Eval("tid") %>_<%=user_id %>_answer_1">正确</label><br />
                                </td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="judge_<%# Eval("tid") %>_<%=user_id %>_score" <%=grade?"type=\"text\"":"type=\"hidden\"" %> value="<%# Eval("score") %>"/>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:<%# showAnswer? Eval("answer_arry").ToString() == "1" ? "正确" : "错误" :"" %></td>
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
                                <td>&nbsp<%#fillingHtml(int.Parse(Eval("tid").ToString()),int.Parse(user_id.ToString()),Eval("answer_arry").ToString(),Eval("value").ToString())%> 
                                </td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="filling_<%# Eval("tid") %>_<%=user_id %>_score"  <%=grade?"type=\"text\"":"type=\"hidden\"" %> value="<%# Eval("score") %>" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:<%# showAnswer? Eval("answer_arry") :"" %></td>
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
                                    <textarea  rows="10" style="width: 98%; height: 100%" id="qa_<%# Eval("tid") %>_<%=user_id %>_answer_0" <% =enableEdit?"": "disabled=\"disabled\""%> onblur="onqaChange(this)"><%# Eval("value") %></textarea>
                                </td>
                                <td>
                                    <input class="easyui-textbox" style="width: 30px; margin: 2px 2px 2px 2px;" id="qa_<%# Eval("tid") %>_<%=user_id %>_score"    <%=grade?"type=\"text\"":"type=\"hidden\"" %> value="<%# Eval("score") %>" />

                                    

                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">参考答案:<%# showAnswer? Eval("answer") :""%></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div style="margin: 0 auto; width: 100%; text-align: center; padding: 5px; margin-top: 5px">
                <a id="btnConfirm" class="easyui-linkbutton" data-options="iconCls:'icon-ok'" href="#" onclick="saveCheckPaper(<%=paper_id %>,<%=user_id %>)">提交</a>&nbsp;&nbsp;&nbsp;&nbsp;
                <a id="btnClose" class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="closeWin()">关闭</a>
            </div>
        </div>
    </form>
</body>
</html>
