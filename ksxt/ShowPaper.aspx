<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowPaper.aspx.cs" Inherits="ksxt.ShowPaper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考试</title>
    <script src="Scripts/jquery-2.1.0.js"></script>
    <script src="Scripts/jquery.easyui-1.4.5.js"></script>
    <script src="Scripts/locale/easyui-lang-zh_CN.js"></script>
    <link href="Content/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="Content/themes/icon.css" rel="stylesheet" type="text/css"/>
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
    </style>
    <script type="text/javascript">
        function closeWin() {
            window.opener = null;
            window.close();
        }
        $(document).ready(function () {
            
        });       
         
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 700px; margin: 0 auto; padding: 2px" class="table-c">
            <table style="margin: 0 auto; background-color: whitesmoke; width: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td colspan="9" style="text-align: center">
                        <asp:Label ID="Label1" runat="server" Text="标题"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center" class="auto-style7">&nbsp;</td>
                    <td style="text-align: center" class="auto-style7">分值</td>
                    <td style="text-align: center" class="auto-style7">数量</td>
                    <td style="text-align: center" class="auto-style7">总分</td>
                    <td style="text-align: center" rowspan="5">&nbsp;</td>
                    <td style="text-align: center" colspan="3">考试时间</td>
                    <td style="text-align: center" rowspan="2">
                        &nbsp;</td>
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
                    
                </div>
                <div title="判断" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                   
                </div>
                <div title="填空" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    
                </div>
                <div title="简答" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    
                </div>
            </div>
            <div style="margin: 0 auto; width: 100%; text-align: center;padding:5px;margin-top:5px">
                <a id="btnConfirm"  class="easyui-linkbutton" data-options="iconCls:'icon-ok'" href="#" onclick="">提交</a>&nbsp;&nbsp;&nbsp;&nbsp;
                                <a id="btnClose"  class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="#" onclick="closeWin()">关闭</a>

            </div>
        </div>
    </form>
</body>
</html>
