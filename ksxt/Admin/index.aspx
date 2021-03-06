﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ksxt.Admin.index" %>

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
    <link href="main.css" rel="stylesheet" type="text/css" />
    <script  type="text/javascript">
        $(document).ready(function () {            
            $("a[name='addPanel']").click(function () {
                addPanel(this.text, this.hreflang);
            });
            //$("#changePass").attr("target", "_blank"); 
        });
        function addPanel(title, url) {
            $('title').text('' + title)
            if ($("#panel").tabs("exists", title)) {                
		        $("#panel").tabs("select", title);
		        var tab = $("#panel").tabs("getSelected");
		        $("#panel").tabs("update",
						        {
							        tab : tab,
							        options : {
								        title : title,
								        content : '<div style="width:100%;height:100%"><iframe height="100%" width="100%" border="0" frameborder="0" src='
										        + url + '></iframe></div>',
								        closable : true,
								        fit : true,
								        selected : true
							        }
						        });
	        } else {
		        $("#panel").tabs("add",
						        {
							        title : title,
							        content : '<div style="width:100%;height:100%"><iframe height="100%" width="100%" border="0" frameborder="0" src='
									        + url + '></iframe></div>',
							        closable : true,
							        fit : true,
							        selected : true
						        });
	        }
        }
    </script>
</head>
<body class="easyui-layout">
        <div id="head" data-options="region:'north'" style="padding: 5px; width: auto; height: 50px;background-image:url('../Image/head.png');background-repeat:no-repeat">
            <table style="width: 100%;">
                <tr>
                    <td style="width:50%">&nbsp;</td>
                    <td  style="text-align:right;padding-right:40px;vertical-align:central;font:300" >&nbsp;
                        <b>帐号:&nbsp;&nbsp;&nbsp;&nbsp;</b><asp:Label ID="labName" runat="server" Text=""></asp:Label> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a  id="changePass" target="_blank" href="../changePassword.aspx">修改密码</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a  href="../logout.aspx" >退出</a>
                    </td>                   
                </tr>                
            </table>
        </div>
        <div id="left" data-options="region:'west',split:true" style="width: 130px;">
            <div class="easyui-accordion" data-options="animate:true" style="width: auto; height: auto;">
                <% if(logonUserType == "1") { %>
                <div title="帐户管理" data-options="collapsible:true,iconcls:'icon-reload'" style="overflow: auto; padding: 10px;">
                    <ul>
                        <li style="height: 30px;font:200"><a href="javascript:void(0);" name="addPanel"
														hreflang="userlist.aspx">帐户管理</a></li>
                    </ul>
                </div>
                <div title="题库管理" data-options="iconcls:'icon-reload'" style="padding: 5px; overflow: auto;">
                    <ul>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="singlist.aspx">选择题</a></li>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="judgelist.aspx">判断题</a></li>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="fillinglist.aspx">填空题</a></li>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="qalist.aspx">问答题</a></li>
                    </ul>
                </div>
                <div title="试卷管理" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <ul>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="paperlist.aspx">试卷列表</a></li>
                    </ul>
                </div>
                <% } %>
                <div title="成绩管理" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <ul>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="checkpaper.aspx">成绩管理</a></li>
                    </ul>
                </div>          
                <div title="待考试卷" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <ul>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="mypaper.aspx">待考试卷</a></li>
                    </ul>
                </div>       
                <div title="试卷练习" data-options="collapsible:true,iconcls:'icon-reload'" style="padding: 10px;">
                    <ul>
                        <li style="height: 30px"><a href="javascript:void(0);" name="addPanel"
														hreflang="../TestPaper.aspx">试卷练习</a></li>
                    </ul>
                </div>        
            </div>
        </div>
        <div id="center" data-options="region:'center'">
            <div class="easyui-tabs"  id="panel" style="width:100%;height:100%" >
            </div>
        </div>    
</body>
</html>
