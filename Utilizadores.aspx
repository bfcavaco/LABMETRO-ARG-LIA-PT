<%@ Page language="c#" Codebehind="Utilizadores.aspx.cs" AutoEventWireup="True" Inherits="LabMetro.Utilizadores" %>
<%@Register TagPrefix=header TagName=inc_header src="INCLUDES/_Header.ascx"%>
<%@Register TagPrefix=menu TagName=inc_menu src="INCLUDES/_Menu.ascx"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>:: LabMetro - Utilizadores ::</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <LINK href="Styles.css" type="text/css" rel="stylesheet">
  </head>
  <body MS_POSITIONING="GridLayout">
	
    <form id="Form1" method="post" runat="server">

            <table width="800" id="tblMain">
                <tr>
                    <td rowspan="2" align="left" valign="top">
                        <menu:inc_menu id="inc_menu" runat="server"></menu:inc_menu></td>
                    <td valign="top" height="60">
                        <header:inc_header id="inc_header" runat="server"></header:inc_header></td>
                </tr>
                <tr>
                    <td valign="top">
                        <!-- body da pagina fica num td, o primeiro é rowspan 2 e contem o menu-->
    <!-- FIM body -->
                    </td>
                </tr>
            </table>
            <!--FIM tabela principal-->
        </form>
    </body>
</HTML>