<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ksxt.Admin.index" %>

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
</head>
<body>
    <div class="easyui-layout" data-options="fit:true"  style="width: auto; height: 600px; padding: 0px">
        <div id="head" data-options="region:'north'"  style="padding: 5px; width: auto; height: 60px"></div>
        <div id="left" data-options="region:'west',split:true" style="width: 250px;">
            <div class="easyui-accordion" data-options="animate:false"  style="width: auto; height: auto;">
                <div title="帐户管理" data-options="collapsible:false,iconcls:'icon-ok'"  style="overflow: auto; padding: 10px;">
                    <h3 style="color: #0099FF;">Accordion for jQuery</h3>
                    <p>Accordion is a part of easyui framework for jQuery. It lets you define your accordion component on web page more easily.</p>
                </div>
                <div title="题库管理" data-options="closed:false,iconcls:'icon-reload'" style="padding: 5px; overflow: auto;">
                    <ul>
                        <li style="height: 20px"><a href="#">选择题</a></li>
                        <li style="height: 20px"><a href="#">判断题</a></li>
                        <li style="height: 20px"><a href="#">填空题</a></li>
                        <li style="height: 20px"><a href="#">问答题</a></li>
                    </ul>
                </div>
                <div title="试卷管理" data-options="collapsible:false,iconcls:'icon-reload'"  style="padding: 10px;">
                    <ul>
                        <li style="height: 20px"><a href="#">试卷列表</a></li>
                    </ul>
                </div>
                <div title="成绩管理" data-options="collapsible:false,iconcls:'icon-reload'" style="padding: 10px;">
                    <ul>
                        <li style="height: 20px"><a href="#">试卷列表</a></li>
                    </ul>
                </div>
                <div title="帐户管理" data-options="collapsible:false,iconcls:'icon-reload'" style="padding: 10px;">
                    <ul>
                        <li style="height: 20px"><a href="#">帐户列表</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="center" data-options="region:'center',fit:true" style="padding: 0px;">
            <div class="easyui-tabs" data-options="fit:true"  style="width: auto; height: 100px;">
                <div title="First Tab" style="padding: 10px;">
                    First Tab
                </div>
                <div title="Second Tab" data-options="closable:true" style="padding: 10px;">
                    Second Tab
                </div>
                <div title="Third Tab" data-options="iconcls:'icon-reload',closable:true" style="padding: 10px;">
                    Third Tab
                </div>
            </div>
        </div>
    </div>
</body>
</html>
